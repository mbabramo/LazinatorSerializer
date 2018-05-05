using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperNullableDecimal : ILazinatorWrapperNullableDecimal
    {
        public static implicit operator LazinatorWrapperNullableDecimal(decimal? x)
        {
            return new LazinatorWrapperNullableDecimal() { Value = x };
        }

        public static implicit operator decimal? (LazinatorWrapperNullableDecimal x)
        {
            return x.Value;
        }

        public override string ToString()
        {
            return Value?.ToString() ?? "";
        }

        public override int GetHashCode()
        {
            return Value?.GetHashCode() ?? 0;
        }

        public override bool Equals(object obj)
        {
            if (obj is decimal v)
                return Value == v;
            else if (obj is LazinatorWrapperNullableDecimal w)
                return Value == w.Value;
            return false;
        }
    }
}