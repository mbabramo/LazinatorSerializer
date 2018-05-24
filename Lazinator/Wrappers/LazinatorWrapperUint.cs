using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperUint : ILazinatorWrapperUint, IComparable, IComparable<uint>, IEquatable<uint>, IComparable<LazinatorWrapperUint>, IEquatable<LazinatorWrapperUint>
    {
        public bool HasValue => true;

        public LazinatorWrapperUint(uint x) : this()
        {
            WrappedValue = x;
        }

        public static implicit operator LazinatorWrapperUint(uint x)
        {
            return new LazinatorWrapperUint(x);
        }

        public static implicit operator uint(LazinatorWrapperUint x)
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
            if (obj is uint v)
                return WrappedValue == v;
            else if (obj is LazinatorWrapperUint w)
                return WrappedValue == w.WrappedValue;
            return false;
        }

        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;
            if (obj is LazinatorWrapperUint other)
                return CompareTo(other);
            if (obj is uint b)
                return CompareTo(b);
            throw new NotImplementedException();
        }

        public int CompareTo(uint other)
        {
            return WrappedValue.CompareTo(other);
        }

        public bool Equals(uint other)
        {
            return WrappedValue.Equals(other);
        }

        public int CompareTo(LazinatorWrapperUint other)
        {
            return WrappedValue.CompareTo(other.WrappedValue);
        }

        public bool Equals(LazinatorWrapperUint other)
        {
            return WrappedValue.Equals(other.WrappedValue);
        }
    }
}
