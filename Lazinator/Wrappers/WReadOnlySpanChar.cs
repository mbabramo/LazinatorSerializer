using System;

namespace Lazinator.Wrappers
{
    /// <summary>
    /// A Lazinator wrapper for a read only span of characters. A wrapper can be used as a generic type where the unwrapped object cannot be.
    /// </summary>
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