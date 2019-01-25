using System;

namespace LazinatorCollectionsTests
{
    public partial struct StructWithBadHashFunction : IStructWithBadHashFunction, IComparable, IComparable<int>, IEquatable<int>, IComparable<StructWithBadHashFunction>, IEquatable<StructWithBadHashFunction>
    {
        public bool HasValue => true;

        public StructWithBadHashFunction(int x) : this()
        {
            WrappedValue = x;
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static implicit operator StructWithBadHashFunction(int x)
        {
            return new StructWithBadHashFunction(x);
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static implicit operator int(StructWithBadHashFunction x)
        {
            return x.WrappedValue;
        }

        public override string ToString()
        {
            return WrappedValue.ToString();
        }

        public override int GetHashCode()
        {
            if (WrappedValue % 2 == 0)
                return -1; // assign many items to same hash for testing purposes
            return WrappedValue.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is int v)
                return WrappedValue == v;
            else if (obj is StructWithBadHashFunction w)
                return WrappedValue == w.WrappedValue;
            return false;
        }

        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;
            if (obj is StructWithBadHashFunction other)
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

        public int CompareTo(StructWithBadHashFunction other)
        {
            return WrappedValue.CompareTo(other.WrappedValue);
        }

        public bool Equals(StructWithBadHashFunction other)
        {
            return WrappedValue.Equals(other.WrappedValue);
        }
    }
}
