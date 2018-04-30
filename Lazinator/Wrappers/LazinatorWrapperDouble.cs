using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperDouble : ILazinatorWrapperDouble
    {
        public static implicit operator LazinatorWrapperDouble(double x)
        {
            return new LazinatorWrapperDouble() { Value = x };
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var other = (LazinatorWrapperDouble)obj;
            return Value == other.Value;
        }
    }
}
