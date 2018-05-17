using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperNullableSByte : ILazinatorWrapperNullableSByte
    {
        public bool HasValue => Value != null;

        public LazinatorWrapperNullableSByte(sbyte? x) : this()
        {
            Value = x;
        }

        public static implicit operator LazinatorWrapperNullableSByte(sbyte? x)
        {
            return new LazinatorWrapperNullableSByte(x);
        }

        public static implicit operator sbyte? (LazinatorWrapperNullableSByte x)
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
            if (obj is LazinatorWrapperNullableSByte w)
                return Value == w.Value;
            if (obj is sbyte v)
                return Value == v;
            if (obj == null)
                return Value == null;
            return false;
        }
    }
}