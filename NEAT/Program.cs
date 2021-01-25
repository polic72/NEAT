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

            Random random = new Random(1);


            int nodeGene_num = 1;

            NodeGene nodeGene_1 = new NodeGene(nodeGene_num++, Node.Sigmoid);
            NodeGene nodeGene_2 = new NodeGene(nodeGene_num++, Node.Sigmoid); Console.WriteLine(nodeGene_1 == null);

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


            bool[] contains_genes = { false, false, false, false, false };

            foreach (ConnectionGene cgene in created_genome.ConnectionGenes)
            {
                if (cgene == connectionGene_1)
                {
                    contains_genes[0] = true;
                }
                else if (cgene == connectionGene_2)
                {
                    contains_genes[1] = true;
                }
            }

            foreach (NodeGene ngene in created_genome.NodeGenes)
            {
                if (ngene == nodeGene_1)
                {
                    contains_genes[2] = true;
                }
                else if (ngene == nodeGene_2)
                {
                    contains_genes[3] = true;
                }
                else if (ngene == nodeGene_3)
                {
                    contains_genes[4] = true;
                }
            }


            Console.WriteLine();

            Console.ReadKey();
        }
    }
}
