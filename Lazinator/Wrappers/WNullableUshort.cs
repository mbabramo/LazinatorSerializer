using System;

namespace Lazinator.Wrappers
{
    public partial struct WNullableUshort : IWNullableUshort
    {
        public bool HasValue => WrappedValue != null;

        public WNullableUshort(ushort? x) : this()
        {
            WrappedValue = x;
        }

        public static implicit operator WNullableUshort(ushort? x)
        {
            return new WNullableUshort(x);
        }

        public static implicit operator ushort? (WNullableUshort x)
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
            if (obj is WNullableUshort w)
                return WrappedValue == w.WrappedValue;
            if (obj is ushort v)
                return WrappedValue == v;
            if (obj == null)
                return WrappedValue == null;
            return false;
        }
    }
}