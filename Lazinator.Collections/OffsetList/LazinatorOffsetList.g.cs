//a176d5e8-54e6-7b72-9565-382eddc96b8f
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.395
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

#nullable disable
namespace Lazinator.Collections.OffsetList
{
    using Lazinator.Attributes;
    using Lazinator.Buffers;
    using Lazinator.Core;
    using Lazinator.Exceptions;
    using Lazinator.Support;
    using static Lazinator.Buffers.WriteUncompressedPrimitives;
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
        
        /* Property definitions */
        
        int _FourByteItems_ByteIndex;
        int _TwoByteItems_ByteIndex;
        int _FourByteItems_ByteLength => _TwoByteItems_ByteIndex - _FourByteItems_ByteIndex;
        int _TwoByteItems_ByteLength => (int) (LazinatorMemoryStorage.Length - _TwoByteItems_ByteIndex);
        
        
        LazinatorFastReadListInt32 _FourByteItems;
        public LazinatorFastReadListInt32 FourByteItems
        {
            get
            {
                if (!_FourByteItems_Accessed)
                {
                    LazinateFourByteItems();
                } 
                return _FourByteItems;
            }
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
        private void LazinateFourByteItems()
        {
            if (LazinatorMemoryStorage.Length == 0)
            {
                _FourByteItems = null;
            }
            else
            {
                LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _FourByteItems_ByteIndex, _FourByteItems_ByteLength, null);if (childData.Length == 0)
                {
                    _FourByteItems = default;
                }
                else 
                {
                    _FourByteItems = new LazinatorFastReadListInt32(childData)
                    {
                        LazinatorParents = new LazinatorParentsCollection(this)
                    };
                    
                }
            }
            _FourByteItems_Accessed = true;
        }
        
        
        LazinatorFastReadListInt16 _TwoByteItems;
        public LazinatorFastReadListInt16 TwoByteItems
        {
            get
            {
                if (!_TwoByteItems_Accessed)
                {
                    LazinateTwoByteItems();
                } 
                return _TwoByteItems;
            }
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
        private void LazinateTwoByteItems()
        {
            if (LazinatorMemoryStorage.Length == 0)
            {
                _TwoByteItems = null;
            }
            else
            {
                LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _TwoByteItems_ByteIndex, _TwoByteItems_ByteLength, null);if (childData.Length == 0)
                {
                    _TwoByteItems = default;
                }
                else 
                {
                    _TwoByteItems = new LazinatorFastReadListInt16(childData)
                    {
                        LazinatorParents = new LazinatorParentsCollection(this)
                    };
                    
                }
            }
            _TwoByteItems_Accessed = true;
        }
        
        
        /* Serialization, deserialization, and object relationships */
        
        public LazinatorOffsetList(IncludeChildrenMode originalIncludeChildrenMode = IncludeChildrenMode.IncludeAllChildren)
        {
            OriginalIncludeChildrenMode = originalIncludeChildrenMode;
        }
        
        public LazinatorOffsetList(LazinatorMemory serializedBytes, ILazinator parent = null, IncludeChildrenMode originalIncludeChildrenMode = IncludeChildrenMode.IncludeAllChildren, int? lazinatorObjectVersion = null)
        {
            if (lazinatorObjectVersion != null)
            {
                LazinatorObjectVersion = (int) lazinatorObjectVersion;
            }
            OriginalIncludeChildrenMode = originalIncludeChildrenMode;
            LazinatorParents = new LazinatorParentsCollection(parent);
            DeserializeLazinator(serializedBytes);
            HasChanged = false;
            DescendantHasChanged = false;
        }
        
        public LazinatorParentsCollection LazinatorParents { get; set; }
        
        public LazinatorMemory LazinatorMemoryStorage
        {
            get;
            set;
        }
        
        public IncludeChildrenMode OriginalIncludeChildrenMode { get; set; }
        
        public bool HasChanged { get; set; }
        
