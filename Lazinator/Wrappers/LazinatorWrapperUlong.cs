using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperUlong : ILazinatorWrapperUlong, IComparable, IComparable<ulong>, IEquatable<ulong>, IComparable<LazinatorWrapperUlong>, IEquatable<LazinatorWrapperUlong>
    {
        public bool HasValue => true;

        public LazinatorWrapperUlong(ulong x) : this()
        {
            WrappedValue = x;
        }

        public static implicit operator LazinatorWrapperUlong(ulong x)
        {
            return new LazinatorWrapperUlong(x);
        }

        public static implicit operator ulong(LazinatorWrapperUlong x)
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
            if (obj is ulong v)
                return WrappedValue == v;
            else if (obj is LazinatorWrapperUlong w)
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

        public int CompareTo(ulong other)
        {
            return WrappedValue.CompareTo(other);
        }

        public bool Equals(ulong other)
        {
            return WrappedValue.Equals(other);
        }

        public int CompareTo(LazinatorWrapperUlong other)
        {
            return WrappedValue.CompareTo(other.WrappedValue);
        }

        public bool Equals(LazinatorWrapperUlong other)
        {
            return WrappedValue.Equals(other.WrappedValue);
        }
    }
}
