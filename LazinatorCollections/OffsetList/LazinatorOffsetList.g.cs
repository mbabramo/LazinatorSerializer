/*Location7885*//*Location7871*///63810280-4007-5708-d75a-35e6b78dfe57
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.380
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

#nullable disable
namespace LazinatorCollections.OffsetList
{/*Location7872*/
    using Lazinator.Attributes;/*Location7873*/
    using Lazinator.Buffers;/*Location7874*/
    using Lazinator.Core;/*Location7875*/
    using Lazinator.Exceptions;/*Location7876*/
    using Lazinator.Support;/*Location7877*/
    using System;/*Location7878*/
    using System.Buffers;/*Location7879*/
    using System.Collections.Generic;/*Location7880*/
    using System.Diagnostics;/*Location7881*/
    using System.IO;/*Location7882*/
    using System.Linq;/*Location7883*/
    using System.Runtime.InteropServices;/*Location7884*/
    using static Lazinator.Core.LazinatorUtilities;
    
    [Autogenerated]
    public sealed partial class LazinatorOffsetList : ILazinator
    {
        /*Location7886*/[DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsStruct => false;
        
        /*Location7887*//* Property definitions */
        
        /*Location7888*/        int _FourByteItems_ByteIndex;
        /*Location7889*/        int _TwoByteItems_ByteIndex;
        /*Location7890*/int _FourByteItems_ByteLength => _TwoByteItems_ByteIndex - _FourByteItems_ByteIndex;
        /*Location7891*/private int _LazinatorOffsetList_EndByteIndex;
        /*Location7892*/int _TwoByteItems_ByteLength => _LazinatorOffsetList_EndByteIndex - _TwoByteItems_ByteIndex;
        
        /*Location7893*/
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        LazinatorFastReadListInt32 _FourByteItems;
        public LazinatorFastReadListInt32 FourByteItems
        {
            [DebuggerStepThrough]
            get
            {
                if (!_FourByteItems_Accessed)
                {
                    Lazinate_FourByteItems();
                } 
                return _FourByteItems;
            }
            [DebuggerStepThrough]
            set
            {
                if (_FourByteItems != null)
                {
                    _FourByteItems.LazinatorParents = _FourByteItems.LazinatorParents.WithRemoved(this);
                }
                if (value != null)
                {
                    value.LazinatorParents = value.LazinatorParents.WithAdded(this);
                }
                
                IsDirty = true;
                DescendantIsDirty = true;
                _FourByteItems = value;
                _FourByteItems_Accessed = true;
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        bool _FourByteItems_Accessed;
        private void Lazinate_FourByteItems()
        {
            if (LazinatorObjectBytes.Length == 0)
            {
                _FourByteItems = null;
            }
            else
            {
                LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _FourByteItems_ByteIndex, _FourByteItems_ByteLength, false, false, null);
                if (childData.Length == 0)
                {
                    _FourByteItems = default;
                }
                else 
                {
                    _FourByteItems = new LazinatorFastReadListInt32(LazinatorConstructorEnum.LazinatorConstructor)
                    {
                        LazinatorParents = new LazinatorParentsCollection(this)
                    };
                    _FourByteItems.DeserializeLazinator(childData);
                }
            }
            
            _FourByteItems_Accessed = true;
        }
        
        /*Location7894*/
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        LazinatorFastReadListInt16 _TwoByteItems;
        public LazinatorFastReadListInt16 TwoByteItems
        {
            [DebuggerStepThrough]
            get
            {
                if (!_TwoByteItems_Accessed)
                {
                    Lazinate_TwoByteItems();
                } 
                return _TwoByteItems;
            }
            [DebuggerStepThrough]
            set
            {
                if (_TwoByteItems != null)
                {
                    _TwoByteItems.LazinatorParents = _TwoByteItems.LazinatorParents.WithRemoved(this);
                }
                if (value != null)
                {
                    value.LazinatorParents = value.LazinatorParents.WithAdded(this);
                }
                
                IsDirty = true;
                DescendantIsDirty = true;
                _TwoByteItems = value;
                _TwoByteItems_Accessed = true;
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        bool _TwoByteItems_Accessed;
        private void Lazinate_TwoByteItems()
        {
            if (LazinatorObjectBytes.Length == 0)
            {
                _TwoByteItems = null;
            }
            else
            {
                LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _TwoByteItems_ByteIndex, _TwoByteItems_ByteLength, false, false, null);
                if (childData.Length == 0)
                {
                    _TwoByteItems = default;
                }
                else 
                {
                    _TwoByteItems = new LazinatorFastReadListInt16(LazinatorConstructorEnum.LazinatorConstructor)
                    {
                        LazinatorParents = new LazinatorParentsCollection(this)
                    };
                    _TwoByteItems.DeserializeLazinator(childData);
                }
            }
            
            _TwoByteItems_Accessed = true;
        }
        
        /*Location7897*/
        /* Serialization, deserialization, and object relationships */
        
        public LazinatorOffsetList(LazinatorConstructorEnum constructorEnum)
        {
        }
        
        public LazinatorOffsetList(LazinatorMemory serializedBytes, ILazinator parent = null)
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
            
            int uniqueID = span.ToDecompressedInt(ref bytesSoFar);
            if (uniqueID != LazinatorUniqueID)
            {
                ThrowHelper.ThrowFormatException();
            }
            
            int lazinatorLibraryVersion = span.ToDecompressedInt(ref bytesSoFar);
            
            int serializedVersionNumber = span.ToDecompressedInt(ref bytesSoFar);
            
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
            var clone = new LazinatorOffsetList(LazinatorConstructorEnum.LazinatorConstructor)
            {
                OriginalIncludeChildrenMode = includeChildrenMode
            };
            clone = CompleteClone(this, clone, includeChildrenMode, cloneBufferOptions);
            return clone;
        }
        
        public ILazinator AssignCloneProperties(ILazinator clone, IncludeChildrenMode includeChildrenMode)
        {
            clone.FreeInMemoryObjects();
            LazinatorOffsetList typedClone = (LazinatorOffsetList) clone;
            /*Location7895*/if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                if (FourByteItems == null)
                {
                    typedClone.FourByteItems = null;
                }
                else
                {
                    typedClone.FourByteItems = (LazinatorFastReadListInt32) FourByteItems.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer);
                }
                
            }
            
            /*Location7896*/if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                if (TwoByteItems == null)
                {
                    typedClone.TwoByteItems = null;
                }
                else
                {
                    typedClone.TwoByteItems = (LazinatorFastReadListInt16) TwoByteItems.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer);
                }
                
            }
            
            
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
            get => _IsDirty|| LazinatorObjectBytes.Length == 0;
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
        
