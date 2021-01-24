using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NEAT.Neural_Network;
using NEAT.Genetic;

namespace NEAT
{
    class Program
    {
        public static void Main(string[] args)
        {
            if (!Genome.IsInitialized())
            {
                Genome.Init();
            }


            NodeGene nodeGene_1 = new NodeGene(1, Neural_Network.Node.Sigmoid);

            NodeGene nodeGene_2 = new NodeGene(1, Neural_Network.Node.Sigmoid);


            Console.WriteLine((nodeGene_1 == nodeGene_2).ToString());

            Console.ReadKey();
        }
    }
}
