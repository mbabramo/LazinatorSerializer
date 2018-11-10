//058c59d0-8a8b-3ec0-fbe8-6330383171ef
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.290
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
    public partial class DotNetList_NonLazinator : ILazinator
    {
        public bool IsStruct => false;
        
        /* Serialization, deserialization, and object relationships */
        
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
        
        public virtual LazinatorMemory SerializeLazinator(IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            return EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, verifyCleanness, IsDirty, DescendantIsDirty, false, LazinatorMemoryStorage, (EncodeManuallyDelegate) EncodeToNewBuffer, updateStoredBuffer);
        }
        
        protected virtual LazinatorMemory EncodeToNewBuffer(IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer) => LazinatorUtilities.EncodeToNewBinaryBufferWriter(this, includeChildrenMode, verifyCleanness, updateStoredBuffer);
        
        public virtual ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode = IncludeChildrenMode.IncludeAllChildren, CloneBufferOptions cloneBufferOptions = CloneBufferOptions.LinkedBuffer)
        {
            var clone = new DotNetList_NonLazinator()
            {
                OriginalIncludeChildrenMode = includeChildrenMode
            };
            if (clone.LazinatorObjectVersion != LazinatorObjectVersion)
            {
                clone.LazinatorObjectVersion = LazinatorObjectVersion;
            }
            
            if (cloneBufferOptions == CloneBufferOptions.NoBuffer)
            {
                AssignCloneProperties(clone, includeChildrenMode);
            }
            else
            {
                LazinatorMemory bytes = EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, false, IsDirty, DescendantIsDirty, false, LazinatorMemoryStorage, (EncodeManuallyDelegate)EncodeToNewBuffer, cloneBufferOptions == CloneBufferOptions.SharedBuffer);
                clone.DeserializeLazinator(bytes);
                if (cloneBufferOptions == CloneBufferOptions.IndependentBuffers)
                {
                    clone.LazinatorMemoryStorage.DisposeIndependently();
                }
            }
            clone.LazinatorParents = default;
            return clone;
        }
        
        protected virtual void AssignCloneProperties(ILazinator clone, IncludeChildrenMode includeChildrenMode)
        {
            clone.FreeInMemoryObjects();
            DotNetList_NonLazinator typedClone = (DotNetList_NonLazinator) clone;
            typedClone.MyListNonLazinatorType = CloneOrChange_List_GNonLazinatorClass_g(MyListNonLazinatorType, l => l?.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer));
            typedClone.MyListNonLazinatorType2 = CloneOrChange_List_GNonLazinatorClass_g(MyListNonLazinatorType2, l => l?.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer));
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
        
        public virtual void EnsureLazinatorMemoryUpToDate()
        {
            if (!IsDirty && !DescendantIsDirty && LazinatorObjectBytes.Length > 0 && OriginalIncludeChildrenMode == IncludeChildrenMode.IncludeAllChildren)
            {
                return;
            }
            EncodeOrRecycleToNewBuffer(IncludeChildrenMode.IncludeAllChildren, OriginalIncludeChildrenMode, false, IsDirty, DescendantIsDirty, false, LazinatorMemoryStorage, (EncodeManuallyDelegate)EncodeToNewBuffer, true);
            OriginalIncludeChildrenMode = IncludeChildrenMode.IncludeAllChildren;
        }
        
        public virtual int GetByteLength()
        {
            EnsureLazinatorMemoryUpToDate();
            return LazinatorObjectBytes.Length;
        }
        
        public virtual uint GetBinaryHashCode32()
        {
            EnsureLazinatorMemoryUpToDate();
            return FarmhashByteSpans.Hash32(LazinatorObjectBytes.Span);
        }
        
        public virtual ulong GetBinaryHashCode64()
        {
            EnsureLazinatorMemoryUpToDate();
            return FarmhashByteSpans.Hash64(LazinatorObjectBytes.Span);
        }
        
        public virtual Guid GetBinaryHashCode128()
        {
            EnsureLazinatorMemoryUpToDate();
            return FarmhashByteSpans.Hash128(LazinatorObjectBytes.Span);
        }
        
        /* Property definitions */
        
        protected int _MyListNonLazinatorType_ByteIndex;
        protected int _MyListNonLazinatorType2_ByteIndex;
        protected virtual int _MyListNonLazinatorType_ByteLength => _MyListNonLazinatorType2_ByteIndex - _MyListNonLazinatorType_ByteIndex;
        private int _DotNetList_NonLazinator_EndByteIndex;
        protected virtual int _MyListNonLazinatorType2_ByteLength => _DotNetList_NonLazinator_EndByteIndex - _MyListNonLazinatorType2_ByteIndex;
        
        
        protected List<NonLazinatorClass> _MyListNonLazinatorType;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public List<NonLazinatorClass> MyListNonLazinatorType
        {
            get
            {
                if (!_MyListNonLazinatorType_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _MyListNonLazinatorType = default(List<NonLazinatorClass>);
                        _MyListNonLazinatorType_Dirty = true; 
                    }
                    else
                    {
                        LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _MyListNonLazinatorType_ByteIndex, _MyListNonLazinatorType_ByteLength, false, false, null);
                        _MyListNonLazinatorType = ConvertFromBytes_List_GNonLazinatorClass_g(childData);
                    }
                    _MyListNonLazinatorType_Accessed = true;
                } 
                return _MyListNonLazinatorType;
            }
            set
            {
                IsDirty = true;
                DescendantIsDirty = true;
                _MyListNonLazinatorType = value;
                _MyListNonLazinatorType_Dirty = true;
                _MyListNonLazinatorType_Accessed = true;
            }
        }
        protected bool _MyListNonLazinatorType_Accessed;
        
        private bool _MyListNonLazinatorType_Dirty;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool MyListNonLazinatorType_Dirty
        {
            get => _MyListNonLazinatorType_Dirty;
            set
            {
                if (_MyListNonLazinatorType_Dirty != value)
                {
                    _MyListNonLazinatorType_Dirty = value;
                }
                if (value && !IsDirty)
                {
                    IsDirty = true;
                }
            }
        }
        
        protected List<NonLazinatorClass> _MyListNonLazinatorType2;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public List<NonLazinatorClass> MyListNonLazinatorType2
        {
            get
            {
                if (!_MyListNonLazinatorType2_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _MyListNonLazinatorType2 = default(List<NonLazinatorClass>);
                    }
                    else
                    {
                        LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _MyListNonLazinatorType2_ByteIndex, _MyListNonLazinatorType2_ByteLength, false, false, null);
                        _MyListNonLazinatorType2 = ConvertFromBytes_List_GNonLazinatorClass_g(childData);
                    }
                    _MyListNonLazinatorType2_Accessed = true;
                }
                IsDirty = true; 
                return _MyListNonLazinatorType2;
            }
            set
            {
                IsDirty = true;
                DescendantIsDirty = true;
                _MyListNonLazinatorType2 = value;
                _MyListNonLazinatorType2_Accessed = true;
            }
        }
        protected bool _MyListNonLazinatorType2_Accessed;
        
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
            yield return ("MyListNonLazinatorType", (object)MyListNonLazinatorType);
            yield return ("MyListNonLazinatorType2", (object)MyListNonLazinatorType2);
            yield break;
        }
        
        public virtual ILazinator ForEachLazinator(Func<ILazinator, ILazinator> changeFunc, bool exploreOnlyDeserializedChildren)
        {
            if ((!exploreOnlyDeserializedChildren && MyListNonLazinatorType != null) || (_MyListNonLazinatorType_Accessed && _MyListNonLazinatorType != null))
            {
                _MyListNonLazinatorType = (List<NonLazinatorClass>) CloneOrChange_List_GNonLazinatorClass_g(_MyListNonLazinatorType, l => l?.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren));
            }
            if ((!exploreOnlyDeserializedChildren && MyListNonLazinatorType2 != null) || (_MyListNonLazinatorType2_Accessed && _MyListNonLazinatorType2 != null))
            {
                _MyListNonLazinatorType2 = (List<NonLazinatorClass>) CloneOrChange_List_GNonLazinatorClass_g(_MyListNonLazinatorType2, l => l?.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren));
            }
            return changeFunc(this);
        }
        
        public virtual void FreeInMemoryObjects()
        {
            _MyListNonLazinatorType = default;
            _MyListNonLazinatorType2 = default;
            _MyListNonLazinatorType_Accessed = _MyListNonLazinatorType2_Accessed = false;
            IsDirty = false;
            DescendantIsDirty = false;
            HasChanged = false;
            DescendantHasChanged = false;
        }
        
        /* Conversion */
        
        public virtual int LazinatorUniqueID => 207;
        
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
            _MyListNonLazinatorType_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            _MyListNonLazinatorType2_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            _DotNetList_NonLazinator_EndByteIndex = bytesSoFar;
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
                    if (_MyListNonLazinatorType_Accessed && _MyListNonLazinatorType != null)
                    {
                        _MyListNonLazinatorType = (List<NonLazinatorClass>) CloneOrChange_List_GNonLazinatorClass_g(_MyListNonLazinatorType, l => l.RemoveBufferInHierarchy());
                    }
                    if (_MyListNonLazinatorType2_Accessed && _MyListNonLazinatorType2 != null)
                    {
                        _MyListNonLazinatorType2 = (List<NonLazinatorClass>) CloneOrChange_List_GNonLazinatorClass_g(_MyListNonLazinatorType2, l => l.RemoveBufferInHierarchy());
                    }
                }
                
            }
            else
            {
                throw new LazinatorSerializationException("Cannot update stored buffer when serializing only some children.");
            }
            
            var newBuffer = writer.Slice(startPosition, length);
            LazinatorMemoryStorage = ReplaceBuffer(LazinatorMemoryStorage, newBuffer, LazinatorParents, startPosition == 0, IsStruct);
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
            if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode && !_MyListNonLazinatorType_Accessed)
            {
                var deserialized = MyListNonLazinatorType;
            }
            WriteNonLazinatorObject(
            nonLazinatorObject: _MyListNonLazinatorType, isBelievedDirty: MyListNonLazinatorType_Dirty || (includeChildrenMode != OriginalIncludeChildrenMode),
            isAccessed: _MyListNonLazinatorType_Accessed, writer: ref writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorMemoryStorage, _MyListNonLazinatorType_ByteIndex, _MyListNonLazinatorType_ByteLength, false, false, null),
            verifyCleanness: verifyCleanness,
            binaryWriterAction: (ref BinaryBufferWriter w, bool v) =>
            ConvertToBytes_List_GNonLazinatorClass_g(ref w, _MyListNonLazinatorType,
            includeChildrenMode, v, updateStoredBuffer));
            if (updateStoredBuffer)
            {
                _MyListNonLazinatorType_ByteIndex = startOfObjectPosition - startPosition;
            }
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode && !_MyListNonLazinatorType2_Accessed)
            {
                var deserialized = MyListNonLazinatorType2;
            }
            WriteNonLazinatorObject(
            nonLazinatorObject: _MyListNonLazinatorType2, isBelievedDirty: _MyListNonLazinatorType2_Accessed || (includeChildrenMode != OriginalIncludeChildrenMode),
            isAccessed: _MyListNonLazinatorType2_Accessed, writer: ref writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorMemoryStorage, _MyListNonLazinatorType2_ByteIndex, _MyListNonLazinatorType2_ByteLength, false, false, null),
            verifyCleanness: false,
            binaryWriterAction: (ref BinaryBufferWriter w, bool v) =>
            ConvertToBytes_List_GNonLazinatorClass_g(ref w, _MyListNonLazinatorType2,
            includeChildrenMode, v, updateStoredBuffer));
            if (updateStoredBuffer)
            {
                _MyListNonLazinatorType2_ByteIndex = startOfObjectPosition - startPosition;
            }
            if (updateStoredBuffer)
            {
                _DotNetList_NonLazinator_EndByteIndex = writer.Position - startPosition;
            }
        }
        
        /* Conversion of supported collections and tuples */
        
        private static List<NonLazinatorClass> ConvertFromBytes_List_GNonLazinatorClass_g(LazinatorMemory storage)
        {
            if (storage.Length == 0)
            {
                return default(List<NonLazinatorClass>);
            }
            storage.LazinatorShouldNotReturnToPool();
            ReadOnlySpan<byte> span = storage.Span;
            
            int bytesSoFar = 0;
            int collectionLength = span.ToDecompressedInt(ref bytesSoFar);
            
            List<NonLazinatorClass> collection = new List<NonLazinatorClass>(collectionLength);
            for (int itemIndex = 0; itemIndex < collectionLength; itemIndex++)
            {
                int lengthCollectionMember = span.ToInt32(ref bytesSoFar);
                if (lengthCollectionMember == 0)
                {
                    collection.Add(default(NonLazinatorClass));
                }
                else
                {
                    LazinatorMemory childData = storage.Slice(bytesSoFar, lengthCollectionMember);
                    var item = NonLazinatorDirectConverter.ConvertFromBytes_NonLazinatorClass(childData);
                    collection.Add(item);
                }
                bytesSoFar += lengthCollectionMember;
            }
            
            return collection;
        }
        
        private static void ConvertToBytes_List_GNonLazinatorClass_g(ref BinaryBufferWriter writer, List<NonLazinatorClass> itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            if (itemToConvert == default(List<NonLazinatorClass>))
            {
                return;
            }
            CompressedIntegralTypes.WriteCompressedInt(ref writer, itemToConvert.Count);
            int itemToConvertCount = itemToConvert.Count;
            for (int itemIndex = 0; itemIndex < itemToConvertCount; itemIndex++)
            {
                if (itemToConvert[itemIndex] == default(NonLazinatorClass))
                {
                    writer.Write((uint)0);
                }
                else 
                {
                    
                    void action(ref BinaryBufferWriter w) => NonLazinatorDirectConverter.ConvertToBytes_NonLazinatorClass(ref w, itemToConvert[itemIndex], includeChildrenMode, verifyCleanness, updateStoredBuffer);
                    WriteToBinaryWithIntLengthPrefix(ref writer, action);
                }
                
            }
        }
        
        private static List<NonLazinatorClass> CloneOrChange_List_GNonLazinatorClass_g(List<NonLazinatorClass> itemToClone, Func<ILazinator, ILazinator> cloneOrChangeFunc)
        {
            if (itemToClone == null)
            {
                return default;
            }
            
            int collectionLength = itemToClone.Count;
            List<NonLazinatorClass> collection = new List<NonLazinatorClass>(collectionLength);
            int itemToCloneCount = itemToClone.Count;
            for (int itemIndex = 0; itemIndex < itemToCloneCount; itemIndex++)
            {
                if (itemToClone[itemIndex] == null)
                {
                    collection.Add(default(NonLazinatorClass));
                }
                else
                {
                    var itemCopied = (NonLazinatorClass) itemToClone[itemIndex];
                    collection.Add(itemCopied);
                }
            }
            return collection;
        }
        
    }
}
