using System;

namespace Lazinator.Wrappers
{
    public partial struct WNullableGuid : IWNullableGuid
    {
        public bool HasValue => WrappedValue != null;

        public WNullableGuid(Guid? x) : this()
        {
            WrappedValue = x;
        }

        public static implicit operator WNullableGuid(Guid? x)
        {
            return new WNullableGuid(x);
        }

        public static implicit operator Guid? (WNullableGuid x)
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
            if (obj is WNullableGuid w)
                return WrappedValue == w.WrappedValue;
            if (obj is Guid v)
                return WrappedValue == v;
            if (obj == null)
                return WrappedValue == null;
            return false;
        }
    }
}