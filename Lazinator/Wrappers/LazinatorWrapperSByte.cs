﻿using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperSByte : ILazinatorWrapperSByte, IComparable, IComparable<sbyte>, IEquatable<sbyte>, IComparable<LazinatorWrapperSByte>, IEquatable<LazinatorWrapperSByte>
    {
        public bool HasValue => true;

        public LazinatorWrapperSByte(sbyte x) : this()
        {
            Value = x;
        }

        public static implicit operator LazinatorWrapperSByte(sbyte x)
        {
            return new LazinatorWrapperSByte(x);
        }

        public static implicit operator sbyte(LazinatorWrapperSByte x)
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
            if (obj is sbyte v)
                return Value == v;
            else if (obj is LazinatorWrapperSByte w)
                return Value == w.Value;
            return false;
        }

        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;
            if (obj is LazinatorWrapperSByte other)
                return CompareTo(other);
            if (obj is sbyte b)
                return CompareTo(b);
            throw new NotImplementedException();
        }

        public int CompareTo(sbyte other)
        {
            return Value.CompareTo(other);
        }

        public bool Equals(sbyte other)
        {
            return Value.Equals(other);
        }

        public int CompareTo(LazinatorWrapperSByte other)
        {
            return Value.CompareTo(other.Value);
        }

        public bool Equals(LazinatorWrapperSByte other)
        {
            return Value.Equals(other.Value);
        }
    }
}
