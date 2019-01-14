using Lazinator.Core;
using Lazinator.Attributes;
using System;
using System.Collections.Generic;

namespace Lazinator.Collections.Factories
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.IAvlSortedKeyMultivalueTreeFactory)]
    public interface IAvlSortedKeyMultivalueTreeFactory<TKey, TValue> 
        where TKey : ILazinator, IComparable<TKey>
        where TValue : ILazinator
    {
        bool AllowDuplicates { get; set; }
    }
}