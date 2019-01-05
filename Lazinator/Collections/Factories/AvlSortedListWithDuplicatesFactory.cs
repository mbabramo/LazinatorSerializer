using Lazinator.Collections.Avl;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.Factories
{
    public class AvlSortedListWithDuplicatesFactory<T> : IAvlSortedListWithDuplicatesFactory<T>, ILazinatorCountableListableFactory<T>, ILazinatorSortableFactory<T> where T : ILazinator, IComparable<T>
    {
        public bool AllowDuplicates => true;

        public ILazinatorCountableListable<T> CreateCountableListable()
        {
            return new AvlSortedList<T>(true);
        }

        public ILazinatorSortable<T> CreateSortable()
        {
            return new AvlSortedList<T>(true);
        }
    }
}