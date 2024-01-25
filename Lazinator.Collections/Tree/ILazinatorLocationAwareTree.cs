using Lazinator.Attributes;
using Lazinator.Collections.Dictionary;
using Lazinator.Core;
using Lazinator.Wrappers;

namespace Lazinator.Collections.Tree
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.ILazinatorLocationAwareTree)]
    public interface ILazinatorLocationAwareTree<T> : ILazinatorGeneralTree<T> where T : ILazinator
    {
        /// <summary>
        /// The locations of all items within the tree. 
        /// The value list is a list of the branch selected at each level of the tree.
        /// </summary>
        LazinatorDictionary<T, LazinatorList<WInt32>> Locations { get; set; }
    }
}