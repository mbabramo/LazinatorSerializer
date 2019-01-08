using Lazinator.Core;
using Lazinator.Attributes;
using Lazinator.Collections.Tuples;

namespace Lazinator.Collections.Avl
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.IAvlKeyValueTree)]
    public interface IAvlKeyValueTree<TKey, TValue>
        where TKey : ILazinator
        where TValue : ILazinator
    {
        AvlTree<LazinatorKeyValue<TKey, TValue>> UnderlyingTree { get; set; }
    }
}