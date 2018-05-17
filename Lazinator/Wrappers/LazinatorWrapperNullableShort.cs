using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperNullableShort : ILazinatorWrapperNullableShort
    {
        public bool HasValue => Value != null;

        public LazinatorWrapperNullableShort(short? x) : this()
        {
            Value = x;
        }

        public static implicit operator LazinatorWrapperNullableShort(short? x)
        {
            return new LazinatorWrapperNullableShort() { Value = x };
        }

        public static implicit operator short? (LazinatorWrapperNullableShort x)
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
            if (obj is LazinatorWrapperNullableShort w)
                return Value == w.Value;
            if (obj is short v)
                return Value == v;
            if (obj == null)
                return Value == null;
            return false;
        }
    }
}