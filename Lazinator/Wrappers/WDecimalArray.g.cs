//c26b8ab0-8b7d-6b6e-f7c4-c6fc1e08800e
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.226
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Lazinator.Wrappers
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
    public partial struct WDecimalArray : ILazinator
    {
        public bool IsStruct => true;
        
        /* Serialization, deserialization, and object relationships */
        
        public LazinatorParentsCollection LazinatorParents { get; set; }
        
        IncludeChildrenMode OriginalIncludeChildrenMode;
        
        public int Deserialize()
        {
            FreeInMemoryObjects();
            int bytesSoFar = 0;
            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
            if (span.Length == 0)
            {
                return 0;
            }
            
            int lazinatorLibraryVersion = span.ToDecompressedInt(ref bytesSoFar);
            
            int serializedVersionNumber = -1; /* versioning disabled */
            
            OriginalIncludeChildrenMode = (IncludeChildrenMode)span.ToByte(ref bytesSoFar);
            
            ConvertFromBytesAfterHeader(OriginalIncludeChildrenMode, serializedVersionNumber, ref bytesSoFar);
            return bytesSoFar;
        }
        
        public LazinatorMemory SerializeLazinator(IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            return EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, verifyCleanness, IsDirty, DescendantIsDirty, false, LazinatorMemoryStorage, (EncodeManuallyDelegate) EncodeToNewBuffer, updateStoredBuffer);
        }
        
        LazinatorMemory EncodeToNewBuffer(IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer) => LazinatorUtilities.EncodeToNewBinaryBufferWriter(this, includeChildrenMode, verifyCleanness, updateStoredBuffer);
        
        public ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode = IncludeChildrenMode.IncludeAllChildren, CloneBufferOptions cloneBufferOptions = CloneBufferOptions.LinkedBuffer)
        {
            var clone = new WDecimalArray()
            {
                OriginalIncludeChildrenMode = includeChildrenMode
            };
            if (clone.LazinatorObjectVersion != LazinatorObjectVersion)
            {
                clone.LazinatorObjectVersion = LazinatorObjectVersion;
            }
            
            if (cloneBufferOptions == CloneBufferOptions.NoBuffer)
            {
                AssignCloneProperties(ref clone, includeChildrenMode);
            }
            else
            {
                LazinatorMemory bytes = EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, false, IsDirty, DescendantIsDirty, false, LazinatorMemoryStorage, (EncodeManuallyDelegate)EncodeToNewBuffer, cloneBufferOptions == CloneBufferOptions.SharedBuffer);
                clone.DeserializeLazinator(bytes);
                if (cloneBufferOptions == CloneBufferOptions.IndependentBuffers)
                {
                    clone.LazinatorMemoryStorage.DisposeIndependently();
                }
            }
            clone.LazinatorParents = default;
            return clone;
        }
        
        void AssignCloneProperties(ref WDecimalArray clone, IncludeChildrenMode includeChildrenMode)
        {
            clone.FreeInMemoryObjects();
            clone.WrappedValue = Clone_decimal_B_b(WrappedValue);
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
        
        public void DeserializeLazinator(LazinatorMemory serializedBytes)
        {
            LazinatorMemoryStorage = serializedBytes;
        }
        
        LazinatorMemory _LazinatorMemoryStorage;
        public LazinatorMemory LazinatorMemoryStorage
        {
            get => _LazinatorMemoryStorage;
            set
            {
                _LazinatorMemoryStorage = value;
                int length = Deserialize();
                if (length != _LazinatorMemoryStorage.Length)
                {
                    _LazinatorMemoryStorage = _LazinatorMemoryStorage.Slice(0, length);
                }
            }
        }
        ReadOnlyMemory<byte> LazinatorObjectBytes => LazinatorMemoryStorage?.Memory ?? LazinatorUtilities.EmptyReadOnlyMemory;
        
        public void EnsureLazinatorMemoryUpToDate()
        {
            if (_LazinatorMemoryStorage == null)
            {
                throw new NotSupportedException("Cannot use EnsureLazinatorMemoryUpToDate on a struct that has not been deserialized. Clone the struct instead."); 
            }
            if (!IsDirty && !DescendantIsDirty && LazinatorObjectBytes.Length > 0 && OriginalIncludeChildrenMode == IncludeChildrenMode.IncludeAllChildren)
            {
                return;
            }
            LazinatorMemoryStorage = EncodeOrRecycleToNewBuffer(IncludeChildrenMode.IncludeAllChildren, OriginalIncludeChildrenMode, false, IsDirty, DescendantIsDirty, false, LazinatorMemoryStorage, (EncodeManuallyDelegate)EncodeToNewBuffer, true);
            OriginalIncludeChildrenMode = IncludeChildrenMode.IncludeAllChildren;
        }
        
        public int GetByteLength()
        {
            EnsureLazinatorMemoryUpToDate();
            return LazinatorObjectBytes.Length;
        }
        
        public uint GetBinaryHashCode32()
        {
            if (_LazinatorMemoryStorage == null)
            {
                var result = SerializeLazinator(IncludeChildrenMode.IncludeAllChildren, false, false);
                return FarmhashByteSpans.Hash32(result.Span);
            }
            else
            {
                EnsureLazinatorMemoryUpToDate();
                return FarmhashByteSpans.Hash32(LazinatorObjectBytes.Span);
            }
        }
        
        public ulong GetBinaryHashCode64()
        {
            if (_LazinatorMemoryStorage == null)
            {
                var result = SerializeLazinator(IncludeChildrenMode.IncludeAllChildren, false, false);
                return FarmhashByteSpans.Hash64(result.Span);
            }
            else
            {
                EnsureLazinatorMemoryUpToDate();
                return FarmhashByteSpans.Hash64(LazinatorObjectBytes.Span);
            }
        }
        
        public Guid GetBinaryHashCode128()
        {
            if (_LazinatorMemoryStorage == null)
            {
                var result = SerializeLazinator(IncludeChildrenMode.IncludeAllChildren, false, false);
                return FarmhashByteSpans.Hash128(result.Span);
            }
            else
            {
                EnsureLazinatorMemoryUpToDate();
                return FarmhashByteSpans.Hash128(LazinatorObjectBytes.Span);
            }
        }
        
        /* Property definitions */
        
        int _WrappedValue_ByteIndex;
        private int _WDecimalArray_EndByteIndex;
        int _WrappedValue_ByteLength => _WDecimalArray_EndByteIndex - _WrappedValue_ByteIndex;
        
        decimal[] _WrappedValue;
        public decimal[] WrappedValue
        {
            [DebuggerStepThrough]
            get
            {
                if (!_WrappedValue_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _WrappedValue = default(decimal[]);
                    }
                    else
                    {
                        LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _WrappedValue_ByteIndex, _WrappedValue_ByteLength, true, false, null);
                        _WrappedValue = ConvertFromBytes_decimal_B_b(childData);
                    }
                    _WrappedValue_Accessed = true;
                }
                IsDirty = true; 
                return _WrappedValue;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                DescendantIsDirty = true;
                _WrappedValue = value;
                _WrappedValue_Accessed = true;
            }
        }
        bool _WrappedValue_Accessed;
        
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
            yield return ("WrappedValue", (object)WrappedValue);
            yield break;
        }
        
        public void FreeInMemoryObjects()
        {
            _WrappedValue = default;
            _WrappedValue_Accessed = false;
            IsDirty = false;
            DescendantIsDirty = false;
            HasChanged = false;
            DescendantHasChanged = false;
        }
        
        /* Conversion */
        
        public int LazinatorUniqueID => 101;
        
        bool ContainsOpenGenericParameters => false;
        public LazinatorGenericIDType LazinatorGenericID
        {
            get => default;
            set { }
        }
        
        public int LazinatorObjectVersion
        {
            get => -1;
            set => throw new LazinatorSerializationException("Lazinator versioning disabled for WDecimalArray.");
        }
        
        
        public void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
            _WrappedValue_ByteIndex = bytesSoFar;
            bytesSoFar = span.Length;
            _WDecimalArray_EndByteIndex = bytesSoFar;
        }
        
        public void SerializeExistingBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren)
            {
                updateStoredBuffer = false;
            }
            int startPosition = writer.Position;
            WritePropertiesIntoBuffer(ref writer, includeChildrenMode, verifyCleanness, updateStoredBuffer, false);
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
                
                var newBuffer = writer.Slice(startPosition);
                if (_LazinatorMemoryStorage != null)
                {
                    _LazinatorMemoryStorage.DisposeWithThis(newBuffer);
                }
                _LazinatorMemoryStorage = newBuffer;
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
            writer.Write((byte)includeChildrenMode);
            // write properties
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren && !_WrappedValue_Accessed)
            {
                var deserialized = WrappedValue;
            }
            var serializedBytesCopy_WrappedValue = LazinatorMemoryStorage;
            var byteIndexCopy_WrappedValue = _WrappedValue_ByteIndex;
            var byteLengthCopy_WrappedValue = _WrappedValue_ByteLength;
            var copy_WrappedValue = _WrappedValue;
            WriteNonLazinatorObject_WithoutLengthPrefix(
            nonLazinatorObject: _WrappedValue, isBelievedDirty: _WrappedValue_Accessed || (includeChildrenMode != OriginalIncludeChildrenMode),
            isAccessed: _WrappedValue_Accessed, writer: ref writer,
            getChildSliceForFieldFn: () => GetChildSlice(serializedBytesCopy_WrappedValue, byteIndexCopy_WrappedValue, byteLengthCopy_WrappedValue, true, false, null),
            verifyCleanness: false,
            binaryWriterAction: (ref BinaryBufferWriter w, bool v) =>
            ConvertToBytes_decimal_B_b(ref w, copy_WrappedValue, includeChildrenMode, v, updateStoredBuffer));
            if (updateStoredBuffer)
            {
                _WrappedValue_ByteIndex = startOfObjectPosition - startPosition;
            }
            if (updateStoredBuffer)
            {
                _WDecimalArray_EndByteIndex = writer.Position - startPosition;
            }
        }
        
        /* Conversion of supported collections and tuples */
        
        private static decimal[] ConvertFromBytes_decimal_B_b(LazinatorMemory storage)
        {
            if (storage.Length == 0)
            {
                return default(decimal[]);
            }
            ReadOnlySpan<byte> span = storage.Span;
            
            int bytesSoFar = 0;
            int collectionLength = span.ToDecompressedInt(ref bytesSoFar);
            
            decimal[] collection = new decimal[collectionLength];
            for (int itemIndex = 0; itemIndex < collectionLength; itemIndex++)
            {
                decimal item = span.ToDecompressedDecimal(ref bytesSoFar);
                collection[itemIndex] = item;
            }
            
            return collection;
        }
        
        private static void ConvertToBytes_decimal_B_b(ref BinaryBufferWriter writer, decimal[] itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            if (itemToConvert == default(decimal[]))
            {
                return;
            }
            CompressedIntegralTypes.WriteCompressedInt(ref writer, itemToConvert.Length);
            int itemToConvertCount = itemToConvert.Length;
            for (int itemIndex = 0; itemIndex < itemToConvertCount; itemIndex++)
            {
                CompressedDecimal.WriteCompressedDecimal(ref writer, itemToConvert[itemIndex]);
            }
        }
        
        private static decimal[] Clone_decimal_B_b(decimal[] itemToClone)
        {
            if (itemToClone == null)
            {
                return default;
            }
            
            int collectionLength = itemToClone.Length;
            decimal[] collection = new decimal[collectionLength];
            int itemToCloneCount = itemToClone.Length;
            for (int itemIndex = 0; itemIndex < itemToCloneCount; itemIndex++)
            {
                var itemCopied = (decimal) itemToClone[itemIndex];
                collection[itemIndex] = itemCopied;
            }
            return collection;
        }
        
    }
}
