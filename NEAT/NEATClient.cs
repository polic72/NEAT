using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NEAT.Genetic.Tracker;
using NEAT.Speciation;
using NEAT.Genetic;
using NEAT.Neural_Network;

namespace NEAT
{
    /// <summary>
    /// The client of the NEAT algorithm. Will handle all organism/species organization and reproduction.
    /// </summary>
    public class NEATClient
    {
        #region Properties

        /// <summary>
        /// The pedigree used to track all genes in this NEAT client.
        /// </summary>
        public Pedigree Pedigree { get; }

        /// <summary>
        /// The total number of organisms this NEAT client will train.
        /// </summary>
        public int NumOrganisms { get; }


        /// <summary>
        /// All of the organisms tracked in this NEAT client.
        /// </summary>
        public List<Organism> Organisms { get; }

        /// <summary>
        /// All of the species tracked in this NEAT client.
        /// </summary>
        public HashSet<Species> Species { get; }


        /// <summary>
        /// The max distance one organism can have to another before being in a different species.
        /// </summary>
        public double CompatibilityDistance { get; internal set; }


        /// <summary>
        /// The percentage of organisms that will survive on <see cref="NEAT.NEATClient.Kill"/>.
        /// </summary>
        public double SurvivingPercentage { get; }

        #endregion Properties


        #region CompatibilityDistanceFunction

        /// <summary>
        /// The delegate of how the <see cref="NEAT.NEATClient.CompatibilityDistance"/> will be adjusted based on the number of species.
        /// </summary>
        /// <param name="numSpecies">The number of species at this instance of time.</param>
        /// <returns>The amount to adjust the compatibility distance by.</returns>
        public delegate double CompatibilityDistanceFunction(int numSpecies);


        /// <summary>
        /// The used CompatibilityDistanceFunction.
        /// </summary>
        protected CompatibilityDistanceFunction CD_function;


        /// <summary>
        /// The NERO standard CompatibilityDistanceFunction.
        /// </summary>
        /// <param name="numSpecies">The number of species at this instance of time.</param>
        /// <returns>The amount to adjust the compatibility distance by.</returns>
        /// <remarks>
        /// If the number of species is 4, stay constant. If greater than 4, increase compatibility distance by 0.3. If less than 4, decrease compatibility distance by 0.3.
        /// </remarks>
        public static double NERO_CD_Function(int numSpecies)
        {
            if (numSpecies == 4)
            {
                return 0;
            }
            else if (numSpecies > 4)
            {
                return 0.3;
            }
            else
            {
                return -0.3;
            }
        }


        /// <summary>
        /// The constant CompatibilityDistanceFunction. No matter the number of species, the compatibility distance will never be adjusted.
        /// </summary>
        /// <param name="numSpecies">The number of species at this instance of time. (Ignored)</param>
        /// <returns>The amount to adjust the compatibility distance by. (Always 0)</returns>
        public static double Constant_CD_Function(int numSpecies)
        {
            return 0;
        }

        #endregion CompatibilityDistanceFunction


        #region EvaluationFunction

        /// <summary>
        /// The delegate of how organisms' neural networks are evaluated and given a fitness.
        /// </summary>
        /// <param name="neuralNetwork">The neural network to evaluate.</param>
        /// <returns>The fitness score of the given neural network.</returns>
        public delegate double EvaluateFunction(NeuralNetwork neuralNetwork);


        /// <summary>
        /// The used EvaluateFunction.
        /// </summary>
        protected EvaluateFunction evaluateFunction;

        #endregion EvaluationFunction


        #region Constructors

