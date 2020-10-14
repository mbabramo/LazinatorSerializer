using Lazinator.Attributes;
using System;

namespace LazinatorCollections.OffsetList
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.ILazinatorFastReadListInt32, -1, false)]
    internal interface ILazinatorFastReadListInt32 : ILazinatorFastReadList<Int32>
    {
    }
}