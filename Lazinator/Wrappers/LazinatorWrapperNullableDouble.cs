﻿using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperNullableDouble : ILazinatorWrapperNullableDouble
    {
        public bool HasValue => Value != null;

        public LazinatorWrapperNullableDouble(double? x) : this()
        {
            Value = x;
        }

        public static implicit operator LazinatorWrapperNullableDouble(double? x)
        {
            return new LazinatorWrapperNullableDouble() { Value = x };
        }

        public static implicit operator double? (LazinatorWrapperNullableDouble x)
        {
            return x.Value;
        }

        public override string ToString()
        {
            return Value?.ToString() ?? "";
        }

        public override int GetHashCode()
        {
            return Value?.GetHashCode() ?? 0;
        }

        public override bool Equals(object obj)
        {
            if (obj is LazinatorWrapperNullableDouble w)
                return Value == w.Value;
            if (obj is double v)
                return Value == v;
            if (obj == null)
                return Value == null;
            return false;
        }
    }
}