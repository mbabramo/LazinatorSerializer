﻿using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperChar : ILazinatorWrapperChar
    {
        public static implicit operator LazinatorWrapperChar(char x)
        {
            return new LazinatorWrapperChar() { Value = x };
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var other = (LazinatorWrapperChar)obj;
            return Value == other.Value;
        }
    }
}
