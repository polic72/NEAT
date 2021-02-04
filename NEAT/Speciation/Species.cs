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
        /// <summary>
        /// The stored organisms in this species.
        /// </summary>
        public HashSet<Organism> Organisms { get; }


        /// <summary>
        /// The average fitness score of the species.
        /// </summary>
        public double AverageFitnessScore { get; internal set; }


        /// <summary>
        /// Constructs a species.
        /// </summary>
        public Species()
        {
            Organisms = new HashSet<Organism>();
        }


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
