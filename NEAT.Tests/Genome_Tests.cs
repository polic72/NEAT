using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

using NEAT.Genetic;
using NEAT.Neural_Network;

namespace NEAT.Tests
{
    [TestClass]
    public class Genome_Tests
    {
        #region Distance

        #region Self

        [TestMethod]
        public void Distance_Self_NoNodes_NoConnections()
        {
            if (!Genome.IsInitialized())
            {
                Genome.Init();
            }


            Random random = new Random();

            Genome genome_1 = new Genome(random);


            Assert.AreEqual(0, genome_1.Distance(genome_1));
        }


        [TestMethod]
        public void Distance_Self_1Node_NoConnections()
        {
            if (!Genome.IsInitialized())
            {
                Genome.Init();
            }


            NodeGene nodeGene = new NodeGene(1, Neural_Network.Node.Sigmoid);


            Random random = new Random();

            Genome genome_1 = new Genome(random);
            genome_1.NodeGenes.Add(nodeGene);


            Assert.AreEqual(0, genome_1.Distance(genome_1));
        }


        [TestMethod]
        public void Distance_Self_ManyNodes_NoConnections()
        {
            if (!Genome.IsInitialized())
            {
                Genome.Init();
            }


            NodeGene nodeGene_1 = new NodeGene(1, Neural_Network.Node.Sigmoid);
            NodeGene nodeGene_2 = new NodeGene(2, Neural_Network.Node.Sigmoid);
            NodeGene nodeGene_3 = new NodeGene(3, Neural_Network.Node.Sigmoid);


            Random random = new Random();

            Genome genome_1 = new Genome(random);
            genome_1.NodeGenes.Add(nodeGene_1);
            genome_1.NodeGenes.Add(nodeGene_2);
            genome_1.NodeGenes.Add(nodeGene_3);


            Assert.AreEqual(0, genome_1.Distance(genome_1));
        }


        [TestMethod]
        public void Distance_Self_TwoNodes_1Connection()
        {
            if (!Genome.IsInitialized())
            {
                Genome.Init();
            }


            NodeGene nodeGene_1 = new NodeGene(1, Neural_Network.Node.Sigmoid);
            NodeGene nodeGene_2 = new NodeGene(2, Neural_Network.Node.Sigmoid);

            ConnectionGene connectionGene = new ConnectionGene(nodeGene_1, nodeGene_2, 1);


            Random random = new Random();

            Genome genome_1 = new Genome(random);
            genome_1.NodeGenes.Add(nodeGene_1);
            genome_1.NodeGenes.Add(nodeGene_2);

            genome_1.ConnectionGenes.Add(connectionGene);


            Assert.AreEqual(0, genome_1.Distance(genome_1));
        }


        [TestMethod]
        public void Distance_Self_ManyNodes_ManyConnections()
        {
            if (!Genome.IsInitialized())
            {
                Genome.Init();
            }


            NodeGene nodeGene_1 = new NodeGene(1, Neural_Network.Node.Sigmoid);
            NodeGene nodeGene_2 = new NodeGene(2, Neural_Network.Node.Sigmoid);

            NodeGene output_nodeGene = new NodeGene(3, Neural_Network.Node.Sigmoid);

            ConnectionGene connectionGene_1 = new ConnectionGene(nodeGene_1, output_nodeGene, 1);
            ConnectionGene connectionGene_2 = new ConnectionGene(nodeGene_2, output_nodeGene, 2);


            Random random = new Random();

            Genome genome_1 = new Genome(random);
            genome_1.NodeGenes.Add(nodeGene_1);
            genome_1.NodeGenes.Add(nodeGene_2);
            genome_1.NodeGenes.Add(output_nodeGene);

            genome_1.ConnectionGenes.Add(connectionGene_1);
            genome_1.ConnectionGenes.Add(connectionGene_2);


            Assert.AreEqual(0, genome_1.Distance(genome_1));
        }

        #endregion Self


        #region NoNodes_NoConnections

        [TestMethod]
        public void Distance_NoNodes_NoConnections_1to2()
        {
            if (!Genome.IsInitialized())
            {
                Genome.Init();
            }


            Random random = new Random();

            Genome genome_1 = new Genome(random);

            Genome genome_2 = new Genome(random);


            Assert.AreEqual(0, genome_1.Distance(genome_2));
        }


