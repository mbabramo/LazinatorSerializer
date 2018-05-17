using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperFloatArray : ILazinatorWrapperFloatArray
    {
        public bool HasValue => Value != null;

        public LazinatorWrapperFloatArray(float[] x) : this()
        {
            Value = x;
        }

        public static implicit operator LazinatorWrapperFloatArray(float[] x)
        {
            return new LazinatorWrapperFloatArray() { Value = x };
        }

        public static implicit operator float[] (LazinatorWrapperFloatArray x)
        {
            return x.Value;
        }
    }
}
