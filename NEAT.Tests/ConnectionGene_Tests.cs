using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

using NEAT.Genetic;

namespace NEAT.Tests
{
    [TestClass]
    public class ConnectionGene_Tests
    {
        [TestMethod]
        public void Equals_ReturnsTrue()
        {
            if (!Genome.IsInitialized())
            {
                Genome.Init();
            }


            NodeGene from = new NodeGene(1, Neural_Network.Node.Sigmoid);

            NodeGene to = new NodeGene(2, Neural_Network.Node.Sigmoid);


            ConnectionGene connectionGene_1 = new ConnectionGene(from, to, 1);

            ConnectionGene connectionGene_2 = new ConnectionGene(from, to, 1);


            Assert.IsTrue(connectionGene_1 == connectionGene_2);
        }
    }
}
