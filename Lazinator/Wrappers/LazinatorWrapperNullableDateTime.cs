﻿using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperNullableDateTime : ILazinatorWrapperNullableDateTime
    {
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var other = (LazinatorWrapperNullableDateTime)obj;
            return Equals(Value, other.Value);
        }
    }
}