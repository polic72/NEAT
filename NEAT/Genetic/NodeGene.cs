using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NEAT.Neural_Network;
using NEAT.Genetic.Tracker;

namespace NEAT.Genetic
{
    /// <summary>
    /// A gene specifying a nueron in the NN. This gene will be used construct a 
    /// <see cref="NEAT.Neural_Network.Node"/>.
    /// </summary>
    public class NodeGene
    {
        /// <summary>
        /// The node gene pattern assosciated with this node gene.
        /// </summary>
        public NodeGenePattern NodeGenePattern { get; }


        /// <summary>
        /// The activation function used by this node.
        /// </summary>
        public Node.ActivationFunction ActivationFunction { get; set; }


        #region Constructors

        /// <summary>
        /// Constructs a node gene with the given node gene pattern and activation function.
        /// </summary>
        /// <param name="nodeGenePattern">The node gene pattern that this node gene implements.</param>
        /// <param name="activationFunction">The activation function of the node.</param>
        /// <exception cref="ArgumentNullException">When the node gene pattern or activation function is null.</exception>
        protected internal NodeGene(NodeGenePattern nodeGenePattern, Node.ActivationFunction activationFunction)
        {
            Helpers.ThrowOnNull(nodeGenePattern, "nodeGenePattern");
            Helpers.ThrowOnNull(activationFunction, "activationFunction");


            NodeGenePattern = nodeGenePattern;

            ActivationFunction = activationFunction;
        }


        /// <summary>
        /// Constructs a node gene with the given node gene pattern and default activation function (<see cref="NEAT.Neural_Network.Node.Sigmoid(double)"/>).
        /// </summary>
        /// <param name="nodeGenePattern">The node gene pattern that this node gene implements.</param>
        /// <exception cref="ArgumentNullException">When the node gene pattern is null.</exception>
        protected internal NodeGene(NodeGenePattern nodeGenePattern)
            : this(nodeGenePattern, Node.Sigmoid)
        {

        }

        #endregion Constructors


        /// <summary>
        /// Whether or not this node gene is equal to the given object.
        /// </summary>
        /// <param name="obj">The object to test.</param>
        /// <returns>True if obj is a NodeGene object and has the same node gene pattern. False otherwise.</returns>
        public override bool Equals(object obj)
        {
            if (obj is NodeGene nodeGene)
            {
                return NodeGenePattern == nodeGene.NodeGenePattern;
            }

            return false;
        }


        /// <summary>
        /// Gets the hash code of this node gene. Is the ConnectionGenePattern hash code as well.
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode()
        {
            return NodeGenePattern.GetHashCode();
        }


        /// <summary>
        /// Whether or not the left node gene is equal to the right one.
        /// </summary>
        /// <param name="left">The left node gene to test.</param>
        /// <param name="right">The right node gene to test.</param>
        /// <returns>True if both node genes have the node gene pattern. False otherwise.</returns>
        public static bool operator ==(NodeGene left, NodeGene right)
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
        /// Whether or not the left node gene is not equal to the right one.
        /// </summary>
        /// <param name="left">The left node gene to test.</param>
        /// <param name="right">The right node gene to test.</param>
        /// <returns>False if both node genes have the same node gene pattern. True otherwise.</returns>
        public static bool operator !=(NodeGene left, NodeGene right)
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
    }
}
