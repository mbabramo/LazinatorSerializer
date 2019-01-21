using Lazinator.Buffers;
using Lazinator.Collections.Avl;
using Lazinator.Collections.Avl.KeyValueTree;
using Lazinator.Collections.Avl.ListTree;
using Lazinator.Collections.Avl.ValueTree;
using Lazinator.Collections.Dictionary;
using Lazinator.Collections.Interfaces;
using Lazinator.Collections.Tree;
using Lazinator.Collections.Tuples;
using Lazinator.Core;
using Lazinator.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lazinator.Collections.Factories
{
    public partial class ContainerFactory : IContainerFactory
    {
        public ContainerFactory()
        {
        }

        public ContainerFactory(ContainerLevel thisLevel)
        {
            ThisLevel = thisLevel;
        }

        public ContainerFactory(ContainerLevel thisLevel, ContainerFactory innerContainerFactory)
        {
            ThisLevel = thisLevel;
            InnerFactory = innerContainerFactory;
        }

        public ContainerFactory(IEnumerable<ContainerLevel> levels)
        {
            ThisLevel = levels.First();
            var remaining = levels.Skip(1);
            var next = remaining.FirstOrDefault();
            if (next == null)
            {
                next = GetDefaultInnerFactory();
                if (next != null)
                    remaining = new List<ContainerLevel>() { next };
            }
            if (remaining.Any())
                InnerFactory = new ContainerFactory(remaining);
        }

        public virtual ContainerLevel GetDefaultInnerFactory()
        {
            switch (ThisLevel.ContainerType)
            {
                case ContainerType.LazinatorList:
                    break;
                case ContainerType.LazinatorSortedList:
                    break;
                case ContainerType.LazinatorLinkedList:
                    break;
                case ContainerType.LazinatorSortedLinkedList:
                    break;
                case ContainerType.AvlTree:
                    break;
                case ContainerType.AvlSortedTree:
                    break;
                case ContainerType.AvlIndexableTree:
                    break;
                case ContainerType.AvlSortedIndexableTree:
                    break;
                case ContainerType.AvlKeyValueTree:
                case ContainerType.AvlSortedKeyValueTree:
                    return new ContainerLevel(ContainerType.AvlTree, ThisLevel.AllowDuplicates, long.MaxValue, ThisLevel.Unbalanced);
                case ContainerType.AvlIndexableKeyValueTree:
                case ContainerType.AvlSortedIndexableKeyValueTree:
                    return new ContainerLevel(ContainerType.AvlIndexableTree, ThisLevel.AllowDuplicates, long.MaxValue, ThisLevel.Unbalanced);
                case ContainerType.AvlListTree:
                case ContainerType.AvlIndexableListTree:
                    return new ContainerLevel(ContainerType.LazinatorList, ThisLevel.AllowDuplicates, 10, ThisLevel.Unbalanced);
                case ContainerType.AvlSortedListTree:
                case ContainerType.AvlSortedIndexableListTree:
                    return new ContainerLevel(ContainerType.LazinatorSortedList, ThisLevel.AllowDuplicates, 10, ThisLevel.Unbalanced);
                case ContainerType.AvlList:
                    return new ContainerLevel(ContainerType.AvlTree, ThisLevel.AllowDuplicates, long.MaxValue, ThisLevel.Unbalanced);
                case ContainerType.AvlSortedList:
                    return new ContainerLevel(ContainerType.AvlSortedTree, ThisLevel.AllowDuplicates, long.MaxValue, ThisLevel.Unbalanced);
                case ContainerType.AvlDictionary:
                    return new ContainerLevel(ContainerType.AvlSortedKeyValueTree, ThisLevel.AllowDuplicates, long.MaxValue, ThisLevel.Unbalanced);
                case ContainerType.AvlSortedDictionary:
                    return new ContainerLevel(ContainerType.AvlSortedKeyValueTree, ThisLevel.AllowDuplicates, long.MaxValue, ThisLevel.Unbalanced);
                case ContainerType.LazinatorDictionary:
                    break;
                default:
                    throw new NotImplementedException();
            }
            return null;
        }

        public virtual IValueContainer<T> CreateValueContainer<T>() where T : ILazinator
        {
            switch (ThisLevel.ContainerType)
            {
                case ContainerType.LazinatorList:
                    return new LazinatorList<T>(ThisLevel.AllowDuplicates);
                case ContainerType.LazinatorLinkedList:
                    return new LazinatorLinkedList<T>(ThisLevel.AllowDuplicates);
                case ContainerType.AvlTree:
                    return new AvlTree<T>(ThisLevel.AllowDuplicates, ThisLevel.Unbalanced);
                case ContainerType.AvlIndexableTree:
                    return new AvlIndexableTree<T>(ThisLevel.AllowDuplicates, ThisLevel.Unbalanced);
                case ContainerType.AvlListTree:
                    return new AvlListTree<T>(ThisLevel.AllowDuplicates, ThisLevel.Unbalanced, InnerFactory);
                case ContainerType.AvlIndexableListTree:
                    throw new NotImplementedException();
                default:
                    throw new NotImplementedException();
            }
        }

        public IValueContainer<T> CreateSortedValueContainer<T>() where T : ILazinator, IComparable<T>
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
                    throw new NotImplementedException();
            }
        }


        public virtual ILazinatorListable<T> CreateLazinatorListable<T>() where T : ILazinator
        {
            switch (ThisLevel.ContainerType)
            {
                case ContainerType.LazinatorList:
                    return new LazinatorList<T>(ThisLevel.AllowDuplicates);
                case ContainerType.LazinatorLinkedList:
                    return new LazinatorLinkedList<T>(ThisLevel.AllowDuplicates);
                case ContainerType.AvlList:
                    return new AvlList<T>(InnerFactory);
                default:
                    throw new NotImplementedException();
            }
        }

        public ILazinatorListable<T> CreatePossiblySortedLazinatorListable<T>() where T : ILazinator, IComparable<T>
        {
            switch (ThisLevel.ContainerType)
            {
                case ContainerType.LazinatorSortedList:
                    return new LazinatorSortedList<T>(ThisLevel.AllowDuplicates);
                case ContainerType.LazinatorSortedLinkedList:
                    return new LazinatorSortedLinkedList<T>(ThisLevel.AllowDuplicates);
                case ContainerType.AvlSortedList:
                    return new AvlSortedList<T>(ThisLevel.AllowDuplicates, InnerFactory);
                default:
                    return CreateLazinatorListable<T>();
            }
        }

        public virtual IKeyValueContainer<K, V> CreateKeyValueContainer<K, V>() where K : ILazinator where V : ILazinator
        {
            switch (ThisLevel.ContainerType)
            {
                case ContainerType.AvlKeyValueTree:
                    return new AvlKeyValueTree<K, V>(InnerFactory, ThisLevel.AllowDuplicates, ThisLevel.Unbalanced);
                case ContainerType.AvlIndexableKeyValueTree:
                    return new AvlIndexableKeyValueTree<K, V>(InnerFactory, ThisLevel.AllowDuplicates, ThisLevel.Unbalanced);
                default:
                    throw new NotImplementedException();
            }
        }

        public IKeyValueContainer<K, V> CreatePossiblySortedKeyValueContainer<K, V>() where K : ILazinator, IComparable<K> where V : ILazinator
        {
            switch (ThisLevel.ContainerType)
            {
                case ContainerType.AvlSortedKeyValueTree:
                    return new AvlSortedKeyValueTree<K, V>(InnerFactory, ThisLevel.AllowDuplicates, ThisLevel.Unbalanced);
                case ContainerType.AvlSortedIndexableKeyValueTree:
                    return new AvlSortedIndexableKeyValueTree<K, V>(InnerFactory, ThisLevel.AllowDuplicates, ThisLevel.Unbalanced);
                default:
                    return CreateKeyValueContainer<K, V>();
            }
        }

        public ILazinatorDictionaryable<K, V> CreateLazinatorDictionaryable<K, V>() where K : ILazinator where V : ILazinator
        {
            switch (ThisLevel.ContainerType)
            {
                case ContainerType.AvlDictionary:
                    return new AvlDictionary<K, V>(ThisLevel.AllowDuplicates, InnerFactory);
                case ContainerType.LazinatorDictionary:
                    return new LazinatorDictionary<K, V>();
                default:
                    throw new NotImplementedException();
            }
        }

        public ILazinatorDictionaryable<K, V> CreatePossiblySortedLazinatorDictionaryable<K, V>() where K : ILazinator, IComparable<K> where V : ILazinator
        {
            switch (ThisLevel.ContainerType)
            {
                case ContainerType.AvlSortedDictionary:
                    return new AvlSortedDictionary<K, V>(ThisLevel.AllowDuplicates, InnerFactory);
                default:
                    return CreateLazinatorDictionaryable<K, V>();
            }
        }

        public virtual IValueContainer<LazinatorKeyValue<K, V>> CreateContainerOfKeyValues<K, V>() where K : ILazinator where V : ILazinator
        {
            switch (ThisLevel.ContainerType)
            {
                case ContainerType.LazinatorList:
                    return new LazinatorList<LazinatorKeyValue<K, V>>(ThisLevel.AllowDuplicates);
                case ContainerType.LazinatorLinkedList:
                    return new LazinatorLinkedList<LazinatorKeyValue<K, V>>(ThisLevel.AllowDuplicates);
                case ContainerType.AvlTree:
                    return new AvlTree<LazinatorKeyValue<K, V>>(ThisLevel.AllowDuplicates, ThisLevel.Unbalanced);
                case ContainerType.AvlIndexableTree:
                    return new AvlIndexableTree<LazinatorKeyValue<K, V>>(ThisLevel.AllowDuplicates, ThisLevel.Unbalanced);
                case ContainerType.AvlListTree:
                    return new AvlListTree<LazinatorKeyValue<K, V>>(ThisLevel.AllowDuplicates, ThisLevel.Unbalanced, InnerFactory);
                case ContainerType.AvlIndexableListTree:
                    throw new NotImplementedException();
                default:
                    throw new NotImplementedException();
            }
        }

        public virtual IKeyValueContainer<WUint, LazinatorKeyValue<K, V>> GetHashableKeyValueContainer<K, V>() where K : ILazinator where V : ILazinator
        {
            switch (ThisLevel.ContainerType)
            {
                case ContainerType.AvlSortedKeyValueTree:
                    return new AvlSortedKeyValueTree<WUint, LazinatorKeyValue<K, V>>(InnerFactory, true, ThisLevel.Unbalanced);
                case ContainerType.AvlSortedIndexableKeyValueTree:
                    return new AvlSortedIndexableKeyValueTree<WUint, LazinatorKeyValue<K, V>>(InnerFactory, true, ThisLevel.Unbalanced);
                default:
                    throw new NotImplementedException();
            }
        }

        public bool RequiresSplitting<T>(IValueContainer<T> container) where T : ILazinator
        {
            if (container is ICountableContainer countable)
                return countable.LongCount > ThisLevel.SplitThreshold;
            if (container is BinaryTree<T> binaryTree)
            {
                return binaryTree.GetApproximateDepth() > ThisLevel.SplitThreshold;
            }
            throw new NotImplementedException();
        }

        public bool FirstIsShorter<T>(IValueContainer<T> first, IValueContainer<T> second) where T : ILazinator
        {
            if (first is ICountableContainer countableFirst && second is ICountableContainer countableSecond)
                return countableFirst.LongCount < countableSecond.LongCount;
            if (first is BinaryTree<T> firstTree && second is BinaryTree<T> secondTree)
            {
                return firstTree.GetApproximateDepth() < secondTree.GetApproximateDepth();
            }
            throw new NotImplementedException();
        }

    }
}
