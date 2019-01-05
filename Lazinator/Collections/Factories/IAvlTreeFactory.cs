using Lazinator.Core;
using Lazinator.Attributes;
using System;
using System.Collections.Generic;

namespace Lazinator.Collections.Factories
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.IAvlTreeFactory)]
    public interface IAvlTreeFactory<TKey, TValue>
        where TKey : ILazinator, IComparable<TKey>
        where TValue : ILazinator
    {
    }
}