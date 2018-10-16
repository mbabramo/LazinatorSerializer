using Lazinator.Attributes;
using Lazinator.Core;
using Lazinator.Wrappers;

namespace Lazinator.Collections.Avl
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.AvlSet)]
    interface IAvlSet<TKey> where TKey : ILazinator, new()
    {
        int Count { get; set; }
        AvlTree<TKey, WByte> UnderlyingTree { get; set; }
    }
}
