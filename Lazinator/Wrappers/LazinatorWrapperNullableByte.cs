using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperNullableByte : ILazinatorWrapperNullableByte
    {
        public bool HasValue => WrappedValue != null;

        public LazinatorWrapperNullableByte(byte? x) : this()
        {
            WrappedValue = x;
        }

        public static implicit operator LazinatorWrapperNullableByte(byte? x)
        {
            return new LazinatorWrapperNullableByte(x);
        }

        public static implicit operator byte? (LazinatorWrapperNullableByte x)
        {
            return x.WrappedValue;
        }

        public override string ToString()
        {
            return WrappedValue?.ToString() ?? "";
        }

        public override int GetHashCode()
        {
            return WrappedValue?.GetHashCode() ?? 0;
        }

        public override bool Equals(object obj)
        {
            if (obj is LazinatorWrapperNullableByte w)
                return WrappedValue == w.WrappedValue;
            if (obj is byte v)
                return WrappedValue == v;
            if (obj == null)
                return WrappedValue == null;
            return false;
        }
    }
}