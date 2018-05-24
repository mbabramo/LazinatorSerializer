using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperNullableFloat : ILazinatorWrapperNullableFloat
    {
        public bool HasValue => WrappedValue != null;

        public LazinatorWrapperNullableFloat(float? x) : this()
        {
            WrappedValue = x;
        }

        public static implicit operator LazinatorWrapperNullableFloat(float? x)
        {
            return new LazinatorWrapperNullableFloat(x);
        }

        public static implicit operator float? (LazinatorWrapperNullableFloat x)
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
            if (obj is LazinatorWrapperNullableFloat w)
                return WrappedValue == w.WrappedValue;
            if (obj is float v)
                return WrappedValue == v;
            if (obj == null)
                return WrappedValue == null;
            return false;
        }
    }
}