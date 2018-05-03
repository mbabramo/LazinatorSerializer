using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperUshort : ILazinatorWrapperUshort, IComparable, IComparable<ushort>, IEquatable<ushort>, IComparable<LazinatorWrapperUshort>, IEquatable<LazinatorWrapperUshort>
    {
        public static implicit operator LazinatorWrapperUshort(ushort x)
        {
            return new LazinatorWrapperUshort() { Value = x };
        }

        public static implicit operator ushort(LazinatorWrapperUshort x)
        {
            return x.Value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var other = (LazinatorWrapperUshort)obj;
            return Value == other.Value;
        }

        public int CompareTo(object obj)
        {
            return Value.CompareTo(obj);
        }

        public int CompareTo(ushort other)
        {
            return Value.CompareTo(other);
        }

        public bool Equals(ushort other)
        {
            return Value.Equals(other);
        }

        public int CompareTo(LazinatorWrapperUshort other)
        {
            return Value.CompareTo(other.Value);
        }

        public bool Equals(LazinatorWrapperUshort other)
        {
            return Value.Equals(other.Value);
        }
    }
}
