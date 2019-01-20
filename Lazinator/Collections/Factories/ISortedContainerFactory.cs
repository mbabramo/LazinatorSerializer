using Lazinator.Core;
using Lazinator.Attributes;
using System;

namespace Lazinator.Collections.Factories
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.ISortedContainerFactory)]
    internal interface ISortedContainerFactory<T> where T : ILazinator, IComparable<T>
    {
        SortedContainerFactory<T> SortedInnerFactory { get; set; }
    }
}