using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperFloat : ILazinatorWrapperFloat, IComparable, IComparable<float>, IEquatable<float>, IComparable<LazinatorWrapperFloat>, IEquatable<LazinatorWrapperFloat>
    {
        public bool IsNull => false;

        public static implicit operator LazinatorWrapperFloat(float x)
        {
            return new LazinatorWrapperFloat() { Value = x };
        }

        public static implicit operator float(LazinatorWrapperFloat x)
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
            if (obj is float v)
                return Value == v;
            else if (obj is LazinatorWrapperFloat w)
                return Value == w.Value;
            return false;
        }

        public int CompareTo(object obj)
        {
            return Value.CompareTo(obj);
        }

        public int CompareTo(float other)
        {
            return Value.CompareTo(other);
        }

        public bool Equals(float other)
        {
            return Value.Equals(other);
        }

        public int CompareTo(LazinatorWrapperFloat other)
        {
            return Value.CompareTo(other.Value);
        }

        public bool Equals(LazinatorWrapperFloat other)
        {
            return Value.Equals(other.Value);
        }
    }
}
