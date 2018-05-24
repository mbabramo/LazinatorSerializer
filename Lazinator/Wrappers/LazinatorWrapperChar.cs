using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperChar : ILazinatorWrapperChar, IComparable, IComparable<char>, IEquatable<char>, IComparable<LazinatorWrapperChar>, IEquatable<LazinatorWrapperChar>
    {
        public bool HasValue => true;

        public LazinatorWrapperChar(char x) : this()
        {
            WrappedValue = x;
        }

        public static implicit operator LazinatorWrapperChar(char x)
        {
            return new LazinatorWrapperChar(x);
        }

        public static implicit operator char(LazinatorWrapperChar x)
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
            if (obj is char v)
                return WrappedValue == v;
            else if (obj is LazinatorWrapperChar w)
                return WrappedValue == w.WrappedValue;
            return false;
        }



        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;
            if (obj is LazinatorWrapperChar other)
                return CompareTo(other);
            if (obj is char b)
                return CompareTo(b);
            throw new NotImplementedException();
        }

        public int CompareTo(char other)
        {
            return WrappedValue.CompareTo(other);
        }

        public bool Equals(char other)
        {
            return WrappedValue.Equals(other);
        }

        public int CompareTo(LazinatorWrapperChar other)
        {
            return WrappedValue.CompareTo(other.WrappedValue);
        }

        public bool Equals(LazinatorWrapperChar other)
        {
            return WrappedValue.Equals(other.WrappedValue);
        }
    }
}
