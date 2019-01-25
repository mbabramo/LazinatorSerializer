using Lazinator.Core;
using Lazinator.Attributes;
using System;
using LazinatorCollections.Avl.ValueTree;
using LazinatorCollections.Factories;
using LazinatorCollections.Interfaces;

namespace LazinatorCollections.Avl.ListTree
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.IAvlIndexableListTree)]
    internal interface IAvlIndexableListTree<T> where T : ILazinator
    {
        [SetterAccessibility("protected")]
        bool AllowDuplicates { get; }
        [SetterAccessibility("protected")]
        bool Unbalanced { get; }
        AvlAggregatedTree<IIndexableMultivalueContainer<T>> UnderlyingTree { get; set; }
        ContainerFactory InnerContainerFactory { get; set; }
    }
}