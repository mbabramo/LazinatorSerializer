using Lazinator.Collections.Avl;
using Lazinator.Collections.Avl.KeyValueTree;
using Lazinator.Collections.Avl.ValueTree;
using Lazinator.Collections.Interfaces;
using Lazinator.Collections.Tuples;
using Lazinator.Core;
using Lazinator.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lazinator.Collections.Factories
{
    public partial class SortedContainerFactory<T> : ContainerFactory<T>, ISortedContainerFactory<T> where T : ILazinator, IComparable<T>
    {
        public SortedContainerFactory(ContainerLevel thisLevel) : base(thisLevel)
        {
        }

        public SortedContainerFactory(IEnumerable<ContainerLevel> levels)
        {
            ThisLevel = levels.First();
            var remaining = levels.Skip(1);
            if (remaining.Any())
            {
                InnerFactory = SortedInnerFactory = new SortedContainerFactory<T>(remaining);
            }
        }

        public override  IValueContainer<T> CreateValueContainer()
        {
            switch (ThisLevel.ContainerType)
            {
                case ContainerType.LazinatorSortedList:
                    return new LazinatorSortedList<T>(ThisLevel.AllowDuplicates);
                case ContainerType.LazinatorSortedLinkedList:
                    return new LazinatorSortedLinkedList<T>(ThisLevel.AllowDuplicates);
                case ContainerType.AvlSortedTree:
                    return new AvlSortedTree<T>(ThisLevel.AllowDuplicates, ThisLevel.Unbalanced);
                case ContainerType.AvlSortedIndexableTree:
                    return new AvlSortedIndexableTree<T>(ThisLevel.AllowDuplicates, ThisLevel.Unbalanced);
                case ContainerType.AvlSortedListTree:
                    throw new NotImplementedException();
                case ContainerType.AvlSortedIndexableListTree:
                    throw new NotImplementedException();
                default:
                    return base.CreateValueContainer();
            }
        }

        public override ILazinatorListable<T> CreateLazinatorListable()
        {
            switch (ThisLevel.ContainerType)
            {
                case ContainerType.LazinatorSortedList:
                    return new LazinatorSortedList<T>(ThisLevel.AllowDuplicates);
                case ContainerType.LazinatorSortedLinkedList:
                    return new LazinatorSortedLinkedList<T>(ThisLevel.AllowDuplicates);
                case ContainerType.AvlSortedList:
                    return new AvlSortedList<T>(ThisLevel.AllowDuplicates, SortedInnerFactory);
                default:
                    return base.CreateLazinatorListable();
            }
        }

        public override ILazinatorDictionaryable<T, V> CreateLazinatorDictionaryable<V>()
        {
            switch (ThisLevel.ContainerType)
            {
                case ContainerType.AvlSortedDictionary:
                    return new AvlSortedDictionary<T, V>(ThisLevel.AllowDuplicates, SortedInnerFactory);
                default:
                    return base.CreateLazinatorDictionaryable<V>();
            }
        }

        public override IKeyValueContainer<T, V> CreateKeyValueContainer<V>()
        {
            switch (ThisLevel.ContainerType)
            {
                case ContainerType.AvlSortedKeyValueTree:
                    return new AvlSortedKeyValueTree<T, V>(InnerFactorySameType, ThisLevel.AllowDuplicates, ThisLevel.Unbalanced);
                case ContainerType.AvlSortedIndexableKeyValueTree:
                    return new AvlSortedIndexableKeyValueTree<T, V>(InnerFactorySameType, ThisLevel.AllowDuplicates, ThisLevel.Unbalanced);
                default:
                    return base.CreateKeyValueContainer<V>();
            }
        }

        public override IKeyValueContainer<WUint, LazinatorKeyValue<T, V>> GetHashableKeyValueContainer<V>() 
        {
            switch (ThisLevel.ContainerType)
            {
                case ContainerType.AvlSortedKeyValueTree:
                    return new AvlSortedKeyValueTree<WUint, LazinatorKeyValue<T, V>>(InnerHashableKeyValueFactory, ThisLevel.AllowDuplicates, ThisLevel.Unbalanced);
                case ContainerType.AvlSortedIndexableKeyValueTree:
                    return new AvlSortedIndexableKeyValueTree<WUint, LazinatorKeyValue<T, V>>(InnerHashableKeyValueFactory, ThisLevel.AllowDuplicates, ThisLevel.Unbalanced);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
