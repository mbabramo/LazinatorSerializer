using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperNullableByte : ILazinatorWrapperNullableByte
    {
        public bool HasValue => Value != null;

        public LazinatorWrapperNullableByte(byte? x) : this()
        {
            Value = x;
        }

        public static implicit operator LazinatorWrapperNullableByte(byte? x)
        {
            return new LazinatorWrapperNullableByte(x);
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
            if (obj is LazinatorWrapperNullableByte w)
                return Value == w.Value;
            if (obj is byte v)
                return Value == v;
            if (obj == null)
                return Value == null;
            return false;
        }
    }
}