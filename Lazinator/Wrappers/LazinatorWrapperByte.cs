using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperByte : ILazinatorWrapperByte
    {
        public static implicit operator LazinatorWrapperByte(byte x)
        {
            return new LazinatorWrapperByte() {Value = x};
        }

        public static implicit operator byte(LazinatorWrapperByte x)
        {
            return x.Value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var other = (LazinatorWrapperByte)obj;
            return Value == other.Value;
        }
    }
}
