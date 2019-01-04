using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.Factories
{
    public class LazinatorLinkedListFactory<T> : ILazinatorCountableListableFactory<T> where T : ILazinator
    {
        public ILazinatorCountableListable<T> CreateCountableListable()
        {
            return new LazinatorLinkedList<T>();
        }
    }
}
