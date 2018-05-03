using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperBool : ILazinatorWrapperBool
    {
        public static implicit operator LazinatorWrapperBool(bool x)
        {
            return new LazinatorWrapperBool() {Value = x};
        }

        public static implicit operator bool(LazinatorWrapperBool x)
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
            var other = (LazinatorWrapperBool)obj;
            return Value == other.Value;
        }
    }
}
