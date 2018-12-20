namespace Lazinator.Wrappers
{
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
