using System;

namespace Lazinator.Wrappers
{
    /// <summary>
    /// A Lazinator wrapper for an unsigned integer. A wrapper can be used as a generic type where the unwrapped object cannot be.
    /// </summary>
    public partial struct WUInt32 : IWUInt32, IComparable, IComparable<uint>, IEquatable<uint>, IComparable<WUInt32>, IEquatable<WUInt32>
    {
        public bool HasValue => true;

        public WUInt32(uint x) : this()
        {
            WrappedValue = x;
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static implicit operator WUInt32(uint x)
        {
            return new WUInt32(x);
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static implicit operator uint(WUInt32 x)
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
            if (obj is uint v)
                return WrappedValue == v;
            else if (obj is WUInt32 w)
                return WrappedValue == w.WrappedValue;
            return false;
        }

        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;
            if (obj is WUInt32 other)
                return CompareTo(other);
            if (obj is uint b)
                return CompareTo(b);
            throw new NotImplementedException();
        }

        public int CompareTo(uint other)
        {
            return WrappedValue.CompareTo(other);
        }

        public bool Equals(uint other)
        {
            return WrappedValue.Equals(other);
        }

        public int CompareTo(WUInt32 other)
        {
            return WrappedValue.CompareTo(other.WrappedValue);
        }

        public bool Equals(WUInt32 other)
        {
            return WrappedValue.Equals(other.WrappedValue);
        }
    }
}
