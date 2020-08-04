using System;

namespace Lazinator.Wrappers
{
    /// <summary>
    /// A Lazinator wrapper for a nullable DateTime. A wrapper can be used as a generic type where the unwrapped object cannot be.
    /// </summary>
    public partial struct WNullableDateTime : IWNullableDateTime
    {
        public bool HasValue => WrappedValue != null;

        public WNullableDateTime(DateTime? x) : this()
        {
            WrappedValue = x;
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static implicit operator WNullableDateTime(DateTime? x)
        {
            return new WNullableDateTime(x);
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static implicit operator DateTime? (WNullableDateTime x)
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
            if (obj is WNullableDateTime w)
                return WrappedValue == w.WrappedValue;
            if (obj is DateTime v)
                return WrappedValue == v;
            if (obj == null)
                return WrappedValue == null;
            return false;
        }
    }
}