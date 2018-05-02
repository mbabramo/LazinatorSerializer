//cbec5f44-11be-77dd-d80e-7adb84b131c2
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

namespace LazinatorTests.Examples.Collections
{
    public partial class Dictionary_Values_SelfSerialized : ILazinator
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
        
        internal virtual MemoryInBuffer EncodeToNewBuffer(IncludeChildrenMode includeChildrenMode, bool verifyCleanness) => LazinatorUtilities.EncodeToNewBinaryBufferWriter(this, includeChildrenMode, verifyCleanness);
        
        public virtual ILazinator CloneLazinator()
        {
            return CloneLazinator(OriginalIncludeChildrenMode);
        }
        
        public virtual ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode)
        {
            MemoryInBuffer bytes = EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, false, false, IsDirty, DescendantIsDirty, false, LazinatorObjectBytes, (StreamManuallyDelegate)EncodeToNewBuffer);
            var clone = new Dictionary_Values_SelfSerialized()
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
        
        /* Field boilerplate */
        
        internal int _MyDictionary_ByteIndex;
        internal int _MySortedDictionary_ByteIndex;
        internal int _MySortedList_ByteIndex;
        internal int _MyDictionary_ByteLength => _MySortedDictionary_ByteIndex - _MyDictionary_ByteIndex;
        internal int _MySortedDictionary_ByteLength => _MySortedList_ByteIndex - _MySortedDictionary_ByteIndex;
        internal int _MySortedList_ByteLength => LazinatorObjectBytes.Length - _MySortedList_ByteIndex;
        
        private System.Collections.Generic.Dictionary<int, LazinatorTests.Examples.ExampleChild> _MyDictionary;
        public System.Collections.Generic.Dictionary<int, LazinatorTests.Examples.ExampleChild> MyDictionary
        {
            [DebuggerStepThrough]
            get
            {
                if (!_MyDictionary_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _MyDictionary = default(System.Collections.Generic.Dictionary<int, LazinatorTests.Examples.ExampleChild>);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _MyDictionary_ByteIndex, _MyDictionary_ByteLength);
                        _MyDictionary = ConvertFromBytes_System_Collections_Generic_Dictionary_int_ExampleChild(childData, DeserializationFactory, null);
                    }
                    _MyDictionary_Accessed = true;
                    IsDirty = true;
                }
                return _MyDictionary;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _MyDictionary = value;
                _MyDictionary_Accessed = true;
            }
        }
        internal bool _MyDictionary_Accessed;
        private System.Collections.Generic.SortedDictionary<int, LazinatorTests.Examples.ExampleChild> _MySortedDictionary;
        public System.Collections.Generic.SortedDictionary<int, LazinatorTests.Examples.ExampleChild> MySortedDictionary
        {
            [DebuggerStepThrough]
            get
            {
                if (!_MySortedDictionary_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _MySortedDictionary = default(System.Collections.Generic.SortedDictionary<int, LazinatorTests.Examples.ExampleChild>);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _MySortedDictionary_ByteIndex, _MySortedDictionary_ByteLength);
                        _MySortedDictionary = ConvertFromBytes_System_Collections_Generic_SortedDictionary_int_ExampleChild(childData, DeserializationFactory, null);
                    }
                    _MySortedDictionary_Accessed = true;
                    IsDirty = true;
                }
                return _MySortedDictionary;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _MySortedDictionary = value;
                _MySortedDictionary_Accessed = true;
            }
        }
        internal bool _MySortedDictionary_Accessed;
        private System.Collections.Generic.SortedList<int, LazinatorTests.Examples.ExampleChild> _MySortedList;
        public System.Collections.Generic.SortedList<int, LazinatorTests.Examples.ExampleChild> MySortedList
        {
            [DebuggerStepThrough]
            get
            {
                if (!_MySortedList_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _MySortedList = default(System.Collections.Generic.SortedList<int, LazinatorTests.Examples.ExampleChild>);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _MySortedList_ByteIndex, _MySortedList_ByteLength);
                        _MySortedList = ConvertFromBytes_System_Collections_Generic_SortedList_int_ExampleChild(childData, DeserializationFactory, null);
                    }
                    _MySortedList_Accessed = true;
                    IsDirty = true;
                }
                return _MySortedList;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _MySortedList = value;
                _MySortedList_Accessed = true;
            }
        }
        internal bool _MySortedList_Accessed;
        
        /* Conversion */
        
        public virtual int LazinatorUniqueID => 104;
        
        public virtual int LazinatorObjectVersion { get; set; } = 0;
        
        public virtual void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
            _MyDictionary_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren) 
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            _MySortedDictionary_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren) 
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            _MySortedList_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren) 
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
            nonLazinatorObject: _MyDictionary, isBelievedDirty: _MyDictionary_Accessed,
            isAccessed: _MyDictionary_Accessed, writer: writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorObjectBytes, _MyDictionary_ByteIndex, _MyDictionary_ByteLength),
            verifyCleanness: false,
            binaryWriterAction: (w, v) =>
            ConvertToBytes_System_Collections_Generic_Dictionary_int_ExampleChild(w, MyDictionary,
            includeChildrenMode, v));
            WriteNonLazinatorObject(
            nonLazinatorObject: _MySortedDictionary, isBelievedDirty: _MySortedDictionary_Accessed,
            isAccessed: _MySortedDictionary_Accessed, writer: writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorObjectBytes, _MySortedDictionary_ByteIndex, _MySortedDictionary_ByteLength),
            verifyCleanness: false,
            binaryWriterAction: (w, v) =>
            ConvertToBytes_System_Collections_Generic_SortedDictionary_int_ExampleChild(w, MySortedDictionary,
            includeChildrenMode, v));
            WriteNonLazinatorObject(
            nonLazinatorObject: _MySortedList, isBelievedDirty: _MySortedList_Accessed,
            isAccessed: _MySortedList_Accessed, writer: writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorObjectBytes, _MySortedList_ByteIndex, _MySortedList_ByteLength),
            verifyCleanness: false,
            binaryWriterAction: (w, v) =>
            ConvertToBytes_System_Collections_Generic_SortedList_int_ExampleChild(w, MySortedList,
            includeChildrenMode, v));
        }
        
        /* Conversion of supported collections and tuples */
        
        private static System.Collections.Generic.Dictionary<int, LazinatorTests.Examples.ExampleChild> ConvertFromBytes_System_Collections_Generic_Dictionary_int_ExampleChild(ReadOnlyMemory<byte> storage, DeserializationFactory deserializationFactory, InformParentOfDirtinessDelegate informParentOfDirtinessDelegate)
        {
            if (storage.Length == 0)
            {
                return default(System.Collections.Generic.Dictionary<int, LazinatorTests.Examples.ExampleChild>);
            }
            ReadOnlySpan<byte> span = storage.Span;
            
            int bytesSoFar = 0;
            int collectionLength = span.ToDecompressedInt(ref bytesSoFar);
            
            System.Collections.Generic.Dictionary<int, LazinatorTests.Examples.ExampleChild> collection = new System.Collections.Generic.Dictionary<int, LazinatorTests.Examples.ExampleChild>(collectionLength);
            for (int i = 0; i < collectionLength; i++)
            {
                int lengthCollectionMember = span.ToInt32(ref bytesSoFar);
                ReadOnlyMemory<byte> childData = storage.Slice(bytesSoFar, lengthCollectionMember);
                var item = ConvertFromBytes_System_Collections_Generic_KeyValuePair_int_ExampleChild(childData, deserializationFactory, informParentOfDirtinessDelegate);
                collection.Add(item.Key, item.Value);
                bytesSoFar += lengthCollectionMember;
            }
            
            return collection;
        }
        
        private static void ConvertToBytes_System_Collections_Generic_Dictionary_int_ExampleChild(BinaryBufferWriter writer, System.Collections.Generic.Dictionary<int, LazinatorTests.Examples.ExampleChild> itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
            if (itemToConvert == default(System.Collections.Generic.Dictionary<int, LazinatorTests.Examples.ExampleChild>))
            {
                return;
            }
            CompressedIntegralTypes.WriteCompressedInt(writer, itemToConvert.Count);
            foreach (var item in itemToConvert)
            {
                void action(BinaryBufferWriter w) => ConvertToBytes_System_Collections_Generic_KeyValuePair_int_ExampleChild(writer, item, includeChildrenMode, verifyCleanness);
                WriteToBinaryWithUintLengthPrefix(writer, action);
            }
        }
        
        private static System.Collections.Generic.KeyValuePair<int, LazinatorTests.Examples.ExampleChild> ConvertFromBytes_System_Collections_Generic_KeyValuePair_int_ExampleChild(ReadOnlyMemory<byte> storage, DeserializationFactory deserializationFactory, InformParentOfDirtinessDelegate informParentOfDirtinessDelegate)
        {
            if (storage.Length == 0)
            {
                return default;
            }
            ReadOnlySpan<byte> span = storage.Span;
            
            int bytesSoFar = 0;
            
            int item1 = span.ToDecompressedInt(ref bytesSoFar);
            
            LazinatorTests.Examples.ExampleChild item2 = default;
            int lengthCollectionMember_item2 = span.ToInt32(ref bytesSoFar);
            if (lengthCollectionMember_item2 != 0)
            {
                ReadOnlyMemory<byte> childData = storage.Slice(bytesSoFar, lengthCollectionMember_item2);
                if (deserializationFactory == null)
                {
                    throw new MissingDeserializationFactoryException();
                }
                item2 = (LazinatorTests.Examples.ExampleChild)deserializationFactory.FactoryCreate(childData, informParentOfDirtinessDelegate);
            }
            bytesSoFar += lengthCollectionMember_item2;
            
            var tupleType = new System.Collections.Generic.KeyValuePair<int, LazinatorTests.Examples.ExampleChild>(item1, item2);
            
            return tupleType;
        }
        
        private static void ConvertToBytes_System_Collections_Generic_KeyValuePair_int_ExampleChild(BinaryBufferWriter writer, System.Collections.Generic.KeyValuePair<int, LazinatorTests.Examples.ExampleChild> itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
            
            CompressedIntegralTypes.WriteCompressedInt(writer, itemToConvert.Key);
            
            if (itemToConvert.Value == null)
            {
                writer.Write((uint) 0);
            }
            else
            {
                void actionValue(BinaryBufferWriter w) => itemToConvert.Value.SerializeExistingBuffer(writer, includeChildrenMode, verifyCleanness);
                WriteToBinaryWithUintLengthPrefix(writer, actionValue);
            };
        }
        
        private static System.Collections.Generic.SortedDictionary<int, LazinatorTests.Examples.ExampleChild> ConvertFromBytes_System_Collections_Generic_SortedDictionary_int_ExampleChild(ReadOnlyMemory<byte> storage, DeserializationFactory deserializationFactory, InformParentOfDirtinessDelegate informParentOfDirtinessDelegate)
        {
            if (storage.Length == 0)
            {
                return default(System.Collections.Generic.SortedDictionary<int, LazinatorTests.Examples.ExampleChild>);
            }
            ReadOnlySpan<byte> span = storage.Span;
            
            int bytesSoFar = 0;
            int collectionLength = span.ToDecompressedInt(ref bytesSoFar);
            
            System.Collections.Generic.SortedDictionary<int, LazinatorTests.Examples.ExampleChild> collection = new System.Collections.Generic.SortedDictionary<int, LazinatorTests.Examples.ExampleChild>();
            for (int i = 0; i < collectionLength; i++)
            {
                int lengthCollectionMember = span.ToInt32(ref bytesSoFar);
                ReadOnlyMemory<byte> childData = storage.Slice(bytesSoFar, lengthCollectionMember);
                var item = ConvertFromBytes_System_Collections_Generic_KeyValuePair_int_ExampleChild(childData, deserializationFactory, informParentOfDirtinessDelegate);
                collection.Add(item.Key, item.Value);
                bytesSoFar += lengthCollectionMember;
            }
            
            return collection;
        }
        
        private static void ConvertToBytes_System_Collections_Generic_SortedDictionary_int_ExampleChild(BinaryBufferWriter writer, System.Collections.Generic.SortedDictionary<int, LazinatorTests.Examples.ExampleChild> itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
            if (itemToConvert == default(System.Collections.Generic.SortedDictionary<int, LazinatorTests.Examples.ExampleChild>))
            {
                return;
            }
            CompressedIntegralTypes.WriteCompressedInt(writer, itemToConvert.Count);
            foreach (var item in itemToConvert)
            {
                void action(BinaryBufferWriter w) => ConvertToBytes_System_Collections_Generic_KeyValuePair_int_ExampleChild(writer, item, includeChildrenMode, verifyCleanness);
                WriteToBinaryWithUintLengthPrefix(writer, action);
            }
        }
        
        private static System.Collections.Generic.SortedList<int, LazinatorTests.Examples.ExampleChild> ConvertFromBytes_System_Collections_Generic_SortedList_int_ExampleChild(ReadOnlyMemory<byte> storage, DeserializationFactory deserializationFactory, InformParentOfDirtinessDelegate informParentOfDirtinessDelegate)
        {
            if (storage.Length == 0)
            {
                return default(System.Collections.Generic.SortedList<int, LazinatorTests.Examples.ExampleChild>);
            }
            ReadOnlySpan<byte> span = storage.Span;
            
            int bytesSoFar = 0;
            int collectionLength = span.ToDecompressedInt(ref bytesSoFar);
            
            System.Collections.Generic.SortedList<int, LazinatorTests.Examples.ExampleChild> collection = new System.Collections.Generic.SortedList<int, LazinatorTests.Examples.ExampleChild>(collectionLength);
            for (int i = 0; i < collectionLength; i++)
            {
                int lengthCollectionMember = span.ToInt32(ref bytesSoFar);
                ReadOnlyMemory<byte> childData = storage.Slice(bytesSoFar, lengthCollectionMember);
                var item = ConvertFromBytes_System_Collections_Generic_KeyValuePair_int_ExampleChild(childData, deserializationFactory, informParentOfDirtinessDelegate);
                collection.Add(item.Key, item.Value);
                bytesSoFar += lengthCollectionMember;
            }
            
            return collection;
        }
        
        private static void ConvertToBytes_System_Collections_Generic_SortedList_int_ExampleChild(BinaryBufferWriter writer, System.Collections.Generic.SortedList<int, LazinatorTests.Examples.ExampleChild> itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
            if (itemToConvert == default(System.Collections.Generic.SortedList<int, LazinatorTests.Examples.ExampleChild>))
            {
                return;
            }
            CompressedIntegralTypes.WriteCompressedInt(writer, itemToConvert.Count);
            foreach (var item in itemToConvert)
            {
                void action(BinaryBufferWriter w) => ConvertToBytes_System_Collections_Generic_KeyValuePair_int_ExampleChild(writer, item, includeChildrenMode, verifyCleanness);
                WriteToBinaryWithUintLengthPrefix(writer, action);
            }
        }
        
    }
}
