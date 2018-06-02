using System;

namespace Lazinator.Wrappers
{
    public partial struct WNullableDecimal : IWNullableDecimal
    {
        public bool HasValue => WrappedValue != null;

        public WNullableDecimal(decimal? x) : this()
        {
            WrappedValue = x;
        }

        public static implicit operator WNullableDecimal(decimal? x)
        {
            return new WNullableDecimal(x);
        }

        public static implicit operator decimal? (WNullableDecimal x)
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
            if (obj is WNullableDecimal w)
                return WrappedValue == w.WrappedValue;
            if (obj is decimal v)
                return WrappedValue == v;
            if (obj == null)
                return WrappedValue == null;
            return false;
        }
    }
}