using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperFloatArray : ILazinatorWrapperFloatArray
    {
        public bool HasValue => WrappedValue != null;

        public LazinatorWrapperFloatArray(float[] x) : this()
        {
            WrappedValue = x;
        }

        public static implicit operator LazinatorWrapperFloatArray(float[] x)
        {
            return new LazinatorWrapperFloatArray(x);
        }

        public static implicit operator float[] (LazinatorWrapperFloatArray x)
        {
            return x.WrappedValue;
        }
    }
}
