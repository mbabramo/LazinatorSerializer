using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperNullableFloat : ILazinatorWrapperNullableFloat
    {
        public static implicit operator LazinatorWrapperNullableFloat(float? x)
        {
            return new LazinatorWrapperNullableFloat() { Value = x };
        }

        public static implicit operator float? (LazinatorWrapperNullableFloat x)
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
            var other = (LazinatorWrapperNullableFloat)obj;
            return Equals(Value, other.Value);
        }
    }
}