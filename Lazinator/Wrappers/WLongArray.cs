namespace Lazinator.Wrappers
{
    public partial struct WLongArray : IWLongArray
    {
        public bool HasValue => WrappedValue != null;

        public WLongArray(long[] x) : this()
        {
            WrappedValue = x;
        }

        public static implicit operator WLongArray(long[] x)
        {
            return new WLongArray(x);
        }

        public static implicit operator long[] (WLongArray x)
        {
            return x.WrappedValue;
        }
    }
}
