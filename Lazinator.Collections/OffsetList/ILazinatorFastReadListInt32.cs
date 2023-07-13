using Lazinator.Attributes;
using System;

namespace Lazinator.Collections.OffsetList
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.ILazinatorFastReadListInt32, -1, true)]
    internal interface ILazinatorFastReadListInt32 : ILazinatorFastReadList<Int32>
    {
    }
}