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
			Random random = new Random();

			List<string> list = new List<string>();
			list.Add("a");
			list.Add("b");
			//list.Add("c");
			//list.Add("d");


			int choice = -1;

			int index = list.IndexOf("b");
			if (index == -1)
			{
				choice = random.Next(list.Count);
			}
			else
			{
				choice = random.Next(list.Count - 1);

				if (choice >= index)
				{
					choice++;
				}
			 }


			Console.WriteLine(choice);

			//Console.ReadKey();
        }
    }
}
