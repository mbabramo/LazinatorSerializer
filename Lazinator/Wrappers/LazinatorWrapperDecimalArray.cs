using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperDecimalArray : ILazinatorWrapperDecimalArray
    {
        public bool HasValue => Value != null;

        public LazinatorWrapperDecimalArray(decimal[] x) : this()
        {
            Value = x;
        }

        public static implicit operator LazinatorWrapperDecimalArray(decimal[] x)
        {
            return new LazinatorWrapperDecimalArray(x);
        }

        public static implicit operator decimal[](LazinatorWrapperDecimalArray x)
        {
            return x.Value;
        }
    }
}
