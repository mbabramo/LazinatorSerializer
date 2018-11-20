using Lazinator.Core;
using Lazinator.Attributes;
using Lazinator.Collections.Avl;

namespace Lazinator.Collections
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorQueue)]
    interface ILazinatorQueue<T> where T : ILazinator
    {
        LazinatorList<T> UnderlyingList { get; set; }
    }
}