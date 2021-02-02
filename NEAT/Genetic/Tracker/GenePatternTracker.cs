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


        private List<Node.ActivationFunction> known_activationFunctions;
        private Random AF_random;


        /// <summary>
        /// Constructs a gene pattern tracker with the given initial max replacing number. Adds Sigmoid and ReLU to known activation functions.
        /// </summary>
        /// <param name="initial_replacingNumber">Should be #input_nodes + #output_nodes + 1.</param>
        public GenePatternTracker(int initial_replacingNumber)
        {
            nodeGenePatterns = new Dictionary<int, NodeGenePattern>();
            connectionGenePatterns = new Dictionary<int, ConnectionGenePattern>();

            max_replacingNumber = initial_replacingNumber;


            known_activationFunctions = new List<Node.ActivationFunction>();
            AF_random = new Random();

            known_activationFunctions.Add(Node.Sigmoid);
            known_activationFunctions.Add(Node.ReLU);
        }


        #region Node_ActivationFunctions

        /// <summary>
        /// Adds the given activation function to the known activation functions.
        /// </summary>
        /// <param name="activationFunction">The activation function to add.</param>
        /// <returns>False if activation function is already added. True otherwise.</returns>
        public bool Add_KnownActivationFunction(Node.ActivationFunction activationFunction)
        {
            if (known_activationFunctions.Contains(activationFunction))
            {
                return false;
            }

            known_activationFunctions.Add(activationFunction);

            return true;
        }


        /// <summary>
        /// Gets a random kown activation function.
        /// </summary>
        /// <returns>The random known activation function.</returns>
        public Node.ActivationFunction GetRandomActivationFunction()
        {
            return known_activationFunctions[AF_random.Next(known_activationFunctions.Count)];
        }


        /// <summary>
        /// Gets a random kown activation function excluding the given activation function.
        /// </summary>
        /// <param name="excluding_activationFunction">The activation function to exclude from the search.</param>
        /// <returns>The random known activation function.</returns>
        public Node.ActivationFunction GetRandomActivationFunction(Node.ActivationFunction excluding_activationFunction)
        {
            if (known_activationFunctions.Count == 0)
            {
                return null;
            }
            else if (known_activationFunctions.Count == 1)
            {
                return known_activationFunctions[0];
            }

            int choice = -1;

            int index = known_activationFunctions.IndexOf(excluding_activationFunction);
            if (index == -1)
            {
                choice = AF_random.Next(known_activationFunctions.Count);
            }
            else
            {
                choice = AF_random.Next(known_activationFunctions.Count - 1);

                if (choice >= index)
                {
                    choice++;
                }
            }

            return known_activationFunctions[choice];
        }

        #endregion Node_ActivationFunctions


        #region NodeGenePattern

        /// <summary>
        /// Adds the given node gene pattern if not already there.
        /// </summary>
        /// <param name="nodeGenePattern">The node gene pattern to add.</param>
        /// <returns>True if added. False if already contained.</returns>
        /// <exception cref="ArgumentNullException">When the node gene pattern is null.</exception>
        public bool AddNodeGenePattern(NodeGenePattern nodeGenePattern)
        {
            Helpers.ThrowOnNull(nodeGenePattern, "nodeGenePattern");


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
        /// <exception cref="ArgumentNullException">When the node gene pattern or activation function is null.</exception>
        public NodeGene Create_NodeGene(NodeGenePattern nodeGenePattern, Node.ActivationFunction activationFunction)
        {
            return new NodeGene(nodeGenePattern, activationFunction);
        }


        /// <summary>
        /// Creates a node gene with the intention of splitting the given connection gene. Gives it a random known activation function.
        /// </summary>
        /// <remarks>
        /// If the node gene pattern does not yet exist, it will be created. Its innovation number will be the replacing number in the given connection gene's pattern. The X will be the 
        /// average of the Xs in the given connection gene pattern's From/To nodes.
        /// </remarks>
        /// <param name="connectionGene">The connection gene with the intention of splitting.</param>
        /// <returns>The created node gene.</returns>
        /// <exception cref="ArgumentNullException">When the connection gene is null.</exception>
        public NodeGene Create_NodeGene(ConnectionGene connectionGene)
        {
            Helpers.ThrowOnNull(connectionGene, "connectionGene");


            NodeGenePattern pattern;

            if (nodeGenePatterns.ContainsKey(connectionGene.ConnectionGenePattern.ReplacingNumber))
            {
                pattern = nodeGenePatterns[connectionGene.ConnectionGenePattern.ReplacingNumber];
            }
            else
            {
                pattern = new NodeGenePattern(connectionGene.ConnectionGenePattern.ReplacingNumber,
                    (connectionGene.ConnectionGenePattern.From.X + connectionGene.ConnectionGenePattern.To.X) / 2);

                nodeGenePatterns.Add(pattern.InnovationNumber, pattern);
            }

            return new NodeGene(pattern, GetRandomActivationFunction());
        }


        /// <summary>
        /// Copies the given node gene.
        /// </summary>
        /// <param name="nodeGene">The node gene to copy.</param>
        /// <returns>The copy of the node gene.</returns>
        /// <exception cref="ArgumentNullException">When the node gene pattern or activation function is null.</exception>
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
        /// <exception cref="ArgumentNullException">When the connection gene pattern is null.</exception>
        public bool AddConnectionGenePattern(ConnectionGenePattern connectionGenePattern)
        {
            Helpers.ThrowOnNull(connectionGenePattern, "connectionGenePattern");


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
        /// <exception cref="ArgumentNullException">When from/to is null.</exception>
        public ConnectionGene Create_ConnectionGene(NodeGene from, NodeGene to, double weight, bool enabled)
        {
            Helpers.ThrowOnNull(from, "from");
            Helpers.ThrowOnNull(to, "to");


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
        /// <exception cref="ArgumentNullException">When the connection gene pattern is null.</exception>
        public ConnectionGene Create_ConnectionGene(ConnectionGenePattern connectionGenePattern, double weight, bool enabled)
        {
            return new ConnectionGene(connectionGenePattern, weight, enabled);
        }


        /// <summary>
        /// Copies the given connection gene.
        /// </summary>
        /// <param name="connectionGene">The connection gene to copy.</param>
        /// <returns>The copy of the connection gene.</returns>
        /// <exception cref="ArgumentNullException">When the connection gene pattern is null.</exception>
        public ConnectionGene Copy_ConnectionGene(ConnectionGene connectionGene)
        {
            return new ConnectionGene(connectionGene.ConnectionGenePattern, connectionGene.Weight, connectionGene.Enabled);
        }

        #endregion ConnectionGenePattern
    }
}
