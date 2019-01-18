using Lazinator.Core;
using Lazinator.Attributes;
using System;

namespace Lazinator.Collections.Factories
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.ILazinatorSortedLinkedListFactory)]
    public interface ILazinatorSortedLinkedListFactory<T> where T : ILazinator, IComparable<T>
    {
        bool AllowDuplicates { get; set; }
    }
}