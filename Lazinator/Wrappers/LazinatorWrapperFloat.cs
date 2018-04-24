using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperFloat : ILazinatorWrapperFloat
    {
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
