using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperFloat : ILazinatorWrapperFloat
    {
        public static implicit operator LazinatorWrapperFloat(float x)
        {
            return new LazinatorWrapperFloat() { Value = x };
        }

        public static implicit operator float(LazinatorWrapperFloat x)
        {
            return x.Value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var other = (LazinatorWrapperFloat)obj;
            return Value == other.Value;
        }
    }
}
