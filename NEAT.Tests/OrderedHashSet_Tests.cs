using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace NEAT.Tests
{
    [TestClass]
    public class OrderedHashSet_Tests
    {
        #region Count

        [DataTestMethod]
        [DataRow(0)]
        [DataRow(1)]
        [DataRow(10)]
        public void Count(int num_objects)
        {
            Random random = new Random(1);

            OrderedHashSet<int> ohs = new OrderedHashSet<int>(random);


            for (int i = 0; i < num_objects; ++i)
            {
                ohs.Add(i);
            }


            Assert.AreEqual<int>(num_objects, ohs.Count);
        }


        [DataTestMethod]
        [DataRow(2, 1)]
        [DataRow(2, 2)]
        public void Count_Remove(int num_objects_start, int remove_num)
        {
            Random random = new Random(1);

            OrderedHashSet<int> ohs = new OrderedHashSet<int>(random);


            for (int i = 0; i < num_objects_start; ++i)
            {
                ohs.Add(i);
            }


            for (int i = 0; i < remove_num; ++i)
            {
                ohs.Remove(i);
            }


            Assert.AreEqual<int>(num_objects_start - remove_num, ohs.Count);
        }

        #endregion Count


        #region Contains

        [TestMethod]
        public void Contains_ReturnsTrue()
        {
            Random random = new Random(1);

            OrderedHashSet<string> ohs = new OrderedHashSet<string>(random);


            string s_1 = "hello";
            string s_2 = "world";

            ohs.Add(s_1);
            ohs.Add(s_2);


            Assert.IsTrue(ohs.Contains(s_1));
        }


        [TestMethod]
        public void Contains_ReturnsFalse()
        {
            Random random = new Random(1);

            OrderedHashSet<string> ohs = new OrderedHashSet<string>(random);


            string s_1 = "hello";
            string s_2 = "world";

            ohs.Add(s_1);


            Assert.IsFalse(ohs.Contains(s_2));
        }

        #endregion Contains


        #region Get

        [DataTestMethod]
        [DataRow(1, 0, 0)]
        [DataRow(3, 2, 2)]
        [DataRow(5, 2, 2)]
        [DataRow(0, 0, 0)] //default(int) = 0
        [DataRow(2, 9, 0)]
        public void Get(int num_objects, int position, int expected)
        {
            Random random = new Random(1);

            OrderedHashSet<int> ohs = new OrderedHashSet<int>(random);


            for (int i = 0; i < num_objects; ++i)
            {
                ohs.Add(i);
            }


            Assert.AreEqual(expected, ohs[position]);
        }

        #endregion Get


        //Remove among other things.
    }
}
