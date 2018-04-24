using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperNullableFloat : ILazinatorWrapperNullableFloat
    {
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var other = (LazinatorWrapperNullableFloat)obj;
            return Equals(Value, other.Value);
        }
    }
}