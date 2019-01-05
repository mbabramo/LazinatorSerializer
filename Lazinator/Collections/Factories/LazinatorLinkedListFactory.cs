using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.Factories
{
    public partial class LazinatorLinkedListFactory<T> : ILazinatorLinkedListFactory<T>, ILazinatorListableFactory<T> where T : ILazinator
    {
        public ILazinatorListable<T> CreateListable()
        {
            return new LazinatorLinkedList<T>();
        }
    }
}
