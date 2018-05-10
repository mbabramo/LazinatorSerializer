using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperReadOnlySpanChar : ILazinatorWrapperReadOnlySpanChar
    {
        public bool HasValue => Value != null;

        public static implicit operator LazinatorWrapperReadOnlySpanChar(ReadOnlySpan<char> x)
        {
            return new LazinatorWrapperReadOnlySpanChar() { Value = x };
        }

        public static implicit operator ReadOnlySpan<char>(LazinatorWrapperReadOnlySpanChar x)
        {
            return x.Value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }


    }
}