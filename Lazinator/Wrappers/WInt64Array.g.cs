//cace5c3b-9838-c55f-35ed-c6dfa6975a46
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.395
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

#nullable disable
namespace Lazinator.Wrappers
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
    public partial struct WInt64Array : ILazinator
    {
        public bool IsStruct => true;
        
        /* Property definitions */
        
        int _WrappedValue_ByteIndex;
        int _WrappedValue_ByteLength => (int) (LazinatorMemoryStorage.Length - _WrappedValue_ByteIndex);
        
        
        Int64[] _WrappedValue;
        public Int64[] WrappedValue
        {
            get
            {
                if (!_WrappedValue_Accessed)
                {
                    LazinateWrappedValue();
                }
                IsDirty = true; 
                return _WrappedValue;
            }
            set
            {
                IsDirty = true;
                DescendantIsDirty = true;
                _WrappedValue = value;
                _WrappedValue_Accessed = true;
            }
        }
        bool _WrappedValue_Accessed;
        private void LazinateWrappedValue()
        {
            if (LazinatorMemoryStorage.Length == 0)
            {
                _WrappedValue = default(Int64[]);
            }
            else
            {
                LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _WrappedValue_ByteIndex, _WrappedValue_ByteLength, null);_WrappedValue = ConvertFromBytes_long_B_b(childData);
            }
            _WrappedValue_Accessed = true;
        }
        
        
        /* Serialization, deserialization, and object relationships */
        
        public WInt64Array(IncludeChildrenMode originalIncludeChildrenMode = IncludeChildrenMode.IncludeAllChildren) : this()
        {
            OriginalIncludeChildrenMode = originalIncludeChildrenMode;
        }
        
        public WInt64Array(LazinatorMemory serializedBytes, ILazinator parent = null, IncludeChildrenMode originalIncludeChildrenMode = IncludeChildrenMode.IncludeAllChildren, int? lazinatorObjectVersion = null) : this()
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
            
            int lazinatorLibraryVersion = span.ToDecompressedInt32(ref bytesSoFar);
            
            int serializedVersionNumber = -1; /* versioning disabled */
            
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
            WInt64Array clone;
            if (cloneBufferOptions == CloneBufferOptions.NoBuffer)
            {
                clone = new WInt64Array(includeChildrenMode);
                clone = (WInt64Array)AssignCloneProperties(clone, includeChildrenMode);
            }
            else
            {
                LazinatorMemory bytes = EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, IsDirty, DescendantIsDirty, false, LazinatorMemoryStorage, this);
                clone = new WInt64Array(bytes);
            }
            return clone;
        }
        
        ILazinator AssignCloneProperties(ILazinator clone, IncludeChildrenMode includeChildrenMode)
        {
            clone.FreeInMemoryObjects();
            WInt64Array typedClone = (WInt64Array) clone;
            typedClone.WrappedValue = CloneOrChange_long_B_b(WrappedValue, l => l?.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer), false);
            
            typedClone.IsDirty = false;
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
            yield break;
        }
        
        
        public IEnumerable<(string propertyName, object descendant)> EnumerateNonLazinatorProperties()
        {
            yield return ("WrappedValue", (object)WrappedValue);
            yield break;
        }
        
        public ILazinator ForEachLazinator(Func<ILazinator, ILazinator> changeFunc, bool exploreOnlyDeserializedChildren, bool changeThisLevel)
        {
            if ((!exploreOnlyDeserializedChildren && WrappedValue != null) || (_WrappedValue_Accessed && _WrappedValue != null))
            {
                _WrappedValue = (Int64[]) CloneOrChange_long_B_b(_WrappedValue, l => l?.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, true), true);
            }
            if (changeThisLevel && changeFunc != null)
            {
                return changeFunc(this);
            }
            return this;
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
        
        public int LazinatorUniqueID => 40;
        
        bool ContainsOpenGenericParameters => false;
        public LazinatorGenericIDType LazinatorGenericID => default;
        
        
        public int LazinatorObjectVersion
        {
            get => -1;
            set => ThrowHelper.ThrowVersioningDisabledException("WInt64Array");
        }
        
        
        int ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            ReadOnlySpan<byte> span = LazinatorMemoryStorage.InitialReadOnlyMemory.Span;
            ConvertFromBytesForPrimitiveProperties(span, includeChildrenMode, serializedVersionNumber, ref bytesSoFar);
            int lengthForLengths = 0;
            int totalChildrenSize = ConvertFromBytesForChildProperties(span, includeChildrenMode, serializedVersionNumber, bytesSoFar + lengthForLengths, ref bytesSoFar);
            return bytesSoFar + totalChildrenSize;
        }
        
        void ConvertFromBytesForPrimitiveProperties(ReadOnlySpan<byte> span, IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
        }
        
        int ConvertFromBytesForChildProperties(ReadOnlySpan<byte> span, IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, int indexOfFirstChild, ref int bytesSoFar)
        {
            int totalChildrenBytes = 0;
            TabbedText.WriteLine($"WrappedValue: Length is at {bytesSoFar}; start location is {indexOfFirstChild + totalChildrenBytes}"); 
            _WrappedValue_ByteIndex = indexOfFirstChild + totalChildrenBytes;
            
            totalChildrenBytes = span.Length - bytesSoFar;
            return totalChildrenBytes;
        }
        
        public void SerializeToExistingBuffer(ref BufferWriter writer, in LazinatorSerializationOptions options)
        {
            TabbedText.WriteLine($"Initiating serialization of Lazinator.Wrappers.WInt64Array ");
            int startPosition = writer.ActiveMemoryPosition;
            WritePropertiesIntoBuffer(ref writer, options, false);
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
            if (_WrappedValue_Accessed && _WrappedValue != null)
            {
                _WrappedValue = (Int64[]) CloneOrChange_long_B_b(_WrappedValue, l => l.RemoveBufferInHierarchy(), true);
            }
            
        }
        
        
        void WritePropertiesIntoBuffer(ref BufferWriter writer, in LazinatorSerializationOptions options, bool includeUniqueID)
        {
            int startPosition = writer.ActiveMemoryPosition;
            TabbedText.WriteLine($"Writing properties for Lazinator.Wrappers.WInt64Array starting at {writer.ActiveMemoryPosition}.");
            TabbedText.WriteLine($"Includes? uniqueID {(LazinatorGenericID.IsEmpty ? LazinatorUniqueID.ToString() : String.Join("","",LazinatorGenericID.TypeAndInnerTypeIDs.ToArray()))} {includeUniqueID}, Lazinator version {Lazinator.Support.LazinatorVersionInfo.LazinatorIntVersion} True, Object version {LazinatorObjectVersion} False, IncludeChildrenMode {options.IncludeChildrenMode} True");
            TabbedText.WriteLine($"IsDirty {IsDirty} DescendantIsDirty {DescendantIsDirty} HasParentClass {LazinatorParents.Any()}");
            if (includeUniqueID)
            {
                CompressedIntegralTypes.WriteCompressedInt(ref writer, LazinatorUniqueID);
            }
            
            CompressedIntegralTypes.WriteCompressedInt(ref writer, Lazinator.Support.LazinatorVersionInfo.LazinatorIntVersion);
            writer.Write((byte)options.IncludeChildrenMode);
            // write properties
            
            
            int lengthForLengths = 0;
            
            var previousLengthsPosition = writer.SetLengthsPosition(lengthForLengths);
            TabbedText.WriteLine($"Byte {writer.ActiveMemoryPosition}, Leaving {lengthForLengths} bytes to store lengths of child objects");
            WriteChildrenPropertiesIntoBuffer(ref writer, options, includeUniqueID, startPosition);
            writer.ResetLengthsPosition(previousLengthsPosition);
            TabbedText.WriteLine($"Byte {writer.ActiveMemoryPosition} (end of WInt64Array) ");
            
        }
        
        void WriteChildrenPropertiesIntoBuffer(ref BufferWriter writer, LazinatorSerializationOptions options, bool includeUniqueID, int startOfObjectPosition)
        {
            if (options.SplittingPossible)
            {
                options = options.WithoutSplittingPossible();
            }
            int startOfChildPosition = 0;
            TabbedText.WriteLine($"Byte {writer.ActiveMemoryPosition}, WrappedValue (accessed? {_WrappedValue_Accessed})");
            TabbedText.Tabs++;
            startOfChildPosition = writer.ActiveMemoryPosition;
            if ((options.IncludeChildrenMode != IncludeChildrenMode.IncludeAllChildren || options.IncludeChildrenMode != OriginalIncludeChildrenMode) && !_WrappedValue_Accessed)
            {
                var deserialized = WrappedValue;
            }
            var serializedBytesCopy_WrappedValue = LazinatorMemoryStorage;
            var byteIndexCopy_WrappedValue = _WrappedValue_ByteIndex;
            var byteLengthCopy_WrappedValue = _WrappedValue_ByteLength;
            var copy_WrappedValue = _WrappedValue;
            WriteNonLazinatorObject_WithoutLengthPrefix(
            nonLazinatorObject: _WrappedValue, isBelievedDirty: _WrappedValue_Accessed || (options.IncludeChildrenMode != OriginalIncludeChildrenMode),
            isAccessed: _WrappedValue_Accessed, writer: ref writer,
            getChildSliceForFieldFn: () => GetChildSlice(serializedBytesCopy_WrappedValue, byteIndexCopy_WrappedValue, byteLengthCopy_WrappedValue, null),
            verifyCleanness: false,
            binaryWriterAction: (ref BufferWriter w, bool v) =>
            ConvertToBytes_long_B_b(ref w, copy_WrappedValue, options));
            if (options.UpdateStoredBuffer)
            {
                _WrappedValue_ByteIndex = startOfChildPosition - startOfObjectPosition;
                
            }
            TabbedText.Tabs--;
            
        }
        /* Conversion of supported collections and tuples */
        
        private static Int64[] ConvertFromBytes_long_B_b(LazinatorMemory storage)
        {
            if (storage.Length == 0)
            {
                return default(Int64[]);
            }
            ReadOnlySpan<byte> span = storage.InitialReadOnlyMemory.Span;
            int bytesSoFar = 0;
            int collectionLength = span.ToDecompressedInt32(ref bytesSoFar);
            
            Int64[] collection = new long[collectionLength];
            for (int itemIndex = 0; itemIndex < collectionLength; itemIndex++)
            {
                long item = span.ToDecompressedInt64(ref bytesSoFar);
                collection[itemIndex] = item;
            }
            
            return collection;
        }
        
        private static void ConvertToBytes_long_B_b(ref BufferWriter writer, Int64[] itemToConvert, LazinatorSerializationOptions options)
        {
            if (itemToConvert == default(Int64[]))
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
        
        private static Int64[] CloneOrChange_long_B_b(Int64[] itemToClone, Func<ILazinator, ILazinator> cloneOrChangeFunc, bool avoidCloningIfPossible)
        {
            if (itemToClone == null)
            {
                return default;
            }
            int collectionLength = itemToClone.Length;
            Int64[] collection = new long[collectionLength];
            int itemToCloneCount = itemToClone.Length;
            for (int itemIndex = 0; itemIndex < itemToCloneCount; itemIndex++)
            {
                var itemCopied = (long) itemToClone[itemIndex];
                collection[itemIndex] = itemCopied;
            }
            return collection;
        }
        
    }
}
