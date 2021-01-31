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
        public Node From { get; }

        /// <summary>
        /// The right-hand node of this connection.
        /// </summary>
        public Node To { get; }


        /// <summary>
        /// The weight of this connection.
        /// </summary>
        public double Weight { get; set; }

        /// <summary>
        /// Whether or not this connection is enabled.
        /// </summary>
        public bool Enabled { get; set; }

        #endregion Properties


        #region Constructors

        /// <summary>
        /// Constructs a connection with the given left-hand and right-hand nodes.
        /// </summary>
        /// <param name="from">The left-hand node of this connection.</param>
        /// <param name="to">The right-hand node of this connection.</param>
        /// <param name="auto_connect">True to automatically add this connection to the nodes' connections; false if undesired.</param>
        public Connection(Node from, Node to, bool auto_connect)
        {
            From = from;
            To = to;

            Enabled = true;


            if (auto_connect)
            {
                from.Connections.Add(this);
                to.Connections.Add(this);
            }
        }


        /// <summary>
        /// Constructs a connection with the given left-hand and right-hand nodes. Automatically connects the nodes.
        /// </summary>
        /// <param name="from">The left-hand node of this connection.</param>
        /// <param name="to">The right-hand node of this connection.</param>
        public Connection(Node from, Node to)
            : this(from, to, true)
        {

        }

        #endregion Constructors
    }
}
