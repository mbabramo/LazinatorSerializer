using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperDecimal : ILazinatorWrapperDecimal, IComparable
    {
        public static implicit operator LazinatorWrapperDecimal(decimal x)
        {
            return new LazinatorWrapperDecimal() { Value = x };
        }

        public static implicit operator decimal(LazinatorWrapperDecimal x)
        {
            return x.Value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var other = (LazinatorWrapperDecimal)obj;
            return Value == other.Value;
        }

        public int CompareTo(object obj)
        {
            return Value.CompareTo(obj);
        }
    }
}
