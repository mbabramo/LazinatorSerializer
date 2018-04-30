using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperInt : ILazinatorWrapperInt
    {
        public static implicit operator LazinatorWrapperInt(int x)
        {
            return new LazinatorWrapperInt() { Value = x };
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var other = (LazinatorWrapperInt)obj;
            return Value == other.Value;
        }
    }
}
