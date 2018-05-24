using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperTimeSpan : ILazinatorWrapperTimeSpan, IComparable, IComparable<TimeSpan>, IEquatable<TimeSpan>, IComparable<LazinatorWrapperTimeSpan>, IEquatable<LazinatorWrapperTimeSpan>
    {
        public bool HasValue => true;

        public LazinatorWrapperTimeSpan(TimeSpan x) : this()
        {
            Value = x;
        }

        public static implicit operator LazinatorWrapperTimeSpan(TimeSpan x)
        {
            return new LazinatorWrapperTimeSpan(x);
        }

        public static implicit operator TimeSpan(LazinatorWrapperTimeSpan x)
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
            if (obj is TimeSpan v)
                return Value == v;
            else if (obj is LazinatorWrapperTimeSpan w)
                return Value == w.Value;
            return false;
        }

        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;
            if (obj is LazinatorWrapperTimeSpan other)
                return CompareTo(other);
            if (obj is TimeSpan b)
                return CompareTo(b);
            throw new NotImplementedException();
        }

        public int CompareTo(TimeSpan other)
        {
            return Value.CompareTo(other);
        }

        public bool Equals(TimeSpan other)
        {
            return Value.Equals(other);
        }

        public int CompareTo(LazinatorWrapperTimeSpan other)
        {
            return Value.CompareTo(other.Value);
        }

        public bool Equals(LazinatorWrapperTimeSpan other)
        {
            return Value.Equals(other.Value);
        }
    }
}
