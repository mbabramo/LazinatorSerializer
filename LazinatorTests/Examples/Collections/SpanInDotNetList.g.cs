//1b51697e-40b8-0e96-90ce-259d8f43e036
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.329
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
    using System;
    using System.Buffers;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;
    using static Lazinator.Core.LazinatorUtilities;
    
    [Autogenerated]
    public partial class SpanInDotNetList : ILazinator
    {
        public bool IsStruct => false;
        
        /* Property definitions */
        
        protected int _SpanList_ByteIndex;
        private int _SpanInDotNetList_EndByteIndex;
        protected virtual int _SpanList_ByteLength => _SpanInDotNetList_EndByteIndex - _SpanList_ByteIndex;
        
        
        protected int _SomeInt;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public int SomeInt
        {
            get
            {
                return _SomeInt;
            }
            set
            {
                IsDirty = true;
                _SomeInt = value;
            }
        }
        
        protected List<SpanAndMemory> _SpanList;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public List<SpanAndMemory> SpanList
        {
            get
            {
                if (!_SpanList_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _SpanList = default(List<SpanAndMemory>);
                    }
                    else
                    {
                        LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _SpanList_ByteIndex, _SpanList_ByteLength, false, false, null);
                        _SpanList = ConvertFromBytes_List_GSpanAndMemory_g(childData);
                    }
                    _SpanList_Accessed = true;
                }
                IsDirty = true; 
                return _SpanList;
            }
            set
            {
                IsDirty = true;
                DescendantIsDirty = true;
                _SpanList = value;
                _SpanList_Accessed = true;
            }
        }
        protected bool _SpanList_Accessed;
        
        /* Serialization, deserialization, and object relationships */
        
        public SpanInDotNetList() : base()
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
            var clone = new SpanInDotNetList()
            {
                OriginalIncludeChildrenMode = includeChildrenMode
            };
            clone = CompleteClone(this, clone, includeChildrenMode, cloneBufferOptions);
            return clone;
        }
        
        public virtual ILazinator AssignCloneProperties(ILazinator clone, IncludeChildrenMode includeChildrenMode)
        {
            clone.FreeInMemoryObjects();
            SpanInDotNetList typedClone = (SpanInDotNetList) clone;
            typedClone.SomeInt = SomeInt;
            typedClone.SpanList = CloneOrChange_List_GSpanAndMemory_g(SpanList, l => l?.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer), false);
            
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
        protected virtual ReadOnlyMemory<byte> LazinatorObjectBytes => LazinatorMemoryStorage.IsEmpty ? LazinatorUtilities.EmptyReadOnlyMemory : LazinatorMemoryStorage.Memory;
        
        public virtual void UpdateStoredBuffer(bool disposePreviousBuffer = false)
        {
            if (!IsDirty && !DescendantIsDirty && LazinatorObjectBytes.Length > 0 && OriginalIncludeChildrenMode == IncludeChildrenMode.IncludeAllChildren)
            {
                return;
            }
            var previousBuffer = LazinatorMemoryStorage;
            LazinatorMemoryStorage = EncodeOrRecycleToNewBuffer(IncludeChildrenMode.IncludeAllChildren, OriginalIncludeChildrenMode, false, IsDirty, DescendantIsDirty, false, previousBuffer, true, this);
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
            yield return ("SomeInt", (object)SomeInt);
            yield return ("SpanList", (object)SpanList);
            yield break;
        }
        
        public virtual ILazinator ForEachLazinator(Func<ILazinator, ILazinator> changeFunc, bool exploreOnlyDeserializedChildren)
        {
            if ((!exploreOnlyDeserializedChildren && SpanList != null) || (_SpanList_Accessed && _SpanList != null))
            {
                _SpanList = (List<SpanAndMemory>) CloneOrChange_List_GSpanAndMemory_g(_SpanList, l => l?.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren), true);
            }
            return changeFunc(this);
        }
        
        public virtual void FreeInMemoryObjects()
        {
            _SpanList = default;
            _SpanList_Accessed = false;
            IsDirty = false;
            DescendantIsDirty = false;
            HasChanged = false;
            DescendantHasChanged = false;
        }
        
        /* Conversion */
        
        public virtual int LazinatorUniqueID => 280;
        
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
            _SomeInt = span.ToDecompressedInt(ref bytesSoFar);
            _SpanList_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            _SpanInDotNetList_EndByteIndex = bytesSoFar;
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
                    if (_SpanList_Accessed && _SpanList != null)
                    {
                        _SpanList = (List<SpanAndMemory>) CloneOrChange_List_GSpanAndMemory_g(_SpanList, l => l.RemoveBufferInHierarchy(), true);
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
            CompressedIntegralTypes.WriteCompressedInt(ref writer, _SomeInt);
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode && !_SpanList_Accessed)
            {
                var deserialized = SpanList;
            }
            WriteNonLazinatorObject(
            nonLazinatorObject: _SpanList, isBelievedDirty: _SpanList_Accessed || (includeChildrenMode != OriginalIncludeChildrenMode),
            isAccessed: _SpanList_Accessed, writer: ref writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorMemoryStorage, _SpanList_ByteIndex, _SpanList_ByteLength, false, false, null),
            verifyCleanness: false,
            binaryWriterAction: (ref BinaryBufferWriter w, bool v) =>
            ConvertToBytes_List_GSpanAndMemory_g(ref w, _SpanList,
            includeChildrenMode, v, updateStoredBuffer));
            if (updateStoredBuffer)
            {
                _SpanList_ByteIndex = startOfObjectPosition - startPosition;
            }
            if (updateStoredBuffer)
            {
                _SpanInDotNetList_EndByteIndex = writer.Position - startPosition;
            }
        }
        
        /* Conversion of supported collections and tuples */
        
        private static List<SpanAndMemory> ConvertFromBytes_List_GSpanAndMemory_g(LazinatorMemory storage)
        {
            if (storage.Length == 0)
            {
                return default(List<SpanAndMemory>);
            }
            ReadOnlySpan<byte> span = storage.Span;
            
            int bytesSoFar = 0;
            int collectionLength = span.ToDecompressedInt(ref bytesSoFar);
            
            List<SpanAndMemory> collection = new List<SpanAndMemory>(collectionLength);
            for (int itemIndex = 0; itemIndex < collectionLength; itemIndex++)
            {
                int lengthCollectionMember = span.ToInt32(ref bytesSoFar);
                if (lengthCollectionMember == 0)
                {
                    collection.Add(default(SpanAndMemory));
                }
                else
                {
                    LazinatorMemory childData = storage.Slice(bytesSoFar, lengthCollectionMember);
                    var item = DeserializationFactory.Instance.CreateBasedOnType<SpanAndMemory>(childData);
                    collection.Add(item);
                }
                bytesSoFar += lengthCollectionMember;
            }
            
            return collection;
        }
        
        private static void ConvertToBytes_List_GSpanAndMemory_g(ref BinaryBufferWriter writer, List<SpanAndMemory> itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            if (itemToConvert == default(List<SpanAndMemory>))
            {
                return;
            }
            CompressedIntegralTypes.WriteCompressedInt(ref writer, itemToConvert.Count);
            int itemToConvertCount = itemToConvert.Count;
            for (int itemIndex = 0; itemIndex < itemToConvertCount; itemIndex++)
            {
                if (itemToConvert[itemIndex] == default(SpanAndMemory))
                {
                    writer.Write((uint)0);
                }
                else 
                {
                    
                    void action(ref BinaryBufferWriter w) => itemToConvert[itemIndex].SerializeExistingBuffer(ref w, includeChildrenMode, verifyCleanness, updateStoredBuffer);
                    WriteToBinaryWithIntLengthPrefix(ref writer, action);
                }
                
            }
        }
        
        private static List<SpanAndMemory> CloneOrChange_List_GSpanAndMemory_g(List<SpanAndMemory> itemToClone, Func<ILazinator, ILazinator> cloneOrChangeFunc, bool avoidCloningIfPossible)
        {
            if (itemToClone == null)
            {
                return default;
            }
            int collectionLength = itemToClone.Count;
            List<SpanAndMemory> collection = avoidCloningIfPossible ? itemToClone : new List<SpanAndMemory>(collectionLength);
            int itemToCloneCount = itemToClone.Count;
            for (int itemIndex = 0; itemIndex < itemToCloneCount; itemIndex++)
            {
                if (avoidCloningIfPossible)
                {
                    if (itemToClone[itemIndex] != null)
                    {
                        itemToClone[itemIndex] = (SpanAndMemory) cloneOrChangeFunc(itemToClone[itemIndex]);
                    }
                    continue;
                }
                if (itemToClone[itemIndex] == null)
                {
                    collection.Add(default(SpanAndMemory));
                }
                else
                {
                    var itemCopied = (SpanAndMemory) cloneOrChangeFunc(itemToClone[itemIndex]);
                    collection.Add(itemCopied);
                }
            }
            return collection;
        }
        
    }
}