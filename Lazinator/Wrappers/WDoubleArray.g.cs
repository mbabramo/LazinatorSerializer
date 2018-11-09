//47a9d77a-6fbe-7a89-723e-0c12004cdff8
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.286
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
    public partial struct WDoubleArray : ILazinator
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsStruct => true;
        
        /* Serialization, deserialization, and object relationships */
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public LazinatorParentsCollection LazinatorParents { get; set; }
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public IncludeChildrenMode OriginalIncludeChildrenMode { get; set; }
        
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
            var clone = new WDoubleArray()
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
        
        void AssignCloneProperties(ref WDoubleArray clone, IncludeChildrenMode includeChildrenMode)
        {
            clone.FreeInMemoryObjects();
            clone.WrappedValue = Clone_double_B_b(WrappedValue, l => l?.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer));
            
            clone.IsDirty = false;}
            
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            public bool HasChanged { get; set; }
            
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            bool _IsDirty;
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
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
                            LazinatorParents.InformParentsOfDirtiness();
                            HasChanged = true;
                        }
                    }
                }
            }
            
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            bool _DescendantHasChanged;
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
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
            
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            bool _DescendantIsDirty;
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
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
                int length = Deserialize();
                if (length != LazinatorMemoryStorage.Length)
                {
                    LazinatorMemoryStorage = LazinatorMemoryStorage.Slice(0, length);
                }
            }
            
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            public LazinatorMemory LazinatorMemoryStorage
            {
                get;
                set;
            }
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            ReadOnlyMemory<byte> LazinatorObjectBytes => LazinatorMemoryStorage?.Memory ?? LazinatorUtilities.EmptyReadOnlyMemory;
            
            public void EnsureLazinatorMemoryUpToDate()
            {
                if (LazinatorMemoryStorage == null)
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
                if (LazinatorMemoryStorage == null)
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
                if (LazinatorMemoryStorage == null)
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
                if (LazinatorMemoryStorage == null)
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
            private int _WDoubleArray_EndByteIndex;
            int _WrappedValue_ByteLength => _WDoubleArray_EndByteIndex - _WrappedValue_ByteIndex;
            
            
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            double[] _WrappedValue;
            public double[] WrappedValue
            {
                [DebuggerStepThrough]
                get
                {
                    if (!_WrappedValue_Accessed)
                    {
                        if (LazinatorObjectBytes.Length == 0)
                        {
                            _WrappedValue = default(double[]);
                        }
                        else
                        {
                            LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _WrappedValue_ByteIndex, _WrappedValue_ByteLength, true, false, null);
                            _WrappedValue = ConvertFromBytes_double_B_b(childData);
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
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
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
            
            public ILazinator ForEachLazinator(Func<ILazinator, ILazinator> changeFunc, bool exploreOnlyDeserializedChildren)
            {
                if ((!exploreOnlyDeserializedChildren && WrappedValue != null) || (_WrappedValue_Accessed && _WrappedValue != null))
                {
                    _WrappedValue = (double[]) Clone_double_B_b(_WrappedValue, l => l.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren));
                }
                return changeFunc(this);
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
            
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            public int LazinatorUniqueID => 102;
            
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            bool ContainsOpenGenericParameters => false;
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            public LazinatorGenericIDType LazinatorGenericID
            {
                get => default;
                set { }
            }
            
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            public int LazinatorObjectVersion
            {
                get => -1;
                set => throw new LazinatorSerializationException("Lazinator versioning disabled for WDoubleArray.");
            }
            
            
            public void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
            {
                ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
                _WrappedValue_ByteIndex = bytesSoFar;
                bytesSoFar = span.Length;
                _WDoubleArray_EndByteIndex = bytesSoFar;
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
                    UpdateStoredBuffer(ref writer, startPosition, writer.Position - startPosition, includeChildrenMode, false);
                }
            }
            
            public void UpdateStoredBuffer(ref BinaryBufferWriter writer, int startPosition, int length, IncludeChildrenMode includeChildrenMode, bool updateDeserializedChildren)
            {
                _IsDirty = false;
                if (includeChildrenMode == IncludeChildrenMode.IncludeAllChildren)
                {
                    _DescendantIsDirty = false;
                    if (updateDeserializedChildren)
                    {
                        if (_WrappedValue_Accessed && _WrappedValue != null)
                        {
                            _WrappedValue = (double[]) Clone_double_B_b(_WrappedValue, l => l.RemoveBufferInHierarchy());
                        }
                    }
                    
                }
                else
                {
                    throw new LazinatorSerializationException("Cannot update stored buffer when serializing only some children.");
                }
                
                var newBuffer = writer.Slice(startPosition, length);
                LazinatorMemoryStorage = ReplaceBuffer(LazinatorMemoryStorage, newBuffer, LazinatorParents, startPosition == 0, IsStruct);
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
                if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode && !_WrappedValue_Accessed)
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
                ConvertToBytes_double_B_b(ref w, copy_WrappedValue, includeChildrenMode, v, updateStoredBuffer));
                if (updateStoredBuffer)
                {
                    _WrappedValue_ByteIndex = startOfObjectPosition - startPosition;
                }
                if (updateStoredBuffer)
                {
                    _WDoubleArray_EndByteIndex = writer.Position - startPosition;
                }
            }
            
            /* Conversion of supported collections and tuples */
            
            private static double[] ConvertFromBytes_double_B_b(LazinatorMemory storage)
            {
                if (storage.Length == 0)
                {
                    return default(double[]);
                }
                storage.LazinatorShouldNotReturnToPool();
                ReadOnlySpan<byte> span = storage.Span;
                
                int bytesSoFar = 0;
                int collectionLength = span.ToDecompressedInt(ref bytesSoFar);
                
                double[] collection = new double[collectionLength];
                for (int itemIndex = 0; itemIndex < collectionLength; itemIndex++)
                {
                    double item = span.ToDouble(ref bytesSoFar);
                    collection[itemIndex] = item;
                }
                
                return collection;
            }
            
            private static void ConvertToBytes_double_B_b(ref BinaryBufferWriter writer, double[] itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
            {
                if (itemToConvert == default(double[]))
                {
                    return;
                }
                CompressedIntegralTypes.WriteCompressedInt(ref writer, itemToConvert.Length);
                int itemToConvertCount = itemToConvert.Length;
                for (int itemIndex = 0; itemIndex < itemToConvertCount; itemIndex++)
                {
                    WriteUncompressedPrimitives.WriteDouble(ref writer, itemToConvert[itemIndex]);
                }
            }
            
            private static double[] Clone_double_B_b(double[] itemToClone, Func<ILazinator, ILazinator> cloneOrChangeFunc)
            {
                if (itemToClone == null)
                {
                    return default;
                }
                
                int collectionLength = itemToClone.Length;
                double[] collection = new double[collectionLength];
                int itemToCloneCount = itemToClone.Length;
                for (int itemIndex = 0; itemIndex < itemToCloneCount; itemIndex++)
                {
                    var itemCopied = (double) itemToClone[itemIndex];
                    collection[itemIndex] = itemCopied;
                }
                return collection;
            }
            
        }
    }
