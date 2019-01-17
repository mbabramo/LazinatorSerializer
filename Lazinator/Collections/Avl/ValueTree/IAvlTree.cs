using Lazinator.Core;
using Lazinator.Attributes;

namespace Lazinator.Collections.Avl.ValueTree
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.IAvlTree)]
    public interface IAvlTree<T> where T : ILazinator
    {
        [DerivationKeyword("override")]
        bool Unbalanced { get; set; }
        string ToTreeString();
    }
}