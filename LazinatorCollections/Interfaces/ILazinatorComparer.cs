using Lazinator.Attributes;
using LazinatorCollections.Interfaces;
using LazinatorCollections.Location;
using Lazinator.Core;
using System.Collections.Generic;

namespace LazinatorCollections
{
    /// <summary>
    /// A nonexclusive Lazinator interface for a custom comparer
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.ILazinatorComparer)]
    public interface ILazinatorComparer<T> : IComparer<T>, ILazinator where T : ILazinator
    {
    }
}
