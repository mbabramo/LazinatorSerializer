using Lazinator.Attributes;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections
{
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.IILazinatorSortable)]
    public interface ILazinatorSortable<T> : ILazinatorCountableListable<T> where T : ILazinator, IComparable<T>
    {
        bool AllowDuplicates { get; set; }
        (int location, bool rejectedAsDuplicate) InsertSorted(T item);
        (int priorLocation, bool existed) RemoveSorted(T item);
        (int location, bool exists) FindSorted(T target);
    }
}
