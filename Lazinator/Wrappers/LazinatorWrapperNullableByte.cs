using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperNullableByte : ILazinatorWrapperNullableByte
    {
        public static implicit operator LazinatorWrapperNullableByte(byte? x)
        {
            return new LazinatorWrapperNullableByte() { Value = x };
        }

        public static implicit operator byte? (LazinatorWrapperNullableByte x)
        {
            return x.Value;
        }

        public override int GetHashCode()
        {
            return Value?.GetHashCode() ?? 0;
        }

        public override bool Equals(object obj)
        {
            var other = (LazinatorWrapperNullableByte)obj;
            return Equals(Value, other.Value);
        }
    }
}