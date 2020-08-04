using System;

namespace Lazinator.Wrappers
{
    /// <summary>
    /// A Lazinator wrapper for a short. A wrapper can be used as a generic type where the unwrapped object cannot be.
    /// </summary>
    public partial struct WShort : IWShort, IComparable, IComparable<short>, IEquatable<short>, IComparable<WShort>, IEquatable<WShort>
    {
        public bool HasValue => true;

        public WShort(short x) : this()
        {
            WrappedValue = x;
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static implicit operator WShort(short x)
        {
            return new WShort(x);
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static implicit operator short(WShort x)
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
            else if (obj is WShort w)
                return WrappedValue == w.WrappedValue;
            return false;
        }

        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;
            if (obj is WShort other)
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

        public int CompareTo(WShort other)
        {
            return WrappedValue.CompareTo(other.WrappedValue);
        }

        public bool Equals(WShort other)
        {
            return WrappedValue.Equals(other.WrappedValue);
        }
    }
}
