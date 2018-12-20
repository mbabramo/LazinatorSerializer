namespace Lazinator.Wrappers
{
    public partial struct WNullableFloat : IWNullableFloat
    {
        public bool HasValue => WrappedValue != null;

        public WNullableFloat(float? x) : this()
        {
            WrappedValue = x;
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static implicit operator WNullableFloat(float? x)
        {
            return new WNullableFloat(x);
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static implicit operator float? (WNullableFloat x)
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
            if (obj is WNullableFloat w)
                return WrappedValue == w.WrappedValue;
            if (obj is float v)
                return WrappedValue == v;
            if (obj == null)
                return WrappedValue == null;
            return false;
        }
    }
}