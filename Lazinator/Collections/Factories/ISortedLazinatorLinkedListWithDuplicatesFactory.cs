using Lazinator.Core;
using Lazinator.Attributes;
using System;

namespace Lazinator.Collections.Factories
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.ISortedLazinatorLinkedListWithDuplicatesFactory)]
    public interface ISortedLazinatorLinkedListWithDuplicatesFactory<T> where T : ILazinator, IComparable<T>
    {
    }
}