namespace Lazinator.Wrappers
{
    /// <summary>
    /// A Lazinator wrapper for an array of ints. A wrapper can be used as a generic type where the unwrapped object cannot be.
    /// </summary>
    public partial struct WInt32Array : IWInt32Array
    {
        public bool HasValue => WrappedValue != null;

        public WInt32Array(int[] x) : this()
        {
            WrappedValue = x;
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static implicit operator WInt32Array(int[] x)
        {
            return new WInt32Array(x);
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static implicit operator int[](WInt32Array x)
        {
            return x.WrappedValue;
        }
    }
}
