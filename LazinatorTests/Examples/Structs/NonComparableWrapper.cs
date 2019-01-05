using System;

namespace LazinatorTests.Examples
{
    public partial struct NonComparableWrapper : INonComparableWrapper
    {
        public bool HasValue => true;

        public NonComparableWrapper(int x) : this()
        {
            WrappedValue = x;
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static implicit operator NonComparableWrapper(int x)
        {
            return new NonComparableWrapper(x);
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static implicit operator int(NonComparableWrapper x)
        {
            return x.WrappedValue;
        }

        public override string ToString()
        {
            return WrappedValue.ToString();
        }

        public override bool Equals(object obj)
        {
            if (obj is int v)
                return WrappedValue == v;
            else if (obj is NonComparableWrapper w)
                return WrappedValue == w.WrappedValue;
            return false;
        }

        public bool Equals(int other)
        {
            return WrappedValue.Equals(other);
        }

        public int CompareTo(NonComparableWrapper other)
        {
            return WrappedValue.CompareTo(other.WrappedValue);
        }
    }
}
