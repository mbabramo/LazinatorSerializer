﻿using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperTimeSpan : ILazinatorWrapperTimeSpan, IComparable, IComparable<TimeSpan>, IEquatable<TimeSpan>, IComparable<LazinatorWrapperTimeSpan>, IEquatable<LazinatorWrapperTimeSpan>
    {
        public static implicit operator LazinatorWrapperTimeSpan(TimeSpan x)
        {
            return new LazinatorWrapperTimeSpan() { Value = x };
        }

        public static implicit operator TimeSpan(LazinatorWrapperTimeSpan x)
        {
            return x.Value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var other = (LazinatorWrapperTimeSpan)obj;
            return Value == other.Value;
        }

        public int CompareTo(object obj)
        {
            return ((IComparable)Value).CompareTo(obj);
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