        /*Location7898*/
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
        
        /*Location7899*/public IEnumerable<(string propertyName, ILazinator descendant)> EnumerateLazinatorDescendants(Func<ILazinator, bool> matchCriterion, bool stopExploringBelowMatch, Func<ILazinator, bool> exploreCriterion, bool exploreOnlyDeserializedChildren, bool enumerateNulls)
        {
            /*Location7900*/if (enumerateNulls && (!exploreOnlyDeserializedChildren || _FourByteItems_Accessed) && FourByteItems == null)
            {
                yield return ("FourByteItems", default);
            }
            else
            {
                if ((!exploreOnlyDeserializedChildren && FourByteItems != null) || (_FourByteItems_Accessed && _FourByteItems != null))
                {
                    bool isMatch_FourByteItems = matchCriterion == null || matchCriterion(FourByteItems);
                    bool shouldExplore_FourByteItems = exploreCriterion == null || exploreCriterion(FourByteItems);
                    if (isMatch_FourByteItems)
                    {
                        yield return ("FourByteItems", FourByteItems);
                    }
                    if ((!stopExploringBelowMatch || !isMatch_FourByteItems) && shouldExplore_FourByteItems)
                    {
                        foreach (var toYield in FourByteItems.EnumerateLazinatorDescendants(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
                        {
                            yield return ("FourByteItems" + "." + toYield.propertyName, toYield.descendant);
                        }
                    }
                }
                
            }
            
            /*Location7901*/if (enumerateNulls && (!exploreOnlyDeserializedChildren || _TwoByteItems_Accessed) && TwoByteItems == null)
            {
                yield return ("TwoByteItems", default);
            }
            else
            {
                if ((!exploreOnlyDeserializedChildren && TwoByteItems != null) || (_TwoByteItems_Accessed && _TwoByteItems != null))
                {
                    bool isMatch_TwoByteItems = matchCriterion == null || matchCriterion(TwoByteItems);
                    bool shouldExplore_TwoByteItems = exploreCriterion == null || exploreCriterion(TwoByteItems);
                    if (isMatch_TwoByteItems)
                    {
                        yield return ("TwoByteItems", TwoByteItems);
                    }
                    if ((!stopExploringBelowMatch || !isMatch_TwoByteItems) && shouldExplore_TwoByteItems)
                    {
                        foreach (var toYield in TwoByteItems.EnumerateLazinatorDescendants(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
                        {
                            yield return ("TwoByteItems" + "." + toYield.propertyName, toYield.descendant);
                        }
                    }
                }
                
            }
            
            /*Location7902*/yield break;
        }
        /*Location7903*/
        
        public IEnumerable<(string propertyName, object descendant)> EnumerateNonLazinatorProperties()
        {
            /*Location7904*/yield break;
        }
        /*Location7905*/
        public ILazinator ForEachLazinator(Func<ILazinator, ILazinator> changeFunc, bool exploreOnlyDeserializedChildren, bool changeThisLevel)
        {
            /*Location7906*/if ((!exploreOnlyDeserializedChildren && FourByteItems != null) || (_FourByteItems_Accessed && _FourByteItems != null))
            {
                _FourByteItems = (LazinatorFastReadListInt32) _FourByteItems.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, true);
            }
            /*Location7907*/if ((!exploreOnlyDeserializedChildren && TwoByteItems != null) || (_TwoByteItems_Accessed && _TwoByteItems != null))
            {
                _TwoByteItems = (LazinatorFastReadListInt16) _TwoByteItems.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, true);
            }
            /*Location7908*/if (changeThisLevel && changeFunc != null)
            {
                return changeFunc(this);
            }
            return this;
        }
        /*Location7909*/
        public void FreeInMemoryObjects()
        {
            _FourByteItems = default;
            _TwoByteItems = default;
            _FourByteItems_Accessed = _TwoByteItems_Accessed = false;
            IsDirty = false;
            DescendantIsDirty = false;
            HasChanged = false;
            DescendantHasChanged = false;
        }
        /*Location7910*/
        /* Conversion */
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public int LazinatorUniqueID => 200;
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        bool ContainsOpenGenericParameters => false;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public LazinatorGenericIDType LazinatorGenericID => default;
        
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public int LazinatorObjectVersion { get; set; } = 0;
        
        
        /*Location7911*/public void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
            /*Location7912*/_FourByteItems_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            
            /*Location7913*/_TwoByteItems_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            
            /*Location7914*/_LazinatorOffsetList_EndByteIndex = bytesSoFar;
            /*Location7915*/        }
            
            /*Location7916*/public void SerializeExistingBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
            {
                /*Location7917*/if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren)
                {
                    updateStoredBuffer = false;
                }
                /*Location7918*/int startPosition = writer.Position;
                WritePropertiesIntoBuffer(ref writer, includeChildrenMode, verifyCleanness, updateStoredBuffer, true);
                /*Location7919*/if (updateStoredBuffer)
                {
                    UpdateStoredBuffer(ref writer, startPosition, writer.Position - startPosition, includeChildrenMode, false);
                    /*Location7920*/}
                    /*Location7921*/}
                    /*Location7922*/
                    public void UpdateStoredBuffer(ref BinaryBufferWriter writer, int startPosition, int length, IncludeChildrenMode includeChildrenMode, bool updateDeserializedChildren)
                    {
                        /*Location7923*/_IsDirty = false;
                        if (includeChildrenMode == IncludeChildrenMode.IncludeAllChildren)
                        {
                            _DescendantIsDirty = false;/*Location7924*/
                            if (updateDeserializedChildren)
                            {
                                UpdateDeserializedChildren(ref writer, startPosition);
                                /*Location7925*/}
                                /*Location7926*//*Location7927*/
                            }
                            else
                            {
                                ThrowHelper.ThrowCannotUpdateStoredBuffer();
                            }
                            /*Location7928*/
                            var newBuffer = writer.Slice(startPosition, length);
                            LazinatorMemoryStorage = newBuffer;
                            /*Location7929*/}
                            /*Location7930*/
                            void UpdateDeserializedChildren(ref BinaryBufferWriter writer, int startPosition)
                            {
                                /*Location7931*/if (_FourByteItems_Accessed && _FourByteItems != null)
                                {
                                    FourByteItems.UpdateStoredBuffer(ref writer, startPosition + _FourByteItems_ByteIndex + sizeof(int), _FourByteItems_ByteLength - sizeof(int), IncludeChildrenMode.IncludeAllChildren, true);
                                }
                                
                                /*Location7932*/if (_TwoByteItems_Accessed && _TwoByteItems != null)
                                {
                                    TwoByteItems.UpdateStoredBuffer(ref writer, startPosition + _TwoByteItems_ByteIndex + sizeof(int), _TwoByteItems_ByteLength - sizeof(int), IncludeChildrenMode.IncludeAllChildren, true);
                                }
                                
                                /*Location7933*/}
                                
