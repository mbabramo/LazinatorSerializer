using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperNullableSByte : ILazinatorWrapperNullableSByte
    {
        public static implicit operator LazinatorWrapperNullableSByte(sbyte? x)
        {
            return new LazinatorWrapperNullableSByte() { Value = x };
        }

        public static implicit operator sbyte? (LazinatorWrapperNullableSByte x)
        {
            return x.Value;
        }

        public override int GetHashCode()
        {
            return Value?.GetHashCode() ?? 0;
        }

        public override bool Equals(object obj)
        {
            var other = (LazinatorWrapperNullableSByte)obj;
            return Equals(Value, other.Value);
        }
    }
}