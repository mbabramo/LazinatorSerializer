namespace Lazinator.Wrappers
{
    public partial struct WNullableLong : IWNullableLong
    {
        public bool HasValue => WrappedValue != null;

        public WNullableLong(long? x) : this()
        {
            WrappedValue = x;
        }

        public static implicit operator WNullableLong(long? x)
        {
            return new WNullableLong(x);
        }

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