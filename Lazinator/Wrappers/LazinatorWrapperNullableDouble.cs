using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperNullableDouble : ILazinatorWrapperNullableDouble
    {
        public static implicit operator LazinatorWrapperNullableDouble(double? x)
        {
            return new LazinatorWrapperNullableDouble() { Value = x };
        }

        public override int GetHashCode()
        {
            return Value?.GetHashCode() ?? 0;
        }

        public override bool Equals(object obj)
        {
            var other = (LazinatorWrapperNullableDouble)obj;
            return Equals(Value, other.Value);
        }
    }
}