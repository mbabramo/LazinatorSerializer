﻿using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperNullableInt : ILazinatorWrapperNullableInt
    {
        public bool HasValue => Value != null;

        public LazinatorWrapperNullableInt(int? x) : this()
        {
            Value = x;
        }

        public static implicit operator LazinatorWrapperNullableInt(int? x)
        {
            return new LazinatorWrapperNullableInt(x);
        }

        public static implicit operator int? (LazinatorWrapperNullableInt x)
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
            if (obj is LazinatorWrapperNullableInt w)
                return Value == w.Value;
            if (obj is int v)
                return Value == v;
            if (obj == null)
                return Value == null;
            return false;
        }
    }
}