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

        public void Evolve()
        {

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

        #endregion Evolution
    }
}
