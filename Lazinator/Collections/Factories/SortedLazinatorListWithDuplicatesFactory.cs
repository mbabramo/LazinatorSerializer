using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.Factories
{
    public partial class SortedLazinatorListWithDuplicatesFactory<T> : ISortedLazinatorListWithDuplicatesFactory<T>, ILazinatorCountableListableFactory<T>, ILazinatorSortableFactory<T> where T : ILazinator, IComparable<T>
    {
        public bool AllowDuplicates => true;

        public ILazinatorCountableListable<T> CreateCountableListable()
        {
            return new SortedLazinatorList<T>()
            {
                AllowDuplicates = true
            };
        }

        public ILazinatorSortable<T> CreateSortable()
        {
            return new SortedLazinatorList<T>()
            {
                AllowDuplicates = true
            };
        }
    }
}