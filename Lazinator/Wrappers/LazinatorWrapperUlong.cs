﻿using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperUlong : ILazinatorWrapperUlong, IComparable, IComparable<ulong>, IEquatable<ulong>, IComparable<LazinatorWrapperUlong>, IEquatable<LazinatorWrapperUlong>
    {
        public static implicit operator LazinatorWrapperUlong(ulong x)
        {
            return new LazinatorWrapperUlong() { Value = x };
        }

        public static implicit operator ulong(LazinatorWrapperUlong x)
        {
            return x.Value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var other = (LazinatorWrapperUlong)obj;
            return Value == other.Value;
        }

        public int CompareTo(object obj)
        {
            return Value.CompareTo(obj);
        }

        public int CompareTo(ulong other)
        {
            return Value.CompareTo(other);
        }

        public bool Equals(ulong other)
        {
            return Value.Equals(other);
        }

        public int CompareTo(LazinatorWrapperUlong other)
        {
            return Value.CompareTo(other.Value);
        }

        public bool Equals(LazinatorWrapperUlong other)
        {
            return Value.Equals(other.Value);
        }
    }
}
