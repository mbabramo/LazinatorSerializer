namespace Lazinator.Wrappers
{
    /// <summary>
    /// A Lazinator wrapper for a nullable long. A wrapper can be used as a generic type where the unwrapped object cannot be.
    /// </summary>
    public partial struct WNullableLong : IWNullableLong
    {
        public bool HasValue => WrappedValue != null;

        public WNullableLong(long? x) : this()
        {
            WrappedValue = x;
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static implicit operator WNullableLong(long? x)
        {
            return new WNullableLong(x);
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static implicit operator long? (WNullableLong x)
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
            if (obj is WNullableLong w)
                return WrappedValue == w.WrappedValue;
            if (obj is long v)
                return WrappedValue == v;
            if (obj == null)
                return WrappedValue == null;
            return false;
        }
    }
}