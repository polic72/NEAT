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
            Node node = new Node(Node.INPUT_X);

            Console.WriteLine(node.Output);

            Console.ReadKey();
        }
    }
}
