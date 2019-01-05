using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.Factories
{
    public partial class SortedLazinatorListFactory<T> : ISortedLazinatorListFactory<T>, ILazinatorCountableListableFactory<T>, ILazinatorSortableFactory<T> where T : ILazinator, IComparable<T>
    {
        public bool AllowDuplicates => false;

        public ILazinatorCountableListable<T> CreateCountableListable()
        {
            return new SortedLazinatorList<T>()
            {
                AllowDuplicates = false
            };
        }

        public ILazinatorSortable<T> CreateSortable()
        {
            return new SortedLazinatorList<T>()
            {
                AllowDuplicates = false
            };
        }
    }
}