        [TestMethod]
        public void Distance_NoNodes_NoConnections_2to1()
        {
            if (!Genome.IsInitialized())
            {
                Genome.Init();
            }


            Random random = new Random();

            Genome genome_1 = new Genome(random);

            Genome genome_2 = new Genome(random);


            Assert.AreEqual(0, genome_2.Distance(genome_1));
        }

        #endregion NoNodes_NoConnections


        #region 1Node_NoConnections

        [TestMethod]
        public void Distance_1Node_NoConnections_1to2()
        {
            if (!Genome.IsInitialized())
            {
                Genome.Init();
            }


            NodeGene nodeGene_1 = new NodeGene(1, Neural_Network.Node.Sigmoid);
            NodeGene nodeGene_2 = new NodeGene(2, Neural_Network.Node.Sigmoid);


            Random random = new Random();

            Genome genome_1 = new Genome(random);
            genome_1.NodeGenes.Add(nodeGene_1);

            Genome genome_2 = new Genome(random);
            genome_2.NodeGenes.Add(nodeGene_2);


            Assert.AreEqual(0, genome_1.Distance(genome_2));
        }


        [TestMethod]
        public void Distance_1Node_NoConnections_2to1()
        {
            if (!Genome.IsInitialized())
            {
                Genome.Init();
            }


            NodeGene nodeGene_1 = new NodeGene(1, Neural_Network.Node.Sigmoid);
            NodeGene nodeGene_2 = new NodeGene(2, Neural_Network.Node.Sigmoid);


            Random random = new Random();

            Genome genome_1 = new Genome(random);
            genome_1.NodeGenes.Add(nodeGene_1);

            Genome genome_2 = new Genome(random);
            genome_2.NodeGenes.Add(nodeGene_2);


            Assert.AreEqual(0, genome_2.Distance(genome_1));
        }

        #endregion 1Node_NoConnections


        #region ManyNodes_NoConnections

        [TestMethod]
        public void Distance_ManyNodes_NoConnections_1to2()
        {
            if (!Genome.IsInitialized())
            {
                Genome.Init();
            }


            NodeGene nodeGene_1 = new NodeGene(1, Neural_Network.Node.Sigmoid);
            NodeGene nodeGene_2 = new NodeGene(2, Neural_Network.Node.Sigmoid);

            NodeGene nodeGene_3 = new NodeGene(1, Neural_Network.Node.Sigmoid);
            NodeGene nodeGene_4 = new NodeGene(2, Neural_Network.Node.Sigmoid);


            Random random = new Random();

            Genome genome_1 = new Genome(random);
            genome_1.NodeGenes.Add(nodeGene_1);
            genome_1.NodeGenes.Add(nodeGene_2);

            Genome genome_2 = new Genome(random);
            genome_2.NodeGenes.Add(nodeGene_3);
            genome_2.NodeGenes.Add(nodeGene_4);


            Assert.AreEqual(0, genome_1.Distance(genome_2));
        }


        [TestMethod]
        public void Distance_ManyNodes_NoConnections_2to1()
        {
            if (!Genome.IsInitialized())
            {
                Genome.Init();
            }


            NodeGene nodeGene_1 = new NodeGene(1, Neural_Network.Node.Sigmoid);
            NodeGene nodeGene_2 = new NodeGene(2, Neural_Network.Node.Sigmoid);

            NodeGene nodeGene_3 = new NodeGene(1, Neural_Network.Node.Sigmoid);
            NodeGene nodeGene_4 = new NodeGene(2, Neural_Network.Node.Sigmoid);


            Random random = new Random();

            Genome genome_1 = new Genome(random);
            genome_1.NodeGenes.Add(nodeGene_1);
            genome_1.NodeGenes.Add(nodeGene_2);

            Genome genome_2 = new Genome(random);
            genome_2.NodeGenes.Add(nodeGene_3);
            genome_2.NodeGenes.Add(nodeGene_4);


            Assert.AreEqual(0, genome_2.Distance(genome_1));
        }

        #endregion ManyNodes_NoConnections


        #region TwoNodes_1Connection_SameWeight

