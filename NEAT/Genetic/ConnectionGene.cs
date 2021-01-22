using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;0

using NEAT.Neural_Network;

namespace NEAT.Genetic
{
    /// <summary>
    /// A gene specifying a synapse in the NN. This gene will be used construct a 
    /// <see cref="NEAT.Neural_Network.Connection"/>.
    /// </summary>
    public class ConnectionGene : Gene
    {
        #region Properties

        /// <summary>
        /// The left-hand node of this connection.
        /// </summary>
        public NodeGene From { get; set; }

        /// <summary>
        /// The right-hand node of this connection.
        /// </summary>
        public NodeGene To { get; set; }


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
        /// Constructs a connection gene with the given innovation number, weight, and enabled.
        /// </summary>
        /// <param name="from">The NodeGene to connect from.</param>
        /// <param name="to">The NodeGene to connect to.</param>
        /// <param name="innovation_number">The innovation number.</param>
        /// <param name="weight">The weight of the connection.</param>
        /// <param name="enabled">Whether or not this connection is enabled.</param>
        public ConnectionGene(NodeGene from, NodeGene to, int innovation_number, double weight, bool enabled) :
            base(innovation_number)
        {
            From = from;
            To = to;

            Weight = weight;
            Enabled = enabled;
        }


        /// <summary>
        /// Constructs a connection gene with the default innovation number (0) and enabled (true), and the given 
        /// weight.
        /// </summary>
        /// <param name="from">The NodeGene to connect from.</param>
        /// <param name="to">The NodeGene to connect to.</param>
        /// <param name="weight">The weight of the connection.</param>
        public ConnectionGene(NodeGene from, NodeGene to, double weight)
            : this(from, to, 0, weight, true)
        {

        }

        #endregion Constructors


        /// <summary>
        /// Whether or not this ConnectionGene is equal to the given object.
        /// </summary>
        /// <param name="obj">The object to test.</param>
        /// <returns>True if obj is a ConnectionGene object and From/To match up. False otherwise.</returns>
        public override bool Equals(object obj)
        {
            if (obj is ConnectionGene connectionGene)
            {
                if (From == connectionGene.From && To == connectionGene.To)
                {
                    return true;
                }
            }

            return false;
        }


        /// <summary>
        /// Gets the hash code of this ConnectionGene. Based off the innovation numbers of the From/To NodeGenes.
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode()
        {
            return From.InnovationNumber * Evolving_NeuralNetwork.MaxNodes + To.InnovationNumber;
        }


        /// <summary>
        /// Whether or not the left ConnectionGene is equal to the right one.
        /// </summary>
        /// <param name="left">The left ConnectionGene to test.</param>
        /// <param name="right">The right ConnectionGene to test.</param>
        /// <returns>True if both ConnectionGene have the same From/To NodeGenes. False otherwise.</returns>
        public static bool operator ==(ConnectionGene left, ConnectionGene right) => left.Equals(right);


        /// <summary>
        /// Whether or not the left ConnectionGene is not equal to the right one.
        /// </summary>
        /// <param name="left">The left ConnectionGene to test.</param>
        /// <param name="right">The right ConnectionGene to test.</param>
        /// <returns>False if both ConnectionGene have the same From/To NodeGenes. True otherwise.</returns>
        public static bool operator !=(ConnectionGene left, ConnectionGene right) => !left.Equals(right);
    }
}
