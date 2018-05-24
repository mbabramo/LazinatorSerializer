using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperLong : ILazinatorWrapperLong, IComparable, IComparable<long>, IEquatable<long>, IComparable<LazinatorWrapperLong>, IEquatable<LazinatorWrapperLong>
    {
        public bool HasValue => true;

        public LazinatorWrapperLong(long x) : this()
        {
            WrappedValue = x;
        }

        public static implicit operator LazinatorWrapperLong(long x)
        {
            return new LazinatorWrapperLong(x);
        }

        public static implicit operator long(LazinatorWrapperLong x)
        {
            return x.WrappedValue;
        }

        public override string ToString()
        {
            return WrappedValue.ToString();
        }

        public override int GetHashCode()
        {
            return WrappedValue.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is long v)
                return WrappedValue == v;
            else if (obj is LazinatorWrapperLong w)
                return WrappedValue == w.WrappedValue;
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
            return WrappedValue.CompareTo(other);
        }

        public bool Equals(long other)
        {
            return WrappedValue.Equals(other);
        }

        public int CompareTo(LazinatorWrapperLong other)
        {
            return WrappedValue.CompareTo(other.WrappedValue);
        }

        public bool Equals(LazinatorWrapperLong other)
        {
            return WrappedValue.Equals(other.WrappedValue);
        }
    }
}
