using Lazinator.Collections.Extensions;
using Lazinator.Collections.Interfaces;
using Lazinator.Collections.Location;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections
{
    public partial class LazinatorSortedList<T> : LazinatorList<T>, ILazinatorSortedList<T>, ILazinatorSorted<T> where T : ILazinator, IComparable<T>
    {
        public LazinatorSortedList(bool allowDuplicates) : base(allowDuplicates)
        {
        }

        public override IValueContainer<T> CreateNewWithSameSettings()
        {
            return new LazinatorSortedList<T>()
            {
                AllowDuplicates = AllowDuplicates
            };
        }

        public override bool Contains(T item) => Contains(item, Comparer<T>.Default);

        public (IContainerLocation location, bool insertedNotReplaced) InsertOrReplace(T item) => InsertOrReplace(item, Comparer<T>.Default);
        public (IContainerLocation location, bool insertedNotReplaced) InsertOrReplace(T item, MultivalueLocationOptions whichOne) => InsertOrReplace(item, whichOne, Comparer<T>.Default);

        public bool TryRemove(T item) => TryRemove(item, Comparer<T>.Default);
        public bool TryRemove(T item, MultivalueLocationOptions whichOne) => TryRemove(item, whichOne, Comparer<T>.Default);

        public (long index, bool exists) FindIndex(T target) => FindIndex(target, Comparer<T>.Default);
        public (long index, bool exists) FindIndex(T target, MultivalueLocationOptions whichOne) => FindIndex(target, whichOne, Comparer<T>.Default);
        public (long index, bool exists) FindIndex(T target, MultivalueLocationOptions whichOne, IComparer<T> comparer) => this.SortedFind(AllowDuplicates, target, whichOne, comparer);
        public (long index, bool exists) FindIndex(T target, IComparer<T> comparer) => this.SortedFind(AllowDuplicates, target, comparer);

    }
}
