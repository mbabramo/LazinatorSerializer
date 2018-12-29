using Lazinator.Attributes;
using Lazinator.Core;

namespace Lazinator.Collections.BigList
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.BigList)]
    public interface IBigList<T> where T : ILazinator
    {
        bool IsAppendOnly { get; set; }
        BigListTree<T> UnderlyingTree { get; set; }
    }
}