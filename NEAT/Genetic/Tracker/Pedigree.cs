using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NEAT.Neural_Network;

namespace NEAT.Genetic.Tracker
{
    /// <summary>
    /// A class that is used to track genes "globally". Used for optimization.
    /// </summary>
    public class Pedigree
    {
        #region Local Constants

        /// <summary>
        /// The maximum number of nodes that any neural network can have.
        /// <para/>
        /// </summary>
        public int MaxNodes { get; }


        /// <summary>
        /// The C1 constant used in <see cref="NEAT.Genetic.Genome.Distance(Genome)"/> calculation.
        /// </summary>
        public double C1 { get; }

        /// <summary>
        /// The C2 constant used in <see cref="NEAT.Genetic.Genome.Distance(Genome)"/> calculation.
        /// </summary>
        public double C2 { get; }

        /// <summary>
        /// The C3 constant used in <see cref="NEAT.Genetic.Genome.Distance(Genome)"/> calculation.
        /// </summary>
        public double C3 { get; }


        /// <summary>
        /// Whether or not to use uniform crossover. If false, use blended crossover. See remarks for more details.
        /// </summary>
        /// <remarks>
        /// In uniform crossover, matching connection genes are randomly chosen for the offspring genome.
        /// <para/>
        /// In blended crossover, the connection weights of matching genes are averaged.
        /// </remarks>
        public bool UniformCrossover { get; }

        /// <summary>
        /// The delta for genome scores can fall between to be considered equal. Used in crossover.
        /// </summary>
        public double Crossover_ScoreDelta { get; }


        #region Mutation Constants

        /// <summary>
        /// The value to be the min/max [-value, value) for the <see cref="NEAT.Genetic.Genome.Mutate_WeightRandom"/> and <see cref="NEAT.Genetic.Genome.Mutate_Link"/>.
        /// </summary>
        public double Mutation_WeightRandom { get; }

        /// <summary>
        /// The strength to adjust the weight during <see cref="NEAT.Genetic.Genome.Mutate_WeightShift"/>.
        /// </summary>
        public double Mutation_WeightShift { get; }


        

        /// <summary>
        /// The probability to mutate a new connection between random nodes. (<see cref="NEAT.Genetic.Genome.Mutate_Link"/>)
        /// </summary>
        /// <remarks>
        /// Used in the <see cref="NEAT.Genetic.Genome.Mutate"/> method.
        /// </remarks>
        public double Probability_MutateLink { get; }

        /// <summary>
        /// The probability to mutate a new node, splitting a connection. (<see cref="NEAT.Genetic.Genome.Mutate_Node"/>)
        /// </summary>
        /// <remarks>
        /// Used in the <see cref="NEAT.Genetic.Genome.Mutate"/> method.
        /// </remarks>
        public double Probability_MutateNode { get; }

        /// <summary>
        /// The probability to mutate the activation function of a random node. (<see cref="NEAT.Genetic.Genome.Mutate_ActivationFunction"/>)
        /// </summary>
        /// <remarks>
        /// Used in the <see cref="NEAT.Genetic.Genome.Mutate"/> method.
        /// </remarks>
        public double Probability_MutateActivationFunction { get; }

        /// <summary>
        /// The probability to mutate the weight of a random connection by setting it randomly. (<see cref="NEAT.Genetic.Genome.Mutate_WeightRandom"/>)
        /// </summary>
        /// <remarks>
        /// Used in the <see cref="NEAT.Genetic.Genome.Mutate"/> method.
        /// </remarks>
        public double Probability_MutateWeightRandom { get; }

        /// <summary>
        /// The probability to mutate the weight of a random connection by shifting it. (<see cref="NEAT.Genetic.Genome.Mutate_WeightShift"/>)
        /// </summary>
        /// <remarks>
        /// Used in the <see cref="NEAT.Genetic.Genome.Mutate"/> method.
        /// </remarks>
        public double Probability_MutateWeightShift { get; }

