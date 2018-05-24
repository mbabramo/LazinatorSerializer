using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperBool : ILazinatorWrapperBool
    {
        public bool HasValue => true;

        public LazinatorWrapperBool(bool x) : this()
        {
            WrappedValue = x;
        }

        public static implicit operator LazinatorWrapperBool(bool x)
        {
            return new LazinatorWrapperBool(x);
        }

        public static implicit operator bool(LazinatorWrapperBool x)
        {
            return x.WrappedValue;
        }

        public override string ToString()
        {
            return WrappedValue.ToString();
        }

        public override int GetHashCode()
        {
            return WrappedValue.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is bool v)
                return WrappedValue == v;
            else if (obj is LazinatorWrapperBool w)
                return WrappedValue == w.WrappedValue;
            return false;
        }
    }
}
