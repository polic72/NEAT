using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NEAT.Genetic.Tracker;

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
        #region Properties

        /// <summary>
        /// The gene tracker associated with this genome. All genes in this genome are in this gene tracker.
        /// </summary>
        public Pedigree Pedigree { get; }

        /// <summary>
        /// The internal random of the genome.
        /// </summary>
        public Random Random { get; }


        /// <summary>
        /// The ConnectionGenes of this genome.
        /// </summary>
        protected internal SortedDictionary<int, ConnectionGene> ConnectionGenes { get; }

        /// <summary>
        /// The NodeGenes of this genome.
        /// </summary>
        protected internal SortedDictionary<int, NodeGene> NodeGenes { get; }

        #endregion Properties


        /// <summary>
        /// Constructs a genome with the given pedigree and random.
        /// </summary>
        /// <param name="pedigree">The pedigree for the genome.</param>
        /// <param name="random">The internal random for the genome.</param>
        /// <exception cref="ArgumentNullException">When either parameter is null.</exception>
        protected internal Genome(Pedigree pedigree, Random random)
        {
            Helpers.ThrowOnNull(pedigree, "genePatternTracker");
            Helpers.ThrowOnNull(random, "random");


            Pedigree = pedigree;
            Random = random;

            ConnectionGenes = new SortedDictionary<int, ConnectionGene>();
            NodeGenes = new SortedDictionary<int, NodeGene>();
        }


        /// <summary>
        /// Gets the distance between this genome and the given genome. Higher = less compatible.
        /// </summary>
        /// <param name="genome">The genome to compare to.</param>
        /// <returns>The distance between this genome and the given genome.</returns>
        /// <remarks>
        /// The distance d can be measured by the following equation:
        /// <para/>
        /// d = c1(E / N) + c2(D / N) + c3 * W
        /// <para/>
        /// Where:
        ///     d = distance |
        ///     E = # excess genes |
        ///     D = # of disjoint genes |
        ///     W = weight difference of similar genes |
        ///     N = # of genes in largest genome (this or them), 1 if #genes &lt; 20 |
        ///     c_ = constant for adjusting
        /// </remarks>
        /// <exception cref="ArgumentNullException">When the given genome is null.</exception>
        public double Distance(Genome genome)
        {
            Helpers.ThrowOnNull(genome, "genome");


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

                int inNum_me = connectionGene_me.ConnectionGenePattern.InnovationNumber;
                int inNum_them = connectionGene_them.ConnectionGenePattern.InnovationNumber;


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


            return Pedigree.C1 * (num_excess / N) + Pedigree.C2 * (num_disjoint / N) + (Pedigree.C3 * weight_diff);
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
        /// <exception cref="ArgumentNullException">When the genome or random is null.</exception>
        public Genome Crossover(double my_score, Genome genome, double their_score, Random random)
        {
            Helpers.ThrowOnNull(genome, "genome");
            Helpers.ThrowOnNull(random, "random");


            Genome created_genome = new Genome(Pedigree, random);


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

                int inNum_me = connectionGene_me.ConnectionGenePattern.InnovationNumber;
                int inNum_them = connectionGene_them.ConnectionGenePattern.InnovationNumber;


                if (inNum_me == inNum_them) //Similar genes, choose either side at random.
                {
                    enumerator_me.MoveNext();
                    enumerator_them.MoveNext();

                    if (Pedigree.Uniform_Crossover)
                    {
                        if (random.NextDouble() < .5)
                        {
                            created_genome.ConnectionGenes.Add(inNum_me, Pedigree.Copy_ConnectionGene(connectionGene_me));
                        }
                        else
                        {
                            created_genome.ConnectionGenes.Add(inNum_them, Pedigree.Copy_ConnectionGene(connectionGene_them));
                        }
                    }
                    else
                    {
                        created_genome.ConnectionGenes.Add(inNum_me, Pedigree.Create_ConnectionGene(connectionGene_me.ConnectionGenePattern,
                            (connectionGene_me.Weight + connectionGene_them.Weight) / 2,
                            (random.NextDouble() < .5) ? connectionGene_me.Enabled : connectionGene_them.Enabled));
                    }
                }
                else if (inNum_me > inNum_them) //Disjoint gene at them, add this gene if allowed.
                {
                    enumerator_them.MoveNext();

                    if (Math.Abs(my_score - their_score) < Pedigree.Crossover_ScoreDelta || their_score > my_score)
                    {
                        created_genome.ConnectionGenes.Add(inNum_them, Pedigree.Copy_ConnectionGene(connectionGene_them));
                    }
                }
                else    //Disjoint gene at me, add this gene if allowed.
                {
                    enumerator_me.MoveNext();

                    if (Math.Abs(my_score - their_score) < Pedigree.Crossover_ScoreDelta || my_score > their_score)
                    {
                        created_genome.ConnectionGenes.Add(inNum_me, Pedigree.Copy_ConnectionGene(connectionGene_me));
                    }
                }
            }


            //Run through the excess connections and add them all if allowed.
            if (enumerator_me.Current != null)  //We have leftover genes, add ours if allowed.
            {
                if (Math.Abs(my_score - their_score) < Pedigree.Crossover_ScoreDelta || my_score > their_score)  //Check legality.
                {
                    do
                    {
                        created_genome.ConnectionGenes.Add(enumerator_me.Current.ConnectionGenePattern.InnovationNumber, 
                            Pedigree.Copy_ConnectionGene(enumerator_me.Current));
                    }
                    while (enumerator_me.MoveNext());
                }
            }
            else if (enumerator_them.Current != null)  //They have leftover genes, add theirs if allowed.
            {
                if (Math.Abs(my_score - their_score) < Pedigree.Crossover_ScoreDelta || their_score > my_score)  //Check legality.
                {
                    do
                    {
                        created_genome.ConnectionGenes.Add(enumerator_them.Current.ConnectionGenePattern.InnovationNumber,
                            Pedigree.Copy_ConnectionGene(enumerator_them.Current));
                    }
                    while (enumerator_them.MoveNext());
                }
            }
            //There is no else because if they have the same number of genes, there is no excess, so don't do anything.

            #endregion ConnectionGenes


            #region NodeGenes

            foreach (ConnectionGene connectionGene in created_genome.ConnectionGenes.Values)
            {
                ConnectionGenePattern pattern = connectionGene.ConnectionGenePattern;

                if (!created_genome.NodeGenes.ContainsKey(pattern.From.InnovationNumber))
                {
                    created_genome.NodeGenes.Add(pattern.From.InnovationNumber, Pedigree.Copy_NodeGene(NodeGenes[pattern.From.InnovationNumber]));
                }

                if (!created_genome.NodeGenes.ContainsKey(pattern.To.InnovationNumber))
                {
                    created_genome.NodeGenes.Add(pattern.To.InnovationNumber, Pedigree.Copy_NodeGene(NodeGenes[pattern.To.InnovationNumber]));
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
        /// <exception cref="ArgumentNullException">When the given genome is null.</exception>
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
            NodeGene nodeGene_a = NodeGenes.RandomValue().Take(1).ElementAt(0);


            IEnumerable<NodeGene> temp_subset = NodeGenes.Values.Where(a => a.NodeGenePattern.X > nodeGene_a.NodeGenePattern.X);

            NodeGene nodeGene_b = temp_subset.ElementAt(Random.Next(temp_subset.Count()));  //Get a random gene with a higher X value.


            ConnectionGene connectionGene = Pedigree.Create_ConnectionGene(nodeGene_a, nodeGene_b, Pedigree.Mutation_WeightRandom * (Random.NextDouble() * 2 - 1), 
                true);

            if (ConnectionGenes.ContainsKey(connectionGene.ConnectionGenePattern.InnovationNumber))   //Can only happen if it already existed in the tracker.
            {
                return; //TODO think of how to handle this, maybe have a retry somewhere?
            }


            ConnectionGenes.Add(connectionGene.ConnectionGenePattern.InnovationNumber, connectionGene);
        }


        /// <summary>
        /// Mutates a random connection splitting it with a node.
        /// </summary>
        public void Mutate_Node()
        {
            ConnectionGene connectionGene = ConnectionGenes.RandomValue().Take(1).ElementAt(0);

            if (connectionGene == null)
            {
                return;
            }


            NodeGene from = NodeGenes[connectionGene.ConnectionGenePattern.From.InnovationNumber];
            NodeGene to = NodeGenes[connectionGene.ConnectionGenePattern.To.InnovationNumber];

            NodeGene created = Pedigree.Create_NodeGene(connectionGene);

            NodeGenes.Add(created.NodeGenePattern.InnovationNumber, created);


            ConnectionGene created_connectionGene_1 = Pedigree.Create_ConnectionGene(from, created, 1, true); //Default weight of 1.
            ConnectionGene created_connectionGene_2 = Pedigree.Create_ConnectionGene(created, to, connectionGene.Weight, connectionGene.Enabled);

            ConnectionGenes.Remove(connectionGene.ConnectionGenePattern.InnovationNumber);

            ConnectionGenes.Add(created_connectionGene_1.ConnectionGenePattern.InnovationNumber, created_connectionGene_1);
            ConnectionGenes.Add(created_connectionGene_2.ConnectionGenePattern.InnovationNumber, created_connectionGene_2);
        }


        /// <summary>
        /// Mutates a random node by giving it a random activation function other than the one it has right now.
        /// </summary>
        public void Mutate_ActivationFunction()
        {
            NodeGene nodeGene = NodeGenes.RandomValue().Take(1).ElementAt(0);

            nodeGene.ActivationFunction = Pedigree.GetRandomActivationFunction(nodeGene.ActivationFunction);
        }


        /// <summary>
        /// Mutates a random connection by shifting its weight up or down by a radom value.
        /// </summary>
        public void Mutate_WeightShift()
        {
            ConnectionGene connectionGene = ConnectionGenes.RandomValue().Take(1).ElementAt(0);

            if (connectionGene != null)
            {
                connectionGene.Weight = connectionGene.Weight + Pedigree.Mutation_WeightShift * (Random.NextDouble() * 2 - 1);
            }
        }


        /// <summary>
        /// Mutates a random connection by radomizing its weight.
        /// </summary>
        public void Mutate_WeightRandom()
        {
            ConnectionGene connectionGene = ConnectionGenes.RandomValue().Take(1).ElementAt(0);

            if (connectionGene != null)
            {
                connectionGene.Weight = Pedigree.Mutation_WeightRandom * (Random.NextDouble() * 2 - 1);
            }
        }


        /// <summary>
        /// Mutates a random connection by inverting its current Enabled status.
        /// </summary>
        public void Mutate_LinkToggle()
        {
            ConnectionGene connectionGene = ConnectionGenes.RandomValue().Take(1).ElementAt(0);

            if (connectionGene != null)
            {
                connectionGene.Enabled = !connectionGene.Enabled;
            }
        }

        #endregion Mutate
    }
}