        /// <summary>
        /// The probability to mutate a random connection by toggling its Enabled state. (<see cref="NEAT.Genetic.Genome.Mutate_LinkToggle"/>)
        /// </summary>
        /// <remarks>
        /// Used in the <see cref="NEAT.Genetic.Genome.Mutate"/> method.
        /// </remarks>
        public double Probability_MutateLinkToggle { get; }

        #endregion Mutation Constants


        #region Defaults

        /// <summary>
        /// 
        /// </summary>
        public static class ConstantDefaults
        {
            /// <summary>
            /// The default value for <see cref="NEAT.Genetic.Tracker.Pedigree.MaxNodes"/>.
            /// </summary>
            public const int Default_MaxNodes = 1048576;


            /// <summary>
            /// The default value for <see cref="NEAT.Genetic.Tracker.Pedigree.C1"/>.
            /// </summary>
            public const double Default_C1 = 1;

            /// <summary>
            /// The default value for <see cref="NEAT.Genetic.Tracker.Pedigree.C2"/>.
            /// </summary>
            public const double Default_C2 = 1;

            /// <summary>
            /// The default value for <see cref="NEAT.Genetic.Tracker.Pedigree.C3"/>.
            /// </summary>
            public const double Default_C3 = 0.4;


            /// <summary>
            /// The default value for <see cref="NEAT.Genetic.Tracker.Pedigree.UniformCrossover"/>.
            /// </summary>
            public const bool Default_UniformCrossover = true;

            /// <summary>
            /// The default value for <see cref="NEAT.Genetic.Tracker.Pedigree.Crossover_ScoreDelta"/>.
            /// </summary>
            public const double Default_CrossoverScoreDelta = 0.001;


            #region Mutation Constants

            /// <summary>
            /// The default value for <see cref="NEAT.Genetic.Tracker.Pedigree.Mutation_WeightRandom"/>.
            /// </summary>
            public const double Default_MutationWeightRandom = 1;


            /// <summary>
            /// The default value for <see cref="NEAT.Genetic.Tracker.Pedigree.Mutation_WeightShift"/>.
            /// </summary>
            public const double Default_MutationWeightShift = 0.3;




            /// <summary>
            /// The default value for <see cref="NEAT.Genetic.Tracker.Pedigree.Probability_MutateLink"/>.
            /// </summary>
            public const double Default_ProbabilityMutateLink = 0.05;

            /// <summary>
            /// The default value for <see cref="NEAT.Genetic.Tracker.Pedigree.Probability_MutateNode"/>.
            /// </summary>
            public const double Default_ProbabilityMutateNode = 0.03;

            /// <summary>
            /// The default value for <see cref="NEAT.Genetic.Tracker.Pedigree.Probability_MutateActivationFunction"/>.
            /// </summary>
            public const double Default_ProbabilityMutateActivationFunction = 0.03;

            /// <summary>
            /// The default value for <see cref="NEAT.Genetic.Tracker.Pedigree.Probability_MutateWeightRandom"/>.
            /// </summary>
            public const double Default_ProbabilityMutateWeightRandom = 0.03;

            /// <summary>
            /// The default value for <see cref="NEAT.Genetic.Tracker.Pedigree.Probability_MutateWeightShift"/>.
            /// </summary>
            public const double Default_ProbabilityMutateWeightShift = 0.05;

            /// <summary>
            /// The default value for <see cref="NEAT.Genetic.Tracker.Pedigree.Probability_MutateLinkToggle"/>.
            /// </summary>
            public const double Default_ProbabilityMutateLinkToggle = 0.04;

            #endregion Mutation Constants
        }

        #endregion Defaults

        #endregion Local Constants


        #region Properties

        /// <summary>
        /// The random object used for all randomization.
        /// </summary>
        public Random Random { get; }


        /// <summary>
        /// The number of input nodes for every genome.
        /// </summary>
        public int Num_InputNodes { get; }

