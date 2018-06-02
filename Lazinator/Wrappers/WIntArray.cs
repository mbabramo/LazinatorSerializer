using System;

namespace Lazinator.Wrappers
{
    public partial struct WIntArray : IWIntArray
    {
        public bool HasValue => WrappedValue != null;

        public WIntArray(int[] x) : this()
        {
            WrappedValue = x;
        }

        public static implicit operator WIntArray(int[] x)
        {
            return new WIntArray(x);
        }

        public static implicit operator int[](WIntArray x)
        {
            return x.WrappedValue;
        }
    }
}
