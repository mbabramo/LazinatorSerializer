using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.Factories
{
    public partial class SortedLazinatorListFactory<T> : ISortedLazinatorListFactory<T>, ILazinatorListableFactory<T>, ILazinatorSortableFactory<T> where T : ILazinator, IComparable<T>
    {
        public ILazinatorListable<T> CreateListable()
        {
            return new SortedLazinatorList<T>()
            {
                AllowDuplicates = AllowDuplicates
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