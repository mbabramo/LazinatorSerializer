﻿using System;

namespace Lazinator.Wrappers
{
    /// <summary>
    /// A Lazinator wrapper for a nullable Guid. A wrapper can be used as a generic type where the unwrapped object cannot be.
    /// </summary>
    public partial struct WNullableGuid : IWNullableGuid
    {
        public bool HasValue => WrappedValue != null;

        public WNullableGuid(Guid? x) : this()
        {
            WrappedValue = x;
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static implicit operator WNullableGuid(Guid? x)
        {
            return new WNullableGuid(x);
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static implicit operator Guid? (WNullableGuid x)
        {
            return x.WrappedValue;
        }

        public override string ToString()
        {
            return WrappedValue?.ToString() ?? "";
        }

        public override int GetHashCode()
        {
            return WrappedValue?.GetHashCode() ?? 0;
        }

        public override bool Equals(object obj)
        {
            if (obj is WNullableGuid w)
                return WrappedValue == w.WrappedValue;
            if (obj is Guid v)
                return WrappedValue == v;
            if (obj == null)
                return WrappedValue == null;
            return false;
        }
    }
}