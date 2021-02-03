using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NEAT.Neural_Network;
using NEAT.Genetic;
using NEAT.Genetic.Tracker;

namespace NEAT
{
    class Program
    {
        public static void Main(string[] args)
        {
            Pedigree pedigree = new Pedigree(4, 1, new Random(42));

            Genome genome_1 = pedigree.CreateGenome();

            genome_1.Mutate_Link();
            genome_1.Mutate_Link();

            genome_1.Mutate_Node();


            Genome genome_2 = pedigree.CreateGenome();

            genome_2.Mutate_Link();
            genome_2.Mutate_Link();

            genome_2.Mutate_Node();


            Genome genome_3 = genome_1.Crossover(genome_2);


            Console.WriteLine(genome_1.Distance(genome_2));

			Console.ReadKey();
        }
    }
}
