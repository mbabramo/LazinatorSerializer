using System;

namespace Lazinator.Wrappers
{
    /// <summary>
    /// A Lazinator wrapper for a short. A wrapper can be used as a generic type where the unwrapped object cannot be.
    /// </summary>
    public partial struct WInt16 : IWInt16, IComparable, IComparable<short>, IEquatable<short>, IComparable<WInt16>, IEquatable<WInt16>
    {
        public bool HasValue => true;

        public WInt16(short x) : this()
        {
            WrappedValue = x;
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static implicit operator WInt16(short x)
        {
            return new WInt16(x);
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static implicit operator short(WInt16 x)
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
            if (obj is short v)
                return WrappedValue == v;
            else if (obj is WInt16 w)
                return WrappedValue == w.WrappedValue;
            return false;
        }

        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;
            if (obj is WInt16 other)
                return CompareTo(other);
            if (obj is short b)
                return CompareTo(b);
            throw new NotImplementedException();
        }

        public int CompareTo(short other)
        {
            return WrappedValue.CompareTo(other);
        }

        public bool Equals(short other)
        {
            return WrappedValue.Equals(other);
        }

        public int CompareTo(WInt16 other)
        {
            return WrappedValue.CompareTo(other.WrappedValue);
        }

        public bool Equals(WInt16 other)
        {
            return WrappedValue.Equals(other.WrappedValue);
        }
    }
}
