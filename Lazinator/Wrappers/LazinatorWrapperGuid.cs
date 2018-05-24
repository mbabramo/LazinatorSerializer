using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperGuid : ILazinatorWrapperGuid
    {
        public bool HasValue => true;

        public LazinatorWrapperGuid(Guid x) : this()
        {
            WrappedValue = x;
        }

        public static implicit operator LazinatorWrapperGuid(Guid x)
        {
            return new LazinatorWrapperGuid(x);
        }

        public static implicit operator Guid(LazinatorWrapperGuid x)
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
            if (obj is Guid v)
                return WrappedValue == v;
            else if (obj is LazinatorWrapperGuid w)
                return WrappedValue == w.WrappedValue;
            return false;
        }
    }
}