using System;

namespace Lazinator.Wrappers
{
    public partial struct WLong : IWLong, IComparable, IComparable<long>, IEquatable<long>, IComparable<WLong>, IEquatable<WLong>
    {
        public bool HasValue => true;

        public WLong(long x) : this()
        {
            WrappedValue = x;
        }

        public static implicit operator WLong(long x)
        {
            return new WLong(x);
        }

        public static implicit operator long(WLong x)
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
            else if (obj is WLong w)
                return WrappedValue == w.WrappedValue;
            return false;
        }

        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;
            if (obj is WLong other)
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

        public int CompareTo(WLong other)
        {
            return WrappedValue.CompareTo(other.WrappedValue);
        }

        public bool Equals(WLong other)
        {
            return WrappedValue.Equals(other.WrappedValue);
        }
    }
}