        /// <summary>
        /// Constructs a NEAT client with the given pedigree, number of organisms, evaluation function, and every constant.
        /// </summary>
        /// <param name="pedigree">The pedigree used to track all genes in this NEAT client.</param>
        /// <param name="numOrganisms">The total number of organisms this NEAT client will train.</param>
        /// <param name="evaluateFunction">The function to evaluate organisms' neural networks and give them a fitness score.</param>
        /// <param name="compatibility_distance">The initial compatibility distance.</param>
        /// <param name="CD_function">The function to adjust the compatibility distance by. Good options are the <see cref="NEAT.NEATClient.NERO_CD_Function(int)"/> and the 
        /// <see cref="NEAT.NEATClient.Constant_CD_Function(int)"/></param>
        /// <param name="surviving_percentage">The percentage of organisms that will survive on <see cref="NEAT.NEATClient.Kill"/>.</param>
        public NEATClient(Pedigree pedigree, int numOrganisms, EvaluateFunction evaluateFunction, double compatibility_distance, CompatibilityDistanceFunction CD_function, 
            double surviving_percentage)
        {
            #region Internal Setters

            Pedigree = pedigree;

            NumOrganisms = numOrganisms;


            Organisms = new List<Organism>(numOrganisms);

            Species = new HashSet<Species>();


            this.evaluateFunction = evaluateFunction;


            CompatibilityDistance = compatibility_distance;

            this.CD_function = CD_function;


            SurvivingPercentage = surviving_percentage;

            #endregion Internal Setters


            #region Initial Conditions

            //Create organisms:
            for (int i = 0; i < NumOrganisms; ++i)
            {
                Organisms.Add(new Organism(Pedigree.CreateGenome()));
            }


            //Create Species:
            Species first_species = new Species();

            Species.Add(first_species);

            first_species.AddOrganism(Organisms[0]);    //Prepares first species to house every organism.


            Speciate();

            #endregion Initial Conditions
        }


        /// <summary>
        /// Constructs a NEAT client with the given pedigree, number of organisms, and evaluation function. Uses 4 for the initial compatibility distance, 
        /// <see cref="NEAT.NEATClient.NERO_CD_Function(int)"/> for the CompatibilityDistanceFunction, and 0.8 for the survival percentage.
        /// </summary>
        /// <param name="pedigree">The pedigree used to track all genes in this NEAT client.</param>
        /// <param name="numOrganisms">The total number of organisms this NEAT client will train.</param>
        /// <param name="evaluateFunction">The function to evaluate organisms' neural networks and give them a fitness score.</param>
        public NEATClient(Pedigree pedigree, int numOrganisms, EvaluateFunction evaluateFunction)
            : this(pedigree, numOrganisms, evaluateFunction, 4, NERO_CD_Function, 0.8)
        {

        }

        #endregion Constructors




        #region Evolution

        /// <summary>
        /// Evolves to the next generation.
        /// </summary>
        /// <remarks>
        /// This method is a helper method that will run <see cref="NEAT.NEATClient.Speciate"/>, <see cref="NEAT.NEATClient.EvaluateScores"/>, <see cref="NEAT.NEATClient.Kill"/>, 
        /// <see cref="NEAT.NEATClient.RemoveExtinctions"/>, and <see cref="NEAT.NEATClient.ReproduceAndReplace"/> in that order.
        /// <para/>
        /// Each of those methods is public and therefore could be run individually, but this ordering is important to the proper evolution to the next generation. In other words, 
        /// each of those methods are designed to be run in that exact order. Why the option was even given to allow users to run them individually is for stepping and education purposes. 
        /// In almost every case, only this method (<see cref="NEAT.NEATClient.Evolve"/>) should be run to move to the next generation.
        /// </remarks>
        public void Evolve()
        {
            Speciate();

            EvaluateScores();

            Kill();

            RemoveExtinctions();

            ReproduceAndReplace();
        }


