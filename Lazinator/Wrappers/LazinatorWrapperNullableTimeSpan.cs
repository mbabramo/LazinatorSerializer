using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperNullableTimeSpan : ILazinatorWrapperNullableTimeSpan
    {
        public bool HasValue => WrappedValue != null;

        public LazinatorWrapperNullableTimeSpan(TimeSpan? x) : this()
        {
            WrappedValue = x;
        }

        public static implicit operator LazinatorWrapperNullableTimeSpan(TimeSpan? x)
        {
            return new LazinatorWrapperNullableTimeSpan(x);
        }

        public static implicit operator TimeSpan? (LazinatorWrapperNullableTimeSpan x)
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
            if (obj is LazinatorWrapperNullableTimeSpan w)
                return WrappedValue == w.WrappedValue;
            if (obj is TimeSpan v)
                return WrappedValue == v;
            if (obj == null)
                return WrappedValue == null;
            return false;
        }
    }
}