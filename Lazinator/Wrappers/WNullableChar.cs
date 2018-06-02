using System;

namespace Lazinator.Wrappers
{
    public partial struct WNullableChar : IWNullableChar
    {
        public bool HasValue => WrappedValue != null;

        public WNullableChar(char? x) : this()
        {
            WrappedValue = x;
        }

        public static implicit operator WNullableChar(char? x)
        {
            return new WNullableChar(x);
        }

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