using Lazinator.Core;
using Lazinator.Attributes;

namespace Lazinator.Collections
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorArray)]
    public interface ILazinatorArray<T> where T : ILazinator
    {
    }
}