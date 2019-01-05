using Lazinator.Collections.Avl;
using Lazinator.Core;
using Lazinator.Wrappers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.Factories
{
    public partial class AvlSortedListFactory<T> : IAvlSortedListFactory<T>, ILazinatorListableFactory<T>, ILazinatorSortableFactory<T> where T : ILazinator, IComparable<T>
    {
        public AvlSortedListFactory(bool allowDuplicates, ILazinatorOrderedKeyableFactory<T, Placeholder> orderedKeyableFactory)
        {
            AllowDuplicates = allowDuplicates;
            if (orderedKeyableFactory == null)
                OrderedKeyableFactory = (ILazinatorOrderedKeyableFactory<T, Placeholder>)new AvlTreeFactory<T, Placeholder>();
            else
                this.OrderedKeyableFactory = orderedKeyableFactory;
        }

        public ILazinatorListable<T> CreateListable()
        {
            return new AvlSortedList<T>(AllowDuplicates, OrderedKeyableFactory);
        }

        public ILazinatorSortable<T> CreateSortable()
        {
            return new AvlSortedList<T>(AllowDuplicates, OrderedKeyableFactory);
        }
    }
}
