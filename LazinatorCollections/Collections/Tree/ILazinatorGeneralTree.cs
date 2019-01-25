using Lazinator.Core;
using Lazinator.Attributes;

namespace Lazinator.Collections.Tree
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.ILazinatorGeneralTree)]
    [UnofficiallyIncorporateInterface("Lazinator.Collections.Tree.ILazinatorGeneralTreeUnofficial`1", "protected")]
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