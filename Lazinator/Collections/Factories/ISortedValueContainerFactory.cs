using Lazinator.Core;
using Lazinator.Attributes;
using System;

namespace Lazinator.Collections.Factories
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.ISortedValueContainerFactory)]
    public interface ISortedValueContainerFactory<T> where T : ILazinator, IComparable<T>
    {
    }
}