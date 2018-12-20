namespace Lazinator.Wrappers
{
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
