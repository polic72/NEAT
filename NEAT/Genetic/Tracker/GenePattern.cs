using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEAT.Genetic.Tracker
{
    public abstract class GenePattern : IComparable<GenePattern>
    {
        /// <summary>
        /// The innovation number of this gene pattern.
        /// </summary>
        /// <remarks>
        /// The innovation number represents the novelty of this gene pattern in the grand scope of the gene tracker.
        /// <para/>
        /// NodeGenePatterns with the same replacing-function and ConnectionGenePatterns with the same attached NodeGenePatterns are considered to have the same innovation number.
        /// </remarks>
        public int InnovationNumber { get; }


        #region Constructors

        /// <summary>
        /// Constructs a gene pattern with the given innovation number.
        /// </summary>
        /// <param name="innovation_number">The innovation number.</param>
        public GenePattern(int innovation_number)
        {
            InnovationNumber = innovation_number;
        }


        /// <summary>
        /// Constructs a gene pattern with the default innovation number (0).
        /// </summary>
        public GenePattern()
            : this(0)
        {

        }

        #endregion Constructors


        /// <summary>
        /// Compares this gene pattern with the given gene pattern. Based on their InnovationNumbers.
        /// </summary>
        /// <param name="other">The other gene pattern to compare to.</param>
        /// <returns>-1 when this InnovationNumber is lower, 1 when this InnovationNumber is greater, 0 otherwise.</returns>
        public int CompareTo(GenePattern other)
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
