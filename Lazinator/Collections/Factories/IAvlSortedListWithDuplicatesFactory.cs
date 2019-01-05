using Lazinator.Core;
using Lazinator.Attributes;
using System;

namespace Lazinator.Collections.Factories
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.IAvlSortedListWithDuplicatesFactory)]
    public interface IAvlSortedListWithDuplicatesFactory<T> where T : ILazinator, IComparable<T>
    {
    }
}