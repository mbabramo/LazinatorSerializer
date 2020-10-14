using Lazinator.Attributes;
using System;

namespace LazinatorCollections.OffsetList
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.ILazinatorFastReadListInt16, -1, false)]
    internal interface ILazinatorFastReadListInt16 : ILazinatorFastReadList<Int16>
    {
    }
}