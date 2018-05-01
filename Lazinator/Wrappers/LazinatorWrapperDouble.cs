using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperDouble : ILazinatorWrapperDouble, IComparable, IComparable<double>, IEquatable<double>, IComparable<LazinatorWrapperDouble>, IEquatable<LazinatorWrapperDouble>
    {
        public static implicit operator LazinatorWrapperDouble(double x)
        {
            return new LazinatorWrapperDouble() { Value = x };
        }

        public static implicit operator double(LazinatorWrapperDouble x)
        {
            return x.Value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var other = (LazinatorWrapperDouble)obj;
            return Value == other.Value;
        }

        public int CompareTo(object obj)
        {
            return Value.CompareTo(obj);
        }

        public int CompareTo(double other)
        {
            return Value.CompareTo(other);
        }

        public bool Equals(double other)
        {
            return Value.Equals(other);
        }

        public int CompareTo(LazinatorWrapperDouble other)
        {
            return Value.CompareTo(other.Value);
        }

        public bool Equals(LazinatorWrapperDouble other)
        {
            return Value.Equals(other.Value);
        }
    }
}
