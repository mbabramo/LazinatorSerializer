using Lazinator.Core;
using Lazinator.Attributes;

namespace Lazinator.Collections
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.ILazinatorQueue)]
    interface ILazinatorQueue<T> where T : ILazinator
    {
        ILazinatorListable<T> UnderlyingList { get; set; }
    }
}