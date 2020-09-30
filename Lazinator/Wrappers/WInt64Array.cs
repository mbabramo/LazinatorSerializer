namespace Lazinator.Wrappers
{
    /// <summary>
    /// A Lazinator wrapper for an array of longs. A wrapper can be used as a generic type where the unwrapped object cannot be.
    /// </summary>
    public partial struct WInt64Array : IWInt64Array
    {
        public bool HasValue => WrappedValue != null;

        public WInt64Array(long[] x) : this()
        {
            WrappedValue = x;
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static implicit operator WInt64Array(long[] x)
        {
            return new WInt64Array(x);
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static implicit operator long[] (WInt64Array x)
        {
            return x.WrappedValue;
        }
    }
}
