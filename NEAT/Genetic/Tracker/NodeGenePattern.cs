using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEAT.Genetic.Tracker
{
    /// <summary>
    /// A gene pattern specifying a NodeGene to track. This gene will be used construct said <see cref="NEAT.Genetic.NodeGene"/>.
    /// </summary>
    public class NodeGenePattern : GenePattern
    {
        /// <summary>
        /// The X position of the node.
        /// </summary>
        public double X { get; }


        #region Constructors

        /// <summary>
        /// Constructs a node gene pattern with the given innovation number and X position.
        /// </summary>
        /// <param name="pedigree">The owning pedigree of this node gene pattern.</param>
        /// <param name="innovation_number">The innovation number.</param>
        /// <param name="x">The X position of the node.</param>
        protected internal NodeGenePattern(Pedigree pedigree, int innovation_number, double x) :
            base(pedigree, innovation_number)
        {
            X = x;
        }

        #endregion Constructors


        /// <summary>
        /// Whether or not this node gene pattern is equal to the given object.
        /// </summary>
        /// <param name="obj">The object to test.</param>
        /// <returns>True if obj is a NodeGenePattern object and has the same innovation number. False otherwise.</returns>
        public override bool Equals(object obj)
        {
            if (obj is NodeGenePattern nodeGenePattern)
            {
                return InnovationNumber == nodeGenePattern.InnovationNumber;
            }

            return false;
        }


        /// <summary>
        /// Gets the hash code of this NodeGenePattern. This is equivalent to the innovation number.
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode()
        {
            return InnovationNumber;
        }


        /// <summary>
        /// Whether or not the left node gene pattern is equal to the right one.
        /// </summary>
        /// <param name="left">The left node gene pattern to test.</param>
        /// <param name="right">The right node gene pattern to test.</param>
        /// <returns>True if both node gene patterns have the same innovation number. False otherwise.</returns>
        public static bool operator ==(NodeGenePattern left, NodeGenePattern right)
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
        /// Whether or not the left node gene pattern is not equal to the right one.
        /// </summary>
        /// <param name="left">The left node gene pattern to test.</param>
        /// <param name="right">The right node gene pattern to test.</param>
        /// <returns>False if both node gene patterns have the same innovation number. True otherwise.</returns>
        public static bool operator !=(NodeGenePattern left, NodeGenePattern right)
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
        /// String representation of this node gene pattern.
        /// </summary>
        /// <returns>String representation of this node gene pattern.</returns>
        public override string ToString()
        {
            return base.ToString() + ", X: " + X.ToString();
        }
    }
}
