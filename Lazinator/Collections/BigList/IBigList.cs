using Lazinator.Attributes;
using Lazinator.Core;

namespace Lazinator.Collections.BigList
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.BigList)]
    public interface IBigList<T> where T : ILazinator
    {
        BigListTree<T> UnderlyingTree { get; set; }
    }
}