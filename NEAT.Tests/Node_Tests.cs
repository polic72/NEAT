using Microsoft.VisualStudio.TestTools.UnitTesting;
using NEAT.Neural_Network;
using System;

namespace NEAT.Tests
{
    [TestClass]
    public class Node_Tests
    {
        [DataTestMethod]
        [DataRow(0)]
        [DataRow(double.Epsilon)]
        [DataRow(double.MinValue)]
        [DataRow(double.MaxValue)]
        [DataRow(double.NaN)]
        public void SetOutput_SetWorks(double output_value)
        {
            Node node = new Node();

            node.SetOutput(output_value);

            Assert.AreEqual<double>(output_value, node.Output);
        }


        [TestMethod]
        public void Calculate_Perceptron_NoConnections()
        {
            Node node = new Node();

            double output = node.Calulate();

            Assert.AreEqual(output, 0.5);
        }


        [DataTestMethod]
        [DataRow(0, 0, 0.5)]
        [DataRow(1, 0, 0.5)]
        [DataRow(1, 1, 0.73105857863)]
        [DataRow(2, 4, 0.99966464986)]
        public void Calculate_Perceptron_OneConnection(double input, double connection_weight, double expected_output)
        {
            Node input_node = new Node();
            input_node.SetOutput(input);

            Node testing_node = new Node(Node.OUTPUT_X);

            Connection connection = new Connection(input_node, testing_node)
            {
                Weight = connection_weight
            };


            double output = testing_node.Calulate();

            Assert.AreEqual(expected_output, output, 0.0001);
        }



        [DataTestMethod]
        [DataRow(0, 0, 0, 0, 0.5)]
        [DataRow(1, 1, 0, 0, 0.5)]
        [DataRow(1, 0, 0, 1, 0.5)]
        [DataRow(1, 1, 1, 1, 0.88079707797)]
        [DataRow(1, 1, .5, 2, 0.92414181997)]
        public void Calculate_Perceptron_TwoConnections(double input_1, double input_2, double connection_weight_1, double connection_weight_2, double expected_output)
        {
            Node input_node_1 = new Node();
            input_node_1.SetOutput(input_1);

            Node input_node_2 = new Node();
            input_node_2.SetOutput(input_2);


            Node testing_node = new Node(Node.OUTPUT_X);


            Connection connection_1 = new Connection(input_node_1, testing_node)
            {
                Weight = connection_weight_1
            };

            Connection connection_2 = new Connection(input_node_2, testing_node)
            {
                Weight = connection_weight_2
            };


            double output = testing_node.Calulate();

            Assert.AreEqual(expected_output, output, 0.0001);
        }
    }
}
