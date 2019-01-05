using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.Factories
{
    public partial class SortedLazinatorLinkedListFactory<T> : ISortedLazinatorLinkedListFactory<T>, ILazinatorCountableListableFactory<T>, ILazinatorSortableFactory<T> where T : ILazinator, IComparable<T>
    {
        public bool AllowDuplicates { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public ILazinatorCountableListable<T> CreateCountableListable()
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
