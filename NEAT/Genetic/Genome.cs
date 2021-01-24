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
    public class Genome
    {
        #region "Constants"

        /// <summary>
        /// The maximum number of nodes that any neural network can have.
        /// <para/>
        /// Set in the <see cref="NEAT.Genetic.Genome.Init(int)"/> method.
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


        #region Initialization

        private static bool initialized = false;

        /// <summary>
        /// Initializes the EvolvingNN class with the given max_nodes.
        /// </summary>
        /// <param name="max_nodes">The maximum number of nodes a neural network can have.</param>
        /// <param name="c1">The c1 constant to set. See <see cref="NEAT.Genetic.Genome.Distance(Genome)"/> for more 
        /// information.</param>
        /// <param name="c2">The c2 constant to set. See <see cref="NEAT.Genetic.Genome.Distance(Genome)"/> for more 
        /// information.</param>
        /// <param name="c3">The c3 constant to set. See <see cref="NEAT.Genetic.Genome.Distance(Genome)"/> for more 
        /// information.</param>
        public static void Init(int max_nodes, double c1, double c2, double c3)
        {
            if (initialized)
            {
                throw new InvalidOperationException("The Genome class is already initialized.");
            }


            MaxNodes = max_nodes;

            C1 = c1;
            C2 = c2;
            C3 = c3;


            initialized = true;
        }


        /// <summary>
        /// Initializes the EvolvingNN class with 2^20 max_nodes, 1.0 c1, 1.0 c2, and 0.4 c3.
        /// TODO update as needed
        /// </summary>
        public static void Init()
        {
            Init((int)Math.Pow(2, 20), 1, 1, .4);
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
        /// The internal random of the genome.
        /// </summary>
        public Random Random { get; }


        /// <summary>
        /// The ConnectionGenes of this genome.
        /// </summary>
        public List<ConnectionGene> ConnectionGenes { get; }

        /// <summary>
        /// The NodeGenes of this genome.
        /// </summary>
        public List<NodeGene> NodeGenes { get; }

        #endregion Properties


        /// <summary>
        /// Constructs a genome with the given random.
        /// </summary>
        /// <param name="random">The internal random for the genome.</param>
        public Genome(Random random)
        {
            Random = random;

            ConnectionGenes = new List<ConnectionGene>();
            NodeGenes = new List<NodeGene>();
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
            int index_me = 0;
            int index_them = 0;


            int num_excess = 0;     //The number of excess genes.
            int num_disjoint = 0;   //The number of disjoint genes.

            double num_similar = 0; //The number of genes that are similar.
            double weight_diff = 0; //The weight difference between similar genes.


            //Step through both genomes and find out how different they are.
            //This method is run a lot, so we want to do this by innovation number for efficiency.
            while (index_me < ConnectionGenes.Count && index_them < genome.ConnectionGenes.Count)
            {
                ConnectionGene connectionGene_me = ConnectionGenes[index_me];
                ConnectionGene connectionGene_them = genome.ConnectionGenes[index_them];

                int inNum_me = connectionGene_me.InnovationNumber;
                int inNum_them = connectionGene_them.InnovationNumber;


                if (inNum_me == inNum_them) //Similar genes.
                {
                    index_me++;
                    index_them++;

                    num_similar++;
                    weight_diff += Math.Abs(connectionGene_me.Weight - connectionGene_them.Weight);
                }
                else if (inNum_me > inNum_them) //Disjoint gene at them, increase them.
                {
                    index_them++;

                    num_disjoint++;
                }
                else    //Disjoint gene at me, increase me.
                {
                    index_me++;

                    num_disjoint++;
                }
            }


            //Count excess genes.
            if (ConnectionGenes.Count > genome.ConnectionGenes.Count)   //We has more genes, use our count.
            {
                num_excess = ConnectionGenes.Count - index_me;
            }
            else if (ConnectionGenes.Count < genome.ConnectionGenes.Count)  //They have more genes, use their count.
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
    }
}