        [TestMethod]
        public void Distance_TwoNodes_1Connection_SameWeight_1to2()
        {
            if (!Genome.IsInitialized())
            {
                Genome.Init();
            }


            NodeGene nodeGene_1 = new NodeGene(1, Neural_Network.Node.Sigmoid);
            NodeGene nodeGene_2 = new NodeGene(2, Neural_Network.Node.Sigmoid);

            NodeGene nodeGene_3 = new NodeGene(1, Neural_Network.Node.Sigmoid);
            NodeGene nodeGene_4 = new NodeGene(2, Neural_Network.Node.Sigmoid);

            ConnectionGene connectionGene_1 = new ConnectionGene(nodeGene_1, nodeGene_2, 1);
            ConnectionGene connectionGene_2 = new ConnectionGene(nodeGene_3, nodeGene_4, 1);


            Random random = new Random();

            Genome genome_1 = new Genome(random);
            genome_1.NodeGenes.Add(nodeGene_1);
            genome_1.NodeGenes.Add(nodeGene_2);

            genome_1.ConnectionGenes.Add(connectionGene_1);


            Genome genome_2 = new Genome(random);
            genome_2.NodeGenes.Add(nodeGene_3);
            genome_2.NodeGenes.Add(nodeGene_4);

            genome_2.ConnectionGenes.Add(connectionGene_2);


            Assert.AreEqual(0, genome_1.Distance(genome_2));
        }


        [TestMethod]
        public void Distance_TwoNodes_1Connection_SameWeight_2to1()
        {
            if (!Genome.IsInitialized())
            {
                Genome.Init();
            }


            NodeGene nodeGene_1 = new NodeGene(1, Neural_Network.Node.Sigmoid);
            NodeGene nodeGene_2 = new NodeGene(2, Neural_Network.Node.Sigmoid);

            NodeGene nodeGene_3 = new NodeGene(1, Neural_Network.Node.Sigmoid);
            NodeGene nodeGene_4 = new NodeGene(2, Neural_Network.Node.Sigmoid);

            ConnectionGene connectionGene_1 = new ConnectionGene(nodeGene_1, nodeGene_2, 1);
            ConnectionGene connectionGene_2 = new ConnectionGene(nodeGene_3, nodeGene_4, 1);


            Random random = new Random();

            Genome genome_1 = new Genome(random);
            genome_1.NodeGenes.Add(nodeGene_1);
            genome_1.NodeGenes.Add(nodeGene_2);

            genome_1.ConnectionGenes.Add(connectionGene_1);


            Genome genome_2 = new Genome(random);
            genome_2.NodeGenes.Add(nodeGene_3);
            genome_2.NodeGenes.Add(nodeGene_4);

            genome_2.ConnectionGenes.Add(connectionGene_2);


            Assert.AreEqual(0, genome_2.Distance(genome_1));
        }

        #endregion TwoNodes_1Connection_SameWeight


        #region TwoNodes_1Connection_DifferentWeight

        [TestMethod]
        public void Distance_TwoNodes_1Connection_DifferentWeight_1to2()
        {
            if (!Genome.IsInitialized())
            {
                Genome.Init();
            }


            NodeGene nodeGene_1 = new NodeGene(1, Neural_Network.Node.Sigmoid);
            NodeGene nodeGene_2 = new NodeGene(2, Neural_Network.Node.Sigmoid);

            NodeGene nodeGene_3 = new NodeGene(1, Neural_Network.Node.Sigmoid);
            NodeGene nodeGene_4 = new NodeGene(2, Neural_Network.Node.Sigmoid);

            ConnectionGene connectionGene_1 = new ConnectionGene(nodeGene_1, nodeGene_2, 1);
            ConnectionGene connectionGene_2 = new ConnectionGene(nodeGene_3, nodeGene_4, 2);


            Random random = new Random();

            Genome genome_1 = new Genome(random);
            genome_1.NodeGenes.Add(nodeGene_1);
            genome_1.NodeGenes.Add(nodeGene_2);

            genome_1.ConnectionGenes.Add(connectionGene_1);


            Genome genome_2 = new Genome(random);
            genome_2.NodeGenes.Add(nodeGene_3);
            genome_2.NodeGenes.Add(nodeGene_4);

            genome_2.ConnectionGenes.Add(connectionGene_2);


            Assert.AreEqual(Genome.C3 * 1, genome_1.Distance(genome_2));
        }


