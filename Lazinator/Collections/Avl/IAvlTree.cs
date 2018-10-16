using Lazinator.Attributes;
using Lazinator.Core;

namespace Lazinator.Collections.Avl
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.AvlTree)]
    interface IAvlTree<TKey, TValue> where TKey : ILazinator, new() where TValue : ILazinator, new()
    {
        AvlNode<TKey, TValue> Root { get; set; }
    }
}
