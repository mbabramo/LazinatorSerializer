using Lazinator.Core;
using Lazinator.Attributes;

namespace Lazinator.Collections.BigList
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.IBigListTree)]
    public interface IBigListTree<T> where T : ILazinator
    {
    }
}