using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperNullableInt : ILazinatorWrapperNullableInt
    {
        public static implicit operator LazinatorWrapperNullableInt(int? x)
        {
            return new LazinatorWrapperNullableInt() { Value = x };
        }

        public static implicit operator int? (LazinatorWrapperNullableInt x)
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
            if (obj is int v)
                return Value == v;
            else if (obj is LazinatorWrapperNullableInt w)
                return Value == w.Value;
            return false;
        }
    }
}