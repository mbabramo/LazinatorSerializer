using Lazinator.Collections.Avl;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.Factories
{
    public class AvlSortedListFactory<T> : ILazinatorCountableListableFactory<T>, ILazinatorSortableFactory<T> where T : ILazinator, IComparable<T>
    {
        public bool AllowDuplicates => false;

        public ILazinatorCountableListable<T> CreateCountableListable()
        {
            return new AvlSortedList<T>(false);
        }

        public ILazinatorSortable<T> CreateSortable()
        {
            return new AvlSortedList<T>(false);
        }
    }
}
