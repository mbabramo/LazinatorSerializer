using Lazinator.Attributes;
using LazinatorAvlCollections.Factories;
using LazinatorCollections.Interfaces;
using Lazinator.Core;
using Lazinator.Wrappers;

namespace LazinatorAvlCollections.Avl
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.IAvlList)]
    interface IAvlList<T> where T : ILazinator
    {
        IIndexableValueContainer<T> UnderlyingTree { get; set; }
    }
}
