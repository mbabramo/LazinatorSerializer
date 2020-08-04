namespace Lazinator.Wrappers
{
    /// <summary>
    /// A Lazinator wrapper for an array of decimals. A wrapper can be used as a generic type where the unwrapped object cannot be.
    /// </summary>
    public partial struct WDecimalArray : IWDecimalArray
    {
        public bool HasValue => WrappedValue != null;

        public WDecimalArray(decimal[] x) : this()
        {
            WrappedValue = x;
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static implicit operator WDecimalArray(decimal[] x)
        {
            return new WDecimalArray(x);
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static implicit operator decimal[](WDecimalArray x)
        {
            return x.WrappedValue;
        }
    }
}
