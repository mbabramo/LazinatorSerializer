using Lazinator.Core;
using Lazinator.Attributes;

namespace LazinatorCollections
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.ILazinatorQueue)]
    interface ILazinatorQueue<T> where T : ILazinator
    {
        ILazinatorListable<T> UnderlyingList { get; set; }
    }
}