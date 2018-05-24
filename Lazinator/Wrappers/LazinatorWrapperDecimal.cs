using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperDecimal : ILazinatorWrapperDecimal, IComparable, IComparable<decimal>, IEquatable<decimal>, IComparable<LazinatorWrapperDecimal>, IEquatable<LazinatorWrapperDecimal>
    {
        public bool HasValue => true;

        public LazinatorWrapperDecimal(decimal x) : this()
        {
            WrappedValue = x;
        }

        public static implicit operator LazinatorWrapperDecimal(decimal x)
        {
            return new LazinatorWrapperDecimal(x);
        }

        public static implicit operator decimal(LazinatorWrapperDecimal x)
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
            if (obj is decimal v)
                return WrappedValue == v;
            else if (obj is LazinatorWrapperDecimal w)
                return WrappedValue == w.WrappedValue;
            return false;
        }

        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;
            if (obj is LazinatorWrapperDecimal other)
                return CompareTo(other);
            if (obj is decimal b)
                return CompareTo(b);
            throw new NotImplementedException();
        }

        public int CompareTo(decimal other)
        {
            return WrappedValue.CompareTo(other);
        }

        public bool Equals(decimal other)
        {
            return WrappedValue.Equals(other);
        }

        public int CompareTo(LazinatorWrapperDecimal other)
        {
            return WrappedValue.CompareTo(other.WrappedValue);
        }

        public bool Equals(LazinatorWrapperDecimal other)
        {
            return WrappedValue.Equals(other.WrappedValue);
        }
    }
}
