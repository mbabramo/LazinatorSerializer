﻿using Lazinator.Collections.Avl;
using Lazinator.Collections.Avl.KeyValueTree;
using Lazinator.Collections.Avl.ValueTree;
using Lazinator.Collections.Interfaces;
using Lazinator.Collections.Tuples;
using Lazinator.Core;
using Lazinator.Wrappers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.Factories
{
    public partial class SortedContainerFactory<T> : ContainerFactory<T>, ISortedContainerFactory<T> where T : ILazinator, IComparable<T>
    {

        public SortedContainerFactory(ContainerLevel thisLevel) : base(thisLevel)
        {
        }

        public SortedContainerFactory(IEnumerable<ContainerLevel> levels) : base(levels)
        {
        }

        public override  IValueContainer<T> CreateValueContainer()
        {
            switch (ThisLevel.ContainerType)
            {
                case ContainerType.LazinatorSortedList:
                    return new LazinatorSortedList<T>()
                    {
                        AllowDuplicates = ThisLevel.AllowDuplicates
                    };
                case ContainerType.LazinatorSortedLinkedList:
                    return new LazinatorSortedLinkedList<T>()
                    {
                        AllowDuplicates = ThisLevel.AllowDuplicates
                    };
                case ContainerType.AvlSortedTree:
                    return new AvlSortedTree<T>()
                    {
                        AllowDuplicates = ThisLevel.AllowDuplicates,
                        Unbalanced = ThisLevel.Unbalanced
                    };
                case ContainerType.AvlSortedIndexableTree:
                    return new AvlSortedIndexableTree<T>()
                    {
                        AllowDuplicates = ThisLevel.AllowDuplicates,
                        Unbalanced = ThisLevel.Unbalanced
                    };
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
                    return new LazinatorSortedList<T>()
                    {
                        AllowDuplicates = ThisLevel.AllowDuplicates
                    };
                case ContainerType.LazinatorSortedLinkedList:
                    return new LazinatorSortedLinkedList<T>()
                    {
                        AllowDuplicates = ThisLevel.AllowDuplicates
                    };
                case ContainerType.AvlSortedList:
                    return new AvlSortedList<T>(ThisLevel.AllowDuplicates, InteriorFactory);
                default:
                    return base.CreateLazinatorListable();
            }
        }

        public override ILazinatorDictionaryable<T, V> CreateLazinatorDictionaryable<V>()
        {
            switch (ThisLevel.ContainerType)
            {
                case ContainerType.AvlSortedDictionary:
                    return new AvlSortedDictionary<T, V>();
                default:
                    return base.CreateLazinatorDictionaryable<V>();
            }
        }

        public override IKeyValueContainer<T, V> CreateKeyValueContainer<V>()
        {
            switch (ThisLevel.ContainerType)
            {
                case ContainerType.AvlSortedKeyValueTree:
                    return new AvlSortedKeyValueTree<T, V>(ThisLevel.AllowDuplicates);
                case ContainerType.AvlSortedIndexableKeyValueTree:
                    return new AvlSortedIndexableKeyValueTree<T, V>(ThisLevel.AllowDuplicates);
                default:
                    return base.CreateKeyValueContainer<V>();
            }
        }

        public override IKeyValueContainer<WUint, LazinatorKeyValue<T, V>> GetHashableKeyValueContainer<V>() 
        {
            switch (ThisLevel.ContainerType)
            {
                case ContainerType.AvlSortedKeyValueTree:
                    return new AvlSortedKeyValueTree<WUint, LazinatorKeyValue<T, V>>(ThisLevel.AllowDuplicates);
                case ContainerType.AvlSortedIndexableKeyValueTree:
                    return new AvlSortedIndexableKeyValueTree<WUint, LazinatorKeyValue<T, V>>(ThisLevel.AllowDuplicates);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
