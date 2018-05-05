using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperString : ILazinatorWrapperString, IComparable, IComparable<string>, IEquatable<string>, IComparable<LazinatorWrapperString>, IEquatable<LazinatorWrapperString>
    {
        public static implicit operator LazinatorWrapperString(string x)
        {
            return new LazinatorWrapperString() { Value = x };
        }

        public static implicit operator string(LazinatorWrapperString x)
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
            if (obj is string v)
                return Value == v;
            else if (obj is LazinatorWrapperString w)
                return Value == w.Value;
            return false;
        }

        public int CompareTo(object obj)
        {
            return Value.CompareTo(obj);
        }

        public int CompareTo(string other)
        {
            return Value.CompareTo(other);
        }

        public bool Equals(string other)
        {
            return Value.Equals(other);
        }

        public int CompareTo(LazinatorWrapperString other)
        {
            return Value.CompareTo(other.Value);
        }

        public bool Equals(LazinatorWrapperString other)
        {
            return Value.Equals(other.Value);
        }
    }
}