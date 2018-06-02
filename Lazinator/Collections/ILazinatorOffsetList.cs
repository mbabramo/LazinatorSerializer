using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.Attributes;

namespace Lazinator.Collections
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorOffsetList)]
    interface ILazinatorOffsetList
    {
        LazinatorFastReadList<short> TwoByteItems { get; set; }
        LazinatorFastReadList<int> FourByteItems { get; set; }
    }
}
