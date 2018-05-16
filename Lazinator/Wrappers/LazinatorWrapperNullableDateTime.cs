using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperNullableDateTime : ILazinatorWrapperNullableDateTime
    {
        public bool HasValue => Value != null;

        public static implicit operator LazinatorWrapperNullableDateTime(DateTime? x)
        {
            return new LazinatorWrapperNullableDateTime() { Value = x };
        }

        public static implicit operator DateTime? (LazinatorWrapperNullableDateTime x)
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
            if (obj is LazinatorWrapperNullableDateTime w)
                return Value == w.Value;
            if (obj is DateTime v)
                return Value == v;
            if (obj == null)
                return Value == null;
            return false;
        }
    }
}