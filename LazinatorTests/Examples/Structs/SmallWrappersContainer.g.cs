//98d8fd82-f59f-4bd5-899a-363c2afd81ed
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.96
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
            
            LazinatorGenericID = GetGenericIDIfApplicable(ContainsOpenGenericParameters, LazinatorUniqueID, span, ref bytesSoFar);
            
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
                InformParentOfDirtinessDelegate = InformParentOfDirtinessDelegate,
                OriginalIncludeChildrenMode = includeChildrenMode,
                HierarchyBytes = bytes,
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
        
        /* Property definitions */
        
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
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _ListWrappedBytes_ByteIndex, _ListWrappedBytes_ByteLength, false, false, null);
                        
                        _ListWrappedBytes = DeserializationFactory.Instance.CreateBaseOrDerivedType(51, () => new LazinatorList<WByte>(), childData, this); 
                    }
                    _ListWrappedBytes_Accessed = true;
                }
                return _ListWrappedBytes;
            }
            [DebuggerStepThrough]
            set
            {
                if (value != null)
                {
                    if (value.LazinatorParentClass != null)
                    {
                        throw new MovedLazinatorException($"The property ListWrappedBytes cannot be set to a Lazinator object with a defined LazinatorParentClass. Set the LazinatorParentClass to null, clone the object, or use the AutocloneAttribute or the AllowMovedAttribute.");
                    }
                    value.LazinatorParentClass = this;
                }
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
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _WrappedBool_ByteIndex, _WrappedBool_ByteLength, false, true, 1);
                        _WrappedBool = new WBool()
                        {
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
                var clone = value;
                
                clone.LazinatorParentClass = this;
                IsDirty = true;
                _WrappedBool = clone;
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
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _WrappedBool_ByteIndex, _WrappedBool_ByteLength, false, true, 1);
                        return new WBool()
                        {
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
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _WrappedByte_ByteIndex, _WrappedByte_ByteLength, false, true, 1);
                        _WrappedByte = new WByte()
                        {
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
                var clone = value;
                
                clone.LazinatorParentClass = this;
                IsDirty = true;
                _WrappedByte = clone;
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
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _WrappedByte_ByteIndex, _WrappedByte_ByteLength, false, true, 1);
                        return new WByte()
                        {
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
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _WrappedChar_ByteIndex, _WrappedChar_ByteLength, false, true, 2);
                        _WrappedChar = new WChar()
                        {
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
                var clone = value;
                
                clone.LazinatorParentClass = this;
                IsDirty = true;
                _WrappedChar = clone;
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
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _WrappedChar_ByteIndex, _WrappedChar_ByteLength, false, true, 2);
                        return new WChar()
                        {
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
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _WrappedNullableBool_ByteIndex, _WrappedNullableBool_ByteLength, false, true, 1);
                        _WrappedNullableBool = new WNullableBool()
                        {
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
                var clone = value;
                
                clone.LazinatorParentClass = this;
                IsDirty = true;
                _WrappedNullableBool = clone;
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
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _WrappedNullableBool_ByteIndex, _WrappedNullableBool_ByteLength, false, true, 1);
                        return new WNullableBool()
                        {
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
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _WrappedNullableByte_ByteIndex, _WrappedNullableByte_ByteLength, false, true, null);
                        _WrappedNullableByte = new WNullableByte()
                        {
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
                var clone = value;
                
                clone.LazinatorParentClass = this;
                IsDirty = true;
                _WrappedNullableByte = clone;
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
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _WrappedNullableByte_ByteIndex, _WrappedNullableByte_ByteLength, false, true, null);
                        return new WNullableByte()
                        {
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
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _WrappedNullableChar_ByteIndex, _WrappedNullableChar_ByteLength, false, true, null);
                        _WrappedNullableChar = new WNullableChar()
                        {
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
                var clone = value;
                
                clone.LazinatorParentClass = this;
                IsDirty = true;
                _WrappedNullableChar = clone;
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
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _WrappedNullableChar_ByteIndex, _WrappedNullableChar_ByteLength, false, true, null);
                        return new WNullableChar()
                        {
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
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _WrappedNullableSByte_ByteIndex, _WrappedNullableSByte_ByteLength, false, true, null);
                        _WrappedNullableSByte = new WNullableSByte()
                        {
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
                var clone = value;
                
                clone.LazinatorParentClass = this;
                IsDirty = true;
                _WrappedNullableSByte = clone;
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
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _WrappedNullableSByte_ByteIndex, _WrappedNullableSByte_ByteLength, false, true, null);
                        return new WNullableSByte()
                        {
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
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _WrappedSByte_ByteIndex, _WrappedSByte_ByteLength, false, true, 1);
                        _WrappedSByte = new WSByte()
                        {
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
                var clone = value;
                
                clone.LazinatorParentClass = this;
                IsDirty = true;
                _WrappedSByte = clone;
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
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _WrappedSByte_ByteIndex, _WrappedSByte_ByteLength, false, true, 1);
                        return new WSByte()
                        {
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
        
        protected virtual bool ContainsOpenGenericParameters => false;
        protected virtual System.Collections.Generic.List<int> _LazinatorGenericID { get; set; }
        public virtual System.Collections.Generic.List<int> LazinatorGenericID
        {
            get => null;
            set { }
        }
        
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
        
        public virtual void SerializeExistingBuffer(BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren)
            {
                updateStoredBuffer = false;
            }
            int startPosition = writer.Position;
            WritePropertiesIntoBuffer(writer, includeChildrenMode, verifyCleanness, updateStoredBuffer, true);
            if (updateStoredBuffer)
            {
                
                _IsDirty = false;
                _DescendantIsDirty = includeChildrenMode != IncludeChildrenMode.IncludeAllChildren && ((_ListWrappedBytes_Accessed && ListWrappedBytes != null && (ListWrappedBytes.IsDirty || ListWrappedBytes.DescendantIsDirty)) || (_WrappedBool_Accessed && (WrappedBool.IsDirty || WrappedBool.DescendantIsDirty)) || (_WrappedByte_Accessed && (WrappedByte.IsDirty || WrappedByte.DescendantIsDirty)) || (_WrappedChar_Accessed && (WrappedChar.IsDirty || WrappedChar.DescendantIsDirty)) || (_WrappedNullableBool_Accessed && (WrappedNullableBool.IsDirty || WrappedNullableBool.DescendantIsDirty)) || (_WrappedNullableByte_Accessed && (WrappedNullableByte.IsDirty || WrappedNullableByte.DescendantIsDirty)) || (_WrappedNullableChar_Accessed && (WrappedNullableChar.IsDirty || WrappedNullableChar.DescendantIsDirty)) || (_WrappedNullableSByte_Accessed && (WrappedNullableSByte.IsDirty || WrappedNullableSByte.DescendantIsDirty)) || (_WrappedSByte_Accessed && (WrappedSByte.IsDirty || WrappedSByte.DescendantIsDirty)));
                
                _LazinatorObjectBytes = writer.Slice(startPosition);
            }
        }
        protected virtual void WritePropertiesIntoBuffer(BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer, bool includeUniqueID)
        {
            int startPosition = writer.Position;
            int startOfObjectPosition = 0;
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
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                WriteChild(writer, _ListWrappedBytes, includeChildrenMode, _ListWrappedBytes_Accessed, () => GetChildSlice(LazinatorObjectBytes, _ListWrappedBytes_ByteIndex, _ListWrappedBytes_ByteLength, false, false, null), verifyCleanness, updateStoredBuffer, false, false, this);
            }
            if (updateStoredBuffer)
            {
                _ListWrappedBytes_ByteIndex = startOfObjectPosition - startPosition;
            }
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                WriteChild(writer, _WrappedBool, includeChildrenMode, _WrappedBool_Accessed, () => GetChildSlice(LazinatorObjectBytes, _WrappedBool_ByteIndex, _WrappedBool_ByteLength, false, true, 1), verifyCleanness, updateStoredBuffer, true, true, this);
            }
            if (updateStoredBuffer)
            {
                _WrappedBool_ByteIndex = startOfObjectPosition - startPosition;
            }
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                WriteChild(writer, _WrappedByte, includeChildrenMode, _WrappedByte_Accessed, () => GetChildSlice(LazinatorObjectBytes, _WrappedByte_ByteIndex, _WrappedByte_ByteLength, false, true, 1), verifyCleanness, updateStoredBuffer, true, true, this);
            }
            if (updateStoredBuffer)
            {
                _WrappedByte_ByteIndex = startOfObjectPosition - startPosition;
            }
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                WriteChild(writer, _WrappedChar, includeChildrenMode, _WrappedChar_Accessed, () => GetChildSlice(LazinatorObjectBytes, _WrappedChar_ByteIndex, _WrappedChar_ByteLength, false, true, 2), verifyCleanness, updateStoredBuffer, true, true, this);
            }
            if (updateStoredBuffer)
            {
                _WrappedChar_ByteIndex = startOfObjectPosition - startPosition;
            }
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                WriteChild(writer, _WrappedNullableBool, includeChildrenMode, _WrappedNullableBool_Accessed, () => GetChildSlice(LazinatorObjectBytes, _WrappedNullableBool_ByteIndex, _WrappedNullableBool_ByteLength, false, true, 1), verifyCleanness, updateStoredBuffer, true, true, this);
            }
            if (updateStoredBuffer)
            {
                _WrappedNullableBool_ByteIndex = startOfObjectPosition - startPosition;
            }
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                WriteChild(writer, _WrappedNullableByte, includeChildrenMode, _WrappedNullableByte_Accessed, () => GetChildSlice(LazinatorObjectBytes, _WrappedNullableByte_ByteIndex, _WrappedNullableByte_ByteLength, false, true, null), verifyCleanness, updateStoredBuffer, true, false, this);
            }
            if (updateStoredBuffer)
            {
                _WrappedNullableByte_ByteIndex = startOfObjectPosition - startPosition;
            }
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                WriteChild(writer, _WrappedNullableChar, includeChildrenMode, _WrappedNullableChar_Accessed, () => GetChildSlice(LazinatorObjectBytes, _WrappedNullableChar_ByteIndex, _WrappedNullableChar_ByteLength, false, true, null), verifyCleanness, updateStoredBuffer, true, false, this);
            }
            if (updateStoredBuffer)
            {
                _WrappedNullableChar_ByteIndex = startOfObjectPosition - startPosition;
            }
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                WriteChild(writer, _WrappedNullableSByte, includeChildrenMode, _WrappedNullableSByte_Accessed, () => GetChildSlice(LazinatorObjectBytes, _WrappedNullableSByte_ByteIndex, _WrappedNullableSByte_ByteLength, false, true, null), verifyCleanness, updateStoredBuffer, true, false, this);
            }
            if (updateStoredBuffer)
            {
                _WrappedNullableSByte_ByteIndex = startOfObjectPosition - startPosition;
            }
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                WriteChild(writer, _WrappedSByte, includeChildrenMode, _WrappedSByte_Accessed, () => GetChildSlice(LazinatorObjectBytes, _WrappedSByte_ByteIndex, _WrappedSByte_ByteLength, false, true, 1), verifyCleanness, updateStoredBuffer, true, true, this);
            }
            if (updateStoredBuffer)
            {
                _WrappedSByte_ByteIndex = startOfObjectPosition - startPosition;
            }
            if (updateStoredBuffer)
            {
                _SmallWrappersContainer_EndByteIndex = writer.Position - startPosition;
            }
        }
        
    }
}
