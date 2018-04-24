using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperGuid : ILazinatorWrapperGuid
    {
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var other = (LazinatorWrapperGuid)obj;
            return Value == other.Value;
        }
    }
}