using Lazinator.Core;
using Lazinator.Attributes;
using System;

namespace Lazinator.Collections.Factories
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.ISortedLazinatorListWithDuplicatesFactory)]
    public interface ISortedLazinatorListWithDuplicatesFactory<T> where T : ILazinator, IComparable<T>
    {
    }
}