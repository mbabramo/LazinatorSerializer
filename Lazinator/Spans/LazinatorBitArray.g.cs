//63d1e651-9885-a4eb-ed09-9f5ee27a7751
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

namespace Lazinator.Spans
{
    public sealed partial class LazinatorBitArray : ILazinator
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
            var clone = new LazinatorBitArray()
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
            if (!IsDirty)
            {
                return;
            }
            MemoryInBuffer bytes = EncodeOrRecycleToNewBuffer(IncludeChildrenMode.IncludeAllChildren, OriginalIncludeChildrenMode, false, false, IsDirty, DescendantIsDirty, false, LazinatorObjectBytes, (StreamManuallyDelegate)EncodeToNewBuffer);
            _IsDirty = false;
            LazinatorObjectBytes = bytes.FilledMemory;
            _ByteSpan_Accessed = false;
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
        
        internal int _ByteSpan_ByteIndex;
        internal int _ByteSpan_ByteLength => LazinatorObjectBytes.Length - _ByteSpan_ByteIndex;
        
        private int __version;
        private int _version
        {
            [DebuggerStepThrough]
            get
            {
                return __version;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                __version = value;
            }
        }
        private int _m_length;
        private int m_length
        {
            [DebuggerStepThrough]
            get
            {
                return _m_length;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _m_length = value;
            }
        }
        private Lazinator.Spans.LazinatorByteSpan _ByteSpan;
        private Lazinator.Spans.LazinatorByteSpan ByteSpan
        {
            [DebuggerStepThrough]
            get
            {
                if (!_ByteSpan_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _ByteSpan = default(Lazinator.Spans.LazinatorByteSpan);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _ByteSpan_ByteIndex, _ByteSpan_ByteLength);
                        if (childData.Length == 0)
                        {
                            _ByteSpan = default;
                        }
                        else _ByteSpan = new Lazinator.Spans.LazinatorByteSpan()
                        {
                            DeserializationFactory = DeserializationFactory,
                            LazinatorParentClass = this,
                            LazinatorObjectBytes = childData,
                        };
                    }
                    _ByteSpan_Accessed = true;
                }
                return _ByteSpan;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _ByteSpan = value;
                if (_ByteSpan != null)
                {
                    _ByteSpan.IsDirty = true;
                }
                _ByteSpan_Accessed = true;
            }
        }
        internal bool _ByteSpan_Accessed;
        
        /* Conversion */
        
        public int LazinatorUniqueID => 91;
        
        public int LazinatorObjectVersion { get; set; } = 0;
        
        public void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
            __version = span.ToDecompressedInt(ref bytesSoFar);
            _m_length = span.ToDecompressedInt(ref bytesSoFar);
            _ByteSpan_ByteIndex = bytesSoFar;
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
            CompressedIntegralTypes.WriteCompressedInt(writer, __version);
            CompressedIntegralTypes.WriteCompressedInt(writer, _m_length);
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren) 
            {
                WriteChildWithLength(writer, _ByteSpan, includeChildrenMode, _ByteSpan_Accessed, () => GetChildSlice(LazinatorObjectBytes, _ByteSpan_ByteIndex, _ByteSpan_ByteLength), verifyCleanness);
            }
        }
        
    }
}
