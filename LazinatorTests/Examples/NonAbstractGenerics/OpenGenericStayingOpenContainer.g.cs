//5fb6bae7-bd1a-5e65-08b8-9b7ecaa9027d
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.106
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LazinatorTests.Examples.NonAbstractGenerics
{
    using Lazinator.Attributes;
    using Lazinator.Buffers;
    using Lazinator.Core;
    using Lazinator.Exceptions;
    using Lazinator.Support;
    using Lazinator.Wrappers;
    using LazinatorTests.Examples;
    using System;
    using System.Buffers;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.InteropServices;
    using static Lazinator.Core.LazinatorUtilities;
    
    [Autogenerated]
    public partial class OpenGenericStayingOpenContainer : ILazinator
    {
        /* Serialization, deserialization, and object relationships */
        
        protected ILazinator _LazinatorParentClass;
        public OpenGenericStayingOpenContainer() : base()
        {
        }
        
        public virtual ILazinator LazinatorParentClass 
        { 
            get => _LazinatorParentClass;
            set
            {
                _LazinatorParentClass = value;
                if (value != null && (value.IsDirty || value.DescendantIsDirty))
                {
                    DescendantIsDirty = true;
                }
            }
        }
        
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
            var clone = new OpenGenericStayingOpenContainer()
            {
                LazinatorParentClass = LazinatorParentClass,
                OriginalIncludeChildrenMode = includeChildrenMode,
                HierarchyBytes = bytes,
            };
            clone.LazinatorParentClass = null;
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
        
        protected int _ClosedGenericFloat_ByteIndex;
        protected int _ClosedGenericInterface_ByteIndex;
        protected int _ClosedGenericNonexclusiveInterface_ByteIndex;
        protected virtual int _ClosedGenericFloat_ByteLength => _ClosedGenericInterface_ByteIndex - _ClosedGenericFloat_ByteIndex;
        protected virtual int _ClosedGenericInterface_ByteLength => _ClosedGenericNonexclusiveInterface_ByteIndex - _ClosedGenericInterface_ByteIndex;
        private int _OpenGenericStayingOpenContainer_EndByteIndex;
        protected virtual int _ClosedGenericNonexclusiveInterface_ByteLength => _OpenGenericStayingOpenContainer_EndByteIndex - _ClosedGenericNonexclusiveInterface_ByteIndex;
        
        private OpenGeneric<WFloat> _ClosedGenericFloat;
        public OpenGeneric<WFloat> ClosedGenericFloat
        {
            get
            {
                if (!_ClosedGenericFloat_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _ClosedGenericFloat = default(OpenGeneric<WFloat>);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _ClosedGenericFloat_ByteIndex, _ClosedGenericFloat_ByteLength, false, false, null);
                        
                        _ClosedGenericFloat = DeserializationFactory.Instance.CreateBaseOrDerivedType(233, () => new OpenGeneric<WFloat>(), childData, this); 
                    }
                    _ClosedGenericFloat_Accessed = true;
                }
                return _ClosedGenericFloat;
            }
            set
            {
                if (value != null)
                {
                    if (value.LazinatorParentClass != null)
                    {
                        throw new MovedLazinatorException($"The property ClosedGenericFloat cannot be set to a Lazinator object with a defined LazinatorParentClass, because AutoChangeParent is set to false in the configuration file and no attribute providing an exception is present.");
                    }
                    value.LazinatorParentClass = this;
                }
                IsDirty = true;
                _ClosedGenericFloat = value;
                if (_ClosedGenericFloat != null)
                {
                    _ClosedGenericFloat.IsDirty = true;
                }
                _ClosedGenericFloat_Accessed = true;
                LazinatorUtilities.ConfirmDescendantDirtinessConsistency(this);
            }
        }
        protected bool _ClosedGenericFloat_Accessed;
        private OpenGeneric<IExampleChild> _ClosedGenericInterface;
        public OpenGeneric<IExampleChild> ClosedGenericInterface
        {
            get
            {
                if (!_ClosedGenericInterface_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _ClosedGenericInterface = default(OpenGeneric<IExampleChild>);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _ClosedGenericInterface_ByteIndex, _ClosedGenericInterface_ByteLength, false, false, null);
                        
                        _ClosedGenericInterface = DeserializationFactory.Instance.CreateBaseOrDerivedType(233, () => new OpenGeneric<IExampleChild>(), childData, this); 
                    }
                    _ClosedGenericInterface_Accessed = true;
                }
                return _ClosedGenericInterface;
            }
            set
            {
                if (value != null)
                {
                    if (value.LazinatorParentClass != null)
                    {
                        throw new MovedLazinatorException($"The property ClosedGenericInterface cannot be set to a Lazinator object with a defined LazinatorParentClass, because AutoChangeParent is set to false in the configuration file and no attribute providing an exception is present.");
                    }
                    value.LazinatorParentClass = this;
                }
                IsDirty = true;
                _ClosedGenericInterface = value;
                if (_ClosedGenericInterface != null)
                {
                    _ClosedGenericInterface.IsDirty = true;
                }
                _ClosedGenericInterface_Accessed = true;
                LazinatorUtilities.ConfirmDescendantDirtinessConsistency(this);
            }
        }
        protected bool _ClosedGenericInterface_Accessed;
        private OpenGeneric<IExampleNonexclusiveInterface> _ClosedGenericNonexclusiveInterface;
        public OpenGeneric<IExampleNonexclusiveInterface> ClosedGenericNonexclusiveInterface
        {
            get
            {
                if (!_ClosedGenericNonexclusiveInterface_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _ClosedGenericNonexclusiveInterface = default(OpenGeneric<IExampleNonexclusiveInterface>);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _ClosedGenericNonexclusiveInterface_ByteIndex, _ClosedGenericNonexclusiveInterface_ByteLength, false, false, null);
                        
                        _ClosedGenericNonexclusiveInterface = DeserializationFactory.Instance.CreateBaseOrDerivedType(233, () => new OpenGeneric<IExampleNonexclusiveInterface>(), childData, this); 
                    }
                    _ClosedGenericNonexclusiveInterface_Accessed = true;
                }
                return _ClosedGenericNonexclusiveInterface;
            }
            set
            {
                if (value != null)
                {
                    if (value.LazinatorParentClass != null)
                    {
                        throw new MovedLazinatorException($"The property ClosedGenericNonexclusiveInterface cannot be set to a Lazinator object with a defined LazinatorParentClass, because AutoChangeParent is set to false in the configuration file and no attribute providing an exception is present.");
                    }
                    value.LazinatorParentClass = this;
                }
                IsDirty = true;
                _ClosedGenericNonexclusiveInterface = value;
                if (_ClosedGenericNonexclusiveInterface != null)
                {
                    _ClosedGenericNonexclusiveInterface.IsDirty = true;
                }
                _ClosedGenericNonexclusiveInterface_Accessed = true;
                LazinatorUtilities.ConfirmDescendantDirtinessConsistency(this);
            }
        }
        protected bool _ClosedGenericNonexclusiveInterface_Accessed;
        
        protected virtual void ResetAccessedProperties()
        {
            _ClosedGenericFloat_Accessed = _ClosedGenericInterface_Accessed = _ClosedGenericNonexclusiveInterface_Accessed = false;
        }
        
        /* Conversion */
        
        public virtual int LazinatorUniqueID => 234;
        
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
            _ClosedGenericFloat_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            _ClosedGenericInterface_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            _ClosedGenericNonexclusiveInterface_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            _OpenGenericStayingOpenContainer_EndByteIndex = bytesSoFar;
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
                _DescendantIsDirty = includeChildrenMode != IncludeChildrenMode.IncludeAllChildren && ((_ClosedGenericFloat_Accessed && ClosedGenericFloat != null && (ClosedGenericFloat.IsDirty || ClosedGenericFloat.DescendantIsDirty)) || (_ClosedGenericInterface_Accessed && ClosedGenericInterface != null && (ClosedGenericInterface.IsDirty || ClosedGenericInterface.DescendantIsDirty)) || (_ClosedGenericNonexclusiveInterface_Accessed && ClosedGenericNonexclusiveInterface != null && (ClosedGenericNonexclusiveInterface.IsDirty || ClosedGenericNonexclusiveInterface.DescendantIsDirty)));
                
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
                WriteChild(writer, _ClosedGenericFloat, includeChildrenMode, _ClosedGenericFloat_Accessed, () => GetChildSlice(LazinatorObjectBytes, _ClosedGenericFloat_ByteIndex, _ClosedGenericFloat_ByteLength, false, false, null), verifyCleanness, updateStoredBuffer, false, false, this);
            }
            if (updateStoredBuffer)
            {
                _ClosedGenericFloat_ByteIndex = startOfObjectPosition - startPosition;
            }
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                WriteChild(writer, _ClosedGenericInterface, includeChildrenMode, _ClosedGenericInterface_Accessed, () => GetChildSlice(LazinatorObjectBytes, _ClosedGenericInterface_ByteIndex, _ClosedGenericInterface_ByteLength, false, false, null), verifyCleanness, updateStoredBuffer, false, false, this);
            }
            if (updateStoredBuffer)
            {
                _ClosedGenericInterface_ByteIndex = startOfObjectPosition - startPosition;
            }
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                WriteChild(writer, _ClosedGenericNonexclusiveInterface, includeChildrenMode, _ClosedGenericNonexclusiveInterface_Accessed, () => GetChildSlice(LazinatorObjectBytes, _ClosedGenericNonexclusiveInterface_ByteIndex, _ClosedGenericNonexclusiveInterface_ByteLength, false, false, null), verifyCleanness, updateStoredBuffer, false, false, this);
            }
            if (updateStoredBuffer)
            {
                _ClosedGenericNonexclusiveInterface_ByteIndex = startOfObjectPosition - startPosition;
            }
            if (updateStoredBuffer)
            {
                _OpenGenericStayingOpenContainer_EndByteIndex = writer.Position - startPosition;
            }
        }
        
    }
}
