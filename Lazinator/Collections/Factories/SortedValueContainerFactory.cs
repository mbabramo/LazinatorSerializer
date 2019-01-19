using Lazinator.Collections.Avl.ValueTree;
using Lazinator.Collections.Interfaces;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.Factories
{
    public partial class SortedValueContainerFactory<T> : ValueContainerFactory<T>, ISortedValueContainerFactory<T> where T : ILazinator, IComparable<T>
    {

        public SortedValueContainerFactory(IEnumerable<ValueContainerLevel> levels) : base(levels)
        {

        }

        public override  IValueContainer<T> CreateContainer()
        {
            switch (ThisLevel.ValueContainerType)
            {
                case ValueContainerType.LazinatorSortedList:
                    return new LazinatorSortedList<T>()
                    {
                        AllowDuplicates = ThisLevel.AllowDuplicates
                    };
                case ValueContainerType.LazinatorSortedLinkedList:
                    return new LazinatorSortedLinkedList<T>()
                    {
                        AllowDuplicates = ThisLevel.AllowDuplicates
                    };
                case ValueContainerType.AvlSortedTree:
                    return new AvlSortedTree<T>()
                    {
                        AllowDuplicates = ThisLevel.AllowDuplicates,
                        Unbalanced = ThisLevel.Unbalanced
                    };
                case ValueContainerType.AvlSortedIndexableTree:
                    return new AvlSortedIndexableTree<T>()
                    {
                        AllowDuplicates = ThisLevel.AllowDuplicates,
                        Unbalanced = ThisLevel.Unbalanced
                    };
                case ValueContainerType.AvlSortedListTree:
                    throw new NotImplementedException();
                case ValueContainerType.AvlSortedIndexableListTree:
                    throw new NotImplementedException();
                default:
                    return base.CreateContainer();
            }
        }
    }
}
