using Lazinator.Attributes;
using Lazinator.Core;
using Lazinator.Wrappers;

namespace Lazinator.Collections.BigList
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.BigListLeafContainer)]
    public interface IBigListLeafContainer<T> where T : ILazinator
    {
        LazinatorList<T> Items { get; set; }
    }
}