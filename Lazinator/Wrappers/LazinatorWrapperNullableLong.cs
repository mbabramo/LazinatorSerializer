using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperNullableLong : ILazinatorWrapperNullableLong
    {
        public static implicit operator LazinatorWrapperNullableLong(long? x)
        {
            return new LazinatorWrapperNullableLong() { Value = x };
        }

        public static implicit operator long? (LazinatorWrapperNullableLong x)
        {
            return x.Value;
        }

        public override int GetHashCode()
        {
            return Value?.GetHashCode() ?? 0;
        }

        public override bool Equals(object obj)
        {
            var other = (LazinatorWrapperNullableLong)obj;
            return Equals(Value, other.Value);
        }
    }
}