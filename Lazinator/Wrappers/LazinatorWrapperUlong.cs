using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperUlong : ILazinatorWrapperUlong, IComparable, IComparable<ulong>, IEquatable<ulong>, IComparable<LazinatorWrapperUlong>, IEquatable<LazinatorWrapperUlong>
    {
        public bool HasValue => true;

        public LazinatorWrapperUlong(ulong x) : this()
        {
            Value = x;
        }

        public static implicit operator LazinatorWrapperUlong(ulong x)
        {
            return new LazinatorWrapperUlong(x);
        }

        public static implicit operator ulong(LazinatorWrapperUlong x)
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
            if (obj is ulong v)
                return Value == v;
            else if (obj is LazinatorWrapperUlong w)
                return Value == w.Value;
            return false;
        }

        public int CompareTo(object obj)
        {
            return Value.CompareTo(obj);
        }

        public int CompareTo(ulong other)
        {
            return Value.CompareTo(other);
        }

        public bool Equals(ulong other)
        {
            return Value.Equals(other);
        }

        public int CompareTo(LazinatorWrapperUlong other)
        {
            return Value.CompareTo(other.Value);
        }

        public bool Equals(LazinatorWrapperUlong other)
        {
            return Value.Equals(other.Value);
        }
    }
}
