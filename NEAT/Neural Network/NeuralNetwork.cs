using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NEAT.Genetic;

namespace NEAT.Neural_Network
{
    /// <summary>
    /// An implementation of an artificial neural network (ANN).
    /// </summary>
    /// <remarks>
    /// Uses <see cref="NEAT.Neural_Network.Node"/>s and <see cref="NEAT.Neural_Network.Connection"/>s to actually perform
    /// the calculations. The architecture of the neural network cannot be altered after creation.
    /// </remarks>
    public class NeuralNetwork
    {
        /// <summary>
        /// The input nodes of the neural network. Do not insert or delete!
        /// TODO consider adding deticated bias node to NeuralNetwork class
        /// </summary>
        protected List<Node> input_nodes = new List<Node>();

        /// <summary>
        /// The hidden nodes of the neural network. Do not insert or delete!
        /// </summary>
        protected List<Node> output_nodes = new List<Node>();

        /// <summary>
        /// The output nodes of the neural network. Do not insert or delete!
        /// </summary>
        protected List<Node> hidden_nodes = new List<Node>();


        #region Constructors

        /// <summary>
        /// Constructs a neural network based on the given genome.
        /// </summary>
        /// <param name="genome">The genome with all of the genes to create a neural network.</param>
        public NeuralNetwork(Genome genome)
        {
            Dictionary<int, Node> temp_nodes = new Dictionary<int, Node>(); //Temporary holder to for creating connections.

            foreach (NodeGene nodeGene in genome.NodeGenes)
            {
                Node node = new Node(nodeGene.X);   //TODO add function gene.

                temp_nodes.Add(nodeGene.InnovationNumber, node);

                if (node.X == 0)
                {
                    input_nodes.Add(node);
                }
                else if (node.X == 1)
                {
                    output_nodes.Add(node);
                }
                else
                {
                    hidden_nodes.Add(node);
                }
            }


            hidden_nodes.Sort();    //Sorts by X thanks to Node.CompareTo(). This needs to happen so each hidden layer outputs in correct order.


            foreach (ConnectionGene connectionGene in genome.ConnectionGenes)
            {
                Node from = temp_nodes[connectionGene.From.InnovationNumber];   //new Node(connections[i].From.InnovationNumber);
                Node to = temp_nodes[connectionGene.To.InnovationNumber]; //new Node(connections[i].To.InnovationNumber);

                //Node from = new Node(connections[i].From.InnovationNumber);
                //Node to = new Node(connections[i].To.InnovationNumber);

                Connection connection = new Connection(from, to);
                connection.Weight = connectionGene.Weight;
                connection.Enabled = connectionGene.Enabled;

                to.Connections.Add(connection);
            }
        }


        //TODO add way to create regular neural network

        #endregion Constructors


        /// <summary>
        /// Sets the values of the input nodes to the given input and performs the feed-forward operation.
        /// </summary>
        /// <param name="input">The input of for this calculation.</param>
        /// <returns>The output of the calculation.</returns>
        /// <exception cref="System.ArgumentException">When the input array length doesn't match the number of input nodes.</exception>
        /// <remarks>
        /// The input array's indexing is based on the indexes of the input nodes. As the makeup of the neural network is not 
        /// editable, this is determinded at constrcutor-time. The same is true for the output.
        /// </remarks>
        public double[] FeedForward(double[] input)
        {
            if (input.Length != input_nodes.Count)
            {
                throw new ArgumentException("The input must be the same length as the input nodes of the genome.", "input");
            }

            for (int i = 0; i < input_nodes.Count; ++i)
            {
                input_nodes[i].SetOutput(input[i]);
            }


            foreach (Node node in hidden_nodes)
            {
                node.Calculate();
            }


            foreach (Node node in output_nodes)
            {
                node.Calculate();
            }


            return output_nodes.Select(x => x.Output).ToArray();    //Order of output nodes is preserved.
        }
    }
}
