using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperIntArray : ILazinatorWrapperIntArray
    {
        public bool HasValue => WrappedValue != null;

        public LazinatorWrapperIntArray(int[] x) : this()
        {
            WrappedValue = x;
        }

        public static implicit operator LazinatorWrapperIntArray(int[] x)
        {
            return new LazinatorWrapperIntArray(x);
        }

        public static implicit operator int[](LazinatorWrapperIntArray x)
        {
            return x.WrappedValue;
        }
    }
}
