﻿using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperNullableShort : ILazinatorWrapperNullableShort
    {
        public static implicit operator LazinatorWrapperNullableShort(short? x)
        {
            return new LazinatorWrapperNullableShort() { Value = x };
        }

        public static implicit operator short? (LazinatorWrapperNullableShort x)
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
            if (obj is short v)
                return Value == v;
            else if (obj is LazinatorWrapperNullableShort w)
                return Value == w.Value;
            return false;
        }
    }
}