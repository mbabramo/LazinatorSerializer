using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperDecimalArray : ILazinatorWrapperDecimalArray
    {
        public bool HasValue => WrappedValue != null;

        public LazinatorWrapperDecimalArray(decimal[] x) : this()
        {
            WrappedValue = x;
        }

        public static implicit operator LazinatorWrapperDecimalArray(decimal[] x)
        {
            return new LazinatorWrapperDecimalArray(x);
        }

        public static implicit operator decimal[](LazinatorWrapperDecimalArray x)
        {
            return x.WrappedValue;
        }
    }
}
