using Lazinator.Core;
using Lazinator.Attributes;
using System;
using Lazinator.Collections.Avl.ValueTree;
using Lazinator.Collections.Factories;
using Lazinator.Collections.Interfaces;

namespace Lazinator.Collections.Avl.ListTree
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.IAvlListTree)]
    internal interface IAvlListTree<T> where T : ILazinator
    {
        [OnSet("", "\nAllowDuplicatesChanged(value);")]
        bool AllowDuplicates { get; set; }
        bool Unbalanced { get; set; }
        AvlIndexableTree<IMultivalueContainer<T>> UnderlyingTree { get; set; }
        ValueContainerFactory<T> InteriorContainerFactory { get; set; }
    }
}