using System;

namespace Lazinator.Wrappers
{
    /// <summary>
    /// A Lazinator wrapper for a TimeSpan. A wrapper can be used as a generic type where the unwrapped object cannot be.
    /// </summary>
    public partial struct WTimeSpan : IWTimeSpan, IComparable, IComparable<TimeSpan>, IEquatable<TimeSpan>, IComparable<WTimeSpan>, IEquatable<WTimeSpan>
    {
        public bool HasValue => true;

        public WTimeSpan(TimeSpan x) : this()
        {
            WrappedValue = x;
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static implicit operator WTimeSpan(TimeSpan x)
        {
            return new WTimeSpan(x);
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static implicit operator TimeSpan(WTimeSpan x)
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
            if (obj is TimeSpan v)
                return WrappedValue == v;
            else if (obj is WTimeSpan w)
                return WrappedValue == w.WrappedValue;
            return false;
        }

        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;
            if (obj is WTimeSpan other)
                return CompareTo(other);
            if (obj is TimeSpan b)
                return CompareTo(b);
            throw new NotImplementedException();
        }

        public int CompareTo(TimeSpan other)
        {
            return WrappedValue.CompareTo(other);
        }

        public bool Equals(TimeSpan other)
        {
            return WrappedValue.Equals(other);
        }

        public int CompareTo(WTimeSpan other)
        {
            return WrappedValue.CompareTo(other.WrappedValue);
        }

        public bool Equals(WTimeSpan other)
        {
            return WrappedValue.Equals(other.WrappedValue);
        }
    }
}
