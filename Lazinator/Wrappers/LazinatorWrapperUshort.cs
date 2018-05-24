using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperUshort : ILazinatorWrapperUshort, IComparable, IComparable<ushort>, IEquatable<ushort>, IComparable<LazinatorWrapperUshort>, IEquatable<LazinatorWrapperUshort>
    {
        public bool HasValue => true;

        public LazinatorWrapperUshort(ushort x) : this()
        {
            Value = x;
        }

        public static implicit operator LazinatorWrapperUshort(ushort x)
        {
            return new LazinatorWrapperUshort(x);
        }

        public static implicit operator ushort(LazinatorWrapperUshort x)
        {
            return x.Value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is ushort v)
                return Value == v;
            else if (obj is LazinatorWrapperUshort w)
                return Value == w.Value;
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
            return Value.CompareTo(other);
        }

        public bool Equals(ushort other)
        {
            return Value.Equals(other);
        }

        public int CompareTo(LazinatorWrapperUshort other)
        {
            return Value.CompareTo(other.Value);
        }

        public bool Equals(LazinatorWrapperUshort other)
        {
            return Value.Equals(other.Value);
        }
    }
}
