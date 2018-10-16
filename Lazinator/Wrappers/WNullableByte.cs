namespace Lazinator.Wrappers
{
    public partial struct WNullableByte : IWNullableByte
    {
        public bool HasValue => WrappedValue != null;

        public WNullableByte(byte? x) : this()
        {
            WrappedValue = x;
        }

        public static implicit operator WNullableByte(byte? x)
        {
            return new WNullableByte(x);
        }

        public static implicit operator byte? (WNullableByte x)
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
            if (obj is WNullableByte w)
                return WrappedValue == w.WrappedValue;
            if (obj is byte v)
                return WrappedValue == v;
            if (obj == null)
                return WrappedValue == null;
            return false;
        }
    }
}