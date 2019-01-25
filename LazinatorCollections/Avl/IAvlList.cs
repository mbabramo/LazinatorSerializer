using Lazinator.Attributes;
using LazinatorCollections.Factories;
using LazinatorCollections.Interfaces;
using Lazinator.Core;
using Lazinator.Wrappers;

namespace LazinatorCollections.Avl
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.IAvlList)]
    interface IAvlList<T> where T : ILazinator
    {
        IIndexableValueContainer<T> UnderlyingTree { get; set; }
    }
}
