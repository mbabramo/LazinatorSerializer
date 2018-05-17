//62ee85af-989b-9b45-d7b0-a6f934317eaf
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
    
    
    public partial class DotNetList_SelfSerialized : ILazinator
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
            var clone = new DotNetList_SelfSerialized()
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
        
        protected int _MyListSerialized_ByteIndex;
        private int _DotNetList_SelfSerialized_EndByteIndex;
        protected virtual int _MyListSerialized_ByteLength => _DotNetList_SelfSerialized_EndByteIndex - _MyListSerialized_ByteIndex;
        
        private System.Collections.Generic.List<LazinatorTests.Examples.ExampleChild> _MyListSerialized;
        public System.Collections.Generic.List<LazinatorTests.Examples.ExampleChild> MyListSerialized
        {
            [DebuggerStepThrough]
            get
            {
                if (!_MyListSerialized_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _MyListSerialized = default(System.Collections.Generic.List<LazinatorTests.Examples.ExampleChild>);
                        _MyListSerialized_Dirty = true;
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _MyListSerialized_ByteIndex, _MyListSerialized_ByteLength);
                        _MyListSerialized = ConvertFromBytes_System_Collections_Generic_List_ExampleChild(childData, DeserializationFactory, () => { MyListSerialized_Dirty = true; });
                    }
                    _MyListSerialized_Accessed = true;
                }
                return _MyListSerialized;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _MyListSerialized = value;
                _MyListSerialized_Dirty = true;
                _MyListSerialized_Accessed = true;
            }
        }
        protected bool _MyListSerialized_Accessed;
        
        private bool _MyListSerialized_Dirty;
        public bool MyListSerialized_Dirty
        {
            [DebuggerStepThrough]
            get => _MyListSerialized_Dirty;
            [DebuggerStepThrough]
            set
            {
                if (_MyListSerialized_Dirty != value)
                {
                    _MyListSerialized_Dirty = value;
                    if (value && !IsDirty)
                    {
                        IsDirty = true;
                    }
                }
            }
        }
        
        /* Conversion */
        
        public virtual int LazinatorUniqueID => 208;
        
        public virtual int LazinatorObjectVersion { get; set; } = 0;
        
        public virtual void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
            _MyListSerialized_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            _DotNetList_SelfSerialized_EndByteIndex = bytesSoFar;
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
            nonLazinatorObject: _MyListSerialized, isBelievedDirty: MyListSerialized_Dirty,
            isAccessed: _MyListSerialized_Accessed, writer: writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorObjectBytes, _MyListSerialized_ByteIndex, _MyListSerialized_ByteLength),
            verifyCleanness: verifyCleanness,
            binaryWriterAction: (w, v) =>
            ConvertToBytes_System_Collections_Generic_List_ExampleChild(w, MyListSerialized,
            includeChildrenMode, v));
        }
        
        /* Conversion of supported collections and tuples */
        
        private static System.Collections.Generic.List<LazinatorTests.Examples.ExampleChild> ConvertFromBytes_System_Collections_Generic_List_ExampleChild(ReadOnlyMemory<byte> storage, DeserializationFactory deserializationFactory, InformParentOfDirtinessDelegate informParentOfDirtinessDelegate)
        {
            if (storage.Length == 0)
            {
                return default(System.Collections.Generic.List<LazinatorTests.Examples.ExampleChild>);
            }
            ReadOnlySpan<byte> span = storage.Span;
            
            int bytesSoFar = 0;
            int collectionLength = span.ToDecompressedInt(ref bytesSoFar);
            
            System.Collections.Generic.List<LazinatorTests.Examples.ExampleChild> collection = new System.Collections.Generic.List<LazinatorTests.Examples.ExampleChild>(collectionLength);
            for (int i = 0; i < collectionLength; i++)
            {
                int lengthCollectionMember = span.ToInt32(ref bytesSoFar);
                if (lengthCollectionMember == 0)
                {
                    collection.Add(default(LazinatorTests.Examples.ExampleChild));
                }
                else
                {
                    ReadOnlyMemory<byte> childData = storage.Slice(bytesSoFar, lengthCollectionMember);
                    if (deserializationFactory == null)
                    {
                        throw new MissingDeserializationFactoryException();
                    }
                    var item = (LazinatorTests.Examples.ExampleChild)deserializationFactory.FactoryCreate(childData, informParentOfDirtinessDelegate);
                    collection.Add(item);
                }
                bytesSoFar += lengthCollectionMember;
            }
            
            return collection;
        }
        
        private static void ConvertToBytes_System_Collections_Generic_List_ExampleChild(BinaryBufferWriter writer, System.Collections.Generic.List<LazinatorTests.Examples.ExampleChild> itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
            if (itemToConvert == default(System.Collections.Generic.List<LazinatorTests.Examples.ExampleChild>))
            {
                return;
            }
            CompressedIntegralTypes.WriteCompressedInt(writer, itemToConvert.Count);
            int itemToConvertCount = itemToConvert.Count;
            for (int itemIndex = 0; itemIndex < itemToConvertCount; itemIndex++)
            {
                if (itemToConvert[itemIndex] == default(LazinatorTests.Examples.ExampleChild))
                {
                    writer.Write((uint)0);
                }
                else 
                {
                    
                    void action(BinaryBufferWriter w) => itemToConvert[itemIndex].SerializeExistingBuffer(writer, includeChildrenMode, verifyCleanness);
                    WriteToBinaryWithIntLengthPrefix(writer, action);
                }
                
            }
        }
        
    }
}
