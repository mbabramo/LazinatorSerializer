using Lazinator.Collections.Extensions;
using Lazinator.Collections.Interfaces;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections
{
    public partial class LazinatorSortedLinkedList<T> : LazinatorLinkedList<T>, ILazinatorSortedLinkedList<T>, ILazinatorSorted<T> where T : IComparable<T>, ILazinator
    {
        public LazinatorSortedLinkedList(bool allowDuplicates) : base(allowDuplicates)
        {
        }

        public override IValueContainer<T> CreateNewWithSameSettings()
        {
            return new LazinatorSortedLinkedList<T>(AllowDuplicates);
        }

        public (long index, bool insertedNotReplaced) InsertGetIndex(T item) => InsertGetIndex(item, Comparer<T>.Default);
        public (long index, bool insertedNotReplaced) InsertGetIndex(T item, MultivalueLocationOptions whichOne) => InsertGetIndex(item, whichOne, Comparer<T>.Default);
        public (long index, bool insertedNotReplaced) InsertGetIndex(T item, IComparer<T> comparer) => InsertGetIndex(item, AllowDuplicates ? MultivalueLocationOptions.InsertAfterLast : MultivalueLocationOptions.Any, comparer);
        public (long index, bool insertedNotReplaced) InsertGetIndex(T item, MultivalueLocationOptions whichOne, IComparer<T> comparer) => this.SortedInsertGetIndex(AllowDuplicates, item, whichOne, comparer);

        public bool TryRemove(T item) => TryRemove(item, Comparer<T>.Default);
        public bool TryRemove(T item, MultivalueLocationOptions whichOne) => TryRemove(item, whichOne, Comparer<T>.Default);

        public (long index, bool exists) Find(T target) => Find(target, Comparer<T>.Default);
        public (long index, bool exists) Find(T target, MultivalueLocationOptions whichOne) => Find(target, whichOne, Comparer<T>.Default);
        public (long index, bool exists) Find(T target, MultivalueLocationOptions whichOne, IComparer<T> comparer) => this.SortedFind(AllowDuplicates, target, whichOne, comparer);
        public (long index, bool exists) Find(T target, IComparer<T> comparer) => this.SortedFind(AllowDuplicates, target, comparer);
    }
}
