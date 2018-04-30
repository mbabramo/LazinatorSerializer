using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperDateTime : ILazinatorWrapperDateTime
    {
        public static implicit operator LazinatorWrapperDateTime(DateTime x)
        {
            return new LazinatorWrapperDateTime() { Value = x };
        }

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
