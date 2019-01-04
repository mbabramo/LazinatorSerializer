using System;
using Lazinator.Core;

namespace Lazinator.Collections.Factories
{
    public class SortedLazinatorLinkedListWithDuplicatesFactory<T> : ILazinatorCountableListableFactory<T>, ILazinatorSortableFactory<T> where T : ILazinator, IComparable<T>
    {
        public bool AllowDuplicates => true;

        public ILazinatorCountableListable<T> CreateCountableListable()
        {
            return new SortedLazinatorLinkedList<T>()
            {
                AllowDuplicates = true
            };
        }

        public ILazinatorSortable<T> CreateSortable()
        {
            return new SortedLazinatorLinkedList<T>()
            {
                AllowDuplicates = true
            };
        }
    }
}