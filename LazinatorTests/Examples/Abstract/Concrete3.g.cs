//0c140114-d588-9648-a375-6f7fa52effff
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.36
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LazinatorTests.Examples.Abstract
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
    public partial class Concrete3 : ILazinator
    {
        /* Serialization, deserialization, and object relationships */
        
        public Concrete3() : base()
        {
        }
        
        public override ILazinator LazinatorParentClass { get; set; }
        
        protected IncludeChildrenMode OriginalIncludeChildrenMode;
        
        public override void Deserialize()
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
        
        public override MemoryInBuffer SerializeNewBuffer(IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
            return EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, true, verifyCleanness, IsDirty, DescendantIsDirty, false, LazinatorObjectBytes, (StreamManuallyDelegate) EncodeToNewBuffer);
        }
        
        protected override MemoryInBuffer EncodeToNewBuffer(IncludeChildrenMode includeChildrenMode, bool verifyCleanness) => LazinatorUtilities.EncodeToNewBinaryBufferWriter(this, includeChildrenMode, verifyCleanness);
        
        public override ILazinator CloneLazinator()
        {
            return CloneLazinator(OriginalIncludeChildrenMode);
        }
        
        public override ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode)
        {
            MemoryInBuffer bytes = EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, false, false, IsDirty, DescendantIsDirty, false, LazinatorObjectBytes, (StreamManuallyDelegate)EncodeToNewBuffer);
            var clone = new Concrete3()
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
        public override bool IsDirty
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
        
        public override InformParentOfDirtinessDelegate InformParentOfDirtinessDelegate { get; set; }
        public override void InformParentOfDirtiness()
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
        public override bool DescendantIsDirty
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
        
        public override void MarkHierarchyClean()
        {
            _IsDirty = false;
            _DescendantIsDirty = false;
        }
        
        public override DeserializationFactory DeserializationFactory { get; set; }
        
        private MemoryInBuffer _HierarchyBytes;
        public override MemoryInBuffer HierarchyBytes
        {
            get => _HierarchyBytes;
            set
            {
                _HierarchyBytes = value;
                LazinatorObjectBytes = value.FilledMemory;
            }
        }
        
        private ReadOnlyMemory<byte> _LazinatorObjectBytes;
        public override ReadOnlyMemory<byte> LazinatorObjectBytes
        {
            get => _LazinatorObjectBytes;
            set
            {
                _LazinatorObjectBytes = value;
                Deserialize();
            }
        }
        
        public override void LazinatorConvertToBytes()
        {
            if (!IsDirty && !DescendantIsDirty)
            {
                return;
            }
            MemoryInBuffer bytes = EncodeOrRecycleToNewBuffer(IncludeChildrenMode.IncludeAllChildren, OriginalIncludeChildrenMode, false, false, IsDirty, DescendantIsDirty, false, LazinatorObjectBytes, (StreamManuallyDelegate)EncodeToNewBuffer);
            _IsDirty = false;
            LazinatorObjectBytes = bytes.FilledMemory;
        }
        
        public override uint GetBinaryHashCode32()
        {
            LazinatorConvertToBytes();
            return FarmhashByteSpans.Hash32(LazinatorObjectBytes.Span);
        }
        
        public override ulong GetBinaryHashCode64()
        {
            LazinatorConvertToBytes();
            return FarmhashByteSpans.Hash64(LazinatorObjectBytes.Span);
        }
        
        /* Field definitions */
        
        protected int _IntList3_ByteIndex;
        protected override int _IntList1_ByteLength => _IntList2_ByteIndex - _IntList1_ByteIndex;
        protected override int _IntList2_ByteLength => _IntList3_ByteIndex - _IntList2_ByteIndex;
        private int _Concrete3_EndByteIndex;
        protected virtual int _IntList3_ByteLength => _Concrete3_EndByteIndex - _IntList3_ByteIndex;
        
        private string _String1;
        public override string String1
        {
            [DebuggerStepThrough]
            get
            {
                return _String1;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _String1 = value;
            }
        }
        private string _String2;
        public override string String2
        {
            [DebuggerStepThrough]
            get
            {
                return _String2;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _String2 = value;
            }
        }
        private string _String3;
        public string String3
        {
            [DebuggerStepThrough]
            get
            {
                return _String3;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _String3 = value;
            }
        }
        private List<int> _IntList1;
        public override List<int> IntList1
        {
            [DebuggerStepThrough]
            get
            {
                if (!_IntList1_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _IntList1 = default(List<int>);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _IntList1_ByteIndex, _IntList1_ByteLength);
                        _IntList1 = ConvertFromBytes_List_Gint_g(childData, DeserializationFactory, null);
                    }
                    _IntList1_Accessed = true;
                    IsDirty = true;
                }
                return _IntList1;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _IntList1 = value;
                _IntList1_Accessed = true;
            }
        }
        private List<int> _IntList2;
        public override List<int> IntList2
        {
            [DebuggerStepThrough]
            get
            {
                if (!_IntList2_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _IntList2 = default(List<int>);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _IntList2_ByteIndex, _IntList2_ByteLength);
                        _IntList2 = ConvertFromBytes_List_Gint_g(childData, DeserializationFactory, null);
                    }
                    _IntList2_Accessed = true;
                    IsDirty = true;
                }
                return _IntList2;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _IntList2 = value;
                _IntList2_Accessed = true;
            }
        }
        private List<int> _IntList3;
        public List<int> IntList3
        {
            [DebuggerStepThrough]
            get
            {
                if (!_IntList3_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _IntList3 = default(List<int>);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _IntList3_ByteIndex, _IntList3_ByteLength);
                        _IntList3 = ConvertFromBytes_List_Gint_g(childData, DeserializationFactory, null);
                    }
                    _IntList3_Accessed = true;
                    IsDirty = true;
                }
                return _IntList3;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _IntList3 = value;
                _IntList3_Accessed = true;
            }
        }
        protected bool _IntList3_Accessed;
        
        /* Conversion */
        
        public override int LazinatorUniqueID => 237;
        
        public override int LazinatorObjectVersion { get; set; } = 0;
        
        public override void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
            _String1 = span.ToString_BrotliCompressedWithLength(ref bytesSoFar);
            _String2 = span.ToString_BrotliCompressedWithLength(ref bytesSoFar);
            _String3 = span.ToString_BrotliCompressedWithLength(ref bytesSoFar);
            _IntList1_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            _IntList2_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            _IntList3_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            _Concrete3_EndByteIndex = bytesSoFar;
        }
        
        public override void SerializeExistingBuffer(BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
            // header information
            CompressedIntegralTypes.WriteCompressedInt(writer, LazinatorUniqueID);
            CompressedIntegralTypes.WriteCompressedInt(writer, Lazinator.Support.LazinatorVersionInfo.LazinatorIntVersion);
            CompressedIntegralTypes.WriteCompressedInt(writer, LazinatorObjectVersion);
            writer.Write((byte)includeChildrenMode);
            // write properties
            EncodeCharAndString.WriteBrotliCompressedWithIntPrefix(writer, _String1);
            EncodeCharAndString.WriteBrotliCompressedWithIntPrefix(writer, _String2);
            EncodeCharAndString.WriteBrotliCompressedWithIntPrefix(writer, _String3);
            WriteNonLazinatorObject(
            nonLazinatorObject: _IntList1, isBelievedDirty: _IntList1_Accessed,
            isAccessed: _IntList1_Accessed, writer: writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorObjectBytes, _IntList1_ByteIndex, _IntList1_ByteLength),
            verifyCleanness: false,
            binaryWriterAction: (w, v) =>
            ConvertToBytes_List_Gint_g(w, IntList1,
            includeChildrenMode, v));
            WriteNonLazinatorObject(
            nonLazinatorObject: _IntList2, isBelievedDirty: _IntList2_Accessed,
            isAccessed: _IntList2_Accessed, writer: writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorObjectBytes, _IntList2_ByteIndex, _IntList2_ByteLength),
            verifyCleanness: false,
            binaryWriterAction: (w, v) =>
            ConvertToBytes_List_Gint_g(w, IntList2,
            includeChildrenMode, v));
            WriteNonLazinatorObject(
            nonLazinatorObject: _IntList3, isBelievedDirty: _IntList3_Accessed,
            isAccessed: _IntList3_Accessed, writer: writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorObjectBytes, _IntList3_ByteIndex, _IntList3_ByteLength),
            verifyCleanness: false,
            binaryWriterAction: (w, v) =>
            ConvertToBytes_List_Gint_g(w, IntList3,
            includeChildrenMode, v));
        }
        
        /* Conversion of supported collections and tuples */
        
        private static List<int> ConvertFromBytes_List_Gint_g(ReadOnlyMemory<byte> storage, DeserializationFactory deserializationFactory, InformParentOfDirtinessDelegate informParentOfDirtinessDelegate)
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
        
        private static void ConvertToBytes_List_Gint_g(BinaryBufferWriter writer, List<int> itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
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
        
    }
}
