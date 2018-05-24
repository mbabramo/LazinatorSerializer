using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Wrappers
{
    public abstract partial class LazinatorWrapperAbstract<T> : ILazinatorWrapperAbstract<T>
    {
        public override string ToString()
        {
            return Wrapped?.ToString() ?? "";
        }

        public override int GetHashCode()
        {
            return Wrapped.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var other = (LazinatorWrapperAbstract<T>)obj;
            if (Wrapped == null)
                return other.Wrapped == null;
            return Wrapped.Equals(other.Wrapped);
        }

        public bool Equals(T other)
        {
            if (Wrapped == null)
                return other == null;
            return Wrapped.Equals(other);
        }

        public bool Equals(LazinatorWrapperAbstract<T> other)
        {
            if (Wrapped == null)
                return other.Wrapped == null;
            return Wrapped.Equals(other.Wrapped);
        }

    }
}
