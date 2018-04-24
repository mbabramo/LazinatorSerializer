using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperDateTime : ILazinatorWrapperDateTime
    {
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var other = (LazinatorWrapperDateTime)obj;
            return Value == other.Value;
        }
    }
}
