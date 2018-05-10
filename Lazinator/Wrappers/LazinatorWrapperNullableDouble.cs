using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperNullableDouble : ILazinatorWrapperNullableDouble
    {
        public bool HasValue => Value != null;

        public static implicit operator LazinatorWrapperNullableDouble(double? x)
        {
            return new LazinatorWrapperNullableDouble() { Value = x };
        }

        public static implicit operator double? (LazinatorWrapperNullableDouble x)
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
            if (obj is double v)
                return Value == v;
            else if (obj is LazinatorWrapperNullableDouble w)
                return Value == w.Value;
            return false;
        }
    }
}