        [TestMethod]
        public void Distance_TwoNodes_1Connection_DifferentWeight_2to1()
        {
            if (!Genome.IsInitialized())
            {
                Genome.Init();
            }


            NodeGene nodeGene_1 = new NodeGene(1, Neural_Network.Node.Sigmoid);
            NodeGene nodeGene_2 = new NodeGene(2, Neural_Network.Node.Sigmoid);

            NodeGene nodeGene_3 = new NodeGene(1, Neural_Network.Node.Sigmoid);
            NodeGene nodeGene_4 = new NodeGene(2, Neural_Network.Node.Sigmoid);

            ConnectionGene connectionGene_1 = new ConnectionGene(nodeGene_1, nodeGene_2, 1);
            ConnectionGene connectionGene_2 = new ConnectionGene(nodeGene_3, nodeGene_4, 2);


            Random random = new Random();

            Genome genome_1 = new Genome(random);
            genome_1.NodeGenes.Add(nodeGene_1);
            genome_1.NodeGenes.Add(nodeGene_2);

            genome_1.ConnectionGenes.Add(connectionGene_1);


            Genome genome_2 = new Genome(random);
            genome_2.NodeGenes.Add(nodeGene_3);
            genome_2.NodeGenes.Add(nodeGene_4);

            genome_2.ConnectionGenes.Add(connectionGene_2);


            Assert.AreEqual(Genome.C3 * 1, genome_2.Distance(genome_1));
        }

        #endregion TwoNodes_1Connection_DifferentWeight


        #region ManyNodes_SameConnections_SameWeight

        [TestMethod]
        public void Distance_ManyNodes_SameConnections_SameWeight_1to2()
        {
            if (!Genome.IsInitialized())
            {
                Genome.Init();
            }


            NodeGene nodeGene_1 = new NodeGene(1, Neural_Network.Node.Sigmoid);
            NodeGene nodeGene_2 = new NodeGene(2, Neural_Network.Node.Sigmoid);

            NodeGene output_nodeGene_1 = new NodeGene(3, Neural_Network.Node.Sigmoid);

            ConnectionGene connectionGene_1 = new ConnectionGene(nodeGene_1, output_nodeGene_1, 1);
            ConnectionGene connectionGene_2 = new ConnectionGene(nodeGene_2, output_nodeGene_1, 2);


            NodeGene nodeGene_3 = new NodeGene(1, Neural_Network.Node.Sigmoid);
            NodeGene nodeGene_4 = new NodeGene(2, Neural_Network.Node.Sigmoid);

            NodeGene output_nodeGene_2 = new NodeGene(3, Neural_Network.Node.Sigmoid);

            ConnectionGene connectionGene_3 = new ConnectionGene(nodeGene_3, output_nodeGene_2, 1);
            ConnectionGene connectionGene_4 = new ConnectionGene(nodeGene_4, output_nodeGene_2, 2);


            Random random = new Random();

            Genome genome_1 = new Genome(random);
            genome_1.NodeGenes.Add(nodeGene_1);
            genome_1.NodeGenes.Add(nodeGene_2);
            genome_1.NodeGenes.Add(output_nodeGene_1);

            genome_1.ConnectionGenes.Add(connectionGene_1);
            genome_1.ConnectionGenes.Add(connectionGene_2);


            Genome genome_2 = new Genome(random);
            genome_2.NodeGenes.Add(nodeGene_3);
            genome_2.NodeGenes.Add(nodeGene_4);

            genome_2.ConnectionGenes.Add(connectionGene_3);
            genome_2.ConnectionGenes.Add(connectionGene_4);


            Assert.AreEqual(0, genome_1.Distance(genome_2));
        }


