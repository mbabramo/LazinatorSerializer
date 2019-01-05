using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.Factories
{
    public partial class LazinatorLinkedListFactory<T> : ILazinatorLinkedListFactory<T>, ILazinatorCountableListableFactory<T> where T : ILazinator
    {
        public ILazinatorCountableListable<T> CreateCountableListable()
        {
            return new LazinatorLinkedList<T>();
        }
    }
}
