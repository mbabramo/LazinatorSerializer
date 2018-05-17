using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperNullableUshort : ILazinatorWrapperNullableUshort
    {
        public bool HasValue => Value != null;

        public LazinatorWrapperNullableUshort(ushort? x) : this()
        {
            Value = x;
        }

        public static implicit operator LazinatorWrapperNullableUshort(ushort? x)
        {
            return new LazinatorWrapperNullableUshort() { Value = x };
        }

        public static implicit operator ushort? (LazinatorWrapperNullableUshort x)
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
            if (obj is LazinatorWrapperNullableUshort w)
                return Value == w.Value;
            if (obj is ushort v)
                return Value == v;
            if (obj == null)
                return Value == null;
            return false;
        }
    }
}