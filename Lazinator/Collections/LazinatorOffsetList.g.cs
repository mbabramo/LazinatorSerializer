//219ec1f6-0130-31a4-d2f7-5cc90be6e4f1
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.207
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
        public bool IsStruct => false;
        
        /* Serialization, deserialization, and object relationships */
        
        public LazinatorOffsetList() : base()
        {
        }
        
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
        
        public LazinatorMemory SerializeNewBuffer(IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            return EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, verifyCleanness, IsDirty, DescendantIsDirty, false, LazinatorObjectBytes, (EncodeManuallyDelegate) EncodeToNewBuffer, updateStoredBuffer);
        }
        
        LazinatorMemory EncodeToNewBuffer(IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer) => LazinatorUtilities.EncodeToNewBinaryBufferWriter(this, includeChildrenMode, verifyCleanness, updateStoredBuffer);
        
        public ILazinator CloneLazinator()
        {
            return CloneLazinator(OriginalIncludeChildrenMode);
        }
        
        public ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode, bool updateStoredBuffer = false)
        {
            LazinatorMemory bytes = EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, false, IsDirty, DescendantIsDirty, false, LazinatorObjectBytes, (EncodeManuallyDelegate)EncodeToNewBuffer, updateStoredBuffer);
            var clone = new LazinatorOffsetList()
            {
                LazinatorParents = LazinatorParents,
                OriginalIncludeChildrenMode = includeChildrenMode,
                HierarchyStorage = bytes,
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
        
        private bool _StorageIsHierarchyRoot;
        public LazinatorMemory HierarchyStorage
        {
            set
            {
                _StorageIsHierarchyRoot = true;
                LazinatorMemoryStorage = value;
            }
        }
        
        LazinatorMemory _LazinatorMemoryStorage;
        public LazinatorMemory LazinatorMemoryStorage
        {
            get => _LazinatorMemoryStorage;
            set
            {
                _LazinatorMemoryStorage = value;
                int length = Deserialize();
                _LazinatorMemoryStorage = _LazinatorMemoryStorage.Slice(0, length);
            }
        }
        ReadOnlyMemory<byte> LazinatorObjectBytes => LazinatorMemoryStorage?.Memory ?? LazinatorUtilities.EmptyReadOnlyMemory;
        
        public void EnsureLazinatorMemoryUpToDate()
        {
            if (!IsDirty && !DescendantIsDirty && LazinatorObjectBytes.Length > 0)
            {
                return;
            }
            EncodeOrRecycleToNewBuffer(IncludeChildrenMode.IncludeAllChildren, OriginalIncludeChildrenMode, false, IsDirty, DescendantIsDirty, false, LazinatorObjectBytes, (EncodeManuallyDelegate)EncodeToNewBuffer, true);
        }
        
        public int GetByteLength()
        {
            EnsureLazinatorMemoryUpToDate();
            return LazinatorObjectBytes.Length;
        }
        
        public uint GetBinaryHashCode32()
        {
            EnsureLazinatorMemoryUpToDate();
            return FarmhashByteSpans.Hash32(LazinatorObjectBytes.Span);
        }
        
        public ulong GetBinaryHashCode64()
        {
            EnsureLazinatorMemoryUpToDate();
            return FarmhashByteSpans.Hash64(LazinatorObjectBytes.Span);
        }
        
        public Guid GetBinaryHashCode128()
        {
            EnsureLazinatorMemoryUpToDate();
            return FarmhashByteSpans.Hash128(LazinatorObjectBytes.Span);
        }
        
        /* Property definitions */
        
        int _FourByteItems_ByteIndex;
        int _TwoByteItems_ByteIndex;
        int _FourByteItems_ByteLength => _TwoByteItems_ByteIndex - _FourByteItems_ByteIndex;
        private int _LazinatorOffsetList_EndByteIndex;
        int _TwoByteItems_ByteLength => _LazinatorOffsetList_EndByteIndex - _TwoByteItems_ByteIndex;
        
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
                        else _FourByteItems = new LazinatorFastReadList<int>()
                        {
                            LazinatorParents = new LazinatorParentsCollection(this),
                            LazinatorMemoryStorage = childData,
                        };
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
        bool _FourByteItems_Accessed;
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
                        else _TwoByteItems = new LazinatorFastReadList<short>()
                        {
                            LazinatorParents = new LazinatorParentsCollection(this),
                            LazinatorMemoryStorage = childData,
                        };
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
        bool _TwoByteItems_Accessed;
        
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
            if (enumerateNulls && (FourByteItems == null))
            {
                yield return ("FourByteItems", default);
            }
            else if ((!exploreOnlyDeserializedChildren && FourByteItems != null) || (_FourByteItems_Accessed && _FourByteItems != null))
            {
                foreach (ILazinator toYield in FourByteItems.EnumerateLazinatorNodes(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
                {
                    yield return ("FourByteItems", toYield);
                }
            }
            if (enumerateNulls && (TwoByteItems == null))
            {
                yield return ("TwoByteItems", default);
            }
            else if ((!exploreOnlyDeserializedChildren && TwoByteItems != null) || (_TwoByteItems_Accessed && _TwoByteItems != null))
            {
                foreach (ILazinator toYield in TwoByteItems.EnumerateLazinatorNodes(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
                {
                    yield return ("TwoByteItems", toYield);
                }
            }
            yield break;
        }
        
        
        public IEnumerable<(string propertyName, object descendant)> EnumerateNonLazinatorProperties()
        {
            yield break;
        }
        
        void ResetAccessedProperties()
        {
            _FourByteItems_Accessed = _TwoByteItems_Accessed = false;
            IsDirty = false;
            DescendantIsDirty = false;
            HasChanged = false;
            DescendantHasChanged = false;
        }
        
        /* Conversion */
        
        public int LazinatorUniqueID => 50;
        
        bool ContainsOpenGenericParameters => false;
        public LazinatorGenericIDType LazinatorGenericID
        {
            get => default;
            set { }
        }
        
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
                
                _IsDirty = false;
                if (includeChildrenMode == IncludeChildrenMode.IncludeAllChildren)
                {
                    _DescendantIsDirty = false;
                }
                else
                {
                    throw new Exception("Cannot update stored buffer when serializing only some children.");
                }
                
                _LazinatorMemoryStorage = writer.Slice(startPosition);
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
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren && !_FourByteItems_Accessed)
                {
                    var deserialized = FourByteItems;
                }
                WriteChild(ref writer, _FourByteItems, includeChildrenMode, _FourByteItems_Accessed, () => GetChildSlice(LazinatorMemoryStorage, _FourByteItems_ByteIndex, _FourByteItems_ByteLength, false, false, null), verifyCleanness, updateStoredBuffer, false, false, this);
            }
            if (updateStoredBuffer)
            {
                _FourByteItems_ByteIndex = startOfObjectPosition - startPosition;
            }
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren && !_TwoByteItems_Accessed)
                {
                    var deserialized = TwoByteItems;
                }
                WriteChild(ref writer, _TwoByteItems, includeChildrenMode, _TwoByteItems_Accessed, () => GetChildSlice(LazinatorMemoryStorage, _TwoByteItems_ByteIndex, _TwoByteItems_ByteLength, false, false, null), verifyCleanness, updateStoredBuffer, false, false, this);
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
