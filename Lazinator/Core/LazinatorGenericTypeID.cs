using System;
using System.Collections.Generic;
using System.Linq;

namespace Lazinator.Core
{
    /// <summary>
    /// Contains information about generic Lazinator types, including inner generics. The code-behind will include a property returning the LazinatorGenericIDType for the Lazinator type, using a method that caches this information,
    /// to minimize the need for reflection.
    /// </summary>
    public readonly struct LazinatorGenericIDType
    {
        public LazinatorGenericIDType(List<int> typeAndInnerTypeIDs)
        {
            TypeAndInnerTypeIDs = typeAndInnerTypeIDs;
        }

        public bool IsEmpty => TypeAndInnerTypeIDs == null || !TypeAndInnerTypeIDs.Any();

        public readonly List<int> TypeAndInnerTypeIDs;

        public override int GetHashCode()
        {
            int res = 0x2D2816FE;
            foreach (var item in TypeAndInnerTypeIDs)
            {
                res = res * 31 + (item.GetHashCode());
            }
            return res;
        }

        public override bool Equals(object obj)
        {
            if (obj is LazinatorGenericIDType other)
            {
                if (IsEmpty && other.IsEmpty)
                    return true;
                if (IsEmpty != other.IsEmpty)
                    return false;
                return TypeAndInnerTypeIDs.SequenceEqual(other.TypeAndInnerTypeIDs);
            }
            return false;
        }

        /// <summary>
        /// A container with a cached LazinatorGenericIDType for each class. This allows us to avoid allocating a new LazinatorGenericIDType each time we access the LazinatorGenericID property.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        static class CachedPerType<T>
        {
            public static LazinatorGenericIDType? GenericIDType;
        }

        public static LazinatorGenericIDType GetCachedForType<T>(Func<LazinatorGenericIDType> func)
        {
            LazinatorGenericIDType? t = CachedPerType<T>.GenericIDType;
            if (t == null)
            {
                t = func();
                CachedPerType<T>.GenericIDType = t;
            }
            return t.Value;
        }
    }
}
