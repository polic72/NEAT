using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NEAT.Neural_Network;
using NEAT.Genetic;
using NEAT.Genetic.Tracker;
using NEAT.Speciation;

namespace NEAT
{
    class Program
    {
        public static void Main(string[] args)
        {
            #region Pedigree Test

            //Pedigree pedigree = new Pedigree(4, 1, new Random(42));

            //Genome genome_1 = pedigree.CreateGenome();

            //genome_1.Mutate_Link();
            //genome_1.Mutate_Link();

            //genome_1.Mutate_Node();


            //Genome genome_2 = pedigree.CreateGenome();

            //genome_2.Mutate_Link();
            //genome_2.Mutate_Link();

            //genome_2.Mutate_Node();


            //Genome genome_3 = genome_1.Crossover(genome_2);


            //NeuralNetwork neuralNetwork = new NeuralNetwork(genome_3);

            //double[] output = neuralNetwork.FeedForward(new double[] { .6, .7, .5, .5 });

            //Console.WriteLine(genome_1.Distance(genome_2));

            #endregion Pedigree Test


            #region NEATClient Test

            Pedigree pedigree = new Pedigree(2, 1, new Random(42));

            NEATClient client = new NEATClient(pedigree, 50, Evaluate);


            for (int i = 0; i < 100; ++i)
            {
                client.Speciate();

                client.EvaluateScores();

                client.Kill();

                client.RemoveExtinctions();

                client.ReproduceAndReplace();

                client.Mutate();
            }


            NeuralNetwork fittest = client.GetMostFitOrganism();


            for (int i = 0; i < 2; ++i)
            {
                for (int q = 0; q < 2; ++q)
                {
                    double[] input = { i, q };


                    Console.WriteLine(i + ", " + q + ": " + fittest.FeedForward(input)[0]);
                }
            }


            Console.WriteLine();

            #endregion NEATClient Test


            Console.ReadKey();
        }


        public static double Evaluate(NeuralNetwork neuralNetwork)
        {
            double score = 0;

            for (int i = 0; i < 2; ++i)
            {
                for (int q = 0; q < 2; ++q)
                {
                    double[] input = { i, q };

                    double desired_output = (i == q) ? 0 : 1;


                    score += Scoring(desired_output, neuralNetwork.FeedForward(input)[0]);
                }
            }


            return score;
        }


        public static double Scoring(double desired, double actual)
        {
            return 2 * Math.Pow(Math.Abs(desired - actual), 2);
        }
    }
}
