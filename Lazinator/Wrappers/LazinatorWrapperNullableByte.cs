using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperNullableByte : ILazinatorWrapperNullableByte
    {
        public bool IsNull => Value == null;

        public static implicit operator LazinatorWrapperNullableByte(byte? x)
        {
            return new LazinatorWrapperNullableByte() { Value = x };
        }

        public static implicit operator byte? (LazinatorWrapperNullableByte x)
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
            if (obj is byte v)
                return Value == v;
            else if (obj is LazinatorWrapperNullableByte w)
                return Value == w.Value;
            return false;
        }
    }
}