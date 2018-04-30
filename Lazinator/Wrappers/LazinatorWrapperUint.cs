using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperUint : ILazinatorWrapperUint
    {
        public static implicit operator LazinatorWrapperUint(uint x)
        {
            return new LazinatorWrapperUint() { Value = x };
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var other = (LazinatorWrapperUint)obj;
            return Value == other.Value;
        }
    }
}
