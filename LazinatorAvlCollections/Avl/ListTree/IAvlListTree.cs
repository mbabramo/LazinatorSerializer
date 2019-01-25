using Lazinator.Core;
using Lazinator.Attributes;
using System;
using LazinatorAvlCollections.Avl.ValueTree;
using LazinatorAvlCollections.Factories;
using LazinatorCollections.Interfaces;
using LazinatorCollections;

namespace LazinatorAvlCollections.Avl.ListTree
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.IAvlListTree)]
    internal interface IAvlListTree<T> where T : ILazinator
    {
        [SetterAccessibility("protected")]
        bool AllowDuplicates { get; }
        [SetterAccessibility("protected")]
        bool Unbalanced { get; }
        AvlTree<IMultivalueContainer<T>> UnderlyingTree { get; set; }
        ContainerFactory InnerContainerFactory { get; set; }
    }
}