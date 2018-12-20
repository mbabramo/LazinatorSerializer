using System;

namespace Lazinator.Wrappers
{
    public partial struct WReadOnlySpanChar : IWReadOnlySpanChar
    {
        public bool HasValue => Value != null;

        public WReadOnlySpanChar(ReadOnlySpan<char> x) : this()
        {
            Value = x;
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static implicit operator WReadOnlySpanChar(ReadOnlySpan<char> x)
        {
            return new WReadOnlySpanChar(x);
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static implicit operator ReadOnlySpan<char>(WReadOnlySpanChar x)
        {
            return x.Value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }


    }
}