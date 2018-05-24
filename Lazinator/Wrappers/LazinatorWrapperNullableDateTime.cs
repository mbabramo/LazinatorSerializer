using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperNullableDateTime : ILazinatorWrapperNullableDateTime
    {
        public bool HasValue => WrappedValue != null;

        public LazinatorWrapperNullableDateTime(DateTime? x) : this()
        {
            WrappedValue = x;
        }

        public static implicit operator LazinatorWrapperNullableDateTime(DateTime? x)
        {
            return new LazinatorWrapperNullableDateTime(x);
        }

        public static implicit operator DateTime? (LazinatorWrapperNullableDateTime x)
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
            if (obj is LazinatorWrapperNullableDateTime w)
                return WrappedValue == w.WrappedValue;
            if (obj is DateTime v)
                return WrappedValue == v;
            if (obj == null)
                return WrappedValue == null;
            return false;
        }
    }
}