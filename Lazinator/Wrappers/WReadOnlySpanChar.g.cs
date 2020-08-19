/*Location12572*//*Location12558*///08eabed3-b113-3320-0ce3-c1e2968d6a86
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
{/*Location12559*/
    using Lazinator.Attributes;/*Location12560*/
    using Lazinator.Buffers;/*Location12561*/
    using Lazinator.Core;/*Location12562*/
    using Lazinator.Exceptions;/*Location12563*/
    using Lazinator.Support;/*Location12564*/
    using System;/*Location12565*/
    using System.Buffers;/*Location12566*/
    using System.Collections.Generic;/*Location12567*/
    using System.Diagnostics;/*Location12568*/
    using System.IO;/*Location12569*/
    using System.Linq;/*Location12570*/
    using System.Runtime.InteropServices;/*Location12571*/
    using static Lazinator.Core.LazinatorUtilities;
    
    [Autogenerated]
    public partial struct WReadOnlySpanChar : ILazinator
    {
        /*Location12573*/[DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsStruct => true;
        
        /*Location12574*//* Property definitions */
        
        /*Location12575*/        int _Value_ByteIndex;
        /*Location12576*/private int _WReadOnlySpanChar_EndByteIndex;
        /*Location12577*/int _Value_ByteLength => _WReadOnlySpanChar_EndByteIndex - _Value_ByteIndex;
        
        /*Location12578*/[DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private ReadOnlyMemory<byte> _Value;
        public ReadOnlySpan<char> Value
        {
            [DebuggerStepThrough]
            get
            {
                if (!_Value_Accessed)
                {
                    LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _Value_ByteIndex, _Value_ByteLength, true, false, null);
                    return MemoryMarshal.Cast<byte, char>(childData.Span);
                }
                
                return MemoryMarshal.Cast<byte, char>(_Value.Span);
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _Value = new ReadOnlyMemory<byte>(MemoryMarshal.Cast<char, byte>(value).ToArray());
                _Value_Accessed = true;
                
            }
        }
        bool _Value_Accessed;
        /*Location12580*/
        /* Serialization, deserialization, and object relationships */
        
        public WReadOnlySpanChar(IncludeChildrenMode originalIncludeChildrenMode) : this()
        {
            OriginalIncludeChildrenMode = originalIncludeChildrenMode;
        }
        
        public WReadOnlySpanChar(LazinatorMemory serializedBytes, ILazinator parent = null) : this()
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
            WReadOnlySpanChar clone;
            if (cloneBufferOptions == CloneBufferOptions.NoBuffer)
            {
                clone = new WReadOnlySpanChar(includeChildrenMode);
                clone = (WReadOnlySpanChar)AssignCloneProperties(clone, includeChildrenMode);
            }
            else
            {
                LazinatorMemory bytes = EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, false, IsDirty, DescendantIsDirty, false, LazinatorMemoryStorage, false, this);
                clone = new WReadOnlySpanChar(bytes);
            }
            return clone;
        }
        
        public ILazinator AssignCloneProperties(ILazinator clone, IncludeChildrenMode includeChildrenMode)
        {
            clone.FreeInMemoryObjects();
            WReadOnlySpanChar typedClone = (WReadOnlySpanChar) clone;
            /*Location12579*/typedClone.Value = CloneOrChange_ReadOnlySpan_Gchar_g(Value, l => l?.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer), false);
            
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
        
        /*Location12581*/
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
        
        /*Location12582*/public IEnumerable<(string propertyName, ILazinator descendant)> EnumerateLazinatorDescendants(Func<ILazinator, bool> matchCriterion, bool stopExploringBelowMatch, Func<ILazinator, bool> exploreCriterion, bool exploreOnlyDeserializedChildren, bool enumerateNulls)
        {
            /*Location12583*/yield break;
        }
        /*Location12584*/
        
        public IEnumerable<(string propertyName, object descendant)> EnumerateNonLazinatorProperties()
        {
            /*Location12585*/yield return ("Value", (object)Value.ToString());
            /*Location12586*/yield break;
        }
        /*Location12587*/
        public ILazinator ForEachLazinator(Func<ILazinator, ILazinator> changeFunc, bool exploreOnlyDeserializedChildren, bool changeThisLevel)
        {
            /*Location12588*/if (!exploreOnlyDeserializedChildren)
            {
                var deserialized_Value = Value;
                if (!_Value_Accessed)
                {
                    Value = deserialized_Value;
                }
            }
            /*Location12589*/if (changeThisLevel && changeFunc != null)
            {
                return changeFunc(this);
            }
            return this;
        }
        /*Location12590*/
        public void FreeInMemoryObjects()
        {
            _Value = default;
            _Value_Accessed = false;
            IsDirty = false;
            DescendantIsDirty = false;
            HasChanged = false;
            DescendantHasChanged = false;
        }
        /*Location12591*/
        /* Conversion */
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public int LazinatorUniqueID => 21;
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        bool ContainsOpenGenericParameters => false;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public LazinatorGenericIDType LazinatorGenericID => default;
        
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public int LazinatorObjectVersion
        {
            get => -1;
            set => ThrowHelper.ThrowVersioningDisabledException("WReadOnlySpanChar");
        }
        
        
        /*Location12592*/public void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
            /*Location12593*/_Value_ByteIndex = bytesSoFar;
            bytesSoFar = span.Length;
            /*Location12594*/_WReadOnlySpanChar_EndByteIndex = bytesSoFar;
            /*Location12595*/        }
            
            /*Location12596*/public void SerializeExistingBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
            {
                /*Location12597*/if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren)
                {
                    updateStoredBuffer = false;
                }
                /*Location12598*/int startPosition = writer.Position;
                WritePropertiesIntoBuffer(ref writer, includeChildrenMode, verifyCleanness, updateStoredBuffer, false);
                /*Location12599*/if (updateStoredBuffer)
                {
                    UpdateStoredBuffer(ref writer, startPosition, writer.Position - startPosition, includeChildrenMode, false);
                    /*Location12600*/}
                    /*Location12601*/}
                    /*Location12602*/
                    public void UpdateStoredBuffer(ref BinaryBufferWriter writer, int startPosition, int length, IncludeChildrenMode includeChildrenMode, bool updateDeserializedChildren)
                    {
                        /*Location12603*/_IsDirty = false;
                        if (includeChildrenMode == IncludeChildrenMode.IncludeAllChildren)
                        {
                            _DescendantIsDirty = false;/*Location12604*/
                            if (updateDeserializedChildren)
                            {
                                UpdateDeserializedChildren(ref writer, startPosition);
                                /*Location12605*/}
                                /*Location12606*//*Location12607*/
                            }
                            else
                            {
                                ThrowHelper.ThrowCannotUpdateStoredBuffer();
                            }
                            /*Location12608*/
                            var newBuffer = writer.Slice(startPosition, length);
                            LazinatorMemoryStorage = newBuffer;
                            /*Location12609*/}
                            /*Location12610*/
                            void UpdateDeserializedChildren(ref BinaryBufferWriter writer, int startPosition)
                            {
                                /*Location12611*/}
                                
                                /*Location12612*/
                                void WritePropertiesIntoBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer, bool includeUniqueID)
                                {
                                    int startPosition = writer.Position;
                                    int startOfObjectPosition = 0;
                                    // header information
                                    /*Location12613*/if (includeUniqueID)
                                    {
                                        CompressedIntegralTypes.WriteCompressedInt(ref writer, LazinatorUniqueID);
                                    }
                                    
                                    /*Location12614*/CompressedIntegralTypes.WriteCompressedInt(ref writer, Lazinator.Support.LazinatorVersionInfo.LazinatorIntVersion);
                                    writer.Write((byte)includeChildrenMode);
                                    /*Location12615*/// write properties
                                    /*Location12616*/startOfObjectPosition = writer.Position;
                                    /*Location12617*/if ((includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode) && !_Value_Accessed)
                                    {
                                        var deserialized = Value;
                                    }
                                    /*Location12618*/var serializedBytesCopy_Value = LazinatorMemoryStorage;
                                    var byteIndexCopy_Value = _Value_ByteIndex;
                                    var byteLengthCopy_Value = _Value_ByteLength;
                                    var copy_Value = _Value;
                                    WriteNonLazinatorObject_WithoutLengthPrefix(
                                    nonLazinatorObject: _Value, isBelievedDirty: _Value_Accessed || (includeChildrenMode != OriginalIncludeChildrenMode),
                                    isAccessed: _Value_Accessed, writer: ref writer,
                                    getChildSliceForFieldFn: () => GetChildSlice(serializedBytesCopy_Value, byteIndexCopy_Value, byteLengthCopy_Value, true, false, null),
                                    verifyCleanness: false,
                                    binaryWriterAction: (ref BinaryBufferWriter w, bool v) =>
                                    copy_Value.Write(ref w));
                                    /*Location12619*/if (updateStoredBuffer)
                                    {
                                        _Value_ByteIndex = startOfObjectPosition - startPosition;
                                    }
                                    /*Location12620*/if (updateStoredBuffer)
                                    {
                                        /*Location12621*/_WReadOnlySpanChar_EndByteIndex = writer.Position - startPosition;
                                        /*Location12622*/}
                                        /*Location12623*/}
                                        /*Location12624*/
                                        /* Conversion of supported collections and tuples */
                                        /*Location12625*/private static ReadOnlySpan<char> CloneOrChange_ReadOnlySpan_Gchar_g(ReadOnlySpan<char> itemToClone, Func<ILazinator, ILazinator> cloneOrChangeFunc, bool avoidCloningIfPossible)
                                        {
                                            var clone = new Span<byte>(new byte[itemToClone.Length * sizeof(char)]);
                                            MemoryMarshal.Cast<char, byte>(itemToClone).CopyTo(clone);
                                            return MemoryMarshal.Cast<byte, char>(clone);
                                        }
                                        /*Location12626*/
                                    }
                                }
