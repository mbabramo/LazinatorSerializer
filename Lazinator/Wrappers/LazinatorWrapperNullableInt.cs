﻿using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperNullableInt : ILazinatorWrapperNullableInt
    {
        public static implicit operator LazinatorWrapperNullableInt(int? x)
        {
            return new LazinatorWrapperNullableInt() { Value = x };
        }

        public override int GetHashCode()
        {
            return Value?.GetHashCode() ?? 0;
        }

        public override bool Equals(object obj)
        {
            var other = (LazinatorWrapperNullableInt)obj;
            return Equals(Value, other.Value);
        }
    }
}