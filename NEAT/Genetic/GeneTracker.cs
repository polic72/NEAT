using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEAT.Genetic
{
    /// <summary>
    /// A class that is used to track genes globally. Used for optimization.
    /// </summary>
    public class GeneTracker
    {
        private readonly Dictionary<int, NodeGene> nodeGenes;
        private readonly Dictionary<int, ConnectionGene> connectionGenes;


        private static int max_replacingNumber;


        /// <summary>
        /// Constructs a GeneTracker with the given initial max replacing number.
        /// </summary>
        /// <param name="initial_max_replacingNumber">Should be #input_nodes + #output_nodes + 1.</param>
        public GeneTracker(int initial_max_replacingNumber)
        {
            nodeGenes = new Dictionary<int, NodeGene>();
            connectionGenes = new Dictionary<int, ConnectionGene>();

            max_replacingNumber = initial_max_replacingNumber;
        }


        #region NodeGene

        /// <summary>
        /// Adds the given node gene if not already there.
        /// </summary>
        /// <param name="nodeGene">The node gene to add.</param>
        /// <returns>True if added. False if already contained.</returns>
        public bool AddNodeGene(NodeGene nodeGene)
        {
            if (nodeGenes.ContainsKey(nodeGene.InnovationNumber))
            {
                return false;
            }


            nodeGenes.Add(nodeGene.InnovationNumber, nodeGene);

            return true;
        }


        /// <summary>
        /// Gets the node gene specified by the given innovation number.
        /// </summary>
        /// <param name="innovation_number">The innovation number of the node to get.</param>
        /// <returns>The node gene with the given innovation number. Null if it was never added.</returns>
        public NodeGene GetNodeGene(int innovation_number)
        {
            if (nodeGenes.ContainsKey(innovation_number))
            {
                return nodeGenes[innovation_number];
            }

            return null;
        }

        #endregion NodeGene


        #region ConnectionGene

        /// <summary>
        /// Adds the given connection gene if not already there.
        /// </summary>
        /// <param name="connectionGene">The connection gene to add.</param>
        /// <returns>True if added. False if already contained.</returns>
        public bool AddConnectionGene(ConnectionGene connectionGene)
        {
            if (connectionGenes.ContainsKey(connectionGene.InnovationNumber))
            {
                return false;
            }


            connectionGenes.Add(connectionGene.InnovationNumber, connectionGene);

            return true;
        }


        /// <summary>
        /// Gets the connection gene specified by the given innovation number.
        /// </summary>
        /// <param name="innovation_number">The innovation number of the connection to get.</param>
        /// <returns>The connection gene with the given innovation number. Null if it was never added.</returns>
        public ConnectionGene GetConnectionGene(int innovation_number)
        {
            if (connectionGenes.ContainsKey(innovation_number))
            {
                return connectionGenes[innovation_number];
            }

            return null;
        }


        /// <summary>
        /// Gets the connection gene if it already exists (the given weight is ignored). Creates a new one otherwise (adds to tracker).
        /// </summary>
        /// <param name="from">The from node gene of the connection gene.</param>
        /// <param name="to">The to node gene of the connection gene.</param>
        /// <param name="weight">The weight of the connection gene. Only used if created.</param>
        /// <returns>The retrieved or created connection gene.</returns>
        public ConnectionGene GetCreate_ConnectionGene(NodeGene from, NodeGene to, double weight)
        {
            int innovation_number = ConnectionGene.GetHashCode(from, to);

            if (connectionGenes.ContainsKey(innovation_number))
            {
                return connectionGenes[innovation_number];
            }


            ConnectionGene created_connectionGene = new ConnectionGene(from, to, weight, max_replacingNumber++);

            connectionGenes.Add(innovation_number, created_connectionGene);

            return created_connectionGene;
        }

        #endregion ConnectionGene
    }
}