        [TestMethod]
        public void Distance_ManyNodes_SameConnections_SameWeight_2to1()
        {
            if (!Genome.IsInitialized())
            {
                Genome.Init();
            }


            NodeGene nodeGene_1 = new NodeGene(1, Neural_Network.Node.Sigmoid);
            NodeGene nodeGene_2 = new NodeGene(2, Neural_Network.Node.Sigmoid);

            NodeGene output_nodeGene_1 = new NodeGene(3, Neural_Network.Node.Sigmoid);

            ConnectionGene connectionGene_1 = new ConnectionGene(nodeGene_1, output_nodeGene_1, 1);
            ConnectionGene connectionGene_2 = new ConnectionGene(nodeGene_2, output_nodeGene_1, 2);


            NodeGene nodeGene_3 = new NodeGene(1, Neural_Network.Node.Sigmoid);
            NodeGene nodeGene_4 = new NodeGene(2, Neural_Network.Node.Sigmoid);

            NodeGene output_nodeGene_2 = new NodeGene(3, Neural_Network.Node.Sigmoid);

            ConnectionGene connectionGene_3 = new ConnectionGene(nodeGene_3, output_nodeGene_2, 1);
            ConnectionGene connectionGene_4 = new ConnectionGene(nodeGene_4, output_nodeGene_2, 2);


            Random random = new Random();

            Genome genome_1 = new Genome(random);
            genome_1.NodeGenes.Add(nodeGene_1);
            genome_1.NodeGenes.Add(nodeGene_2);
            genome_1.NodeGenes.Add(output_nodeGene_1);

            genome_1.ConnectionGenes.Add(connectionGene_1);
            genome_1.ConnectionGenes.Add(connectionGene_2);


            Genome genome_2 = new Genome(random);
            genome_2.NodeGenes.Add(nodeGene_3);
            genome_2.NodeGenes.Add(nodeGene_4);

            genome_2.ConnectionGenes.Add(connectionGene_3);
            genome_2.ConnectionGenes.Add(connectionGene_4);


            Assert.AreEqual(0, genome_2.Distance(genome_1));
        }

        #endregion ManyNodes_SameConnections_SameWeight


        #region ManyNodes_SameConnections_DifferentWeight

        [TestMethod]
        public void Distance_ManyNodes_SameConnections_DifferentWeight_1to2()
        {
            if (!Genome.IsInitialized())
            {
                Genome.Init();
            }


            NodeGene nodeGene_1 = new NodeGene(1, Neural_Network.Node.Sigmoid);
            NodeGene nodeGene_2 = new NodeGene(2, Neural_Network.Node.Sigmoid);

            NodeGene output_nodeGene_1 = new NodeGene(3, Neural_Network.Node.Sigmoid);

            ConnectionGene connectionGene_1 = new ConnectionGene(nodeGene_1, output_nodeGene_1, 1);
            ConnectionGene connectionGene_2 = new ConnectionGene(nodeGene_2, output_nodeGene_1, 2);


            NodeGene nodeGene_3 = new NodeGene(1, Neural_Network.Node.Sigmoid);
            NodeGene nodeGene_4 = new NodeGene(2, Neural_Network.Node.Sigmoid);

            NodeGene output_nodeGene_2 = new NodeGene(3, Neural_Network.Node.Sigmoid);

            ConnectionGene connectionGene_3 = new ConnectionGene(nodeGene_3, output_nodeGene_2, 1);
            ConnectionGene connectionGene_4 = new ConnectionGene(nodeGene_4, output_nodeGene_2, 4);


            Random random = new Random();

            Genome genome_1 = new Genome(random);
            genome_1.NodeGenes.Add(nodeGene_1);
            genome_1.NodeGenes.Add(nodeGene_2);
            genome_1.NodeGenes.Add(output_nodeGene_1);

            genome_1.ConnectionGenes.Add(connectionGene_1);
            genome_1.ConnectionGenes.Add(connectionGene_2);


            Genome genome_2 = new Genome(random);
            genome_2.NodeGenes.Add(nodeGene_3);
            genome_2.NodeGenes.Add(nodeGene_4);

            genome_2.ConnectionGenes.Add(connectionGene_3);
            genome_2.ConnectionGenes.Add(connectionGene_4);


            Assert.AreEqual(Genome.C3 * 1, genome_1.Distance(genome_2));
        }


