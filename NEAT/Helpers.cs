using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEAT
{
    /// <summary>
    /// A little helper class for bits and bobs throughout the library.
    /// </summary>
    public static class Helpers
    {
        #region RandomValue

        /// <summary>
        /// Gets a random value.
        /// </summary>
        /// <typeparam name="TKey">The key type.</typeparam>
        /// <typeparam name="TValue">The value type.</typeparam>
        /// <param name="dictionary">The dictionary to get the value from.</param>
        /// <param name="random">The random object to use.</param>
        /// <returns>The random value.</returns>
        public static IEnumerable<TValue> RandomValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, Random random)
        {
            List<TValue> values = Enumerable.ToList(dictionary.Values);

            int size = dictionary.Count;

            while (true)
            {
                yield return values[random.Next(size)];
            }
        }


        /// <summary>
        /// Gets a random value.
        /// </summary>
        /// <typeparam name="TKey">The key type.</typeparam>
        /// <typeparam name="TValue">The value type.</typeparam>
        /// <param name="dictionary">The dictionary to get the value from.</param>
        /// <returns>The random value.</returns>
        public static IEnumerable<TValue> RandomValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
        {
            Random random = new Random();

            List<TValue> values = Enumerable.ToList(dictionary.Values);

            int size = dictionary.Count;

            while (true)
            {
                yield return values[random.Next(size)];
            }
        }

        #endregion RandomValue


        /// <summary>
        /// Throws an ArgumentNullException when the parameter is null.
        /// </summary>
        /// <param name="param">The parameter to check.</param>
        /// <param name="param_name">The name of the parameter for debug purposes.</param>
        internal static void ThrowOnNull(object param, string param_name)
        {
            if (param == null)
            {
                throw new ArgumentNullException(param_name, "\"" + param_name + "\" can't be null.");
            }
        }
    }
}
