using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperNullableGuid : ILazinatorWrapperNullableGuid
    {
        public static implicit operator LazinatorWrapperNullableGuid(Guid? x)
        {
            return new LazinatorWrapperNullableGuid() { Value = x };
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
            if (obj is Guid v)
                return Value == v;
            else if (obj is LazinatorWrapperNullableGuid w)
                return Value == w.Value;
            return false;
        }
    }
}