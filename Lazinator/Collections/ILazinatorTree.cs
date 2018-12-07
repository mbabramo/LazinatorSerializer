using Lazinator.Core;
using Lazinator.Attributes;

namespace Lazinator.Collections
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorTree)]
    [UnofficiallyIncorporateInterface("Lazinator.Collections.ILazinatorTreeUnofficial`1", "protected")]
    public interface ILazinatorTree<T> where T : ILazinator
    {
        T Item { get; set; }
    }

    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorTreeUnofficial)]
    public interface ILazinatorTreeUnofficial<T> where T : ILazinator
    {
        LazinatorList<LazinatorTree<T>> Children { get; set; }
    }
}