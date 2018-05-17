using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperNullableBool : ILazinatorWrapperNullableBool
    {
        public bool HasValue => Value != null;

        public LazinatorWrapperNullableBool(bool? x) : this()
        {
            Value = x;
        }

        public override string ToString()
        {
            return Value?.ToString() ?? "";
        }

        public static implicit operator LazinatorWrapperNullableBool(bool? x)
        {
            return new LazinatorWrapperNullableBool(x);
        }

        public static implicit operator bool?(LazinatorWrapperNullableBool x)
        {
            return x.Value;
        }

        public override int GetHashCode()
        {
            return Value?.GetHashCode() ?? 0;
        }

        public override bool Equals(object obj)
        {
            if (obj is LazinatorWrapperNullableBool w)
                return Value == w.Value;
            if (obj is bool v)
                return Value == v;
            if (obj == null)
                return Value == null;
            return false;
        }
    }
}