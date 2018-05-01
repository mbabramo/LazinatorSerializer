﻿using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperGuid : ILazinatorWrapperGuid
    {
        public static implicit operator LazinatorWrapperGuid(Guid x)
        {
            return new LazinatorWrapperGuid() { Value = x };
        }

        public static implicit operator Guid(LazinatorWrapperGuid x)
        {
            return x.Value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var other = (LazinatorWrapperGuid)obj;
            return Value == other.Value;
        }
    }
}