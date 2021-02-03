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

            Genome genome = pedigree.CreateGenome();

            genome.Mutate_Link();
            genome.Mutate_Link();

            genome.Mutate_Node();


            Console.WriteLine();

			Console.ReadKey();
        }
    }
}
