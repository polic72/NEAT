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

            sd.Add(2, "2");
            sd.Add(6, "6");
            sd.Add(3, "3");
            sd.Add(4, "4");
            sd.Add(8, "8");
            sd.Add(5, "5");
            sd.Add(7, "7");
            sd.Add(1, "1");

            SortedDictionary<int, string>.ValueCollection.Enumerator enumerator = sd.Values.GetEnumerator();

            enumerator.MoveNext();

            while (enumerator.Current != null)
            {
                Console.WriteLine(enumerator.Current);

                enumerator.MoveNext();
            }


            List<string> o = new List<string>();o.Add("efw");
            Console.WriteLine(o.Contains(null));

			Console.ReadKey();
        }
    }
}
