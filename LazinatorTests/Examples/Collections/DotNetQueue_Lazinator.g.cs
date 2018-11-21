//00433bd4-72dd-3b83-c78a-1061c26f0b09
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.321
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LazinatorTests.Examples.Collections
{
    using Lazinator.Attributes;
    using Lazinator.Buffers;
    using Lazinator.Core;
    using Lazinator.Exceptions;
    using Lazinator.Support;
    using LazinatorTests.Examples;
    using System;
    using System.Buffers;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;
    using static Lazinator.Core.LazinatorUtilities;
    
    [Autogenerated]
    public partial class DotNetQueue_Lazinator : ILazinator
    {
        public bool IsStruct => false;
        
        /* Property definitions */
        
        protected int _MyQueueSerialized_ByteIndex;
        private int _DotNetQueue_Lazinator_EndByteIndex;
        protected virtual int _MyQueueSerialized_ByteLength => _DotNetQueue_Lazinator_EndByteIndex - _MyQueueSerialized_ByteIndex;
        
        
        protected Queue<ExampleChild> _MyQueueSerialized;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public Queue<ExampleChild> MyQueueSerialized
        {
            get
            {
                if (!_MyQueueSerialized_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _MyQueueSerialized = default(Queue<ExampleChild>);
                    }
                    else
                    {
                        LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _MyQueueSerialized_ByteIndex, _MyQueueSerialized_ByteLength, false, false, null);
                        _MyQueueSerialized = ConvertFromBytes_Queue_GExampleChild_g(childData);
                    }
                    _MyQueueSerialized_Accessed = true;
                }
                IsDirty = true; 
                return _MyQueueSerialized;
            }
            set
            {
                IsDirty = true;
                DescendantIsDirty = true;
                _MyQueueSerialized = value;
                _MyQueueSerialized_Accessed = true;
            }
        }
        protected bool _MyQueueSerialized_Accessed;
        
        /* Serialization, deserialization, and object relationships */
        
        public DotNetQueue_Lazinator() : base()
        {
        }
        
        public virtual LazinatorParentsCollection LazinatorParents { get; set; }
        
        public virtual IncludeChildrenMode OriginalIncludeChildrenMode { get; set; }
        
        public virtual int Deserialize()
        {
            FreeInMemoryObjects();
            int bytesSoFar = 0;
            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
            if (span.Length == 0)
            {
                return 0;
            }
            
            LazinatorGenericID = GetGenericIDIfApplicable(ContainsOpenGenericParameters, LazinatorUniqueID, span, ref bytesSoFar);
            
            int lazinatorLibraryVersion = span.ToDecompressedInt(ref bytesSoFar);
            
            int serializedVersionNumber = span.ToDecompressedInt(ref bytesSoFar);
            
            OriginalIncludeChildrenMode = (IncludeChildrenMode)span.ToByte(ref bytesSoFar);
            
            ConvertFromBytesAfterHeader(OriginalIncludeChildrenMode, serializedVersionNumber, ref bytesSoFar);
            return bytesSoFar;
        }
        
        public virtual LazinatorMemory SerializeLazinator(IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer) => EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, verifyCleanness, IsDirty, DescendantIsDirty, false, LazinatorMemoryStorage, updateStoredBuffer, this);
        
        public virtual ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode = IncludeChildrenMode.IncludeAllChildren, CloneBufferOptions cloneBufferOptions = CloneBufferOptions.IndependentBuffers)
        {
            var clone = new DotNetQueue_Lazinator()
            {
                OriginalIncludeChildrenMode = includeChildrenMode
            };
            clone = CompleteClone(this, clone, includeChildrenMode, cloneBufferOptions);
            return clone;
        }
        
        public virtual ILazinator AssignCloneProperties(ILazinator clone, IncludeChildrenMode includeChildrenMode)
        {
            clone.FreeInMemoryObjects();
            DotNetQueue_Lazinator typedClone = (DotNetQueue_Lazinator) clone;
            typedClone.MyQueueSerialized = CloneOrChange_Queue_GExampleChild_g(MyQueueSerialized, l => l?.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer), false);
            
            return typedClone;
        }
        
        public virtual bool HasChanged { get; set; }
        
        protected bool _IsDirty;
        public virtual bool IsDirty
        {
            [DebuggerStepThrough]
            get => _IsDirty|| LazinatorObjectBytes.Length == 0;
            [DebuggerStepThrough]
            set
            {
                if (_IsDirty != value)
                {
                    _IsDirty = value;
                    if (_IsDirty)
                    {
                        LazinatorParents.InformParentsOfDirtiness();
                        HasChanged = true;
                    }
                }
            }
        }
        
        protected bool _DescendantHasChanged;
        public virtual bool DescendantHasChanged
        {
            [DebuggerStepThrough]
            get => _DescendantHasChanged;
            [DebuggerStepThrough]
            set
            {
                _DescendantHasChanged = value;
            }
        }
        
        protected bool _DescendantIsDirty;
        public virtual bool DescendantIsDirty
        {
            [DebuggerStepThrough]
            get => _DescendantIsDirty;
            [DebuggerStepThrough]
            set
            {
                if (_DescendantIsDirty != value)
                {
                    _DescendantIsDirty = value;
                    if (_DescendantIsDirty)
                    {
                        LazinatorParents.InformParentsOfDirtiness();
                        _DescendantHasChanged = true;
                    }
                }
            }
        }
        
        public virtual void DeserializeLazinator(LazinatorMemory serializedBytes)
        {
            LazinatorMemoryStorage = serializedBytes;
            int length = Deserialize();
            if (length != LazinatorMemoryStorage.Length)
            {
                LazinatorMemoryStorage = LazinatorMemoryStorage.Slice(0, length);
            }
        }
        
        public virtual LazinatorMemory LazinatorMemoryStorage
        {
            get;
            set;
        }
        protected virtual ReadOnlyMemory<byte> LazinatorObjectBytes => LazinatorMemoryStorage?.Memory ?? LazinatorUtilities.EmptyReadOnlyMemory;
        
        public virtual void UpdateStoredBuffer(bool disposePreviousBuffer = false)
        {
            if (!IsDirty && !DescendantIsDirty && LazinatorObjectBytes.Length > 0 && OriginalIncludeChildrenMode == IncludeChildrenMode.IncludeAllChildren)
            {
                return;
            }
            var previousBuffer = LazinatorMemoryStorage;
            EncodeOrRecycleToNewBuffer(IncludeChildrenMode.IncludeAllChildren, OriginalIncludeChildrenMode, false, IsDirty, DescendantIsDirty, false, previousBuffer, true, this);
            OriginalIncludeChildrenMode = IncludeChildrenMode.IncludeAllChildren;
            if (disposePreviousBuffer)
            {
                previousBuffer.Dispose();
            }
        }
        
        public virtual int GetByteLength()
        {
            UpdateStoredBuffer();
            return LazinatorObjectBytes.Length;
        }
        
        public virtual bool NonBinaryHash32 => false;
        
        
        public IEnumerable<ILazinator> EnumerateLazinatorNodes(Func<ILazinator, bool> matchCriterion, bool stopExploringBelowMatch, Func<ILazinator, bool> exploreCriterion, bool exploreOnlyDeserializedChildren, bool enumerateNulls)
        {
            bool match = (matchCriterion == null) ? true : matchCriterion(this);
            bool explore = (!match || !stopExploringBelowMatch) && ((exploreCriterion == null) ? true : exploreCriterion(this));
            if (match)
            {
                yield return this;
            }
            if (explore)
            {
                foreach (var item in EnumerateLazinatorDescendants(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
                {
                    yield return item.descendant;
                }
            }
        }
        
        public virtual IEnumerable<(string propertyName, ILazinator descendant)> EnumerateLazinatorDescendants(Func<ILazinator, bool> matchCriterion, bool stopExploringBelowMatch, Func<ILazinator, bool> exploreCriterion, bool exploreOnlyDeserializedChildren, bool enumerateNulls)
        {
            yield break;
        }
        
        
        public virtual IEnumerable<(string propertyName, object descendant)> EnumerateNonLazinatorProperties()
        {
            yield return ("MyQueueSerialized", (object)MyQueueSerialized);
            yield break;
        }
        
        public virtual ILazinator ForEachLazinator(Func<ILazinator, ILazinator> changeFunc, bool exploreOnlyDeserializedChildren)
        {
            if ((!exploreOnlyDeserializedChildren && MyQueueSerialized != null) || (_MyQueueSerialized_Accessed && _MyQueueSerialized != null))
            {
                _MyQueueSerialized = (Queue<ExampleChild>) CloneOrChange_Queue_GExampleChild_g(_MyQueueSerialized, l => l?.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren), true);
            }
            return changeFunc(this);
        }
        
        public virtual void FreeInMemoryObjects()
        {
            _MyQueueSerialized = default;
            _MyQueueSerialized_Accessed = false;
            IsDirty = false;
            DescendantIsDirty = false;
            HasChanged = false;
            DescendantHasChanged = false;
        }
        
        /* Conversion */
        
        public virtual int LazinatorUniqueID => 270;
        
        protected virtual bool ContainsOpenGenericParameters => false;
        protected virtual LazinatorGenericIDType _LazinatorGenericID { get; set; }
        public virtual LazinatorGenericIDType LazinatorGenericID
        {
            get => default;
            set { }
        }
        
        public virtual int LazinatorObjectVersion { get; set; } = 0;
        
        
        public virtual void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
            _MyQueueSerialized_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            _DotNetQueue_Lazinator_EndByteIndex = bytesSoFar;
        }
        
        public virtual void SerializeExistingBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren)
            {
                updateStoredBuffer = false;
            }
            int startPosition = writer.Position;
            WritePropertiesIntoBuffer(ref writer, includeChildrenMode, verifyCleanness, updateStoredBuffer, true);
            if (updateStoredBuffer)
            {
                UpdateStoredBuffer(ref writer, startPosition, writer.Position - startPosition, includeChildrenMode, false);
            }
        }
        
        public virtual void UpdateStoredBuffer(ref BinaryBufferWriter writer, int startPosition, int length, IncludeChildrenMode includeChildrenMode, bool updateDeserializedChildren)
        {
            _IsDirty = false;
            if (includeChildrenMode == IncludeChildrenMode.IncludeAllChildren)
            {
                _DescendantIsDirty = false;
                if (updateDeserializedChildren)
                {
                    if (_MyQueueSerialized_Accessed && _MyQueueSerialized != null)
                    {
                        _MyQueueSerialized = (Queue<ExampleChild>) CloneOrChange_Queue_GExampleChild_g(_MyQueueSerialized, l => l.RemoveBufferInHierarchy(), true);
                    }
                }
                
            }
            else
            {
                throw new LazinatorSerializationException("Cannot update stored buffer when serializing only some children.");
            }
            
            var newBuffer = writer.Slice(startPosition, length);
            LazinatorMemoryStorage = newBuffer;
        }
        
        protected virtual void WritePropertiesIntoBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer, bool includeUniqueID)
        {
            int startPosition = writer.Position;
            int startOfObjectPosition = 0;
            // header information
            if (includeUniqueID)
            {
                if (LazinatorGenericID.IsEmpty)
                {
                    CompressedIntegralTypes.WriteCompressedInt(ref writer, LazinatorUniqueID);
                }
                else
                {
                    WriteLazinatorGenericID(ref writer, LazinatorGenericID);
                }
            }
            CompressedIntegralTypes.WriteCompressedInt(ref writer, Lazinator.Support.LazinatorVersionInfo.LazinatorIntVersion);
            CompressedIntegralTypes.WriteCompressedInt(ref writer, LazinatorObjectVersion);
            writer.Write((byte)includeChildrenMode);
            // write properties
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode && !_MyQueueSerialized_Accessed)
            {
                var deserialized = MyQueueSerialized;
            }
            WriteNonLazinatorObject(
            nonLazinatorObject: _MyQueueSerialized, isBelievedDirty: _MyQueueSerialized_Accessed || (includeChildrenMode != OriginalIncludeChildrenMode),
            isAccessed: _MyQueueSerialized_Accessed, writer: ref writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorMemoryStorage, _MyQueueSerialized_ByteIndex, _MyQueueSerialized_ByteLength, false, false, null),
            verifyCleanness: false,
            binaryWriterAction: (ref BinaryBufferWriter w, bool v) =>
            ConvertToBytes_Queue_GExampleChild_g(ref w, _MyQueueSerialized,
            includeChildrenMode, v, updateStoredBuffer));
            if (updateStoredBuffer)
            {
                _MyQueueSerialized_ByteIndex = startOfObjectPosition - startPosition;
            }
            if (updateStoredBuffer)
            {
                _DotNetQueue_Lazinator_EndByteIndex = writer.Position - startPosition;
            }
        }
        
        /* Conversion of supported collections and tuples */
        
        private static Queue<ExampleChild> ConvertFromBytes_Queue_GExampleChild_g(LazinatorMemory storage)
        {
            if (storage.Length == 0)
            {
                return default(Queue<ExampleChild>);
            }
            ReadOnlySpan<byte> span = storage.Span;
            
            int bytesSoFar = 0;
            int collectionLength = span.ToDecompressedInt(ref bytesSoFar);
            
            Queue<ExampleChild> collection = new Queue<ExampleChild>(collectionLength);
            for (int itemIndex = 0; itemIndex < collectionLength; itemIndex++)
            {
                int lengthCollectionMember = span.ToInt32(ref bytesSoFar);
                if (lengthCollectionMember == 0)
                {
                    collection.Enqueue(default(ExampleChild));
                }
                else
                {
                    LazinatorMemory childData = storage.Slice(bytesSoFar, lengthCollectionMember);
                    var item = DeserializationFactory.Instance.CreateBasedOnType<ExampleChild>(childData);
                    collection.Enqueue(item);
                }
                bytesSoFar += lengthCollectionMember;
            }
            
            return collection;
        }
        
        private static void ConvertToBytes_Queue_GExampleChild_g(ref BinaryBufferWriter writer, Queue<ExampleChild> itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            if (itemToConvert == default(Queue<ExampleChild>))
            {
                return;
            }
            CompressedIntegralTypes.WriteCompressedInt(ref writer, itemToConvert.Count);
            int itemToConvertCount = itemToConvert.Count;
            var q = System.Linq.Enumerable.ToList(itemToConvert);
            for (int itemIndex = 0; itemIndex < itemToConvertCount; itemIndex++)
            {
                if (q[itemIndex] == default(ExampleChild))
                {
                    writer.Write((uint)0);
                }
                else 
                {
                    
                    void action(ref BinaryBufferWriter w) => q[itemIndex].SerializeExistingBuffer(ref w, includeChildrenMode, verifyCleanness, updateStoredBuffer);
                    WriteToBinaryWithIntLengthPrefix(ref writer, action);
                }
                
            }
        }
        
        private static Queue<ExampleChild> CloneOrChange_Queue_GExampleChild_g(Queue<ExampleChild> itemToClone, Func<ILazinator, ILazinator> cloneOrChangeFunc, bool avoidCloningIfPossible)
        {
            if (itemToClone == null)
            {
                return default;
            }
            int collectionLength = itemToClone.Count;
            Queue<ExampleChild> collection = new Queue<ExampleChild>(collectionLength);
            int itemToCloneCount = itemToClone.Count;
            var q = System.Linq.Enumerable.ToList(itemToClone);
            for (int itemIndex = 0; itemIndex < itemToCloneCount; itemIndex++)
            {
                if (q[itemIndex] == null)
                {
                    collection.Enqueue(default(ExampleChild));
                }
                else
                {
                    var itemCopied = (ExampleChild) cloneOrChangeFunc(q[itemIndex]);
                    collection.Enqueue(itemCopied);
                }
            }
            return collection;
        }
        
    }
}
