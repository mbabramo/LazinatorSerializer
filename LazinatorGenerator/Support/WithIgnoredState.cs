using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorGenerator.Support
{
    /// <summary>
    /// Adds state to a struct, but ignores the state when comparing equality.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    public struct WithIgnoredState<T, U>
    {
        public T Item;
        public U ExtraInfo;

        public WithIgnoredState(T item, U extraInfo)
        {
            Item = item;
            ExtraInfo = extraInfo;
        }

        public static bool operator==(WithIgnoredState<T, U> first, WithIgnoredState<T, U> second)
        {
            return first.Item.Equals(second.Item);
        }

        public static bool operator !=(WithIgnoredState<T, U> first, WithIgnoredState<T, U> second)
        {
            return !first.Item.Equals(second.Item);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is WithIgnoredState<T, U> other))
                return false;
            return other.Item.Equals(Item);
        }

        public override int GetHashCode()
        {
            return Item.GetHashCode();
        }
    }
}
