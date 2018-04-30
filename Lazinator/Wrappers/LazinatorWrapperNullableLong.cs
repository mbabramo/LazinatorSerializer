using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperNullableLong : ILazinatorWrapperNullableLong
    {
        public override int GetHashCode()
        {
            return Value?.GetHashCode() ?? 0;
        }

        public override bool Equals(object obj)
        {
            var other = (LazinatorWrapperNullableLong)obj;
            return Equals(Value, other.Value);
        }
    }
}