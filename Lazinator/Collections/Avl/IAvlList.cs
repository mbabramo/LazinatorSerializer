using Lazinator.Attributes;
using Lazinator.Core;
using Lazinator.Wrappers;

namespace Lazinator.Collections.Avl
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.AvlList)]
    interface IAvlList<T> where T : ILazinator
    {
        AvlTree<Placeholder, T> UnderlyingTree { get; set; }
    }
}
