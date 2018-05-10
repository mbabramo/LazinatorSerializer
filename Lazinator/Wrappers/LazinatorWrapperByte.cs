using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperByte : ILazinatorWrapperByte, IComparable, IComparable<byte>, IEquatable<byte>, IComparable<LazinatorWrapperByte>, IEquatable<LazinatorWrapperByte>
    {
        public bool IsNull => false;

        public static implicit operator LazinatorWrapperByte(byte x)
        {
            return new LazinatorWrapperByte() {Value = x};
        }

        public static implicit operator byte(LazinatorWrapperByte x)
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
            if (obj is byte v)
                return Value == v;
            else if (obj is LazinatorWrapperByte w)
                return Value == w.Value;
            return false;
        }

        public int CompareTo(object obj)
        {
            return Value.CompareTo(obj);
        }

        public int CompareTo(byte other)
        {
            return Value.CompareTo(other);
        }

        public bool Equals(byte other)
        {
            return Value.Equals(other);
        }

        public int CompareTo(LazinatorWrapperByte other)
        {
            return Value.CompareTo(other.Value);
        }

        public bool Equals(LazinatorWrapperByte other)
        {
            return Value.Equals(other.Value);
        }
    }
}
