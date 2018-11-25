//5d297483-cfa8-d57b-7577-1d34d552e175
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.328
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Lazinator.Collections
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
    public sealed partial class LazinatorOffsetList : ILazinator
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsStruct => false;
        
        /* Property definitions */
        
        int _FourByteItems_ByteIndex;
        int _TwoByteItems_ByteIndex;
        int _FourByteItems_ByteLength => _TwoByteItems_ByteIndex - _FourByteItems_ByteIndex;
        private int _LazinatorOffsetList_EndByteIndex;
        int _TwoByteItems_ByteLength => _LazinatorOffsetList_EndByteIndex - _TwoByteItems_ByteIndex;
        
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        LazinatorFastReadList<int> _FourByteItems;
        public LazinatorFastReadList<int> FourByteItems
        {
            [DebuggerStepThrough]
            get
            {
                if (!_FourByteItems_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _FourByteItems = default(LazinatorFastReadList<int>);
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
                            _FourByteItems = new LazinatorFastReadList<int>()
                            {
                                LazinatorParents = new LazinatorParentsCollection(this)
                            };
                            _FourByteItems.DeserializeLazinator(childData);
                        }
                    }
                    _FourByteItems_Accessed = true;
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
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        LazinatorFastReadList<short> _TwoByteItems;
        public LazinatorFastReadList<short> TwoByteItems
        {
            [DebuggerStepThrough]
            get
            {
                if (!_TwoByteItems_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _TwoByteItems = default(LazinatorFastReadList<short>);
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
                            _TwoByteItems = new LazinatorFastReadList<short>()
                            {
                                LazinatorParents = new LazinatorParentsCollection(this)
                            };
                            _TwoByteItems.DeserializeLazinator(childData);
                        }
                    }
                    _TwoByteItems_Accessed = true;
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
        
        /* Serialization, deserialization, and object relationships */
        
        public LazinatorOffsetList() : base()
        {
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
                throw new FormatException("Wrong Lazinator type initialized.");
            }
            
            int lazinatorLibraryVersion = span.ToDecompressedInt(ref bytesSoFar);
            
            int serializedVersionNumber = span.ToDecompressedInt(ref bytesSoFar);
            
            OriginalIncludeChildrenMode = (IncludeChildrenMode)span.ToByte(ref bytesSoFar);
            
            ConvertFromBytesAfterHeader(OriginalIncludeChildrenMode, serializedVersionNumber, ref bytesSoFar);
            return bytesSoFar;
        }
        
        public LazinatorMemory SerializeLazinator(IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer) => EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, verifyCleanness, IsDirty, DescendantIsDirty, false, LazinatorMemoryStorage, updateStoredBuffer, this);
        
        public ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode = IncludeChildrenMode.IncludeAllChildren, CloneBufferOptions cloneBufferOptions = CloneBufferOptions.IndependentBuffers)
        {
            var clone = new LazinatorOffsetList()
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
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                if (FourByteItems == null)
                {
                    typedClone.FourByteItems = default(LazinatorFastReadList<int>);
                }
                else
                {
                    typedClone.FourByteItems = (LazinatorFastReadList<int>) FourByteItems.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer);
                }
            }
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                if (TwoByteItems == null)
                {
                    typedClone.TwoByteItems = default(LazinatorFastReadList<short>);
                }
                else
                {
                    typedClone.TwoByteItems = (LazinatorFastReadList<short>) TwoByteItems.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer);
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
        ReadOnlyMemory<byte> LazinatorObjectBytes => LazinatorMemoryStorage.IsEmpty ? LazinatorUtilities.EmptyReadOnlyMemory : LazinatorMemoryStorage.Memory;
        
        public void UpdateStoredBuffer(bool disposePreviousBuffer = false)
        {
            if (!IsDirty && !DescendantIsDirty && LazinatorObjectBytes.Length > 0 && OriginalIncludeChildrenMode == IncludeChildrenMode.IncludeAllChildren)
            {
                return;
            }
            var previousBuffer = LazinatorMemoryStorage;
            LazinatorMemoryStorage = EncodeOrRecycleToNewBuffer(IncludeChildrenMode.IncludeAllChildren, OriginalIncludeChildrenMode, false, IsDirty, DescendantIsDirty, false, previousBuffer, true, this);
            OriginalIncludeChildrenMode = IncludeChildrenMode.IncludeAllChildren;
            if (disposePreviousBuffer)
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
            if (enumerateNulls && (!exploreOnlyDeserializedChildren || _FourByteItems_Accessed) && FourByteItems == null)
            {
                yield return ("FourByteItems", default);
            }
            else
            {
                if ((!exploreOnlyDeserializedChildren && FourByteItems != null) || (_FourByteItems_Accessed && _FourByteItems != null))
                {
                    bool isMatch = matchCriterion == null || matchCriterion(FourByteItems);
                    bool shouldExplore = exploreCriterion == null || exploreCriterion(FourByteItems);
                    if (isMatch)
                    {
                        yield return ("FourByteItems", FourByteItems);
                    }
                    if ((!stopExploringBelowMatch || !isMatch) && shouldExplore)
                    {
                        foreach (var toYield in FourByteItems.EnumerateLazinatorDescendants(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
                        {
                            yield return ("FourByteItems" + "." + toYield.propertyName, toYield.descendant);
                        }
                    }
                }
            }
            if (enumerateNulls && (!exploreOnlyDeserializedChildren || _TwoByteItems_Accessed) && TwoByteItems == null)
            {
                yield return ("TwoByteItems", default);
            }
            else
            {
                if ((!exploreOnlyDeserializedChildren && TwoByteItems != null) || (_TwoByteItems_Accessed && _TwoByteItems != null))
                {
                    bool isMatch = matchCriterion == null || matchCriterion(TwoByteItems);
                    bool shouldExplore = exploreCriterion == null || exploreCriterion(TwoByteItems);
                    if (isMatch)
                    {
                        yield return ("TwoByteItems", TwoByteItems);
                    }
                    if ((!stopExploringBelowMatch || !isMatch) && shouldExplore)
                    {
                        foreach (var toYield in TwoByteItems.EnumerateLazinatorDescendants(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
                        {
                            yield return ("TwoByteItems" + "." + toYield.propertyName, toYield.descendant);
                        }
                    }
                }
            }
            yield break;
        }
        
        
        public IEnumerable<(string propertyName, object descendant)> EnumerateNonLazinatorProperties()
        {
            yield break;
        }
        
        public ILazinator ForEachLazinator(Func<ILazinator, ILazinator> changeFunc, bool exploreOnlyDeserializedChildren)
        {
            if ((!exploreOnlyDeserializedChildren && FourByteItems != null) || (_FourByteItems_Accessed && _FourByteItems != null))
            {
                _FourByteItems = (LazinatorFastReadList<int>) _FourByteItems.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren);
            }
            if ((!exploreOnlyDeserializedChildren && TwoByteItems != null) || (_TwoByteItems_Accessed && _TwoByteItems != null))
            {
                _TwoByteItems = (LazinatorFastReadList<short>) _TwoByteItems.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren);
            }
            return changeFunc(this);
        }
        
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
        
        /* Conversion */
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public int LazinatorUniqueID => 50;
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        bool ContainsOpenGenericParameters => false;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public LazinatorGenericIDType LazinatorGenericID
        {
            get => default;
            set { }
        }
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public int LazinatorObjectVersion { get; set; } = 0;
        
        
        public void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
            _FourByteItems_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            _TwoByteItems_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            _LazinatorOffsetList_EndByteIndex = bytesSoFar;
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
                    if (_FourByteItems_Accessed && _FourByteItems != null)
                    {
                        _FourByteItems.UpdateStoredBuffer(ref writer, startPosition + _FourByteItems_ByteIndex + sizeof(int), _FourByteItems_ByteLength - sizeof(int), IncludeChildrenMode.IncludeAllChildren, true);
                    }
                    if (_TwoByteItems_Accessed && _TwoByteItems != null)
                    {
                        _TwoByteItems.UpdateStoredBuffer(ref writer, startPosition + _TwoByteItems_ByteIndex + sizeof(int), _TwoByteItems_ByteLength - sizeof(int), IncludeChildrenMode.IncludeAllChildren, true);
                    }
                }
                
            }
            else
            {
                throw new LazinatorSerializationException("Cannot update stored buffer when serializing only some children.");
            }
            
            var newBuffer = writer.Slice(startPosition, length);
            LazinatorMemoryStorage = newBuffer;
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
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode && !_FourByteItems_Accessed)
                {
                    var deserialized = FourByteItems;
                }
                WriteChild(ref writer, ref _FourByteItems, includeChildrenMode, _FourByteItems_Accessed, () => GetChildSlice(LazinatorMemoryStorage, _FourByteItems_ByteIndex, _FourByteItems_ByteLength, false, false, null), verifyCleanness, updateStoredBuffer, false, false, this);
            }
            if (updateStoredBuffer)
            {
                _FourByteItems_ByteIndex = startOfObjectPosition - startPosition;
            }
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode && !_TwoByteItems_Accessed)
                {
                    var deserialized = TwoByteItems;
                }
                WriteChild(ref writer, ref _TwoByteItems, includeChildrenMode, _TwoByteItems_Accessed, () => GetChildSlice(LazinatorMemoryStorage, _TwoByteItems_ByteIndex, _TwoByteItems_ByteLength, false, false, null), verifyCleanness, updateStoredBuffer, false, false, this);
            }
            if (updateStoredBuffer)
            {
                _TwoByteItems_ByteIndex = startOfObjectPosition - startPosition;
            }
            if (updateStoredBuffer)
            {
                _LazinatorOffsetList_EndByteIndex = writer.Position - startPosition;
            }
        }
        
    }
}
