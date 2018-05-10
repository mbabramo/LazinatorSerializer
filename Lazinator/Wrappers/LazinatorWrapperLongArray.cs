using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperLongArray : ILazinatorWrapperLongArray
    {
        public bool HasValue => Value != null;

        public static implicit operator LazinatorWrapperLongArray(long[] x)
        {
            return new LazinatorWrapperLongArray() { Value = x };
        }

        public static implicit operator long[] (LazinatorWrapperLongArray x)
        {
            return x.Value;
        }
    }
}
