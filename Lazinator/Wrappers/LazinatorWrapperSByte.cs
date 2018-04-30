﻿using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperSByte : ILazinatorWrapperSByte
    {
        public static implicit operator LazinatorWrapperSByte(sbyte x)
        {
            return new LazinatorWrapperSByte() { Value = x };
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var other = (LazinatorWrapperSByte)obj;
            return Value == other.Value;
        }
    }
}
