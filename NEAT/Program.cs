using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NEAT.Neural_Network;

namespace NEAT
{
    class Program
    {
        public static void Main(string[] args)
        {
            double input_1 = 1;
            double input_2= 0;
            double connection_weight_1 = 0;
            double connection_weight_2 = 1;
            double expected_output = 0.5;


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


            Console.WriteLine(output);

            Console.ReadKey();
        }
    }
}
