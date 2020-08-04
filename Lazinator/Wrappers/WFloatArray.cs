namespace Lazinator.Wrappers
{
    /// <summary>
    /// A Lazinator wrapper for an array of floats. A wrapper can be used as a generic type where the unwrapped object cannot be.
    /// </summary>
    public partial struct WFloatArray : IWFloatArray
    {
        public bool HasValue => WrappedValue != null;

        public WFloatArray(float[] x) : this()
        {
            WrappedValue = x;
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static implicit operator WFloatArray(float[] x)
        {
            return new WFloatArray(x);
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static implicit operator float[] (WFloatArray x)
        {
            return x.WrappedValue;
        }
    }
}
