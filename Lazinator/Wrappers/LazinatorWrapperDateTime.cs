using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperDateTime : ILazinatorWrapperDateTime, IComparable
    {
        public static implicit operator LazinatorWrapperDateTime(DateTime x)
        {
            return new LazinatorWrapperDateTime() { Value = x };
        }

        public static implicit operator DateTime(LazinatorWrapperDateTime x)
        {
            return x.Value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var other = (LazinatorWrapperDateTime)obj;
            return Value == other.Value;
        }

        public int CompareTo(object obj)
        {
            return Value.CompareTo(obj);
        }
    }
}
