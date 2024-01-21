using Lazinator.Core;
using Lazinator.Attributes;
using Lazinator.Collections.Tuples;
using LazinatorAvlCollections.Avl.ValueTree;
using Lazinator.Collections.Interfaces;
using LazinatorAvlCollections.Factories;
using Lazinator.Collections;

namespace LazinatorAvlCollections.Avl.KeyValueTree
{
    [Lazinator((int)LazinatorAvlCollectionUniqueIDs.IAvlKeyValueTree)]
    public interface IAvlKeyValueTree<TKey, TValue>
        where TKey : ILazinator
        where TValue : ILazinator
    {
        [SetterAccessibility("protected")]
        bool AllowDuplicates { get; }
        [SetterAccessibility("protected")]
        bool Unbalanced { get; }
        ContainerFactory InnerContainerFactory { get; set; }
        IMultivalueContainer<LazinatorKeyValue<TKey, TValue>> UnderlyingContainer { get; set; }
    }
}