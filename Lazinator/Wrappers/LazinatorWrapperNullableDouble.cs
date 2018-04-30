using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperNullableDouble : ILazinatorWrapperNullableDouble
    {
        public override int GetHashCode()
        {
            return Value?.GetHashCode() ?? 0;
        }

        public override bool Equals(object obj)
        {
            var other = (LazinatorWrapperNullableDouble)obj;
            return Equals(Value, other.Value);
        }
    }
}