using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEAT.Neural_Network
{
    /// <summary>
    /// A synapse in a nueral network.
    /// </summary>
    public class Connection
    {
        #region Properties

        /// <summary>
        /// The left-hand node of this connection.
        /// </summary>
        public Node From { get; set; }

        /// <summary>
        /// The right-hand node of this connection.
        /// </summary>
        public Node To { get; set; }


        /// <summary>
        /// The weight of this connection.
        /// </summary>
        public double Weight { get; set; }

        /// <summary>
        /// Whether or not this connection is enabled.
        /// </summary>
        public bool Enabled { get; set; }

        #endregion Properties


        /// <summary>
        /// Constructs a connection gene with the given innovation number.
        /// </summary>
        public Connection(Node from, Node to)
        {
            From = from;
            To = to;
        }
    }
}
