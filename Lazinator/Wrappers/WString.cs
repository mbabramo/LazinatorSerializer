using System;

namespace Lazinator.Wrappers
{
    public partial struct WString : IWString, IComparable, IComparable<string>, IEquatable<string>, IComparable<WString>, IEquatable<WString>
    {
        public bool HasValue => WrappedValue != null;

        public WString(string x) : this()
        {
            WrappedValue = x;
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static implicit operator WString(string x)
        {
            return new WString(x);
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static implicit operator string(WString x)
        {
            return x.WrappedValue;
        }

        public override string ToString()
        {
            return WrappedValue?.ToString() ?? "";
        }

        public override int GetHashCode()
        {
            return WrappedValue.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is string v)
                return WrappedValue == v;
            else if (obj is WString w)
                return WrappedValue == w.WrappedValue;
            return false;
        }

        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;
            if (obj is WString other)
                return CompareTo(other);
            if (obj is string b)
                return CompareTo(b);
            throw new NotImplementedException();
        }

        public int CompareTo(string other)
        {
            if (WrappedValue == null)
                return other == null ? 0 : -1;
            return WrappedValue.CompareTo(other);
        }

        public bool Equals(string other)
        {
            if (WrappedValue == null)
                return other == null ? true : false;
            return WrappedValue.Equals(other);
        }

        public int CompareTo(WString other)
        {
            if (WrappedValue == null)
                return other == null ? 0 : -1;
            return WrappedValue.CompareTo(other.WrappedValue);
        }

        public bool Equals(WString other)
        {
            if (WrappedValue == null)
                return other.WrappedValue == null ? true : false;
            return WrappedValue.Equals(other.WrappedValue);
        }
    }
}