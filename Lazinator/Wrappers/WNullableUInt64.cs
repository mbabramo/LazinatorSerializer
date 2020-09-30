namespace Lazinator.Wrappers
{
    /// <summary>
    /// A Lazinator wrapper for a nullable unsigned long. A wrapper can be used as a generic type where the unwrapped object cannot be.
    /// </summary>
    public partial struct WNullableUInt64 : IWNullableUInt64
    {
        public bool HasValue => WrappedValue != null;

        public WNullableUInt64(ulong? x) : this()
        {
            WrappedValue = x;
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static implicit operator WNullableUInt64(ulong? x)
        {
            return new WNullableUInt64(x);
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static implicit operator ulong? (WNullableUInt64 x)
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
            if (obj is WNullableUInt64 w)
                return WrappedValue == w.WrappedValue;
            if (obj is ulong v)
                return WrappedValue == v;
            if (obj == null)
                return WrappedValue == null;
            return false;
        }
    }
}