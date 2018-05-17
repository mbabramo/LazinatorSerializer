﻿using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperNullableDecimal : ILazinatorWrapperNullableDecimal
    {
        public bool HasValue => Value != null;

        public LazinatorWrapperNullableDecimal(decimal? x) : this()
        {
            Value = x;
        }

        public static implicit operator LazinatorWrapperNullableDecimal(decimal? x)
        {
            return new LazinatorWrapperNullableDecimal(x);
        }

        public static implicit operator decimal? (LazinatorWrapperNullableDecimal x)
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
            if (obj is LazinatorWrapperNullableDecimal w)
                return Value == w.Value;
            if (obj is decimal v)
                return Value == v;
            if (obj == null)
                return Value == null;
            return false;
        }
    }
}