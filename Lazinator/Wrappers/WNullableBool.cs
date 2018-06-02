using System;

namespace Lazinator.Wrappers
{
    public partial struct WNullableBool : IWNullableBool
    {
        public bool HasValue => WrappedValue != null;

        public WNullableBool(bool? x) : this()
        {
            WrappedValue = x;
        }

        public override string ToString()
        {
            return WrappedValue?.ToString() ?? "";
        }

        public static implicit operator WNullableBool(bool? x)
        {
            return new WNullableBool(x);
        }

        public static implicit operator bool?(WNullableBool x)
        {
            return x.WrappedValue;
        }

        public override int GetHashCode()
        {
            return WrappedValue?.GetHashCode() ?? 0;
        }

        public override bool Equals(object obj)
        {
            if (obj is WNullableBool w)
                return WrappedValue == w.WrappedValue;
            if (obj is bool v)
                return WrappedValue == v;
            if (obj == null)
                return WrappedValue == null;
            return false;
        }
    }
}