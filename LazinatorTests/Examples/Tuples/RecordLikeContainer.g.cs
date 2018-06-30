//08ba58fa-183c-a490-5ada-eed480da9aee
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.170
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LazinatorTests.Examples.Tuples
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
    public partial class RecordLikeContainer : ILazinator
    {
        public bool IsStruct => false;
        
        /* Serialization, deserialization, and object relationships */
        
        public RecordLikeContainer() : base()
        {
        }
        
        public virtual LazinatorParentsCollection LazinatorParents { get; set; }
        
        protected IncludeChildrenMode OriginalIncludeChildrenMode;
        
        public virtual int Deserialize()
        {
            ResetAccessedProperties();
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
        
        public virtual MemoryInBuffer SerializeNewBuffer(IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
            return EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, verifyCleanness, IsDirty, DescendantIsDirty, false, LazinatorObjectBytes, (StreamManuallyDelegate) EncodeToNewBuffer);
        }
        
        protected virtual MemoryInBuffer EncodeToNewBuffer(IncludeChildrenMode includeChildrenMode, bool verifyCleanness) => LazinatorUtilities.EncodeToNewBinaryBufferWriter(this, includeChildrenMode, verifyCleanness);
        
        public virtual ILazinator CloneLazinator()
        {
            return CloneLazinator(OriginalIncludeChildrenMode);
        }
        
        public virtual ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode)
        {
            MemoryInBuffer bytes = EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, false, IsDirty, DescendantIsDirty, false, LazinatorObjectBytes, (StreamManuallyDelegate)EncodeToNewBuffer);
            var clone = new RecordLikeContainer()
            {
                LazinatorParents = LazinatorParents,
                OriginalIncludeChildrenMode = includeChildrenMode,
                HierarchyBytes = bytes,
            };
            clone.LazinatorParents = default;
            return clone;
        }
        
        public virtual bool HasChanged { get; set; }
        
        protected bool _IsDirty;
        public virtual bool IsDirty
        {
            [DebuggerStepThrough]
            get => _IsDirty || _LazinatorObjectBytes.Length == 0;
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
        
        private MemoryInBuffer _HierarchyBytes;
        public virtual MemoryInBuffer HierarchyBytes
        {
            set
            {
                _HierarchyBytes = value;
                LazinatorObjectBytes = value.FilledMemory;
            }
        }
        
        protected ReadOnlyMemory<byte> _LazinatorObjectBytes;
        public virtual ReadOnlyMemory<byte> LazinatorObjectBytes
        {
            get => _LazinatorObjectBytes;
            set
            {
                _LazinatorObjectBytes = value;
                int length = Deserialize();
                _LazinatorObjectBytes = _LazinatorObjectBytes.Slice(0, length);
            }
        }
        
        public virtual void LazinatorConvertToBytes()
        {
            if (!IsDirty && !DescendantIsDirty && _LazinatorObjectBytes.Length > 0)
            {
                return;
            }
            MemoryInBuffer bytes = EncodeOrRecycleToNewBuffer(IncludeChildrenMode.IncludeAllChildren, OriginalIncludeChildrenMode, false, IsDirty, DescendantIsDirty, false, LazinatorObjectBytes, (StreamManuallyDelegate)EncodeToNewBuffer);
            _LazinatorObjectBytes = bytes.FilledMemory;
        }
        
        public virtual int GetByteLength()
        {
            LazinatorConvertToBytes();
            return _LazinatorObjectBytes.Length;
        }
        
        public virtual uint GetBinaryHashCode32()
        {
            LazinatorConvertToBytes();
            return FarmhashByteSpans.Hash32(LazinatorObjectBytes.Span);
        }
        
        public virtual ulong GetBinaryHashCode64()
        {
            LazinatorConvertToBytes();
            return FarmhashByteSpans.Hash64(LazinatorObjectBytes.Span);
        }
        
        public virtual Guid GetBinaryHashCode128()
        {
            LazinatorConvertToBytes();
            return FarmhashByteSpans.Hash128(LazinatorObjectBytes.Span);
        }
        
        /* Property definitions */
        
        protected int _MyMismatchedRecordLikeType_ByteIndex;
        protected int _MyRecordLikeClass_ByteIndex;
        protected int _MyRecordLikeType_ByteIndex;
        protected virtual int _MyMismatchedRecordLikeType_ByteLength => _MyRecordLikeClass_ByteIndex - _MyMismatchedRecordLikeType_ByteIndex;
        protected virtual int _MyRecordLikeClass_ByteLength => _MyRecordLikeType_ByteIndex - _MyRecordLikeClass_ByteIndex;
        private int _RecordLikeContainer_EndByteIndex;
        protected virtual int _MyRecordLikeType_ByteLength => _RecordLikeContainer_EndByteIndex - _MyRecordLikeType_ByteIndex;
        
        protected MismatchedRecordLikeType _MyMismatchedRecordLikeType;
        public MismatchedRecordLikeType MyMismatchedRecordLikeType
        {
            get
            {
                if (!_MyMismatchedRecordLikeType_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _MyMismatchedRecordLikeType = default(MismatchedRecordLikeType);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _MyMismatchedRecordLikeType_ByteIndex, _MyMismatchedRecordLikeType_ByteLength, false, false, null);
                        _MyMismatchedRecordLikeType = ConvertFromBytes_MismatchedRecordLikeType(childData);
                    }
                    _MyMismatchedRecordLikeType_Accessed = true;
                } 
                return _MyMismatchedRecordLikeType;
            }
            set
            {IsDirty = true;
                DescendantIsDirty = true;
                _MyMismatchedRecordLikeType = value;
                _MyMismatchedRecordLikeType_Accessed = true;
            }
        }
        protected bool _MyMismatchedRecordLikeType_Accessed;
        protected RecordLikeClass _MyRecordLikeClass;
        public RecordLikeClass MyRecordLikeClass
        {
            get
            {
                if (!_MyRecordLikeClass_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _MyRecordLikeClass = default(RecordLikeClass);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _MyRecordLikeClass_ByteIndex, _MyRecordLikeClass_ByteLength, false, false, null);
                        _MyRecordLikeClass = ConvertFromBytes_RecordLikeClass(childData);
                    }
                    _MyRecordLikeClass_Accessed = true;
                }
                IsDirty = true; 
                return _MyRecordLikeClass;
            }
            set
            {IsDirty = true;
                DescendantIsDirty = true;
                _MyRecordLikeClass = value;
                _MyRecordLikeClass_Accessed = true;
            }
        }
        protected bool _MyRecordLikeClass_Accessed;
        protected RecordLikeType _MyRecordLikeType;
        public RecordLikeType MyRecordLikeType
        {
            get
            {
                if (!_MyRecordLikeType_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _MyRecordLikeType = default(RecordLikeType);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _MyRecordLikeType_ByteIndex, _MyRecordLikeType_ByteLength, false, false, null);
                        _MyRecordLikeType = ConvertFromBytes_RecordLikeType(childData);
                    }
                    _MyRecordLikeType_Accessed = true;
                } 
                return _MyRecordLikeType;
            }
            set
            {IsDirty = true;
                DescendantIsDirty = true;
                _MyRecordLikeType = value;
                _MyRecordLikeType_Accessed = true;
            }
        }
        protected bool _MyRecordLikeType_Accessed;
        
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
            yield return ("MyMismatchedRecordLikeType", (object)MyMismatchedRecordLikeType);
            yield return ("MyRecordLikeClass", (object)MyRecordLikeClass);
            yield return ("MyRecordLikeType", (object)MyRecordLikeType);
            yield break;
        }
        
        protected virtual void ResetAccessedProperties()
        {
            _MyMismatchedRecordLikeType_Accessed = _MyRecordLikeClass_Accessed = _MyRecordLikeType_Accessed = false;
            IsDirty = false;
            DescendantIsDirty = false;
        }
        
        /* Conversion */
        
        public virtual int LazinatorUniqueID => 226;
        
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
            _MyMismatchedRecordLikeType_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            _MyRecordLikeClass_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            _MyRecordLikeType_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            _RecordLikeContainer_EndByteIndex = bytesSoFar;
        }
        
        public virtual void SerializeExistingBuffer(BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren)
            {
                updateStoredBuffer = false;
            }
            int startPosition = writer.Position;
            WritePropertiesIntoBuffer(writer, includeChildrenMode, verifyCleanness, updateStoredBuffer, true);
            if (updateStoredBuffer)
            {
                
                _IsDirty = false;
                if (includeChildrenMode == IncludeChildrenMode.IncludeAllChildren)
                {
                    _DescendantIsDirty = false;
                }
                else
                {
                    throw new Exception("Cannot update stored buffer when serializing only some children.");
                }
                
                _LazinatorObjectBytes = writer.Slice(startPosition);
            }
        }
        protected virtual void WritePropertiesIntoBuffer(BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer, bool includeUniqueID)
        {
            int startPosition = writer.Position;
            int startOfObjectPosition = 0;
            // header information
            if (includeUniqueID)
            {
                if (LazinatorGenericID.IsEmpty)
                {
                    CompressedIntegralTypes.WriteCompressedInt(writer, LazinatorUniqueID);
                }
                else
                {
                    WriteLazinatorGenericID(writer, LazinatorGenericID);
                }
            }
            CompressedIntegralTypes.WriteCompressedInt(writer, Lazinator.Support.LazinatorVersionInfo.LazinatorIntVersion);
            CompressedIntegralTypes.WriteCompressedInt(writer, LazinatorObjectVersion);
            writer.Write((byte)includeChildrenMode);
            // write properties
            startOfObjectPosition = writer.Position;
            WriteNonLazinatorObject(
            nonLazinatorObject: _MyMismatchedRecordLikeType, isBelievedDirty: _MyMismatchedRecordLikeType_Accessed,
            isAccessed: _MyMismatchedRecordLikeType_Accessed, writer: writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorObjectBytes, _MyMismatchedRecordLikeType_ByteIndex, _MyMismatchedRecordLikeType_ByteLength, false, false, null),
            verifyCleanness: false,
            binaryWriterAction: (w, v) =>
            ConvertToBytes_MismatchedRecordLikeType(w, _MyMismatchedRecordLikeType,
            includeChildrenMode, v, updateStoredBuffer));
            if (updateStoredBuffer)
            {
                _MyMismatchedRecordLikeType_ByteIndex = startOfObjectPosition - startPosition;
            }
            startOfObjectPosition = writer.Position;
            WriteNonLazinatorObject(
            nonLazinatorObject: _MyRecordLikeClass, isBelievedDirty: _MyRecordLikeClass_Accessed,
            isAccessed: _MyRecordLikeClass_Accessed, writer: writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorObjectBytes, _MyRecordLikeClass_ByteIndex, _MyRecordLikeClass_ByteLength, false, false, null),
            verifyCleanness: false,
            binaryWriterAction: (w, v) =>
            ConvertToBytes_RecordLikeClass(w, _MyRecordLikeClass,
            includeChildrenMode, v, updateStoredBuffer));
            if (updateStoredBuffer)
            {
                _MyRecordLikeClass_ByteIndex = startOfObjectPosition - startPosition;
            }
            startOfObjectPosition = writer.Position;
            WriteNonLazinatorObject(
            nonLazinatorObject: _MyRecordLikeType, isBelievedDirty: _MyRecordLikeType_Accessed,
            isAccessed: _MyRecordLikeType_Accessed, writer: writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorObjectBytes, _MyRecordLikeType_ByteIndex, _MyRecordLikeType_ByteLength, false, false, null),
            verifyCleanness: false,
            binaryWriterAction: (w, v) =>
            ConvertToBytes_RecordLikeType(w, _MyRecordLikeType,
            includeChildrenMode, v, updateStoredBuffer));
            if (updateStoredBuffer)
            {
                _MyRecordLikeType_ByteIndex = startOfObjectPosition - startPosition;
            }
            if (updateStoredBuffer)
            {
                _RecordLikeContainer_EndByteIndex = writer.Position - startPosition;
            }
        }
        
        /* Conversion of supported collections and tuples */
        
        private static MismatchedRecordLikeType ConvertFromBytes_MismatchedRecordLikeType(ReadOnlyMemory<byte> storage)
        {
            if (storage.Length == 0)
            {
                return default;
            }
            ReadOnlySpan<byte> span = storage.Span;
            
            int bytesSoFar = 0;
            
            int item1 = span.ToDecompressedInt(ref bytesSoFar);
            
            string item2 = span.ToString_VarIntLengthUtf8(ref bytesSoFar);
            
            var tupleType = new MismatchedRecordLikeType(item1, item2);
            
            return tupleType;
        }
        
        private static void ConvertToBytes_MismatchedRecordLikeType(BinaryBufferWriter writer, MismatchedRecordLikeType itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            
            CompressedIntegralTypes.WriteCompressedInt(writer, itemToConvert.Age);
            
            EncodeCharAndString.WriteStringUtf8WithVarIntPrefix(writer, itemToConvert.Name);
        }
        
        private static RecordLikeClass ConvertFromBytes_RecordLikeClass(ReadOnlyMemory<byte> storage)
        {
            if (storage.Length == 0)
            {
                return default;
            }
            ReadOnlySpan<byte> span = storage.Span;
            
            int bytesSoFar = 0;
            
            int item1 = span.ToDecompressedInt(ref bytesSoFar);
            
            Example item2 = default;
            int lengthCollectionMember_item2 = span.ToInt32(ref bytesSoFar);
            if (lengthCollectionMember_item2 != 0)
            {
                ReadOnlyMemory<byte> childData = storage.Slice(bytesSoFar, lengthCollectionMember_item2);
                item2 = DeserializationFactory.Instance.CreateBasedOnType<Example>(childData);
            }
            bytesSoFar += lengthCollectionMember_item2;
            
            var tupleType = new RecordLikeClass(item1, item2);
            
            return tupleType;
        }
        
        private static void ConvertToBytes_RecordLikeClass(BinaryBufferWriter writer, RecordLikeClass itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            
            CompressedIntegralTypes.WriteCompressedInt(writer, itemToConvert.Age);
            
            if (itemToConvert.Example == null)
            {
                writer.Write((uint)0);
            }
            else
            {
                void actionExample(BinaryBufferWriter w) => itemToConvert.Example.SerializeExistingBuffer(writer, includeChildrenMode, verifyCleanness, updateStoredBuffer);
                WriteToBinaryWithIntLengthPrefix(writer, actionExample);
            };
        }
        
        private static RecordLikeType ConvertFromBytes_RecordLikeType(ReadOnlyMemory<byte> storage)
        {
            if (storage.Length == 0)
            {
                return default;
            }
            ReadOnlySpan<byte> span = storage.Span;
            
            int bytesSoFar = 0;
            
            int item1 = span.ToDecompressedInt(ref bytesSoFar);
            
            string item2 = span.ToString_VarIntLengthUtf8(ref bytesSoFar);
            
            var tupleType = new RecordLikeType(item1, item2);
            
            return tupleType;
        }
        
        private static void ConvertToBytes_RecordLikeType(BinaryBufferWriter writer, RecordLikeType itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            
            CompressedIntegralTypes.WriteCompressedInt(writer, itemToConvert.Age);
            
            EncodeCharAndString.WriteStringUtf8WithVarIntPrefix(writer, itemToConvert.Name);
        }
        
    }
}
