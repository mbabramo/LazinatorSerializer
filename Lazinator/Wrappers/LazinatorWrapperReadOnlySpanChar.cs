using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperReadOnlySpanChar : ILazinatorWrapperReadOnlySpanChar
    {
        public static implicit operator LazinatorWrapperReadOnlySpanChar(ReadOnlySpan<char> x)
        {
            return new LazinatorWrapperReadOnlySpanChar() { Value = x };
        }
    }
}