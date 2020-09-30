namespace Lazinator.Wrappers
{
    /// <summary>
    /// A Lazinator wrapper for a nullable int. A wrapper can be used as a generic type where the unwrapped object cannot be.
    /// </summary>
    public partial struct WNullableInt32 : IWNullableInt32
    {
        public bool HasValue => WrappedValue != null;

        public WNullableInt32(int? x) : this()
        {
            WrappedValue = x;
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static implicit operator WNullableInt32(int? x)
        {
            return new WNullableInt32(x);
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static implicit operator int? (WNullableInt32 x)
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
            if (obj is WNullableInt32 w)
                return WrappedValue == w.WrappedValue;
            if (obj is int v)
                return WrappedValue == v;
            if (obj == null)
                return WrappedValue == null;
            return false;
        }
    }
}