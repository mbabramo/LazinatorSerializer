using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperNullableFloat : ILazinatorWrapperNullableFloat
    {
        public bool HasValue => Value != null;

        public LazinatorWrapperNullableFloat(float? x) : this()
        {
            Value = x;
        }

        public static implicit operator LazinatorWrapperNullableFloat(float? x)
        {
            return new LazinatorWrapperNullableFloat(x);
        }

        public static implicit operator float? (LazinatorWrapperNullableFloat x)
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
            if (obj is LazinatorWrapperNullableFloat w)
                return Value == w.Value;
            if (obj is float v)
                return Value == v;
            if (obj == null)
                return Value == null;
            return false;
        }
    }
}