using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperNullableDateTime : ILazinatorWrapperNullableDateTime
    {
        public static implicit operator LazinatorWrapperNullableDateTime(DateTime? x)
        {
            return new LazinatorWrapperNullableDateTime() { Value = x };
        }

        public static implicit operator DateTime? (LazinatorWrapperNullableDateTime x)
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
            var other = (LazinatorWrapperNullableDateTime)obj;
            return Equals(Value, other.Value);
        }
    }
}