                                /*Location7934*/
                                void WritePropertiesIntoBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer, bool includeUniqueID)
                                {
                                    int startPosition = writer.Position;
                                    int startOfObjectPosition = 0;
                                    // header information
                                    /*Location7935*/if (includeUniqueID)
                                    {
                                        CompressedIntegralTypes.WriteCompressedInt(ref writer, LazinatorUniqueID);
                                    }
                                    
                                    /*Location7936*/CompressedIntegralTypes.WriteCompressedInt(ref writer, Lazinator.Support.LazinatorVersionInfo.LazinatorIntVersion);
                                    CompressedIntegralTypes.WriteCompressedInt(ref writer, LazinatorObjectVersion);
                                    writer.Write((byte)includeChildrenMode);
                                    /*Location7937*/// write properties
                                    /*Location7938*/startOfObjectPosition = writer.Position;
                                    /*Location7939*/if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
                                    {
                                        if ((includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode) && !_FourByteItems_Accessed)
                                        {
                                            var deserialized = FourByteItems;
                                        }
                                        WriteChild(ref writer, ref _FourByteItems, includeChildrenMode, _FourByteItems_Accessed, () => GetChildSlice(LazinatorMemoryStorage, _FourByteItems_ByteIndex, _FourByteItems_ByteLength, false, false, null), verifyCleanness, updateStoredBuffer, false, false, this);
                                    }
                                    
                                    /*Location7940*/if (updateStoredBuffer)
                                    {
                                        _FourByteItems_ByteIndex = startOfObjectPosition - startPosition;
                                    }
                                    /*Location7941*/startOfObjectPosition = writer.Position;
                                    /*Location7942*/if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
                                    {
                                        if ((includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode) && !_TwoByteItems_Accessed)
                                        {
                                            var deserialized = TwoByteItems;
                                        }
                                        WriteChild(ref writer, ref _TwoByteItems, includeChildrenMode, _TwoByteItems_Accessed, () => GetChildSlice(LazinatorMemoryStorage, _TwoByteItems_ByteIndex, _TwoByteItems_ByteLength, false, false, null), verifyCleanness, updateStoredBuffer, false, false, this);
                                    }
                                    
                                    /*Location7943*/if (updateStoredBuffer)
                                    {
                                        _TwoByteItems_ByteIndex = startOfObjectPosition - startPosition;
                                    }
                                    /*Location7944*/if (updateStoredBuffer)
                                    {
                                        /*Location7945*/_LazinatorOffsetList_EndByteIndex = writer.Position - startPosition;
                                        /*Location7946*/}
                                        /*Location7947*/}
                                        /*Location7948*/
                                    }
                                }
