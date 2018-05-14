//1a6163b6-aba6-bc08-ad40-72d360ddbe20
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.24
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

namespace LazinatorTests.Examples.Hierarchy
{
    public partial class ReflexiveExample : ILazinator
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
            var clone = new ReflexiveExample()
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
            _ReflexiveClass_Accessed = _ReflexiveInterface_Accessed = false;
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
        
        internal int _ReflexiveClass_ByteIndex;
        internal int _ReflexiveInterface_ByteIndex;
        internal virtual int _ReflexiveClass_ByteLength => _ReflexiveInterface_ByteIndex - _ReflexiveClass_ByteIndex;
        private int _ReflexiveExample_EndByteIndex;
        internal virtual int _ReflexiveInterface_ByteLength => _ReflexiveExample_EndByteIndex - _ReflexiveInterface_ByteIndex;
        
        private LazinatorTests.Examples.Hierarchy.ReflexiveExample _ReflexiveClass;
        public LazinatorTests.Examples.Hierarchy.ReflexiveExample ReflexiveClass
        {
            [DebuggerStepThrough]
            get
            {
                if (!_ReflexiveClass_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _ReflexiveClass = default(LazinatorTests.Examples.Hierarchy.ReflexiveExample);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _ReflexiveClass_ByteIndex, _ReflexiveClass_ByteLength);
                        
                        if (DeserializationFactory == null)
                        {
                            LazinatorDeserializationException.ThrowNoDeserializationFactory();
                        }
                        _ReflexiveClass = DeserializationFactory.Create(247, () => new LazinatorTests.Examples.Hierarchy.ReflexiveExample(), childData, this); 
                    }
                    _ReflexiveClass_Accessed = true;
                }
                return _ReflexiveClass;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _ReflexiveClass = value;
                if (_ReflexiveClass != null)
                {
                    _ReflexiveClass.IsDirty = true;
                }
                _ReflexiveClass_Accessed = true;
            }
        }
        internal bool _ReflexiveClass_Accessed;
        private LazinatorTests.Examples.Hierarchy.IReflexiveExample _ReflexiveInterface;
        public LazinatorTests.Examples.Hierarchy.IReflexiveExample ReflexiveInterface
        {
            [DebuggerStepThrough]
            get
            {
                if (!_ReflexiveInterface_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _ReflexiveInterface = default(LazinatorTests.Examples.Hierarchy.IReflexiveExample);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _ReflexiveInterface_ByteIndex, _ReflexiveInterface_ByteLength);
                        
                        if (DeserializationFactory == null)
                        {
                            LazinatorDeserializationException.ThrowNoDeserializationFactory();
                        }
                        ReflexiveInterface = (LazinatorTests.Examples.Hierarchy.IReflexiveExample)DeserializationFactory.FactoryCreate(childData, this); 
                    }
                    _ReflexiveInterface_Accessed = true;
                }
                return _ReflexiveInterface;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _ReflexiveInterface = value;
                if (_ReflexiveInterface != null)
                {
                    _ReflexiveInterface.IsDirty = true;
                }
                _ReflexiveInterface_Accessed = true;
            }
        }
        internal bool _ReflexiveInterface_Accessed;
        
        /* Conversion */
        
        public virtual int LazinatorUniqueID => 247;
        
        public virtual int LazinatorObjectVersion { get; set; } = 0;
        
        public virtual void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
            _ReflexiveClass_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            _ReflexiveInterface_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            _ReflexiveExample_EndByteIndex = bytesSoFar;
        }
        
        public virtual void SerializeExistingBuffer(BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
            // header information
            CompressedIntegralTypes.WriteCompressedInt(writer, LazinatorUniqueID);
            CompressedIntegralTypes.WriteCompressedInt(writer, Lazinator.Support.LazinatorVersionInfo.LazinatorIntVersion);
            CompressedIntegralTypes.WriteCompressedInt(writer, LazinatorObjectVersion);
            writer.Write((byte)includeChildrenMode);
            // write properties
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                WriteChildWithLength(writer, _ReflexiveClass, includeChildrenMode, _ReflexiveClass_Accessed, () => GetChildSlice(LazinatorObjectBytes, _ReflexiveClass_ByteIndex, _ReflexiveClass_ByteLength), verifyCleanness, false);
            }
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                WriteChildWithLength(writer, _ReflexiveInterface, includeChildrenMode, _ReflexiveInterface_Accessed, () => GetChildSlice(LazinatorObjectBytes, _ReflexiveInterface_ByteIndex, _ReflexiveInterface_ByteLength), verifyCleanness, false);
            }
        }
        
    }
}
