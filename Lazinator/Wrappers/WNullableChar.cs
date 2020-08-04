namespace Lazinator.Wrappers
{
    /// <summary>
    /// A Lazinator wrapper for a nullable char. A wrapper can be used as a generic type where the unwrapped object cannot be.
    /// </summary>
    public partial struct WNullableChar : IWNullableChar
    {
        public bool HasValue => WrappedValue != null;

        public WNullableChar(char? x) : this()
        {
            WrappedValue = x;
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static implicit operator WNullableChar(char? x)
        {
            return new WNullableChar(x);
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static implicit operator char? (WNullableChar x)
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
            if (obj is WNullableChar w)
                return WrappedValue == w.WrappedValue;
            if (obj is char v)
                return WrappedValue == v;
            if (obj == null)
                return WrappedValue == null;
            return false;
        }
    }
}