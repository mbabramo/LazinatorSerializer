/*Location12444*//*Location12430*///7a613754-d8e2-cf98-44cd-5ae37bcecc64
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.380
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

#nullable disable
namespace Lazinator.Wrappers
{/*Location12431*/
    using Lazinator.Attributes;/*Location12432*/
    using Lazinator.Buffers;/*Location12433*/
    using Lazinator.Core;/*Location12434*/
    using Lazinator.Exceptions;/*Location12435*/
    using Lazinator.Support;/*Location12436*/
    using System;/*Location12437*/
    using System.Buffers;/*Location12438*/
    using System.Collections.Generic;/*Location12439*/
    using System.Diagnostics;/*Location12440*/
    using System.IO;/*Location12441*/
    using System.Linq;/*Location12442*/
    using System.Runtime.InteropServices;/*Location12443*/
    using static Lazinator.Core.LazinatorUtilities;
    
    [Autogenerated]
    public partial struct WLongArray : ILazinator
    {
        /*Location12445*/[DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsStruct => true;
        
        /*Location12446*//* Property definitions */
        
        /*Location12447*/        int _WrappedValue_ByteIndex;
        /*Location12448*/private int _WLongArray_EndByteIndex;
        /*Location12449*/int _WrappedValue_ByteLength => _WLongArray_EndByteIndex - _WrappedValue_ByteIndex;
        
        /*Location12450*/
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        long[] _WrappedValue;
        public long[] WrappedValue
        {
            [DebuggerStepThrough]
            get
            {
                if (!_WrappedValue_Accessed)
                {
                    Lazinate_WrappedValue();
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
        private void Lazinate_WrappedValue()
        {
            if (LazinatorObjectBytes.Length == 0)
            {
                _WrappedValue = default(long[]);
            }
            else
            {
                LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _WrappedValue_ByteIndex, _WrappedValue_ByteLength, true, false, null);
                _WrappedValue = ConvertFromBytes_long_B_b(childData);
            }
            
            _WrappedValue_Accessed = true;
        }
        
        /*Location12452*/
        /* Serialization, deserialization, and object relationships */
        
        public WLongArray(IncludeChildrenMode originalIncludeChildrenMode) : this()
        {
            OriginalIncludeChildrenMode = originalIncludeChildrenMode;
        }
        
        public WLongArray(LazinatorMemory serializedBytes, ILazinator parent = null) : this()
        {
            LazinatorParents = new LazinatorParentsCollection(parent);
            DeserializeLazinator(serializedBytes);
            HasChanged = false;
            DescendantHasChanged = false;
        }
        
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
            if (LazinatorMemoryStorage.IsEmpty || includeChildrenMode != OriginalIncludeChildrenMode || (verifyCleanness || IsDirty || (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && DescendantIsDirty)))
            {
                return EncodeToNewBuffer(includeChildrenMode, verifyCleanness, updateStoredBuffer);
            }
            BinaryBufferWriter writer = new BinaryBufferWriter(LazinatorMemoryStorage.Length);
            writer.Write(LazinatorMemoryStorage.Span);
            return writer.LazinatorMemory;
        }
        
        LazinatorMemory EncodeToNewBuffer(IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer) 
        {
            int bufferSize = LazinatorMemoryStorage.Length == 0 ? ExpandableBytes.DefaultMinBufferSize : LazinatorMemoryStorage.Length;
            BinaryBufferWriter writer = new BinaryBufferWriter(bufferSize);
            SerializeExistingBuffer(ref writer, includeChildrenMode, verifyCleanness, updateStoredBuffer);
            return writer.LazinatorMemory;
        }
        
        public ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode = IncludeChildrenMode.IncludeAllChildren, CloneBufferOptions cloneBufferOptions = CloneBufferOptions.IndependentBuffers)
        {
            WLongArray clone;
            if (cloneBufferOptions == CloneBufferOptions.NoBuffer)
            {
                clone = new WLongArray(includeChildrenMode);
                clone = (WLongArray)AssignCloneProperties(clone, includeChildrenMode);
            }
            else
            {
                LazinatorMemory bytes = EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, false, IsDirty, DescendantIsDirty, false, LazinatorMemoryStorage, false, this);
                clone = new WLongArray(bytes);
            }
            return clone;
        }
        
        public ILazinator AssignCloneProperties(ILazinator clone, IncludeChildrenMode includeChildrenMode)
        {
            clone.FreeInMemoryObjects();
            WLongArray typedClone = (WLongArray) clone;
            /*Location12451*/typedClone.WrappedValue = CloneOrChange_long_B_b(WrappedValue, l => l?.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer), false);
            
            typedClone.IsDirty = false;
            return typedClone;
        }
        
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
        ReadOnlyMemory<byte> LazinatorObjectBytes => LazinatorMemoryStorage.IsEmpty ? LazinatorMemory.EmptyReadOnlyMemory : LazinatorMemoryStorage.Memory;
        
