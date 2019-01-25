using System;

namespace LazinatorCollectionsTests
{
    public partial struct NonComparableWrapperString : INonComparableWrapperString
    {
        public bool HasValue => WrappedValue != null;

        public NonComparableWrapperString(string x) : this()
        {
            WrappedValue = x;
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static implicit operator NonComparableWrapperString(string x)
        {
            return new NonComparableWrapperString(x);
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static implicit operator string(NonComparableWrapperString x)
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
            else if (obj is NonComparableWrapperString w)
                return WrappedValue == w.WrappedValue;
            return false;
        }

        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;
            if (obj is NonComparableWrapperString other)
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

        public int CompareTo(NonComparableWrapperString other)
        {
            if (WrappedValue == null)
                return other == null ? 0 : -1;
            return WrappedValue.CompareTo(other.WrappedValue);
        }

        public bool Equals(NonComparableWrapperString other)
        {
            if (WrappedValue == null)
                return other.WrappedValue == null ? true : false;
            return WrappedValue.Equals(other.WrappedValue);
        }
    }
}