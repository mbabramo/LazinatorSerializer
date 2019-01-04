using System;
using Lazinator.Core;

namespace Lazinator.Collections.Factories
{
    public class SortedLazinatorLinkedListWithDuplicatesFactory<T> : ILazinatorCountableListableFactory<T> where T : ILazinator, IComparable<T>
    {
        public ILazinatorCountableListable<T> CreateCountableListable()
        {
            return new SortedLazinatorLinkedList<T>()
            {
                AllowDuplicates = true
            };
        }
    }
}