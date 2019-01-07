using Lazinator.Core;
using Lazinator.Attributes;
using System;

namespace Lazinator.Collections.Factories
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.IAvlIndexableTreeFactory)]
    public interface IAvlIndexableTreeFactory<T> : IIndexableContainerFactory<T> where T : ILazinator
    {
    }
}