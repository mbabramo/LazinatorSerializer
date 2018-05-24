using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperLong : ILazinatorWrapperLong, IComparable, IComparable<long>, IEquatable<long>, IComparable<LazinatorWrapperLong>, IEquatable<LazinatorWrapperLong>
    {
        public bool HasValue => true;

        public LazinatorWrapperLong(long x) : this()
        {
            Value = x;
        }

        public static implicit operator LazinatorWrapperLong(long x)
        {
            return new LazinatorWrapperLong(x);
        }

        public static implicit operator long(LazinatorWrapperLong x)
        {
            return x.Value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is long v)
                return Value == v;
            else if (obj is LazinatorWrapperLong w)
                return Value == w.Value;
            return false;
        }

        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;
            if (obj is LazinatorWrapperLong other)
                return CompareTo(other);
            if (obj is long b)
                return CompareTo(b);
            throw new NotImplementedException();
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
