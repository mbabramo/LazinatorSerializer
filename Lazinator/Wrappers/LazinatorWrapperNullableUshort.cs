using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperNullableUshort : ILazinatorWrapperNullableUshort
    {
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var other = (LazinatorWrapperNullableUshort)obj;
            return Equals(Value, other.Value);
        }
    }
}