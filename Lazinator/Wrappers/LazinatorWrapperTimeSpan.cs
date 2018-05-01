using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperTimeSpan : ILazinatorWrapperTimeSpan, IComparable
    {
        public static implicit operator LazinatorWrapperTimeSpan(TimeSpan x)
        {
            return new LazinatorWrapperTimeSpan() { Value = x };
        }

        public static implicit operator TimeSpan(LazinatorWrapperTimeSpan x)
        {
            return x.Value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var other = (LazinatorWrapperTimeSpan)obj;
            return Value == other.Value;
        }

        public int CompareTo(object obj)
        {
            return ((IComparable)Value).CompareTo(obj);
        }
    }
}
