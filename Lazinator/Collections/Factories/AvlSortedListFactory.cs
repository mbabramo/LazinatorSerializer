using Lazinator.Buffers;
using Lazinator.Collections.Interfaces;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.Factories
{
    public partial class AvlSortedListFactory<T> : IAvlSortedListFactory<T>, ILazinatorSortableFactory<T> where T : ILazinator, IComparable<T>
    {
        public AvlSortedListFactory(bool allowDuplicates, ISortedIndexableContainerFactory<T> sortedIndexableContainerFactory)
        {
            AllowDuplicates = allowDuplicates;
            SortedIndexableContainerFactory = sortedIndexableContainerFactory;
        }

        public ISortedIndexableContainerFactory<T> SortedIndexableContainerFactory { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool AllowDuplicates { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
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

        public ILazinator AssignCloneProperties(ILazinator clone, IncludeChildrenMode includeChildrenMode)
        {
            throw new NotImplementedException();
        }

        public ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode = IncludeChildrenMode.IncludeAllChildren, CloneBufferOptions cloneBufferOptions = CloneBufferOptions.IndependentBuffers)
        {
            throw new NotImplementedException();
        }

        public ILazinatorListable<T> CreateListable()
        {
            throw new NotImplementedException();
        }

        public ILazinatorSortable<T> CreateSortable()
        {
            throw new NotImplementedException();
        }

        public ISortedIndexableContainer<T> CreateSortedIndexableContainer()
        {
            throw new NotImplementedException();
        }

        public void DeserializeLazinator(LazinatorMemory serialized)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<(string propertyName, ILazinator descendant)> EnumerateLazinatorDescendants(Func<ILazinator, bool> matchCriterion, bool stopExploringBelowMatch, Func<ILazinator, bool> exploreCriterion, bool exploreOnlyDeserializedChildren, bool enumerateNulls)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ILazinator> EnumerateLazinatorNodes(Func<ILazinator, bool> matchCriterion, bool stopExploringBelowMatch, Func<ILazinator, bool> exploreCriterion, bool exploreOnlyDeserializedChildren, bool enumerateNulls)
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

        public void FreeInMemoryObjects()
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

        public LazinatorMemory SerializeLazinator(IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            throw new NotImplementedException();
        }

        public void UpdateStoredBuffer()
        {
            throw new NotImplementedException();
        }

        public void UpdateStoredBuffer(ref BinaryBufferWriter writer, int startPosition, int length, IncludeChildrenMode includeChildrenMode, bool updateDeserializedChildren)
        {
            throw new NotImplementedException();
        }
    }
}
