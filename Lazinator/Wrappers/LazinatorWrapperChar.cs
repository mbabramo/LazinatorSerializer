using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperChar : ILazinatorWrapperChar, IComparable, IComparable<char>, IEquatable<char>, IComparable<LazinatorWrapperChar>, IEquatable<LazinatorWrapperChar>
    {
        public bool HasValue => true;

        public LazinatorWrapperChar(char x) : this()
        {
            Value = x;
        }

        public static implicit operator LazinatorWrapperChar(char x)
        {
            return new LazinatorWrapperChar(x);
        }

        public static implicit operator char(LazinatorWrapperChar x)
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
            if (obj is char v)
                return Value == v;
            else if (obj is LazinatorWrapperChar w)
                return Value == w.Value;
            return false;
        }

        public int CompareTo(object obj)
        {
            return Value.CompareTo(obj);
        }

        public int CompareTo(char other)
        {
            return Value.CompareTo(other);
        }

        public bool Equals(char other)
        {
            return Value.Equals(other);
        }

        public int CompareTo(LazinatorWrapperChar other)
        {
            return Value.CompareTo(other.Value);
        }

        public bool Equals(LazinatorWrapperChar other)
        {
            return Value.Equals(other.Value);
        }
    }
}
