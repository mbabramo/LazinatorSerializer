using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperInt : ILazinatorWrapperInt, IComparable
    {
        public static implicit operator LazinatorWrapperInt(int x)
        {
            return new LazinatorWrapperInt() { Value = x };
        }

        public static implicit operator int(LazinatorWrapperInt x)
        {
            return x.Value;
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

        public int CompareTo(object obj)
        {
            return Value.CompareTo(obj);
        }
    }
}
