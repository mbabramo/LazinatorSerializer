using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Core
{
    public class LazinatorGenericIDType
    {
        public LazinatorGenericIDType(List<int> typeAndInnerTypeIDs)
        {
            TypeAndInnerTypeIDs = typeAndInnerTypeIDs;
        }

        public List<int> TypeAndInnerTypeIDs;

        public override int GetHashCode()
        {
            int res = 0x2D2816FE;
            foreach (var item in TypeAndInnerTypeIDs)
            {
                res = res * 31 + (item.GetHashCode());
            }
            return res;
        }
    }
}
