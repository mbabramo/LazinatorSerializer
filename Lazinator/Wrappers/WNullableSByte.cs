namespace Lazinator.Wrappers
{
    /// <summary>
    /// A Lazinator wrapper for a nullable signed byte. A wrapper can be used as a generic type where the unwrapped object cannot be.
    /// </summary>
    public partial struct WNullableSByte : IWNullableSByte
    {
        public bool HasValue => WrappedValue != null;

        public WNullableSByte(sbyte? x) : this()
        {
            WrappedValue = x;
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static implicit operator WNullableSByte(sbyte? x)
        {
            return new WNullableSByte(x);
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static implicit operator sbyte? (WNullableSByte x)
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
            if (obj is WNullableSByte w)
                return WrappedValue == w.WrappedValue;
            if (obj is sbyte v)
                return WrappedValue == v;
            if (obj == null)
                return WrappedValue == null;
            return false;
        }
    }
}