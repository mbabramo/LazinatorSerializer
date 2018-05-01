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

        public override int GetHashCode()
        {
            return Value?.GetHashCode() ?? 0;
        }

        public override bool Equals(object obj)
        {
            var other = (LazinatorWrapperNullableDecimal)obj;
            return Equals(Value, other.Value);
        }
    }
}