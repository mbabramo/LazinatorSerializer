using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperNullableDecimal : ILazinatorWrapperNullableDecimal
    {
        public bool HasValue => WrappedValue != null;

        public LazinatorWrapperNullableDecimal(decimal? x) : this()
        {
            WrappedValue = x;
        }

        public static implicit operator LazinatorWrapperNullableDecimal(decimal? x)
        {
            return new LazinatorWrapperNullableDecimal(x);
        }

        public static implicit operator decimal? (LazinatorWrapperNullableDecimal x)
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
            if (obj is LazinatorWrapperNullableDecimal w)
                return WrappedValue == w.WrappedValue;
            if (obj is decimal v)
                return WrappedValue == v;
            if (obj == null)
                return WrappedValue == null;
            return false;
        }
    }
}