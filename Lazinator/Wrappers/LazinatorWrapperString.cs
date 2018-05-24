using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperString : ILazinatorWrapperString, IComparable, IComparable<string>, IEquatable<string>, IComparable<LazinatorWrapperString>, IEquatable<LazinatorWrapperString>
    {
        public bool HasValue => WrappedValue != null;

        public LazinatorWrapperString(string x) : this()
        {
            WrappedValue = x;
        }

        public static implicit operator LazinatorWrapperString(string x)
        {
            return new LazinatorWrapperString(x);
        }

        public static implicit operator string(LazinatorWrapperString x)
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
            if (obj is string v)
                return WrappedValue == v;
            else if (obj is LazinatorWrapperString w)
                return WrappedValue == w.WrappedValue;
            return false;
        }

        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;
            if (obj is LazinatorWrapperString other)
                return CompareTo(other);
            if (obj is string b)
                return CompareTo(b);
            throw new NotImplementedException();
        }

        public int CompareTo(string other)
        {
            return WrappedValue.CompareTo(other);
        }

        public bool Equals(string other)
        {
            return WrappedValue.Equals(other);
        }

        public int CompareTo(LazinatorWrapperString other)
        {
            return WrappedValue.CompareTo(other.WrappedValue);
        }

        public bool Equals(LazinatorWrapperString other)
        {
            return WrappedValue.Equals(other.WrappedValue);
        }
    }
}