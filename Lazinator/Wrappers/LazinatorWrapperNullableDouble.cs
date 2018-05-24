using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperNullableDouble : ILazinatorWrapperNullableDouble
    {
        public bool HasValue => WrappedValue != null;

        public LazinatorWrapperNullableDouble(double? x) : this()
        {
            WrappedValue = x;
        }

        public static implicit operator LazinatorWrapperNullableDouble(double? x)
        {
            return new LazinatorWrapperNullableDouble(x);
        }

        public static implicit operator double? (LazinatorWrapperNullableDouble x)
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
            if (obj is LazinatorWrapperNullableDouble w)
                return WrappedValue == w.WrappedValue;
            if (obj is double v)
                return WrappedValue == v;
            if (obj == null)
                return WrappedValue == null;
            return false;
        }
    }
}