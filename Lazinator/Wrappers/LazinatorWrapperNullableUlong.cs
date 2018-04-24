using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperNullableUlong : ILazinatorWrapperNullableUlong
    {
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var other = (LazinatorWrapperNullableUlong)obj;
            return Equals(Value, other.Value);
        }
    }
}