using System;

namespace Lazinator.Wrappers
{
    public partial struct WChar : IWChar, IComparable, IComparable<char>, IEquatable<char>, IComparable<WChar>, IEquatable<WChar>
    {
        public bool HasValue => true;

        public WChar(char x) : this()
        {
            WrappedValue = x;
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static implicit operator WChar(char x)
        {
            return new WChar(x);
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static implicit operator char(WChar x)
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
            else if (obj is WChar w)
                return WrappedValue == w.WrappedValue;
            return false;
        }



        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;
            if (obj is WChar other)
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

        public int CompareTo(WChar other)
        {
            return WrappedValue.CompareTo(other.WrappedValue);
        }

        public bool Equals(WChar other)
        {
            return WrappedValue.Equals(other.WrappedValue);
        }
    }
}
