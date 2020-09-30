using System;

namespace Lazinator.Wrappers
{
    /// <summary>
    /// A Lazinator wrapper for a long. A wrapper can be used as a generic type where the unwrapped object cannot be.
    /// </summary>
    public partial struct WInt64 : IWInt64, IComparable, IComparable<long>, IEquatable<long>, IComparable<WInt64>, IEquatable<WInt64>
    {
        public bool HasValue => true;

        public WInt64(long x) : this()
        {
            WrappedValue = x;
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static implicit operator WInt64(long x)
        {
            return new WInt64(x);
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static implicit operator long(WInt64 x)
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
            if (obj is long v)
                return WrappedValue == v;
            else if (obj is WInt64 w)
                return WrappedValue == w.WrappedValue;
            return false;
        }

        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;
            if (obj is WInt64 other)
                return CompareTo(other);
            if (obj is long b)
                return CompareTo(b);
            throw new NotImplementedException();
        }

        public int CompareTo(long other)
        {
            return WrappedValue.CompareTo(other);
        }

        public bool Equals(long other)
        {
            return WrappedValue.Equals(other);
        }

        public int CompareTo(WInt64 other)
        {
            return WrappedValue.CompareTo(other.WrappedValue);
        }

        public bool Equals(WInt64 other)
        {
            return WrappedValue.Equals(other.WrappedValue);
        }
    }
}
