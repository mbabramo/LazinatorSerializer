using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperLongArray : ILazinatorWrapperLongArray
    {
        public bool HasValue => WrappedValue != null;

        public LazinatorWrapperLongArray(long[] x) : this()
        {
            WrappedValue = x;
        }

        public static implicit operator LazinatorWrapperLongArray(long[] x)
        {
            return new LazinatorWrapperLongArray(x);
        }

        public static implicit operator long[] (LazinatorWrapperLongArray x)
        {
            return x.WrappedValue;
        }
    }
}
