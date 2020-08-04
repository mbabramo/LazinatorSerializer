using System;

namespace Lazinator.Wrappers
{
    /// <summary>
    /// A Lazinator wrapper for a Guid. A wrapper can be used as a generic type where the unwrapped object cannot be.
    /// </summary>
    public partial struct WGuid : IWGuid, IComparable, IComparable<uint>, IEquatable<uint>, IComparable<WGuid>, IEquatable<WGuid>
    {
        public bool HasValue => true;

        public WGuid(Guid x) : this()
        {
            WrappedValue = x;
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static implicit operator WGuid(Guid x)
        {
            return new WGuid(x);
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static implicit operator Guid(WGuid x)
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
            if (obj is Guid v)
                return WrappedValue == v;
            else if (obj is WGuid w)
                return WrappedValue == w.WrappedValue;
            return false;
        }
        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;
            if (obj is WGuid other)
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

        public int CompareTo(WGuid other)
        {
            return WrappedValue.CompareTo(other.WrappedValue);
        }

        public bool Equals(WGuid other)
        {
            return WrappedValue.Equals(other.WrappedValue);
        }
    }
}