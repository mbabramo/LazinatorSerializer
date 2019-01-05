
using Lazinator.Attributes;
using Lazinator.Collections.Factories;
using Lazinator.Collections.Tuples;
using Lazinator.Core;
using System;

namespace Lazinator.Collections.Avl
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.IAvlListNodeTree)]
    interface IAvlListNodeTree<TKey, TValue> where TKey : ILazinator, IComparable<TKey> where TValue : ILazinator
    {
        AvlTree<LazinatorKeyValue<TKey, TValue>, AvlListNodeContents<TKey, TValue>> UnderlyingTree { get; set; }
        ILazinatorSortableFactory<LazinatorKeyValue<TKey, TValue>> SortableListFactory { get; set; }
        int MaxItemsPerNode { get; set; }
        bool AllowDuplicates { get; set; }
    }
}
