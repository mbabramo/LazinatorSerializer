using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperNullableTimeSpan : ILazinatorWrapperNullableTimeSpan
    {
        public static implicit operator LazinatorWrapperNullableTimeSpan(TimeSpan? x)
        {
            return new LazinatorWrapperNullableTimeSpan() { Value = x };
        }

        public static implicit operator TimeSpan? (LazinatorWrapperNullableTimeSpan x)
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
            var other = (LazinatorWrapperNullableTimeSpan)obj;
            return Equals(Value, other.Value);
        }
    }
}