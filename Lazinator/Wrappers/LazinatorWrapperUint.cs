using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperUint : ILazinatorWrapperUint, IComparable, IComparable<uint>, IEquatable<uint>, IComparable<LazinatorWrapperUint>, IEquatable<LazinatorWrapperUint>
    {
        public static implicit operator LazinatorWrapperUint(uint x)
        {
            return new LazinatorWrapperUint() { Value = x };
        }

        public static implicit operator uint(LazinatorWrapperUint x)
        {
            return x.Value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var other = (LazinatorWrapperUint)obj;
            return Value == other.Value;
        }

        public int CompareTo(object obj)
        {
            return Value.CompareTo(obj);
        }

        public int CompareTo(uint other)
        {
            return Value.CompareTo(other);
        }

        public bool Equals(uint other)
        {
            return Value.Equals(other);
        }

        public int CompareTo(LazinatorWrapperUint other)
        {
            return Value.CompareTo(other.Value);
        }

        public bool Equals(LazinatorWrapperUint other)
        {
            return Value.Equals(other.Value);
        }
    }
}
