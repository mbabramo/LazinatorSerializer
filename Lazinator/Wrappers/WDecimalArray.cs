namespace Lazinator.Wrappers
{
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
