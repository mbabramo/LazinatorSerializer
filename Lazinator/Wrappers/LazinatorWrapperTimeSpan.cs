using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperTimeSpan : ILazinatorWrapperTimeSpan
    {
        public static implicit operator LazinatorWrapperTimeSpan(TimeSpan x)
        {
            return new LazinatorWrapperTimeSpan() { Value = x };
        }

        public static implicit operator TimeSpan(LazinatorWrapperTimeSpan x)
        {
            return x.Value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var other = (LazinatorWrapperTimeSpan)obj;
            return Value == other.Value;
        }
    }
}
