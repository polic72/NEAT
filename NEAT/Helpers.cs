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

        #region Dictionary

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


        ///// <summary>
        ///// Gets a random value.
        ///// </summary>
        ///// <typeparam name="TKey">The key type.</typeparam>
        ///// <typeparam name="TValue">The value type.</typeparam>
        ///// <param name="dictionary">The dictionary to get the value from.</param>
        ///// <returns>The random value.</returns>
        //public static IEnumerable<TValue> RandomValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
        //{
        //    Random random = new Random();

        //    List<TValue> values = Enumerable.ToList(dictionary.Values);

        //    int size = dictionary.Count;

        //    while (true)
        //    {
        //        yield return values[random.Next(size)];
        //    }
        //}

        #endregion Dictionary


        #region Set

        /// <summary>
        /// Gets a random value.
        /// </summary>
        /// <typeparam name="T">The set type.</typeparam>
        /// <param name="set">The set to get the value from.</param>
        /// <param name="random">The random object to use.</param>
        /// <returns>default(T) if the set.Count is 0 or something bad happened. The random object otherwise.</returns>
        public static T RandomValue<T>(this ISet<T> set, Random random)
        {
            if (set.Count == 0)
            {
                return default;
            }


            int index = random.Next(set.Count);

            int i = 0;
            foreach (T obj in set)
            {
                if (i++ == index)
                {
                    return obj;
                }
            }

            return default;
        }


        ///// <summary>
        ///// Gets a random value.
        ///// </summary>
        ///// <typeparam name="T">The set type.</typeparam>
        ///// <param name="set">The set to get the value from.</param>
        ///// <returns>default(T) if the set.Count is 0 or something bad happened. The random object otherwise.</returns>
        //public static T RandomValue<T>(this ISet<T> set)
        //{
        //    return RandomValue(set, new Random());
        //}

        #endregion Set


        #region Set Excluding

        /// <summary>
        /// Gets a random value.
        /// </summary>
        /// <typeparam name="T">The set type. Must implement <see cref="IEquatable{T}"/>.</typeparam>
        /// <param name="set">The set to get the value from.</param>
        /// <param name="random">The random object to use.</param>
        /// <param name="excluding_obj">The T object to exclude from the search.</param>
        /// <returns>default(T) if the set.Count is 0 or something bad happened. The random object otherwise.</returns>
        public static T RandomValue<T>(this ISet<T> set, Random random, T excluding_obj)
        {
            if (set.Count == 0)
            {
                return default;
            }


            IEnumerable<T> exclusion = set.Where(x => !Equals(x, excluding_obj));

            int index = random.Next(set.Count);

            int i = 0;
            foreach (T obj in exclusion)
            {
                if (i++ == index)
                {
                    return obj;
                }
            }

            return default;
        }


        ///// <summary>
        ///// Gets a random value.
        ///// </summary>
        ///// <typeparam name="T">The set type. Must implement <see cref="IEquatable{T}"/>.</typeparam>
        ///// <param name="set">The set to get the value from.</param>
        ///// <param name="excluding_obj">The T object to exclude from the search.</param>
        ///// <returns>default(T) if the set.Count is 0 or something bad happened. The random object otherwise.</returns>
        //public static T RandomValue<T>(this ISet<T> set, T excluding_obj)
        //{
        //    return RandomValue(set, new Random(), excluding_obj);
        //}

        #endregion Set Excluding

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
