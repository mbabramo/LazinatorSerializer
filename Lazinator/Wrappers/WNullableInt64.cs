namespace Lazinator.Wrappers
{
    /// <summary>
    /// A Lazinator wrapper for a nullable long. A wrapper can be used as a generic type where the unwrapped object cannot be.
    /// </summary>
    public partial struct WNullableInt64 : IWNullableInt64
    {
        public bool HasValue => WrappedValue != null;

        public WNullableInt64(long? x) : this()
        {
            WrappedValue = x;
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static implicit operator WNullableInt64(long? x)
        {
            return new WNullableInt64(x);
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static implicit operator long? (WNullableInt64 x)
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
            if (obj is WNullableInt64 w)
                return WrappedValue == w.WrappedValue;
            if (obj is long v)
                return WrappedValue == v;
            if (obj == null)
                return WrappedValue == null;
            return false;
        }
    }
}