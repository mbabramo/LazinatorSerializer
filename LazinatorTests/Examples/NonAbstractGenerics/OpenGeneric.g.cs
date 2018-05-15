//afd729ca-bf26-8fdd-696a-a85d55dd5465
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.25, on 5/15/2018 11:17:52 AM
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LazinatorTests.Examples.NonAbstractGenerics
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
    
    public partial class OpenGeneric<T> : ILazinator
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
            var clone = new OpenGeneric<T>()
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
        
        internal int _MyListT_ByteIndex;
        internal int _MyT_ByteIndex;
        internal virtual int _MyListT_ByteLength => _MyT_ByteIndex - _MyListT_ByteIndex;
        private int _OpenGeneric_T_EndByteIndex = 0;
        internal virtual int _MyT_ByteLength => _OpenGeneric_T_EndByteIndex - _MyT_ByteIndex;
        
        private System.Collections.Generic.List<T> _MyListT;
        public virtual System.Collections.Generic.List<T> MyListT
        {
            [DebuggerStepThrough]
            get
            {
                if (!_MyListT_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _MyListT = default(System.Collections.Generic.List<T>);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _MyListT_ByteIndex, _MyListT_ByteLength);
                        _MyListT = ConvertFromBytes_System_Collections_Generic_List_T(childData, DeserializationFactory, null);
                    }
                    _MyListT_Accessed = true;
                    IsDirty = true;
                }
                return _MyListT;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _MyListT = value;
                _MyListT_Accessed = true;
            }
        }
        internal bool _MyListT_Accessed;
        private T _MyT;
        public virtual T MyT
        {
            [DebuggerStepThrough]
            get
            {
                if (!_MyT_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _MyT = default(T);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _MyT_ByteIndex, _MyT_ByteLength);
                        if (childData.Length == 0)
                        {
                            _MyT = default;
                        }
                        else _MyT = new T()
                        {
                            DeserializationFactory = DeserializationFactory,
                            LazinatorParentClass = this,
                            LazinatorObjectBytes = childData,
                        };
                    }
                    _MyT_Accessed = true;
                }
                return _MyT;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _MyT = value;
                _MyT_Accessed = true;
            }
        }
        internal bool _MyT_Accessed;
        
        /* Conversion */
        
        public virtual int LazinatorUniqueID => 233;
        
        public virtual int LazinatorObjectVersion { get; set; } = 0;
        
        public virtual void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
            _MyListT_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            _MyT_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            _OpenGeneric_T_EndByteIndex = bytesSoFar;
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
            nonLazinatorObject: _MyListT, isBelievedDirty: _MyListT_Accessed,
            isAccessed: _MyListT_Accessed, writer: writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorObjectBytes, _MyListT_ByteIndex, _MyListT_ByteLength),
            verifyCleanness: false,
            binaryWriterAction: (w, v) =>
            ConvertToBytes_System_Collections_Generic_List_T(w, MyListT,
            includeChildrenMode, v));
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                WriteChildWithLength(writer, _MyT, includeChildrenMode, _MyT_Accessed, () => GetChildSlice(LazinatorObjectBytes, _MyT_ByteIndex, _MyT_ByteLength), verifyCleanness, false);
            }
        }
        
        /* Conversion of supported collections and tuples */
        
        private static System.Collections.Generic.List<T> ConvertFromBytes_System_Collections_Generic_List_T(ReadOnlyMemory<byte> storage, DeserializationFactory deserializationFactory, InformParentOfDirtinessDelegate informParentOfDirtinessDelegate)
        {
            if (storage.Length == 0)
            {
                return default(System.Collections.Generic.List<T>);
            }
            ReadOnlySpan<byte> span = storage.Span;
            
            int bytesSoFar = 0;
            int collectionLength = span.ToDecompressedInt(ref bytesSoFar);
            
            System.Collections.Generic.List<T> collection = new System.Collections.Generic.List<T>(collectionLength);
            for (int i = 0; i < collectionLength; i++)
            {
                int lengthCollectionMember = span.ToInt32(ref bytesSoFar);
                if (lengthCollectionMember == 0)
                {
                    collection.Add(default(T));
                }
                else
                {
                    ReadOnlyMemory<byte> childData = storage.Slice(bytesSoFar, lengthCollectionMember);
                    if (deserializationFactory == null)
                    {
                        throw new MissingDeserializationFactoryException();
                    }
                    var item = (T)deserializationFactory.FactoryCreate(childData, informParentOfDirtinessDelegate);
                    collection.Add(item);
                }
                bytesSoFar += lengthCollectionMember;
            }
            
            return collection;
        }
        
        private static void ConvertToBytes_System_Collections_Generic_List_T(BinaryBufferWriter writer, System.Collections.Generic.List<T> itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
            if (itemToConvert == default(System.Collections.Generic.List<T>))
            {
                return;
            }
            CompressedIntegralTypes.WriteCompressedInt(writer, itemToConvert.Count);
            int itemToConvertCount = itemToConvert.Count;
            for (int itemIndex = 0; itemIndex < itemToConvertCount; itemIndex++)
            {
                if (System.Collections.Generic.EqualityComparer<T>.Default.Equals(itemToConvert[itemIndex], default(T)))
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
