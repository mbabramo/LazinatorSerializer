using System;

namespace Lazinator.Wrappers
{
    /// <summary>
    /// A Lazinator wrapper for an int. A wrapper can be used as a generic type where the unwrapped object cannot be.
    /// </summary>
    public partial struct WInt : IWInt, IComparable, IComparable<int>, IEquatable<int>, IComparable<WInt>, IEquatable<WInt>
    {
        public bool HasValue => true;

        public WInt(int x) : this()
        {
            WrappedValue = x;
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static implicit operator WInt(int x)
        {
            return new WInt(x);
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static implicit operator int(WInt x)
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
            if (obj is int v)
                return WrappedValue == v;
            else if (obj is WInt w)
                return WrappedValue == w.WrappedValue;
            return false;
        }

        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;
            if (obj is WInt other)
                return CompareTo(other);
            if (obj is int b)
                return CompareTo(b);
            throw new NotImplementedException();
        }

        public int CompareTo(int other)
        {
            return WrappedValue.CompareTo(other);
        }

        public bool Equals(int other)
        {
            return WrappedValue.Equals(other);
        }

        public int CompareTo(WInt other)
        {
            return WrappedValue.CompareTo(other.WrappedValue);
        }

        public bool Equals(WInt other)
        {
            return WrappedValue.Equals(other.WrappedValue);
        }
    }
}
