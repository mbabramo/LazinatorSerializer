using Lazinator.Attributes;
using Lazinator.Core;
using Lazinator.Wrappers;

namespace Lazinator.Collections.BigList
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.BigListLeafNode)]
    public interface IBigListLeafNode<T> where T : ILazinator
    {
        LazinatorList<T> Items { get; set; }
    }
}