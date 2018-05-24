using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperNullableShort : ILazinatorWrapperNullableShort
    {
        public bool HasValue => WrappedValue != null;

        public LazinatorWrapperNullableShort(short? x) : this()
        {
            WrappedValue = x;
        }

        public static implicit operator LazinatorWrapperNullableShort(short? x)
        {
            return new LazinatorWrapperNullableShort(x);
        }

        public static implicit operator short? (LazinatorWrapperNullableShort x)
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
            if (obj is LazinatorWrapperNullableShort w)
                return WrappedValue == w.WrappedValue;
            if (obj is short v)
                return WrappedValue == v;
            if (obj == null)
                return WrappedValue == null;
            return false;
        }
    }
}