using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperNullableTimeSpan : ILazinatorWrapperNullableTimeSpan
    {
        public bool HasValue => Value != null;

        public static implicit operator LazinatorWrapperNullableTimeSpan(TimeSpan? x)
        {
            return new LazinatorWrapperNullableTimeSpan() { Value = x };
        }

        public static implicit operator TimeSpan? (LazinatorWrapperNullableTimeSpan x)
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
            if (obj is TimeSpan v)
                return Value == v;
            else if (obj is LazinatorWrapperNullableTimeSpan w)
                return Value == w.Value;
            return false;
        }
    }
}