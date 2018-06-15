//786b0a83-fb16-2670-95a4-f86ac40a3a27
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.117
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
    using System;
    using System.Buffers;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.InteropServices;
    using static Lazinator.Core.LazinatorUtilities;
    
    [Autogenerated]
    public partial struct ExampleStruct : ILazinator
    {
        /* Serialization, deserialization, and object relationships */
        
        ILazinator _LazinatorParentClass;
        public ILazinator LazinatorParentClass 
        { 
            get => _LazinatorParentClass;
            set
            {
                _LazinatorParentClass = value;
                if (value != null && (IsDirty || DescendantIsDirty))
                {
                    value.DescendantIsDirty = true;
                }
            }
        }
        
        IncludeChildrenMode OriginalIncludeChildrenMode;
        
        public int Deserialize()
        {
            ResetAccessedProperties();
            int bytesSoFar = 0;
            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
            if (span.Length == 0)
            {
                return 0;
            }
            
            if (LazinatorParentClass != null)
            {
                throw new LazinatorDeserializationException("A Lazinator struct may include a Lazinator class or interface as a property only when the Lazinator struct has no parent class.");
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
        
        public MemoryInBuffer SerializeNewBuffer(IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
            return EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, true, verifyCleanness, IsDirty, DescendantIsDirty, false, LazinatorObjectBytes, (StreamManuallyDelegate) EncodeToNewBuffer);
        }
        
        MemoryInBuffer EncodeToNewBuffer(IncludeChildrenMode includeChildrenMode, bool verifyCleanness) => LazinatorUtilities.EncodeToNewBinaryBufferWriter(this, includeChildrenMode, verifyCleanness);
        
        public ILazinator CloneLazinator()
        {
            return CloneLazinator(OriginalIncludeChildrenMode);
        }
        
        public ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode)
        {
            MemoryInBuffer bytes = EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, false, false, IsDirty, DescendantIsDirty, false, LazinatorObjectBytes, (StreamManuallyDelegate)EncodeToNewBuffer);
            var clone = new ExampleStruct()
            {
                LazinatorParentClass = LazinatorParentClass,
                OriginalIncludeChildrenMode = includeChildrenMode,
                HierarchyBytes = bytes,
            };
            clone.LazinatorParentClass = null;
            return clone;
        }
        
        bool _IsDirty;
        public bool IsDirty
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
        
        public InformParentOfDirtinessDelegate InformParentOfDirtinessDelegate { get; set; }
        public void InformParentOfDirtiness()
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
        
        bool _DescendantIsDirty;
        public bool DescendantIsDirty
        {
            [DebuggerStepThrough]
            get => _DescendantIsDirty || (_MyChild1_Accessed && MyChild1 != null && (MyChild1.IsDirty || MyChild1.DescendantIsDirty)) || (_MyChild2_Accessed && MyChild2 != null && (MyChild2.IsDirty || MyChild2.DescendantIsDirty));
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
        
        private MemoryInBuffer _HierarchyBytes;
        public MemoryInBuffer HierarchyBytes
        {
            set
            {
                _HierarchyBytes = value;
                LazinatorObjectBytes = value.FilledMemory;
            }
        }
        
        ReadOnlyMemory<byte> _LazinatorObjectBytes;
        public ReadOnlyMemory<byte> LazinatorObjectBytes
        {
            get => _LazinatorObjectBytes;
            set
            {
                _LazinatorObjectBytes = value;
                int length = Deserialize();
                _LazinatorObjectBytes = _LazinatorObjectBytes.Slice(0, length);
            }
        }
        
        public void LazinatorConvertToBytes()
        {
            if (!IsDirty && !DescendantIsDirty && _LazinatorObjectBytes.Length > 0)
            {
                return;
            }
            MemoryInBuffer bytes = EncodeOrRecycleToNewBuffer(IncludeChildrenMode.IncludeAllChildren, OriginalIncludeChildrenMode, false, false, IsDirty, DescendantIsDirty, false, LazinatorObjectBytes, (StreamManuallyDelegate)EncodeToNewBuffer);
            _LazinatorObjectBytes = bytes.FilledMemory;
        }
        
        public int GetByteLength()
        {
            LazinatorConvertToBytes();
            return _LazinatorObjectBytes.Length;
        }
        
        public uint GetBinaryHashCode32()
        {
            LazinatorConvertToBytes();
            return FarmhashByteSpans.Hash32(LazinatorObjectBytes.Span);
        }
        
        public ulong GetBinaryHashCode64()
        {
            LazinatorConvertToBytes();
            return FarmhashByteSpans.Hash64(LazinatorObjectBytes.Span);
        }
        
        public Guid GetBinaryHashCode128()
        {
            LazinatorConvertToBytes();
            return FarmhashByteSpans.Hash128(LazinatorObjectBytes.Span);
        }
        
        /* Property definitions */
        
        int _MyChild1_ByteIndex;
        int _MyChild2_ByteIndex;
        int _MyLazinatorList_ByteIndex;
        int _MyListValues_ByteIndex;
        int _MyTuple_ByteIndex;
        int _MyChild1_ByteLength => _MyChild2_ByteIndex - _MyChild1_ByteIndex;
        int _MyChild2_ByteLength => _MyLazinatorList_ByteIndex - _MyChild2_ByteIndex;
        int _MyLazinatorList_ByteLength => _MyListValues_ByteIndex - _MyLazinatorList_ByteIndex;
        int _MyListValues_ByteLength => _MyTuple_ByteIndex - _MyListValues_ByteIndex;
        private int _ExampleStruct_EndByteIndex;
        int _MyTuple_ByteLength => _ExampleStruct_EndByteIndex - _MyTuple_ByteIndex;
        
        private bool _MyBool;
        public bool MyBool
        {
            get
            {
                return _MyBool;
            }
            set
            {
                IsDirty = true;
                _MyBool = value;
            }
        }
        private char _MyChar;
        public char MyChar
        {
            get
            {
                return _MyChar;
            }
            set
            {
                IsDirty = true;
                _MyChar = value;
            }
        }
        private ExampleChild _MyChild1;
        public ExampleChild MyChild1
        {
            get
            {
                if (!_MyChild1_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _MyChild1 = default(ExampleChild);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _MyChild1_ByteIndex, _MyChild1_ByteLength, false, false, null);
                        
                        _MyChild1 = DeserializationFactory.Instance.CreateBaseOrDerivedType(213, () => new ExampleChild(), childData); 
                    }
                    _MyChild1_Accessed = true;
                }
                return _MyChild1;
            }
            set
            {
                if (value != null)
                {
                    if (value.LazinatorParentClass != null)
                    {
                        throw new MovedLazinatorException($"The property MyChild1 cannot be set to a Lazinator object with a defined LazinatorParentClass, because AutoChangeParent is set to false in the configuration file and no attribute providing an exception is present.");
                    }
                    value.LazinatorParentClass = null;
                }
                IsDirty = true;
                _MyChild1 = value;
                if (_MyChild1 != null)
                {
                    _MyChild1.IsDirty = true;
                }
                _MyChild1_Accessed = true;
            }
        }
        bool _MyChild1_Accessed;
        private ExampleChild _MyChild2;
        public ExampleChild MyChild2
        {
            get
            {
                if (!_MyChild2_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _MyChild2 = default(ExampleChild);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _MyChild2_ByteIndex, _MyChild2_ByteLength, false, false, null);
                        
                        _MyChild2 = DeserializationFactory.Instance.CreateBaseOrDerivedType(213, () => new ExampleChild(), childData); 
                    }
                    _MyChild2_Accessed = true;
                }
                return _MyChild2;
            }
            set
            {
                if (value != null)
                {
                    if (value.LazinatorParentClass != null)
                    {
                        throw new MovedLazinatorException($"The property MyChild2 cannot be set to a Lazinator object with a defined LazinatorParentClass, because AutoChangeParent is set to false in the configuration file and no attribute providing an exception is present.");
                    }
                    value.LazinatorParentClass = null;
                }
                IsDirty = true;
                _MyChild2 = value;
                if (_MyChild2 != null)
                {
                    _MyChild2.IsDirty = true;
                }
                _MyChild2_Accessed = true;
            }
        }
        bool _MyChild2_Accessed;
        private List<Example> _MyLazinatorList;
        public List<Example> MyLazinatorList
        {
            get
            {
                if (!_MyLazinatorList_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _MyLazinatorList = default(List<Example>);
                        _MyLazinatorList_Dirty = true;
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _MyLazinatorList_ByteIndex, _MyLazinatorList_ByteLength, false, false, null);
                        _MyLazinatorList = ConvertFromBytes_List_GExample_g(childData, null);
                    }
                    _MyLazinatorList_Accessed = true;
                }
                return _MyLazinatorList;
            }
            set
            {
                IsDirty = true;
                _MyLazinatorList = value;
                _MyLazinatorList_Dirty = true;
                _MyLazinatorList_Accessed = true;
            }
        }
        bool _MyLazinatorList_Accessed;
        
        private bool _MyLazinatorList_Dirty;
        public bool MyLazinatorList_Dirty
        {
            get => _MyLazinatorList_Dirty;
            set
            {
                if (_MyLazinatorList_Dirty != value)
                {
                    _MyLazinatorList_Dirty = value;
                    if (value && !IsDirty)
                    {
                        IsDirty = true;
                    }
                }
            }
        }
        private List<int> _MyListValues;
        public List<int> MyListValues
        {
            get
            {
                if (!_MyListValues_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _MyListValues = default(List<int>);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _MyListValues_ByteIndex, _MyListValues_ByteLength, false, false, null);
                        _MyListValues = ConvertFromBytes_List_Gint_g(childData, null);
                    }
                    _MyListValues_Accessed = true;
                }
                IsDirty = true;
                return _MyListValues;
            }
            set
            {
                IsDirty = true;
                _MyListValues = value;
                _MyListValues_Accessed = true;
            }
        }
        bool _MyListValues_Accessed;
        private (NonLazinatorClass myitem1, int? myitem2) _MyTuple;
        public (NonLazinatorClass myitem1, int? myitem2) MyTuple
        {
            get
            {
                if (!_MyTuple_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _MyTuple = default((NonLazinatorClass myitem1, int? myitem2));
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _MyTuple_ByteIndex, _MyTuple_ByteLength, false, false, null);
                        _MyTuple = ConvertFromBytes__PNonLazinatorClass_C32myitem1_c_C32int_C63_C32myitem2_p(childData, null);
                    }
                    _MyTuple_Accessed = true;
                }
                IsDirty = true;
                return _MyTuple;
            }
            set
            {
                IsDirty = true;
                _MyTuple = value;
                _MyTuple_Accessed = true;
            }
        }
        bool _MyTuple_Accessed;
        
        void ResetAccessedProperties()
        {
            _MyChild1_Accessed = _MyChild2_Accessed = _MyLazinatorList_Accessed = _MyListValues_Accessed = _MyTuple_Accessed = false;
        }
        
        /* Conversion */
        
        public int LazinatorUniqueID => 216;
        
        bool ContainsOpenGenericParameters => false;
        public System.Collections.Generic.List<int> LazinatorGenericID
        {
            get => null;
            set { }
        }
        
        private bool _LazinatorObjectVersionChanged;
        private int _LazinatorObjectVersionOverride;
        public int LazinatorObjectVersion
        {
            get => _LazinatorObjectVersionChanged ? _LazinatorObjectVersionOverride : 0;
            set
            {
                _LazinatorObjectVersionOverride = value;
                _LazinatorObjectVersionChanged = true;
            }
        }
        
        
        public void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
            _MyBool = span.ToBoolean(ref bytesSoFar);
            _MyChar = span.ToChar(ref bytesSoFar);
            _MyChild1_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            _MyChild2_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            _MyLazinatorList_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            _MyListValues_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            _MyTuple_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            _ExampleStruct_EndByteIndex = bytesSoFar;
        }
        
        public void SerializeExistingBuffer(BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
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
                _DescendantIsDirty = includeChildrenMode != IncludeChildrenMode.IncludeAllChildren && ((_MyChild1_Accessed && MyChild1 != null && (MyChild1.IsDirty || MyChild1.DescendantIsDirty)) || (_MyChild2_Accessed && MyChild2 != null && (MyChild2.IsDirty || MyChild2.DescendantIsDirty)));
                
                _LazinatorObjectBytes = writer.Slice(startPosition);
            }
        }
        void WritePropertiesIntoBuffer(BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer, bool includeUniqueID)
        {
            int startPosition = writer.Position;
            int startOfObjectPosition = 0;
            // header information
            if (includeUniqueID)
            {
                CompressedIntegralTypes.WriteCompressedInt(writer, LazinatorUniqueID);
            }
            
            CompressedIntegralTypes.WriteCompressedInt(writer, Lazinator.Support.LazinatorVersionInfo.LazinatorIntVersion);
            CompressedIntegralTypes.WriteCompressedInt(writer, LazinatorObjectVersion);
            writer.Write((byte)includeChildrenMode);
            // write properties
            WriteUncompressedPrimitives.WriteBool(writer, _MyBool);
            EncodeCharAndString.WriteCharInTwoBytes(writer, _MyChar);
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)  
            {
                var serializedBytesCopy = LazinatorObjectBytes;
                var byteIndexCopy = _MyChild1_ByteIndex;
                var byteLengthCopy = _MyChild1_ByteLength;
                WriteChild(writer, _MyChild1, includeChildrenMode, _MyChild1_Accessed, () => GetChildSlice(serializedBytesCopy, byteIndexCopy, byteLengthCopy, false, false, null), verifyCleanness, updateStoredBuffer, false, false, null);
            }
            if (updateStoredBuffer)
            {
                _MyChild1_ByteIndex = startOfObjectPosition - startPosition;
            }
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)  
            {
                var serializedBytesCopy = LazinatorObjectBytes;
                var byteIndexCopy = _MyChild2_ByteIndex;
                var byteLengthCopy = _MyChild2_ByteLength;
                WriteChild(writer, _MyChild2, includeChildrenMode, _MyChild2_Accessed, () => GetChildSlice(serializedBytesCopy, byteIndexCopy, byteLengthCopy, false, false, null), verifyCleanness, updateStoredBuffer, false, false, null);
            }
            if (updateStoredBuffer)
            {
                _MyChild2_ByteIndex = startOfObjectPosition - startPosition;
            }
            startOfObjectPosition = writer.Position;
            var serializedBytesCopy_MyLazinatorList = LazinatorObjectBytes;
            var byteIndexCopy_MyLazinatorList = _MyLazinatorList_ByteIndex;
            var byteLengthCopy_MyLazinatorList = _MyLazinatorList_ByteLength;
            var copy_MyLazinatorList = MyLazinatorList;
            WriteNonLazinatorObject(
            nonLazinatorObject: _MyLazinatorList, isBelievedDirty: MyLazinatorList_Dirty,
            isAccessed: _MyLazinatorList_Accessed, writer: writer,
            getChildSliceForFieldFn: () => GetChildSlice(serializedBytesCopy_MyLazinatorList, byteIndexCopy_MyLazinatorList, byteLengthCopy_MyLazinatorList, false, false, null),
            verifyCleanness: verifyCleanness,
            binaryWriterAction: (w, v) =>
            ConvertToBytes_List_GExample_g(w, copy_MyLazinatorList, includeChildrenMode, v, updateStoredBuffer));
            if (updateStoredBuffer)
            {
                _MyLazinatorList_ByteIndex = startOfObjectPosition - startPosition;
            }
            startOfObjectPosition = writer.Position;
            var serializedBytesCopy_MyListValues = LazinatorObjectBytes;
            var byteIndexCopy_MyListValues = _MyListValues_ByteIndex;
            var byteLengthCopy_MyListValues = _MyListValues_ByteLength;
            var copy_MyListValues = MyListValues;
            WriteNonLazinatorObject(
            nonLazinatorObject: _MyListValues, isBelievedDirty: _MyListValues_Accessed,
            isAccessed: _MyListValues_Accessed, writer: writer,
            getChildSliceForFieldFn: () => GetChildSlice(serializedBytesCopy_MyListValues, byteIndexCopy_MyListValues, byteLengthCopy_MyListValues, false, false, null),
            verifyCleanness: false,
            binaryWriterAction: (w, v) =>
            ConvertToBytes_List_Gint_g(w, copy_MyListValues, includeChildrenMode, v, updateStoredBuffer));
            if (updateStoredBuffer)
            {
                _MyListValues_ByteIndex = startOfObjectPosition - startPosition;
            }
            startOfObjectPosition = writer.Position;
            var serializedBytesCopy_MyTuple = LazinatorObjectBytes;
            var byteIndexCopy_MyTuple = _MyTuple_ByteIndex;
            var byteLengthCopy_MyTuple = _MyTuple_ByteLength;
            var copy_MyTuple = MyTuple;
            WriteNonLazinatorObject(
            nonLazinatorObject: _MyTuple, isBelievedDirty: _MyTuple_Accessed,
            isAccessed: _MyTuple_Accessed, writer: writer,
            getChildSliceForFieldFn: () => GetChildSlice(serializedBytesCopy_MyTuple, byteIndexCopy_MyTuple, byteLengthCopy_MyTuple, false, false, null),
            verifyCleanness: false,
            binaryWriterAction: (w, v) =>
            ConvertToBytes__PNonLazinatorClass_C32myitem1_c_C32int_C63_C32myitem2_p(w, copy_MyTuple, includeChildrenMode, v, updateStoredBuffer));
            if (updateStoredBuffer)
            {
                _MyTuple_ByteIndex = startOfObjectPosition - startPosition;
            }
            if (updateStoredBuffer)
            {
                _ExampleStruct_EndByteIndex = writer.Position - startPosition;
            }
        }
        
        /* Conversion of supported collections and tuples */
        
        private static List<Example> ConvertFromBytes_List_GExample_g(ReadOnlyMemory<byte> storage, InformParentOfDirtinessDelegate informParentOfDirtinessDelegate)
        {
            if (storage.Length == 0)
            {
                return default(List<Example>);
            }
            ReadOnlySpan<byte> span = storage.Span;
            
            int bytesSoFar = 0;
            int collectionLength = span.ToDecompressedInt(ref bytesSoFar);
            
            List<Example> collection = new List<Example>(collectionLength);
            for (int i = 0; i < collectionLength; i++)
            {
                int lengthCollectionMember = span.ToInt32(ref bytesSoFar);
                if (lengthCollectionMember == 0)
                {
                    collection.Add(default(Example));
                }
                else
                {
                    ReadOnlyMemory<byte> childData = storage.Slice(bytesSoFar, lengthCollectionMember);
                    var item = DeserializationFactory.Instance.CreateBasedOnTypeSpecifyingDelegate<Example>(childData, informParentOfDirtinessDelegate);
                    collection.Add(item);
                }
                bytesSoFar += lengthCollectionMember;
            }
            
            return collection;
        }
        
        private static void ConvertToBytes_List_GExample_g(BinaryBufferWriter writer, List<Example> itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            if (itemToConvert == default(List<Example>))
            {
                return;
            }
            CompressedIntegralTypes.WriteCompressedInt(writer, itemToConvert.Count);
            int itemToConvertCount = itemToConvert.Count;
            for (int itemIndex = 0; itemIndex < itemToConvertCount; itemIndex++)
            {
                if (itemToConvert[itemIndex] == default(Example))
                {
                    writer.Write((uint)0);
                }
                else 
                {
                    
                    void action(BinaryBufferWriter w) => itemToConvert[itemIndex].SerializeExistingBuffer(writer, includeChildrenMode, verifyCleanness, updateStoredBuffer);
                    WriteToBinaryWithIntLengthPrefix(writer, action);
                }
                
            }
        }
        
        private static List<int> ConvertFromBytes_List_Gint_g(ReadOnlyMemory<byte> storage, InformParentOfDirtinessDelegate informParentOfDirtinessDelegate)
        {
            if (storage.Length == 0)
            {
                return default(List<int>);
            }
            ReadOnlySpan<byte> span = storage.Span;
            
            int bytesSoFar = 0;
            int collectionLength = span.ToDecompressedInt(ref bytesSoFar);
            
            List<int> collection = new List<int>(collectionLength);
            for (int i = 0; i < collectionLength; i++)
            {
                int item = span.ToDecompressedInt(ref bytesSoFar);
                collection.Add(item);
            }
            
            return collection;
        }
        
        private static void ConvertToBytes_List_Gint_g(BinaryBufferWriter writer, List<int> itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            if (itemToConvert == default(List<int>))
            {
                return;
            }
            CompressedIntegralTypes.WriteCompressedInt(writer, itemToConvert.Count);
            int itemToConvertCount = itemToConvert.Count;
            for (int itemIndex = 0; itemIndex < itemToConvertCount; itemIndex++)
            {
                CompressedIntegralTypes.WriteCompressedInt(writer, itemToConvert[itemIndex]);
            }
        }
        
        private static (NonLazinatorClass myitem1, int? myitem2) ConvertFromBytes__PNonLazinatorClass_C32myitem1_c_C32int_C63_C32myitem2_p(ReadOnlyMemory<byte> storage, InformParentOfDirtinessDelegate informParentOfDirtinessDelegate)
        {
            if (storage.Length == 0)
            {
                return default;
            }
            ReadOnlySpan<byte> span = storage.Span;
            
            int bytesSoFar = 0;
            
            NonLazinatorClass item1 = default;
            int lengthCollectionMember_item1 = span.ToInt32(ref bytesSoFar);
            if (lengthCollectionMember_item1 != 0)
            {
                ReadOnlyMemory<byte> childData = storage.Slice(bytesSoFar, lengthCollectionMember_item1);
                item1 = NonLazinatorDirectConverter.ConvertFromBytes_NonLazinatorClass(childData, informParentOfDirtinessDelegate);
            }
            bytesSoFar += lengthCollectionMember_item1;
            
            int? item2 = span.ToDecompressedNullableInt(ref bytesSoFar);
            
            var tupleType = (item1, item2);
            
            return tupleType;
        }
        
        private static void ConvertToBytes__PNonLazinatorClass_C32myitem1_c_C32int_C63_C32myitem2_p(BinaryBufferWriter writer, (NonLazinatorClass myitem1, int? myitem2) itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            
            if (itemToConvert.Item1 == null)
            {
                writer.Write((uint)0);
            }
            else
            {
                void actionItem1(BinaryBufferWriter w) => NonLazinatorDirectConverter.ConvertToBytes_NonLazinatorClass(writer, itemToConvert.Item1, includeChildrenMode, verifyCleanness, updateStoredBuffer);
                WriteToBinaryWithIntLengthPrefix(writer, actionItem1);
            }
            
            CompressedIntegralTypes.WriteCompressedNullableInt(writer, itemToConvert.Item2);
        }
        
    }
}
