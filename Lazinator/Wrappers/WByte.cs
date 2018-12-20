using System;

namespace Lazinator.Wrappers
{
    public partial struct WByte : IWByte, IComparable, IComparable<byte>, IEquatable<byte>, IComparable<WByte>, IEquatable<WByte>
    {
        public bool HasValue => true;

        public WByte(byte x) : this()
        {
            WrappedValue = x;
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static implicit operator WByte(byte x)
        {
            return new WByte(x);
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static implicit operator byte(WByte x)
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
            else if (obj is WByte w)
                return WrappedValue == w.WrappedValue;
            return false;
        }

        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;
            if (obj is WByte other)
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

        public int CompareTo(WByte other)
        {
            return WrappedValue.CompareTo(other.WrappedValue);
        }

        public bool Equals(WByte other)
        {
            return WrappedValue.Equals(other.WrappedValue);
        }
    }
}
