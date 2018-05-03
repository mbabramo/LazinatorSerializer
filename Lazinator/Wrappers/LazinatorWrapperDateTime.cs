using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperDateTime : ILazinatorWrapperDateTime, IComparable, IComparable<DateTime>, IEquatable<DateTime>, IComparable<LazinatorWrapperDateTime>, IEquatable<LazinatorWrapperDateTime>
    {
        public static implicit operator LazinatorWrapperDateTime(DateTime x)
        {
            return new LazinatorWrapperDateTime() { Value = x };
        }

        public static implicit operator DateTime(LazinatorWrapperDateTime x)
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
            var other = (LazinatorWrapperDateTime)obj;
            return Value == other.Value;
        }

        public int CompareTo(object obj)
        {
            return Value.CompareTo(obj);
        }

        public int CompareTo(DateTime other)
        {
            return Value.CompareTo(other);
        }

        public bool Equals(DateTime other)
        {
            return Value.Equals(other);
        }

        public int CompareTo(LazinatorWrapperDateTime other)
        {
            return Value.CompareTo(other.Value);
        }

        public bool Equals(LazinatorWrapperDateTime other)
        {
            return Value.Equals(other.Value);
        }
    }
}
