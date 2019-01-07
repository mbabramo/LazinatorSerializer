using Lazinator.Core;
using Lazinator.Attributes;
using System;

namespace Lazinator.Collections.Avl
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.IAvlIndexableTree)]
    public interface IAvlIndexableTree<T> where T : ILazinator
    {
    }
}