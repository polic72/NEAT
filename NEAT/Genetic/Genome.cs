using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEAT.Genetic
{
    /// <summary>
    /// A neural network made from its stored genetic material.
    /// </summary>
    /// <remarks>
    /// When comparing 2 genomes, there are a few things to think about:
    /// <para/>
    /// Similar: Two genes are similar when they share the same innovation number and are at the same spot.
    /// <para/>
    /// Disjoint: A gene is disjointed if its innovation number is less that of the neighbor in the same spot.
    /// <para/>
    /// Excess: A gene is excess if it doesn't have a neighbor.
    /// </remarks>
    public class Genome //TODO Consider adding bias node to Genome (absolutely do)
    {
        #region "Constants"

        /// <summary>
        /// The maximum number of nodes that any neural network can have.
        /// <para/>
        /// Set in the <see cref="NEAT.Genetic.Genome.Init(int, double, double, double, bool, double, double, double)"/> method.
        /// </summary>
        public static int MaxNodes { get; private set; }


        /// <summary>
        /// The C1 constant used in <see cref="NEAT.Genetic.Genome.Distance(Genome)"/> calculation.
        /// </summary>
        public static double C1 { get; private set; }

        /// <summary>
        /// The C2 constant used in <see cref="NEAT.Genetic.Genome.Distance(Genome)"/> calculation.
        /// </summary>
        public static double C2 { get; private set; }

        /// <summary>
        /// The C3 constant used in <see cref="NEAT.Genetic.Genome.Distance(Genome)"/> calculation.
        /// </summary>
        public static double C3 { get; private set; }


        /// <summary>
        /// Whether or not to use uniform crossover. If false, use blended crossover. See remarks for more details.
        /// </summary>
        /// <remarks>
        /// In uniform crossover, matching genes are randomly chosen for the offspring genome.
        /// <para/>
        /// In blended crossover, the connection weights of matching genes are averaged.
        /// </remarks>
        public static bool Uniform_Crossover { get; private set; }

        /// <summary>
        /// The delta for genome scores can fall between to be considered equal. Used in corssover.
        /// </summary>
        public static double Crossover_ScoreDelta { get; private set; }


        #region Mutation Constants

        /// <summary>
        /// The value to be the min/max [-value, value) for the 
        /// <see cref="NEAT.Genetic.Genome.Mutate_WeightRandom"/> and 
        /// <see cref="NEAT.Genetic.Genome.Mutate_Link"/>.
        /// </summary>
        public static double Mutation_WeightRandom { get; private set; }


        /// <summary>
        /// The strength to adjust the weight during 
        /// <see cref="NEAT.Genetic.Genome.Mutate_WeightShift"/>.
        /// </summary>
        public static double Mutation_WeightShift { get; private set; }

        #endregion Mutation Constants




        #region Initialization

        private static bool initialized = false;

        /// <summary>
        /// Initializes the EvolvingNN class with the given constants.
        /// </summary>
        /// <param name="max_nodes">The maximum number of nodes a neural network can have.</param>
        /// <param name="c1">The c1 constant to set. See <see cref="NEAT.Genetic.Genome.Distance(Genome)"/> for more 
        /// information.</param>
        /// <param name="c2">The c2 constant to set. See <see cref="NEAT.Genetic.Genome.Distance(Genome)"/> for more 
        /// information.</param>
        /// <param name="c3">The c3 constant to set. See <see cref="NEAT.Genetic.Genome.Distance(Genome)"/> for more 
        /// information.</param>
        /// <param name="uniform_crossover">Whether or not to use uniform crossover. See 
        /// <see cref="NEAT.Genetic.Genome.Uniform_Crossover"/> for more information.</param>
        /// <param name="crossover_scoreDelta">The delta for genome scores can fall between to be considered equal. 
        /// Used in corssover.</param>
        /// <param name="mutate_weightRandom">The value to be the min/max [-value, value) for random weight mutations.</param>
        /// <param name="mutate_weightShift">The strength to adjust the weight for weight shift mutations.</param>
        public static void Init(int max_nodes, double c1, double c2, double c3, bool uniform_crossover, double crossover_scoreDelta,
            double mutate_weightRandom, double mutate_weightShift)
        {
            if (initialized)
            {
                throw new InvalidOperationException("The Genome class is already initialized.");
            }


            MaxNodes = max_nodes;

            C1 = c1;
            C2 = c2;
            C3 = c3;

            Uniform_Crossover = uniform_crossover;

            Crossover_ScoreDelta = crossover_scoreDelta;


            Mutation_WeightRandom = mutate_weightRandom;

            Mutation_WeightShift = mutate_weightShift;


            initialized = true;
        }


        /// <summary>
        /// Initializes the EvolvingNN class with:
        /// <list type="bullet">
        /// <item>MaxNodes: 2^20</item>
        /// <item>C1: 1  <term/>  C2: 1  <term/>  C3: 0.4</item>
        /// <item>Uniform_Crossover: true</item>
        /// <item>Crossover_ScoreDelta: 0.001</item>
        /// <item>Mutation_WeightRandom: 1</item>
        /// <item>Mutation_WeightShift: 0.3</item>
        /// </list>
        /// TODO update as needed
        /// </summary>
        public static void Init()
        {
            Init((int)Math.Pow(2, 20), 1, 1, .4, true, .001, 1, .3);
        }


        /// <summary>
        /// Tells whether or not the Genome constants are initialized.
        /// </summary>
        /// <returns>True if initialized, false otherwise.</returns>
        public static bool IsInitialized()
        {
            return initialized;
        }

        #endregion Initialization

        #endregion "Constants"


        #region Properties

        /// <summary>
        /// The gene tracker associated with this genome. All genes in this genome are in this gene tracker.
        /// </summary>
        public GeneTracker GeneTracker { get; }

        /// <summary>
        /// The internal random of the genome.
        /// </summary>
        public Random Random { get; }


        /// <summary>
        /// The ConnectionGenes of this genome.
        /// </summary>
        public SortedDictionary<int, ConnectionGene> ConnectionGenes { get; }

        /// <summary>
        /// The NodeGenes of this genome.
        /// </summary>
        public SortedDictionary<int, NodeGene> NodeGenes { get; }

        #endregion Properties


        /// <summary>
        /// Constructs a genome with the given random.
        /// </summary>
        /// <param name="geneTracker">The gene tracker for the genome.</param>
        /// <param name="random">The internal random for the genome.</param>
        public Genome(GeneTracker geneTracker, Random random)
        {
            GeneTracker = geneTracker;
            Random = random;

            ConnectionGenes = new SortedDictionary<int, ConnectionGene>();
            NodeGenes = new SortedDictionary<int, NodeGene>();
        }


        /// <summary>
        /// Gets the distance between this Genome and the given Genome. Higher = less compatible.
        /// </summary>
        /// <param name="genome">The Genome to compare to.</param>
        /// <returns>The distance between this Genome and the given Genome.</returns>
        /// <remarks>
        /// The distance d can be measured by the following equation:
        /// <para/>
        /// d = c1(E / N) + c2(D / N) + c3 * W
        /// <para/>
        /// Where:
        ///     d = distance
        ///     E = # excess genes
        ///     D = # of disjoint genes
        ///     W = weight difference of similar genes
        ///     N = # of genes in largest genome (this or them), 1 if #genes &lt; 20
        ///     c_ = constant for adjusting
        /// </remarks>
        public double Distance(Genome genome)
        {
            int index_me = 0;   //Leaving indexes from old implementation due to their mathematical usefulness later.
            int index_them = 0;


            int num_excess = 0;     //The number of excess genes.
            int num_disjoint = 0;   //The number of disjoint genes.

            double num_similar = 0; //The number of genes that are similar.
            double weight_diff = 0; //The weight difference between similar genes.


            SortedDictionary<int, ConnectionGene>.ValueCollection.Enumerator enumerator_me = ConnectionGenes.Values.GetEnumerator();
            SortedDictionary<int, ConnectionGene>.ValueCollection.Enumerator enumerator_them = genome.ConnectionGenes.Values.GetEnumerator();

            enumerator_me.MoveNext();   //Preps for first current.
            enumerator_them.MoveNext();


            //Step through both genomes and find out how different they are.
            //This method is run a lot, so we want to do this by innovation number for efficiency.
            while (index_me < ConnectionGenes.Count && index_them < genome.ConnectionGenes.Count)
            {
                ConnectionGene connectionGene_me = enumerator_me.Current;
                ConnectionGene connectionGene_them = enumerator_them.Current;

                int inNum_me = connectionGene_me.InnovationNumber;
                int inNum_them = connectionGene_them.InnovationNumber;


                if (inNum_me == inNum_them) //Similar genes.
                {
                    index_me++;
                    index_them++;
                    enumerator_me.MoveNext();
                    enumerator_them.MoveNext();

                    num_similar++;
                    weight_diff += Math.Abs(connectionGene_me.Weight - connectionGene_them.Weight);
                }
                else if (inNum_me > inNum_them) //Disjoint gene at them, increase them.
                {
                    index_them++;
                    enumerator_them.MoveNext();

                    num_disjoint++;
                }
                else    //Disjoint gene at me, increase me.
                {
                    index_me++;
                    enumerator_me.MoveNext();

                    num_disjoint++;
                }
            }


            //Count excess genes.
            if (index_me < ConnectionGenes.Count)   //We have leftover genes, use our count.
            {
                num_excess = ConnectionGenes.Count - index_me;
            }
            else if (index_them < genome.ConnectionGenes.Count)  //They have leftover genes, use their count.
            {
                num_excess = genome.ConnectionGenes.Count - index_them;
            }
            //There is no else because if they have the same number of genes, there is no excess, so use the default 0.


            if (num_similar > 0)
            {
                //The weight_diff would be 0 if num_similar was too, so only do this if they aren't 0.
                weight_diff /= num_similar;
            }


            double N = Math.Max(ConnectionGenes.Count, genome.ConnectionGenes.Count);
            N = (N < 20) ? 1 : N;   //Only needed for large networks.


            return C1 * (num_excess / N) + C2 * (num_disjoint / N) + (C3 * weight_diff);
        }


        #region Crossover

        /// <summary>
        /// Crosses over this genome with the given genome.
        /// </summary>
        /// <param name="my_score">Score of this genome. See remarks.</param>
        /// <param name="genome">The genome to cross over with.</param>
        /// <param name="their_score">Score of the given genome. See remarks.</param>
        /// <param name="random">The random object for the created genome.</param>
        /// <returns>The crossed-over genome.</returns>
        /// <remarks>
        /// When the given scores are equal, use the following rules:
        /// <list type="bullet">
        /// <item>On similar connection genes: See <see cref="NEAT.Genetic.Genome.Uniform_Crossover"/>.</item>
        /// <item>On disjoint connection genes: Adds all to the created genome.</item>
        /// <item>On excess connection genes: Adds all to the created genome.</item>
        /// </list>
        /// 
        /// When the given scores are not equal, use the following rules:
        /// <list type="bullet">
        /// <item>On similar connection genes: See <see cref="NEAT.Genetic.Genome.Uniform_Crossover"/>.</item>
        /// <item>On disjoint connection genes: Adds genes from parent with higher score.</item>
        /// <item>On excess connection genes: Adds genes from parent with higher score.</item>
        /// </list>
        /// </remarks>
        public Genome Crossover(double my_score, Genome genome, double their_score, Random random)
        {
            Genome created_genome = new Genome(GeneTracker, random);


            #region ConnectionGenes

            SortedDictionary<int, ConnectionGene>.ValueCollection.Enumerator enumerator_me = ConnectionGenes.Values.GetEnumerator();
            SortedDictionary<int, ConnectionGene>.ValueCollection.Enumerator enumerator_them = genome.ConnectionGenes.Values.GetEnumerator();

            enumerator_me.MoveNext();   //Preps for first current.
            enumerator_them.MoveNext();


            //Step through both genomes and cross them over randomly.
            while (enumerator_me.Current != null && enumerator_them.Current != null)
            {
                ConnectionGene connectionGene_me = enumerator_me.Current;
                ConnectionGene connectionGene_them = enumerator_them.Current;

                int inNum_me = connectionGene_me.InnovationNumber;
                int inNum_them = connectionGene_them.InnovationNumber;


                if (inNum_me == inNum_them) //Similar genes, choose either side at random.
                {
                    enumerator_me.MoveNext();
                    enumerator_them.MoveNext();

                    if (Uniform_Crossover)
                    {
                        if (random.NextDouble() < .5)
                        {
                            created_genome.ConnectionGenes.Add(inNum_me, connectionGene_me);
                        }
                        else
                        {
                            created_genome.ConnectionGenes.Add(inNum_them, connectionGene_them);
                        }
                    }
                    else
                    {
                        ConnectionGene temp = new ConnectionGene(connectionGene_me.From, connectionGene_me.To,
                            (connectionGene_me.Weight + connectionGene_them.Weight) / 2);

                        created_genome.ConnectionGenes.Add(inNum_me, temp);
                    }
                }
                else if (inNum_me > inNum_them) //Disjoint gene at them, add this gene if allowed.
                {
                    enumerator_them.MoveNext();

                    if (Math.Abs(my_score - their_score) < Crossover_ScoreDelta || their_score > my_score)
                    {
                        created_genome.ConnectionGenes.Add(inNum_them, connectionGene_them);
                    }
                }
                else    //Disjoint gene at me, add this gene if allowed.
                {
                    enumerator_me.MoveNext();

                    if (Math.Abs(my_score - their_score) < Crossover_ScoreDelta || my_score > their_score)
                    {
                        created_genome.ConnectionGenes.Add(inNum_me, connectionGene_me);
                    }
                }
            }


            //Run through the excess connections and add them all if allowed.
            if (enumerator_me.Current != null)  //We have leftover genes, add ours if allowed.
            {
                if (Math.Abs(my_score - their_score) < Crossover_ScoreDelta || my_score > their_score)  //Check legality.
                {
                    do
                    {
                        created_genome.ConnectionGenes.Add(enumerator_me.Current.InnovationNumber, enumerator_me.Current);
                    }
                    while (enumerator_me.MoveNext());
                }
            }
            else if (enumerator_them.Current != null)  //They have leftover genes, add theirs if allowed.
            {
                if (Math.Abs(my_score - their_score) < Crossover_ScoreDelta || their_score > my_score)  //Check legality.
                {
                    do
                    {
                        created_genome.ConnectionGenes.Add(enumerator_them.Current.InnovationNumber, enumerator_them.Current);
                    }
                    while (enumerator_them.MoveNext());
                }
            }
            //There is no else because if they have the same number of genes, there is no excess, so don't do anything.

            #endregion ConnectionGenes


            #region NodeGenes

            foreach (ConnectionGene connectionGene in created_genome.ConnectionGenes.Values)
            {
                if (!created_genome.NodeGenes.ContainsKey(connectionGene.From.InnovationNumber))
                {
                    created_genome.NodeGenes.Add(connectionGene.From.InnovationNumber, connectionGene.From);
                }

                if (!created_genome.NodeGenes.ContainsKey(connectionGene.To.InnovationNumber))
                {
                    created_genome.NodeGenes.Add(connectionGene.To.InnovationNumber, connectionGene.To);
                }
            }

            #endregion NodeGenes


            return created_genome;
        }


        /// <summary>
        /// Crosses over this genome with the given genome, giving both the same score. Gives the created genome this genome's random.
        /// </summary>
        /// <param name="genome">The genome to cross over with.</param>
        /// <returns>The crossed-over genome.</returns>
        public Genome Crossover(Genome genome)
        {
            return Crossover(0, genome, 0, Random);
        }

        #endregion Crossover


        #region Mutate

        /// <summary>
        /// Mutates the genome by creating a new connection. 
        /// </summary>
        public void Mutate_Link()
        {
            //Get first node to connect. It is random.
            NodeGene nodeGene_a = NodeGenes.RandomValue().Take(1).ElementAt(0); //NodeGenes[Random.Next(NodeGenes.Count) + 1];  //Innovation numbers start at 1.


            IEnumerable<NodeGene> temp_subset = NodeGenes.Values.Where(a => a.X > nodeGene_a.X);

            NodeGene nodeGene_b = temp_subset.ElementAt(Random.Next(temp_subset.Count()));  //Get a random gene with a higher X value.


            ConnectionGene connectionGene = GeneTracker.GetCreate_ConnectionGene(nodeGene_a, nodeGene_b, Mutation_WeightRandom * (Random.NextDouble() * 2 - 1));

            if (ConnectionGenes.ContainsKey(connectionGene.InnovationNumber))   //Can only happen if it already existed in the tracker.
            {
                return; //TODO think of how to handle this, maybe have a retry somewhere?
            }


            ConnectionGenes.Add(connectionGene.InnovationNumber, connectionGene);
        }


        ///// <summary>
        ///// Mutates a random connection splitting it with a node.
        ///// </summary>
        //public void Mutate_Node()
        //{
        //    ConnectionGene connectionGene = ConnectionGenes.RandomValue().Take(1).ElementAt(0);

        //    if (connectionGene == null)
        //    {
        //        return;
        //    }


        //    NodeGene from = connectionGene.From;
        //    NodeGene to = connectionGene.To;

        //    NodeGene created = NEAT.CreateNode();

        //    created.X = (from.X + to.X) / 2;
        //    created.Y = (from.Y + to.Y) / 2 + random.NextDouble() * 0.1 + .05;

        //    Nodes.Add(created);


        //    ConnectionGene created_connectionGene_1 = NEAT.CreateConnection(from, created);
        //    ConnectionGene created_connectionGene_2 = NEAT.CreateConnection(created, to);

        //    Connections.Remove(connectionGene);

        //    Connections.Add(created_connectionGene_1);
        //    Connections.Add(created_connectionGene_2);


        //    created_connectionGene_1.Weight = 1;                        //Default weight.
        //    created_connectionGene_2.Weight = connectionGene.Weight;    //Old connection's weight.

        //    created_connectionGene_2.Enabled = connectionGene.Enabled;  //Old enabled state.
        //}


        /// <summary>
        /// Mutates a random connection by shifting its weight up or down by a radom value.
        /// </summary>
        public void Mutate_WeightShift()
        {
            ConnectionGene connectionGene = ConnectionGenes.RandomValue().Take(1).ElementAt(0); //ConnectionGenes[Random.Next(ConnectionGenes.Count) + 1];

            if (connectionGene != null)
            {
                connectionGene.Weight = connectionGene.Weight + Mutation_WeightShift * (Random.NextDouble() * 2 - 1);
            }
        }


        /// <summary>
        /// Mutates a random connection by radomizing its weight.
        /// </summary>
        public void Mutate_WeightRandom()
        {
            ConnectionGene connectionGene = ConnectionGenes.RandomValue().Take(1).ElementAt(0); //ConnectionGenes[Random.Next(ConnectionGenes.Count) + 1];

            if (connectionGene != null)
            {
                connectionGene.Weight = Mutation_WeightRandom * (Random.NextDouble() * 2 - 1);
            }
        }


        /// <summary>
        /// Mutates a random connection by inverting its current Enabled status.
        /// </summary>
        public void Mutate_LinkToggle()
        {
            ConnectionGene connectionGene = ConnectionGenes.RandomValue().Take(1).ElementAt(0); //ConnectionGenes[Random.Next(ConnectionGenes.Count) + 1];

            if (connectionGene != null)
            {
                connectionGene.Enabled = !connectionGene.Enabled;
            }
        }

        #endregion Mutate
    }
}