        public void UpdateStoredBuffer()
        {
            if (!IsDirty && !DescendantIsDirty && LazinatorObjectBytes.Length > 0 && OriginalIncludeChildrenMode == IncludeChildrenMode.IncludeAllChildren)
            {
                return;
            }
            var previousBuffer = LazinatorMemoryStorage;
            if (LazinatorMemoryStorage.IsEmpty || IncludeChildrenMode.IncludeAllChildren != OriginalIncludeChildrenMode || (IsDirty || DescendantIsDirty))
            {
                LazinatorMemoryStorage = EncodeToNewBuffer(IncludeChildrenMode.IncludeAllChildren, false, true);
            }
            else
            {
                BinaryBufferWriter writer = new BinaryBufferWriter(LazinatorMemoryStorage.Length);
                writer.Write(LazinatorMemoryStorage.Span);
                LazinatorMemoryStorage = writer.LazinatorMemory;
            }
            OriginalIncludeChildrenMode = IncludeChildrenMode.IncludeAllChildren;
            if (!LazinatorParents.Any())
            {
                previousBuffer.Dispose();
            }
        }
        
        public int GetByteLength()
        {
            UpdateStoredBuffer();
            return LazinatorObjectBytes.Length;
        }
        
        public bool NonBinaryHash32 => false;
        
        /*Location12453*/
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
        
        /*Location12454*/public IEnumerable<(string propertyName, ILazinator descendant)> EnumerateLazinatorDescendants(Func<ILazinator, bool> matchCriterion, bool stopExploringBelowMatch, Func<ILazinator, bool> exploreCriterion, bool exploreOnlyDeserializedChildren, bool enumerateNulls)
        {
            /*Location12455*/yield break;
        }
        /*Location12456*/
        
