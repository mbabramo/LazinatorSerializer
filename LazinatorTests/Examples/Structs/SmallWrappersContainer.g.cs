//27d2f31d-35b4-ca1d-41cf-36627d7945e6
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.72
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LazinatorTests.Examples.Structs
{
    using Lazinator.Attributes;
    using Lazinator.Buffers;
    using Lazinator.Collections;
    using Lazinator.Core;
    using Lazinator.Exceptions;
    using Lazinator.Support;
    using Lazinator.Wrappers;
    using System;
    using System.Buffers;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.InteropServices;
    using static Lazinator.Core.LazinatorUtilities;
    
    [Autogenerated]
    public partial class SmallWrappersContainer : ILazinator
    {
        /* Serialization, deserialization, and object relationships */
        
        public SmallWrappersContainer() : base()
        {
        }
        
        public virtual ILazinator LazinatorParentClass { get; set; }
        
        protected IncludeChildrenMode OriginalIncludeChildrenMode;
        
        public virtual int Deserialize()
        {
            ResetAccessedProperties();
            int bytesSoFar = 0;
            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
            if (span.Length == 0)
            {
                return 0;
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
            return bytesSoFar;
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
            var clone = new SmallWrappersContainer()
            {
                DeserializationFactory = DeserializationFactory,
                LazinatorParentClass = LazinatorParentClass,
                InformParentOfDirtinessDelegate = InformParentOfDirtinessDelegate,
                OriginalIncludeChildrenMode = includeChildrenMode,
                HierarchyBytes = bytes
            };
            return clone;
        }
        
        protected bool _IsDirty;
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
        
        protected bool _DescendantIsDirty;
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
            if (_ListWrappedBytes_Accessed)
            {
                ListWrappedBytes.MarkHierarchyClean();
            }
            if (_WrappedBool_Accessed)
            {
                WrappedBool.MarkHierarchyClean();
            }
            if (_WrappedByte_Accessed)
            {
                WrappedByte.MarkHierarchyClean();
            }
            if (_WrappedChar_Accessed)
            {
                WrappedChar.MarkHierarchyClean();
            }
            if (_WrappedNullableBool_Accessed)
            {
                WrappedNullableBool.MarkHierarchyClean();
            }
            if (_WrappedNullableByte_Accessed)
            {
                WrappedNullableByte.MarkHierarchyClean();
            }
            if (_WrappedNullableChar_Accessed)
            {
                WrappedNullableChar.MarkHierarchyClean();
            }
            if (_WrappedNullableSByte_Accessed)
            {
                WrappedNullableSByte.MarkHierarchyClean();
            }
            if (_WrappedSByte_Accessed)
            {
                WrappedSByte.MarkHierarchyClean();
            }
        }
        
        public virtual DeserializationFactory DeserializationFactory { get; set; }
        
        private MemoryInBuffer _HierarchyBytes;
        public virtual MemoryInBuffer HierarchyBytes
        {
            set
            {
                _HierarchyBytes = value;
                LazinatorObjectBytes = value.FilledMemory;
            }
        }
        
        protected ReadOnlyMemory<byte> _LazinatorObjectBytes;
        public virtual ReadOnlyMemory<byte> LazinatorObjectBytes
        {
            get => _LazinatorObjectBytes;
            set
            {
                _LazinatorObjectBytes = value;
                int length = Deserialize();
                _LazinatorObjectBytes = _LazinatorObjectBytes.Slice(0, length);
            }
        }
        
        public virtual void LazinatorConvertToBytes()
        {
            if (!IsDirty && !DescendantIsDirty && _LazinatorObjectBytes.Length > 0)
            {
                return;
            }
            MemoryInBuffer bytes = EncodeOrRecycleToNewBuffer(IncludeChildrenMode.IncludeAllChildren, OriginalIncludeChildrenMode, false, false, IsDirty, DescendantIsDirty, false, LazinatorObjectBytes, (StreamManuallyDelegate)EncodeToNewBuffer);
            _LazinatorObjectBytes = bytes.FilledMemory;
        }
        
        public virtual int GetByteLength()
        {
            LazinatorConvertToBytes();
            return _LazinatorObjectBytes.Length;
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
        
        public virtual Guid GetBinaryHashCode128()
        {
            LazinatorConvertToBytes();
            return FarmhashByteSpans.Hash128(LazinatorObjectBytes.Span);
        }
        
        /* Field definitions */
        
        protected int _ListWrappedBytes_ByteIndex;
        protected int _WrappedBool_ByteIndex;
        protected int _WrappedByte_ByteIndex;
        protected int _WrappedChar_ByteIndex;
        protected int _WrappedNullableBool_ByteIndex;
        protected int _WrappedNullableByte_ByteIndex;
        protected int _WrappedNullableChar_ByteIndex;
        protected int _WrappedNullableSByte_ByteIndex;
        protected int _WrappedSByte_ByteIndex;
        protected virtual int _ListWrappedBytes_ByteLength => _WrappedBool_ByteIndex - _ListWrappedBytes_ByteIndex;
        protected virtual int _WrappedBool_ByteLength => _WrappedByte_ByteIndex - _WrappedBool_ByteIndex;
        protected virtual int _WrappedByte_ByteLength => _WrappedChar_ByteIndex - _WrappedByte_ByteIndex;
        protected virtual int _WrappedChar_ByteLength => _WrappedNullableBool_ByteIndex - _WrappedChar_ByteIndex;
        protected virtual int _WrappedNullableBool_ByteLength => _WrappedNullableByte_ByteIndex - _WrappedNullableBool_ByteIndex;
        protected virtual int _WrappedNullableByte_ByteLength => _WrappedNullableChar_ByteIndex - _WrappedNullableByte_ByteIndex;
        protected virtual int _WrappedNullableChar_ByteLength => _WrappedNullableSByte_ByteIndex - _WrappedNullableChar_ByteIndex;
        protected virtual int _WrappedNullableSByte_ByteLength => _WrappedSByte_ByteIndex - _WrappedNullableSByte_ByteIndex;
        private int _SmallWrappersContainer_EndByteIndex;
        protected virtual int _WrappedSByte_ByteLength => _SmallWrappersContainer_EndByteIndex - _WrappedSByte_ByteIndex;
        
        private LazinatorList<WByte> _ListWrappedBytes;
        public LazinatorList<WByte> ListWrappedBytes
        {
            [DebuggerStepThrough]
            get
            {
                if (!_ListWrappedBytes_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _ListWrappedBytes = default(LazinatorList<WByte>);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _ListWrappedBytes_ByteIndex, _ListWrappedBytes_ByteLength);
                        
                        if (DeserializationFactory == null)
                        {
                            DeserializationFactory = DeserializationFactory.GetInstance();
                        }
                        _ListWrappedBytes = DeserializationFactory.FactoryCreateBaseOrDerivedType(51, () => new LazinatorList<WByte>(), childData, this); 
                    }
                    _ListWrappedBytes_Accessed = true;
                }
                return _ListWrappedBytes;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _ListWrappedBytes = value;
                if (_ListWrappedBytes != null)
                {
                    _ListWrappedBytes.IsDirty = true;
                }
                _ListWrappedBytes_Accessed = true;
            }
        }
        protected bool _ListWrappedBytes_Accessed;
        private WBool _WrappedBool;
        public WBool WrappedBool
        {
            [DebuggerStepThrough]
            get
            {
                if (!_WrappedBool_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _WrappedBool = default(WBool);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _WrappedBool_ByteIndex, _WrappedBool_ByteLength, true, 1);
                        _WrappedBool = new WBool()
                        {
                            DeserializationFactory = DeserializationFactory,
                            LazinatorParentClass = this,
                            LazinatorObjectBytes = childData,
                        };
                    }
                    _WrappedBool_Accessed = true;
                }
                return _WrappedBool;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _WrappedBool = value;
                _WrappedBool_Accessed = true;
            }
        }
        protected bool _WrappedBool_Accessed;
        public WBool WrappedBool_Copy
        {
            [DebuggerStepThrough]
            get
            {
                if (!_WrappedBool_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        return default(WBool);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _WrappedBool_ByteIndex, _WrappedBool_ByteLength);
                        return new WBool()
                        {
                            DeserializationFactory = DeserializationFactory,
                            LazinatorObjectBytes = childData,
                        };
                    }
                }
                return _WrappedBool;
            }
        }
        private WByte _WrappedByte;
        public WByte WrappedByte
        {
            [DebuggerStepThrough]
            get
            {
                if (!_WrappedByte_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _WrappedByte = default(WByte);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _WrappedByte_ByteIndex, _WrappedByte_ByteLength, true, 1);
                        _WrappedByte = new WByte()
                        {
                            DeserializationFactory = DeserializationFactory,
                            LazinatorParentClass = this,
                            LazinatorObjectBytes = childData,
                        };
                    }
                    _WrappedByte_Accessed = true;
                }
                return _WrappedByte;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _WrappedByte = value;
                _WrappedByte_Accessed = true;
            }
        }
        protected bool _WrappedByte_Accessed;
        public WByte WrappedByte_Copy
        {
            [DebuggerStepThrough]
            get
            {
                if (!_WrappedByte_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        return default(WByte);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _WrappedByte_ByteIndex, _WrappedByte_ByteLength);
                        return new WByte()
                        {
                            DeserializationFactory = DeserializationFactory,
                            LazinatorObjectBytes = childData,
                        };
                    }
                }
                return _WrappedByte;
            }
        }
        private WChar _WrappedChar;
        public WChar WrappedChar
        {
            [DebuggerStepThrough]
            get
            {
                if (!_WrappedChar_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _WrappedChar = default(WChar);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _WrappedChar_ByteIndex, _WrappedChar_ByteLength, true, 2);
                        _WrappedChar = new WChar()
                        {
                            DeserializationFactory = DeserializationFactory,
                            LazinatorParentClass = this,
                            LazinatorObjectBytes = childData,
                        };
                    }
                    _WrappedChar_Accessed = true;
                }
                return _WrappedChar;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _WrappedChar = value;
                _WrappedChar_Accessed = true;
            }
        }
        protected bool _WrappedChar_Accessed;
        public WChar WrappedChar_Copy
        {
            [DebuggerStepThrough]
            get
            {
                if (!_WrappedChar_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        return default(WChar);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _WrappedChar_ByteIndex, _WrappedChar_ByteLength);
                        return new WChar()
                        {
                            DeserializationFactory = DeserializationFactory,
                            LazinatorObjectBytes = childData,
                        };
                    }
                }
                return _WrappedChar;
            }
        }
        private WNullableBool _WrappedNullableBool;
        public WNullableBool WrappedNullableBool
        {
            [DebuggerStepThrough]
            get
            {
                if (!_WrappedNullableBool_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _WrappedNullableBool = default(WNullableBool);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _WrappedNullableBool_ByteIndex, _WrappedNullableBool_ByteLength, true, 1);
                        _WrappedNullableBool = new WNullableBool()
                        {
                            DeserializationFactory = DeserializationFactory,
                            LazinatorParentClass = this,
                            LazinatorObjectBytes = childData,
                        };
                    }
                    _WrappedNullableBool_Accessed = true;
                }
                return _WrappedNullableBool;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _WrappedNullableBool = value;
                _WrappedNullableBool_Accessed = true;
            }
        }
        protected bool _WrappedNullableBool_Accessed;
        public WNullableBool WrappedNullableBool_Copy
        {
            [DebuggerStepThrough]
            get
            {
                if (!_WrappedNullableBool_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        return default(WNullableBool);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _WrappedNullableBool_ByteIndex, _WrappedNullableBool_ByteLength);
                        return new WNullableBool()
                        {
                            DeserializationFactory = DeserializationFactory,
                            LazinatorObjectBytes = childData,
                        };
                    }
                }
                return _WrappedNullableBool;
            }
        }
        private WNullableByte _WrappedNullableByte;
        public WNullableByte WrappedNullableByte
        {
            [DebuggerStepThrough]
            get
            {
                if (!_WrappedNullableByte_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _WrappedNullableByte = default(WNullableByte);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _WrappedNullableByte_ByteIndex, _WrappedNullableByte_ByteLength, true);
                        _WrappedNullableByte = new WNullableByte()
                        {
                            DeserializationFactory = DeserializationFactory,
                            LazinatorParentClass = this,
                            LazinatorObjectBytes = childData,
                        };
                    }
                    _WrappedNullableByte_Accessed = true;
                }
                return _WrappedNullableByte;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _WrappedNullableByte = value;
                _WrappedNullableByte_Accessed = true;
            }
        }
        protected bool _WrappedNullableByte_Accessed;
        public WNullableByte WrappedNullableByte_Copy
        {
            [DebuggerStepThrough]
            get
            {
                if (!_WrappedNullableByte_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        return default(WNullableByte);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _WrappedNullableByte_ByteIndex, _WrappedNullableByte_ByteLength);
                        return new WNullableByte()
                        {
                            DeserializationFactory = DeserializationFactory,
                            LazinatorObjectBytes = childData,
                        };
                    }
                }
                return _WrappedNullableByte;
            }
        }
        private WNullableChar _WrappedNullableChar;
        public WNullableChar WrappedNullableChar
        {
            [DebuggerStepThrough]
            get
            {
                if (!_WrappedNullableChar_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _WrappedNullableChar = default(WNullableChar);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _WrappedNullableChar_ByteIndex, _WrappedNullableChar_ByteLength, true);
                        _WrappedNullableChar = new WNullableChar()
                        {
                            DeserializationFactory = DeserializationFactory,
                            LazinatorParentClass = this,
                            LazinatorObjectBytes = childData,
                        };
                    }
                    _WrappedNullableChar_Accessed = true;
                }
                return _WrappedNullableChar;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _WrappedNullableChar = value;
                _WrappedNullableChar_Accessed = true;
            }
        }
        protected bool _WrappedNullableChar_Accessed;
        public WNullableChar WrappedNullableChar_Copy
        {
            [DebuggerStepThrough]
            get
            {
                if (!_WrappedNullableChar_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        return default(WNullableChar);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _WrappedNullableChar_ByteIndex, _WrappedNullableChar_ByteLength);
                        return new WNullableChar()
                        {
                            DeserializationFactory = DeserializationFactory,
                            LazinatorObjectBytes = childData,
                        };
                    }
                }
                return _WrappedNullableChar;
            }
        }
        private WNullableSByte _WrappedNullableSByte;
        public WNullableSByte WrappedNullableSByte
        {
            [DebuggerStepThrough]
            get
            {
                if (!_WrappedNullableSByte_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _WrappedNullableSByte = default(WNullableSByte);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _WrappedNullableSByte_ByteIndex, _WrappedNullableSByte_ByteLength, true);
                        _WrappedNullableSByte = new WNullableSByte()
                        {
                            DeserializationFactory = DeserializationFactory,
                            LazinatorParentClass = this,
                            LazinatorObjectBytes = childData,
                        };
                    }
                    _WrappedNullableSByte_Accessed = true;
                }
                return _WrappedNullableSByte;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _WrappedNullableSByte = value;
                _WrappedNullableSByte_Accessed = true;
            }
        }
        protected bool _WrappedNullableSByte_Accessed;
        public WNullableSByte WrappedNullableSByte_Copy
        {
            [DebuggerStepThrough]
            get
            {
                if (!_WrappedNullableSByte_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        return default(WNullableSByte);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _WrappedNullableSByte_ByteIndex, _WrappedNullableSByte_ByteLength);
                        return new WNullableSByte()
                        {
                            DeserializationFactory = DeserializationFactory,
                            LazinatorObjectBytes = childData,
                        };
                    }
                }
                return _WrappedNullableSByte;
            }
        }
        private WSByte _WrappedSByte;
        public WSByte WrappedSByte
        {
            [DebuggerStepThrough]
            get
            {
                if (!_WrappedSByte_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _WrappedSByte = default(WSByte);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _WrappedSByte_ByteIndex, _WrappedSByte_ByteLength, true, 1);
                        _WrappedSByte = new WSByte()
                        {
                            DeserializationFactory = DeserializationFactory,
                            LazinatorParentClass = this,
                            LazinatorObjectBytes = childData,
                        };
                    }
                    _WrappedSByte_Accessed = true;
                }
                return _WrappedSByte;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _WrappedSByte = value;
                _WrappedSByte_Accessed = true;
            }
        }
        protected bool _WrappedSByte_Accessed;
        public WSByte WrappedSByte_Copy
        {
            [DebuggerStepThrough]
            get
            {
                if (!_WrappedSByte_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        return default(WSByte);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _WrappedSByte_ByteIndex, _WrappedSByte_ByteLength);
                        return new WSByte()
                        {
                            DeserializationFactory = DeserializationFactory,
                            LazinatorObjectBytes = childData,
                        };
                    }
                }
                return _WrappedSByte;
            }
        }
        
        protected virtual void ResetAccessedProperties()
        {
            _ListWrappedBytes_Accessed = _WrappedBool_Accessed = _WrappedByte_Accessed = _WrappedChar_Accessed = _WrappedNullableBool_Accessed = _WrappedNullableByte_Accessed = _WrappedNullableChar_Accessed = _WrappedNullableSByte_Accessed = _WrappedSByte_Accessed = false;
        }
        
        /* Conversion */
        
        public virtual int LazinatorUniqueID => 262;
        
        public virtual int LazinatorObjectVersion { get; set; } = 0;
        
        public virtual void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
            _ListWrappedBytes_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            _WrappedBool_ByteIndex = bytesSoFar++;
            _WrappedByte_ByteIndex = bytesSoFar++;
            _WrappedChar_ByteIndex = bytesSoFar;
            bytesSoFar += 2;
            _WrappedNullableBool_ByteIndex = bytesSoFar++;
            _WrappedNullableByte_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                bytesSoFar = span.ToByte(ref bytesSoFar) + bytesSoFar;
            }
            _WrappedNullableChar_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                bytesSoFar = span.ToByte(ref bytesSoFar) + bytesSoFar;
            }
            _WrappedNullableSByte_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                bytesSoFar = span.ToByte(ref bytesSoFar) + bytesSoFar;
            }
            _WrappedSByte_ByteIndex = bytesSoFar++;
            _SmallWrappersContainer_EndByteIndex = bytesSoFar;
        }
        
        public virtual void SerializeExistingBuffer(BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
            int startPosition = writer.Position;
            WritePropertiesIntoBuffer(writer, includeChildrenMode, verifyCleanness, true);
            
            _IsDirty = false;
            _DescendantIsDirty = includeChildrenMode != IncludeChildrenMode.IncludeAllChildren && ((_ListWrappedBytes_Accessed && ListWrappedBytes != null && (ListWrappedBytes.IsDirty || ListWrappedBytes.DescendantIsDirty)) || (_WrappedBool_Accessed && (WrappedBool.IsDirty || WrappedBool.DescendantIsDirty)) || (_WrappedByte_Accessed && (WrappedByte.IsDirty || WrappedByte.DescendantIsDirty)) || (_WrappedChar_Accessed && (WrappedChar.IsDirty || WrappedChar.DescendantIsDirty)) || (_WrappedNullableBool_Accessed && (WrappedNullableBool.IsDirty || WrappedNullableBool.DescendantIsDirty)) || (_WrappedNullableByte_Accessed && (WrappedNullableByte.IsDirty || WrappedNullableByte.DescendantIsDirty)) || (_WrappedNullableChar_Accessed && (WrappedNullableChar.IsDirty || WrappedNullableChar.DescendantIsDirty)) || (_WrappedNullableSByte_Accessed && (WrappedNullableSByte.IsDirty || WrappedNullableSByte.DescendantIsDirty)) || (_WrappedSByte_Accessed && (WrappedSByte.IsDirty || WrappedSByte.DescendantIsDirty)));
            
            _LazinatorObjectBytes = writer.Slice(startPosition);
        }
        protected virtual void WritePropertiesIntoBuffer(BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool includeUniqueID)
        {
            // header information
            if (includeUniqueID)
            {
                CompressedIntegralTypes.WriteCompressedInt(writer, LazinatorUniqueID);
            }
            CompressedIntegralTypes.WriteCompressedInt(writer, Lazinator.Support.LazinatorVersionInfo.LazinatorIntVersion);
            CompressedIntegralTypes.WriteCompressedInt(writer, LazinatorObjectVersion);
            writer.Write((byte)includeChildrenMode);
            // write properties
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                WriteChildWithLength(writer, _ListWrappedBytes, includeChildrenMode, _ListWrappedBytes_Accessed, () => GetChildSlice(LazinatorObjectBytes, _ListWrappedBytes_ByteIndex, _ListWrappedBytes_ByteLength), verifyCleanness, false, false, this);
            }
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                WriteChildWithLength(writer, _WrappedBool, includeChildrenMode, _WrappedBool_Accessed, () => GetChildSlice(LazinatorObjectBytes, _WrappedBool_ByteIndex, _WrappedBool_ByteLength), verifyCleanness, true, true, this);
            }
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                WriteChildWithLength(writer, _WrappedByte, includeChildrenMode, _WrappedByte_Accessed, () => GetChildSlice(LazinatorObjectBytes, _WrappedByte_ByteIndex, _WrappedByte_ByteLength), verifyCleanness, true, true, this);
            }
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                WriteChildWithLength(writer, _WrappedChar, includeChildrenMode, _WrappedChar_Accessed, () => GetChildSlice(LazinatorObjectBytes, _WrappedChar_ByteIndex, _WrappedChar_ByteLength), verifyCleanness, true, true, this);
            }
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                WriteChildWithLength(writer, _WrappedNullableBool, includeChildrenMode, _WrappedNullableBool_Accessed, () => GetChildSlice(LazinatorObjectBytes, _WrappedNullableBool_ByteIndex, _WrappedNullableBool_ByteLength), verifyCleanness, true, true, this);
            }
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                WriteChildWithLength(writer, _WrappedNullableByte, includeChildrenMode, _WrappedNullableByte_Accessed, () => GetChildSlice(LazinatorObjectBytes, _WrappedNullableByte_ByteIndex, _WrappedNullableByte_ByteLength), verifyCleanness, true, false, this);
            }
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                WriteChildWithLength(writer, _WrappedNullableChar, includeChildrenMode, _WrappedNullableChar_Accessed, () => GetChildSlice(LazinatorObjectBytes, _WrappedNullableChar_ByteIndex, _WrappedNullableChar_ByteLength), verifyCleanness, true, false, this);
            }
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                WriteChildWithLength(writer, _WrappedNullableSByte, includeChildrenMode, _WrappedNullableSByte_Accessed, () => GetChildSlice(LazinatorObjectBytes, _WrappedNullableSByte_ByteIndex, _WrappedNullableSByte_ByteLength), verifyCleanness, true, false, this);
            }
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                WriteChildWithLength(writer, _WrappedSByte, includeChildrenMode, _WrappedSByte_Accessed, () => GetChildSlice(LazinatorObjectBytes, _WrappedSByte_ByteIndex, _WrappedSByte_ByteLength), verifyCleanness, true, true, this);
            }
        }
        
    }
}