        /// <summary>
        /// Separates the current organisms into the correct species. Makes new species and deletes extinct species as needed. See <see cref="NEAT.NEATClient.Evolve"/> before using!
        /// </summary>
        public void Speciate()
        {
            foreach (Organism organism in Organisms)
            {
                bool found_species = false;

                foreach (Species species in Species)
                {
                    Organism random_organism = species.GetRandomOrganism(Pedigree.Random, organism);


                    if (random_organism == null)    //The organism we're at is the only organism in the species we're at. See if we should leave it in this species.
                    {
                        bool kill_species = false;

                        IEnumerable<Species> all_other_species = Species.Where(x => x != species);

                        foreach (Species other_species in all_other_species)
                        {
                            Organism other_random_organism = other_species.GetRandomOrganism(Pedigree.Random);    //No need to exclude the organism because it can't be here.

                            if (organism.Genome.Distance(other_random_organism.Genome) < CompatibilityDistance) //The organism can fit in another species, put it there.
                            {
                                organism.Species.RemoveOrganism(organism);

                                other_species.AddOrganism(organism);


                                kill_species = true;
                                break;
                            }
                        }


                        if (kill_species)   //This species no longer has any organisms, it is now extinct.
                        {
                            Species.Remove(species);
                        }


                        found_species = true;
                        break;
                    }


                    if (organism.Genome.Distance(random_organism.Genome) < CompatibilityDistance)
                    {
                        if (organism.Species != random_organism.Species)    //If the species to set is actually different.
                        {
                            organism.Species?.RemoveOrganism(organism);

                            species.AddOrganism(organism);
                        }

                        found_species = true;
                        break;
                    }
                }

                if (!found_species)
                {
                    organism.Species?.RemoveOrganism(organism);


                    Species new_species = new Species();

                    Species.Add(new_species);

                    new_species.AddOrganism(organism);
                }
            }
        }


        /// <summary>
        /// Evaluates the scores of every organism and species. Applies the sharing function to the scores first. See <see cref="NEAT.NEATClient.Evolve"/> before using!
        /// </summary>
        /// <remarks>
        /// The new fitness (f'i) of organism i is as follows:
        /// <para/>
        /// f'i = fi / Σ( sh( dist(i, j) ) )| j=i to j=n
        /// <para/>
        /// Where:
        /// <list type="bullet">
        /// <item>i: The organism at this step.</item>
        /// <item>fi: The fitness score of organism i returned by the EvaluationFunction.</item>
        /// <item>sh(): A function that returns 1 when the distance is less than the compatibility distance. 0 otherwise.</item>
        /// <item>dist(): The genome distance function.</item>
        /// <item>j: Another organism (never equal to i).</item>
        /// <item>n: The total number of stored organisms.</item>
        /// </list>
        /// </remarks>
        public void EvaluateScores()
        {
            foreach (Organism organism in Organisms)
            {
                double initial_fitness = evaluateFunction(organism.NeuralNetwork);


                int sharing_sum = 0;

                foreach (Organism other_organism in Organisms.Where(x => x != organism))    //Loop through every other organism.
                {
                    if (organism.Genome.Distance(other_organism.Genome) < CompatibilityDistance)
                    {
                        ++sharing_sum;
                    }
                }


                if (sharing_sum == 0)   //No other organisms are similar enough to this organism.
                {
                    organism.FitnessScore = initial_fitness;
                }
                else
                {
                    organism.FitnessScore = initial_fitness / sharing_sum;
                }
            }


            foreach (Species species in Species)
            {
                species.CalculateFitnessScore();
            }
        }


        /// <summary>
        /// Kills all of the worst performing organisms. The percentage that will die is 1 - <see cref="NEAT.NEATClient.SurvivingPercentage"/>. 
        /// See <see cref="NEAT.NEATClient.Evolve"/> before using!
        /// </summary>
        public void Kill()
        {
            Organisms.Sort();   //Sorts the organisms by fitness score, greatest to lowest.


            double num_toKill = (1 - SurvivingPercentage) * Organisms.Count;

            for (int i = 0; i < num_toKill; ++i)
            {
                Organism organism = Organisms[Organisms.Count - 1];


                organism.Species.RemoveOrganism(organism);

                Organisms.RemoveAt(Organisms.Count - 1);
            }
        }


        /// <summary>
        /// Removes all stored extinct species (species with no organisms). See <see cref="NEAT.NEATClient.Evolve"/> before using!
        /// </summary>
        public void RemoveExtinctions()
        {
            Species.RemoveWhere(x => x.Size == 0);
        }


