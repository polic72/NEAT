using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NEAT.Genetic;
using NEAT.Neural_Network;

namespace NEAT.Speciation
{
    /// <summary>
    /// 
    /// </summary>
    public class Organism
    {
        #region Properties

        /// <summary>
        /// The genome of this organism.
        /// </summary>
        public Genome Genome { get; }

        /// <summary>
        /// The neural network used to calculate output from this organism.
        /// </summary>
        /// <remarks>
        /// The state of this property is not synced with the genome automatically. Whenever the genome changes (through mutation), the 
        /// <see cref="NEAT.Speciation.Organism.UpdateNeuralNetwork"/> method needs to be run.
        /// </remarks>
        public NeuralNetwork NeuralNetwork { get; internal set; }


        /// <summary>
        /// The fitness score of this organism.
        /// </summary>
        public double Score { get; set; }

        /// <summary>
        /// The species this organism belongs to.
        /// </summary>
        public Species Species { get; set; }

        #endregion Properties


        /// <summary>
        /// Constructs an organism with the given genome that it contains.
        /// </summary>
        /// <param name="genome">The genome of the organism.</param>
        public Organism(Genome genome)
        {
            Genome = genome;

            NeuralNetwork = new NeuralNetwork(Genome);
        }


        /// <summary>
        /// Updates the internal neural network to the current state of the genome.
        /// </summary>
        public void UpdateNeuralNetwork()
        {
            NeuralNetwork = new NeuralNetwork(Genome);
        }
    }
}
