using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperInt : ILazinatorWrapperInt, IComparable, IComparable<int>, IEquatable<int>, IComparable<LazinatorWrapperInt>, IEquatable<LazinatorWrapperInt>
    {
        public bool HasValue => true;

        public LazinatorWrapperInt(int x) : this()
        {
            WrappedValue = x;
        }

        public static implicit operator LazinatorWrapperInt(int x)
        {
            return new LazinatorWrapperInt(x);
        }

        public static implicit operator int(LazinatorWrapperInt x)
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
            if (obj is int v)
                return WrappedValue == v;
            else if (obj is LazinatorWrapperInt w)
                return WrappedValue == w.WrappedValue;
            return false;
        }

        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;
            if (obj is LazinatorWrapperInt other)
                return CompareTo(other);
            if (obj is int b)
                return CompareTo(b);
            throw new NotImplementedException();
        }

        public int CompareTo(int other)
        {
            return WrappedValue.CompareTo(other);
        }

        public bool Equals(int other)
        {
            return WrappedValue.Equals(other);
        }

        public int CompareTo(LazinatorWrapperInt other)
        {
            return WrappedValue.CompareTo(other.WrappedValue);
        }

        public bool Equals(LazinatorWrapperInt other)
        {
            return WrappedValue.Equals(other.WrappedValue);
        }
    }
}
