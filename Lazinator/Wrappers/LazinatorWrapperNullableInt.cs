using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperNullableInt : ILazinatorWrapperNullableInt
    {
        public bool HasValue => WrappedValue != null;

        public LazinatorWrapperNullableInt(int? x) : this()
        {
            WrappedValue = x;
        }

        public static implicit operator LazinatorWrapperNullableInt(int? x)
        {
            return new LazinatorWrapperNullableInt(x);
        }

        public static implicit operator int? (LazinatorWrapperNullableInt x)
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
            if (obj is LazinatorWrapperNullableInt w)
                return WrappedValue == w.WrappedValue;
            if (obj is int v)
                return WrappedValue == v;
            if (obj == null)
                return WrappedValue == null;
            return false;
        }
    }
}