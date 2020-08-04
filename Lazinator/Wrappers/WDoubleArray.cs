namespace Lazinator.Wrappers
{
    /// <summary>
    /// A Lazinator wrapper for an array of doubles. A wrapper can be used as a generic type where the unwrapped object cannot be.
    /// </summary>
    public partial struct WDoubleArray : IWDoubleArray
    {
        public bool HasValue => WrappedValue != null;

        public WDoubleArray(double[] x) : this()
        {
            WrappedValue = x;
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static implicit operator WDoubleArray(double[] x)
        {
            return new WDoubleArray(x);
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static implicit operator double[] (WDoubleArray x)
        {
            return x.WrappedValue;
        }
    }
}
