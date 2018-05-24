using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperTimeSpan : ILazinatorWrapperTimeSpan, IComparable, IComparable<TimeSpan>, IEquatable<TimeSpan>, IComparable<LazinatorWrapperTimeSpan>, IEquatable<LazinatorWrapperTimeSpan>
    {
        public bool HasValue => true;

        public LazinatorWrapperTimeSpan(TimeSpan x) : this()
        {
            WrappedValue = x;
        }

        public static implicit operator LazinatorWrapperTimeSpan(TimeSpan x)
        {
            return new LazinatorWrapperTimeSpan(x);
        }

        public static implicit operator TimeSpan(LazinatorWrapperTimeSpan x)
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
            if (obj is TimeSpan v)
                return WrappedValue == v;
            else if (obj is LazinatorWrapperTimeSpan w)
                return WrappedValue == w.WrappedValue;
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
            return WrappedValue.CompareTo(other);
        }

        public bool Equals(TimeSpan other)
        {
            return WrappedValue.Equals(other);
        }

        public int CompareTo(LazinatorWrapperTimeSpan other)
        {
            return WrappedValue.CompareTo(other.WrappedValue);
        }

        public bool Equals(LazinatorWrapperTimeSpan other)
        {
            return WrappedValue.Equals(other.WrappedValue);
        }
    }
}
