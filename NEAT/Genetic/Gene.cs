using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEAT.Genetic
{
    /// <summary>
    /// A base class for a gene.
    /// </summary>
    public abstract class Gene : IComparable<Gene>
    {
        /// <summary>
        /// The innovation number of this gene.
        /// </summary>
        /// <remarks>
        /// The innovation number represents the novelty of this gene in the grand scope of the entire algorithm.
        /// 
        /// NodeGenes with the same X value and ConnectionGenes attached are considered to have the same innovation 
        /// number.
        /// 
        /// ConnectionGenes with the same from and to NodeGenes, weight, and enabled status are considered to have the 
        /// same innovation number.
        /// TODO change if Y does anything.
        /// </remarks>
        public int InnovationNumber { get; }


        /// <summary>
        /// Constructs a gene with the given innovation number.
        /// </summary>
        /// <param name="innovation_number">The innovation number.</param>
        public Gene(int innovation_number)
        {
            InnovationNumber = innovation_number;
        }


        /// <summary>
        /// Constructs a gene with the default innovation number (0).
        /// </summary>
        public Gene()
            : this(0)
        {

        }


        /// <summary>
        /// Compares this gene with the given gene. Based on their InnovationNumbers.
        /// </summary>
        /// <param name="other">The other gene to compare to.</param>
        /// <returns>-1 when this InnovationNumber is lower, 1 when this InnovationNumber is greater, 0 otherwise.</returns>
        public int CompareTo(Gene other)
        {
            if (InnovationNumber < other.InnovationNumber)
            {
                return -1;
            }
            else if (InnovationNumber > other.InnovationNumber)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }
}