        bool _IsDirty;
        public bool IsDirty
        {
            [DebuggerStepThrough]
            get => _IsDirty|| LazinatorMemoryStorage.Length == 0;
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
        
        public bool NonBinaryHash32 => false;
        
        void DeserializeLazinator(LazinatorMemory serializedBytes)
        {
            LazinatorMemoryStorage = serializedBytes;
            int length = Deserialize();
            if (length != LazinatorMemoryStorage.Length)
            {
                LazinatorMemoryStorage = LazinatorMemoryStorage.Slice(0, length);
            }
        }
        
        int Deserialize()
        {
            FreeInMemoryObjects();
            int bytesSoFar = 0;
            ReadOnlySpan<byte> span = LazinatorMemoryStorage.InitialReadOnlyMemory.Span;
            if (span.Length == 0)
            {
                return 0;
            }
            
            int uniqueID = span.ToDecompressedInt32(ref bytesSoFar);
            if (uniqueID != LazinatorUniqueID)
            {
                ThrowHelper.ThrowFormatException();
            }
            
            int lazinatorLibraryVersion = span.ToDecompressedInt32(ref bytesSoFar);
            
            int serializedVersionNumber = span.ToDecompressedInt32(ref bytesSoFar);
            
            OriginalIncludeChildrenMode = (IncludeChildrenMode)span.ToByte(ref bytesSoFar);
            
            int totalBytes = ConvertFromBytesAfterHeader(OriginalIncludeChildrenMode, serializedVersionNumber, ref bytesSoFar);
            return totalBytes;
        }
        
        public void SerializeLazinator()
        {
            if (!IsDirty && !DescendantIsDirty && LazinatorMemoryStorage.Length > 0 && OriginalIncludeChildrenMode == IncludeChildrenMode.IncludeAllChildren)
            {
                return;
                
            }
            var previousBuffer = LazinatorMemoryStorage;
            if (LazinatorMemoryStorage.IsEmpty || IncludeChildrenMode.IncludeAllChildren != OriginalIncludeChildrenMode || (IsDirty || DescendantIsDirty))
            {
                LazinatorMemoryStorage = EncodeToNewBuffer(LazinatorSerializationOptions.Default);
            }
            else
            {
                BufferWriter writer = new BufferWriter(LazinatorMemoryStorage.LengthInt ?? 0);
                LazinatorMemoryStorage.WriteToBuffer(ref writer);
                LazinatorMemoryStorage = writer.LazinatorMemory;
            }
            OriginalIncludeChildrenMode = IncludeChildrenMode.IncludeAllChildren;
            if (!LazinatorParents.Any())
            {
                previousBuffer.Dispose();
            }
        }
        
        public LazinatorMemory SerializeLazinator(in LazinatorSerializationOptions options) 
        {
            if (LazinatorMemoryStorage.IsEmpty || options.IncludeChildrenMode != OriginalIncludeChildrenMode || (options.SerializeDiffs || options.VerifyCleanness || IsDirty || (options.IncludeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && DescendantIsDirty)))
            {
                return EncodeToNewBuffer(options);
            }
            BufferWriter writer = new BufferWriter(LazinatorMemoryStorage.LengthInt ?? 0);
            LazinatorMemoryStorage.WriteToBuffer(ref writer);
            return writer.LazinatorMemory;
        }
        
        LazinatorMemory EncodeToNewBuffer(in LazinatorSerializationOptions options) 
        {
            int bufferSize = LazinatorMemoryStorage.Length == 0 ? ExpandableBytes.DefaultMinBufferSize : LazinatorMemoryStorage.LengthInt ?? ExpandableBytes.DefaultMinBufferSize;
            BufferWriter writer = new BufferWriter(bufferSize);
            SerializeToExistingBuffer(ref writer, options);
            return writer.LazinatorMemory;
        }
        
        public ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode = IncludeChildrenMode.IncludeAllChildren, CloneBufferOptions cloneBufferOptions = CloneBufferOptions.IndependentBuffers)
        {
            LazinatorOffsetList clone;
            if (cloneBufferOptions == CloneBufferOptions.NoBuffer)
            {
                clone = new LazinatorOffsetList(includeChildrenMode);
                clone.LazinatorObjectVersion = LazinatorObjectVersion;
                clone = (LazinatorOffsetList)AssignCloneProperties(clone, includeChildrenMode);
            }
            else
            {
                LazinatorMemory bytes = EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, IsDirty, DescendantIsDirty, false, LazinatorMemoryStorage, this);
                clone = new LazinatorOffsetList(bytes);
            }
            return clone;
        }
        
        ILazinator AssignCloneProperties(ILazinator clone, IncludeChildrenMode includeChildrenMode)
        {
            clone.FreeInMemoryObjects();
            LazinatorOffsetList typedClone = (LazinatorOffsetList) clone;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
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
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
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
            if (enumerateNulls && (!exploreOnlyDeserializedChildren || _TwoByteItems_Accessed) && TwoByteItems == null)
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
            yield break;
        }
        
        
        public IEnumerable<(string propertyName, object descendant)> EnumerateNonLazinatorProperties()
        {
            yield break;
        }
        
        public ILazinator ForEachLazinator(Func<ILazinator, ILazinator> changeFunc, bool exploreOnlyDeserializedChildren, bool changeThisLevel)
        {
            if ((!exploreOnlyDeserializedChildren && FourByteItems != null) || (_FourByteItems_Accessed && _FourByteItems != null))
            {
                _FourByteItems = (LazinatorFastReadListInt32) _FourByteItems.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, true);
            }
            if ((!exploreOnlyDeserializedChildren && TwoByteItems != null) || (_TwoByteItems_Accessed && _TwoByteItems != null))
            {
                _TwoByteItems = (LazinatorFastReadListInt16) _TwoByteItems.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, true);
            }
            if (changeThisLevel && changeFunc != null)
            {
                return changeFunc(this);
            }
            return this;
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
        
        public int LazinatorUniqueID => 200;
        
        bool ContainsOpenGenericParameters => false;
        public LazinatorGenericIDType LazinatorGenericID => default;
        
        
        public int LazinatorObjectVersion { get; set; } = 0;
        
        
        int ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            TabbedText.WriteLine($"");
            TabbedText.WriteLine($"Converting Lazinator.Collections.OffsetList.LazinatorOffsetList at position: " + LazinatorMemoryStorage.ToLocationString());
            ReadOnlySpan<byte> span = LazinatorMemoryStorage.InitialReadOnlyMemory.Span;
            ConvertFromBytesForPrimitiveProperties(span, includeChildrenMode, serializedVersionNumber, ref bytesSoFar);
            TabbedText.Tabs++;
            int lengthForLengths = 0;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                lengthForLengths += 8;
            }
            int totalChildrenSize = ConvertFromBytesForChildLengths(span, includeChildrenMode, serializedVersionNumber, bytesSoFar + lengthForLengths, ref bytesSoFar);;
            TabbedText.Tabs--;
            return bytesSoFar + totalChildrenSize;
        }
        
        void ConvertFromBytesForPrimitiveProperties(ReadOnlySpan<byte> span, IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
        }
        
        int ConvertFromBytesForChildLengths(ReadOnlySpan<byte> span, IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, int indexOfFirstChild, ref int bytesSoFar)
        {
            int totalChildrenBytes = 0;
            TabbedText.WriteLine($"FourByteItems: Length is {bytesSoFar} past above position; start location is {indexOfFirstChild + totalChildrenBytes} past above position"); 
            _FourByteItems_ByteIndex = indexOfFirstChild + totalChildrenBytes;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                totalChildrenBytes += span.ToInt32(ref bytesSoFar);
            }
            TabbedText.WriteLine($"TwoByteItems: Length is {bytesSoFar} past above position; start location is {indexOfFirstChild + totalChildrenBytes} past above position"); 
            _TwoByteItems_ByteIndex = indexOfFirstChild + totalChildrenBytes;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                totalChildrenBytes += span.ToInt32(ref bytesSoFar);
            }
            return totalChildrenBytes;
        }
        
        public void SerializeToExistingBuffer(ref BufferWriter writer, in LazinatorSerializationOptions options)
        {
            TabbedText.WriteLine("");
            TabbedText.WriteLine($"Initiating serialization of Lazinator.Collections.OffsetList.LazinatorOffsetList at position {writer.ToLocationString()}");
            int startPosition = writer.ActiveMemoryPosition;
            WritePropertiesIntoBuffer(ref writer, options, true);
            if (options.UpdateStoredBuffer)
            {
                UpdateStoredBuffer(ref writer, startPosition, writer.ActiveMemoryPosition - startPosition, options.IncludeChildrenMode, false);
            }
        }
        
        public void UpdateStoredBuffer(ref BufferWriter writer, long startPosition, long length, IncludeChildrenMode includeChildrenMode, bool updateDeserializedChildren)
        {
            _IsDirty = false;
            if (includeChildrenMode == IncludeChildrenMode.IncludeAllChildren)
            {
                _DescendantIsDirty = false;
                if (updateDeserializedChildren)
                {
                    UpdateDeserializedChildren(ref writer, startPosition);
                }
                
            }
            else
            {
                ThrowHelper.ThrowCannotUpdateStoredBuffer();
            }
            
            var newBuffer = writer.Slice(startPosition, length);
            LazinatorMemoryStorage = newBuffer;
        }
        
        void UpdateDeserializedChildren(ref BufferWriter writer, long startPosition)
        {
            if (_FourByteItems_Accessed && _FourByteItems != null)
            {
                FourByteItems.UpdateStoredBuffer(ref writer, startPosition + _FourByteItems_ByteIndex, _FourByteItems_ByteLength, IncludeChildrenMode.IncludeAllChildren, true);
            }
            if (_TwoByteItems_Accessed && _TwoByteItems != null)
            {
                TwoByteItems.UpdateStoredBuffer(ref writer, startPosition + _TwoByteItems_ByteIndex, _TwoByteItems_ByteLength, IncludeChildrenMode.IncludeAllChildren, true);
            }
            
        }
        
        
        void WritePropertiesIntoBuffer(ref BufferWriter writer, in LazinatorSerializationOptions options, bool includeUniqueID)
        {
            int startPosition = writer.ActiveMemoryPosition;
            TabbedText.WriteLine($"Writing properties for Lazinator.Collections.OffsetList.LazinatorOffsetList.");
            TabbedText.WriteLine($"Properties uniqueID {(LazinatorGenericID.IsEmpty ? LazinatorUniqueID.ToString() : String.Join(",",LazinatorGenericID.TypeAndInnerTypeIDs.ToArray()))} {(includeUniqueID ? "Included" : "Omitted")}, Lazinator version {Lazinator.Support.LazinatorVersionInfo.LazinatorIntVersion} Included, Object version {LazinatorObjectVersion} Included, IncludeChildrenMode {options.IncludeChildrenMode} Included");
            TabbedText.WriteLine($"IsDirty {IsDirty} DescendantIsDirty {DescendantIsDirty} HasParentClass {LazinatorParents.Any()}");
            if (includeUniqueID)
            {
                CompressedIntegralTypes.WriteCompressedInt(ref writer, LazinatorUniqueID);
            }
            
            CompressedIntegralTypes.WriteCompressedInt(ref writer, Lazinator.Support.LazinatorVersionInfo.LazinatorIntVersion);
            CompressedIntegralTypes.WriteCompressedInt(ref writer, LazinatorObjectVersion);
            writer.Write((byte)options.IncludeChildrenMode);
            // write properties
            
            
            int lengthForLengths = 0;
            if (options.IncludeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && options.IncludeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                lengthForLengths += 8;
            }
            
            var previousLengthsPosition = writer.SetLengthsPosition(lengthForLengths);
            TabbedText.WriteLine($"Location {writer.ToLocationString()}, after skipping {lengthForLengths} bytes to store lengths of child objects");
            WriteChildrenPropertiesIntoBuffer(ref writer, options, includeUniqueID, startPosition);
            writer.ResetLengthsPosition(previousLengthsPosition);
            TabbedText.WriteLine($"Position {writer.ToLocationString()} (end of LazinatorOffsetList) ");
            
        }
        
        void WriteChildrenPropertiesIntoBuffer(ref BufferWriter writer, LazinatorSerializationOptions options, bool includeUniqueID, int startOfObjectPosition)
        {
            if (options.SplittingPossible)
            {
                options = options.WithoutSplittingPossible();
            }
            int startOfChildPosition = 0;
            int lengthValue = 0;
            TabbedText.WriteLine($"Position {writer.ToLocationString()}, FourByteItems (accessed? {_FourByteItems_Accessed}) (backing var null? {_FourByteItems == null}) ");
            TabbedText.Tabs++;
            startOfChildPosition = writer.ActiveMemoryPosition;
            if (options.IncludeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && options.IncludeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                if ((options.IncludeChildrenMode != IncludeChildrenMode.IncludeAllChildren || options.IncludeChildrenMode != OriginalIncludeChildrenMode) && !_FourByteItems_Accessed)
                {
                    var deserialized = FourByteItems;
                }
                WriteChild(ref writer, ref _FourByteItems, options, _FourByteItems_Accessed, () => GetChildSlice(LazinatorMemoryStorage, _FourByteItems_ByteIndex, _FourByteItems_ByteLength, null), this);
                lengthValue = writer.ActiveMemoryPosition - startOfChildPosition;
                if (lengthValue > int.MaxValue)
                {
                    ThrowHelper.ThrowTooLargeException(int.MaxValue);
                }
                writer.RecordLength((int) lengthValue);
            }
            if (options.UpdateStoredBuffer)
            {
                _FourByteItems_ByteIndex = startOfChildPosition - startOfObjectPosition;
                
            }
            TabbedText.Tabs--;
            TabbedText.WriteLine($"Position {writer.ToLocationString()}, TwoByteItems (accessed? {_TwoByteItems_Accessed}) (backing var null? {_TwoByteItems == null}) ");
            TabbedText.Tabs++;
            startOfChildPosition = writer.ActiveMemoryPosition;
            if (options.IncludeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && options.IncludeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                if ((options.IncludeChildrenMode != IncludeChildrenMode.IncludeAllChildren || options.IncludeChildrenMode != OriginalIncludeChildrenMode) && !_TwoByteItems_Accessed)
                {
                    var deserialized = TwoByteItems;
                }
                WriteChild(ref writer, ref _TwoByteItems, options, _TwoByteItems_Accessed, () => GetChildSlice(LazinatorMemoryStorage, _TwoByteItems_ByteIndex, _TwoByteItems_ByteLength, null), this);
                lengthValue = writer.ActiveMemoryPosition - startOfChildPosition;
                if (lengthValue > int.MaxValue)
                {
                    ThrowHelper.ThrowTooLargeException(int.MaxValue);
                }
                writer.RecordLength((int) lengthValue);
            }
            if (options.UpdateStoredBuffer)
            {
                _TwoByteItems_ByteIndex = startOfChildPosition - startOfObjectPosition;
                
            }
            TabbedText.Tabs--;
            
        }
    }
}
