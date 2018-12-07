using Lazinator.Core;
using Lazinator.Attributes;

namespace Lazinator.Collections
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorGeneralTree)]
    [UnofficiallyIncorporateInterface("Lazinator.Collections.ILazinatorGeneralTreeUnofficial`1", "protected")]
    public interface ILazinatorGeneralTree<T> where T : ILazinator
    {
        T Item { get; set; }
    }

    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorGeneralTreeUnofficial)]
    public interface ILazinatorGeneralTreeUnofficial<T> where T : ILazinator
    {
        LazinatorList<LazinatorGeneralTree<T>> Children { get; set; }
    }
}