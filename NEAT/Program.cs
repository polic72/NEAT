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
            Node node_1 = new Node();
            Node node_2 = new Node();

            Node node_3 = new Node(Node.OUTPUT_X);

            Connection connection = new Connection(node_1, node_3)
            {
                Weight = 1
            };

            Connection connection2 = new Connection(node_2, node_3)
            {
                Weight = 2
            };


            Console.WriteLine(node_3.Calulate());

            Console.ReadKey();
        }
    }
}