        [TestMethod]
        public void Distance_ManyNodes_SameConnections_DifferentWeight_2to1()
        {
            if (!Genome.IsInitialized())
            {
                Genome.Init();
            }


            NodeGene nodeGene_1 = new NodeGene(1, Neural_Network.Node.Sigmoid);
            NodeGene nodeGene_2 = new NodeGene(2, Neural_Network.Node.Sigmoid);

            NodeGene output_nodeGene_1 = new NodeGene(3, Neural_Network.Node.Sigmoid);

            ConnectionGene connectionGene_1 = new ConnectionGene(nodeGene_1, output_nodeGene_1, 1);
            ConnectionGene connectionGene_2 = new ConnectionGene(nodeGene_2, output_nodeGene_1, 2);


            NodeGene nodeGene_3 = new NodeGene(1, Neural_Network.Node.Sigmoid);
            NodeGene nodeGene_4 = new NodeGene(2, Neural_Network.Node.Sigmoid);

            NodeGene output_nodeGene_2 = new NodeGene(3, Neural_Network.Node.Sigmoid);

            ConnectionGene connectionGene_3 = new ConnectionGene(nodeGene_3, output_nodeGene_2, 1);
            ConnectionGene connectionGene_4 = new ConnectionGene(nodeGene_4, output_nodeGene_2, 4);


            Random random = new Random();

            Genome genome_1 = new Genome(random);
            genome_1.NodeGenes.Add(nodeGene_1);
            genome_1.NodeGenes.Add(nodeGene_2);
            genome_1.NodeGenes.Add(output_nodeGene_1);

            genome_1.ConnectionGenes.Add(connectionGene_1);
            genome_1.ConnectionGenes.Add(connectionGene_2);


            Genome genome_2 = new Genome(random);
            genome_2.NodeGenes.Add(nodeGene_3);
            genome_2.NodeGenes.Add(nodeGene_4);

            genome_2.ConnectionGenes.Add(connectionGene_3);
            genome_2.ConnectionGenes.Add(connectionGene_4);


            Assert.AreEqual(Genome.C3 * 1, genome_2.Distance(genome_1));
        }

        #endregion ManyNodes_SameConnections_DifferentWeight


        #region ManyNodes_DifferentConnections_SameWeight

        [TestMethod]
        public void Distance_ManyNodes_DifferentConnections_SameWeight_1to2()
        {
            if (!Genome.IsInitialized())
            {
                Genome.Init();
            }


            NodeGene nodeGene_1 = new NodeGene(1, Neural_Network.Node.Sigmoid);
            NodeGene nodeGene_2 = new NodeGene(2, Neural_Network.Node.Sigmoid);

            NodeGene output_nodeGene_1 = new NodeGene(3, Neural_Network.Node.Sigmoid);

            ConnectionGene connectionGene_1 = new ConnectionGene(nodeGene_1, output_nodeGene_1, 1);
            ConnectionGene connectionGene_2 = new ConnectionGene(nodeGene_2, output_nodeGene_1, 2);


            NodeGene nodeGene_3 = new NodeGene(4, Neural_Network.Node.Sigmoid);
            NodeGene nodeGene_4 = new NodeGene(5, Neural_Network.Node.Sigmoid);

            NodeGene output_nodeGene_2 = new NodeGene(3, Neural_Network.Node.Sigmoid);

            ConnectionGene connectionGene_3 = new ConnectionGene(nodeGene_3, output_nodeGene_2, 1);
            ConnectionGene connectionGene_4 = new ConnectionGene(nodeGene_4, output_nodeGene_2, 2);


            Random random = new Random();

            Genome genome_1 = new Genome(random);
            genome_1.NodeGenes.Add(nodeGene_1);
            genome_1.NodeGenes.Add(nodeGene_2);
            genome_1.NodeGenes.Add(output_nodeGene_1);

            genome_1.ConnectionGenes.Add(connectionGene_1);
            genome_1.ConnectionGenes.Add(connectionGene_2);


            Genome genome_2 = new Genome(random);
            genome_2.NodeGenes.Add(nodeGene_3);
            genome_2.NodeGenes.Add(nodeGene_4);

            genome_2.ConnectionGenes.Add(connectionGene_3);
            genome_2.ConnectionGenes.Add(connectionGene_4);


            Assert.AreEqual(Genome.C1 * 2 + Genome.C2 * 2, genome_1.Distance(genome_2));
        }


