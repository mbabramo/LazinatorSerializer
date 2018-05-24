using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperDouble : ILazinatorWrapperDouble, IComparable, IComparable<double>, IEquatable<double>, IComparable<LazinatorWrapperDouble>, IEquatable<LazinatorWrapperDouble>
    {
        public bool HasValue => true;

        public LazinatorWrapperDouble(double x) : this()
        {
            WrappedValue = x;
        }

        public static implicit operator LazinatorWrapperDouble(double x)
        {
            return new LazinatorWrapperDouble(x);
        }

        public static implicit operator double(LazinatorWrapperDouble x)
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
            if (obj is double v)
                return WrappedValue == v;
            else if (obj is LazinatorWrapperDouble w)
                return WrappedValue == w.WrappedValue;
            return false;
        }

        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;
            if (obj is LazinatorWrapperDouble other)
                return CompareTo(other);
            if (obj is double b)
                return CompareTo(b);
            throw new NotImplementedException();
        }

        public int CompareTo(double other)
        {
            return WrappedValue.CompareTo(other);
        }

        public bool Equals(double other)
        {
            return WrappedValue.Equals(other);
        }

        public int CompareTo(LazinatorWrapperDouble other)
        {
            return WrappedValue.CompareTo(other.WrappedValue);
        }

        public bool Equals(LazinatorWrapperDouble other)
        {
            return WrappedValue.Equals(other.WrappedValue);
        }
    }
}
