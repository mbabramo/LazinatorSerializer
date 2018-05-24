using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperDateTime : ILazinatorWrapperDateTime, IComparable, IComparable<DateTime>, IEquatable<DateTime>, IComparable<LazinatorWrapperDateTime>, IEquatable<LazinatorWrapperDateTime>
    {
        public bool HasValue => true;

        public LazinatorWrapperDateTime(DateTime x) : this()
        {
            WrappedValue = x;
        }

        public static implicit operator LazinatorWrapperDateTime(DateTime x)
        {
            return new LazinatorWrapperDateTime(x);
        }

        public static implicit operator DateTime(LazinatorWrapperDateTime x)
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
            else if (obj is LazinatorWrapperDateTime w)
                return WrappedValue == w.WrappedValue;
            return false;
        }

        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;
            if (obj is LazinatorWrapperDateTime other)
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

        public int CompareTo(LazinatorWrapperDateTime other)
        {
            return WrappedValue.CompareTo(other.WrappedValue);
        }

        public bool Equals(LazinatorWrapperDateTime other)
        {
            return WrappedValue.Equals(other.WrappedValue);
        }
    }
}
