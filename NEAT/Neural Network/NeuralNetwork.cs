﻿using System;
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
        /// </summary>
        protected readonly List<Node> input_nodes = new List<Node>();

        /// <summary>
        /// The bias node of the neural network.
        /// </summary>
        protected readonly Node bias_node;

        /// <summary>
        /// The hidden nodes of the neural network. Do not insert or delete! Reset after each get of Output.
        /// </summary>
        protected readonly List<Node> output_nodes = new List<Node>();

        /// <summary>
        /// The output nodes of the neural network. Do not insert or delete! Reset after each get of Output.
        /// </summary>
        protected readonly List<Node> hidden_nodes = new List<Node>();


        #region Constructors

        /// <summary>
        /// Constructs a neural network based on the given genome.
        /// </summary>
        /// <param name="genome">The genome with all of the genes to create a neural network.</param>
        /// <exception cref="ArgumentNullException">When the genome is null.</exception>
        /// <remarks>
        /// The bias node is the last input node.
        /// </remarks>
        public NeuralNetwork(Genome genome)
        {
            Helpers.ThrowOnNull(genome, "genome");


            Dictionary<int, Node> temp_nodes = new Dictionary<int, Node>(); //Temporary holder for creating connections.

            Node temp_biasNode = null;

            foreach (NodeGene nodeGene in genome.NodeGenes.Values)
            {
                Node node = new Node(nodeGene.NodeGenePattern.X, nodeGene.ActivationFunction);

                temp_nodes.Add(nodeGene.NodeGenePattern.InnovationNumber, node);

                if (node.X == 0)
                {
                    input_nodes.Add(node);

                    temp_biasNode = node;   //These nodes are sorted by innovation number, we can take advantage of this so the last known input node is the bias node.
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


            input_nodes.RemoveAt(input_nodes.Count - 1);    //The bias node is in the input nodes, and should not be.

            bias_node = temp_biasNode;
            bias_node.SetOutput(1); //The bias node will always have an output of 1.


            hidden_nodes.Sort();    //Sorts by X thanks to Node.CompareTo(). This needs to happen so each hidden layer outputs in correct order.


            foreach (ConnectionGene connectionGene in genome.ConnectionGenes.Values)
            {
                Node from = temp_nodes[connectionGene.ConnectionGenePattern.From.InnovationNumber];   //new Node(connections[i].From.InnovationNumber);
                Node to = temp_nodes[connectionGene.ConnectionGenePattern.To.InnovationNumber]; //new Node(connections[i].To.InnovationNumber);

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
        /// <remarks>
        /// The input array's indexing is based on the indexes of the input nodes. As the makeup of the neural network is not 
        /// editable, this is determinded at constrcutor-time. The same is true for the output.
        /// </remarks>
        /// <exception cref="ArgumentNullException">When the input is null.</exception>
        /// <exception cref="ArgumentException">When the input is not the same size as number of input nodes.</exception>
        public double[] FeedForward(double[] input)
        {
            Helpers.ThrowOnNull(input, "input");

            if (input.Length != input_nodes.Count)
            {
                throw new ArgumentException("The input must be the same length as the input nodes of the genome.", "input");
            }

            for (int i = 0; i < input_nodes.Count; ++i)
            {
                input_nodes[i].SetOutput(input[i]);
            }


            //foreach (Node node in hidden_nodes)
            //{
            //    node.SetOutput(node.Calculate());
            //}


            //foreach (Node node in output_nodes)
            //{
            //    node.SetOutput(node.Calculate());
            //}


            double[] output = output_nodes.Select(x => x.Output).ToArray();    //Order of output nodes is preserved.


            hidden_nodes.ForEach(node => node.ResetOutput());
            output_nodes.ForEach(node => node.ResetOutput());


            return output;
        }
    }
}
