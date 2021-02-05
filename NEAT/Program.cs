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

            Pedigree pedigree = new Pedigree(4, 1, new Random(42));

            NEATClient client = new NEATClient(pedigree, 2);

            IEnumerable<Organism> oh = client.Organisms.Where(x => x != client.Organisms[0]);

            client.Organisms[0].Genome.Mutate_Link();
            client.Organisms[0].Genome.Mutate_Link();

            client.Organisms[0].Genome.Mutate_Node();
            client.Organisms[0].Genome.Mutate_Node();
            client.Organisms[0].Genome.Mutate_Node();

            //client.Speciate();


            Console.WriteLine(client.Organisms[0].Genome.Distance(client.Organisms[1].Genome));

            #endregion NEATClient Test


            Console.ReadKey();
        }
    }
}
