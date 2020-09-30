namespace Lazinator.Wrappers
{
    /// <summary>
    /// A Lazinator wrapper for a nullable short. A wrapper can be used as a generic type where the unwrapped object cannot be.
    /// </summary>
    public partial struct WNullableInt16 : IWNullableInt16
    {
        public bool HasValue => WrappedValue != null;

        public WNullableInt16(short? x) : this()
        {
            WrappedValue = x;
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static implicit operator WNullableInt16(short? x)
        {
            return new WNullableInt16(x);
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static implicit operator short? (WNullableInt16 x)
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
            if (obj is WNullableInt16 w)
                return WrappedValue == w.WrappedValue;
            if (obj is short v)
                return WrappedValue == v;
            if (obj == null)
                return WrappedValue == null;
            return false;
        }
    }
}