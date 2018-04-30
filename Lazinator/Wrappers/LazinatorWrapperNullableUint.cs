using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperNullableUint : ILazinatorWrapperNullableUint
    {
        public static implicit operator LazinatorWrapperNullableUint(uint? x)
        {
            return new LazinatorWrapperNullableUint() { Value = x };
        }

        public override int GetHashCode()
        {
            return Value?.GetHashCode() ?? 0;
        }

        public override bool Equals(object obj)
        {
            var other = (LazinatorWrapperNullableUint)obj;
            return Equals(Value, other.Value);
        }
    }
}