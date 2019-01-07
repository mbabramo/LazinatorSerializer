using Lazinator.Attributes;
using Lazinator.Collections.Factories;
using Lazinator.Core;
using Lazinator.Wrappers;

namespace Lazinator.Collections.Avl
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.IAvlList)]
    interface IAvlList<T> where T : ILazinator
    {
        AvlIndexableTree<T> UnderlyingTree { get; set; }
    }
}
