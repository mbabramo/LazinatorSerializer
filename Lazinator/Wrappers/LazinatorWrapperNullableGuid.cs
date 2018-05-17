using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperNullableGuid : ILazinatorWrapperNullableGuid
    {
        public bool HasValue => Value != null;

        public LazinatorWrapperNullableGuid(Guid? x) : this()
        {
            Value = x;
        }

        public static implicit operator LazinatorWrapperNullableGuid(Guid? x)
        {
            return new LazinatorWrapperNullableGuid(x);
        }

        public static implicit operator Guid? (LazinatorWrapperNullableGuid x)
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
            if (obj is LazinatorWrapperNullableGuid w)
                return Value == w.Value;
            if (obj is Guid v)
                return Value == v;
            if (obj == null)
                return Value == null;
            return false;
        }
    }
}