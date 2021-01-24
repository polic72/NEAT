using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEAT.Neural_Network
{
    /// <summary>
    /// A node in a neural network.
    /// </summary>
    public class Node : IComparable<Node>
    {
        #region Constants

        /// <summary>
        /// The X value of an input node.
        /// </summary>
        public const double INPUT_X = 0;

        /// <summary>
        /// The X value of an input node.
        /// </summary>
        public const double OUTPUT_X = 1;

        #endregion Constants


        #region Properties

        /// <summary>
        /// The X coordinate of this node.
        /// </summary>
        /// <remarks>Input nodes should have an X value of 0; output nodes should have an X value of 1.</remarks>
        public double X { get; }


        private bool manual_output;
        private double output;

        /// <summary>
        /// The output of the node.
        /// </summary>
        public double Output
        {
            get
            {
                return manual_output ? output : Calulate();
            }
        }


        /// <summary>
        /// The input connections of the node. This node should be the To of every connection.
        /// </summary>
        public HashSet<Connection> Connections { get; }

        #endregion Properties


        /// <summary>
        /// The delegate of the activation function for this node.
        /// </summary>
        /// <param name="x">The input of the function.</param>
        /// <returns>The output of the function.</returns>
        public delegate double ActivationFunction(double x);

        private ActivationFunction Activation;


        #region Constructors

        /// <summary>
        /// Constructs the node with the given x value. Uses the given activation function.
        /// </summary>
        /// <param name="x">The x coordinate of this node.</param>
        /// <param name="activationFunction">The activation function to use for this node.</param>
        public Node(double x, ActivationFunction activationFunction)
        {
            X = x;

            Activation = activationFunction;

            Connections = new HashSet<Connection>();
        }


        /// <summary>
        /// Constructs the node with the given x value. Uses the sigmoid activation function.
        /// </summary>
        /// <param name="x">The x coordinate of this node.</param>
        public Node(double x)
            : this(x, Sigmoid)
        {

        }


        /// <summary>
        /// Constructs the node with the <see cref="NEAT.Neural_Network.Node.INPUT_X"/> x value. Uses the sigmoid activation function.
        /// </summary>
        public Node()
            : this(INPUT_X)
        {

        }

        #endregion Constructors


        /// <summary>
        /// Calculates the output of the node. Also sets the Output property.
        /// </summary>
        /// <returns>The output of the node.</returns>
        public double Calulate()
        {
            double sum = 0;

            foreach (Connection connection in Connections)
            {
                if (connection.Enabled && connection.From.X < X)    //Does not use X values on the same or greater layers. Ignores this Node at the same time, genius.
                {
                    sum += connection.From.Output * connection.Weight;
                }
            }

            output = Activation(sum);

            return output;
        }


        /// <summary>
        /// Sets the output of this node to the given value. This should ONLY be used for input nodes.
        /// </summary>
        /// <param name="output">The value to set the output to.</param>
        public void SetOutput(double output)
        {
            manual_output = true;

            this.output = output;
        }


        /// <summary>
        /// Compares this Node to the given Node.
        /// </summary>
        /// <param name="other">The other node to compare this node to.</param>
        /// <returns>-1 if this node's X is greater than the given, 1 if lesser than, 0 otherwise.</returns>
        public int CompareTo(Node other)
        {
            if (X > other.X)
            {
                return -1;
            }
            else if (X < other.X)
            {
                return 1;
            }

            return 0;
        }


        /// <summary>
        /// The sigmoid function. Used as the default activation function.
        /// </summary>
        /// <param name="x">The input for the function.</param>
        /// <returns>The output of the sigmoid function.</returns>
        public static double Sigmoid(double x)
        {
            return 1.0 / (1 + Math.Exp(-x));
        }
    }
}