        [TestMethod]
        public void Distance_ManyNodes_DifferentConnections_SameWeight_2to1()
        {
            if (!Genome.IsInitialized())
            {
                Genome.Init();
            }


            NodeGene nodeGene_1 = new NodeGene(1, Neural_Network.Node.Sigmoid);
            NodeGene nodeGene_2 = new NodeGene(2, Neural_Network.Node.Sigmoid);

            NodeGene output_nodeGene_1 = new NodeGene(3, Neural_Network.Node.Sigmoid);

            ConnectionGene connectionGene_1 = new ConnectionGene(nodeGene_1, output_nodeGene_1, 1);
            ConnectionGene connectionGene_2 = new ConnectionGene(nodeGene_2, output_nodeGene_1, 2);


            NodeGene nodeGene_3 = new NodeGene(4, Neural_Network.Node.Sigmoid);
            NodeGene nodeGene_4 = new NodeGene(5, Neural_Network.Node.Sigmoid);

            NodeGene output_nodeGene_2 = new NodeGene(3, Neural_Network.Node.Sigmoid);

            ConnectionGene connectionGene_3 = new ConnectionGene(nodeGene_3, output_nodeGene_2, 1);
            ConnectionGene connectionGene_4 = new ConnectionGene(nodeGene_4, output_nodeGene_2, 2);


            Random random = new Random();

            Genome genome_1 = new Genome(random);
            genome_1.NodeGenes.Add(nodeGene_1);
            genome_1.NodeGenes.Add(nodeGene_2);
            genome_1.NodeGenes.Add(output_nodeGene_1);

            genome_1.ConnectionGenes.Add(connectionGene_1);
            genome_1.ConnectionGenes.Add(connectionGene_2);


            Genome genome_2 = new Genome(random);
            genome_2.NodeGenes.Add(nodeGene_3);
            genome_2.NodeGenes.Add(nodeGene_4);

            genome_2.ConnectionGenes.Add(connectionGene_3);
            genome_2.ConnectionGenes.Add(connectionGene_4);


            Assert.AreEqual(Genome.C1 * 2 + Genome.C2 * 2, genome_2.Distance(genome_1));
        }

        #endregion ManyNodes_DifferentConnections_SameWeight


        #region ManyNodes_DifferentConnections_DifferentWeight

        [TestMethod]
        public void Distance_ManyNodes_DifferentConnections_DifferentWeight_1to2()
        {
            if (!Genome.IsInitialized())
            {
                Genome.Init();
            }


            NodeGene nodeGene_1 = new NodeGene(1, Neural_Network.Node.Sigmoid);
            NodeGene nodeGene_2 = new NodeGene(2, Neural_Network.Node.Sigmoid);

            NodeGene output_nodeGene_1 = new NodeGene(3, Neural_Network.Node.Sigmoid);

            ConnectionGene connectionGene_1 = new ConnectionGene(nodeGene_1, output_nodeGene_1, 1);
            ConnectionGene connectionGene_2 = new ConnectionGene(nodeGene_2, output_nodeGene_1, 2);


            NodeGene nodeGene_3 = new NodeGene(4, Neural_Network.Node.Sigmoid);
            NodeGene nodeGene_4 = new NodeGene(5, Neural_Network.Node.Sigmoid);

            NodeGene output_nodeGene_2 = new NodeGene(3, Neural_Network.Node.Sigmoid);

            ConnectionGene connectionGene_3 = new ConnectionGene(nodeGene_3, output_nodeGene_2, 1);
            ConnectionGene connectionGene_4 = new ConnectionGene(nodeGene_4, output_nodeGene_2, 4);


            Random random = new Random();

            Genome genome_1 = new Genome(random);
            genome_1.NodeGenes.Add(nodeGene_1);
            genome_1.NodeGenes.Add(nodeGene_2);
            genome_1.NodeGenes.Add(output_nodeGene_1);

            genome_1.ConnectionGenes.Add(connectionGene_1);
            genome_1.ConnectionGenes.Add(connectionGene_2);


            Genome genome_2 = new Genome(random);
            genome_2.NodeGenes.Add(nodeGene_3);
            genome_2.NodeGenes.Add(nodeGene_4);

            genome_2.ConnectionGenes.Add(connectionGene_3);
            genome_2.ConnectionGenes.Add(connectionGene_4);


            Assert.AreEqual(Genome.C1 * 2 + Genome.C2 * 2, genome_1.Distance(genome_2));
        }


