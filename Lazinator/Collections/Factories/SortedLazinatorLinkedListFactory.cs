using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.Factories
{
    public partial class SortedLazinatorLinkedListFactory<T> : ISortedLazinatorLinkedListFactory<T>, ILazinatorListableFactory<T>, ILazinatorSortableFactory<T> where T : ILazinator, IComparable<T>
    {
        public ILazinatorListable<T> CreateListable()
        {
            return new SortedLazinatorLinkedList<T>()
            {
                AllowDuplicates = AllowDuplicates
            };
        }

        public ILazinatorSortable<T> CreateSortable()
        {
            return new SortedLazinatorLinkedList<T>()
            {
                AllowDuplicates = false
            };
        }
    }
}
