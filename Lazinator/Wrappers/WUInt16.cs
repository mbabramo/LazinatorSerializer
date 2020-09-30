using System;

namespace Lazinator.Wrappers
{
    /// <summary>
    /// A Lazinator wrapper for an unsigned short. A wrapper can be used as a generic type where the unwrapped object cannot be.
    /// </summary>
    public partial struct WUInt16 : IWUInt16, IComparable, IComparable<ushort>, IEquatable<ushort>, IComparable<WUInt16>, IEquatable<WUInt16>
    {
        public bool HasValue => true;

        public WUInt16(ushort x) : this()
        {
            WrappedValue = x;
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static implicit operator WUInt16(ushort x)
        {
            return new WUInt16(x);
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static implicit operator ushort(WUInt16 x)
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
            else if (obj is WUInt16 w)
                return WrappedValue == w.WrappedValue;
            return false;
        }

        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;
            if (obj is WUInt16 other)
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

        public int CompareTo(WUInt16 other)
        {
            return WrappedValue.CompareTo(other.WrappedValue);
        }

        public bool Equals(WUInt16 other)
        {
            return WrappedValue.Equals(other.WrappedValue);
        }
    }
}
