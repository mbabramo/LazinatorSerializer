/*Location11150*//*Location11136*///3b6743ca-5fef-a527-c731-f9a2c82d6f6f
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
{/*Location11137*/
    using Lazinator.Attributes;/*Location11138*/
    using Lazinator.Buffers;/*Location11139*/
    using Lazinator.Core;/*Location11140*/
    using Lazinator.Exceptions;/*Location11141*/
    using Lazinator.Support;/*Location11142*/
    using System;/*Location11143*/
    using System.Buffers;/*Location11144*/
    using System.Collections.Generic;/*Location11145*/
    using System.Diagnostics;/*Location11146*/
    using System.IO;/*Location11147*/
    using System.Linq;/*Location11148*/
    using System.Runtime.InteropServices;/*Location11149*/
    using static Lazinator.Core.LazinatorUtilities;
    
    [Autogenerated]
    public partial struct WFloatArray : ILazinator
    {
        /*Location11151*/[DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsStruct => true;
        
        /*Location11152*//* Property definitions */
        
        /*Location11153*/        int _WrappedValue_ByteIndex;
        /*Location11154*/private int _WFloatArray_EndByteIndex;
        /*Location11155*/int _WrappedValue_ByteLength => _WFloatArray_EndByteIndex - _WrappedValue_ByteIndex;
        
        /*Location11156*/
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        float[] _WrappedValue;
        public float[] WrappedValue
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
                _WrappedValue = default(float[]);
            }
            else
            {
                LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _WrappedValue_ByteIndex, _WrappedValue_ByteLength, true, false, null);
                _WrappedValue = ConvertFromBytes_float_B_b(childData);
            }
            
            _WrappedValue_Accessed = true;
        }
        
        /*Location11158*/
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
            var clone = new WFloatArray()
            {
                OriginalIncludeChildrenMode = includeChildrenMode
            };
            clone = CompleteClone(this, clone, includeChildrenMode, cloneBufferOptions);
            return clone;
        }
        
        public ILazinator AssignCloneProperties(ILazinator clone, IncludeChildrenMode includeChildrenMode)
        {
            clone.FreeInMemoryObjects();
            WFloatArray typedClone = (WFloatArray) clone;
            /*Location11157*/typedClone.WrappedValue = CloneOrChange_float_B_b(WrappedValue, l => l.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer), false);
            
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
        
        /*Location11159*/
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
        
        /*Location11160*/public IEnumerable<(string propertyName, ILazinator descendant)> EnumerateLazinatorDescendants(Func<ILazinator, bool> matchCriterion, bool stopExploringBelowMatch, Func<ILazinator, bool> exploreCriterion, bool exploreOnlyDeserializedChildren, bool enumerateNulls)
        {
            /*Location11161*/yield break;
        }
        /*Location11162*/
        
        public IEnumerable<(string propertyName, object descendant)> EnumerateNonLazinatorProperties()
        {
            /*Location11163*/yield return ("WrappedValue", (object)WrappedValue);
            /*Location11164*/yield break;
        }
        /*Location11165*/
        public ILazinator ForEachLazinator(Func<ILazinator, ILazinator> changeFunc, bool exploreOnlyDeserializedChildren, bool changeThisLevel)
        {
            /*Location11166*/if ((!exploreOnlyDeserializedChildren && WrappedValue != null) || (_WrappedValue_Accessed && _WrappedValue != null))
            {
                _WrappedValue = (float[]) CloneOrChange_float_B_b(_WrappedValue, l => l.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, true), true);
            }
            /*Location11167*/if (changeThisLevel && changeFunc != null)
            {
                return changeFunc(this);
            }
            return this;
        }
        /*Location11168*/
        public void FreeInMemoryObjects()
        {
            _WrappedValue = default;
            _WrappedValue_Accessed = false;
            IsDirty = false;
            DescendantIsDirty = false;
            HasChanged = false;
            DescendantHasChanged = false;
        }
        /*Location11169*/
        /* Conversion */
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public int LazinatorUniqueID => 38;
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        bool ContainsOpenGenericParameters => false;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public LazinatorGenericIDType LazinatorGenericID => default;
        
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public int LazinatorObjectVersion
        {
            get => -1;
            set => ThrowHelper.ThrowVersioningDisabledException("WFloatArray");
        }
        
        
        /*Location11170*/public void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
            /*Location11171*/_WrappedValue_ByteIndex = bytesSoFar;
            bytesSoFar = span.Length;
            /*Location11172*/_WFloatArray_EndByteIndex = bytesSoFar;
            /*Location11173*/        }
            
            /*Location11174*/public void SerializeExistingBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
            {
                /*Location11175*/if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren)
                {
                    updateStoredBuffer = false;
                }
                /*Location11176*/int startPosition = writer.Position;
                WritePropertiesIntoBuffer(ref writer, includeChildrenMode, verifyCleanness, updateStoredBuffer, false);
                /*Location11177*/if (updateStoredBuffer)
                {
                    UpdateStoredBuffer(ref writer, startPosition, writer.Position - startPosition, includeChildrenMode, false);
                    /*Location11178*/}
                    /*Location11179*/}
                    /*Location11180*/
                    public void UpdateStoredBuffer(ref BinaryBufferWriter writer, int startPosition, int length, IncludeChildrenMode includeChildrenMode, bool updateDeserializedChildren)
                    {
                        /*Location11181*/_IsDirty = false;
                        if (includeChildrenMode == IncludeChildrenMode.IncludeAllChildren)
                        {
                            _DescendantIsDirty = false;/*Location11182*/
                            if (updateDeserializedChildren)
                            {
                                UpdateDeserializedChildren(ref writer, startPosition);
                                /*Location11183*/}
                                /*Location11184*//*Location11185*/
                            }
                            else
                            {
                                ThrowHelper.ThrowCannotUpdateStoredBuffer();
                            }
                            /*Location11186*/
                            var newBuffer = writer.Slice(startPosition, length);
                            LazinatorMemoryStorage = newBuffer;
                            /*Location11187*/}
                            /*Location11188*/
                            void UpdateDeserializedChildren(ref BinaryBufferWriter writer, int startPosition)
                            {
                                /*Location11189*/if (_WrappedValue_Accessed && _WrappedValue != null)
                                {
                                    _WrappedValue = (float[]) CloneOrChange_float_B_b(_WrappedValue, l => l.RemoveBufferInHierarchy(), true);
                                }
                                /*Location11190*/}
                                
                                /*Location11191*/
                                void WritePropertiesIntoBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer, bool includeUniqueID)
                                {
                                    int startPosition = writer.Position;
                                    int startOfObjectPosition = 0;
                                    // header information
                                    /*Location11192*/if (includeUniqueID)
                                    {
                                        CompressedIntegralTypes.WriteCompressedInt(ref writer, LazinatorUniqueID);
                                    }
                                    
                                    /*Location11193*/CompressedIntegralTypes.WriteCompressedInt(ref writer, Lazinator.Support.LazinatorVersionInfo.LazinatorIntVersion);
                                    writer.Write((byte)includeChildrenMode);
                                    /*Location11194*/// write properties
                                    /*Location11195*/startOfObjectPosition = writer.Position;
                                    /*Location11196*/if ((includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode) && !_WrappedValue_Accessed)
                                    {
                                        var deserialized = WrappedValue;
                                    }
                                    /*Location11197*/var serializedBytesCopy_WrappedValue = LazinatorMemoryStorage;
                                    var byteIndexCopy_WrappedValue = _WrappedValue_ByteIndex;
                                    var byteLengthCopy_WrappedValue = _WrappedValue_ByteLength;
                                    var copy_WrappedValue = _WrappedValue;
                                    WriteNonLazinatorObject_WithoutLengthPrefix(
                                    nonLazinatorObject: _WrappedValue, isBelievedDirty: _WrappedValue_Accessed || (includeChildrenMode != OriginalIncludeChildrenMode),
                                    isAccessed: _WrappedValue_Accessed, writer: ref writer,
                                    getChildSliceForFieldFn: () => GetChildSlice(serializedBytesCopy_WrappedValue, byteIndexCopy_WrappedValue, byteLengthCopy_WrappedValue, true, false, null),
                                    verifyCleanness: false,
                                    binaryWriterAction: (ref BinaryBufferWriter w, bool v) =>
                                    ConvertToBytes_float_B_b(ref w, copy_WrappedValue, includeChildrenMode, v, updateStoredBuffer));
                                    /*Location11198*/if (updateStoredBuffer)
                                    {
                                        _WrappedValue_ByteIndex = startOfObjectPosition - startPosition;
                                    }
                                    /*Location11199*/if (updateStoredBuffer)
                                    {
                                        /*Location11200*/_WFloatArray_EndByteIndex = writer.Position - startPosition;
                                        /*Location11201*/}
                                        /*Location11202*/}
                                        /*Location11203*/
                                        /* Conversion of supported collections and tuples */
                                        /*Location11204*/
                                        private static float[] ConvertFromBytes_float_B_b(LazinatorMemory storage)
                                        {
                                            if (storage.Length == 0)
                                            {
                                                return default(float[]);
                                            }
                                            ReadOnlySpan<byte> span = storage.Span;
                                            
                                            int bytesSoFar = 0;
                                            int collectionLength = span.ToDecompressedInt(ref bytesSoFar);
                                            
                                            float[] collection = new float[collectionLength];
                                            for (int itemIndex = 0; itemIndex < collectionLength; itemIndex++)
                                            {
                                                float item = span.ToSingle(ref bytesSoFar);
                                                collection[itemIndex] = item;
                                            }
                                            
                                            return collection;
                                        }/*Location11205*/
                                        
                                        private static void ConvertToBytes_float_B_b(ref BinaryBufferWriter writer, float[] itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
                                        {
                                            if (itemToConvert == default(float[]))
                                            {
                                                return;
                                            }
                                            CompressedIntegralTypes.WriteCompressedInt(ref writer, itemToConvert.Length);
                                            int itemToConvertCount = itemToConvert.Length;
                                            for (int itemIndex = 0; itemIndex < itemToConvertCount; itemIndex++)
                                            {
                                                WriteUncompressedPrimitives.WriteSingle(ref writer, itemToConvert[itemIndex]);
                                            }
                                        }
                                        /*Location11206*/
                                        private static float[] CloneOrChange_float_B_b(float[] itemToClone, Func<ILazinator, ILazinator> cloneOrChangeFunc, bool avoidCloningIfPossible)
                                        {
                                            if (itemToClone == null)
                                            {
                                                return default;
                                            }
                                            
                                            int collectionLength = itemToClone.Length;
                                            float[] collection = new float[collectionLength];
                                            int itemToCloneCount = itemToClone.Length;
                                            for (int itemIndex = 0; itemIndex < itemToCloneCount; itemIndex++)
                                            {
                                                var itemCopied = (float) itemToClone[itemIndex];
                                                collection[itemIndex] = itemCopied;
                                            }
                                            return collection;
                                        }
                                        /*Location11207*/
                                    }
                                }
