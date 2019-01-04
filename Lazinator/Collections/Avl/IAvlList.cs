using Lazinator.Attributes;
using Lazinator.Core;
using Lazinator.Wrappers;

namespace Lazinator.Collections.Avl
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.IAvlList)]
    interface IAvlList<T> where T : ILazinator
    {
        ILazinatorKeyable<Placeholder, T> UnderlyingTree { get; set; }
        ILazinatorOrderedKeyable<Placeholder, T> UnderlyingTree2 { get; set; }
    }
}
