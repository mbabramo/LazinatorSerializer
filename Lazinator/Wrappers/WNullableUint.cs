using System;

namespace Lazinator.Wrappers
{
    public partial struct WNullableUint : IWNullableUint
    {
        public bool HasValue => WrappedValue != null;

        public WNullableUint(uint? x) : this()
        {
            WrappedValue = x;
        }

        public static implicit operator WNullableUint(uint? x)
        {
            return new WNullableUint(x);
        }

        public static implicit operator uint? (WNullableUint x)
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
            if (obj is WNullableUint w)
                return WrappedValue == w.WrappedValue;
            if (obj is uint v)
                return WrappedValue == v;
            if (obj == null)
                return WrappedValue == null;
            return false;
        }
    }
}