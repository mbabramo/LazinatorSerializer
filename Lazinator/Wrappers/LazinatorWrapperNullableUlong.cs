using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperNullableUlong : ILazinatorWrapperNullableUlong
    {
        public bool IsNull => Value == null;

        public static implicit operator LazinatorWrapperNullableUlong(ulong? x)
        {
            return new LazinatorWrapperNullableUlong() { Value = x };
        }

        public static implicit operator ulong? (LazinatorWrapperNullableUlong x)
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
            if (obj is ulong v)
                return Value == v;
            else if (obj is LazinatorWrapperNullableUlong w)
                return Value == w.Value;
            return false;
        }
    }
}