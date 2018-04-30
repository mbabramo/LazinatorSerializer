using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperUshort : ILazinatorWrapperUshort
    {
        public static implicit operator LazinatorWrapperUshort(ushort x)
        {
            return new LazinatorWrapperUshort() { Value = x };
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var other = (LazinatorWrapperUshort)obj;
            return Value == other.Value;
        }
    }
}
