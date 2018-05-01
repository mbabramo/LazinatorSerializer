using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperUlong : ILazinatorWrapperUlong
    {
        public static implicit operator LazinatorWrapperUlong(ulong x)
        {
            return new LazinatorWrapperUlong() { Value = x };
        }

        public static implicit operator ulong(LazinatorWrapperUlong x)
        {
            return x.Value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var other = (LazinatorWrapperUlong)obj;
            return Value == other.Value;
        }
    }
}
