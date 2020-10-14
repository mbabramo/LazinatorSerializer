//7fc8f669-5871-47a9-ea58-69b19abb183a
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.393
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
    public partial struct WFloatArray : ILazinator
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsStruct => true;
        
        /* Property definitions */
        
        int _WrappedValue_ByteIndex;
        private int _WFloatArray_EndByteIndex;
        int _WrappedValue_ByteLength => _WFloatArray_EndByteIndex - _WrappedValue_ByteIndex;
        
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Single[] _WrappedValue;
        public Single[] WrappedValue
        {
            [DebuggerStepThrough]
            get
            {
                if (!_WrappedValue_Accessed)
                {
                    LazinateWrappedValue();
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
        private void LazinateWrappedValue()
        {
            if (LazinatorMemoryStorage.Length == 0)
            {
                _WrappedValue = default(Single[]);
            }else
            {
                LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _WrappedValue_ByteIndex, _WrappedValue_ByteLength, true, false, null);
                _WrappedValue = ConvertFromBytes_float_B_b(childData);
            }
            _WrappedValue_Accessed = true;
        }
        
        
        /* Serialization, deserialization, and object relationships */
        
        public WFloatArray(IncludeChildrenMode originalIncludeChildrenMode = IncludeChildrenMode.IncludeAllChildren) : this()
        {
            OriginalIncludeChildrenMode = originalIncludeChildrenMode;
        }
        
        public WFloatArray(LazinatorMemory serializedBytes, ILazinator parent = null, IncludeChildrenMode originalIncludeChildrenMode = IncludeChildrenMode.IncludeAllChildren, int? lazinatorObjectVersion = null) : this()
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
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public LazinatorParentsCollection LazinatorParents { get; set; }
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public IncludeChildrenMode OriginalIncludeChildrenMode { get; set; }
        
        int Deserialize()
        {
            FreeInMemoryObjects();
            int bytesSoFar = 0;
            ReadOnlySpan<byte> span = LazinatorMemoryStorage.InitialMemory.Span;
            if (span.Length == 0)
            {
                return 0;
            }
            
            int lazinatorLibraryVersion = span.ToDecompressedInt32(ref bytesSoFar);
            
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
            LazinatorMemoryStorage.WriteToBinaryBuffer(ref writer);
            return writer.LazinatorMemory;
        }
        
        LazinatorMemory EncodeToNewBuffer(IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer) 
        {
            int bufferSize = LazinatorMemoryStorage.Length == 0 ? ExpandableBytes.DefaultMinBufferSize : LazinatorMemoryStorage.Length;
            BinaryBufferWriter writer = new BinaryBufferWriter(bufferSize);
            SerializeToExistingBuffer(ref writer, includeChildrenMode, verifyCleanness, updateStoredBuffer);
            return writer.LazinatorMemory;
        }
        
        public ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode = IncludeChildrenMode.IncludeAllChildren, CloneBufferOptions cloneBufferOptions = CloneBufferOptions.IndependentBuffers)
        {
            WFloatArray clone;
            if (cloneBufferOptions == CloneBufferOptions.NoBuffer)
            {
                clone = new WFloatArray(includeChildrenMode);
                clone = (WFloatArray)AssignCloneProperties(clone, includeChildrenMode);
            }
            else
            {
                LazinatorMemory bytes = EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, false, IsDirty, DescendantIsDirty, false, LazinatorMemoryStorage, false, this);
                clone = new WFloatArray(bytes);
            }
            return clone;
        }
        
        ILazinator AssignCloneProperties(ILazinator clone, IncludeChildrenMode includeChildrenMode)
        {
            clone.FreeInMemoryObjects();
            WFloatArray typedClone = (WFloatArray) clone;
            typedClone.WrappedValue = CloneOrChange_float_B_b(WrappedValue, l => l?.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer), false);
            
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
        
        void DeserializeLazinator(LazinatorMemory serializedBytes)
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
        
        public void UpdateStoredBuffer()
        {
            if (!IsDirty && !DescendantIsDirty && LazinatorMemoryStorage.Length > 0 && OriginalIncludeChildrenMode == IncludeChildrenMode.IncludeAllChildren)
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
                LazinatorMemoryStorage.WriteToBinaryBuffer(ref writer);
                LazinatorMemoryStorage = writer.LazinatorMemory;
            }
            OriginalIncludeChildrenMode = IncludeChildrenMode.IncludeAllChildren;
            if (!LazinatorParents.Any())
            {
                previousBuffer.Dispose();
            }
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
                _WrappedValue = (Single[]) CloneOrChange_float_B_b(_WrappedValue, l => l?.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, true), true);
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
        
        
        void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            ReadOnlySpan<byte> span = LazinatorMemoryStorage.InitialMemory.Span;
            ConvertFromBytesForPrimitiveProperties(span, includeChildrenMode, serializedVersionNumber, ref bytesSoFar);
            int lengthForLengths = 0;
            ConvertFromBytesForChildProperties(span, includeChildrenMode, serializedVersionNumber, bytesSoFar + lengthForLengths, ref bytesSoFar);
        }
        
        void ConvertFromBytesForPrimitiveProperties(ReadOnlySpan<byte> span, IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
        }
        
        int ConvertFromBytesForChildProperties(ReadOnlySpan<byte> span, IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, int indexOfFirstChild, ref int bytesSoFar)
        {
            int totalChildrenBytes = 0;
            _WrappedValue_ByteIndex = indexOfFirstChild + totalChildrenBytes;
            totalChildrenBytes = span.Length - bytesSoFar;
            _WFloatArray_EndByteIndex = indexOfFirstChild + totalChildrenBytes;
            return totalChildrenBytes;
        }
        
        public void SerializeToExistingBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            TabbedText.WriteLine($"Initiating serialization of Lazinator.Wrappers.WFloatArray ");
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
        
        void UpdateDeserializedChildren(ref BinaryBufferWriter writer, int startPosition)
        {
            if (_WrappedValue_Accessed && _WrappedValue != null)
            {
                _WrappedValue = (Single[]) CloneOrChange_float_B_b(_WrappedValue, l => l.RemoveBufferInHierarchy(), true);
            }
        }
        
        
        void WritePropertiesIntoBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer, bool includeUniqueID)
        {
            int startPosition = writer.Position;
            TabbedText.WriteLine($"Writing properties for Lazinator.Wrappers.WFloatArray starting at {writer.Position}.");
            TabbedText.WriteLine($"Includes? uniqueID {(LazinatorGenericID.IsEmpty ? LazinatorUniqueID.ToString() : String.Join("","",LazinatorGenericID.TypeAndInnerTypeIDs.ToArray()))} {includeUniqueID}, Lazinator version {Lazinator.Support.LazinatorVersionInfo.LazinatorIntVersion} True, Object version {LazinatorObjectVersion} False, IncludeChildrenMode {includeChildrenMode} True");
            TabbedText.WriteLine($"IsDirty {IsDirty} DescendantIsDirty {DescendantIsDirty} HasParentClass {LazinatorParents.Any()}");
            if (includeUniqueID)
            {
                CompressedIntegralTypes.WriteCompressedInt(ref writer, LazinatorUniqueID);
            }
            
            CompressedIntegralTypes.WriteCompressedInt(ref writer, Lazinator.Support.LazinatorVersionInfo.LazinatorIntVersion);
            writer.Write((byte)includeChildrenMode);
            // write properties
            
            
            int lengthForLengths = 0;
            Span<byte> lengthsSpan = writer.FreeSpan.Slice(0, lengthForLengths);
            writer.Skip(lengthForLengths);
            WriteChildrenPropertiesIntoBuffer(ref writer, includeChildrenMode, verifyCleanness, updateStoredBuffer, includeUniqueID, startPosition, lengthsSpan);
            TabbedText.WriteLine($"Byte {writer.Position} (end of WFloatArray) ");
        }
        
        void WriteChildrenPropertiesIntoBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer, bool includeUniqueID, int startOfObjectPosition, Span<byte> lengthsSpan)
        {
            int startOfChildPosition = 0;
            TabbedText.WriteLine($"Byte {writer.Position}, WrappedValue (accessed? {_WrappedValue_Accessed})");
            TabbedText.Tabs++;
            startOfChildPosition = writer.Position;
            if (updateStoredBuffer)
            {
                _WrappedValue_ByteIndex = writer.Position - startOfObjectPosition;
            }
            if ((includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode) && !_WrappedValue_Accessed)
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
            ConvertToBytes_float_B_b(ref w, copy_WrappedValue, includeChildrenMode, v, updateStoredBuffer));
            TabbedText.Tabs--;
            if (updateStoredBuffer)
            {
                _WFloatArray_EndByteIndex = writer.Position - startOfObjectPosition;
            }
        }
        
        /* Conversion of supported collections and tuples */
        
        private static Single[] ConvertFromBytes_float_B_b(LazinatorMemory storage)
        {
            if (storage.Length == 0)
            {
                return default(Single[]);
            }
            ReadOnlySpan<byte> span = storage.InitialMemory.Span;
            int bytesSoFar = 0;
            int collectionLength = span.ToDecompressedInt32(ref bytesSoFar);
            
            Single[] collection = new float[collectionLength];
            for (int itemIndex = 0; itemIndex < collectionLength; itemIndex++)
            {
                float item = span.ToSingle(ref bytesSoFar);
                collection[itemIndex] = item;
            }
            
            return collection;
        }
        
        private static void ConvertToBytes_float_B_b(ref BinaryBufferWriter writer, Single[] itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            if (itemToConvert == default(Single[]))
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
        
        private static Single[] CloneOrChange_float_B_b(Single[] itemToClone, Func<ILazinator, ILazinator> cloneOrChangeFunc, bool avoidCloningIfPossible)
        {
            if (itemToClone == null)
            {
                return default;
            }
            int collectionLength = itemToClone.Length;
            Single[] collection = new float[collectionLength];
            int itemToCloneCount = itemToClone.Length;
            for (int itemIndex = 0; itemIndex < itemToCloneCount; itemIndex++)
            {
                var itemCopied = (float) itemToClone[itemIndex];
                collection[itemIndex] = itemCopied;
            }
            return collection;
        }
        
    }
}
