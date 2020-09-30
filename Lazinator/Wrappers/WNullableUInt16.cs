namespace Lazinator.Wrappers
{
    /// <summary>
    /// A Lazinator wrapper for a nullable unsigned short. A wrapper can be used as a generic type where the unwrapped object cannot be.
    /// </summary>
    public partial struct WNullableUInt16 : IWNullableUInt16
    {
        public bool HasValue => WrappedValue != null;

        public WNullableUInt16(ushort? x) : this()
        {
            WrappedValue = x;
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static implicit operator WNullableUInt16(ushort? x)
        {
            return new WNullableUInt16(x);
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static implicit operator ushort? (WNullableUInt16 x)
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
            if (obj is WNullableUInt16 w)
                return WrappedValue == w.WrappedValue;
            if (obj is ushort v)
                return WrappedValue == v;
            if (obj == null)
                return WrappedValue == null;
            return false;
        }
    }
}