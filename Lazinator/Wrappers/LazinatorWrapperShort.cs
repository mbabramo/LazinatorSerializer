using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperShort : ILazinatorWrapperShort, IComparable, IComparable<short>, IEquatable<short>, IComparable<LazinatorWrapperShort>, IEquatable<LazinatorWrapperShort>
    {
        public bool HasValue => true;

        public LazinatorWrapperShort(short x) : this()
        {
            Value = x;
        }

        public static implicit operator LazinatorWrapperShort(short x)
        {
            return new LazinatorWrapperShort(x);
        }

        public static implicit operator short(LazinatorWrapperShort x)
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
            if (obj is short v)
                return Value == v;
            else if (obj is LazinatorWrapperShort w)
                return Value == w.Value;
            return false;
        }

        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;
            if (obj is LazinatorWrapperShort other)
                return CompareTo(other);
            if (obj is short b)
                return CompareTo(b);
            throw new NotImplementedException();
        }

        public int CompareTo(short other)
        {
            return Value.CompareTo(other);
        }

        public bool Equals(short other)
        {
            return Value.Equals(other);
        }

        public int CompareTo(LazinatorWrapperShort other)
        {
            return Value.CompareTo(other.Value);
        }

        public bool Equals(LazinatorWrapperShort other)
        {
            return Value.Equals(other.Value);
        }
    }
}
