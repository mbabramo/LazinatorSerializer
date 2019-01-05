using Lazinator.Collections.Avl;
using Lazinator.Core;
using Lazinator.Wrappers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.Factories
{
    public class AvlListFactory<T> : IAvlListFactory<T>, ILazinatorCountableListableFactory<T> where T : ILazinator
    {
        public ILazinatorOrderedKeyableFactory<Placeholder, T> OrderedKeyableFactory { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public AvlListFactory(ILazinatorOrderedKeyableFactory<Placeholder, T> orderedKeyableFactory)
        {
            if (orderedKeyableFactory == null)
                OrderedKeyableFactory = (ILazinatorOrderedKeyableFactory<Placeholder, T>) new AvlTreeFactory<Placeholder, T>();
            else
                this.OrderedKeyableFactory = orderedKeyableFactory;
        }

        public ILazinatorCountableListable<T> CreateCountableListable()
        {
            return new AvlList<T>(OrderedKeyableFactory);
        }
    }
}
