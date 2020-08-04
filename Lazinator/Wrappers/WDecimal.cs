using System;

namespace Lazinator.Wrappers
{
    /// <summary>
    /// A Lazinator wrapper for a decimal. A wrapper can be used as a generic type where the unwrapped object cannot be.
    /// </summary>
    public partial struct WDecimal : IWDecimal, IComparable, IComparable<decimal>, IEquatable<decimal>, IComparable<WDecimal>, IEquatable<WDecimal>
    {
        public bool HasValue => true;

        public WDecimal(decimal x) : this()
        {
            WrappedValue = x;
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static implicit operator WDecimal(decimal x)
        {
            return new WDecimal(x);
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static implicit operator decimal(WDecimal x)
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
            if (obj is decimal v)
                return WrappedValue == v;
            else if (obj is WDecimal w)
                return WrappedValue == w.WrappedValue;
            return false;
        }

        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;
            if (obj is WDecimal other)
                return CompareTo(other);
            if (obj is decimal b)
                return CompareTo(b);
            throw new NotImplementedException();
        }

        public int CompareTo(decimal other)
        {
            return WrappedValue.CompareTo(other);
        }

        public bool Equals(decimal other)
        {
            return WrappedValue.Equals(other);
        }

        public int CompareTo(WDecimal other)
        {
            return WrappedValue.CompareTo(other.WrappedValue);
        }

        public bool Equals(WDecimal other)
        {
            return WrappedValue.Equals(other.WrappedValue);
        }
    }
}
