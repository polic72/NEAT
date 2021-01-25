using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

using NEAT.Genetic;
using NEAT.Neural_Network;

namespace NEAT.Tests
{
    [TestClass]
    public class NodeGene_Tests
    {
        [TestMethod]
        public void Equals_ReturnsTrue()
        {
            if (!Genome.IsInitialized())
            {
                Genome.Init();
            }


            NodeGene nodeGene_1 = new NodeGene(1, Node.INPUT_X, Neural_Network.Node.Sigmoid);

            NodeGene nodeGene_2 = new NodeGene(1, Node.INPUT_X, Neural_Network.Node.Sigmoid);


            Assert.IsTrue(nodeGene_1 == nodeGene_2);
        }


        [TestMethod]
        public void Equals_ReturnsFalse()
        {
            if (!Genome.IsInitialized())
            {
                Genome.Init();
            }


            NodeGene nodeGene_1 = new NodeGene(1, Node.INPUT_X, Neural_Network.Node.Sigmoid);

            NodeGene nodeGene_2 = new NodeGene(2, Node.INPUT_X, Neural_Network.Node.Sigmoid);


            Assert.IsTrue(nodeGene_1 != nodeGene_2);
        }
    }
}
