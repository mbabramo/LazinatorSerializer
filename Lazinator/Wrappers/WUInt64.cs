using System;

namespace Lazinator.Wrappers
{
    /// <summary>
    /// A Lazinator wrapper for an unsigned long. A wrapper can be used as a generic type where the unwrapped object cannot be.
    /// </summary>
    public partial struct WUInt64 : IWUInt64, IComparable, IComparable<ulong>, IEquatable<ulong>, IComparable<WUInt64>, IEquatable<WUInt64>
    {
        public bool HasValue => true;

        public WUInt64(ulong x) : this()
        {
            WrappedValue = x;
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static implicit operator WUInt64(ulong x)
        {
            return new WUInt64(x);
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static implicit operator ulong(WUInt64 x)
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
            if (obj is ulong v)
                return WrappedValue == v;
            else if (obj is WUInt64 w)
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

        public int CompareTo(ulong other)
        {
            return WrappedValue.CompareTo(other);
        }

        public bool Equals(ulong other)
        {
            return WrappedValue.Equals(other);
        }

        public int CompareTo(WUInt64 other)
        {
            return WrappedValue.CompareTo(other.WrappedValue);
        }

        public bool Equals(WUInt64 other)
        {
            return WrappedValue.Equals(other.WrappedValue);
        }
    }
}
