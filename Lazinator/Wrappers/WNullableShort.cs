namespace Lazinator.Wrappers
{
    public partial struct WNullableShort : IWNullableShort
    {
        public bool HasValue => WrappedValue != null;

        public WNullableShort(short? x) : this()
        {
            WrappedValue = x;
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static implicit operator WNullableShort(short? x)
        {
            return new WNullableShort(x);
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static implicit operator short? (WNullableShort x)
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
            if (obj is WNullableShort w)
                return WrappedValue == w.WrappedValue;
            if (obj is short v)
                return WrappedValue == v;
            if (obj == null)
                return WrappedValue == null;
            return false;
        }
    }
}