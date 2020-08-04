using System;

namespace Lazinator.Wrappers
{
    /// <summary>
    /// A Lazinator wrapper for a float. A wrapper can be used as a generic type where the unwrapped object cannot be.
    /// </summary>
    public partial struct WFloat : IWFloat, IComparable, IComparable<float>, IEquatable<float>, IComparable<WFloat>, IEquatable<WFloat>
    {
        public bool HasValue => true;

        public WFloat(float x) : this()
        {
            WrappedValue = x;
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static implicit operator WFloat(float x)
        {
            return new WFloat(x);
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static implicit operator float(WFloat x)
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
            if (obj is float v)
                return WrappedValue == v;
            else if (obj is WFloat w)
                return WrappedValue == w.WrappedValue;
            return false;
        }

        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;
            if (obj is WFloat other)
                return CompareTo(other);
            if (obj is float b)
                return CompareTo(b);
            throw new NotImplementedException();
        }

        public int CompareTo(float other)
        {
            return WrappedValue.CompareTo(other);
        }

        public bool Equals(float other)
        {
            return WrappedValue.Equals(other);
        }

        public int CompareTo(WFloat other)
        {
            return WrappedValue.CompareTo(other.WrappedValue);
        }

        public bool Equals(WFloat other)
        {
            return WrappedValue.Equals(other.WrappedValue);
        }
    }
}
