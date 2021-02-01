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
            SortedDictionary<int, string> sd = new SortedDictionary<int, string>();

            for (int i = 0; i < 10; ++i)
            {
                sd.Add(i, i.ToString());
            }


            IEnumerable<string> efewg = RandomValue(sd).Take(5);

            sd.Add(30, "30");

            IEnumerable<string> ewe = RandomValue(sd).Take(5);

            Console.WriteLine(RandomValue(sd));

            Console.ReadKey();
        }


        private static double RELU(double x)
        {
            return x;
        }


        private static IEnumerable<TValue> RandomValue<TKey, TValue>(IDictionary<TKey, TValue> dictionary)
        {
            Random rand = new Random();
            List<TValue> values = Enumerable.ToList(dictionary.Values);
            int size = dictionary.Count;
            while (true)
            {
                yield return values[rand.Next(size)];
            }
        }
    }
}
