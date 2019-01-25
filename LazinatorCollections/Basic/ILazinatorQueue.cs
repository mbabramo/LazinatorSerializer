using Lazinator.Core;
using Lazinator.Attributes;
namespace LazinatorCollections.Basic.Avl;

namespace LazinatorCollections.Basic.Basic
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.ILazinatorQueue)]
    interface ILazinatorQueue<T> where T : ILazinator
    {
        LazinatorList<T> UnderlyingList { get; set; }
    }
}