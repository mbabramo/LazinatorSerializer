using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Buffers
{
    public static class ReadOnlySpanMatching
    {

        public static bool Matches(this ReadOnlySpan<byte> one, ReadOnlySpan<byte> other)
        {
            if (other.Length != one.Length)
                return false;
            for (int i = 0; i < one.Length; i++)
                if (one[i] != other[i])
                    return false;
            return true;
        }

        public static int FindFirstNonMatch(this ReadOnlySpan<byte> one, ReadOnlySpan<byte> other)
        {
            if (other.Length != one.Length)
                return int.MaxValue;
            for (int i = 0; i < one.Length; i++)
                if (one[i] != other[i])
                    return i;
            return int.MaxValue;
        }
    }
}
