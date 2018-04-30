using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperShort : ILazinatorWrapperShort
    {
        public static implicit operator LazinatorWrapperShort(short x)
        {
            return new LazinatorWrapperShort() { Value = x };
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var other = (LazinatorWrapperShort)obj;
            return Value == other.Value;
        }
    }
}
