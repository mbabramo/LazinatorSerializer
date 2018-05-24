using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperNullableLong : ILazinatorWrapperNullableLong
    {
        public bool HasValue => WrappedValue != null;

        public LazinatorWrapperNullableLong(long? x) : this()
        {
            WrappedValue = x;
        }

        public static implicit operator LazinatorWrapperNullableLong(long? x)
        {
            return new LazinatorWrapperNullableLong(x);
        }

        public static implicit operator long? (LazinatorWrapperNullableLong x)
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
            if (obj is LazinatorWrapperNullableLong w)
                return WrappedValue == w.WrappedValue;
            if (obj is long v)
                return WrappedValue == v;
            if (obj == null)
                return WrappedValue == null;
            return false;
        }
    }
}