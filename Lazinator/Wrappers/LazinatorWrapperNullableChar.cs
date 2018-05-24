using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperNullableChar : ILazinatorWrapperNullableChar
    {
        public bool HasValue => WrappedValue != null;

        public LazinatorWrapperNullableChar(char? x) : this()
        {
            WrappedValue = x;
        }

        public static implicit operator LazinatorWrapperNullableChar(char? x)
        {
            return new LazinatorWrapperNullableChar(x);
        }

        public static implicit operator char? (LazinatorWrapperNullableChar x)
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
            if (obj is LazinatorWrapperNullableChar w)
                return WrappedValue == w.WrappedValue;
            if (obj is char v)
                return WrappedValue == v;
            if (obj == null)
                return WrappedValue == null;
            return false;
        }
    }
}