using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NEAT.Neural_Network;

namespace NEAT.Genetic.Tracker
{
    /// <summary>
    /// A class that is used to track genes globally. Used for optimization.
    /// </summary>
    public class GenePatternTracker
    {
        private readonly Dictionary<int, NodeGenePattern> nodeGenePatterns;
        private readonly Dictionary<int, ConnectionGenePattern> connectionGenePatterns;


        private static int max_replacingNumber;


        /// <summary>
        /// Constructs a gene pattern tracker with the given initial max replacing number.
        /// </summary>
        /// <param name="initial_replacingNumber">Should be #input_nodes + #output_nodes + 1.</param>
        public GenePatternTracker(int initial_replacingNumber)
        {
            nodeGenePatterns = new Dictionary<int, NodeGenePattern>();
            connectionGenePatterns = new Dictionary<int, ConnectionGenePattern>();

            max_replacingNumber = initial_replacingNumber;
        }


        #region NodeGenePattern

        /// <summary>
        /// Adds the given node gene pattern if not already there.
        /// </summary>
        /// <param name="nodeGenePattern">The node gene pattern to add.</param>
        /// <returns>True if added. False if already contained.</returns>
        public bool AddNodeGenePattern(NodeGenePattern nodeGenePattern)
        {
            if (nodeGenePatterns.ContainsKey(nodeGenePattern.InnovationNumber))
            {
                return false;
            }


            nodeGenePatterns.Add(nodeGenePattern.InnovationNumber, nodeGenePattern);

            return true;
        }


        /// <summary>
        /// Gets the node gene pattern specified by the given innovation number.
        /// </summary>
        /// <param name="innovation_number">The innovation number of the node to get.</param>
        /// <returns>The node gene with the given innovation number. Null if it was never added.</returns>
        public NodeGenePattern GetNodeGenePattern(int innovation_number)
        {
            if (nodeGenePatterns.ContainsKey(innovation_number))
            {
                return nodeGenePatterns[innovation_number];
            }

            return null;
        }


        /// <summary>
        /// Creates a node gene with the given pattern and activation function.
        /// </summary>
        /// <param name="nodeGenePattern">The node gene pattern of the node gene.</param>
        /// <param name="activationFunction">The activation function of the node gene.</param>
        /// <returns>The created node gene.</returns>
        public NodeGene Create_NodeGene(NodeGenePattern nodeGenePattern, Node.ActivationFunction activationFunction)
        {
            return new NodeGene(nodeGenePattern, activationFunction);
        }


        /// <summary>
        /// Copies the given node gene.
        /// </summary>
        /// <param name="nodeGene">The node gene to copy.</param>
        /// <returns>The copy of the node gene.</returns>
        public NodeGene Copy_NodeGene(NodeGene nodeGene)
        {
            return new NodeGene(nodeGene.NodeGenePattern, nodeGene.ActivationFunction);
        }

        #endregion NodeGenePattern


        #region ConnectionGenePattern

        /// <summary>
        /// Adds the given connection gene pattern if not already there.
        /// </summary>
        /// <param name="connectionGenePattern">The connection gene pattern to add.</param>
        /// <returns>True if added. False if already contained.</returns>
        public bool AddConnectionGenePattern(ConnectionGenePattern connectionGenePattern)
        {
            if (connectionGenePatterns.ContainsKey(connectionGenePattern.InnovationNumber))
            {
                return false;
            }


            connectionGenePatterns.Add(connectionGenePattern.InnovationNumber, connectionGenePattern);

            return true;
        }


        /// <summary>
        /// Gets the connection gene pattern specified by the given innovation number.
        /// </summary>
        /// <param name="innovation_number">The innovation number of the connection to get.</param>
        /// <returns>The connection gene pattern with the given innovation number. Null if it was never added.</returns>
        public ConnectionGenePattern GetConnectionGene(int innovation_number)
        {
            if (connectionGenePatterns.ContainsKey(innovation_number))
            {
                return connectionGenePatterns[innovation_number];
            }

            return null;
        }


        /// <summary>
        /// Creates a connection gene with the given connections, weight, and enabled. Adds the pattern to the tracker if it doesn't exist.
        /// </summary>
        /// <param name="from">The from node gene of the connection gene.</param>
        /// <param name="to">The to node gene of the connection gene.</param>
        /// <param name="weight">The weight of the connection gene.</param>
        /// <param name="enabled">Whether or not the connection is enabled.</param>
        /// <returns>The created connection gene.</returns>
        public ConnectionGene Create_ConnectionGene(NodeGene from, NodeGene to, double weight, bool enabled)
        {
            int innovation_number = ConnectionGenePattern.GetHashCode(from.NodeGenePattern, to.NodeGenePattern);

            if (connectionGenePatterns.ContainsKey(innovation_number))
            {
                return new ConnectionGene(connectionGenePatterns[innovation_number], weight, enabled);
            }


            ConnectionGenePattern created_connectionGenePattern = new ConnectionGenePattern(innovation_number, from.NodeGenePattern, to.NodeGenePattern, max_replacingNumber++);

            connectionGenePatterns.Add(innovation_number, created_connectionGenePattern);

            return new ConnectionGene(created_connectionGenePattern, weight, enabled);
        }


        /// <summary>
        /// Creates a connection gene with the given pattern, weight, and enabled.
        /// </summary>
        /// <param name="connectionGenePattern">The connection gene pattern of the connection gene.</param>
        /// <param name="weight">The weight of the connection gene.</param>
        /// <param name="enabled">Whether or not the connection is enabled.</param>
        /// <returns>The created connection gene.</returns>
        public ConnectionGene Create_ConnectionGene(ConnectionGenePattern connectionGenePattern, double weight, bool enabled)
        {
            return new ConnectionGene(connectionGenePattern, weight, enabled);
        }


        /// <summary>
        /// Copies the given connection gene.
        /// </summary>
        /// <param name="connectionGene">The connection gene to copy.</param>
        /// <returns>The copy of the connection gene.</returns>
        public ConnectionGene Copy_ConnectionGene(ConnectionGene connectionGene)
        {
            return new ConnectionGene(connectionGene.ConnectionGenePattern, connectionGene.Weight, connectionGene.Enabled);
        }

        #endregion ConnectionGenePattern
    }
}
