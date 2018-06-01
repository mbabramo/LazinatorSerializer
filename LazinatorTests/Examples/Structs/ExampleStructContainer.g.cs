//bf05690c-a851-1e16-e4e5-10d5d461322d
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.70
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LazinatorTests.Examples
{
    using Lazinator.Attributes;
    using Lazinator.Buffers;
    using Lazinator.Core;
    using Lazinator.Exceptions;
    using Lazinator.Support;
    using Lazinator.Wrappers;
    using System;
    using System.Buffers;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.InteropServices;
    using static Lazinator.Core.LazinatorUtilities;
    
    [Autogenerated]
    public partial class ExampleStructContainer : ILazinator
    {
        /* Serialization, deserialization, and object relationships */
        
        public ExampleStructContainer() : base()
        {
        }
        
        public virtual ILazinator LazinatorParentClass { get; set; }
        
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
            
            int uniqueID = span.ToDecompressedInt(ref bytesSoFar);
            if (uniqueID != LazinatorUniqueID)
            {
                throw new FormatException("Wrong self-serialized type initialized.");
            }
            
            int lazinatorLibraryVersion = span.ToDecompressedInt(ref bytesSoFar);
            
            int serializedVersionNumber = span.ToDecompressedInt(ref bytesSoFar);
            
            OriginalIncludeChildrenMode = (IncludeChildrenMode)span.ToByte(ref bytesSoFar);
            
            ConvertFromBytesAfterHeader(OriginalIncludeChildrenMode, serializedVersionNumber, ref bytesSoFar);
            return bytesSoFar;
        }
        
        public virtual MemoryInBuffer SerializeNewBuffer(IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
            return EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, true, verifyCleanness, IsDirty, DescendantIsDirty, false, LazinatorObjectBytes, (StreamManuallyDelegate) EncodeToNewBuffer);
        }
        
        protected virtual MemoryInBuffer EncodeToNewBuffer(IncludeChildrenMode includeChildrenMode, bool verifyCleanness) => LazinatorUtilities.EncodeToNewBinaryBufferWriter(this, includeChildrenMode, verifyCleanness);
        
        public virtual ILazinator CloneLazinator()
        {
            return CloneLazinator(OriginalIncludeChildrenMode);
        }
        
        public virtual ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode)
        {
            MemoryInBuffer bytes = EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, false, false, IsDirty, DescendantIsDirty, false, LazinatorObjectBytes, (StreamManuallyDelegate)EncodeToNewBuffer);
            var clone = new ExampleStructContainer()
            {
                DeserializationFactory = DeserializationFactory,
                LazinatorParentClass = LazinatorParentClass,
                InformParentOfDirtinessDelegate = InformParentOfDirtinessDelegate,
                OriginalIncludeChildrenMode = includeChildrenMode,
                HierarchyBytes = bytes
            };
            return clone;
        }
        
        protected bool _IsDirty;
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
            {
                InformParentOfDirtinessDelegate();
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
                    if (_DescendantIsDirty && LazinatorParentClass != null)
                    {
                        LazinatorParentClass.DescendantIsDirty = true;
                    }
                }
            }
        }
        
        public virtual void MarkHierarchyClean()
        {
            _IsDirty = false;
            _DescendantIsDirty = false;
            if (_IntWrapper_Accessed)
            {
                IntWrapper.MarkHierarchyClean();
            }
            if (_MyExampleStruct_Accessed)
            {
                MyExampleStruct.MarkHierarchyClean();
            }
        }
        
        public virtual DeserializationFactory DeserializationFactory { get; set; }
        
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
            if (!IsDirty && !DescendantIsDirty)
            {
                return;
            }
            MemoryInBuffer bytes = EncodeOrRecycleToNewBuffer(IncludeChildrenMode.IncludeAllChildren, OriginalIncludeChildrenMode, false, false, IsDirty, DescendantIsDirty, false, LazinatorObjectBytes, (StreamManuallyDelegate)EncodeToNewBuffer);
            LazinatorObjectBytes = bytes.FilledMemory;
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
        
        /* Field definitions */
        
        protected int _IntWrapper_ByteIndex;
        protected int _MyExampleStruct_ByteIndex;
        protected int _MyListExampleStruct_ByteIndex;
        protected int _MyListNullableExampleStruct_ByteIndex;
        protected virtual int _IntWrapper_ByteLength => _MyExampleStruct_ByteIndex - _IntWrapper_ByteIndex;
        protected virtual int _MyExampleStruct_ByteLength => _MyListExampleStruct_ByteIndex - _MyExampleStruct_ByteIndex;
        protected virtual int _MyListExampleStruct_ByteLength => _MyListNullableExampleStruct_ByteIndex - _MyListExampleStruct_ByteIndex;
        private int _ExampleStructContainer_EndByteIndex;
        protected virtual int _MyListNullableExampleStruct_ByteLength => _ExampleStructContainer_EndByteIndex - _MyListNullableExampleStruct_ByteIndex;
        
        private LazinatorWrapperInt _IntWrapper;
        public LazinatorWrapperInt IntWrapper
        {
            [DebuggerStepThrough]
            get
            {
                if (!_IntWrapper_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _IntWrapper = default(LazinatorWrapperInt);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _IntWrapper_ByteIndex, _IntWrapper_ByteLength, true);
                        _IntWrapper = new LazinatorWrapperInt()
                        {
                            DeserializationFactory = DeserializationFactory,
                            LazinatorParentClass = this,
                            LazinatorObjectBytes = childData,
                        };
                    }
                    _IntWrapper_Accessed = true;
                }
                return _IntWrapper;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _IntWrapper = value;
                _IntWrapper_Accessed = true;
            }
        }
        protected bool _IntWrapper_Accessed;
        public LazinatorWrapperInt IntWrapper_Copy
        {
            [DebuggerStepThrough]
            get
            {
                if (!_IntWrapper_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        return default(LazinatorWrapperInt);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _IntWrapper_ByteIndex, _IntWrapper_ByteLength);
                        return new LazinatorWrapperInt()
                        {
                            DeserializationFactory = DeserializationFactory,
                            LazinatorObjectBytes = childData,
                        };
                    }
                }
                return _IntWrapper;
            }
        }
        private ExampleStruct _MyExampleStruct;
        public ExampleStruct MyExampleStruct
        {
            [DebuggerStepThrough]
            get
            {
                if (!_MyExampleStruct_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _MyExampleStruct = default(ExampleStruct);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _MyExampleStruct_ByteIndex, _MyExampleStruct_ByteLength);
                        _MyExampleStruct = new ExampleStruct()
                        {
                            DeserializationFactory = DeserializationFactory,
                            LazinatorParentClass = this,
                            LazinatorObjectBytes = childData,
                        };
                    }
                    _MyExampleStruct_Accessed = true;
                }
                return _MyExampleStruct;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _MyExampleStruct = value;
                _MyExampleStruct_Accessed = true;
            }
        }
        protected bool _MyExampleStruct_Accessed;
        public ExampleStruct MyExampleStruct_Copy
        {
            [DebuggerStepThrough]
            get
            {
                if (!_MyExampleStruct_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        return default(ExampleStruct);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _MyExampleStruct_ByteIndex, _MyExampleStruct_ByteLength);
                        return new ExampleStruct()
                        {
                            DeserializationFactory = DeserializationFactory,
                            LazinatorObjectBytes = childData,
                        };
                    }
                }
                return _MyExampleStruct;
            }
        }
        private List<ExampleStruct> _MyListExampleStruct;
        public List<ExampleStruct> MyListExampleStruct
        {
            [DebuggerStepThrough]
            get
            {
                if (!_MyListExampleStruct_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _MyListExampleStruct = default(List<ExampleStruct>);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _MyListExampleStruct_ByteIndex, _MyListExampleStruct_ByteLength);
                        _MyListExampleStruct = ConvertFromBytes_List_GExampleStruct_g(childData, DeserializationFactory, null);
                    }
                    _MyListExampleStruct_Accessed = true;
                    IsDirty = true;
                }
                return _MyListExampleStruct;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _MyListExampleStruct = value;
                _MyListExampleStruct_Accessed = true;
            }
        }
        protected bool _MyListExampleStruct_Accessed;
        private List<LazinatorWrapperNullableStruct<ExampleStruct>> _MyListNullableExampleStruct;
        public List<LazinatorWrapperNullableStruct<ExampleStruct>> MyListNullableExampleStruct
        {
            [DebuggerStepThrough]
            get
            {
                if (!_MyListNullableExampleStruct_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _MyListNullableExampleStruct = default(List<LazinatorWrapperNullableStruct<ExampleStruct>>);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _MyListNullableExampleStruct_ByteIndex, _MyListNullableExampleStruct_ByteLength);
                        _MyListNullableExampleStruct = ConvertFromBytes_List_GLazinatorWrapperNullableStruct_GExampleStruct_g_g(childData, DeserializationFactory, null);
                    }
                    _MyListNullableExampleStruct_Accessed = true;
                    IsDirty = true;
                }
                return _MyListNullableExampleStruct;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _MyListNullableExampleStruct = value;
                _MyListNullableExampleStruct_Accessed = true;
            }
        }
        protected bool _MyListNullableExampleStruct_Accessed;
        
        protected virtual void ResetAccessedProperties()
        {
            _IntWrapper_Accessed = _MyExampleStruct_Accessed = _MyListExampleStruct_Accessed = _MyListNullableExampleStruct_Accessed = false;
        }
        
        /* Conversion */
        
        public virtual int LazinatorUniqueID => 217;
        
        public virtual int LazinatorObjectVersion { get; set; } = 0;
        
        public virtual void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
            _IntWrapper_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                bytesSoFar = span.ToByte(ref bytesSoFar) + bytesSoFar;
            }
            _MyExampleStruct_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            _MyListExampleStruct_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            _MyListNullableExampleStruct_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            _ExampleStructContainer_EndByteIndex = bytesSoFar;
        }
        
        public virtual void SerializeExistingBuffer(BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
            int startPosition = writer.Position;
            WritePropertiesIntoBuffer(writer, includeChildrenMode, verifyCleanness);
            
            _IsDirty = false;
            _DescendantIsDirty = includeChildrenMode != IncludeChildrenMode.IncludeAllChildren && ((_IntWrapper_Accessed && (IntWrapper.IsDirty || IntWrapper.DescendantIsDirty)) || (_MyExampleStruct_Accessed && (MyExampleStruct.IsDirty || MyExampleStruct.DescendantIsDirty)));
            
            _LazinatorObjectBytes = writer.Slice(startPosition);
        }
        protected virtual void WritePropertiesIntoBuffer(BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
            // header information
            CompressedIntegralTypes.WriteCompressedInt(writer, LazinatorUniqueID);
            CompressedIntegralTypes.WriteCompressedInt(writer, Lazinator.Support.LazinatorVersionInfo.LazinatorIntVersion);
            CompressedIntegralTypes.WriteCompressedInt(writer, LazinatorObjectVersion);
            writer.Write((byte)includeChildrenMode);
            // write properties
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                WriteChildWithLength(writer, _IntWrapper, includeChildrenMode, _IntWrapper_Accessed, () => GetChildSlice(LazinatorObjectBytes, _IntWrapper_ByteIndex, _IntWrapper_ByteLength), verifyCleanness, true, this);
            }
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                WriteChildWithLength(writer, _MyExampleStruct, includeChildrenMode, _MyExampleStruct_Accessed, () => GetChildSlice(LazinatorObjectBytes, _MyExampleStruct_ByteIndex, _MyExampleStruct_ByteLength), verifyCleanness, false, this);
            }
            WriteNonLazinatorObject(
            nonLazinatorObject: _MyListExampleStruct, isBelievedDirty: _MyListExampleStruct_Accessed,
            isAccessed: _MyListExampleStruct_Accessed, writer: writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorObjectBytes, _MyListExampleStruct_ByteIndex, _MyListExampleStruct_ByteLength),
            verifyCleanness: false,
            binaryWriterAction: (w, v) =>
            ConvertToBytes_List_GExampleStruct_g(w, MyListExampleStruct,
            includeChildrenMode, v));
            WriteNonLazinatorObject(
            nonLazinatorObject: _MyListNullableExampleStruct, isBelievedDirty: _MyListNullableExampleStruct_Accessed,
            isAccessed: _MyListNullableExampleStruct_Accessed, writer: writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorObjectBytes, _MyListNullableExampleStruct_ByteIndex, _MyListNullableExampleStruct_ByteLength),
            verifyCleanness: false,
            binaryWriterAction: (w, v) =>
            ConvertToBytes_List_GLazinatorWrapperNullableStruct_GExampleStruct_g_g(w, MyListNullableExampleStruct,
            includeChildrenMode, v));
        }
        
        /* Conversion of supported collections and tuples */
        
        private static List<ExampleStruct> ConvertFromBytes_List_GExampleStruct_g(ReadOnlyMemory<byte> storage, DeserializationFactory deserializationFactory, InformParentOfDirtinessDelegate informParentOfDirtinessDelegate)
        {
            if (storage.Length == 0)
            {
                return default(List<ExampleStruct>);
            }
            ReadOnlySpan<byte> span = storage.Span;
            
            int bytesSoFar = 0;
            int collectionLength = span.ToDecompressedInt(ref bytesSoFar);
            
            List<ExampleStruct> collection = new List<ExampleStruct>(collectionLength);
            for (int i = 0; i < collectionLength; i++)
            {
                int lengthCollectionMember = span.ToInt32(ref bytesSoFar);
                ReadOnlyMemory<byte> childData = storage.Slice(bytesSoFar, lengthCollectionMember);
                var item = new ExampleStruct()
                {
                    DeserializationFactory = deserializationFactory,
                    InformParentOfDirtinessDelegate = informParentOfDirtinessDelegate,
                    LazinatorObjectBytes = childData,
                };
                collection.Add(item);
                bytesSoFar += lengthCollectionMember;
            }
            
            return collection;
        }
        
        private static void ConvertToBytes_List_GExampleStruct_g(BinaryBufferWriter writer, List<ExampleStruct> itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
            if (itemToConvert == default(List<ExampleStruct>))
            {
                return;
            }
            CompressedIntegralTypes.WriteCompressedInt(writer, itemToConvert.Count);
            int itemToConvertCount = itemToConvert.Count;
            for (int itemIndex = 0; itemIndex < itemToConvertCount; itemIndex++)
            {
                void action(BinaryBufferWriter w) => itemToConvert[itemIndex].SerializeExistingBuffer(writer, includeChildrenMode, verifyCleanness);
                WriteToBinaryWithIntLengthPrefix(writer, action);
            }
        }
        
        private static List<LazinatorWrapperNullableStruct<ExampleStruct>> ConvertFromBytes_List_GLazinatorWrapperNullableStruct_GExampleStruct_g_g(ReadOnlyMemory<byte> storage, DeserializationFactory deserializationFactory, InformParentOfDirtinessDelegate informParentOfDirtinessDelegate)
        {
            if (storage.Length == 0)
            {
                return default(List<LazinatorWrapperNullableStruct<ExampleStruct>>);
            }
            ReadOnlySpan<byte> span = storage.Span;
            
            int bytesSoFar = 0;
            int collectionLength = span.ToDecompressedInt(ref bytesSoFar);
            
            List<LazinatorWrapperNullableStruct<ExampleStruct>> collection = new List<LazinatorWrapperNullableStruct<ExampleStruct>>(collectionLength);
            for (int i = 0; i < collectionLength; i++)
            {
                int lengthCollectionMember = span.ToInt32(ref bytesSoFar);
                ReadOnlyMemory<byte> childData = storage.Slice(bytesSoFar, lengthCollectionMember);
                var item = new LazinatorWrapperNullableStruct<ExampleStruct>()
                {
                    DeserializationFactory = deserializationFactory,
                    InformParentOfDirtinessDelegate = informParentOfDirtinessDelegate,
                    LazinatorObjectBytes = childData,
                };
                collection.Add(item);
                bytesSoFar += lengthCollectionMember;
            }
            
            return collection;
        }
        
        private static void ConvertToBytes_List_GLazinatorWrapperNullableStruct_GExampleStruct_g_g(BinaryBufferWriter writer, List<LazinatorWrapperNullableStruct<ExampleStruct>> itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
            if (itemToConvert == default(List<LazinatorWrapperNullableStruct<ExampleStruct>>))
            {
                return;
            }
            CompressedIntegralTypes.WriteCompressedInt(writer, itemToConvert.Count);
            int itemToConvertCount = itemToConvert.Count;
            for (int itemIndex = 0; itemIndex < itemToConvertCount; itemIndex++)
            {
                void action(BinaryBufferWriter w) => itemToConvert[itemIndex].SerializeExistingBuffer(writer, includeChildrenMode, verifyCleanness);
                WriteToBinaryWithIntLengthPrefix(writer, action);
            }
        }
        
    }
}
