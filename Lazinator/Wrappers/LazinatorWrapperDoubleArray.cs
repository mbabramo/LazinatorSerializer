using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperDoubleArray : ILazinatorWrapperDoubleArray
    {
        public bool HasValue => WrappedValue != null;

        public LazinatorWrapperDoubleArray(double[] x) : this()
        {
            WrappedValue = x;
        }

        public static implicit operator LazinatorWrapperDoubleArray(double[] x)
        {
            return new LazinatorWrapperDoubleArray(x);
        }

        public static implicit operator double[] (LazinatorWrapperDoubleArray x)
        {
            return x.WrappedValue;
        }
    }
}
