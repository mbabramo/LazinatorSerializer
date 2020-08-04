using System;

namespace Lazinator.Wrappers
{
    /// <summary>
    /// A Lazinator wrapper for a DateTime. A wrapper can be used as a generic type where the unwrapped object cannot be.
    /// </summary>
    public partial struct WDateTime : IWDateTime, IComparable, IComparable<DateTime>, IEquatable<DateTime>, IComparable<WDateTime>, IEquatable<WDateTime>
    {
        public bool HasValue => true;

        public WDateTime(DateTime x) : this()
        {
            WrappedValue = x;
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static implicit operator WDateTime(DateTime x)
        {
            return new WDateTime(x);
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static implicit operator DateTime(WDateTime x)
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
            if (obj is DateTime v)
                return WrappedValue == v;
            else if (obj is WDateTime w)
                return WrappedValue == w.WrappedValue;
            return false;
        }

        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;
            if (obj is WDateTime other)
                return CompareTo(other);
            if (obj is DateTime b)
                return CompareTo(b);
            throw new NotImplementedException();
        }

        public int CompareTo(DateTime other)
        {
            return WrappedValue.CompareTo(other);
        }

        public bool Equals(DateTime other)
        {
            return WrappedValue.Equals(other);
        }

        public int CompareTo(WDateTime other)
        {
            return WrappedValue.CompareTo(other.WrappedValue);
        }

        public bool Equals(WDateTime other)
        {
            return WrappedValue.Equals(other.WrappedValue);
        }
    }
}
