using Lazinator.Collections.Avl;
using Lazinator.Core;
using Lazinator.Wrappers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.Factories
{
    public class AvlSortedListFactory<T> : IAvlSortedListFactory<T>, ILazinatorCountableListableFactory<T>, ILazinatorSortableFactory<T> where T : ILazinator, IComparable<T>
    {
        public ILazinatorOrderedKeyableFactory<T, Placeholder> OrderedKeyableFactory { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public AvlSortedListFactory(ILazinatorOrderedKeyableFactory<T, Placeholder> orderedKeyableFactory)
        {
            if (orderedKeyableFactory == null)
                OrderedKeyableFactory = (ILazinatorOrderedKeyableFactory<T, Placeholder>)new AvlTreeFactory<T, Placeholder>();
            else
                this.OrderedKeyableFactory = orderedKeyableFactory;
        }

        public bool AllowDuplicates => false;

        public ILazinatorCountableListable<T> CreateCountableListable()
        {
            return new AvlSortedList<T>(OrderedKeyableFactory);
        }

        public ILazinatorSortable<T> CreateSortable()
        {
            return new AvlSortedList<T>(OrderedKeyableFactory);
        }
    }
}
