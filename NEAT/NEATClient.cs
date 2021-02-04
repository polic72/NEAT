using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NEAT.Genetic.Tracker;
using NEAT.Speciation;
using NEAT.Genetic;

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


        #region Constructors

        /// <summary>
        /// Constructs a NEAT client with the given pedigree, number of organisms, and every constant.
        /// </summary>
        /// <param name="pedigree">The pedigree used to track all genes in this NEAT client.</param>
        /// <param name="numOrganisms">The total number of organisms this NEAT client will train.</param>
        /// <param name="compatibility_distance">The initial compatibility distance.</param>
        /// <param name="CD_function">The function to adjust the compatibility distance by. Good options are the <see cref="NEAT.NEATClient.NERO_CD_Function(int)"/> and the 
        /// <see cref="NEAT.NEATClient.Constant_CD_Function(int)"/></param>
        public NEATClient(Pedigree pedigree, int numOrganisms, double compatibility_distance, CompatibilityDistanceFunction CD_function)
        {
            #region Internal Setters

            Pedigree = pedigree;

            NumOrganisms = numOrganisms;


            Organisms = new List<Organism>(numOrganisms);

            Species = new HashSet<Species>();


            CompatibilityDistance = compatibility_distance;

            this.CD_function = CD_function;

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
        /// Constructs a NEAT client with the given pedigree and number of organisms. Uses 4 for the initial compatibility distance and <see cref="NEAT.NEATClient.NERO_CD_Function(int)"/> 
        /// for the CompatibilityDistanceFunction.
        /// </summary>
        /// <param name="pedigree">The pedigree used to track all genes in this NEAT client.</param>
        /// <param name="numOrganisms">The total number of organisms this NEAT client will train.</param>
        public NEATClient(Pedigree pedigree, int numOrganisms)
            : this(pedigree, numOrganisms, 4, NERO_CD_Function)
        {

        }

        #endregion Constructors


        /// <summary>
        /// Separates the current organisms into the correct species. Makes new species as needed.
        /// </summary>
        public void Speciate()
        {
            foreach (Organism organism in Organisms)
            {
                bool found_species = false;

                foreach (Species species in Species)
                {
                    Organism random_organism = species.GetRandomOrganism(Pedigree.Random, organism);

                    if (random_organism == null)    //The organism we're at is the only organism in the species we're at. Leave it in this species.
                    {
                        found_species = true;
                        break;
                    }

                    if (organism.Genome.Distance(random_organism.Genome) < CompatibilityDistance)
                    {
                        if (organism.Species != random_organism.Species)    //If the species to set is actually different.
                        {
                            organism.Species.RemoveOrganism(organism);


                            species.AddOrganism(organism);
                        }

                        found_species = true;
                        break;
                    }
                }

                if (!found_species)
                {
                    organism.Species.RemoveOrganism(organism);


                    Species new_species = new Species();

                    Species.Add(new_species);

                    new_species.AddOrganism(organism);
                }
            }
        }
    }
}
