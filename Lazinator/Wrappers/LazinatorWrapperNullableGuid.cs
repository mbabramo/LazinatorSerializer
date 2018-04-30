using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperNullableGuid : ILazinatorWrapperNullableGuid
    {
        public override int GetHashCode()
        {
            return Value?.GetHashCode() ?? 0;
        }

        public override bool Equals(object obj)
        {
            var other = (LazinatorWrapperNullableGuid)obj;
            return Equals(Value, other.Value);
        }
    }
}