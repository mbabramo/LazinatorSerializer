using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperShort : ILazinatorWrapperShort, IComparable, IComparable<short>, IEquatable<short>, IComparable<LazinatorWrapperShort>, IEquatable<LazinatorWrapperShort>
    {
        public static implicit operator LazinatorWrapperShort(short x)
        {
            return new LazinatorWrapperShort() { Value = x };
        }

        public static implicit operator short(LazinatorWrapperShort x)
        {
            return x.Value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var other = (LazinatorWrapperShort)obj;
            return Value == other.Value;
        }

        public int CompareTo(object obj)
        {
            return Value.CompareTo(obj);
        }

        public int CompareTo(short other)
        {
            return Value.CompareTo(other);
        }

        public bool Equals(short other)
        {
            return Value.Equals(other);
        }

        public int CompareTo(LazinatorWrapperShort other)
        {
            return Value.CompareTo(other.Value);
        }

        public bool Equals(LazinatorWrapperShort other)
        {
            return Value.Equals(other.Value);
        }
    }
}
