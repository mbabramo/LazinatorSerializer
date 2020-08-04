namespace Lazinator.Wrappers
{
    /// <summary>
    /// A Lazinator wrapper for an array of ints. A wrapper can be used as a generic type where the unwrapped object cannot be.
    /// </summary>
    public partial struct WIntArray : IWIntArray
    {
        public bool HasValue => WrappedValue != null;

        public WIntArray(int[] x) : this()
        {
            WrappedValue = x;
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static implicit operator WIntArray(int[] x)
        {
            return new WIntArray(x);
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static implicit operator int[](WIntArray x)
        {
            return x.WrappedValue;
        }
    }
}
