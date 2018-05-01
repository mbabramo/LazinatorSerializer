using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperInt : ILazinatorWrapperInt, IComparable, IComparable<int>, IEquatable<int>, IComparable<LazinatorWrapperInt>, IEquatable<LazinatorWrapperInt>
    {
        public static implicit operator LazinatorWrapperInt(int x)
        {
            return new LazinatorWrapperInt() { Value = x };
        }

        public static implicit operator int(LazinatorWrapperInt x)
        {
            return x.Value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var other = (LazinatorWrapperInt)obj;
            return Value == other.Value;
        }

        public int CompareTo(object obj)
        {
            return Value.CompareTo(obj);
        }

        public int CompareTo(int other)
        {
            return Value.CompareTo(other);
        }

        public bool Equals(int other)
        {
            return Value.Equals(other);
        }

        public int CompareTo(LazinatorWrapperInt other)
        {
            return Value.CompareTo(other.Value);
        }

        public bool Equals(LazinatorWrapperInt other)
        {
            return Value.Equals(other.Value);
        }
    }
}
