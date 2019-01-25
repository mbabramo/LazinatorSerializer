using Lazinator.Core;
using Lazinator.Attributes;
using LazinatorCollections.Tuples;
using LazinatorCollections.Avl.ValueTree;
using LazinatorCollections.Interfaces;
using LazinatorCollections.Factories;

namespace LazinatorCollections.Avl.KeyValueTree
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
        ContainerFactory InnerContainerFactory { get; set; }
        IMultivalueContainer<LazinatorKeyValue<TKey, TValue>> UnderlyingContainer { get; set; }
    }
}