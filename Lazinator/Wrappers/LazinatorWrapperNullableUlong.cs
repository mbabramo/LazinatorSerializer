using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperNullableUlong : ILazinatorWrapperNullableUlong
    {
        public bool HasValue => WrappedValue != null;

        public LazinatorWrapperNullableUlong(ulong? x) : this()
        {
            WrappedValue = x;
        }

        public static implicit operator LazinatorWrapperNullableUlong(ulong? x)
        {
            return new LazinatorWrapperNullableUlong(x);
        }

        public static implicit operator ulong? (LazinatorWrapperNullableUlong x)
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
            if (obj is LazinatorWrapperNullableUlong w)
                return WrappedValue == w.WrappedValue;
            if (obj is ulong v)
                return WrappedValue == v;
            if (obj == null)
                return WrappedValue == null;
            return false;
        }
    }
}