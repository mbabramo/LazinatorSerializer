//c039faa3-304e-7514-fa46-3ad7a067a1fa
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.34
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
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.InteropServices;
    using static Lazinator.Core.LazinatorUtilities;
    
    [Autogenerated]
    public partial class ExampleChild : ILazinator
    {
        /* Serialization, deserialization, and object relationships */
        
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
            var clone = new ExampleChild()
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
        
        /* Field definitions */
        
        protected int _ByteSpan_ByteIndex;
        private int _ExampleChild_EndByteIndex;
        protected virtual int _ByteSpan_ByteLength => _ExampleChild_EndByteIndex - _ByteSpan_ByteIndex;
        
        private long _MyLong;
        public long MyLong
        {
            [DebuggerStepThrough]
            get
            {
                return _MyLong;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _MyLong = value;
            }
        }
        private short _MyShort;
        public short MyShort
        {
            [DebuggerStepThrough]
            get
            {
                return _MyShort;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _MyShort = value;
            }
        }
        private ReadOnlyMemory<byte> _ByteSpan;
        public ReadOnlySpan<byte> ByteSpan
        {
            [DebuggerStepThrough]
            get
            {
                if (!_ByteSpan_Accessed)
                {
                    ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _ByteSpan_ByteIndex, _ByteSpan_ByteLength);
                    _ByteSpan = childData;
                    _ByteSpan_Accessed = true;
                }
                return _ByteSpan.Span;
            }
            [DebuggerStepThrough]
            set
            {
                
                IsDirty = true;
                _ByteSpan = new ReadOnlyMemory<byte>(MemoryMarshal.Cast<byte, byte>(value).ToArray());
                _ByteSpan_Accessed = true;
            }
        }
        protected bool _ByteSpan_Accessed;
        
        /* Conversion */
        
        public virtual int LazinatorUniqueID => 213;
        
        public virtual int LazinatorObjectVersion { get; set; } = 0;
        
        public virtual void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
            _MyLong = span.ToDecompressedLong(ref bytesSoFar);
            _MyShort = span.ToDecompressedShort(ref bytesSoFar);
            _ByteSpan_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            _ExampleChild_EndByteIndex = bytesSoFar;
        }
        
        public virtual void SerializeExistingBuffer(BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
            // header information
            CompressedIntegralTypes.WriteCompressedInt(writer, LazinatorUniqueID);
            CompressedIntegralTypes.WriteCompressedInt(writer, Lazinator.Support.LazinatorVersionInfo.LazinatorIntVersion);
            CompressedIntegralTypes.WriteCompressedInt(writer, LazinatorObjectVersion);
            writer.Write((byte)includeChildrenMode);
            // write properties
            CompressedIntegralTypes.WriteCompressedLong(writer, _MyLong);
            CompressedIntegralTypes.WriteCompressedShort(writer, _MyShort);
            WriteNonLazinatorObject(
            nonLazinatorObject: _ByteSpan, isBelievedDirty: _ByteSpan_Accessed,
            isAccessed: _ByteSpan_Accessed, writer: writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorObjectBytes, _ByteSpan_ByteIndex, _ByteSpan_ByteLength),
            verifyCleanness: false,
            binaryWriterAction: (w, v) =>
            ConvertToBytes_ReadOnlySpan_Gbyte_g(w, ByteSpan,
            includeChildrenMode, v));
        }
        
        /* Conversion of supported collections and tuples */
        
        private static void ConvertToBytes_ReadOnlySpan_Gbyte_g(BinaryBufferWriter writer, ReadOnlySpan<byte> itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
            ReadOnlySpan<byte> toConvert = MemoryMarshal.Cast<byte, byte>(itemToConvert);
            for (int i = 0; i < toConvert.Length; i++)
            {
                writer.Write(toConvert[i]);
            }
        }
        
    }
}
