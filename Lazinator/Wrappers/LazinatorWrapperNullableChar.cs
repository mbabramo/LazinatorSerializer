﻿using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperNullableChar : ILazinatorWrapperNullableChar
    {
        public override int GetHashCode()
        {
            return Value?.GetHashCode() ?? 0;
        }

        public override bool Equals(object obj)
        {
            var other = (LazinatorWrapperNullableChar)obj;
            return Equals(Value, other.Value);
        }
    }
}