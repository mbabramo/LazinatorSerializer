using Lazinator.Core;
using Lazinator.Attributes;
using LazinatorCollections.ListDerivatives.Avl;

namespace LazinatorCollections.ListDerivatives
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.ILazinatorQueue)]
    interface ILazinatorQueue<T> where T : ILazinator
    {
        LazinatorList<T> UnderlyingList { get; set; }
    }
}