using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEAT
{
    static class Helpers
    {
        private static Random random = new Random();


        /// <summary>
        /// Gets a random value.
        /// </summary>
        /// <typeparam name="TKey">The key type.</typeparam>
        /// <typeparam name="TValue">The value type.</typeparam>
        /// <param name="dictionary">The dictionary to get the value from.</param>
        /// <returns>The random value.</returns>
        public static IEnumerable<TValue> RandomValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
        {
            List<TValue> values = Enumerable.ToList(dictionary.Values);

            int size = dictionary.Count;

            while (true)
            {
                yield return values[random.Next(size)];
            }
        }
    }
}
