using Lazinator.Attributes;
using System;

namespace Lazinator.Collections.OffsetList
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.ILazinatorFastReadListInt32, -1, false)]
    internal interface ILazinatorFastReadListInt32 : ILazinatorFastReadList<Int32>
    {
    }
}