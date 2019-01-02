using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.Factories
{
    public class SortedLazinatorListWithDuplicatesFactory<T> : ILazinatorCountableListableFactory<T> where T : ILazinator, IComparable<T>
    {
        public ILazinatorCountableListable<T> CreateCountableListable()
        {
            return new SortedLazinatorList<T>()
            {
                AllowDuplicates = true
            };
        }
    }
}