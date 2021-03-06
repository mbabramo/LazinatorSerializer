﻿namespace Lazinator.Wrappers
{
    /// <summary>
    /// A Lazinator wrapper for a nullable decimal. A wrapper can be used as a generic type where the unwrapped object cannot be.
    /// </summary>
    public partial struct WNullableDecimal : IWNullableDecimal
    {
        public bool HasValue => WrappedValue != null;

        public WNullableDecimal(decimal? x) : this()
        {
            WrappedValue = x;
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static implicit operator WNullableDecimal(decimal? x)
        {
            return new WNullableDecimal(x);
        }

        [System.Diagnostics.DebuggerStepThrough]
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