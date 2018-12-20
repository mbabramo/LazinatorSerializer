using Lazinator.Attributes;
using Lazinator.Core;

namespace Lazinator.Collections.BigList
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.BigListNode)]
    public interface IBigListNode<T> where T : ILazinator
    {
        int MaxLeafCount { get; set; }
        long Count { get; set; }
    }
}