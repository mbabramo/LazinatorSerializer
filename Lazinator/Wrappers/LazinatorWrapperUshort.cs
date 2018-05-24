using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperUshort : ILazinatorWrapperUshort, IComparable, IComparable<ushort>, IEquatable<ushort>, IComparable<LazinatorWrapperUshort>, IEquatable<LazinatorWrapperUshort>
    {
        public bool HasValue => true;

        public LazinatorWrapperUshort(ushort x) : this()
        {
            WrappedValue = x;
        }

        public static implicit operator LazinatorWrapperUshort(ushort x)
        {
            return new LazinatorWrapperUshort(x);
        }

        public static implicit operator ushort(LazinatorWrapperUshort x)
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
            if (obj is ushort v)
                return WrappedValue == v;
            else if (obj is LazinatorWrapperUshort w)
                return WrappedValue == w.WrappedValue;
            return false;
        }

        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;
            if (obj is LazinatorWrapperUshort other)
                return CompareTo(other);
            if (obj is ushort b)
                return CompareTo(b);
            throw new NotImplementedException();
        }

        public int CompareTo(ushort other)
        {
            return WrappedValue.CompareTo(other);
        }

        public bool Equals(ushort other)
        {
            return WrappedValue.Equals(other);
        }

        public int CompareTo(LazinatorWrapperUshort other)
        {
            return WrappedValue.CompareTo(other.WrappedValue);
        }

        public bool Equals(LazinatorWrapperUshort other)
        {
            return WrappedValue.Equals(other.WrappedValue);
        }
    }
}
