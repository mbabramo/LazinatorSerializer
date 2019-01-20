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
    public partial class ContainerFactory<T> : IContainerFactory<T>, IContainerFactory where T : ILazinator
    {
        public ContainerLevel ThisLevel { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IContainerFactory InnerFactory { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public ContainerFactory<T> InnerFactorySameType => (ContainerFactory<T>)InnerFactory;
        public ContainerFactory<LazinatorKeyValue<T, V>> InnerKeyValueFactory<V>() where V : ILazinator => (ContainerFactory<LazinatorKeyValue<T, V>>)InnerFactory;
        public ContainerFactory<WUint> InnerHashableKeyValueFactory => (ContainerFactory<WUint>)InnerFactory;

        public bool HasChanged { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool DescendantHasChanged { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool IsDirty { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool DescendantIsDirty { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public LazinatorMemory LazinatorMemoryStorage { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IncludeChildrenMode OriginalIncludeChildrenMode => throw new NotImplementedException();

        public bool IsStruct => throw new NotImplementedException();

        public bool NonBinaryHash32 => throw new NotImplementedException();

        public LazinatorParentsCollection LazinatorParents { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int LazinatorObjectVersion { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public ContainerFactory()
        {
        }

        public ContainerFactory(ContainerLevel thisLevel)
        {
            ThisLevel = thisLevel;
        }

        public ContainerFactory(ContainerLevel thisLevel, IContainerFactory innerContainerFactory)
        {
            ThisLevel = thisLevel;
            InnerFactory = innerContainerFactory;
        }

        public ContainerFactory(IEnumerable<ContainerLevel> levels)
        {
            ThisLevel = levels.First();
            var remaining = levels.Skip(1);
            if (remaining.Any())
                InnerFactory = new ContainerFactory<T>(remaining);
        }

        public virtual IValueContainer<T> CreateValueContainer()
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
                    return new AvlListTree<T>(ThisLevel.AllowDuplicates, ThisLevel.Unbalanced, InnerFactorySameType);
                case ContainerType.AvlIndexableListTree:
                    throw new NotImplementedException();
                default:
                    throw new NotImplementedException();
            }
        }

        public virtual ILazinatorListable<T> CreateLazinatorListable()
        {
            switch (ThisLevel.ContainerType)
            {
                case ContainerType.LazinatorList:
                    return new LazinatorList<T>(ThisLevel.AllowDuplicates);
                case ContainerType.LazinatorLinkedList:
                    return new LazinatorLinkedList<T>(ThisLevel.AllowDuplicates);
                case ContainerType.AvlList:
                    return new AvlList<T>(InnerFactorySameType);
                default:
                    throw new NotImplementedException();
            }
        }

        public virtual ILazinatorDictionaryable<T, V> CreateLazinatorDictionaryable<V>() where V : ILazinator
        {
            switch (ThisLevel.ContainerType)
            {
                case ContainerType.LazinatorDictionary:
                    return new LazinatorDictionary<T, V>();
                case ContainerType.AvlDictionary:
                    return new AvlDictionary<T, V>(ThisLevel.AllowDuplicates, InnerFactorySameType);
                default:
                    throw new NotImplementedException();
            }
        }

        public virtual IKeyValueContainer<T, V> CreateKeyValueContainer<V>() where V : ILazinator
        {
            switch (ThisLevel.ContainerType)
            {
                case ContainerType.AvlKeyValueTree:
                    return new AvlKeyValueTree<T, V>(InnerFactorySameType, ThisLevel.AllowDuplicates, ThisLevel.Unbalanced);
                case ContainerType.AvlIndexableKeyValueTree:
                    return new AvlIndexableKeyValueTree<T, V>(InnerFactorySameType, ThisLevel.AllowDuplicates, ThisLevel.Unbalanced);
                default:
                    throw new NotImplementedException();
            }
        }

        public virtual IValueContainer<LazinatorKeyValue<T, V>> CreateContainerOfKeyValues<V>() where V : ILazinator
        {
            switch (ThisLevel.ContainerType)
            {
                case ContainerType.LazinatorList:
                    return new LazinatorList<LazinatorKeyValue<T, V>>(ThisLevel.AllowDuplicates);
                case ContainerType.LazinatorLinkedList:
                    return new LazinatorLinkedList<LazinatorKeyValue<T, V>>(ThisLevel.AllowDuplicates);
                case ContainerType.AvlTree:
                    return new AvlTree<LazinatorKeyValue<T, V>>(ThisLevel.AllowDuplicates, ThisLevel.Unbalanced);
                case ContainerType.AvlIndexableTree:
                    return new AvlIndexableTree<LazinatorKeyValue<T, V>>(ThisLevel.AllowDuplicates, ThisLevel.Unbalanced);
                case ContainerType.AvlListTree:
                    return new AvlListTree<LazinatorKeyValue<T, V>>(ThisLevel.AllowDuplicates, ThisLevel.Unbalanced, InnerKeyValueFactory<V>());
                case ContainerType.AvlIndexableListTree:
                    throw new NotImplementedException();
                default:
                    throw new NotImplementedException();
            }
        }

        public virtual IKeyValueContainer<WUint, LazinatorKeyValue<T, V>> GetHashableKeyValueContainer<V>() where V : ILazinator
        {
            switch (ThisLevel.ContainerType)
            {
                case ContainerType.AvlKeyValueTree:
                    return new AvlSortedKeyValueTree<WUint, LazinatorKeyValue<T, V>>(InnerHashableKeyValueFactory, true, ThisLevel.Unbalanced);
                case ContainerType.AvlIndexableKeyValueTree:
                    return new AvlSortedIndexableKeyValueTree<WUint, LazinatorKeyValue<T, V>>(InnerHashableKeyValueFactory, true, ThisLevel.Unbalanced);
                default:
                    throw new NotImplementedException();
            }
        }

        public bool RequiresSplitting(IValueContainer<T> container)
        {
            if (container is ICountableContainer countable)
                return countable.LongCount > ThisLevel.SplitThreshold;
            if (container is BinaryTree<T> binaryTree)
            {
                return binaryTree.GetApproximateDepth() > ThisLevel.SplitThreshold;
            }
            throw new NotImplementedException();
        }

        public bool FirstIsShorter(IValueContainer<T> first, IValueContainer<T> second)
        {
            if (first is ICountableContainer countableFirst && second is ICountableContainer countableSecond)
                return countableFirst.LongCount < countableSecond.LongCount;
            if (first is BinaryTree<T> firstTree && second is BinaryTree<T> secondTree)
            {
                return firstTree.GetApproximateDepth() < secondTree.GetApproximateDepth();
            }
            throw new NotImplementedException();
        }

        public LazinatorMemory SerializeLazinator(IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            throw new NotImplementedException();
        }

        public void DeserializeLazinator(LazinatorMemory serialized)
        {
            throw new NotImplementedException();
        }

        public ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode = IncludeChildrenMode.IncludeAllChildren, CloneBufferOptions cloneBufferOptions = CloneBufferOptions.IndependentBuffers)
        {
            throw new NotImplementedException();
        }

        public void UpdateStoredBuffer()
        {
            throw new NotImplementedException();
        }

        public void FreeInMemoryObjects()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ILazinator> EnumerateLazinatorNodes(Func<ILazinator, bool> matchCriterion, bool stopExploringBelowMatch, Func<ILazinator, bool> exploreCriterion, bool exploreOnlyDeserializedChildren, bool enumerateNulls)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<(string propertyName, ILazinator descendant)> EnumerateLazinatorDescendants(Func<ILazinator, bool> matchCriterion, bool stopExploringBelowMatch, Func<ILazinator, bool> exploreCriterion, bool exploreOnlyDeserializedChildren, bool enumerateNulls)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<(string propertyName, object descendant)> EnumerateNonLazinatorProperties()
        {
            throw new NotImplementedException();
        }

        public ILazinator ForEachLazinator(Func<ILazinator, ILazinator> changeFunc, bool exploreOnlyDeserializedChildren, bool changeThisLevel)
        {
            throw new NotImplementedException();
        }

        public int GetByteLength()
        {
            throw new NotImplementedException();
        }

        public void SerializeExistingBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            throw new NotImplementedException();
        }

        public void UpdateStoredBuffer(ref BinaryBufferWriter writer, int startPosition, int length, IncludeChildrenMode includeChildrenMode, bool updateDeserializedChildren)
        {
            throw new NotImplementedException();
        }

        public ILazinator AssignCloneProperties(ILazinator clone, IncludeChildrenMode includeChildrenMode)
        {
            throw new NotImplementedException();
        }
    }
}
