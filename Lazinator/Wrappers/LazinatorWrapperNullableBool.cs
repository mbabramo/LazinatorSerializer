using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperNullableBool : ILazinatorWrapperNullableBool
    {
        public static implicit operator LazinatorWrapperNullableBool(bool? x)
        {
            return new LazinatorWrapperNullableBool() { Value = x };
        }

        public static implicit operator bool?(LazinatorWrapperNullableBool x)
        {
            return x.Value;
        }

        public override int GetHashCode()
        {
            return Value?.GetHashCode() ?? 0;
        }

        public override bool Equals(object obj)
        {
            var other = (LazinatorWrapperNullableBool)obj;
            return Equals(Value, other.Value);
        }
    }
}