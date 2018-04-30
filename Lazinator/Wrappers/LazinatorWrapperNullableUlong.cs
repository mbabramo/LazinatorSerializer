﻿using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperNullableUlong : ILazinatorWrapperNullableUlong
    {
        public static implicit operator LazinatorWrapperNullableUlong(ulong? x)
        {
            return new LazinatorWrapperNullableUlong() { Value = x };
        }

        public override int GetHashCode()
        {
            return Value?.GetHashCode() ?? 0;
        }

        public override bool Equals(object obj)
        {
            var other = (LazinatorWrapperNullableUlong)obj;
            return Equals(Value, other.Value);
        }
    }
}