using Lazinator.Core;
using Lazinator.Attributes;
using System;

namespace Lazinator.Collections.Factories
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.ISortedLazinatorLinkedListFactory)]
    public interface ISortedLazinatorLinkedListFactory<T> where T : ILazinator, IComparable<T>
    {
    }
}