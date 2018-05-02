using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Wrappers
{
    public abstract partial class LazinatorWrapper<T> : ILazinatorWrapper<T>
    {
        public T Value { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var other = (LazinatorWrapper<T>)obj;
            if (Value == null)
                return other.Value == null;
            return Value.Equals(other.Value);
        }

        public bool Equals(T other)
        {
            if (Value == null)
                return other == null;
            return Value.Equals(other);
        }

        public bool Equals(LazinatorWrapper<T> other)
        {
            if (Value == null)
                return other.Value == null;
            return Value.Equals(other.Value);
        }

    }
}
