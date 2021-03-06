﻿namespace Lazinator.Wrappers
{
    /// <summary>
    /// A Lazinator wrapper for a nullable double. A wrapper can be used as a generic type where the unwrapped object cannot be.
    /// </summary>
    public partial struct WNullableDouble : IWNullableDouble
    {
        public bool HasValue => WrappedValue != null;

        public WNullableDouble(double? x) : this()
        {
            WrappedValue = x;
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static implicit operator WNullableDouble(double? x)
        {
            return new WNullableDouble(x);
        }

        [System.Diagnostics.DebuggerStepThrough]
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