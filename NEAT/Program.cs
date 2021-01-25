using System;
using System.Collections.Generic;
//using System.Linq;
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
            SortedSet<NodeGene> set = new SortedSet<NodeGene>();

            int num = 1;


            NodeGene nodeGene_1 = new NodeGene(num++, Node.INPUT_X, Node.Sigmoid);
            NodeGene nodeGene_2 = new NodeGene(num++, Node.INPUT_X, Node.Sigmoid);

            ConnectionGene connectionGene_1 = new ConnectionGene(nodeGene_1, nodeGene_2, 4);


            NodeGene nodeGene_3 = new NodeGene(num++, Node.INPUT_X, Node.Sigmoid);
            NodeGene nodeGene_4 = new NodeGene(num++, Node.INPUT_X, Node.Sigmoid);

            ConnectionGene connectionGene_2 = new ConnectionGene(nodeGene_1, nodeGene_2, 4);


            set.Add(nodeGene_1);
            set.Add(nodeGene_2);
            set.Add(nodeGene_3);
            set.Add(nodeGene_4);


            Random random = new Random();

            NodeGene wejf = null;

            set.TryGetValue(new NodeGene() { InnovationNumber = random.Next(set.Count) + 1}, out wejf);


            Console.WriteLine();

            Console.ReadKey();
        }
    }
}
