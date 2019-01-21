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
        public ContainerLevel ThisLevel { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ContainerFactory InnerFactory { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

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
            if (remaining.Any())
                InnerFactory = new ContainerFactory(remaining);
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

        public virtual ILazinatorDictionaryable<K, V> CreateLazinatorDictionaryable<K, V>() where K : ILazinator where V : ILazinator
        {
            switch (ThisLevel.ContainerType)
            {
                case ContainerType.LazinatorDictionary:
                    return new LazinatorDictionary<K, V>();
                case ContainerType.AvlDictionary:
                    return new AvlDictionary<K, V>(ThisLevel.AllowDuplicates, InnerFactory);
                default:
                    throw new NotImplementedException();
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
                case ContainerType.AvlKeyValueTree:
                    return new AvlSortedKeyValueTree<WUint, LazinatorKeyValue<K, V>>(InnerFactory, true, ThisLevel.Unbalanced);
                case ContainerType.AvlIndexableKeyValueTree:
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
