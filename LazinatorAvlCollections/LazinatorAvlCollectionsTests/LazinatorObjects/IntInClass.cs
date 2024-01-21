using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorAvlCollectionsTests.LazinatorObjects
{
    public partial class IntInClass : IIntInClass, IComparable<IntInClass>
    {
        public IntInClass(int x)
        {
            WrappedValue = x;
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static implicit operator IntInClass(int x)
        {
            return new IntInClass(x);
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static implicit operator int(IntInClass x)
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
            if (obj is int v)
                return WrappedValue == v;
            else if (obj is IntInClass w)
                return WrappedValue == w.WrappedValue;
            return false;
        }

        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;
            if (obj is IntInClass other)
                return CompareTo(other);
            if (obj is int b)
                return CompareTo(b);
            throw new NotImplementedException();
        }

        public int CompareTo(int other)
        {
            return WrappedValue.CompareTo(other);
        }

        public bool Equals(int other)
        {
            return WrappedValue.Equals(other);
        }

        public int CompareTo(IntInClass other)
        {
            return WrappedValue.CompareTo(other.WrappedValue);
        }

        public bool Equals(IntInClass other)
        {
            return WrappedValue.Equals(other.WrappedValue);
        }
    }
}
