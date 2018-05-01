﻿using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperUshort : ILazinatorWrapperUshort, IComparable
    {
        public static implicit operator LazinatorWrapperUshort(ushort x)
        {
            return new LazinatorWrapperUshort() { Value = x };
        }

        public static implicit operator ushort(LazinatorWrapperUshort x)
        {
            return x.Value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var other = (LazinatorWrapperUshort)obj;
            return Value == other.Value;
        }

        public int CompareTo(object obj)
        {
            return Value.CompareTo(obj);
        }
    }
}
