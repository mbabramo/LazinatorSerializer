//511602bd-2353-aa6b-0991-c23f08672c04
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.26
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LazinatorTests.Examples.Collections
{
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
    
    
    public partial class DotNetList_Values : ILazinator
    {
        /* Boilerplate for every non-abstract ILazinator object */
        
        public virtual ILazinator LazinatorParentClass { get; set; }
        
        protected IncludeChildrenMode OriginalIncludeChildrenMode;
        
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
        
        protected virtual MemoryInBuffer EncodeToNewBuffer(IncludeChildrenMode includeChildrenMode, bool verifyCleanness) => LazinatorUtilities.EncodeToNewBinaryBufferWriter(this, includeChildrenMode, verifyCleanness);
        
        public virtual ILazinator CloneLazinator()
        {
            return CloneLazinator(OriginalIncludeChildrenMode);
        }
        
        public virtual ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode)
        {
            MemoryInBuffer bytes = EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, false, false, IsDirty, DescendantIsDirty, false, LazinatorObjectBytes, (StreamManuallyDelegate)EncodeToNewBuffer);
            var clone = new DotNetList_Values()
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
            {
                InformParentOfDirtinessDelegate();
            }
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
        
        public virtual void MarkHierarchyClean()
        {
            _IsDirty = false;
            _DescendantIsDirty = false;
            _MyLinkedListInt_Dirty = false;
            _MyListInt_Dirty = false;
            _MySortedSetInt_Dirty = false;
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
        
        protected int _MyLinkedListInt_ByteIndex;
        protected int _MyListInt_ByteIndex;
        protected int _MySortedSetInt_ByteIndex;
        protected virtual int _MyLinkedListInt_ByteLength => _MyListInt_ByteIndex - _MyLinkedListInt_ByteIndex;
        protected virtual int _MyListInt_ByteLength => _MySortedSetInt_ByteIndex - _MyListInt_ByteIndex;
        private int _DotNetList_Values_EndByteIndex;
        protected virtual int _MySortedSetInt_ByteLength => _DotNetList_Values_EndByteIndex - _MySortedSetInt_ByteIndex;
        
        private System.Collections.Generic.LinkedList<int> _MyLinkedListInt;
        public System.Collections.Generic.LinkedList<int> MyLinkedListInt
        {
            [DebuggerStepThrough]
            get
            {
                if (!_MyLinkedListInt_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _MyLinkedListInt = default(System.Collections.Generic.LinkedList<int>);
                        _MyLinkedListInt_Dirty = true;
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _MyLinkedListInt_ByteIndex, _MyLinkedListInt_ByteLength);
                        _MyLinkedListInt = ConvertFromBytes_System_Collections_Generic_LinkedList_int(childData, DeserializationFactory, () => { MyLinkedListInt_Dirty = true; });
                    }
                    _MyLinkedListInt_Accessed = true;
                }
                return _MyLinkedListInt;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _MyLinkedListInt = value;
                _MyLinkedListInt_Dirty = true;
                _MyLinkedListInt_Accessed = true;
            }
        }
        protected bool _MyLinkedListInt_Accessed;
        
        private bool _MyLinkedListInt_Dirty;
        public bool MyLinkedListInt_Dirty
        {
            [DebuggerStepThrough]
            get => _MyLinkedListInt_Dirty;
            [DebuggerStepThrough]
            set
            {
                if (_MyLinkedListInt_Dirty != value)
                {
                    _MyLinkedListInt_Dirty = value;
                    if (value && !IsDirty)
                    {
                        IsDirty = true;
                    }
                }
            }
        }
        private System.Collections.Generic.List<int> _MyListInt;
        public System.Collections.Generic.List<int> MyListInt
        {
            [DebuggerStepThrough]
            get
            {
                if (!_MyListInt_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _MyListInt = default(System.Collections.Generic.List<int>);
                        _MyListInt_Dirty = true;
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _MyListInt_ByteIndex, _MyListInt_ByteLength);
                        _MyListInt = ConvertFromBytes_System_Collections_Generic_List_int(childData, DeserializationFactory, () => { MyListInt_Dirty = true; });
                    }
                    _MyListInt_Accessed = true;
                }
                return _MyListInt;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _MyListInt = value;
                _MyListInt_Dirty = true;
                _MyListInt_Accessed = true;
            }
        }
        protected bool _MyListInt_Accessed;
        
        private bool _MyListInt_Dirty;
        public bool MyListInt_Dirty
        {
            [DebuggerStepThrough]
            get => _MyListInt_Dirty;
            [DebuggerStepThrough]
            set
            {
                if (_MyListInt_Dirty != value)
                {
                    _MyListInt_Dirty = value;
                    if (value && !IsDirty)
                    {
                        IsDirty = true;
                    }
                }
            }
        }
        private System.Collections.Generic.SortedSet<int> _MySortedSetInt;
        public System.Collections.Generic.SortedSet<int> MySortedSetInt
        {
            [DebuggerStepThrough]
            get
            {
                if (!_MySortedSetInt_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _MySortedSetInt = default(System.Collections.Generic.SortedSet<int>);
                        _MySortedSetInt_Dirty = true;
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _MySortedSetInt_ByteIndex, _MySortedSetInt_ByteLength);
                        _MySortedSetInt = ConvertFromBytes_System_Collections_Generic_SortedSet_int(childData, DeserializationFactory, () => { MySortedSetInt_Dirty = true; });
                    }
                    _MySortedSetInt_Accessed = true;
                }
                return _MySortedSetInt;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _MySortedSetInt = value;
                _MySortedSetInt_Dirty = true;
                _MySortedSetInt_Accessed = true;
            }
        }
        protected bool _MySortedSetInt_Accessed;
        
        private bool _MySortedSetInt_Dirty;
        public bool MySortedSetInt_Dirty
        {
            [DebuggerStepThrough]
            get => _MySortedSetInt_Dirty;
            [DebuggerStepThrough]
            set
            {
                if (_MySortedSetInt_Dirty != value)
                {
                    _MySortedSetInt_Dirty = value;
                    if (value && !IsDirty)
                    {
                        IsDirty = true;
                    }
                }
            }
        }
        
        /* Conversion */
        
        public virtual int LazinatorUniqueID => 209;
        
        public virtual int LazinatorObjectVersion { get; set; } = 0;
        
        public virtual void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
            _MyLinkedListInt_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            _MyListInt_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            _MySortedSetInt_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            _DotNetList_Values_EndByteIndex = bytesSoFar;
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
            nonLazinatorObject: _MyLinkedListInt, isBelievedDirty: MyLinkedListInt_Dirty,
            isAccessed: _MyLinkedListInt_Accessed, writer: writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorObjectBytes, _MyLinkedListInt_ByteIndex, _MyLinkedListInt_ByteLength),
            verifyCleanness: verifyCleanness,
            binaryWriterAction: (w, v) =>
            ConvertToBytes_System_Collections_Generic_LinkedList_int(w, MyLinkedListInt,
            includeChildrenMode, v));
            WriteNonLazinatorObject(
            nonLazinatorObject: _MyListInt, isBelievedDirty: MyListInt_Dirty,
            isAccessed: _MyListInt_Accessed, writer: writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorObjectBytes, _MyListInt_ByteIndex, _MyListInt_ByteLength),
            verifyCleanness: verifyCleanness,
            binaryWriterAction: (w, v) =>
            ConvertToBytes_System_Collections_Generic_List_int(w, MyListInt,
            includeChildrenMode, v));
            WriteNonLazinatorObject(
            nonLazinatorObject: _MySortedSetInt, isBelievedDirty: MySortedSetInt_Dirty,
            isAccessed: _MySortedSetInt_Accessed, writer: writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorObjectBytes, _MySortedSetInt_ByteIndex, _MySortedSetInt_ByteLength),
            verifyCleanness: verifyCleanness,
            binaryWriterAction: (w, v) =>
            ConvertToBytes_System_Collections_Generic_SortedSet_int(w, MySortedSetInt,
            includeChildrenMode, v));
        }
        
        /* Conversion of supported collections and tuples */
        
        private static System.Collections.Generic.LinkedList<int> ConvertFromBytes_System_Collections_Generic_LinkedList_int(ReadOnlyMemory<byte> storage, DeserializationFactory deserializationFactory, InformParentOfDirtinessDelegate informParentOfDirtinessDelegate)
        {
            if (storage.Length == 0)
            {
                return default(System.Collections.Generic.LinkedList<int>);
            }
            ReadOnlySpan<byte> span = storage.Span;
            
            int bytesSoFar = 0;
            int collectionLength = span.ToDecompressedInt(ref bytesSoFar);
            
            System.Collections.Generic.LinkedList<int> collection = new System.Collections.Generic.LinkedList<int>();
            for (int i = 0; i < collectionLength; i++)
            {
                int item = span.ToDecompressedInt(ref bytesSoFar);
                collection.AddLast(item);
            }
            
            return collection;
        }
        
        private static void ConvertToBytes_System_Collections_Generic_LinkedList_int(BinaryBufferWriter writer, System.Collections.Generic.LinkedList<int> itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
            if (itemToConvert == default(System.Collections.Generic.LinkedList<int>))
            {
                return;
            }
            CompressedIntegralTypes.WriteCompressedInt(writer, itemToConvert.Count);
            int itemToConvertCount = itemToConvert.Count;
            for (int itemIndex = 0; itemIndex < itemToConvertCount; itemIndex++)
            {
                CompressedIntegralTypes.WriteCompressedInt(writer, System.Linq.Enumerable.ElementAt(itemToConvert, itemIndex));
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
        
        private static System.Collections.Generic.SortedSet<int> ConvertFromBytes_System_Collections_Generic_SortedSet_int(ReadOnlyMemory<byte> storage, DeserializationFactory deserializationFactory, InformParentOfDirtinessDelegate informParentOfDirtinessDelegate)
        {
            if (storage.Length == 0)
            {
                return default(System.Collections.Generic.SortedSet<int>);
            }
            ReadOnlySpan<byte> span = storage.Span;
            
            int bytesSoFar = 0;
            int collectionLength = span.ToDecompressedInt(ref bytesSoFar);
            
            System.Collections.Generic.SortedSet<int> collection = new System.Collections.Generic.SortedSet<int>();
            for (int i = 0; i < collectionLength; i++)
            {
                int item = span.ToDecompressedInt(ref bytesSoFar);
                collection.Add(item);
            }
            
            return collection;
        }
        
        private static void ConvertToBytes_System_Collections_Generic_SortedSet_int(BinaryBufferWriter writer, System.Collections.Generic.SortedSet<int> itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
            if (itemToConvert == default(System.Collections.Generic.SortedSet<int>))
            {
                return;
            }
            CompressedIntegralTypes.WriteCompressedInt(writer, itemToConvert.Count);
            int itemToConvertCount = itemToConvert.Count;
            var sortedSet = System.Linq.Enumerable.ToList(itemToConvert);
            for (int itemIndex = 0; itemIndex < itemToConvertCount; itemIndex++)
            {
                CompressedIntegralTypes.WriteCompressedInt(writer, sortedSet[itemIndex]);
            }
        }
        
    }
}
