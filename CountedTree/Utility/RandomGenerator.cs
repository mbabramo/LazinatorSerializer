using System;
using System.Collections.Generic;
using System.Linq;

namespace Utility
{
    public static class RandomGenerator
    {
        static Random r = null;
        public static int? _seedOverride = 8; // Change this to find bugs in tests that work most of the time but not always. We can set this to null to randomize by time and should do that when deploying to production, so that we can get real GUIDs and avoid duplicates. DEBUG
        public static int? SeedOverride { get { return _seedOverride; } set { r = null; _seedOverride = value; } }
        
        public static void Reset(int? overrideValue)
        {
            if (overrideValue != null && overrideValue != 0)
                SeedOverride = overrideValue;
            else if (SeedOverride != null)
                SeedOverride = SeedOverride + 1;
            r = null;
        }

        public static uint GetRandomUnsignedInt()
        {
            return (uint) GetRandom(0, Int32.MaxValue);
        }

        public static Guid GetGuid()
        {
            if (SeedOverride == null)
                return Guid.NewGuid(); // use official method of generating GUID
            else
                return RelativeGuids.FromLongs(new Tuple<long, long>(GetRandomLong(), GetRandomLong())); // use Rand as basis -- useful if SeedOverride is non null, so one can generate repeatable results for testing.
        }

        /// <summary> 
        /// Get a Random object which is cached between requests. 
        /// The advantage over creating the object locally is that the .Next 
        /// call will always return a new value. If creating several times locally 
        /// with a generated seed (like millisecond ticks), the same number can be 
        /// returned. 
        /// </summary> 
        /// <returns>A Random object which is cached between calls.</returns> 
        internal static Random GetRandomObject(int? seed)
        {
            if (r == null)
            {
                if (seed == null)
                    r = new Random();
                else
                    r = new Random((int)seed);
            }
            return r;
        }

        /// <summary> 
        /// GetRandom with no parameters. 
        /// </summary> 
        /// <returns>A Random object which is cached between calls.</returns> 
        internal static Random GetRandomObject()
        {
            if (SeedOverride == 0)
                throw new Exception("Seed override should not be zero. Set to null to use computer time to generate randomness.");
            return GetRandomObject(SeedOverride ?? (int)DateTime.Now.Ticks);
        }

        /// <summary> 
        /// Returns a double between 0 inclusive and 1 non-inclusive
        /// </summary> 
        /// <returns>A double between 0 and 1.</returns>
        public static double GetRandom()
        {
            Random r = GetRandomObject();
            lock (r)
            {
                return r.NextDouble();
            }
        }

        public static int GetRandomInt()
        {
            Random r = GetRandomObject();
            lock (r)
            {
                return r.Next();
            }
        }


        public static long GetRandomLong()
        {
            Random r = GetRandomObject();
            lock (r)
            {
                byte[] buf = new byte[8];
                r.NextBytes(buf);
                long longRand = BitConverter.ToInt64(buf, 0);
                return longRand;
            }
        }

        public static double GetRandom(double low, double high)
        {
            return low + GetRandom() * (high - low);
        }


        public static float GetRandom(float low, float high)
        {
            return low + ((float)GetRandom()) * (high - low);
        }

        public static decimal GetRandom(decimal low, decimal high, bool useRound=true)
        {
            var r = (decimal)GetRandom((double)low, (double)high);
            if (useRound)
            {
                return  Math.Round(r, 4);
            }
            else
            {
                return r;
            }
        }

        public static DateTime GetRandom(DateTime first, DateTime last)
        {
            int seconds = (int)((last - first).TotalSeconds);
            int randSeconds = GetRandom(0, seconds);
            return first + new TimeSpan(0, 0, randSeconds);
        }

        /// <summary> 
        /// GetRandom with two integer parameters. 
        /// </summary> 
        /// <returns>An int greater than or equal to low and less than or equal to high.</returns>
        public static int GetRandom(int low, int high)
        {
            if (high == Int32.MaxValue)
                high = Int32.MaxValue - 1;
            Random r = GetRandomObject();
            lock (r)
            {
                return r.Next(low, high + 1);
            }
        }

        public static T PickRandom<T>(IEnumerable<T> items)
        {
            List<T> itemsList = items.ToList();
            return itemsList[GetRandom(0, itemsList.Count() - 1)];
        }

        public static T PickRandom<T>(List<T> items)
        {
            return PickRandom<T>((IEnumerable<T>)items);
        }

        public static T PickRandom<T>(params T[] items)
        {
            return PickRandom<T>(items.ToList());
        }

        public static List<int> PickRandomInts(int min, int count, int numItems)
        {
            List<int> sourceList = Enumerable.Range(min, count).ToList();
            return GetRandomSample(sourceList, numItems).ToList();
        }

        public static List<T> PickRandomItems<T>(IEnumerable<T> items, int numItems, bool maintainOrder = false)
        {
            List<T> outputList = new List<T>();
            List<T> sourceList = items.ToList();
            List<int> randomInts = PickRandomInts(0, sourceList.Count(), numItems);
            if (maintainOrder)
                randomInts = randomInts.OrderBy(x => x).ToList();
            for (int i = 0; i < numItems; i++)
                outputList.Add(sourceList[randomInts[i]]);
            return outputList;
        }

        public static IEnumerable<T> GetRandomSample<T>(this IList<T> list, int sampleSize)
        {
            if (list == null) throw new ArgumentNullException("list");
            if (sampleSize > list.Count) throw new ArgumentException("sampleSize may not be greater than list count", "sampleSize");
            var indices = new Dictionary<int, int>(); int index;

            for (int i = 0; i < sampleSize; i++)
            {
                int j = GetRandom(i, list.Count - 1);
                if (!indices.TryGetValue(j, out index)) index = j;

                yield return list[index];

                if (!indices.TryGetValue(i, out index)) index = i;
                indices[j] = index;
            }
        }

        public static bool GetRandomBool()
        {
            return PickRandom(new bool[] { true, false });
        }
    }

    public static class ShuffleExtension
    {
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = RandomGenerator.GetRandom(0, n);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
