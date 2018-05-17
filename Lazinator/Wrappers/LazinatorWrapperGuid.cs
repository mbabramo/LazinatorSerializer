using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperGuid : ILazinatorWrapperGuid
    {
        public bool HasValue => true;

        public LazinatorWrapperGuid(Guid x) : this()
        {
            Value = x;
        }

        public static implicit operator LazinatorWrapperGuid(Guid x)
        {
            return new LazinatorWrapperGuid() { Value = x };
        }

        public static implicit operator Guid(LazinatorWrapperGuid x)
        {
            return x.Value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is Guid v)
                return Value == v;
            else if (obj is LazinatorWrapperGuid w)
                return Value == w.Value;
            return false;
        }
    }
}