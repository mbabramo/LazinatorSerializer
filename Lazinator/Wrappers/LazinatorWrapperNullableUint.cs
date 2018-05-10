using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperNullableUint : ILazinatorWrapperNullableUint
    {
        public bool IsNull => Value == null;

        public static implicit operator LazinatorWrapperNullableUint(uint? x)
        {
            return new LazinatorWrapperNullableUint() { Value = x };
        }

        public static implicit operator uint? (LazinatorWrapperNullableUint x)
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
            if (obj is uint v)
                return Value == v;
            else if (obj is LazinatorWrapperNullableUint w)
                return Value == w.Value;
            return false;
        }
    }
}