        [TestMethod]
        public void Distance_ManyNodes_DifferentConnections_DifferentWeight_2to1()
        {
            if (!Genome.IsInitialized())
            {
                Genome.Init();
            }


            NodeGene nodeGene_1 = new NodeGene(1, Neural_Network.Node.Sigmoid);
            NodeGene nodeGene_2 = new NodeGene(2, Neural_Network.Node.Sigmoid);

            NodeGene output_nodeGene_1 = new NodeGene(3, Neural_Network.Node.Sigmoid);

            ConnectionGene connectionGene_1 = new ConnectionGene(nodeGene_1, output_nodeGene_1, 1);
            ConnectionGene connectionGene_2 = new ConnectionGene(nodeGene_2, output_nodeGene_1, 2);


            NodeGene nodeGene_3 = new NodeGene(4, Neural_Network.Node.Sigmoid);
            NodeGene nodeGene_4 = new NodeGene(5, Neural_Network.Node.Sigmoid);

            NodeGene output_nodeGene_2 = new NodeGene(3, Neural_Network.Node.Sigmoid);

            ConnectionGene connectionGene_3 = new ConnectionGene(nodeGene_3, output_nodeGene_2, 1);
            ConnectionGene connectionGene_4 = new ConnectionGene(nodeGene_4, output_nodeGene_2, 4);


            Random random = new Random();

            Genome genome_1 = new Genome(random);
            genome_1.NodeGenes.Add(nodeGene_1);
            genome_1.NodeGenes.Add(nodeGene_2);
            genome_1.NodeGenes.Add(output_nodeGene_1);

            genome_1.ConnectionGenes.Add(connectionGene_1);
            genome_1.ConnectionGenes.Add(connectionGene_2);


            Genome genome_2 = new Genome(random);
            genome_2.NodeGenes.Add(nodeGene_3);
            genome_2.NodeGenes.Add(nodeGene_4);

            genome_2.ConnectionGenes.Add(connectionGene_3);
            genome_2.ConnectionGenes.Add(connectionGene_4);


            Assert.AreEqual(Genome.C1 * 2 + Genome.C2 * 2, genome_2.Distance(genome_1));
        }

        #endregion ManyNodes_DifferentConnections_DifferentWeight

        #endregion Distance


        #region Crossover

        #region Self

        [TestMethod]
        public void Crossover_Self_1Connection()
        {
            if (!Genome.IsInitialized())
            {
                Genome.Init();
            }


            Random random = new Random(1);


            int nodeGene_num = 1;

            NodeGene nodeGene_1 = new NodeGene(nodeGene_num++, Node.Sigmoid);
            NodeGene nodeGene_2 = new NodeGene(nodeGene_num++, Node.Sigmoid);

            ConnectionGene connectionGene = new ConnectionGene(nodeGene_1, nodeGene_2, 4);


            Genome genome = new Genome(random);
            genome.NodeGenes.Add(nodeGene_1);
            genome.NodeGenes.Add(nodeGene_2);

            genome.ConnectionGenes.Add(connectionGene);


            Genome created_genome = genome.Crossover(genome);


            bool[] contains_genes = { false, false, false };

            foreach (ConnectionGene cgene in created_genome.ConnectionGenes)
            {
                if (cgene == connectionGene)
                {
                    contains_genes[0] = true;

                    break;
                }
            }

            foreach (NodeGene ngene in created_genome.NodeGenes)
            {
                if (ngene == nodeGene_1)
                {
                    contains_genes[1] = true;
                }
                else if (ngene == nodeGene_2)
                {
                    contains_genes[2] = true;
                }
            }


            Assert.AreEqual(3, contains_genes.Count(x => x == true));
        }


        [TestMethod]
        public void Crossover_Self_2Connections()
        {
            if (!Genome.IsInitialized())
            {
                Genome.Init();
            }


            Random random = new Random(1);


            int nodeGene_num = 1;

            NodeGene nodeGene_1 = new NodeGene(nodeGene_num++, Node.Sigmoid);
            NodeGene nodeGene_2 = new NodeGene(nodeGene_num++, Node.Sigmoid);

            NodeGene nodeGene_3 = new NodeGene(nodeGene_num++, Node.Sigmoid);

            ConnectionGene connectionGene_1 = new ConnectionGene(nodeGene_1, nodeGene_3, 4);
            ConnectionGene connectionGene_2 = new ConnectionGene(nodeGene_2, nodeGene_3, -1);


            Genome genome = new Genome(random);
            genome.NodeGenes.Add(nodeGene_1);
            genome.NodeGenes.Add(nodeGene_2);
            genome.NodeGenes.Add(nodeGene_3);

            genome.ConnectionGenes.Add(connectionGene_1);
            genome.ConnectionGenes.Add(connectionGene_2);


            Genome created_genome = genome.Crossover(genome);


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


            Assert.AreEqual(5, contains_genes.Count(x => x == true));
        }

        #endregion Self


        #region Other

        [TestMethod]
        public void Crossover_2Disjoint()
        {
            if (!Genome.IsInitialized())
            {
                Genome.Init();
            }

            Random random = new Random(1);


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


            Assert.AreEqual(5, contains_genes.Count(x => x == true));
        }

        #endregion Other

        #endregion Crossover
    }
}
