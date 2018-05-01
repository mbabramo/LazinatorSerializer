using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperNullableDouble : ILazinatorWrapperNullableDouble
    {
        public static implicit operator LazinatorWrapperNullableDouble(double? x)
        {
            return new LazinatorWrapperNullableDouble() { Value = x };
        }

        public static implicit operator double? (LazinatorWrapperNullableDouble x)
        {
            return x.Value;
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