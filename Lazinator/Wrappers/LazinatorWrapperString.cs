using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperString : ILazinatorWrapperString
    {
        public static implicit operator LazinatorWrapperString(string x)
        {
            return new LazinatorWrapperString() { Value = x };
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var other = (LazinatorWrapperString)obj;
            return Value == other.Value;
        }
    }
}