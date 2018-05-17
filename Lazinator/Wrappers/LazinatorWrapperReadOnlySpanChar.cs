using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperReadOnlySpanChar : ILazinatorWrapperReadOnlySpanChar
    {
        public bool HasValue => Value != null;

        public LazinatorWrapperReadOnlySpanChar(ReadOnlySpan<char> x) : this()
        {
            Value = x;
        }

        public static implicit operator LazinatorWrapperReadOnlySpanChar(ReadOnlySpan<char> x)
        {
            return new LazinatorWrapperReadOnlySpanChar(x);
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