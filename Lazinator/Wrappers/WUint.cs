using System;

namespace Lazinator.Wrappers
{
    public partial struct WUint : IWUint, IComparable, IComparable<uint>, IEquatable<uint>, IComparable<WUint>, IEquatable<WUint>
    {
        public bool HasValue => true;

        public WUint(uint x) : this()
        {
            WrappedValue = x;
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static implicit operator WUint(uint x)
        {
            return new WUint(x);
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static implicit operator uint(WUint x)
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
            else if (obj is WUint w)
                return WrappedValue == w.WrappedValue;
            return false;
        }

        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;
            if (obj is WUint other)
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

        public int CompareTo(WUint other)
        {
            return WrappedValue.CompareTo(other.WrappedValue);
        }

        public bool Equals(WUint other)
        {
            return WrappedValue.Equals(other.WrappedValue);
        }
    }
}
