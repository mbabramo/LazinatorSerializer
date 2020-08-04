namespace Lazinator.Wrappers
{
    /// <summary>
    /// A Lazinator wrapper for an array of longs. A wrapper can be used as a generic type where the unwrapped object cannot be.
    /// </summary>
    public partial struct WLongArray : IWLongArray
    {
        public bool HasValue => WrappedValue != null;

        public WLongArray(long[] x) : this()
        {
            WrappedValue = x;
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static implicit operator WLongArray(long[] x)
        {
            return new WLongArray(x);
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static implicit operator long[] (WLongArray x)
        {
            return x.WrappedValue;
        }
    }
}
