using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperByte : ILazinatorWrapperByte, IComparable, IComparable<byte>, IEquatable<byte>, IComparable<LazinatorWrapperByte>, IEquatable<LazinatorWrapperByte>
    {
        public bool HasValue => true;

        public LazinatorWrapperByte(byte x) : this()
        {
            WrappedValue = x;
        }

        public static implicit operator LazinatorWrapperByte(byte x)
        {
            return new LazinatorWrapperByte(x);
        }

        public static implicit operator byte(LazinatorWrapperByte x)
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
            if (obj is byte v)
                return WrappedValue == v;
            else if (obj is LazinatorWrapperByte w)
                return WrappedValue == w.WrappedValue;
            return false;
        }

        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;
            if (obj is LazinatorWrapperByte other)
                return CompareTo(other);
            if (obj is byte b)
                return CompareTo(b);
            throw new NotImplementedException();
        }

        public int CompareTo(byte other)
        {
            return WrappedValue.CompareTo(other);
        }

        public bool Equals(byte other)
        {
            return WrappedValue.Equals(other);
        }

        public int CompareTo(LazinatorWrapperByte other)
        {
            return WrappedValue.CompareTo(other.WrappedValue);
        }

        public bool Equals(LazinatorWrapperByte other)
        {
            return WrappedValue.Equals(other.WrappedValue);
        }
    }
}
