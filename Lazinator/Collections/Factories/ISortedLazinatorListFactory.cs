using Lazinator.Core;
using Lazinator.Attributes;
using System;

namespace Lazinator.Collections.Factories
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.ISortedLazinatorListFactory)]
    public interface ISortedLazinatorListFactory<T> where T : ILazinator, IComparable<T>
    {
    }
}