using Lazinator.Attributes;
using Lazinator.Collections.Interfaces;
using Lazinator.Collections.Location;
using Lazinator.Core;
using System.Collections.Generic;

namespace Lazinator.Collections
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
