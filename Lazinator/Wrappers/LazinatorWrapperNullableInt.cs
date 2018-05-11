using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperNullableInt : ILazinatorWrapperNullableInt, IComparable, IComparable<int?>, IEquatable<int?>, IComparable<LazinatorWrapperNullableInt>, IEquatable<LazinatorWrapperNullableInt>
    {
        public bool HasValue => Value != null;

        public static implicit operator LazinatorWrapperNullableInt(int? x)
        {
            return new LazinatorWrapperNullableInt() { Value = x };
        }

        public static implicit operator int? (LazinatorWrapperNullableInt x)
        {
            return x.Value;
        }

        public override string ToString()
        {
            return Value?.ToString() ?? "";
        }

        public override int GetHashCode()
        {
            return Value?.GetHashCode() ?? 0;
        }

        public override bool Equals(object obj)
        {
            if (obj is int v)
                return Value == v;
            else if (obj is LazinatorWrapperNullableInt w)
                return Value == w.Value;
            return false;
        }

        public int CompareTo(object obj)
        {
            LazinatorWrapperNullableInt other = obj as LazinatorWrapperNullableInt;
            //Compare nulls acording MSDN specification

            //Two nulls are equal
            if (!HasValue && !other.HasValue)
                return 0;

            //Any object is greater than null
            if (HasValue && !other.HasValue)
                return 1;

            if (other.HasValue && !HasValue)
                return -1;

            //Otherwise compare the two values
            return Value.CompareTo(other.Value);
        }

        public int CompareTo(int other)
        {
            return Value.CompareTo(other);
        }

        public bool Equals(int other)
        {
            return Value.Equals(other);
        }

        public int CompareTo(LazinatorWrapperInt other)
        {
            return Value.CompareTo(other.Value);
        }

        public bool Equals(LazinatorWrapperInt other)
        {
            return Value.Equals(other.Value);
        }
    }
}