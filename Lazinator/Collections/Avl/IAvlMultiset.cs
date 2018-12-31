using Lazinator.Attributes;
using Lazinator.Core;
using Lazinator.Wrappers;

namespace Lazinator.Collections.Avl
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.AvlMultiset)]
    interface IAvlMultiset<T> where T : ILazinator
    {
        AvlSet<LazinatorTuple<T, WInt>> UnderlyingSet { get; set; }
        int NumItemsAdded { get; set; }
    }
}
