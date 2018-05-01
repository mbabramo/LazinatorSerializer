using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperString : ILazinatorWrapperString, IComparable
    {
        public static implicit operator LazinatorWrapperString(string x)
        {
            return new LazinatorWrapperString() { Value = x };
        }

        public static implicit operator string(LazinatorWrapperString x)
        {
            return x.Value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var other = (LazinatorWrapperString)obj;
            return Value == other.Value;
        }

        public int CompareTo(object obj)
        {
            return Value.CompareTo(obj);
        }
    }
}