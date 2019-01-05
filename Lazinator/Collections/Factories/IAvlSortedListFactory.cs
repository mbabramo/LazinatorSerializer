using Lazinator.Core;
using Lazinator.Attributes;
using System;
using Lazinator.Wrappers;

namespace Lazinator.Collections.Factories
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.IAvlSortedListFactory)]
    public interface IAvlSortedListFactory<T> where T : ILazinator, IComparable<T>
    {
        ILazinatorOrderedKeyableFactory<T, Placeholder> OrderedKeyableFactory { get; set; }
        bool AllowDuplicates { get; set; }
    }
}