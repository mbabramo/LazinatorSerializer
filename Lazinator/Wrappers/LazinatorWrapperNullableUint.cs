using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperNullableUint : ILazinatorWrapperNullableUint
    {
        public bool HasValue => WrappedValue != null;

        public LazinatorWrapperNullableUint(uint? x) : this()
        {
            WrappedValue = x;
        }

        public static implicit operator LazinatorWrapperNullableUint(uint? x)
        {
            return new LazinatorWrapperNullableUint(x);
        }

        public static implicit operator uint? (LazinatorWrapperNullableUint x)
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
            if (obj is LazinatorWrapperNullableUint w)
                return WrappedValue == w.WrappedValue;
            if (obj is uint v)
                return WrappedValue == v;
            if (obj == null)
                return WrappedValue == null;
            return false;
        }
    }
}