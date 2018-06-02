using System;

namespace Lazinator.Wrappers
{
    public partial struct WNullableUlong : IWNullableUlong
    {
        public bool HasValue => WrappedValue != null;

        public WNullableUlong(ulong? x) : this()
        {
            WrappedValue = x;
        }

        public static implicit operator WNullableUlong(ulong? x)
        {
            return new WNullableUlong(x);
        }

        public static implicit operator ulong? (WNullableUlong x)
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
            if (obj is WNullableUlong w)
                return WrappedValue == w.WrappedValue;
            if (obj is ulong v)
                return WrappedValue == v;
            if (obj == null)
                return WrappedValue == null;
            return false;
        }
    }
}