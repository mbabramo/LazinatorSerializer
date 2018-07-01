using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lazinator.Core
{
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
    }
}
