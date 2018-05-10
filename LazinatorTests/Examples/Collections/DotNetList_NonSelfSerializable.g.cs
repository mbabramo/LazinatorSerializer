//4241c032-1947-4006-9c2f-217e988ef135
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.20
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Buffers;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using Lazinator.Buffers; 
using Lazinator.Collections;
using Lazinator.Core; 
using static Lazinator.Core.LazinatorUtilities;
using Lazinator.Exceptions;
using Lazinator.Support;
using Lazinator.Wrappers;

namespace LazinatorTests.Examples.Collections
{
    public partial class DotNetList_NonSelfSerializable : ILazinator
    {
        /* Boilerplate for every non-abstract ILazinator object */
        
        public virtual ILazinator LazinatorParentClass { get; set; }
        
        protected internal IncludeChildrenMode OriginalIncludeChildrenMode;
        
        public virtual void Deserialize()
        {
            int bytesSoFar = 0;
            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
            if (span.Length == 0)
            {
                return;
            }
            
            int uniqueID = span.ToDecompressedInt(ref bytesSoFar);
            if (uniqueID != LazinatorUniqueID)
            {
                throw new FormatException("Wrong self-serialized type initialized.");
            }
            
            int lazinatorLibraryVersion = span.ToDecompressedInt(ref bytesSoFar);
            
            int serializedVersionNumber = span.ToDecompressedInt(ref bytesSoFar);
            
            OriginalIncludeChildrenMode = (IncludeChildrenMode)span.ToByte(ref bytesSoFar);
            
            ConvertFromBytesAfterHeader(OriginalIncludeChildrenMode, serializedVersionNumber, ref bytesSoFar);
        }
        
        public virtual MemoryInBuffer SerializeNewBuffer(IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
            return EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, true, verifyCleanness, IsDirty, DescendantIsDirty, false, LazinatorObjectBytes, (StreamManuallyDelegate) EncodeToNewBuffer);
        }
        
        protected internal virtual MemoryInBuffer EncodeToNewBuffer(IncludeChildrenMode includeChildrenMode, bool verifyCleanness) => LazinatorUtilities.EncodeToNewBinaryBufferWriter(this, includeChildrenMode, verifyCleanness);
        
        public virtual ILazinator CloneLazinator()
        {
            return CloneLazinator(OriginalIncludeChildrenMode);
        }
        
