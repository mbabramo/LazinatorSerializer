using Lazinator.Core;
using Lazinator.Attributes;
using System;

namespace Lazinator.Collections.Factories
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.IAvlSortedListFactory)]
    public interface IAvlSortedListFactory<T> where T : ILazinator, IComparable<T>
    {
        bool AllowDuplicates { get; set; }
    }
}