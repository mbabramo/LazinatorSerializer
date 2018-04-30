using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperNullableByte : ILazinatorWrapperNullableByte
    {
        public override int GetHashCode()
        {
            return Value?.GetHashCode() ?? 0;
        }

        public override bool Equals(object obj)
        {
            var other = (LazinatorWrapperNullableByte)obj;
            return Equals(Value, other.Value);
        }
    }
}