using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperShort : ILazinatorWrapperShort, IComparable, IComparable<short>, IEquatable<short>, IComparable<LazinatorWrapperShort>, IEquatable<LazinatorWrapperShort>
    {
        public bool HasValue => true;

        public LazinatorWrapperShort(short x) : this()
        {
            WrappedValue = x;
        }

        public static implicit operator LazinatorWrapperShort(short x)
        {
            return new LazinatorWrapperShort(x);
        }

        public static implicit operator short(LazinatorWrapperShort x)
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
            if (obj is short v)
                return WrappedValue == v;
            else if (obj is LazinatorWrapperShort w)
                return WrappedValue == w.WrappedValue;
            return false;
        }

        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;
            if (obj is LazinatorWrapperShort other)
                return CompareTo(other);
            if (obj is short b)
                return CompareTo(b);
            throw new NotImplementedException();
        }

        public int CompareTo(short other)
        {
            return WrappedValue.CompareTo(other);
        }

        public bool Equals(short other)
        {
            return WrappedValue.Equals(other);
        }

        public int CompareTo(LazinatorWrapperShort other)
        {
            return WrappedValue.CompareTo(other.WrappedValue);
        }

        public bool Equals(LazinatorWrapperShort other)
        {
            return WrappedValue.Equals(other.WrappedValue);
        }
    }
}
