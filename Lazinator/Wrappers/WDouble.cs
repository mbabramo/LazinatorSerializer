using System;

namespace Lazinator.Wrappers
{
    /// <summary>
    /// A Lazinator wrapper for a double. A wrapper can be used as a generic type where the unwrapped object cannot be.
    /// </summary>
    public partial struct WDouble : IWDouble, IComparable, IComparable<double>, IEquatable<double>, IComparable<WDouble>, IEquatable<WDouble>
    {
        public bool HasValue => true;

        public WDouble(double x) : this()
        {
            WrappedValue = x;
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static implicit operator WDouble(double x)
        {
            return new WDouble(x);
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static implicit operator double(WDouble x)
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
            if (obj is double v)
                return WrappedValue == v;
            else if (obj is WDouble w)
                return WrappedValue == w.WrappedValue;
            return false;
        }

        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;
            if (obj is WDouble other)
                return CompareTo(other);
            if (obj is double b)
                return CompareTo(b);
            throw new NotImplementedException();
        }

        public int CompareTo(double other)
        {
            return WrappedValue.CompareTo(other);
        }

        public bool Equals(double other)
        {
            return WrappedValue.Equals(other);
        }

        public int CompareTo(WDouble other)
        {
            return WrappedValue.CompareTo(other.WrappedValue);
        }

        public bool Equals(WDouble other)
        {
            return WrappedValue.Equals(other.WrappedValue);
        }
    }
}
