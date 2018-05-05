//78483076-f237-48be-a07a-ea85062b53c3
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

namespace Lazinator.Collections.Avl
{
    public sealed partial class AvlNode<TKey, TValue> : ILazinator
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
            var clone = new AvlNode<TKey, TValue>()
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
            if (!IsDirty && !DescendantIsDirty)
            {
                return;
            }
            MemoryInBuffer bytes = EncodeOrRecycleToNewBuffer(IncludeChildrenMode.IncludeAllChildren, OriginalIncludeChildrenMode, false, false, IsDirty, DescendantIsDirty, false, LazinatorObjectBytes, (StreamManuallyDelegate)EncodeToNewBuffer);
            _IsDirty = false;
            LazinatorObjectBytes = bytes.FilledMemory;
            _Left_Accessed = _Right_Accessed = false;
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
        
        internal int _Left_ByteIndex;
        internal int _Right_ByteIndex;
        internal int _Key_ByteIndex;
        internal int _Value_ByteIndex;
        internal int _Left_ByteLength => _Right_ByteIndex - _Left_ByteIndex;
        internal int _Right_ByteLength => _Key_ByteIndex - _Right_ByteIndex;
        internal int _Key_ByteLength => _Value_ByteIndex - _Key_ByteIndex;
        internal int _Value_ByteLength => LazinatorObjectBytes.Length - _Value_ByteIndex;
        
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
        private Lazinator.Collections.Avl.AvlNode<TKey, TValue> _Left;
        public Lazinator.Collections.Avl.AvlNode<TKey, TValue> Left
        {
            [DebuggerStepThrough]
            get
            {
                if (!_Left_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _Left = default(Lazinator.Collections.Avl.AvlNode<TKey, TValue>);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _Left_ByteIndex, _Left_ByteLength);
                        if (childData.Length == 0)
                        {
                            _Left = default;
                        }
                        else _Left = new Lazinator.Collections.Avl.AvlNode<TKey, TValue>()
                        {
                            DeserializationFactory = DeserializationFactory,
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
        internal bool _Left_Accessed;
        private Lazinator.Collections.Avl.AvlNode<TKey, TValue> _Right;
        public Lazinator.Collections.Avl.AvlNode<TKey, TValue> Right
        {
            [DebuggerStepThrough]
            get
            {
                if (!_Right_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _Right = default(Lazinator.Collections.Avl.AvlNode<TKey, TValue>);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _Right_ByteIndex, _Right_ByteLength);
                        if (childData.Length == 0)
                        {
                            _Right = default;
                        }
                        else _Right = new Lazinator.Collections.Avl.AvlNode<TKey, TValue>()
                        {
                            DeserializationFactory = DeserializationFactory,
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
        internal bool _Right_Accessed;
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
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _Key_ByteIndex, _Key_ByteLength);
                        if (childData.Length == 0)
                        {
                            _Key = default;
                        }
                        else _Key = new TKey()
                        {
                            DeserializationFactory = DeserializationFactory,
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
        internal bool _Key_Accessed;
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
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _Value_ByteIndex, _Value_ByteLength);
                        if (childData.Length == 0)
                        {
                            _Value = default;
                        }
                        else _Value = new TValue()
                        {
                            DeserializationFactory = DeserializationFactory,
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
        internal bool _Value_Accessed;
        
        /* Conversion */
        
        public int LazinatorUniqueID => 93;
        
        public int LazinatorObjectVersion { get; set; } = 0;
        
        public void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
            _Balance = span.ToDecompressedInt(ref bytesSoFar);
            _Left_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren) 
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            _Right_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren) 
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            _Key_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren) 
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            _Value_ByteIndex = bytesSoFar;
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
            CompressedIntegralTypes.WriteCompressedInt(writer, _Balance);
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren) 
            {
                WriteChildWithLength(writer, _Left, includeChildrenMode, _Left_Accessed, () => GetChildSlice(LazinatorObjectBytes, _Left_ByteIndex, _Left_ByteLength), verifyCleanness, false);
            }
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren) 
            {
                WriteChildWithLength(writer, _Right, includeChildrenMode, _Right_Accessed, () => GetChildSlice(LazinatorObjectBytes, _Right_ByteIndex, _Right_ByteLength), verifyCleanness, false);
            }
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren) 
            {
                WriteChildWithLength(writer, _Key, includeChildrenMode, _Key_Accessed, () => GetChildSlice(LazinatorObjectBytes, _Key_ByteIndex, _Key_ByteLength), verifyCleanness, false);
            }
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren) 
            {
                WriteChildWithLength(writer, _Value, includeChildrenMode, _Value_Accessed, () => GetChildSlice(LazinatorObjectBytes, _Value_ByteIndex, _Value_ByteLength), verifyCleanness, false);
            }
        }
        
    }
}
