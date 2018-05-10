//e4a50b37-854d-8341-9df9-92ce0d6cbd59
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.22
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

namespace LazinatorTests.Examples
{
    public partial struct NonLazinatorContainer : ILazinator
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
        
        internal int _NonLazinatorClass_ByteIndex;
        internal int _NonLazinatorInterchangeableClass_ByteIndex;
        internal int _NonLazinatorStruct_ByteIndex;
        internal int _NonLazinatorClass_ByteLength => _NonLazinatorInterchangeableClass_ByteIndex - _NonLazinatorClass_ByteIndex;
        internal int _NonLazinatorInterchangeableClass_ByteLength => _NonLazinatorStruct_ByteIndex - _NonLazinatorInterchangeableClass_ByteIndex;
        internal int _NonLazinatorStruct_ByteLength => LazinatorObjectBytes.Length - _NonLazinatorStruct_ByteIndex;
        
        private LazinatorTests.Examples.NonLazinatorClass _NonLazinatorClass;
        public LazinatorTests.Examples.NonLazinatorClass NonLazinatorClass
        {
            [DebuggerStepThrough]
            get
            {
                if (!_NonLazinatorClass_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _NonLazinatorClass = default(LazinatorTests.Examples.NonLazinatorClass);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _NonLazinatorClass_ByteIndex, _NonLazinatorClass_ByteLength);
                        _NonLazinatorClass = LazinatorTests.Examples.NonLazinatorDirectConverter.ConvertFromBytes_LazinatorTests_Examples_NonLazinatorClass(childData, DeserializationFactory, null);
                    }
                    _NonLazinatorClass_Accessed = true;
                    IsDirty = true;
                }
                return _NonLazinatorClass;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _NonLazinatorClass = value;
                _NonLazinatorClass_Accessed = true;
            }
        }
        internal bool _NonLazinatorClass_Accessed;
        private LazinatorTests.Examples.NonLazinatorInterchangeableClass _NonLazinatorInterchangeableClass;
        public LazinatorTests.Examples.NonLazinatorInterchangeableClass NonLazinatorInterchangeableClass
        {
            [DebuggerStepThrough]
            get
            {
                if (!_NonLazinatorInterchangeableClass_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _NonLazinatorInterchangeableClass = default(LazinatorTests.Examples.NonLazinatorInterchangeableClass);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _NonLazinatorInterchangeableClass_ByteIndex, _NonLazinatorInterchangeableClass_ByteLength);
                        _NonLazinatorInterchangeableClass = ConvertFromBytes_LazinatorTests_Examples_NonLazinatorInterchangeableClass(childData, DeserializationFactory, null);
                    }
                    _NonLazinatorInterchangeableClass_Accessed = true;
                    IsDirty = true;
                }
                return _NonLazinatorInterchangeableClass;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _NonLazinatorInterchangeableClass = value;
                _NonLazinatorInterchangeableClass_Accessed = true;
            }
        }
        internal bool _NonLazinatorInterchangeableClass_Accessed;
        private LazinatorTests.Examples.NonLazinatorStruct _NonLazinatorStruct;
        public LazinatorTests.Examples.NonLazinatorStruct NonLazinatorStruct
        {
            [DebuggerStepThrough]
            get
            {
                if (!_NonLazinatorStruct_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _NonLazinatorStruct = default(LazinatorTests.Examples.NonLazinatorStruct);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _NonLazinatorStruct_ByteIndex, _NonLazinatorStruct_ByteLength);
                        _NonLazinatorStruct = LazinatorTests.Examples.NonLazinatorDirectConverter.ConvertFromBytes_LazinatorTests_Examples_NonLazinatorStruct(childData, DeserializationFactory, null);
                    }
                    _NonLazinatorStruct_Accessed = true;
                    IsDirty = true;
                }
                return _NonLazinatorStruct;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _NonLazinatorStruct = value;
                _NonLazinatorStruct_Accessed = true;
            }
        }
        internal bool _NonLazinatorStruct_Accessed;
        
        /* Conversion */
        
        public int LazinatorUniqueID => 232;
        
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
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            _NonLazinatorInterchangeableClass_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            _NonLazinatorStruct_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
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
            LazinatorTests.Examples.NonLazinatorDirectConverter.ConvertToBytes_LazinatorTests_Examples_NonLazinatorClass(w, copy_NonLazinatorClass, includeChildrenMode, v));
            var serializedBytesCopy_NonLazinatorInterchangeableClass = LazinatorObjectBytes;
            var byteIndexCopy_NonLazinatorInterchangeableClass = _NonLazinatorInterchangeableClass_ByteIndex;
            var byteLengthCopy_NonLazinatorInterchangeableClass = _NonLazinatorInterchangeableClass_ByteLength;
            var copy_NonLazinatorInterchangeableClass = NonLazinatorInterchangeableClass;
            WriteNonLazinatorObject(
            nonLazinatorObject: _NonLazinatorInterchangeableClass, isBelievedDirty: _NonLazinatorInterchangeableClass_Accessed,
            isAccessed: _NonLazinatorInterchangeableClass_Accessed, writer: writer,
            getChildSliceForFieldFn: () => GetChildSlice(serializedBytesCopy_NonLazinatorInterchangeableClass, byteIndexCopy_NonLazinatorInterchangeableClass, byteLengthCopy_NonLazinatorInterchangeableClass),
            verifyCleanness: false,
            binaryWriterAction: (w, v) =>
            ConvertToBytes_LazinatorTests_Examples_NonLazinatorInterchangeableClass(w, copy_NonLazinatorInterchangeableClass, includeChildrenMode, v));
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
            LazinatorTests.Examples.NonLazinatorDirectConverter.ConvertToBytes_LazinatorTests_Examples_NonLazinatorStruct(w, copy_NonLazinatorStruct, includeChildrenMode, v));
        }
        
        public static LazinatorTests.Examples.NonLazinatorInterchangeableClass ConvertFromBytes_LazinatorTests_Examples_NonLazinatorInterchangeableClass(ReadOnlyMemory<byte> storage, DeserializationFactory deserializationFactory, LazinatorUtilities.InformParentOfDirtinessDelegate informParentOfDirtinessDelegate)
        {
            LazinatorTests.Examples.NonLazinatorInterchangeClass interchange = new LazinatorTests.Examples.NonLazinatorInterchangeClass()
            {
                DeserializationFactory = deserializationFactory,
                LazinatorObjectBytes = storage
            };
            return interchange.Interchange();
        }
        
        public static void ConvertToBytes_LazinatorTests_Examples_NonLazinatorInterchangeableClass(BinaryBufferWriter writer,
        LazinatorTests.Examples.NonLazinatorInterchangeableClass itemToConvert, IncludeChildrenMode includeChildrenMode,
        bool verifyCleanness)
        {
            LazinatorTests.Examples.NonLazinatorInterchangeClass interchange = new LazinatorTests.Examples.NonLazinatorInterchangeClass(itemToConvert);
            interchange.SerializeExistingBuffer(writer, includeChildrenMode, verifyCleanness);
        }
        
    }
}
