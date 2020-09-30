using Lazinator.Attributes;
using LazinatorCollections.Dictionary;
using Lazinator.Core;
using Lazinator.Wrappers;

namespace LazinatorCollections.Tree
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.ILazinatorLocationAwareTree)]
    public interface ILazinatorLocationAwareTree<T> where T : ILazinator
    {
        LazinatorDictionary<T, LazinatorList<WInt32>> Locations { get; set; }
    }
}