        /// <summary>
        /// Reproduces the surviving species based on their fitness scores. Chooses the organisms in the species to mate based on fitness scores as well. Replaces the previous generation 
        /// with the newly created organisms. See <see cref="NEAT.NEATClient.Evolve"/> before using!
        /// </summary>
        /// <remarks>
        /// Decides the amount of offspring (nk) each species (k) should be allotted via the following equation:
        /// <para/>
        /// nk = P * (Fk / Ft)
        /// <para/>
        /// Where:
        /// <list type="bullet">
        /// <item>k: The species.</item>
        /// <item>nk: The number of new organisms for species k in the next generation.</item>
        /// <item>P: The desired new population count.</item>
        /// <item>Fk: The average fitness score of the species.</item>
        /// <item>Ft: The sum of all fitness score averages of every species.</item>
        /// </list>
        /// </remarks>
        public void ReproduceAndReplace()
        {
            double total_speciesFitness = Species.Sum(x => x.AverageFitnessScore);


            Dictionary<Species, ReproduceAndReplace_Node> stored_distributionsSpecies = new Dictionary<Species, ReproduceAndReplace_Node>(Species.Count);


            List<Organism> next_generation = new List<Organism>(NumOrganisms);  //All of the new organisms.

            HashSet<Species> next_species = new HashSet<Species>(Species.Count);


            #region Reproduction

            foreach (Species species in Species)
            {
                //Calculate the number of organisms every species will have:
                int num_alloted_offspring = (int)(NumOrganisms * (species.AverageFitnessScore / total_speciesFitness));


                //Prepare new species for placement:
                Species creation_species = new Species();

                next_species.Add(creation_species);


                //Prepare ScoredDistribution for selecting organisms to mate:
                ScoredDistribution<Organism> internal_organisms = new ScoredDistribution<Organism>();

                foreach (Organism organism in species)
                {
                    internal_organisms.Add(organism, organism.FitnessScore);
                }

                stored_distributionsSpecies.Add(species, new ReproduceAndReplace_Node(internal_organisms, creation_species));


                //Actaully mate the organisms:
                for (int i = 0; i < num_alloted_offspring; ++i)
                {
                    Organism random_organism_1 = internal_organisms.ChooseValue();

                    Organism random_organism_2 = internal_organisms.ChooseValue(random_organism_1); //We don't want any self-replication...

                    if (random_organism_2 == null)
                    {
                        random_organism_2 = random_organism_1;  //Unless that's the only option ;) Only happens when the organism is the only one in the species.
                    }


                    Organism baby = new Organism(random_organism_1.Genome.Crossover(random_organism_1.FitnessScore, random_organism_2.Genome, random_organism_2.FitnessScore,
                        Pedigree.Random));

                    next_generation.Add(baby);

                    creation_species.AddOrganism(baby);
                }
            }


            if (next_generation.Count != NumOrganisms)   //We need one more organism, give it to a random species. Super rare that this doesn't happen.
            {
                ReproduceAndReplace_Node chosen_last_node = stored_distributionsSpecies[Species.RandomValue()];


                ScoredDistribution<Organism> last_round_distribution = chosen_last_node.scoredDistribution_organisms;

                Species last_round_species = chosen_last_node.new_species;


                Organism random_organism_1 = last_round_distribution.ChooseValue();

                Organism random_organism_2 = last_round_distribution.ChooseValue(random_organism_1); //We don't want any self-replication...

                if (random_organism_2 == null)
                {
                    random_organism_2 = random_organism_1;  //Unless that's the only option ;) Only happens when the organism is the only one in the species.
                }


                Organism baby = new Organism(random_organism_1.Genome.Crossover(random_organism_1.FitnessScore, random_organism_2.Genome, random_organism_2.FitnessScore,
                        Pedigree.Random));

                next_generation.Add(baby);

                last_round_species.AddOrganism(baby);
            }

            #endregion Reproduction


            #region Replacement

            //Organisms:
            Organisms.Clear();

            Organisms.AddRange(next_generation);


            //Species:
            Species.Clear();

            foreach (Species species in next_species)
            {
                Species.Add(species);
            }

            #endregion Replacement
        }


        /// <summary>
        /// Helper struct for holding on to some values for use in the <see cref="NEAT.NEATClient.ReproduceAndReplace"/> method.
        /// </summary>
        private struct ReproduceAndReplace_Node
        {
            public ScoredDistribution<Organism> scoredDistribution_organisms;

            public Species new_species;


            public ReproduceAndReplace_Node(ScoredDistribution<Organism> scoredDistribution_organisms, Species new_species)
            {
                this.scoredDistribution_organisms = scoredDistribution_organisms;

                this.new_species = new_species;
            }
        }

        #endregion Evolution
    }
}
