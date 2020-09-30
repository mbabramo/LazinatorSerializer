namespace Lazinator.Wrappers
{
    /// <summary>
    /// A Lazinator wrapper for a nullable unsigned int. A wrapper can be used as a generic type where the unwrapped object cannot be.
    /// </summary>
    public partial struct WNullableUInt32 : IWNullableUInt32
    {
        public bool HasValue => WrappedValue != null;

        public WNullableUInt32(uint? x) : this()
        {
            WrappedValue = x;
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static implicit operator WNullableUInt32(uint? x)
        {
            return new WNullableUInt32(x);
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static implicit operator uint? (WNullableUInt32 x)
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
            if (obj is WNullableUInt32 w)
                return WrappedValue == w.WrappedValue;
            if (obj is uint v)
                return WrappedValue == v;
            if (obj == null)
                return WrappedValue == null;
            return false;
        }
    }
}