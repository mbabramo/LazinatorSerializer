namespace Lazinator.Wrappers
{
    public partial struct WDoubleArray : IWDoubleArray
    {
        public bool HasValue => WrappedValue != null;

        public WDoubleArray(double[] x) : this()
        {
            WrappedValue = x;
        }

        public static implicit operator WDoubleArray(double[] x)
        {
            return new WDoubleArray(x);
        }

        public static implicit operator double[] (WDoubleArray x)
        {
            return x.WrappedValue;
        }
    }
}
