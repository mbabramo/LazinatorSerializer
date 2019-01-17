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
        bool AllowDuplicates { get; set; }
        bool Unbalanced { get; set; }
        AvlIndexableTree<IMultivalueContainer<T>> UnderlyingTree2 { get; set; }
        AvlIndexableTree<ILazinatorListable<T>> UnderlyingTree { get; set; }
        ILazinatorListableFactory<T> ListableFactory { get; set; }
    }
}