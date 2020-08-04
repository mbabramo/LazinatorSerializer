namespace Lazinator.Wrappers
{
    /// <summary>
    /// A Lazinator wrapper for a nullable int. A wrapper can be used as a generic type where the unwrapped object cannot be.
    /// </summary>
    public partial struct WNullableInt : IWNullableInt
    {
        public bool HasValue => WrappedValue != null;

        public WNullableInt(int? x) : this()
        {
            WrappedValue = x;
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static implicit operator WNullableInt(int? x)
        {
            return new WNullableInt(x);
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static implicit operator int? (WNullableInt x)
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
            if (obj is WNullableInt w)
                return WrappedValue == w.WrappedValue;
            if (obj is int v)
                return WrappedValue == v;
            if (obj == null)
                return WrappedValue == null;
            return false;
        }
    }
}