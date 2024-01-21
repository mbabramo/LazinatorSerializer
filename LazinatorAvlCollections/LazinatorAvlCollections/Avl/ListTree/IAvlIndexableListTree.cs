using Lazinator.Core;
using Lazinator.Attributes;
using System;
using LazinatorAvlCollections.Avl.ValueTree;
using LazinatorAvlCollections.Factories;
using Lazinator.Collections.Interfaces;
using Lazinator.Collections;

namespace LazinatorAvlCollections.Avl.ListTree
{
    [Lazinator((int)LazinatorAvlCollectionUniqueIDs.IAvlIndexableListTree)]
    internal interface IAvlIndexableListTree<T> where T : ILazinator
    {
        [SetterAccessibility("protected")]
        bool AllowDuplicates { get; }
        [SetterAccessibility("protected")]
        bool Unbalanced { get; }
        AvlAggregatedTree<IIndexableMultivalueContainer<T>> UnderlyingTree { get; set; }
        ContainerFactory InnerContainerFactory { get; set; }
        [SetterAccessibility("protected")]
        T InnermostFirst { get; }
        [SetterAccessibility("protected")]
        T InnermostLast { get; }
    }
}