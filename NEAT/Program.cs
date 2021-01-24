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

            Random random = new Random();


            int nodeGene_num = 1;

            NodeGene nodeGene_1 = new NodeGene(nodeGene_num++, Node.Sigmoid);
            NodeGene nodeGene_2 = new NodeGene(nodeGene_num++, Node.Sigmoid);

            NodeGene nodeGene_3 = new NodeGene(nodeGene_num++, Node.Sigmoid);

            ConnectionGene connectionGene_1 = new ConnectionGene(nodeGene_1, nodeGene_2, 1);
            ConnectionGene connectionGene_2 = new ConnectionGene(nodeGene_1, nodeGene_3, 1);


            Genome genome_1 = new Genome(random);
            genome_1.NodeGenes.Add(nodeGene_1);
            genome_1.NodeGenes.Add(nodeGene_2);

            genome_1.ConnectionGenes.Add(connectionGene_1);


            Genome genome_2 = new Genome(random);
            genome_2.NodeGenes.Add(nodeGene_1);
            genome_2.NodeGenes.Add(nodeGene_3);

            genome_2.ConnectionGenes.Add(connectionGene_2);


            Genome created_genome = genome_1.Crossover(genome_2);


            Console.WriteLine((nodeGene_1 == nodeGene_2).ToString());

            Console.ReadKey();
        }
    }
}
