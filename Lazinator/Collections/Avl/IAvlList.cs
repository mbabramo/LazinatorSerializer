using Lazinator.Attributes;
using Lazinator.Core;
using Lazinator.Wrappers;

namespace Lazinator.Collections.Avl
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.IAvlList)]
    interface IAvlList<T> where T : ILazinator
    {
        ILazinatorOrderedKeyable<Placeholder, T> UnderlyingTree { get; set; }
    }
}
