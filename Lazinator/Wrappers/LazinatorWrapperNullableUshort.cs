using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperNullableUshort : ILazinatorWrapperNullableUshort
    {
        public static implicit operator LazinatorWrapperNullableUshort(ushort? x)
        {
            return new LazinatorWrapperNullableUshort() { Value = x };
        }

        public static implicit operator ushort? (LazinatorWrapperNullableUshort x)
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
            var other = (LazinatorWrapperNullableUshort)obj;
            return Equals(Value, other.Value);
        }
    }
}