        /// <summary>
        /// The number of output nodes for every genome.
        /// </summary>
        public int Num_OutputNodes { get; }

        #endregion Properties


        private readonly Dictionary<int, NodeGenePattern> nodeGenePatterns;
        private readonly Dictionary<int, ConnectionGenePattern> connectionGenePatterns;


        private static int max_replacingNumber;


        private List<Node.ActivationFunction> known_activationFunctions;


        #region Constructors

        /// <summary>
        /// Constructs a pedigree with the given initial max replacing number and every local constant. Adds Sigmoid and ReLU to known activation functions.
        /// </summary>
        /// <param name="num_inputNodes">The number of input nodes for every genome.</param>
        /// <param name="num_outputNodes">The number of output nodes for every genome.</param>
        /// <param name="random">The random object used for all randomization.</param>
        /// 
        /// <param name="max_nodes">The maximum number of nodes a neural network can have.</param>
        /// <param name="c1">The c1 constant to set. See <see cref="NEAT.Genetic.Genome.Distance(Genome)"/> for more information.</param>
        /// <param name="c2">The c2 constant to set. See <see cref="NEAT.Genetic.Genome.Distance(Genome)"/> for more information.</param>
        /// <param name="c3">The c3 constant to set. See <see cref="NEAT.Genetic.Genome.Distance(Genome)"/> for more information.</param>
        /// <param name="uniformCrossover">Whether or not to use uniform crossover. <see cref="NEAT.Genetic.Tracker.Pedigree.UniformCrossover"/> for more information.</param>
        /// <param name="crossover_scoreDelta">The delta for genome scores can fall between to be considered equal. Used in crossover.</param>
        /// 
        /// <param name="mutate_weightRandom">The value to be the min/max [-value, value) for random weight mutations.</param>
        /// <param name="mutate_weightShift">The strength to adjust the weight for weight shift mutations.</param>
        /// 
        /// <param name="probability_mutateLink">The probability to mutate a new connection between random nodes.</param>
        /// <param name="probability_mutateNode">The probability to mutate a new node, splitting a connection.</param>
        /// <param name="probability_mutateActivationFunction">The probability to mutate the activation function of a random node.</param>
        /// <param name="probability_mutateWeightRandom">The probability to mutate the weight of a random connection by setting it randomly.</param>
        /// <param name="probability_mutateWeightShift">The probability to mutate the weight of a random connection by shifting it.</param>
        /// <param name="probability_mutateLinkToggle">The probability to mutate a random connection by toggling its Enabled state.</param>
        public Pedigree(int num_inputNodes, int num_outputNodes, Random random, int max_nodes, double c1, double c2, double c3, bool uniformCrossover, double crossover_scoreDelta,
            double mutate_weightRandom, double mutate_weightShift,
            double probability_mutateLink, double probability_mutateNode, double probability_mutateActivationFunction, double probability_mutateWeightRandom,
            double probability_mutateWeightShift, double probability_mutateLinkToggle)
        {
            #region Internal Setters

            nodeGenePatterns = new Dictionary<int, NodeGenePattern>();
            connectionGenePatterns = new Dictionary<int, ConnectionGenePattern>();


            Num_InputNodes = num_inputNodes;
            Num_OutputNodes = num_outputNodes;

            Random = random;


            max_replacingNumber = Num_InputNodes + Num_OutputNodes + 1;


            known_activationFunctions = new List<Node.ActivationFunction>();
            random = new Random();

            known_activationFunctions.Add(Node.Sigmoid);
            known_activationFunctions.Add(Node.ReLU);

            #endregion Internal Setters


            #region Local Constants

            MaxNodes = max_nodes;

            C1 = c1;
            C2 = c2;
            C3 = c3;

            UniformCrossover = uniformCrossover;

            Crossover_ScoreDelta = crossover_scoreDelta;


            Mutation_WeightRandom = mutate_weightRandom;

            Mutation_WeightShift = mutate_weightShift;


            Probability_MutateLink = probability_mutateLink;
            Probability_MutateNode = probability_mutateNode;
            Probability_MutateActivationFunction = probability_mutateActivationFunction;
            Probability_MutateWeightRandom = probability_mutateWeightRandom;
            Probability_MutateWeightShift = probability_mutateWeightShift;
            Probability_MutateLinkToggle = probability_mutateLinkToggle;

            #endregion Local Constants


            #region Node Initialization

            for (int i = 1; i <= Num_InputNodes; ++i)
            {
                nodeGenePatterns.Add(i, new NodeGenePattern(this, i, Node.INPUT_X));
            }


            for (int i = 1; i <= Num_OutputNodes; ++i)
            {
                nodeGenePatterns.Add(i + Num_InputNodes, new NodeGenePattern(this, i + Num_InputNodes, Node.OUTPUT_X));
            }

            #endregion Node Initialization
        }