        public virtual ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode)
        {
            MemoryInBuffer bytes = EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, false, false, IsDirty, DescendantIsDirty, false, LazinatorObjectBytes, (StreamManuallyDelegate)EncodeToNewBuffer);
            var clone = new DotNetList_NonSelfSerializable()
            {
                DeserializationFactory = DeserializationFactory,
                LazinatorParentClass = LazinatorParentClass,
                InformParentOfDirtinessDelegate = InformParentOfDirtinessDelegate,
                OriginalIncludeChildrenMode = includeChildrenMode,
                HierarchyBytes = bytes
            };
            return clone;
        }
        
        private bool _IsDirty;
        public virtual bool IsDirty
        {
            [DebuggerStepThrough]
            get => _IsDirty;
            [DebuggerStepThrough]
            set
            {
                if (_IsDirty != value)
                {
                    _IsDirty = value;
                    if (_IsDirty)
                    {
                        InformParentOfDirtiness();
                    }
                }
            }
        }
        
        public virtual InformParentOfDirtinessDelegate InformParentOfDirtinessDelegate { get; set; }
        public virtual void InformParentOfDirtiness()
        {
            if (InformParentOfDirtinessDelegate == null)
            {
                if (LazinatorParentClass != null)
                {
                    LazinatorParentClass.DescendantIsDirty = true;
                }
            }
            else
            InformParentOfDirtinessDelegate();
        }
        
        private bool _DescendantIsDirty;
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
                    if (_DescendantIsDirty && LazinatorParentClass != null)
                    {
                        LazinatorParentClass.DescendantIsDirty = true;
                    }
                }
            }
        }
        
        public virtual DeserializationFactory DeserializationFactory { get; set; }
        
        private MemoryInBuffer _HierarchyBytes;
        public virtual MemoryInBuffer HierarchyBytes
        {
            get => _HierarchyBytes;
            set
            {
                _HierarchyBytes = value;
                LazinatorObjectBytes = value.FilledMemory;
            }
        }
        
        private ReadOnlyMemory<byte> _LazinatorObjectBytes;
        public virtual ReadOnlyMemory<byte> LazinatorObjectBytes
        {
            get => _LazinatorObjectBytes;
            set
            {
                _LazinatorObjectBytes = value;
                Deserialize();
            }
        }
        
        public virtual void LazinatorConvertToBytes()
        {
            if (!IsDirty && !DescendantIsDirty)
            {
                return;
            }
            MemoryInBuffer bytes = EncodeOrRecycleToNewBuffer(IncludeChildrenMode.IncludeAllChildren, OriginalIncludeChildrenMode, false, false, IsDirty, DescendantIsDirty, false, LazinatorObjectBytes, (StreamManuallyDelegate)EncodeToNewBuffer);
            _IsDirty = false;
            LazinatorObjectBytes = bytes.FilledMemory;
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
        
        /* Field boilerplate */
        
        internal int _MyListNonLazinatorType_ByteIndex;
        internal int _MyListNonLazinatorType2_ByteIndex;
        internal int _MyListNonLazinatorType_ByteLength => _MyListNonLazinatorType2_ByteIndex - _MyListNonLazinatorType_ByteIndex;
        internal int _MyListNonLazinatorType2_ByteLength => LazinatorObjectBytes.Length - _MyListNonLazinatorType2_ByteIndex;
        
        private System.Collections.Generic.List<LazinatorTests.Examples.NonLazinatorClass> _MyListNonLazinatorType;
        public System.Collections.Generic.List<LazinatorTests.Examples.NonLazinatorClass> MyListNonLazinatorType
        {
            [DebuggerStepThrough]
            get
            {
                if (!_MyListNonLazinatorType_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _MyListNonLazinatorType = default(System.Collections.Generic.List<LazinatorTests.Examples.NonLazinatorClass>);
                        _MyListNonLazinatorType_Dirty = true;
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _MyListNonLazinatorType_ByteIndex, _MyListNonLazinatorType_ByteLength);
                        _MyListNonLazinatorType = ConvertFromBytes_System_Collections_Generic_List_NonLazinatorClass(childData, DeserializationFactory, () => { MyListNonLazinatorType_Dirty = true; });
                    }
                    _MyListNonLazinatorType_Accessed = true;
                }
                return _MyListNonLazinatorType;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _MyListNonLazinatorType = value;
                _MyListNonLazinatorType_Dirty = true;
                _MyListNonLazinatorType_Accessed = true;
            }
        }
        internal bool _MyListNonLazinatorType_Accessed;
        
        private bool _MyListNonLazinatorType_Dirty;
        public bool MyListNonLazinatorType_Dirty
        {
            [DebuggerStepThrough]
            get => _MyListNonLazinatorType_Dirty;
            [DebuggerStepThrough]
            set
            {
                if (_MyListNonLazinatorType_Dirty != value)
                {
                    _MyListNonLazinatorType_Dirty = value;
                    if (value && !IsDirty)
                    IsDirty = true;
                }
            }
        }
        private System.Collections.Generic.List<LazinatorTests.Examples.NonLazinatorClass> _MyListNonLazinatorType2;
        public System.Collections.Generic.List<LazinatorTests.Examples.NonLazinatorClass> MyListNonLazinatorType2
        {
            [DebuggerStepThrough]
            get
            {
                if (!_MyListNonLazinatorType2_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _MyListNonLazinatorType2 = default(System.Collections.Generic.List<LazinatorTests.Examples.NonLazinatorClass>);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _MyListNonLazinatorType2_ByteIndex, _MyListNonLazinatorType2_ByteLength);
                        _MyListNonLazinatorType2 = ConvertFromBytes_System_Collections_Generic_List_NonLazinatorClass(childData, DeserializationFactory, null);
                    }
                    _MyListNonLazinatorType2_Accessed = true;
                    IsDirty = true;
                }
                return _MyListNonLazinatorType2;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _MyListNonLazinatorType2 = value;
                _MyListNonLazinatorType2_Accessed = true;
            }
        }
        internal bool _MyListNonLazinatorType2_Accessed;
        
        /* Conversion */
        
        public virtual int LazinatorUniqueID => 207;
        
        public virtual int LazinatorObjectVersion { get; set; } = 0;
        
        public virtual void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
            _MyListNonLazinatorType_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            _MyListNonLazinatorType2_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
        }
        
        public virtual void SerializeExistingBuffer(BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
            // header information
            CompressedIntegralTypes.WriteCompressedInt(writer, LazinatorUniqueID);
            CompressedIntegralTypes.WriteCompressedInt(writer, Lazinator.Support.LazinatorVersionInfo.LazinatorIntVersion);
            CompressedIntegralTypes.WriteCompressedInt(writer, LazinatorObjectVersion);
            writer.Write((byte)includeChildrenMode);
            // write properties
            WriteNonLazinatorObject(
            nonLazinatorObject: _MyListNonLazinatorType, isBelievedDirty: MyListNonLazinatorType_Dirty,
            isAccessed: _MyListNonLazinatorType_Accessed, writer: writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorObjectBytes, _MyListNonLazinatorType_ByteIndex, _MyListNonLazinatorType_ByteLength),
            verifyCleanness: verifyCleanness,
            binaryWriterAction: (w, v) =>
            ConvertToBytes_System_Collections_Generic_List_NonLazinatorClass(w, MyListNonLazinatorType,
            includeChildrenMode, v));
            WriteNonLazinatorObject(
            nonLazinatorObject: _MyListNonLazinatorType2, isBelievedDirty: _MyListNonLazinatorType2_Accessed,
            isAccessed: _MyListNonLazinatorType2_Accessed, writer: writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorObjectBytes, _MyListNonLazinatorType2_ByteIndex, _MyListNonLazinatorType2_ByteLength),
            verifyCleanness: false,
            binaryWriterAction: (w, v) =>
            ConvertToBytes_System_Collections_Generic_List_NonLazinatorClass(w, MyListNonLazinatorType2,
            includeChildrenMode, v));
        }
        
        /* Conversion of supported collections and tuples */
        
        private static System.Collections.Generic.List<LazinatorTests.Examples.NonLazinatorClass> ConvertFromBytes_System_Collections_Generic_List_NonLazinatorClass(ReadOnlyMemory<byte> storage, DeserializationFactory deserializationFactory, InformParentOfDirtinessDelegate informParentOfDirtinessDelegate)
        {
            if (storage.Length == 0)
            {
                return default(System.Collections.Generic.List<LazinatorTests.Examples.NonLazinatorClass>);
            }
            ReadOnlySpan<byte> span = storage.Span;
            
            int bytesSoFar = 0;
            int collectionLength = span.ToDecompressedInt(ref bytesSoFar);
            
            System.Collections.Generic.List<LazinatorTests.Examples.NonLazinatorClass> collection = new System.Collections.Generic.List<LazinatorTests.Examples.NonLazinatorClass>(collectionLength);
            for (int i = 0; i < collectionLength; i++)
            {
                int lengthCollectionMember = span.ToInt32(ref bytesSoFar);
                if (lengthCollectionMember == 0)
                {
                    collection.Add(default(LazinatorTests.Examples.NonLazinatorClass));
                }
                else
                {
                    ReadOnlyMemory<byte> childData = storage.Slice(bytesSoFar, lengthCollectionMember);
                    var item = LazinatorTests.Examples.NonLazinatorDirectConverter.ConvertFromBytes_LazinatorTests_Examples_NonLazinatorClass(childData, deserializationFactory, informParentOfDirtinessDelegate);
                    collection.Add(item);
                }
                bytesSoFar += lengthCollectionMember;
            }
            
            return collection;
        }
        
        private static void ConvertToBytes_System_Collections_Generic_List_NonLazinatorClass(BinaryBufferWriter writer, System.Collections.Generic.List<LazinatorTests.Examples.NonLazinatorClass> itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
            if (itemToConvert == default(System.Collections.Generic.List<LazinatorTests.Examples.NonLazinatorClass>))
            {
                return;
            }
            CompressedIntegralTypes.WriteCompressedInt(writer, itemToConvert.Count);
            int itemToConvertCount = itemToConvert.Count;
            for (int itemIndex = 0; itemIndex < itemToConvertCount; itemIndex++)
            {
                if (itemToConvert[itemIndex] == default(LazinatorTests.Examples.NonLazinatorClass))
                {
                    writer.Write((uint)0);
                }
                else 
                {
                    
                    void action(BinaryBufferWriter w) => LazinatorTests.Examples.NonLazinatorDirectConverter.ConvertToBytes_LazinatorTests_Examples_NonLazinatorClass(writer, itemToConvert[itemIndex], includeChildrenMode, verifyCleanness);
                    WriteToBinaryWithIntLengthPrefix(writer, action);
                }
                
            }
        }
        
    }
}
