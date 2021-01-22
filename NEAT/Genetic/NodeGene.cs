using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NEAT.Neural_Network;

namespace NEAT.Genetic
{
    /// <summary>
    /// A gene specifying a nueron in the NN. This gene will be used construct a 
    /// <see cref="NEAT.Neural_Network.Node"/>.
    /// </summary>
    public class NodeGene : Gene
    {
        /// <summary>
        /// The X position of the node.
        /// </summary>
        public double X { get; set; }


        /// <summary>
        /// The activation function used by this node.
        /// </summary>
        public Node.ActivationFunction ActivationFunction { get; set; }


        #region Constructors

        /// <summary>
        /// Constructs a node gene with the given innovation number and activation function.
        /// </summary>
        /// <param name="innovation_number">The innovation number.</param>
        /// <param name="activationFunction">The activation function of the node.</param>
        public NodeGene(int innovation_number, Node.ActivationFunction activationFunction) :
            base(innovation_number)
        {
            ActivationFunction = activationFunction;
        }


        /// <summary>
        /// Constructs a node gene with the default innovation number (0) and given activation function.
        /// </summary>
        /// <param name="activationFunction">The activation function of the node.</param>
        public NodeGene(Node.ActivationFunction activationFunction)
            : base()
        {
            ActivationFunction = activationFunction;
        }


        /// <summary>
        /// Constructs a node gene with the default innovation number (0) and activation function (sigmoid).
        /// </summary>
        public NodeGene()
            : base()
        {
            ActivationFunction = Node.Sigmoid;
        }

        #endregion Constructors


        /// <summary>
        /// Whether or not this NodeGene is equal to the given object.
        /// </summary>
        /// <param name="obj">The object to test.</param>
        /// <returns>True if obj is a NodeGene object and has the same innovation number. False otherwise.</returns>
        public override bool Equals(object obj)
        {
            if (obj is NodeGene nodeGene)
            {
                return InnovationNumber == nodeGene.InnovationNumber;
            }

            return false;
        }


        /// <summary>
        /// Gets the hash code of this NodeGene. This is equivalent to the innovation number.
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode()
        {
            return InnovationNumber;
        }


        /// <summary>
        /// Whether or not the left NodeGene is equal to the right one.
        /// </summary>
        /// <param name="left">The left NodeGene to test.</param>
        /// <param name="right">The right NodeGene to test.</param>
        /// <returns>True if both NodeGenes have the same innovation number. False otherwise.</returns>
        public static bool operator ==(NodeGene left, NodeGene right) => left.Equals(right);


        /// <summary>
        /// Whether or not the left NodeGene is not equal to the right one.
        /// </summary>
        /// <param name="left">The left NodeGene to test.</param>
        /// <param name="right">The right NodeGene to test.</param>
        /// <returns>False if both NodeGenes have the same innovation number. True otherwise.</returns>
        public static bool operator !=(NodeGene left, NodeGene right) => !left.Equals(right);
    }
}
