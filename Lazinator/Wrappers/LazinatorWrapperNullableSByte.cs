using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperNullableSByte : ILazinatorWrapperNullableSByte
    {
        public bool HasValue => WrappedValue != null;

        public LazinatorWrapperNullableSByte(sbyte? x) : this()
        {
            WrappedValue = x;
        }

        public static implicit operator LazinatorWrapperNullableSByte(sbyte? x)
        {
            return new LazinatorWrapperNullableSByte(x);
        }

        public static implicit operator sbyte? (LazinatorWrapperNullableSByte x)
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
            if (obj is LazinatorWrapperNullableSByte w)
                return WrappedValue == w.WrappedValue;
            if (obj is sbyte v)
                return WrappedValue == v;
            if (obj == null)
                return WrappedValue == null;
            return false;
        }
    }
}