        public IEnumerable<(string propertyName, object descendant)> EnumerateNonLazinatorProperties()
        {
            /*Location12457*/yield return ("WrappedValue", (object)WrappedValue);
            /*Location12458*/yield break;
        }
        /*Location12459*/
        public ILazinator ForEachLazinator(Func<ILazinator, ILazinator> changeFunc, bool exploreOnlyDeserializedChildren, bool changeThisLevel)
        {
            /*Location12460*/if ((!exploreOnlyDeserializedChildren && WrappedValue != null) || (_WrappedValue_Accessed && _WrappedValue != null))
            {
                _WrappedValue = (long[]) CloneOrChange_long_B_b(_WrappedValue, l => l?.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, true), true);
            }
            /*Location12461*/if (changeThisLevel && changeFunc != null)
            {
                return changeFunc(this);
            }
            return this;
        }
        /*Location12462*/
        public void FreeInMemoryObjects()
        {
            _WrappedValue = default;
            _WrappedValue_Accessed = false;
            IsDirty = false;
            DescendantIsDirty = false;
            HasChanged = false;
            DescendantHasChanged = false;
        }
        /*Location12463*/
        /* Conversion */
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public int LazinatorUniqueID => 40;
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        bool ContainsOpenGenericParameters => false;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public LazinatorGenericIDType LazinatorGenericID => default;
        
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public int LazinatorObjectVersion
        {
            get => -1;
            set => ThrowHelper.ThrowVersioningDisabledException("WLongArray");
        }
        
        
        /*Location12464*/public void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
            /*Location12465*/_WrappedValue_ByteIndex = bytesSoFar;
            bytesSoFar = span.Length;
            /*Location12466*/_WLongArray_EndByteIndex = bytesSoFar;
            /*Location12467*/        }
            
            /*Location12468*/public void SerializeExistingBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
            {
                /*Location12469*/if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren)
                {
                    updateStoredBuffer = false;
                }
                /*Location12470*/int startPosition = writer.Position;
                WritePropertiesIntoBuffer(ref writer, includeChildrenMode, verifyCleanness, updateStoredBuffer, false);
                /*Location12471*/if (updateStoredBuffer)
                {
                    UpdateStoredBuffer(ref writer, startPosition, writer.Position - startPosition, includeChildrenMode, false);
                    /*Location12472*/}
                    /*Location12473*/}
                    /*Location12474*/
                    public void UpdateStoredBuffer(ref BinaryBufferWriter writer, int startPosition, int length, IncludeChildrenMode includeChildrenMode, bool updateDeserializedChildren)
                    {
                        /*Location12475*/_IsDirty = false;
                        if (includeChildrenMode == IncludeChildrenMode.IncludeAllChildren)
                        {
                            _DescendantIsDirty = false;/*Location12476*/
                            if (updateDeserializedChildren)
                            {
                                UpdateDeserializedChildren(ref writer, startPosition);
                                /*Location12477*/}
                                /*Location12478*//*Location12479*/
                            }
                            else
                            {
                                ThrowHelper.ThrowCannotUpdateStoredBuffer();
                            }
                            /*Location12480*/
                            var newBuffer = writer.Slice(startPosition, length);
                            LazinatorMemoryStorage = newBuffer;
                            /*Location12481*/}
                            /*Location12482*/
                            void UpdateDeserializedChildren(ref BinaryBufferWriter writer, int startPosition)
                            {
                                /*Location12483*/if (_WrappedValue_Accessed && _WrappedValue != null)
                                {
                                    _WrappedValue = (long[]) CloneOrChange_long_B_b(_WrappedValue, l => l.RemoveBufferInHierarchy(), true);
                                }
                                /*Location12484*/}
                                
                                /*Location12485*/
                                void WritePropertiesIntoBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer, bool includeUniqueID)
                                {
                                    int startPosition = writer.Position;
                                    int startOfObjectPosition = 0;
                                    // header information
                                    /*Location12486*/if (includeUniqueID)
                                    {
                                        CompressedIntegralTypes.WriteCompressedInt(ref writer, LazinatorUniqueID);
                                    }
                                    
                                    /*Location12487*/CompressedIntegralTypes.WriteCompressedInt(ref writer, Lazinator.Support.LazinatorVersionInfo.LazinatorIntVersion);
                                    writer.Write((byte)includeChildrenMode);
                                    /*Location12488*/// write properties
                                    /*Location12489*/startOfObjectPosition = writer.Position;
                                    /*Location12490*/if ((includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode) && !_WrappedValue_Accessed)
                                    {
                                        var deserialized = WrappedValue;
                                    }
                                    /*Location12491*/var serializedBytesCopy_WrappedValue = LazinatorMemoryStorage;
                                    var byteIndexCopy_WrappedValue = _WrappedValue_ByteIndex;
                                    var byteLengthCopy_WrappedValue = _WrappedValue_ByteLength;
                                    var copy_WrappedValue = _WrappedValue;
                                    WriteNonLazinatorObject_WithoutLengthPrefix(
                                    nonLazinatorObject: _WrappedValue, isBelievedDirty: _WrappedValue_Accessed || (includeChildrenMode != OriginalIncludeChildrenMode),
                                    isAccessed: _WrappedValue_Accessed, writer: ref writer,
                                    getChildSliceForFieldFn: () => GetChildSlice(serializedBytesCopy_WrappedValue, byteIndexCopy_WrappedValue, byteLengthCopy_WrappedValue, true, false, null),
                                    verifyCleanness: false,
                                    binaryWriterAction: (ref BinaryBufferWriter w, bool v) =>
                                    ConvertToBytes_long_B_b(ref w, copy_WrappedValue, includeChildrenMode, v, updateStoredBuffer));
                                    /*Location12492*/if (updateStoredBuffer)
                                    {
                                        _WrappedValue_ByteIndex = startOfObjectPosition - startPosition;
                                    }
                                    /*Location12493*/if (updateStoredBuffer)
                                    {
                                        /*Location12494*/_WLongArray_EndByteIndex = writer.Position - startPosition;
                                        /*Location12495*/}
                                        /*Location12496*/}
                                        /*Location12497*/
                                        /* Conversion of supported collections and tuples */
                                        /*Location12498*/
                                        private static long[] ConvertFromBytes_long_B_b(LazinatorMemory storage)
                                        {
                                            if (storage.Length == 0)
                                            {
                                                return default(long[]);
                                            }
                                            ReadOnlySpan<byte> span = storage.Span;
                                            int bytesSoFar = 0;
                                            int collectionLength = span.ToDecompressedInt(ref bytesSoFar);
                                            
                                            long[] collection = new long[collectionLength];
                                            for (int itemIndex = 0; itemIndex < collectionLength; itemIndex++)
                                            {
                                                long item = span.ToDecompressedLong(ref bytesSoFar);
                                                collection[itemIndex] = item;
                                            }
                                            
                                            return collection;
                                        }/*Location12499*/
                                        
                                        private static void ConvertToBytes_long_B_b(ref BinaryBufferWriter writer, long[] itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
                                        {
                                            if (itemToConvert == default(long[]))
                                            {
                                                return;
                                            }
                                            CompressedIntegralTypes.WriteCompressedInt(ref writer, itemToConvert.Length);
                                            int itemToConvertCount = itemToConvert.Length;
                                            for (int itemIndex = 0; itemIndex < itemToConvertCount; itemIndex++)
                                            {
                                                CompressedIntegralTypes.WriteCompressedLong(ref writer, itemToConvert[itemIndex]);
                                            }
                                        }
                                        /*Location12500*/
                                        private static long[] CloneOrChange_long_B_b(long[] itemToClone, Func<ILazinator, ILazinator> cloneOrChangeFunc, bool avoidCloningIfPossible)
                                        {
                                            if (itemToClone == null)
                                            {
                                                return default;
                                            }
                                            
                                            int collectionLength = itemToClone.Length;
                                            long[] collection = new long[collectionLength];
                                            int itemToCloneCount = itemToClone.Length;
                                            for (int itemIndex = 0; itemIndex < itemToCloneCount; itemIndex++)
                                            {
                                                var itemCopied = (long) itemToClone[itemIndex];
                                                collection[itemIndex] = itemCopied;
                                            }
                                            return collection;
                                        }
                                        /*Location12501*/
                                    }
                                }
