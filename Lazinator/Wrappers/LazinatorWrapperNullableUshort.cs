using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperNullableUshort : ILazinatorWrapperNullableUshort
    {
        public bool HasValue => WrappedValue != null;

        public LazinatorWrapperNullableUshort(ushort? x) : this()
        {
            WrappedValue = x;
        }

        public static implicit operator LazinatorWrapperNullableUshort(ushort? x)
        {
            return new LazinatorWrapperNullableUshort(x);
        }

        public static implicit operator ushort? (LazinatorWrapperNullableUshort x)
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
            if (obj is LazinatorWrapperNullableUshort w)
                return WrappedValue == w.WrappedValue;
            if (obj is ushort v)
                return WrappedValue == v;
            if (obj == null)
                return WrappedValue == null;
            return false;
        }
    }
}