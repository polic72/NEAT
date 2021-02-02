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
    /// A gene specifying a synapse in the NN. This gene will be used construct a 
    /// <see cref="NEAT.Neural_Network.Connection"/>.
    /// </summary>
    public class ConnectionGene
    {
        #region Properties

        /// <summary>
        /// The connection gene pattern assosciated with this connection gene.
        /// </summary>
        public ConnectionGenePattern ConnectionGenePattern { get; }

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
        /// Constructs a connection gene with the given weight and enabled.
        /// </summary>
        /// <param name="connectionGenePattern">The connection gene pattern that this connection gene implements.</param>
        /// <param name="weight">The weight of the connection.</param>
        /// <param name="enabled">Whether or not this connection is enabled.</param>
        /// <exception cref="ArgumentNullException">When the connection gene pattern is null.</exception>
        protected internal ConnectionGene(ConnectionGenePattern connectionGenePattern, double weight, bool enabled)
        {
            Helpers.ThrowOnNull(connectionGenePattern, "connectionGenePattern");


            ConnectionGenePattern = connectionGenePattern;

            Weight = weight;
            Enabled = enabled;
        }


        /// <summary>
        /// Constructs a connection gene with and enabled (true), and the given weight.
        /// </summary>
        /// <param name="connectionGenePattern">The connection gene pattern that this connection gene implements.</param>
        /// <param name="weight">The weight of the connection.</param>
        /// <exception cref="ArgumentNullException">When the connection gene pattern is null.</exception>
        protected internal ConnectionGene(ConnectionGenePattern connectionGenePattern, double weight)
            : this(connectionGenePattern, weight, true)
        {

        }

        #endregion Constructors


        /// <summary>
        /// Whether or not this connection gene is equal to the given object.
        /// </summary>
        /// <param name="obj">The object to test.</param>
        /// <returns>True if obj is a ConnectionGene object and ConnectionGenePatterns match up. False otherwise.</returns>
        public override bool Equals(object obj)
        {
            if (obj is ConnectionGene connectionGene)
            {
                if (ConnectionGenePattern == connectionGene.ConnectionGenePattern)
                {
                    return true;
                }
            }

            return false;
        }


        /// <summary>
        /// Gets the hash code of this connection gene. Is the ConnectionGenePattern hash code as well.
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode()
        {
            return ConnectionGenePattern.GetHashCode();
        }


        /// <summary>
        /// Whether or not the left connection gene is equal to the right one.
        /// </summary>
        /// <param name="left">The left connection gene to test.</param>
        /// <param name="right">The right connection gene to test.</param>
        /// <returns>True if both connection genes have the same ConnectionGenePatterns. False otherwise.</returns>
        public static bool operator ==(ConnectionGene left, ConnectionGene right)
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
        /// Whether or not the left connection gene is not equal to the right one.
        /// </summary>
        /// <param name="left">The left connection gene to test.</param>
        /// <param name="right">The right connection gene to test.</param>
        /// <returns>False if both connection genes have the same ConnectionGenePatterns. True otherwise.</returns>
        public static bool operator !=(ConnectionGene left, ConnectionGene right)
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
