using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEAT.Genetic
{
    public class Genome
    {
        #region "Constants"

        /// <summary>
        /// The maximum number of nodes that any neural network can have.
        /// <para/>
        /// Set in the <see cref="NEAT.Genome.Init(int)"/> method.
        /// </summary>
        public static int MaxNodes { get; private set; }


        #region Initialization

        private static bool initialized = false;

        /// <summary>
        /// Initializes the EvolvingNN class with the given max_nodes.
        /// </summary>
        /// <param name="max_nodes">The maximum number of nodes a neural network can have.</param>
        public static void Init(int max_nodes)
        {
            if (initialized)
            {
                throw new InvalidOperationException("The Genome class is already initialized.");
            }


            MaxNodes = max_nodes;


            initialized = true;
        }

        #endregion Initialization

        #endregion "Constants"
    }
}
