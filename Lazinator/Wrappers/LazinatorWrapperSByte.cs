using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperSByte : ILazinatorWrapperSByte, IComparable, IComparable<sbyte>, IEquatable<sbyte>, IComparable<LazinatorWrapperSByte>, IEquatable<LazinatorWrapperSByte>
    {
        public bool HasValue => true;

        public LazinatorWrapperSByte(sbyte x) : this()
        {
            WrappedValue = x;
        }

        public static implicit operator LazinatorWrapperSByte(sbyte x)
        {
            return new LazinatorWrapperSByte(x);
        }

        public static implicit operator sbyte(LazinatorWrapperSByte x)
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
            if (obj is sbyte v)
                return WrappedValue == v;
            else if (obj is LazinatorWrapperSByte w)
                return WrappedValue == w.WrappedValue;
            return false;
        }

        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;
            if (obj is LazinatorWrapperSByte other)
                return CompareTo(other);
            if (obj is sbyte b)
                return CompareTo(b);
            throw new NotImplementedException();
        }

        public int CompareTo(sbyte other)
        {
            return WrappedValue.CompareTo(other);
        }

        public bool Equals(sbyte other)
        {
            return WrappedValue.Equals(other);
        }

        public int CompareTo(LazinatorWrapperSByte other)
        {
            return WrappedValue.CompareTo(other.WrappedValue);
        }

        public bool Equals(LazinatorWrapperSByte other)
        {
            return WrappedValue.Equals(other.WrappedValue);
        }
    }
}
