using Lazinator.Attributes;
using Lazinator.Core;

namespace Lazinator.Collections.BigList
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.IBigList)]
    public interface IBigList<T> where T : ILazinator
    {
        bool IsAppendOnly { get; set; }
        BigListTree<T> UnderlyingTree { get; set; }
    }
}