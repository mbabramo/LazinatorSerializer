using Lazinator.Core;
using Lazinator.Attributes;

namespace Lazinator.Collections
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.ILazinatorArray)]
    public interface ILazinatorArray<T> where T : ILazinator
    {
        ILazinatorListable<T> UnderlyingList { get; set; }
    }
}