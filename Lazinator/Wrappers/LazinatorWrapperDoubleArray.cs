using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperDoubleArray : ILazinatorWrapperDoubleArray
    {
        public bool HasValue => Value != null;

        public static implicit operator LazinatorWrapperDoubleArray(double[] x)
        {
            return new LazinatorWrapperDoubleArray() { Value = x };
        }

        public static implicit operator double[] (LazinatorWrapperDoubleArray x)
        {
            return x.Value;
        }
    }
}
