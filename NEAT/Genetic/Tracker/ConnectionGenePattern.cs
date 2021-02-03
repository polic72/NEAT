using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEAT.Genetic.Tracker
{
    /// <summary>
    /// A gene pattern specifying a ConnectionGene to track. This gene will be used construct said <see cref="NEAT.Genetic.ConnectionGene"/>.
    /// </summary>
    public class ConnectionGenePattern : GenePattern
    {
        #region Properties

        /// <summary>
        /// The left-hand node of this connection.
        /// </summary>
        public NodeGenePattern From { get; }

        /// <summary>
        /// The right-hand node of this connection.
        /// </summary>
        public NodeGenePattern To { get; }

        /// <summary>
        /// The innovation number of the node that replaces this connection on <see cref="NEAT.Genetic.Genome.Mutate_Node"/>.
        /// </summary>
        public int ReplacingNumber { get; }

        #endregion Properties


        /// <summary>
        /// Constructs a connection gene pattern with the given innovation number, from/to nodes, and replacing number.
        /// </summary>
        /// <param name="pedigree">The owning pedigree of this connection gene pattern.</param>
        /// <param name="innovation_number">The innovation number.</param>
        /// <param name="from">The NodeGene to connect from.</param>
        /// <param name="to">The NodeGene to connect to.</param>
        /// <param name="replacing_number">The innovation number of the node that replaces this connection.</param>
        /// <exception cref="ArgumentNullException">When from/to is null.</exception>
        protected internal ConnectionGenePattern(Pedigree pedigree, int innovation_number, NodeGenePattern from, NodeGenePattern to, int replacing_number) :
            base(pedigree, innovation_number)
        {
            Helpers.ThrowOnNull(from, "from");
            Helpers.ThrowOnNull(to, "to");


            From = from;
            To = to;

            ReplacingNumber = replacing_number;
        }


        /// <summary>
        /// Whether or not this connection gene pattern is equal to the given object.
        /// </summary>
        /// <param name="obj">The object to test.</param>
        /// <returns>True if obj is a ConnectionGenePattern object and From/To match up. False otherwise.</returns>
        public override bool Equals(object obj)
        {
            if (obj is ConnectionGenePattern connectionGenePattern)
            {
                if (From == connectionGenePattern.From && To == connectionGenePattern.To)
                {
                    return true;
                }
            }

            return false;
        }


        /// <summary>
        /// Gets the hash code of this connection gene pattern. Based off the innovation numbers of the From/To node gene patterns.
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode()
        {
            return From.InnovationNumber * Pedigree.MaxNodes + To.InnovationNumber;
        }


        /// <summary>
        /// Gets the hash code of a potential connection gene pattern. Based off the innovation numbers of the given node gene patterns.
        /// </summary>
        /// <param name="pedigree">The owning pedigree of the potential connection gene pattern.</param>
        /// <param name="from">The from node of the potential connection gene pattern.</param>
        /// <param name="to">The to node of the potential connection gene pattern.</param>
        /// <returns>The hash code.</returns>
        /// <exception cref="ArgumentNullException">When from/to is null.</exception>
        public static int GetHashCode(Pedigree pedigree, NodeGenePattern from, NodeGenePattern to)
        {
            Helpers.ThrowOnNull(from, "from");
            Helpers.ThrowOnNull(to, "to");

            return from.InnovationNumber * pedigree.MaxNodes + to.InnovationNumber;
        }


        /// <summary>
        /// Whether or not the left connection gene pattern is equal to the right one.
        /// </summary>
        /// <param name="left">The left connection gene pattern to test.</param>
        /// <param name="right">The right connection gene pattern to test.</param>
        /// <returns>True if both connection gene patterns have the same From/To node gene patterns. False otherwise.</returns>
        public static bool operator ==(ConnectionGenePattern left, ConnectionGenePattern right)
        {
            if (left is null)
            {
                if (right is null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return left.Equals(right);
        }


        /// <summary>
        /// Whether or not the left connection gene pattern is not equal to the right one.
        /// </summary>
        /// <param name="left">The left connection gene pattern to test.</param>
        /// <param name="right">The right connection gene pattern to test.</param>
        /// <returns>False if both connection gene patterns have the same From/To node gene patterns. True otherwise.</returns>
        public static bool operator !=(ConnectionGenePattern left, ConnectionGenePattern right)
        {
            if (left is null)
            {
                if (right is null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }

            return !left.Equals(right);
        }


        /// <summary>
        /// String representation of this connection gene pattern.
        /// </summary>
        /// <returns>String representation of this connection gene pattern.</returns>
        public override string ToString()
        {
            return base.ToString() + ", From: {" + From.ToString() + "}, To: {" + To.ToString() + "}, RepNum: " + ReplacingNumber;
        }
    }
}
