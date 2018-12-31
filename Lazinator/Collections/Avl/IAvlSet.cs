using Lazinator.Attributes;
using Lazinator.Core;
using Lazinator.Wrappers;
using System;

namespace Lazinator.Collections.Avl
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.AvlSet)]
    interface IAvlSet<TKey> where TKey : ILazinator, IComparable<TKey>
    {
        int Count { get; set; }
        AvlTree<TKey, WByte> UnderlyingTree { get; set; }
    }
}
