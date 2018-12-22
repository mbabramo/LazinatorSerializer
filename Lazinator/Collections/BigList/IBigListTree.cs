using Lazinator.Core;
using Lazinator.Attributes;

namespace Lazinator.Collections.BigList
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.BigListTree)]
    public interface IBigListTree<T> where T : ILazinator
    {
    }
}