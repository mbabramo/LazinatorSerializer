//cbb9ed32-7e5a-91e9-f959-0d06604625ef
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.28
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Lazinator.Collections
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
    
    
    public sealed partial class LazinatorOffsetList : ILazinator
    {
        /* Boilerplate for every non-abstract ILazinator object */
        
        public ILazinator LazinatorParentClass { get; set; }
        
        IncludeChildrenMode OriginalIncludeChildrenMode;
        
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
            PostDeserialization();
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
            var clone = new LazinatorOffsetList()
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
            {
                InformParentOfDirtinessDelegate();
            }
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
        
        public void MarkHierarchyClean()
        {
            _IsDirty = false;
            _DescendantIsDirty = false;
            if (_FourByteItems_Accessed)
            {
                FourByteItems.MarkHierarchyClean();
            }
            if (_TwoByteItems_Accessed)
            {
                TwoByteItems.MarkHierarchyClean();
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
            _FourByteItems_Accessed = _TwoByteItems_Accessed = false;
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
        
        /* Field boilerplate */
        
        int _FourByteItems_ByteIndex;
        int _TwoByteItems_ByteIndex;
        int _FourByteItems_ByteLength => _TwoByteItems_ByteIndex - _FourByteItems_ByteIndex;
        private int _LazinatorOffsetList_EndByteIndex;
        int _TwoByteItems_ByteLength => _LazinatorOffsetList_EndByteIndex - _TwoByteItems_ByteIndex;
        
        private Lazinator.Collections.LazinatorFastReadList<int> _FourByteItems;
        public Lazinator.Collections.LazinatorFastReadList<int> FourByteItems
        {
            [DebuggerStepThrough]
            get
            {
                if (!_FourByteItems_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _FourByteItems = default(Lazinator.Collections.LazinatorFastReadList<int>);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _FourByteItems_ByteIndex, _FourByteItems_ByteLength);
                        if (childData.Length == 0)
                        {
                            _FourByteItems = default;
                        }
                        else _FourByteItems = new Lazinator.Collections.LazinatorFastReadList<int>()
                        {
                            DeserializationFactory = DeserializationFactory,
                            LazinatorParentClass = this,
                            LazinatorObjectBytes = childData,
                        };
                    }
                    _FourByteItems_Accessed = true;
                }
                return _FourByteItems;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _FourByteItems = value;
                if (_FourByteItems != null)
                {
                    _FourByteItems.IsDirty = true;
                }
                _FourByteItems_Accessed = true;
            }
        }
        bool _FourByteItems_Accessed;
        private Lazinator.Collections.LazinatorFastReadList<short> _TwoByteItems;
        public Lazinator.Collections.LazinatorFastReadList<short> TwoByteItems
        {
            [DebuggerStepThrough]
            get
            {
                if (!_TwoByteItems_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _TwoByteItems = default(Lazinator.Collections.LazinatorFastReadList<short>);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _TwoByteItems_ByteIndex, _TwoByteItems_ByteLength);
                        if (childData.Length == 0)
                        {
                            _TwoByteItems = default;
                        }
                        else _TwoByteItems = new Lazinator.Collections.LazinatorFastReadList<short>()
                        {
                            DeserializationFactory = DeserializationFactory,
                            LazinatorParentClass = this,
                            LazinatorObjectBytes = childData,
                        };
                    }
                    _TwoByteItems_Accessed = true;
                }
                return _TwoByteItems;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _TwoByteItems = value;
                if (_TwoByteItems != null)
                {
                    _TwoByteItems.IsDirty = true;
                }
                _TwoByteItems_Accessed = true;
            }
        }
        bool _TwoByteItems_Accessed;
        
        /* Conversion */
        
        public int LazinatorUniqueID => 50;
        
        public int LazinatorObjectVersion { get; set; } = 0;
        
        public void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
            _FourByteItems_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            _TwoByteItems_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            _LazinatorOffsetList_EndByteIndex = bytesSoFar;
        }
        
        public void SerializeExistingBuffer(BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
            // header information
            CompressedIntegralTypes.WriteCompressedInt(writer, LazinatorUniqueID);
            CompressedIntegralTypes.WriteCompressedInt(writer, Lazinator.Support.LazinatorVersionInfo.LazinatorIntVersion);
            CompressedIntegralTypes.WriteCompressedInt(writer, LazinatorObjectVersion);
            writer.Write((byte)includeChildrenMode);
            // write properties
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                WriteChildWithLength(writer, _FourByteItems, includeChildrenMode, _FourByteItems_Accessed, () => GetChildSlice(LazinatorObjectBytes, _FourByteItems_ByteIndex, _FourByteItems_ByteLength), verifyCleanness, false);
            }
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                WriteChildWithLength(writer, _TwoByteItems, includeChildrenMode, _TwoByteItems_Accessed, () => GetChildSlice(LazinatorObjectBytes, _TwoByteItems_ByteIndex, _TwoByteItems_ByteLength), verifyCleanness, false);
            }
        }
        
    }
}
