using System;
using System.Linq;

namespace Utility
{
    /// <summary>
    /// This allows one to obtain deterministic, reversible IDs by combining Guid values. Note that because we do unchecked arithmetic within each half of the Guid, there are no guarantees that "adding" two Guids will produce a greater value. This is only a little bit faster than GetDeterministicGuid but is useful when we need to be able to reverse Guids.
    /// </summary>
    public static class RelativeGuids
    {
        const int guidLength = 16;

        public static Tuple<long, long> ToLongs(this Guid g)
        {
            byte[] b = g.ToByteArray();
            return new Tuple<long, long>(BitConverter.ToInt64(b, 0), BitConverter.ToInt64(b, 8));
        }

        public static Guid FromLongs(this Tuple<long, long> longs)
        {
            byte[] b = BitConverter.GetBytes(longs.Item1).Concat(BitConverter.GetBytes(longs.Item2)).ToArray();
            return new Guid(b);
        }

        public static Guid Reverse(this Guid A)
        {
            Tuple<long, long> longs = A.ToLongs();
            return FromLongs(new Tuple<long, long>(longs.Item2, longs.Item1)); // reverse order of longs
        }

        public static Guid Add(this Guid A, Guid B)
        {
            Tuple<long, long> a = A.ToLongs();
            Tuple<long, long> b = B.ToLongs();
            unchecked
            {
                return FromLongs(new Tuple<long, long>(a.Item1 + b.Item1, a.Item2 + b.Item2));
            }
        }

        public static Guid Subtract(this Guid A, Guid B)
        {
            Tuple<long, long> a = A.ToLongs();
            Tuple<long, long> b = B.ToLongs();
            unchecked
            {
                return FromLongs(new Tuple<long, long>(a.Item1 - b.Item1, a.Item2 - b.Item2));
            }
        }

        public static Guid Add(this Guid A, long B)
        {
            Tuple<long, long> a = A.ToLongs();
            Tuple<long, long> b = new Tuple<long, long>(0, B);
            unchecked
            {
                return FromLongs(new Tuple<long, long>(a.Item1 + b.Item1, a.Item2 + b.Item2));
            }
        }

        public static Guid Subtract(this Guid A, long B)
        {
            Tuple<long, long> a = A.ToLongs();
            Tuple<long, long> b = new Tuple<long, long>(0, B);
            unchecked
            {
                return FromLongs(new Tuple<long, long>(a.Item1 - b.Item1, a.Item2 - b.Item2));
            }
        }

        public static Guid Add(this Guid A, byte B)
        {
            return A.Add((long)B);
        }

        public static Guid Subtract(this Guid A, byte B)
        {
            return A.Subtract((long)B);
        }

        public static byte ConvertBackToByte(this Guid A)
        {
            return (byte) A.ToLongs().Item2;
        }


    }
}
