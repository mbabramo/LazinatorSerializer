using System;

namespace Lazinator.Buffers
{
    /// <summary>
    /// Determines whether ReadOnlySpans match and if not where the match fails.
    /// </summary>
    public static class ReadOnlySpanMatching
    {

        public static bool Matches(this ReadOnlySpan<byte> one, ReadOnlySpan<byte> other)
        {
            if (other.Length != one.Length)
                return false;

            return System.MemoryExtensions.SequenceEqual<byte>(one, other);
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
