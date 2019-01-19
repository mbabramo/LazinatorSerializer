using Lazinator.Core;
using Lazinator.Attributes;
using Lazinator.Collections.Interfaces;
using System.Collections.Generic;

namespace Lazinator.Collections.Factories
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.IAvlTree)]
    public interface IAvlListTreeFactory<T> where T : ILazinator
    {
        List<(AvlListTreeFactory<T>.TreeLevelType TreeLevelType, long InteriorMaxCapacity)> LevelsInfo { get; set; }

        bool RequiresSplitting(IMultivalueContainer<T> container);
        bool FirstIsShorter(IMultivalueContainer<T> first, IMultivalueContainer<T> second);
        IMultivalueContainer<T> CreateInteriorCollection(bool allowDuplicates);
    }
}