using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.Factories
{
    public partial class LazinatorSortedLinkedListFactory<T> : ILazinatorSortedLinkedListFactory<T>, ILazinatorListableFactory<T>, ILazinatorSortedFactory<T> where T : ILazinator, IComparable<T>
    {
        public ILazinatorListable<T> CreateListable()
        {
            return new LazinatorSortedLinkedList<T>()
            {
                AllowDuplicates = AllowDuplicates
            };
        }

        public ILazinatorSorted<T> CreateSorted()
        {
            return new LazinatorSortedLinkedList<T>()
            {
                AllowDuplicates = false
            };
        }
    }
}
