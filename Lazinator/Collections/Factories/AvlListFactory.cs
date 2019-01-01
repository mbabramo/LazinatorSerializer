using Lazinator.Collections.Avl;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.Factories
{
    public class AvlListFactory<T> : ILazinatorCountableListableFactory<T> where T : ILazinator
    {
        public ILazinatorCountableListable<T> CreateCountableListable()
        {
            return new AvlList<T>();
        }
    }
}
