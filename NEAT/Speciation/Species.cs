using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEAT.Speciation
{
    /// <summary>
    /// A group of genetically similar organisms.
    /// </summary>
    public class Species
    {
        #region Properties

        /// <summary>
        /// The stored organisms in this species.
        /// </summary>
        protected HashSet<Organism> Organisms { get; }


        /// <summary>
        /// The average fitness score of the species.
        /// </summary>
        public double AverageFitnessScore { get; internal set; }


        /// <summary>
        /// The number of organisms in this species.
        /// </summary>
        public int Size => Organisms.Count;

        #endregion Properties


        /// <summary>
        /// Constructs a species.
        /// </summary>
        public Species()
        {
            Organisms = new HashSet<Organism>();
        }


        #region Organisms

        /// <summary>
        /// Adds this organism to the species. Sets the <see cref="NEAT.Speciation.Organism.Species"/> propterty to this species.
        /// </summary>
        /// <param name="organism">The organism to add.</param>
        /// <returns>False if the organism is already present, therefore not added. True otherwise.</returns>
        public bool AddOrganism(Organism organism)
        {
            organism.Species = this;

            return Organisms.Add(organism);
        }


        /// <summary>
        /// Removes this organism from the species. Sets the <see cref="NEAT.Speciation.Organism.Species"/> propterty to null.
        /// </summary>
        /// <param name="organism">The organism to remove.</param>
        /// <returns>True if the organism is already present, therefore removed. False otherwise.</returns>
        public bool RemoveOrganism(Organism organism)
        {
            organism.Species = null;

            return Organisms.Remove(organism);
        }


        /// <summary>
        /// Tells whether or not the given organism is in this species.
        /// </summary>
        /// <param name="organism">The organism to check.</param>
        /// <returns>True if the organism is present. False otherwise.</returns>
        public bool ContainsOrganism(Organism organism)
        {
            return Organisms.Contains(organism);
        }


        /// <summary>
        /// Gets a random organism.
        /// </summary>
        /// <param name="random">The random object to use.</param>
        /// <returns>A random organism.</returns>
        public Organism GetRandomOrganism(Random random)
        {
            return Organisms.RandomValue(random);
        }


        /// <summary>
        /// Gets a random organism excluding the given organism.
        /// </summary>
        /// <param name="random">The random object to use.</param>
        /// <param name="excluding_organism">The organism to exclude from the search.</param>
        /// <returns>A random organism.</returns>
        public Organism GetRandomOrganism(Random random, Organism excluding_organism)
        {
            return Organisms.RandomValue(random, excluding_organism);
        }

        #endregion Organisms


        /// <summary>
        /// Calculates the average fitness score of the species.
        /// </summary>
        public void CalculateFitnessScore()
        {
            if (Organisms.Count == 0)
            {
                AverageFitnessScore = 0;
            }
            else
            {
                AverageFitnessScore = Organisms.Average(x => x.FitnessScore);
            }
        }
    }
}
