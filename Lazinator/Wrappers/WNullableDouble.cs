using System;

namespace Lazinator.Wrappers
{
    public partial struct WNullableDouble : IWNullableDouble
    {
        public bool HasValue => WrappedValue != null;

        public WNullableDouble(double? x) : this()
        {
            WrappedValue = x;
        }

        public static implicit operator WNullableDouble(double? x)
        {
            return new WNullableDouble(x);
        }

        public static implicit operator double? (WNullableDouble x)
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
            if (obj is WNullableDouble w)
                return WrappedValue == w.WrappedValue;
            if (obj is double v)
                return WrappedValue == v;
            if (obj == null)
                return WrappedValue == null;
            return false;
        }
    }
}