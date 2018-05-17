using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperNullableChar : ILazinatorWrapperNullableChar
    {
        public bool HasValue => Value != null;

        public LazinatorWrapperNullableChar(char? x) : this()
        {
            Value = x;
        }

        public static implicit operator LazinatorWrapperNullableChar(char? x)
        {
            return new LazinatorWrapperNullableChar() { Value = x };
        }

        public static implicit operator char? (LazinatorWrapperNullableChar x)
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
            if (obj is LazinatorWrapperNullableChar w)
                return Value == w.Value;
            if (obj is char v)
                return Value == v;
            if (obj == null)
                return Value == null;
            return false;
        }
    }
}