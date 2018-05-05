//5ed5d1f2-9652-77e3-db5e-74fadbacc518
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool.
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

namespace LazinatorTests.Examples
{
    public partial struct ExampleStruct : ILazinator
    {
        /* Boilerplate for every non-abstract ILazinator object */
        
        public ILazinator LazinatorParentClass { get; set; }
        
        internal IncludeChildrenMode OriginalIncludeChildrenMode;
        
        public void Deserialize()
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
        
        public MemoryInBuffer SerializeNewBuffer(IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
            return EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, true, verifyCleanness, IsDirty, DescendantIsDirty, false, LazinatorObjectBytes, (StreamManuallyDelegate) EncodeToNewBuffer);
        }
        
        internal MemoryInBuffer EncodeToNewBuffer(IncludeChildrenMode includeChildrenMode, bool verifyCleanness) => LazinatorUtilities.EncodeToNewBinaryBufferWriter(this, includeChildrenMode, verifyCleanness);
        
        public ILazinator CloneLazinator()
        {
            return CloneLazinator(OriginalIncludeChildrenMode);
        }
        
        public ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode)
        {
            MemoryInBuffer bytes = EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, false, false, IsDirty, DescendantIsDirty, false, LazinatorObjectBytes, (StreamManuallyDelegate)EncodeToNewBuffer);
            var clone = new ExampleStruct()
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
            InformParentOfDirtinessDelegate();
        }
        
        private bool _DescendantIsDirty;
        public bool DescendantIsDirty
        {
            [DebuggerStepThrough]
            get => _DescendantIsDirty || (MyChild1 != null && (MyChild1.IsDirty || MyChild1.DescendantIsDirty)) || (MyChild2 != null && (MyChild2.IsDirty || MyChild2.DescendantIsDirty));
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
        
        public DeserializationFactory DeserializationFactory { get; set; }
        
        private MemoryInBuffer _HierarchyBytes;
        public MemoryInBuffer HierarchyBytes
        {
            get => _HierarchyBytes;
            set
            {
                _HierarchyBytes = value;
                LazinatorObjectBytes = value.FilledMemory;
            }
        }
        
        private ReadOnlyMemory<byte> _LazinatorObjectBytes;
        public ReadOnlyMemory<byte> LazinatorObjectBytes
        {
            get => _LazinatorObjectBytes;
            set
            {
                _LazinatorObjectBytes = value;
                Deserialize();
            }
        }
        
        public void LazinatorConvertToBytes()
        {
            if (!IsDirty && !DescendantIsDirty)
            {
                return;
            }
            MemoryInBuffer bytes = EncodeOrRecycleToNewBuffer(IncludeChildrenMode.IncludeAllChildren, OriginalIncludeChildrenMode, false, false, IsDirty, DescendantIsDirty, false, LazinatorObjectBytes, (StreamManuallyDelegate)EncodeToNewBuffer);
            _IsDirty = false;
            LazinatorObjectBytes = bytes.FilledMemory;
            _MyChild1_Accessed = _MyChild2_Accessed = false;
        }
        
        public uint GetBinaryHashCode32()
        {
            LazinatorConvertToBytes();
            return Farmhash.Hash32(LazinatorObjectBytes.Span);
        }
        
        public ulong GetBinaryHashCode64()
        {
            LazinatorConvertToBytes();
            return Farmhash.Hash64(LazinatorObjectBytes.Span);
        }
        
        /* Field boilerplate */
        
        internal int _MyChild1_ByteIndex;
        internal int _MyChild2_ByteIndex;
        internal int _MyLazinatorList_ByteIndex;
        internal int _MyListValues_ByteIndex;
        internal int _MyTuple_ByteIndex;
        internal int _MyChild1_ByteLength => _MyChild2_ByteIndex - _MyChild1_ByteIndex;
        internal int _MyChild2_ByteLength => _MyLazinatorList_ByteIndex - _MyChild2_ByteIndex;
        internal int _MyLazinatorList_ByteLength => _MyListValues_ByteIndex - _MyLazinatorList_ByteIndex;
        internal int _MyListValues_ByteLength => _MyTuple_ByteIndex - _MyListValues_ByteIndex;
        internal int _MyTuple_ByteLength => LazinatorObjectBytes.Length - _MyTuple_ByteIndex;
        
        private bool _MyBool;
        public bool MyBool
        {
            [DebuggerStepThrough]
            get
            {
                return _MyBool;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _MyBool = value;
            }
        }
        private char _MyChar;
        public char MyChar
        {
            [DebuggerStepThrough]
            get
            {
                return _MyChar;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _MyChar = value;
            }
        }
        private LazinatorTests.Examples.ExampleChild _MyChild1;
        public LazinatorTests.Examples.ExampleChild MyChild1
        {
            [DebuggerStepThrough]
            get
            {
                if (!_MyChild1_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _MyChild1 = default(LazinatorTests.Examples.ExampleChild);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _MyChild1_ByteIndex, _MyChild1_ByteLength);
                        
                        if (DeserializationFactory == null)
                        {
                            LazinatorDeserializationException.ThrowNoDeserializationFactory();
                        }
                        _MyChild1 = DeserializationFactory.Create(213, () => new LazinatorTests.Examples.ExampleChild(), childData); 
                    }
                    _MyChild1_Accessed = true;
                }
                return _MyChild1;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _MyChild1 = value;
                if (_MyChild1 != null)
                {
                    _MyChild1.IsDirty = true;
                }
                _MyChild1_Accessed = true;
            }
        }
        internal bool _MyChild1_Accessed;
        private LazinatorTests.Examples.ExampleChild _MyChild2;
        public LazinatorTests.Examples.ExampleChild MyChild2
        {
            [DebuggerStepThrough]
            get
            {
                if (!_MyChild2_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _MyChild2 = default(LazinatorTests.Examples.ExampleChild);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _MyChild2_ByteIndex, _MyChild2_ByteLength);
                        
                        if (DeserializationFactory == null)
                        {
                            LazinatorDeserializationException.ThrowNoDeserializationFactory();
                        }
                        _MyChild2 = DeserializationFactory.Create(213, () => new LazinatorTests.Examples.ExampleChild(), childData); 
                    }
                    _MyChild2_Accessed = true;
                }
                return _MyChild2;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _MyChild2 = value;
                if (_MyChild2 != null)
                {
                    _MyChild2.IsDirty = true;
                }
                _MyChild2_Accessed = true;
            }
        }
        internal bool _MyChild2_Accessed;
        private System.Collections.Generic.List<LazinatorTests.Examples.Example> _MyLazinatorList;
        public System.Collections.Generic.List<LazinatorTests.Examples.Example> MyLazinatorList
        {
            [DebuggerStepThrough]
            get
            {
                if (!_MyLazinatorList_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _MyLazinatorList = default(System.Collections.Generic.List<LazinatorTests.Examples.Example>);
                        _MyLazinatorList_Dirty = true;
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _MyLazinatorList_ByteIndex, _MyLazinatorList_ByteLength);
                        _MyLazinatorList = ConvertFromBytes_System_Collections_Generic_List_Example(childData, DeserializationFactory, null);
                    }
                    _MyLazinatorList_Accessed = true;
                }
                return _MyLazinatorList;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _MyLazinatorList = value;
                _MyLazinatorList_Dirty = true;
                _MyLazinatorList_Accessed = true;
            }
        }
        internal bool _MyLazinatorList_Accessed;
        
        private bool _MyLazinatorList_Dirty;
        public bool MyLazinatorList_Dirty
        {
            [DebuggerStepThrough]
            get => _MyLazinatorList_Dirty;
            [DebuggerStepThrough]
            set
            {
                if (_MyLazinatorList_Dirty != value)
                {
                    _MyLazinatorList_Dirty = value;
                    if (value && !IsDirty)
                    IsDirty = true;
                }
            }
        }
        private System.Collections.Generic.List<int> _MyListValues;
        public System.Collections.Generic.List<int> MyListValues
        {
            [DebuggerStepThrough]
            get
            {
                if (!_MyListValues_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _MyListValues = default(System.Collections.Generic.List<int>);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _MyListValues_ByteIndex, _MyListValues_ByteLength);
                        _MyListValues = ConvertFromBytes_System_Collections_Generic_List_int(childData, DeserializationFactory, null);
                    }
                    _MyListValues_Accessed = true;
                    IsDirty = true;
                }
                return _MyListValues;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _MyListValues = value;
                _MyListValues_Accessed = true;
            }
        }
        internal bool _MyListValues_Accessed;
        private ValueTuple<LazinatorTests.Examples.NonLazinatorClass, int?> _MyTuple;
        public ValueTuple<LazinatorTests.Examples.NonLazinatorClass, int?> MyTuple
        {
            [DebuggerStepThrough]
            get
            {
                if (!_MyTuple_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _MyTuple = default(ValueTuple<LazinatorTests.Examples.NonLazinatorClass, int?>);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _MyTuple_ByteIndex, _MyTuple_ByteLength);
                        _MyTuple = ConvertFromBytes_ValueTuple_NonLazinatorClass_Nullable_int(childData, DeserializationFactory, null);
                    }
                    _MyTuple_Accessed = true;
                    IsDirty = true;
                }
                return _MyTuple;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _MyTuple = value;
                _MyTuple_Accessed = true;
            }
        }
        internal bool _MyTuple_Accessed;
        
        /* Conversion */
        
        public int LazinatorUniqueID => 216;
        
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
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren) 
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            _MyChild2_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren) 
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            _MyLazinatorList_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren) 
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            _MyListValues_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren) 
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            _MyTuple_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren) 
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
        }
        
        public void SerializeExistingBuffer(BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
            // header information
            CompressedIntegralTypes.WriteCompressedInt(writer, LazinatorUniqueID);
            CompressedIntegralTypes.WriteCompressedInt(writer, Lazinator.Support.LazinatorVersionInfo.LazinatorIntVersion);
            CompressedIntegralTypes.WriteCompressedInt(writer, LazinatorObjectVersion);
            writer.Write((byte)includeChildrenMode);
            // write properties
            WriteUncompressedPrimitives.WriteBool(writer, _MyBool);
            EncodeCharAndString.WriteCharInTwoBytes(writer, _MyChar);
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren)  
            {
                var serializedBytesCopy = LazinatorObjectBytes;
                var byteIndexCopy = _MyChild1_ByteIndex;
                var byteLengthCopy = _MyChild1_ByteLength;
                WriteChildWithLength(writer, _MyChild1, includeChildrenMode, _MyChild1_Accessed, () => GetChildSlice(serializedBytesCopy, byteIndexCopy, byteLengthCopy), verifyCleanness, false);
            }
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren)  
            {
                var serializedBytesCopy = LazinatorObjectBytes;
                var byteIndexCopy = _MyChild2_ByteIndex;
                var byteLengthCopy = _MyChild2_ByteLength;
                WriteChildWithLength(writer, _MyChild2, includeChildrenMode, _MyChild2_Accessed, () => GetChildSlice(serializedBytesCopy, byteIndexCopy, byteLengthCopy), verifyCleanness, false);
            }
            var serializedBytesCopy_MyLazinatorList = LazinatorObjectBytes;
            var byteIndexCopy_MyLazinatorList = _MyLazinatorList_ByteIndex;
            var byteLengthCopy_MyLazinatorList = _MyLazinatorList_ByteLength;
            var copy_MyLazinatorList = MyLazinatorList;
            WriteNonLazinatorObject(
            nonLazinatorObject: _MyLazinatorList, isBelievedDirty: MyLazinatorList_Dirty,
            isAccessed: _MyLazinatorList_Accessed, writer: writer,
            getChildSliceForFieldFn: () => GetChildSlice(serializedBytesCopy_MyLazinatorList, byteIndexCopy_MyLazinatorList, byteLengthCopy_MyLazinatorList),
            verifyCleanness: verifyCleanness,
            binaryWriterAction: (w, v) =>
            ConvertToBytes_System_Collections_Generic_List_Example(w, copy_MyLazinatorList, includeChildrenMode, v));
            var serializedBytesCopy_MyListValues = LazinatorObjectBytes;
            var byteIndexCopy_MyListValues = _MyListValues_ByteIndex;
            var byteLengthCopy_MyListValues = _MyListValues_ByteLength;
            var copy_MyListValues = MyListValues;
            WriteNonLazinatorObject(
            nonLazinatorObject: _MyListValues, isBelievedDirty: _MyListValues_Accessed,
            isAccessed: _MyListValues_Accessed, writer: writer,
            getChildSliceForFieldFn: () => GetChildSlice(serializedBytesCopy_MyListValues, byteIndexCopy_MyListValues, byteLengthCopy_MyListValues),
            verifyCleanness: false,
            binaryWriterAction: (w, v) =>
            ConvertToBytes_System_Collections_Generic_List_int(w, copy_MyListValues, includeChildrenMode, v));
            var serializedBytesCopy_MyTuple = LazinatorObjectBytes;
            var byteIndexCopy_MyTuple = _MyTuple_ByteIndex;
            var byteLengthCopy_MyTuple = _MyTuple_ByteLength;
            var copy_MyTuple = MyTuple;
            WriteNonLazinatorObject(
            nonLazinatorObject: _MyTuple, isBelievedDirty: _MyTuple_Accessed,
            isAccessed: _MyTuple_Accessed, writer: writer,
            getChildSliceForFieldFn: () => GetChildSlice(serializedBytesCopy_MyTuple, byteIndexCopy_MyTuple, byteLengthCopy_MyTuple),
            verifyCleanness: false,
            binaryWriterAction: (w, v) =>
            ConvertToBytes_ValueTuple_NonLazinatorClass_Nullable_int(w, copy_MyTuple, includeChildrenMode, v));
        }
        
        /* Conversion of supported collections and tuples */
        
        private static System.Collections.Generic.List<LazinatorTests.Examples.Example> ConvertFromBytes_System_Collections_Generic_List_Example(ReadOnlyMemory<byte> storage, DeserializationFactory deserializationFactory, InformParentOfDirtinessDelegate informParentOfDirtinessDelegate)
        {
            if (storage.Length == 0)
            {
                return default(System.Collections.Generic.List<LazinatorTests.Examples.Example>);
            }
            ReadOnlySpan<byte> span = storage.Span;
            
            int bytesSoFar = 0;
            int collectionLength = span.ToDecompressedInt(ref bytesSoFar);
            
            System.Collections.Generic.List<LazinatorTests.Examples.Example> collection = new System.Collections.Generic.List<LazinatorTests.Examples.Example>(collectionLength);
            for (int i = 0; i < collectionLength; i++)
            {
                int lengthCollectionMember = span.ToInt32(ref bytesSoFar);
                if (lengthCollectionMember == 0)
                {
                    collection.Add(default(LazinatorTests.Examples.Example));
                }
                else
                {
                    ReadOnlyMemory<byte> childData = storage.Slice(bytesSoFar, lengthCollectionMember);
                    if (deserializationFactory == null)
                    {
                        throw new MissingDeserializationFactoryException();
                    }
                    var item = (LazinatorTests.Examples.Example)deserializationFactory.FactoryCreate(childData, informParentOfDirtinessDelegate);
                    collection.Add(item);
                }
                bytesSoFar += lengthCollectionMember;
            }
            
            return collection;
        }
        
        private static void ConvertToBytes_System_Collections_Generic_List_Example(BinaryBufferWriter writer, System.Collections.Generic.List<LazinatorTests.Examples.Example> itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
            if (itemToConvert == default(System.Collections.Generic.List<LazinatorTests.Examples.Example>))
            {
                return;
            }
            CompressedIntegralTypes.WriteCompressedInt(writer, itemToConvert.Count);
            int itemToConvertCount = itemToConvert.Count;
            for (int itemIndex = 0; itemIndex < itemToConvertCount; itemIndex++)
            {
                if (itemToConvert[itemIndex] == default(LazinatorTests.Examples.Example))
                {
                    writer.Write((uint)0);
                }
                else 
                {
                    
                    void action(BinaryBufferWriter w) => itemToConvert[itemIndex].SerializeExistingBuffer(writer, includeChildrenMode, verifyCleanness);
                    WriteToBinaryWithUintLengthPrefix(writer, action);
                }
                
            }
        }
        
        private static System.Collections.Generic.List<int> ConvertFromBytes_System_Collections_Generic_List_int(ReadOnlyMemory<byte> storage, DeserializationFactory deserializationFactory, InformParentOfDirtinessDelegate informParentOfDirtinessDelegate)
        {
            if (storage.Length == 0)
            {
                return default(System.Collections.Generic.List<int>);
            }
            ReadOnlySpan<byte> span = storage.Span;
            
            int bytesSoFar = 0;
            int collectionLength = span.ToDecompressedInt(ref bytesSoFar);
            
            System.Collections.Generic.List<int> collection = new System.Collections.Generic.List<int>(collectionLength);
            for (int i = 0; i < collectionLength; i++)
            {
                int item = span.ToDecompressedInt(ref bytesSoFar);
                collection.Add(item);
            }
            
            return collection;
        }
        
        private static void ConvertToBytes_System_Collections_Generic_List_int(BinaryBufferWriter writer, System.Collections.Generic.List<int> itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
            if (itemToConvert == default(System.Collections.Generic.List<int>))
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
        
        private static ValueTuple<LazinatorTests.Examples.NonLazinatorClass, int?> ConvertFromBytes_ValueTuple_NonLazinatorClass_Nullable_int(ReadOnlyMemory<byte> storage, DeserializationFactory deserializationFactory, InformParentOfDirtinessDelegate informParentOfDirtinessDelegate)
        {
            if (storage.Length == 0)
            {
                return default;
            }
            ReadOnlySpan<byte> span = storage.Span;
            
            int bytesSoFar = 0;
            
            LazinatorTests.Examples.NonLazinatorClass item1 = default;
            int lengthCollectionMember_item1 = span.ToInt32(ref bytesSoFar);
            if (lengthCollectionMember_item1 != 0)
            {
                ReadOnlyMemory<byte> childData = storage.Slice(bytesSoFar, lengthCollectionMember_item1);
                item1 = ConvertFromBytes_LazinatorTests_Examples_NonLazinatorClass(childData, deserializationFactory, informParentOfDirtinessDelegate);
            }
            bytesSoFar += lengthCollectionMember_item1;
            
            int? item2 = span.ToDecompressedNullableInt(ref bytesSoFar);
            
            var tupleType = new ValueTuple<LazinatorTests.Examples.NonLazinatorClass, int?>(item1, item2);
            
            return tupleType;
        }
        
        private static void ConvertToBytes_ValueTuple_NonLazinatorClass_Nullable_int(BinaryBufferWriter writer, ValueTuple<LazinatorTests.Examples.NonLazinatorClass, int?> itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
            
            if (itemToConvert.Item1 == null)
            {
                writer.Write((uint) 0);
            }
            else
            {
                void actionItem1(BinaryBufferWriter w) => ConvertToBytes_LazinatorTests_Examples_NonLazinatorClass(writer, itemToConvert.Item1, includeChildrenMode, verifyCleanness);
                WriteToBinaryWithUintLengthPrefix(writer, actionItem1);
            }
            
            CompressedIntegralTypes.WriteCompressedNullableInt(writer, itemToConvert.Item2);
        }
        
    }
}