        /// <summary>
        /// Constructs a pedigree with the given initial max replacing number. Adds Sigmoid and ReLU to known activation functions.
        /// </summary>
        /// <param name="num_inputNodes">The number of input nodes for every genome.</param>
        /// <param name="num_outputNodes">The number of output nodes for every genome.</param>
        /// <param name="random">The random object used for all randomization.</param>
        /// <remarks>
        /// Uses the default values for every constant. See <see cref="NEAT.Genetic.Tracker.Pedigree.ConstantDefaults"/>.
        /// </remarks>
        public Pedigree(int num_inputNodes, int num_outputNodes, Random random)
            : this(num_inputNodes, num_outputNodes, random,
                  ConstantDefaults.Default_MaxNodes, ConstantDefaults.Default_C1, ConstantDefaults.Default_C2, ConstantDefaults.Default_C3,
                  ConstantDefaults.Default_UniformCrossover, ConstantDefaults.Default_CrossoverScoreDelta,
                  ConstantDefaults.Default_MutationWeightRandom, ConstantDefaults.Default_MutationWeightShift,

                  ConstantDefaults.Default_ProbabilityMutateLink, ConstantDefaults.Default_ProbabilityMutateNode, ConstantDefaults.Default_ProbabilityMutateActivationFunction,
                  ConstantDefaults.Default_ProbabilityMutateWeightRandom, ConstantDefaults.Default_ProbabilityMutateWeightShift, ConstantDefaults.Default_ProbabilityMutateLinkToggle)
        {

        }

        #endregion Constructors


        #region Genome

        /// <summary>
        /// Creates a genome with the only the input and output nodes. Each have the <see cref="NEAT.Neural_Network.Node.Sigmoid(double)"/> activation function.
        /// </summary>
        /// <returns>The created genome.</returns>
        public Genome CreateGenome()
        {
            Genome genome = new Genome(this, Random);

            for (int i = 1; i <= (Num_InputNodes + Num_OutputNodes); ++i)
            {
                genome.NodeGenes.Add(i, new NodeGene(nodeGenePatterns[i]));
            }

            return genome;
        }

        #endregion Genome


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
            return known_activationFunctions[Random.Next(known_activationFunctions.Count)];
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
                choice = Random.Next(known_activationFunctions.Count);
            }
            else
            {
                choice = Random.Next(known_activationFunctions.Count - 1);

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
                pattern = new NodeGenePattern(this, connectionGene.ConnectionGenePattern.ReplacingNumber,
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


            int innovation_number = ConnectionGenePattern.GetHashCode(this, from.NodeGenePattern, to.NodeGenePattern);

            if (connectionGenePatterns.ContainsKey(innovation_number))
            {
                return new ConnectionGene(connectionGenePatterns[innovation_number], weight, enabled);
            }


            ConnectionGenePattern created_connectionGenePattern = new ConnectionGenePattern(this, innovation_number, from.NodeGenePattern, to.NodeGenePattern, max_replacingNumber++);

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
