using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperNullableShort : ILazinatorWrapperNullableShort
    {
        public static implicit operator LazinatorWrapperNullableShort(short? x)
        {
            return new LazinatorWrapperNullableShort() { Value = x };
        }

        public static implicit operator short? (LazinatorWrapperNullableShort x)
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
            var other = (LazinatorWrapperNullableShort)obj;
            return Equals(Value, other.Value);
        }
    }
}