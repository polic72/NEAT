using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEAT.Neural_Network
{
    public class Node : IComparable<Node>
    {
        /// <summary>
        /// The X value of an input node.
        /// </summary>
        public const double INPUT_X = 0;

        /// <summary>
        /// The X value of an input node.
        /// </summary>
        public const double OUTPUT_X = 1;


        /// <summary>
        /// The X position of this node.
        /// </summary>
        /// <remarks>Input nodes should have an X value of 0; output nodes should have an X value of 1.</remarks>
        public double X { get; }


        private double? output;

        public double Ouput
        {
            get
            {
                return (output != null) ? output.Value : Calulate();
            }
        }


        public Node(double x)
        {
            X = x;
        }


        /// <summary>
        /// Calculates the output of the node.
        /// </summary>
        /// <returns></returns>
        public double Calulate()
        {
            output = 1;

            return output.Value;
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
            if (X < other.X)
            {
                return 1;
            }

            return 0;
        }
    }
}
