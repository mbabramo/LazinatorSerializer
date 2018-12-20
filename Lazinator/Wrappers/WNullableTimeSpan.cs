using System;

namespace Lazinator.Wrappers
{
    public partial struct WNullableTimeSpan : IWNullableTimeSpan
    {
        public bool HasValue => WrappedValue != null;

        public WNullableTimeSpan(TimeSpan? x) : this()
        {
            WrappedValue = x;
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static implicit operator WNullableTimeSpan(TimeSpan? x)
        {
            return new WNullableTimeSpan(x);
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static implicit operator TimeSpan? (WNullableTimeSpan x)
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
            if (obj is WNullableTimeSpan w)
                return WrappedValue == w.WrappedValue;
            if (obj is TimeSpan v)
                return WrappedValue == v;
            if (obj == null)
                return WrappedValue == null;
            return false;
        }
    }
}