using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperNullableSByte : ILazinatorWrapperNullableSByte
    {
        public bool IsNull => Value == null;

        public static implicit operator LazinatorWrapperNullableSByte(sbyte? x)
        {
            return new LazinatorWrapperNullableSByte() { Value = x };
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
            if (obj is sbyte v)
                return Value == v;
            else if (obj is LazinatorWrapperNullableSByte w)
                return Value == w.Value;
            return false;
        }
    }
}