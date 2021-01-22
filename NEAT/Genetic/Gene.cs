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
    public abstract class Gene
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
        public int InnovationNumber { get; set; }


        /// <summary>
        /// Constructs a gene with the given innovation number.
        /// </summary>
        /// <param name="innovation_number">The innovation number.</param>
        public Gene(int innovation_number)
        {
            InnovationNumber = innovation_number;
        }
    }
}
