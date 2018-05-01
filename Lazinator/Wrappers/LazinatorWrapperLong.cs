using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperLong : ILazinatorWrapperLong, IComparable, IComparable<long>, IEquatable<long>, IComparable<LazinatorWrapperLong>, IEquatable<LazinatorWrapperLong>
    {
        public static implicit operator LazinatorWrapperLong(long x)
        {
            return new LazinatorWrapperLong() { Value = x };
        }

        public static implicit operator long(LazinatorWrapperLong x)
        {
            return x.Value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var other = (LazinatorWrapperLong)obj;
            return Value == other.Value;
        }

        public int CompareTo(object obj)
        {
            return Value.CompareTo(obj);
        }

        public int CompareTo(long other)
        {
            return Value.CompareTo(other);
        }

        public bool Equals(long other)
        {
            return Value.Equals(other);
        }

        public int CompareTo(LazinatorWrapperLong other)
        {
            return Value.CompareTo(other.Value);
        }

        public bool Equals(LazinatorWrapperLong other)
        {
            return Value.Equals(other.Value);
        }
    }
}
