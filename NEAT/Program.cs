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
            Node input = new Node(Node.INPUT_X, RELU);
            input.SetOutput(2);


            Node hidden_1 = new Node(.2, RELU);

            Node hidden_2 = new Node(.4, RELU);


            Node output = new Node(Node.OUTPUT_X, RELU);


            Connection connection_1 = new Connection(input, hidden_1)
            {
                Weight = 1
            };
            Connection connection_2 = new Connection(hidden_1, hidden_2)
            {
                Weight = 1
            };
            Connection connection_3 = new Connection(hidden_2, output)
            {
                Weight = 1
            };


            Console.WriteLine(output.Output);

            Console.ReadKey();
        }


        private static double RELU(double x)
        {
            return x;
        }
    }
}
