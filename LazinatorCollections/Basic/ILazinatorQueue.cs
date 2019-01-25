using Lazinator.Core;
using Lazinator.Attributes;
using LazinatorCollections.Avl;

namespace LazinatorCollections
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.ILazinatorQueue)]
    interface ILazinatorQueue<T> where T : ILazinator
    {
        LazinatorList<T> UnderlyingList { get; set; }
    }
}