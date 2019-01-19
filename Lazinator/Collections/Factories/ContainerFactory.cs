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
    public partial class ContainerFactory<T> : IContainerFactory<T>, ILazinator where T : ILazinator
    {
        public ContainerLevel ThisLevel { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ContainerFactory<T> InteriorFactory { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
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

        public ContainerFactory(IEnumerable<ContainerLevel> levels)
        {
            ThisLevel = levels.First();
            var remaining = levels.Skip(1);
            if (remaining.Any())
                InteriorFactory = new ContainerFactory<T>(remaining);
        }

        public virtual IValueContainer<T> CreateValueContainer()
        {
            switch (ThisLevel.ContainerType)
            {
                case ContainerType.LazinatorList:
                    return new LazinatorList<T>()
                    {
                        AllowDuplicates = ThisLevel.AllowDuplicates
                    };
                case ContainerType.LazinatorLinkedList:
                    return new LazinatorLinkedList<T>()
                    {
                        AllowDuplicates = ThisLevel.AllowDuplicates
                    };
                case ContainerType.AvlTree:
                    return new AvlTree<T>()
                    {
                        AllowDuplicates = ThisLevel.AllowDuplicates,
                        Unbalanced = ThisLevel.Unbalanced
                    };
                case ContainerType.AvlIndexableTree:
                    return new AvlIndexableTree<T>()
                    {
                        AllowDuplicates = ThisLevel.AllowDuplicates,
                        Unbalanced = ThisLevel.Unbalanced
                    };
                case ContainerType.AvlListTree:
                    return new AvlListTree<T>(ThisLevel.AllowDuplicates, ThisLevel.Unbalanced, InteriorFactory);
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
                    return new LazinatorList<T>()
                    {
                        AllowDuplicates = ThisLevel.AllowDuplicates
                    };
                case ContainerType.LazinatorLinkedList:
                    return new LazinatorLinkedList<T>()
                    {
                        AllowDuplicates = ThisLevel.AllowDuplicates
                    };
                
                case ContainerType.AvlList:
                    return new AvlList<T>(InteriorFactory);
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
                    return new AvlDictionary<T, V>(ThisLevel.AllowDuplicates, InteriorFactory);
                default:
                    throw new NotImplementedException();
            }
        }

        public virtual IKeyValueContainer<T, V> CreateKeyValueContainer<V>() where V : ILazinator
        {
            switch (ThisLevel.ContainerType)
            {
                case ContainerType.AvlKeyValueTree:
                    return new AvlKeyValueTree<T, V>(ThisLevel.AllowDuplicates);
                case ContainerType.AvlIndexableKeyValueTree:
                    return new AvlIndexableKeyValueTree<T, V>(ThisLevel.AllowDuplicates);
                default:
                    throw new NotImplementedException();
            }
        }

        public virtual IKeyValueContainer<WUint, LazinatorKeyValue<T, V>> GetHashableKeyValueContainer<V>() where V : ILazinator
        {
            switch (ThisLevel.ContainerType)
            {
                case ContainerType.AvlKeyValueTree:
                    return new AvlKeyValueTree<WUint, LazinatorKeyValue<T, V>>(ThisLevel.AllowDuplicates);
                case ContainerType.AvlIndexableKeyValueTree:
                    return new AvlIndexableKeyValueTree<WUint, LazinatorKeyValue<T, V>>(ThisLevel.AllowDuplicates);
                default:
                    throw new NotImplementedException();
            }
        }

        public IValueContainer<T> CreateInteriorValueContainer()
        {
            return InteriorFactory.CreateValueContainer();
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
