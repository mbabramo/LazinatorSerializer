using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.Factories
{
    public partial class LazinatorListFactory<T> : ILazinatorListFactory<T>, ILazinatorListableFactory<T> where T : ILazinator
    {
        public ILazinatorListable<T> CreateListable()
        {
            return new LazinatorList<T>();
        }
    }
}
