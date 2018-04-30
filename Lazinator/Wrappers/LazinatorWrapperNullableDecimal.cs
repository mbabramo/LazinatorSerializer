using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperNullableDecimal : ILazinatorWrapperNullableDecimal
    {
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