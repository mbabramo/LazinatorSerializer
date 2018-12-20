using System;

namespace Lazinator.Wrappers
{
    public partial struct WSByte : IWSByte, IComparable, IComparable<sbyte>, IEquatable<sbyte>, IComparable<WSByte>, IEquatable<WSByte>
    {
        public bool HasValue => true;

        public WSByte(sbyte x) : this()
        {
            WrappedValue = x;
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static implicit operator WSByte(sbyte x)
        {
            return new WSByte(x);
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static implicit operator sbyte(WSByte x)
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
            else if (obj is WSByte w)
                return WrappedValue == w.WrappedValue;
            return false;
        }

        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;
            if (obj is WSByte other)
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

        public int CompareTo(WSByte other)
        {
            return WrappedValue.CompareTo(other.WrappedValue);
        }

        public bool Equals(WSByte other)
        {
            return WrappedValue.Equals(other.WrappedValue);
        }
    }
}
