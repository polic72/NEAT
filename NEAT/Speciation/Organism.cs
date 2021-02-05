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
    /// An individual organism that has its own genome, has a fitness score, and belongs to a species.
    /// </summary>
    public class Organism : IEquatable<Organism>, IComparable<Organism>
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
        public double FitnessScore { get; set; }

        /// <summary>
        /// The species this organism belongs to.
        /// </summary>
        public Species Species { get; set; }

        #endregion Properties


        /// <summary>
        /// Constructs an organism with the given genome that it contains.
        /// </summary>
        /// <param name="genome">The genome of the organism.</param>
        /// <exception cref="ArgumentNullException">When the genome is null.</exception>
        public Organism(Genome genome)
        {
            Helpers.ThrowOnNull(genome, "genome");


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


        #region Overrides and Operators

        /// <summary>
        /// Whether or not this organism is equal to the given organism.
        /// </summary>
        /// <param name="other">The other organism to test.</param>
        /// <returns>True if both genomes are equal. False otherwise.</returns>
        public bool Equals(Organism other)
        {
            return Genome.Equals(other.Genome);
        }


        /// <summary>
        /// Whether or not this organism is equal to the given object.
        /// </summary>
        /// <param name="obj">The object to test.</param>
        /// <returns>True if obj is an Organism object and their genomes are equal. False otherwise.</returns>
        public override bool Equals(object obj)
        {
            if (obj is Organism organism)
            {
                return Equals(organism);
            }

            return false;
        }


        /// <summary>
        /// Gets the hash code of this organism. Is the genome hash code as well.
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode()
        {
            return Genome.GetHashCode();
        }


        /// <summary>
        /// Compares this organism to the given organism. Compares based on fitness score.
        /// </summary>
        /// <param name="other">The organism to compare to.</param>
        /// <returns>This organism's fitness score - the given organism's fitness score. Rounds up if positive, down if negative. Then negates value.</returns>
        public int CompareTo(Organism other)
        {
            double difference = FitnessScore - other.FitnessScore;

            return -(int)(Math.Sign(difference) * Math.Ceiling(Math.Abs(difference)));
        }




        /// <summary>
        /// Whether or not the left organism is equal to the right one.
        /// </summary>
        /// <param name="left">The left organism to test.</param>
        /// <param name="right">The right organism to test.</param>
        /// <returns>True if both organisms have the same genome. False otherwise.</returns>
        public static bool operator ==(Organism left, Organism right)
        {
            if (left is null)
            {
                if (right is null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (right == null)
                {
                    return false;
                }
                else
                {
                    return left.Equals(right);
                }
            }
        }


        /// <summary>
        /// Whether or not the left organism is not equal to the right one.
        /// </summary>
        /// <param name="left">The left organism to test.</param>
        /// <param name="right">The right organism to test.</param>
        /// <returns>False if both organisms have the same genome. True otherwise.</returns>
        public static bool operator !=(Organism left, Organism right)
        {
            if (left is null)
            {
                if (right is null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                if (right == null)
                {
                    return true;
                }
                else
                {
                    return !left.Equals(right);
                }
            }
        }

        #endregion Overrides and Operators
    }
}
