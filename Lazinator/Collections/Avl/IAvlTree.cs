using Lazinator.Core;
using Lazinator.Attributes;

namespace Lazinator.Collections.Avl
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.IAvlTree)]
    public interface IAvlTree<T> where T : ILazinator
    {
        bool Balanced { get; set; }
        string ToTreeString();
    }
}