using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperDecimal : ILazinatorWrapperDecimal, IComparable, IComparable<decimal>, IEquatable<decimal>, IComparable<LazinatorWrapperDecimal>, IEquatable<LazinatorWrapperDecimal>
    {
        public static implicit operator LazinatorWrapperDecimal(decimal x)
        {
            return new LazinatorWrapperDecimal() { Value = x };
        }

        public static implicit operator decimal(LazinatorWrapperDecimal x)
        {
            return x.Value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var other = (LazinatorWrapperDecimal)obj;
            return Value == other.Value;
        }

        public int CompareTo(object obj)
        {
            return Value.CompareTo(obj);
        }

        public int CompareTo(decimal other)
        {
            return Value.CompareTo(other);
        }

        public bool Equals(decimal other)
        {
            return Value.Equals(other);
        }

        public int CompareTo(LazinatorWrapperDecimal other)
        {
            return Value.CompareTo(other.Value);
        }

        public bool Equals(LazinatorWrapperDecimal other)
        {
            return Value.Equals(other.Value);
        }
    }
}
