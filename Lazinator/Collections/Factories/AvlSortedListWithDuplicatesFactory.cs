using Lazinator.Collections.Avl;
using Lazinator.Core;
using Lazinator.Wrappers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.Factories
{
    public partial class AvlSortedListWithDuplicatesFactory<T> : IAvlSortedListWithDuplicatesFactory<T>, ILazinatorCountableListableFactory<T>, ILazinatorSortableFactory<T> where T : ILazinator, IComparable<T>
    {
        public bool AllowDuplicates => true;

        public ILazinatorOrderedKeyableFactory<T, Placeholder> OrderedKeyableFactory { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public AvlSortedListWithDuplicatesFactory(ILazinatorOrderedKeyableFactory<T, Placeholder> orderedKeyableFactory)
        {
            if (orderedKeyableFactory == null)
                OrderedKeyableFactory = (ILazinatorOrderedKeyableFactory<T, Placeholder>)new AvlTreeFactory<T, Placeholder>();
            else
                this.OrderedKeyableFactory = orderedKeyableFactory;
        }

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