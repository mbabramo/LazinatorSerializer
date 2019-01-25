using Lazinator.Core;
using Lazinator.Attributes;

namespace LazinatorCollections.Tree
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.ILazinatorGeneralTree)]
    [UnofficiallyIncorporateInterface("LazinatorCollections.Tree.ILazinatorGeneralTreeUnofficial`1", "protected")]
    public interface ILazinatorGeneralTree<T> where T : ILazinator
    {
        T Item { get; set; }
    }

    [Lazinator((int)LazinatorCollectionUniqueIDs.ILazinatorGeneralTreeUnofficial)]
    public interface ILazinatorGeneralTreeUnofficial<T> where T : ILazinator
    {
        LazinatorList<LazinatorGeneralTree<T>> Children { get; set; }
    }
}