//08faf94f-a102-7694-b680-ce9861a9dfb0
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.207
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LazinatorTests.Examples
{
    using Lazinator.Attributes;
    using Lazinator.Buffers;
    using Lazinator.Core;
    using Lazinator.Exceptions;
    using Lazinator.Support;
    using System;
    using System.Buffers;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;
    using static Lazinator.Core.LazinatorUtilities;
    
    [Autogenerated]
    public partial struct NonLazinatorContainer : ILazinator
    {
        public bool IsStruct => true;
        
        /* Serialization, deserialization, and object relationships */
        
        public LazinatorParentsCollection LazinatorParents { get; set; }
        
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
            
            int uniqueID = span.ToDecompressedInt(ref bytesSoFar);
            if (uniqueID != LazinatorUniqueID)
            {
                throw new FormatException("Wrong Lazinator type initialized.");
            }
            
            int lazinatorLibraryVersion = span.ToDecompressedInt(ref bytesSoFar);
            
            int serializedVersionNumber = span.ToDecompressedInt(ref bytesSoFar);
            
            OriginalIncludeChildrenMode = (IncludeChildrenMode)span.ToByte(ref bytesSoFar);
            
            ConvertFromBytesAfterHeader(OriginalIncludeChildrenMode, serializedVersionNumber, ref bytesSoFar);
            return bytesSoFar;
        }
        
        public LazinatorMemory SerializeNewBuffer(IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
            return EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, verifyCleanness, IsDirty, DescendantIsDirty, false, LazinatorObjectBytes, (StreamManuallyDelegate) EncodeToNewBuffer);
        }
        
        LazinatorMemory EncodeToNewBuffer(IncludeChildrenMode includeChildrenMode, bool verifyCleanness) => LazinatorUtilities.EncodeToNewBinaryBufferWriter(this, includeChildrenMode, verifyCleanness);
        
        public ILazinator CloneLazinator()
        {
            return CloneLazinator(OriginalIncludeChildrenMode);
        }
        
        public ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode)
        {
            LazinatorMemory bytes = EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, false, IsDirty, DescendantIsDirty, false, LazinatorObjectBytes, (StreamManuallyDelegate)EncodeToNewBuffer);
            var clone = new NonLazinatorContainer()
            {
                LazinatorParents = LazinatorParents,
                OriginalIncludeChildrenMode = includeChildrenMode,
                HierarchyBytes = bytes,
            };
            clone.LazinatorParents = default;
            return clone;
        }
        
        public bool HasChanged { get; set; }
        
        bool _IsDirty;
        public bool IsDirty
        {
            [DebuggerStepThrough]
            get => _IsDirty || LazinatorObjectBytes.Length == 0;
            [DebuggerStepThrough]
            set
            {
                if (_IsDirty != value)
                {
                    _IsDirty = value;
                    if (_IsDirty)
                    {
                        LazinatorParents.InformParentsOfDirtiness();
                        HasChanged = true;
                    }
                }
            }
        }
        
        bool _DescendantHasChanged;
        public bool DescendantHasChanged
        {
            [DebuggerStepThrough]
            get => _DescendantHasChanged;
            [DebuggerStepThrough]
            set
            {
                _DescendantHasChanged = value;
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
                    if (_DescendantIsDirty)
                    {
                        LazinatorParents.InformParentsOfDirtiness();
                        _DescendantHasChanged = true;
                    }
                }
            }
        }
        
        private LazinatorMemory _HierarchyBytes;
        public LazinatorMemory HierarchyBytes
        {
            set
            {
                _HierarchyBytes = value;
                LazinatorObjectBytes = value.Memory;
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
            if (!IsDirty && !DescendantIsDirty && LazinatorObjectBytes.Length > 0)
            {
                return;
            }
            LazinatorMemory bytes = EncodeOrRecycleToNewBuffer(IncludeChildrenMode.IncludeAllChildren, OriginalIncludeChildrenMode, false, IsDirty, DescendantIsDirty, false, LazinatorObjectBytes, (StreamManuallyDelegate)EncodeToNewBuffer);
            _LazinatorObjectBytes = bytes.Memory;
        }
        
        public int GetByteLength()
        {
            LazinatorConvertToBytes();
            return LazinatorObjectBytes.Length;
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
        
        /* Property definitions */
        
        int _NonLazinatorClass_ByteIndex;
        int _NonLazinatorInterchangeableClass_ByteIndex;
        int _NonLazinatorStruct_ByteIndex;
        int _NonLazinatorClass_ByteLength => _NonLazinatorInterchangeableClass_ByteIndex - _NonLazinatorClass_ByteIndex;
        int _NonLazinatorInterchangeableClass_ByteLength => _NonLazinatorStruct_ByteIndex - _NonLazinatorInterchangeableClass_ByteIndex;
        private int _NonLazinatorContainer_EndByteIndex;
        int _NonLazinatorStruct_ByteLength => _NonLazinatorContainer_EndByteIndex - _NonLazinatorStruct_ByteIndex;
        
        NonLazinatorClass _NonLazinatorClass;
        public NonLazinatorClass NonLazinatorClass
        {
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
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _NonLazinatorClass_ByteIndex, _NonLazinatorClass_ByteLength, false, false, null);
                        _NonLazinatorClass = NonLazinatorDirectConverter.ConvertFromBytes_NonLazinatorClass(childData);
                    }
                    _NonLazinatorClass_Accessed = true;
                }
                IsDirty = true; 
                return _NonLazinatorClass;
            }
            set
            {
                IsDirty = true;
                DescendantIsDirty = true;
                _NonLazinatorClass = value;
                _NonLazinatorClass_Accessed = true;
            }
        }
        bool _NonLazinatorClass_Accessed;
        NonLazinatorInterchangeableClass _NonLazinatorInterchangeableClass;
        public NonLazinatorInterchangeableClass NonLazinatorInterchangeableClass
        {
            get
            {
                if (!_NonLazinatorInterchangeableClass_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _NonLazinatorInterchangeableClass = default(NonLazinatorInterchangeableClass);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _NonLazinatorInterchangeableClass_ByteIndex, _NonLazinatorInterchangeableClass_ByteLength, false, false, null);
                        _NonLazinatorInterchangeableClass = ConvertFromBytes_NonLazinatorInterchangeableClass(childData);
                    }
                    _NonLazinatorInterchangeableClass_Accessed = true;
                }
                IsDirty = true; 
                return _NonLazinatorInterchangeableClass;
            }
            set
            {
                IsDirty = true;
                DescendantIsDirty = true;
                _NonLazinatorInterchangeableClass = value;
                _NonLazinatorInterchangeableClass_Accessed = true;
            }
        }
        bool _NonLazinatorInterchangeableClass_Accessed;
        NonLazinatorStruct _NonLazinatorStruct;
        public NonLazinatorStruct NonLazinatorStruct
        {
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
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _NonLazinatorStruct_ByteIndex, _NonLazinatorStruct_ByteLength, false, false, null);
                        _NonLazinatorStruct = NonLazinatorDirectConverter.ConvertFromBytes_NonLazinatorStruct(childData);
                    }
                    _NonLazinatorStruct_Accessed = true;
                }
                IsDirty = true; 
                return _NonLazinatorStruct;
            }
            set
            {
                IsDirty = true;
                DescendantIsDirty = true;
                _NonLazinatorStruct = value;
                _NonLazinatorStruct_Accessed = true;
            }
        }
        bool _NonLazinatorStruct_Accessed;
        
        public IEnumerable<ILazinator> EnumerateLazinatorNodes(Func<ILazinator, bool> matchCriterion, bool stopExploringBelowMatch, Func<ILazinator, bool> exploreCriterion, bool exploreOnlyDeserializedChildren, bool enumerateNulls)
        {
            bool match = (matchCriterion == null) ? true : matchCriterion(this);
            bool explore = (!match || !stopExploringBelowMatch) && ((exploreCriterion == null) ? true : exploreCriterion(this));
            if (match)
            {
                yield return this;
            }
            if (explore)
            {
                foreach (var item in EnumerateLazinatorDescendants(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
                {
                    yield return item.descendant;
                }
            }
        }
        
        public IEnumerable<(string propertyName, ILazinator descendant)> EnumerateLazinatorDescendants(Func<ILazinator, bool> matchCriterion, bool stopExploringBelowMatch, Func<ILazinator, bool> exploreCriterion, bool exploreOnlyDeserializedChildren, bool enumerateNulls)
        {
            yield break;
        }
        
        
        public IEnumerable<(string propertyName, object descendant)> EnumerateNonLazinatorProperties()
        {
            yield return ("NonLazinatorClass", (object)NonLazinatorClass);
            yield return ("NonLazinatorInterchangeableClass", (object)NonLazinatorInterchangeableClass);
            yield return ("NonLazinatorStruct", (object)NonLazinatorStruct);
            yield break;
        }
        
        void ResetAccessedProperties()
        {
            _NonLazinatorClass_Accessed = _NonLazinatorInterchangeableClass_Accessed = _NonLazinatorStruct_Accessed = false;
            IsDirty = false;
            DescendantIsDirty = false;
            HasChanged = false;
            DescendantHasChanged = false;
        }
        
        /* Conversion */
        
        public int LazinatorUniqueID => 232;
        
        bool ContainsOpenGenericParameters => false;
        public LazinatorGenericIDType LazinatorGenericID
        {
            get => default;
            set { }
        }
        
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
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            _NonLazinatorInterchangeableClass_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            _NonLazinatorStruct_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            _NonLazinatorContainer_EndByteIndex = bytesSoFar;
        }
        
        public void SerializeExistingBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren)
            {
                updateStoredBuffer = false;
            }
            int startPosition = writer.Position;
            WritePropertiesIntoBuffer(ref writer, includeChildrenMode, verifyCleanness, updateStoredBuffer, true);
            if (updateStoredBuffer)
            {
                
                _IsDirty = false;
                if (includeChildrenMode == IncludeChildrenMode.IncludeAllChildren)
                {
                    _DescendantIsDirty = false;
                }
                else
                {
                    throw new Exception("Cannot update stored buffer when serializing only some children.");
                }
                
                _LazinatorObjectBytes = writer.Slice(startPosition);
            }
        }
        void WritePropertiesIntoBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer, bool includeUniqueID)
        {
            int startPosition = writer.Position;
            int startOfObjectPosition = 0;
            // header information
            if (includeUniqueID)
            {
                CompressedIntegralTypes.WriteCompressedInt(ref writer, LazinatorUniqueID);
            }
            
            CompressedIntegralTypes.WriteCompressedInt(ref writer, Lazinator.Support.LazinatorVersionInfo.LazinatorIntVersion);
            CompressedIntegralTypes.WriteCompressedInt(ref writer, LazinatorObjectVersion);
            writer.Write((byte)includeChildrenMode);
            // write properties
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren && !_NonLazinatorClass_Accessed)
            {
                var deserialized = NonLazinatorClass;
            }
            var serializedBytesCopy_NonLazinatorClass = LazinatorObjectBytes;
            var byteIndexCopy_NonLazinatorClass = _NonLazinatorClass_ByteIndex;
            var byteLengthCopy_NonLazinatorClass = _NonLazinatorClass_ByteLength;
            var copy_NonLazinatorClass = _NonLazinatorClass;
            WriteNonLazinatorObject(
            nonLazinatorObject: _NonLazinatorClass, isBelievedDirty: _NonLazinatorClass_Accessed,
            isAccessed: _NonLazinatorClass_Accessed, writer: ref writer,
            getChildSliceForFieldFn: () => GetChildSlice(serializedBytesCopy_NonLazinatorClass, byteIndexCopy_NonLazinatorClass, byteLengthCopy_NonLazinatorClass, false, false, null),
            verifyCleanness: false,
            binaryWriterAction: (ref BinaryBufferWriter w, bool v) =>
            NonLazinatorDirectConverter.ConvertToBytes_NonLazinatorClass(ref w, copy_NonLazinatorClass, includeChildrenMode, v, updateStoredBuffer));
            if (updateStoredBuffer)
            {
                _NonLazinatorClass_ByteIndex = startOfObjectPosition - startPosition;
            }
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren && !_NonLazinatorInterchangeableClass_Accessed)
            {
                var deserialized = NonLazinatorInterchangeableClass;
            }
            var serializedBytesCopy_NonLazinatorInterchangeableClass = LazinatorObjectBytes;
            var byteIndexCopy_NonLazinatorInterchangeableClass = _NonLazinatorInterchangeableClass_ByteIndex;
            var byteLengthCopy_NonLazinatorInterchangeableClass = _NonLazinatorInterchangeableClass_ByteLength;
            var copy_NonLazinatorInterchangeableClass = _NonLazinatorInterchangeableClass;
            WriteNonLazinatorObject(
            nonLazinatorObject: _NonLazinatorInterchangeableClass, isBelievedDirty: _NonLazinatorInterchangeableClass_Accessed,
            isAccessed: _NonLazinatorInterchangeableClass_Accessed, writer: ref writer,
            getChildSliceForFieldFn: () => GetChildSlice(serializedBytesCopy_NonLazinatorInterchangeableClass, byteIndexCopy_NonLazinatorInterchangeableClass, byteLengthCopy_NonLazinatorInterchangeableClass, false, false, null),
            verifyCleanness: false,
            binaryWriterAction: (ref BinaryBufferWriter w, bool v) =>
            ConvertToBytes_NonLazinatorInterchangeableClass(ref w, copy_NonLazinatorInterchangeableClass, includeChildrenMode, v, updateStoredBuffer));
            if (updateStoredBuffer)
            {
                _NonLazinatorInterchangeableClass_ByteIndex = startOfObjectPosition - startPosition;
            }
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren && !_NonLazinatorStruct_Accessed)
            {
                var deserialized = NonLazinatorStruct;
            }
            var serializedBytesCopy_NonLazinatorStruct = LazinatorObjectBytes;
            var byteIndexCopy_NonLazinatorStruct = _NonLazinatorStruct_ByteIndex;
            var byteLengthCopy_NonLazinatorStruct = _NonLazinatorStruct_ByteLength;
            var copy_NonLazinatorStruct = _NonLazinatorStruct;
            WriteNonLazinatorObject(
            nonLazinatorObject: _NonLazinatorStruct, isBelievedDirty: _NonLazinatorStruct_Accessed,
            isAccessed: _NonLazinatorStruct_Accessed, writer: ref writer,
            getChildSliceForFieldFn: () => GetChildSlice(serializedBytesCopy_NonLazinatorStruct, byteIndexCopy_NonLazinatorStruct, byteLengthCopy_NonLazinatorStruct, false, false, null),
            verifyCleanness: false,
            binaryWriterAction: (ref BinaryBufferWriter w, bool v) =>
            NonLazinatorDirectConverter.ConvertToBytes_NonLazinatorStruct(ref w, copy_NonLazinatorStruct, includeChildrenMode, v, updateStoredBuffer));
            if (updateStoredBuffer)
            {
                _NonLazinatorStruct_ByteIndex = startOfObjectPosition - startPosition;
            }
            if (updateStoredBuffer)
            {
                _NonLazinatorContainer_EndByteIndex = writer.Position - startPosition;
            }
        }
        
        private static NonLazinatorInterchangeableClass ConvertFromBytes_NonLazinatorInterchangeableClass(ReadOnlyMemory<byte> storage)
        {
            NonLazinatorInterchangeClass interchange = new NonLazinatorInterchangeClass()
            {
                LazinatorObjectBytes = storage
            };
            return interchange.Interchange_NonLazinatorInterchangeableClass();
        }
        
        private static void ConvertToBytes_NonLazinatorInterchangeableClass(ref BinaryBufferWriter writer,
        NonLazinatorInterchangeableClass itemToConvert, IncludeChildrenMode includeChildrenMode,
        bool verifyCleanness, bool updateStoredBuffer)
        {
            NonLazinatorInterchangeClass interchange = new NonLazinatorInterchangeClass(itemToConvert);
            interchange.SerializeExistingBuffer(ref writer, includeChildrenMode, verifyCleanness, updateStoredBuffer);
        }
        
    }
}
