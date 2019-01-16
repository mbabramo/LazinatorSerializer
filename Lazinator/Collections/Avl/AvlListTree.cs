using Lazinator.Buffers;
using Lazinator.Collections.Interfaces;
using Lazinator.Collections.Tree;
using Lazinator.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.Avl
{
    public partial class AvlListTree<T> : IAvlListTree<T>, IIndexableContainer<T>, ILazinatorSplittable where T : ILazinator
    {
        public AvlIndexableTree<ILazinatorListable<T>> UnderlyingTree { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool Unbalanced { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool AllowDuplicates { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public long LongCount => throw new NotImplementedException();

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

        public IEnumerable<T> AsEnumerable(bool reverse = false, long skip = 0)
        {
            throw new NotImplementedException();
        }

        public ILazinator AssignCloneProperties(ILazinator clone, IncludeChildrenMode includeChildrenMode)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode = IncludeChildrenMode.IncludeAllChildren, CloneBufferOptions cloneBufferOptions = CloneBufferOptions.IndependentBuffers)
        {
            throw new NotImplementedException();
        }

        public bool Contains(T item, IComparer<T> comparer)
        {
            throw new NotImplementedException();
        }

        public IValueContainer<T> CreateNewWithSameSettings()
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

        public (long index, bool exists) Find(T target, IComparer<T> comparer)
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

        public T GetAt(long index)
        {
            throw new NotImplementedException();
        }

        public int GetByteLength()
        {
            throw new NotImplementedException();
        }

        public IEnumerator<T> GetEnumerator(bool reverse = false, long skip = 0)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<T> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public bool GetValue(T item, IComparer<T> comparer, out T match)
        {
            throw new NotImplementedException();
        }

        public void InsertAt(long index, T item)
        {
            throw new NotImplementedException();
        }

        public (long index, bool insertedNotReplaced) InsertGetIndex(T item, IComparer<T> comparer)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(long index)
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

        public void SetAt(long index, T value)
        {
            throw new NotImplementedException();
        }

        public ILazinatorSplittable SplitOff()
        {
            throw new NotImplementedException();
        }

        public bool TryInsert(T item, IComparer<T> comparer)
        {
            throw new NotImplementedException();
        }

        public bool TryRemove(T item, IComparer<T> comparer)
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

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }

}