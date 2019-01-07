using Lazinator.Buffers;
using Lazinator.Collections.Avl;
using Lazinator.Collections.Interfaces;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.Factories
{
    public partial class AvlSortedListFactory<T> : IAvlSortedListFactory<T>, ILazinatorSortableFactory<T> where T : ILazinator, IComparable<T>
    {
        public AvlSortedListFactory(bool allowDuplicates, ISortedIndexableContainerFactory<T> sortedIndexableContainerFactory)
        {
            AllowDuplicates = allowDuplicates;
            SortedIndexableContainerFactory = sortedIndexableContainerFactory;
        }

        public ILazinatorListable<T> CreateListable()
        {
            return new AvlSortedList<T>(AllowDuplicates, SortedIndexableContainerFactory);
        }

        public ILazinatorSortable<T> CreateSortable()
        {
            return new AvlSortedList<T>(AllowDuplicates, SortedIndexableContainerFactory);
        }
    }
}
