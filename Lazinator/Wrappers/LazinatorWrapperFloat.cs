using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperFloat : ILazinatorWrapperFloat, IComparable, IComparable<float>, IEquatable<float>, IComparable<LazinatorWrapperFloat>, IEquatable<LazinatorWrapperFloat>
    {
        public bool HasValue => true;

        public LazinatorWrapperFloat(float x) : this()
        {
            WrappedValue = x;
        }

        public static implicit operator LazinatorWrapperFloat(float x)
        {
            return new LazinatorWrapperFloat(x);
        }

        public static implicit operator float(LazinatorWrapperFloat x)
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
            if (obj is float v)
                return WrappedValue == v;
            else if (obj is LazinatorWrapperFloat w)
                return WrappedValue == w.WrappedValue;
            return false;
        }

        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;
            if (obj is LazinatorWrapperFloat other)
                return CompareTo(other);
            if (obj is float b)
                return CompareTo(b);
            throw new NotImplementedException();
        }

        public int CompareTo(float other)
        {
            return WrappedValue.CompareTo(other);
        }

        public bool Equals(float other)
        {
            return WrappedValue.Equals(other);
        }

        public int CompareTo(LazinatorWrapperFloat other)
        {
            return WrappedValue.CompareTo(other.WrappedValue);
        }

        public bool Equals(LazinatorWrapperFloat other)
        {
            return WrappedValue.Equals(other.WrappedValue);
        }
    }
}
