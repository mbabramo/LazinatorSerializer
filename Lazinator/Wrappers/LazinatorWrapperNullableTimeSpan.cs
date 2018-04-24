﻿using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperNullableTimeSpan : ILazinatorWrapperNullableTimeSpan
    {
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var other = (LazinatorWrapperNullableTimeSpan)obj;
            return Equals(Value, other.Value);
        }
    }
}