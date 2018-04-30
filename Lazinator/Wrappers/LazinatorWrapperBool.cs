using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperBool : ILazinatorWrapperBool
    {
        public static implicit operator LazinatorWrapperBool(bool x)
        {
            return new LazinatorWrapperBool() {Value = x};
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var other = (LazinatorWrapperBool)obj;
            return Value == other.Value;
        }
    }
}
