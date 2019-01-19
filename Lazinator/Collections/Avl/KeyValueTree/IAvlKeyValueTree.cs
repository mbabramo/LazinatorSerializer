using Lazinator.Core;
using Lazinator.Attributes;
using Lazinator.Collections.Tuples;
using Lazinator.Collections.Avl.ValueTree;

namespace Lazinator.Collections.Avl.KeyValueTree
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.IAvlKeyValueTree)]
    public interface IAvlKeyValueTree<TKey, TValue>
        where TKey : ILazinator
        where TValue : ILazinator
    {
        [SetterAccessibility("protected")]
        bool AllowDuplicates { get; }
        [SetterAccessibility("protected")]
        bool Unbalanced { get; }
        AvlTree<LazinatorKeyValue<TKey, TValue>> UnderlyingTree { get; set; }
    }
}