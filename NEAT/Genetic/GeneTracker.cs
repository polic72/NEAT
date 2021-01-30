using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEAT.Genetic
{
    public static class GeneTracker
    {
        private static Dictionary<int, NodeGene> nodeGenes;
        private static Dictionary<int, ConnectionGene> connectionGenes;


        static GeneTracker()
        {
            nodeGenes = new Dictionary<int, NodeGene>();
            connectionGenes = new Dictionary<int, ConnectionGene>();
        }


        #region NodeGene

        /// <summary>
        /// Adds the given node gene if not already there.
        /// </summary>
        /// <param name="nodeGene">The node gene to add.</param>
        /// <returns>True if added. False if already contained.</returns>
        public static bool AddNodeGene(NodeGene nodeGene)
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
        public static NodeGene GetNodeGene(int innovation_number)
        {
            if (nodeGenes.ContainsKey(innovation_number))
            {
                return nodeGenes[innovation_number];
            }

            return null;
        }

        #endregion NodeGene
    }
}
