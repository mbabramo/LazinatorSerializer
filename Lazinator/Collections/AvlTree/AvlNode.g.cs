//99b46f9d-dc6e-24a2-5eac-527ae2f9e76c
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.77
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Lazinator.Collections.Avl
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
    public sealed partial class AvlNode<TKey, TValue> : ILazinator
    {
        /* Serialization, deserialization, and object relationships */
        
        public AvlNode() : base()
        {
        }
        
        public ILazinator LazinatorParentClass { get; set; }
        
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
            
            LazinatorGenericID = GetGenericIDIfApplicable(ContainsOpenGenericParameters, LazinatorUniqueID, span, ref bytesSoFar);
            
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
            var clone = new AvlNode<TKey, TValue>()
            {
                LazinatorParentClass = LazinatorParentClass,
                InformParentOfDirtinessDelegate = InformParentOfDirtinessDelegate,
                OriginalIncludeChildrenMode = includeChildrenMode,
                HierarchyBytes = bytes
            };
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
            if (_Left_Accessed)
            {
                Left.MarkHierarchyClean();
            }
            if (_Right_Accessed)
            {
                Right.MarkHierarchyClean();
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
        
        /* Field definitions */
        
        int _Key_ByteIndex;
        int _Left_ByteIndex;
        int _Right_ByteIndex;
        int _Value_ByteIndex;
        int _Key_ByteLength => _Left_ByteIndex - _Key_ByteIndex;
        int _Left_ByteLength => _Right_ByteIndex - _Left_ByteIndex;
        int _Right_ByteLength => _Value_ByteIndex - _Right_ByteIndex;
        private int _AvlNode_TKey_TValue_EndByteIndex = 0;
        int _Value_ByteLength => _AvlNode_TKey_TValue_EndByteIndex - _Value_ByteIndex;
        
        private int _Balance;
        public int Balance
        {
            [DebuggerStepThrough]
            get
            {
                return _Balance;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _Balance = value;
            }
        }
        private int _Count;
        public int Count
        {
            [DebuggerStepThrough]
            get
            {
                return _Count;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _Count = value;
            }
        }
        private TKey _Key;
        public TKey Key
        {
            [DebuggerStepThrough]
            get
            {
                if (!_Key_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _Key = default(TKey);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _Key_ByteIndex, _Key_ByteLength, false, false, null);
                        if (childData.Length == 0)
                        {
                            _Key = default;
                        }
                        else _Key = new TKey()
                        {
                            LazinatorParentClass = this,
                            LazinatorObjectBytes = childData,
                        };
                    }
                    _Key_Accessed = true;
                }
                return _Key;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _Key = value;
                _Key_Accessed = true;
            }
        }
        bool _Key_Accessed;
        private AvlNode<TKey, TValue> _Left;
        public AvlNode<TKey, TValue> Left
        {
            [DebuggerStepThrough]
            get
            {
                if (!_Left_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _Left = default(AvlNode<TKey, TValue>);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _Left_ByteIndex, _Left_ByteLength, false, false, null);
                        if (childData.Length == 0)
                        {
                            _Left = default;
                        }
                        else _Left = new AvlNode<TKey, TValue>()
                        {
                            LazinatorParentClass = this,
                            LazinatorObjectBytes = childData,
                        };
                    }
                    _Left_Accessed = true;
                }
                return _Left;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _Left = value;
                if (_Left != null)
                {
                    _Left.IsDirty = true;
                }
                _Left_Accessed = true;
            }
        }
        bool _Left_Accessed;
        private AvlNode<TKey, TValue> _Right;
        public AvlNode<TKey, TValue> Right
        {
            [DebuggerStepThrough]
            get
            {
                if (!_Right_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _Right = default(AvlNode<TKey, TValue>);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _Right_ByteIndex, _Right_ByteLength, false, false, null);
                        if (childData.Length == 0)
                        {
                            _Right = default;
                        }
                        else _Right = new AvlNode<TKey, TValue>()
                        {
                            LazinatorParentClass = this,
                            LazinatorObjectBytes = childData,
                        };
                    }
                    _Right_Accessed = true;
                }
                return _Right;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _Right = value;
                if (_Right != null)
                {
                    _Right.IsDirty = true;
                }
                _Right_Accessed = true;
            }
        }
        bool _Right_Accessed;
        private TValue _Value;
        public TValue Value
        {
            [DebuggerStepThrough]
            get
            {
                if (!_Value_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _Value = default(TValue);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _Value_ByteIndex, _Value_ByteLength, false, false, null);
                        if (childData.Length == 0)
                        {
                            _Value = default;
                        }
                        else _Value = new TValue()
                        {
                            LazinatorParentClass = this,
                            LazinatorObjectBytes = childData,
                        };
                    }
                    _Value_Accessed = true;
                }
                return _Value;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _Value = value;
                _Value_Accessed = true;
            }
        }
        bool _Value_Accessed;
        
        void ResetAccessedProperties()
        {
            _Left_Accessed = _Right_Accessed = false;
        }
        
        /* Conversion */
        
        public int LazinatorUniqueID => 93;
        
        bool ContainsOpenGenericParameters => true;
        System.Collections.Generic.List<int> _LazinatorGenericID { get; set; }
        public System.Collections.Generic.List<int> LazinatorGenericID
        {
            get
            {
                if (_LazinatorGenericID == null)
                {
                    _LazinatorGenericID = DeserializationFactory.Instance.GetUniqueIDListForGenericType(93, new Type[] { typeof(TKey), typeof(TValue) });
                }
                return _LazinatorGenericID;
            }
            set
            {
                _LazinatorGenericID = value;
            }
        }
        
        public int LazinatorObjectVersion { get; set; } = 0;
        
        
        public void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;_Balance = span.ToDecompressedInt(ref bytesSoFar);
            _Count = span.ToDecompressedInt(ref bytesSoFar);
            _Key_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            _Left_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            _Right_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            _Value_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            _AvlNode_TKey_TValue_EndByteIndex = bytesSoFar;
        }
        
        public void SerializeExistingBuffer(BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
            int startPosition = writer.Position;
            WritePropertiesIntoBuffer(writer, includeChildrenMode, verifyCleanness, true);
            
            _IsDirty = false;
            _DescendantIsDirty = includeChildrenMode != IncludeChildrenMode.IncludeAllChildren && ((_Left_Accessed && Left != null && (Left.IsDirty || Left.DescendantIsDirty)) || (_Right_Accessed && Right != null && (Right.IsDirty || Right.DescendantIsDirty)));
            
            _LazinatorObjectBytes = writer.Slice(startPosition);
        }
        void WritePropertiesIntoBuffer(BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool includeUniqueID)
        {
            // header information
            if (includeUniqueID)
            {
                if (LazinatorGenericID == null)
                {
                    CompressedIntegralTypes.WriteCompressedInt(writer, LazinatorUniqueID);
                }
                else
                {
                    WriteLazinatorGenericID(writer, LazinatorGenericID);
                }
            }
            CompressedIntegralTypes.WriteCompressedInt(writer, Lazinator.Support.LazinatorVersionInfo.LazinatorIntVersion);
            CompressedIntegralTypes.WriteCompressedInt(writer, LazinatorObjectVersion);
            writer.Write((byte)includeChildrenMode);
            // write properties
            CompressedIntegralTypes.WriteCompressedInt(writer, _Balance);
            CompressedIntegralTypes.WriteCompressedInt(writer, _Count);
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                WriteChildWithLength(writer, _Key, includeChildrenMode, _Key_Accessed, () => GetChildSlice(LazinatorObjectBytes, _Key_ByteIndex, _Key_ByteLength, false, false, null), verifyCleanness, false, false, this);
            }
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                WriteChildWithLength(writer, _Left, includeChildrenMode, _Left_Accessed, () => GetChildSlice(LazinatorObjectBytes, _Left_ByteIndex, _Left_ByteLength, false, false, null), verifyCleanness, false, false, this);
            }
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                WriteChildWithLength(writer, _Right, includeChildrenMode, _Right_Accessed, () => GetChildSlice(LazinatorObjectBytes, _Right_ByteIndex, _Right_ByteLength, false, false, null), verifyCleanness, false, false, this);
            }
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                WriteChildWithLength(writer, _Value, includeChildrenMode, _Value_Accessed, () => GetChildSlice(LazinatorObjectBytes, _Value_ByteIndex, _Value_ByteLength, false, false, null), verifyCleanness, false, false, this);
            }
        }
        
    }
}
