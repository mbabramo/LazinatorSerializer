using Lazinator.Collections.Interfaces;
using Lazinator.Collections.Location;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.Avl.ValueTree
{
    public partial class AvlSortedTree<T> : AvlTree<T>, IAvlSortedTree<T>, ISortedMultivalueContainer<T> where T : ILazinator , IComparable<T>
    {
        public AvlSortedTree(bool allowDuplicates, bool unbalanced) : base(allowDuplicates, unbalanced)
        {
        }

        public override IValueContainer<T> CreateNewWithSameSettings()
        {
            return new AvlSortedTree<T>() { AllowDuplicates = AllowDuplicates };
        }

        public bool Contains(T item) => Contains(item, Comparer<T>.Default);

        public (IContainerLocation location, bool insertedNotReplaced) InsertOrReplace(T item) => InsertOrReplace(item, AllowDuplicates ? MultivalueLocationOptions.InsertAfterLast : MultivalueLocationOptions.Any);
        public (IContainerLocation location, bool insertedNotReplaced) InsertOrReplace(T item, MultivalueLocationOptions whichOne) => InsertOrReplace(item, whichOne, Comparer<T>.Default);

        public bool TryRemove(T item) => TryRemove(item, MultivalueLocationOptions.Any);
        public bool TryRemove(T item, MultivalueLocationOptions whichOne) => TryRemove(item, Comparer<T>.Default);

        public bool TryRemoveAll(T item) => TryRemoveAll(item, Comparer<T>.Default);

        public long Count(T item) => Count(item, Comparer<T>.Default);
    }
}
