using Lazinator.Core;
using Lazinator.Attributes;
using System;

namespace Lazinator.Collections.Factories
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.ISortedContainerFactory)]
    internal interface ISortedContainerFactory where T : ILazinator, IComparable<T>
    {
        SortedContainerFactory SortedInnerFactory { get; set; }
    }
}