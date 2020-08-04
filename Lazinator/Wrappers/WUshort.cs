using System;

namespace Lazinator.Wrappers
{
    /// <summary>
    /// A Lazinator wrapper for an unsigned short. A wrapper can be used as a generic type where the unwrapped object cannot be.
    /// </summary>
    public partial struct WUshort : IWUshort, IComparable, IComparable<ushort>, IEquatable<ushort>, IComparable<WUshort>, IEquatable<WUshort>
    {
        public bool HasValue => true;

        public WUshort(ushort x) : this()
        {
            WrappedValue = x;
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static implicit operator WUshort(ushort x)
        {
            return new WUshort(x);
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static implicit operator ushort(WUshort x)
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
            if (obj is ushort v)
                return WrappedValue == v;
            else if (obj is WUshort w)
                return WrappedValue == w.WrappedValue;
            return false;
        }

        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;
            if (obj is WUshort other)
                return CompareTo(other);
            if (obj is ushort b)
                return CompareTo(b);
            throw new NotImplementedException();
        }

        public int CompareTo(ushort other)
        {
            return WrappedValue.CompareTo(other);
        }

        public bool Equals(ushort other)
        {
            return WrappedValue.Equals(other);
        }

        public int CompareTo(WUshort other)
        {
            return WrappedValue.CompareTo(other.WrappedValue);
        }

        public bool Equals(WUshort other)
        {
            return WrappedValue.Equals(other.WrappedValue);
        }
    }
}
