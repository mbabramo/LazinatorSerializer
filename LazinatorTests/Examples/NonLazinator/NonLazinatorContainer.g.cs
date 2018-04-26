//81081004-f0f9-5ad1-e2ce-0b2a960b8c35
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
using System.Collections.Generic;
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

namespace LazinatorTests.Examples
{
    public partial struct NonLazinatorContainer : ILazinator
    {
        /* Boilerplate for every base class implementing ILazinator */
        
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
        
        internal MemoryInBuffer EncodeToNewBuffer(IncludeChildrenMode includeChildrenMode, bool verifyCleanness) => LazinatorUtilities.EncodeToNewBuffer(this, includeChildrenMode, verifyCleanness);
        
        public ILazinator Clone()
        {
            return Clone(OriginalIncludeChildrenMode);
        }
        
        public ILazinator Clone(IncludeChildrenMode includeChildrenMode)
        {
            MemoryInBuffer bytes = EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, false, false, IsDirty, DescendantIsDirty, false, LazinatorObjectBytes, (StreamManuallyDelegate)EncodeToNewBuffer);
            var clone = new NonLazinatorContainer()
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
            //[DebuggerStepThrough]
            get => _IsDirty;
            //[DebuggerStepThrough]
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
            //[DebuggerStepThrough]
            get => _DescendantIsDirty;
            //[DebuggerStepThrough]
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
        
        /* Field boilerplate */
        
        internal int _NonLazinatorClass_ByteIndex;
        internal int _NonLazinatorStruct_ByteIndex;
        internal int _NonLazinatorClass_ByteLength => _NonLazinatorStruct_ByteIndex - _NonLazinatorClass_ByteIndex;
        internal int _NonLazinatorStruct_ByteLength => LazinatorObjectBytes.Length - _NonLazinatorStruct_ByteIndex;
        
        private NonLazinatorClass _NonLazinatorClass;
        public NonLazinatorClass NonLazinatorClass
        {
            //[DebuggerStepThrough]
            get
            {
                if (!_NonLazinatorClass_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _NonLazinatorClass = default(NonLazinatorClass);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _NonLazinatorClass_ByteIndex, _NonLazinatorClass_ByteLength);
                        _NonLazinatorClass = ConvertFromBytes_NonLazinatorClass(childData, DeserializationFactory, null);
                    }
                    _NonLazinatorClass_Accessed = true;
                    IsDirty = true;
                }
                return _NonLazinatorClass;
            }
            //[DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _NonLazinatorClass = value;
                _NonLazinatorClass_Accessed = true;
            }
        }
        internal bool _NonLazinatorClass_Accessed;
        private NonLazinatorStruct _NonLazinatorStruct;
        public NonLazinatorStruct NonLazinatorStruct
        {
            //[DebuggerStepThrough]
            get
            {
                if (!_NonLazinatorStruct_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _NonLazinatorStruct = default(NonLazinatorStruct);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _NonLazinatorStruct_ByteIndex, _NonLazinatorStruct_ByteLength);
                        _NonLazinatorStruct = ConvertFromBytes_NonLazinatorStruct(childData, DeserializationFactory, null);
                    }
                    _NonLazinatorStruct_Accessed = true;
                    IsDirty = true;
                }
                return _NonLazinatorStruct;
            }
            //[DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _NonLazinatorStruct = value;
                _NonLazinatorStruct_Accessed = true;
            }
        }
        internal bool _NonLazinatorStruct_Accessed;
        
        /* Conversion */
        
        public int LazinatorUniqueID => 125;
        
        private bool _LazinatorObjectVersionChanged;
        private int _LazinatorObjectVersionOverride;
        public int LazinatorObjectVersion
        {
            get => _LazinatorObjectVersionChanged ? _LazinatorObjectVersionOverride : 0;
            set
            {
                _LazinatorObjectVersionOverride = value;
                _LazinatorObjectVersionChanged = true;
            }
        }
        
        public void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
            _NonLazinatorClass_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren) 
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            _NonLazinatorStruct_ByteIndex = bytesSoFar;
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
            var serializedBytesCopy_NonLazinatorClass = LazinatorObjectBytes;
            var byteIndexCopy_NonLazinatorClass = _NonLazinatorClass_ByteIndex;
            var byteLengthCopy_NonLazinatorClass = _NonLazinatorClass_ByteLength;
            var copy_NonLazinatorClass = NonLazinatorClass;
            WriteNonLazinatorObject(
            nonLazinatorObject: _NonLazinatorClass, isBelievedDirty: _NonLazinatorClass_Accessed,
            isAccessed: _NonLazinatorClass_Accessed, writer: writer,
            getChildSliceForFieldFn: () => GetChildSlice(serializedBytesCopy_NonLazinatorClass, byteIndexCopy_NonLazinatorClass, byteLengthCopy_NonLazinatorClass),
            verifyCleanness: false,
            binaryWriterAction: (w, v) =>
            ConvertToBytes_NonLazinatorClass(w, copy_NonLazinatorClass, includeChildrenMode, v));
            var serializedBytesCopy_NonLazinatorStruct = LazinatorObjectBytes;
            var byteIndexCopy_NonLazinatorStruct = _NonLazinatorStruct_ByteIndex;
            var byteLengthCopy_NonLazinatorStruct = _NonLazinatorStruct_ByteLength;
            var copy_NonLazinatorStruct = NonLazinatorStruct;
            WriteNonLazinatorObject(
            nonLazinatorObject: _NonLazinatorStruct, isBelievedDirty: _NonLazinatorStruct_Accessed,
            isAccessed: _NonLazinatorStruct_Accessed, writer: writer,
            getChildSliceForFieldFn: () => GetChildSlice(serializedBytesCopy_NonLazinatorStruct, byteIndexCopy_NonLazinatorStruct, byteLengthCopy_NonLazinatorStruct),
            verifyCleanness: false,
            binaryWriterAction: (w, v) =>
            ConvertToBytes_NonLazinatorStruct(w, copy_NonLazinatorStruct, includeChildrenMode, v));
        }
        
    }
}
