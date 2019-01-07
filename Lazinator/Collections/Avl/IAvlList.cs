using Lazinator.Attributes;
using Lazinator.Collections.Factories;
using Lazinator.Collections.Interfaces;
using Lazinator.Core;
using Lazinator.Wrappers;

namespace Lazinator.Collections.Avl
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.IAvlList)]
    interface IAvlList<T> where T : ILazinator
    {
        IIndexableContainer<T> UnderlyingTree { get; set; }
    }
}
