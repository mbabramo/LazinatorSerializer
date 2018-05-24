using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperNullableBool : ILazinatorWrapperNullableBool
    {
        public bool HasValue => WrappedValue != null;

        public LazinatorWrapperNullableBool(bool? x) : this()
        {
            WrappedValue = x;
        }

        public override string ToString()
        {
            return WrappedValue?.ToString() ?? "";
        }

        public static implicit operator LazinatorWrapperNullableBool(bool? x)
        {
            return new LazinatorWrapperNullableBool(x);
        }

        public static implicit operator bool?(LazinatorWrapperNullableBool x)
        {
            return x.WrappedValue;
        }

        public override int GetHashCode()
        {
            return WrappedValue?.GetHashCode() ?? 0;
        }

        public override bool Equals(object obj)
        {
            if (obj is LazinatorWrapperNullableBool w)
                return WrappedValue == w.WrappedValue;
            if (obj is bool v)
                return WrappedValue == v;
            if (obj == null)
                return WrappedValue == null;
            return false;
        }
    }
}