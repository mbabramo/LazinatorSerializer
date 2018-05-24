using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperNullableGuid : ILazinatorWrapperNullableGuid
    {
        public bool HasValue => WrappedValue != null;

        public LazinatorWrapperNullableGuid(Guid? x) : this()
        {
            WrappedValue = x;
        }

        public static implicit operator LazinatorWrapperNullableGuid(Guid? x)
        {
            return new LazinatorWrapperNullableGuid(x);
        }

        public static implicit operator Guid? (LazinatorWrapperNullableGuid x)
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
            if (obj is LazinatorWrapperNullableGuid w)
                return WrappedValue == w.WrappedValue;
            if (obj is Guid v)
                return WrappedValue == v;
            if (obj == null)
                return WrappedValue == null;
            return false;
        }
    }
}