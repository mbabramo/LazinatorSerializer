using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperLong : ILazinatorWrapperLong
    {
        public static implicit operator LazinatorWrapperLong(long x)
        {
            return new LazinatorWrapperLong() { Value = x };
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var other = (LazinatorWrapperLong)obj;
            return Value == other.Value;
        }
    }
}
