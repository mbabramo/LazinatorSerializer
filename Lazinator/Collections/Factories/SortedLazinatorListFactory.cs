using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.Factories
{
    public partial class SortedLazinatorListFactory<T> : ISortedLazinatorListFactory<T>, ILazinatorListableFactory<T>, ILazinatorSortedFactory<T> where T : ILazinator, IComparable<T>
    {
        public ILazinatorListable<T> CreateListable()
        {
            return new SortedLazinatorList<T>()
            {
                AllowDuplicates = AllowDuplicates
            };
        }

        public ILazinatorSorted<T> CreateSorted()
        {
            return new SortedLazinatorList<T>()
            {
                AllowDuplicates = false
            };
        }
    }
}