using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Wrappers
{
    public abstract partial class LazinatorWrapperAbstract<T> : ILazinatorWrapperAbstract<T>
    {
        public override string ToString()
        {
            return Value?.ToString() ?? "";
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var other = (LazinatorWrapperAbstract<T>)obj;
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

        public bool Equals(LazinatorWrapperAbstract<T> other)
        {
            if (Value == null)
                return other.Value == null;
            return Value.Equals(other.Value);
        }

    }
}
