using Lazinator.Attributes;
using LazinatorAvlCollections.Factories;
using Lazinator.Collections.Interfaces;
using Lazinator.Core;
using Lazinator.Wrappers;

namespace LazinatorAvlCollections.Avl
{
    [Lazinator((int)LazinatorAvlCollectionUniqueIDs.IAvlList)]
    interface IAvlList<T> where T : ILazinator
    {
        IIndexableValueContainer<T> UnderlyingTree { get; set; }
    }
}
