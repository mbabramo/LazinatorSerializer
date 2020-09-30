using System;

namespace Lazinator.Wrappers
{
    /// <summary>
    /// A Lazinator wrapper for an int. A wrapper can be used as a generic type where the unwrapped object cannot be.
    /// </summary>
    public partial struct WInt32 : IWInt32, IComparable, IComparable<int>, IEquatable<int>, IComparable<WInt32>, IEquatable<WInt32>
    {
        public bool HasValue => true;

        public WInt32(int x) : this()
        {
            WrappedValue = x;
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static implicit operator WInt32(int x)
        {
            return new WInt32(x);
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static implicit operator int(WInt32 x)
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
            else if (obj is WInt32 w)
                return WrappedValue == w.WrappedValue;
            return false;
        }

        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;
            if (obj is WInt32 other)
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

        public int CompareTo(WInt32 other)
        {
            return WrappedValue.CompareTo(other.WrappedValue);
        }

        public bool Equals(WInt32 other)
        {
            return WrappedValue.Equals(other.WrappedValue);
        }
    }
}
