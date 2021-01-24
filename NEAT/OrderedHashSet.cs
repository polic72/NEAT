using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEAT
{
    /// <summary>
    /// Acts as a list with a hashed set next to it for a quick contains method. Only allows 1 of every object.
    /// </summary>
    /// <typeparam name="T">The objects being stored.</typeparam>
    public class OrderedHashSet<T> : IEnumerable<T>
    {
        private HashSet<T> set;
        private List<T> list;


        /// <summary>
        /// The internal random of the OrderedHashSet.
        /// </summary>
        public Random Random { get; }


        /// <summary>
        /// The number of elements in this OrderedHashSet.
        /// </summary>
        public int Count { get { return list.Count; } }


        #region Constructors

        /// <summary>
        /// Constructs an OrderedHashSet with the given random.
        /// </summary>
        /// <param name="random">The internal random for the OrderedHashSet.</param>
        public OrderedHashSet(Random random)
        {
            set = new HashSet<T>();
            list = new List<T>();

            Random = random;
        }


        /// <summary>
        /// Constructs an OrderedHashSet with the given random and capacity.
        /// </summary>
        /// <param name="random">The internal random for the OrderedHashSet.</param>
        /// <param name="capacity">The initial capacity.</param>
        public OrderedHashSet(Random random, int capacity)
        {
            set = new HashSet<T>(capacity);
            list = new List<T>(capacity);

            Random = random;
        }

        #endregion Constructors


        /// <summary>
        /// Whether or not this OrderedHashSet contains the given object.
        /// </summary>
        /// <param name="obj">The object to check.</param>
        /// <returns>True if this OrderedHashSet contains it, false otherwise.</returns>
        public bool Contains(T obj)
        {
            return set.Contains(obj);
        }


        /// <summary>
        /// Adds the given T to this OrderedHashSet.
        /// </summary>
        /// <param name="obj">The T object to add.</param>
        /// <returns>True if added, false if already contained.</returns>
        public bool Add(T obj)
        {
            if (!set.Contains(obj))
            {
                set.Add(obj);
                list.Add(obj);

                return true;
            }

            return false;
        }


        /// <summary>
        /// Removes the given T.
        /// </summary>
        /// <param name="obj">The T object to remove.</param>
        /// <returns>True if found and removed, false otherwise.</returns>
        public bool Remove(T obj)
        {
            return set.Remove(obj) && list.Remove(obj); //This is fine because they will always have the same objects in them.
        }


        /// <summary>
        /// Clears every object in this OrderedHashSet.
        /// </summary>
        public void Clear()
        {
            set.Clear();
            list.Clear();
        }


        #region IEnumerator<T> Implementation

        /// <summary>
        /// Returns an enumerator that iterates through the <see cref="NEAT.OrderedHashSet{T}"/>.
        /// </summary>
        /// <returns>A <see cref="System.Collections.Generic.List{T}.Enumerator"/>, that's how the internals work.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return list.GetEnumerator();
        }


        /// <summary>
        /// Required for <see cref="IEnumerable{T}"/>. Just look at <see cref="NEAT.OrderedHashSet{T}.GetEnumerator"/>.
        /// </summary>
        /// <returns>What <see cref="NEAT.OrderedHashSet{T}.GetEnumerator"/> returns.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion IEnumerator<T> Implementation


        /// <summary>
        /// Gets the T at the given index.
        /// </summary>
        /// <param name="index">The index to query.</param>
        /// <returns>The T at the given index, default(T) if not a valid index.</returns>
        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= list.Count)
                {
                    return default;
                }

                return list[index];
            }
        }
    }
}
