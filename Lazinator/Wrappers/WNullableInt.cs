namespace Lazinator.Wrappers
{
    public partial struct WNullableInt : IWNullableInt
    {
        public bool HasValue => WrappedValue != null;

        public WNullableInt(int? x) : this()
        {
            WrappedValue = x;
        }

        public static implicit operator WNullableInt(int? x)
        {
            return new WNullableInt(x);
        }

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