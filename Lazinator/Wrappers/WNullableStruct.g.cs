//76ad30da-fb91-f216-48c2-2523a4939c33
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
    public partial struct WNullableStruct<T> : ILazinator
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsStruct => true;
        
        /* Property definitions */
        
        int _NonNullValue_ByteIndex;
        int _NonNullValue_ByteLength => LazinatorMemoryStorage.Length - _NonNullValue_ByteIndex;
        
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        bool _HasValue;
        public bool HasValue
        {
            [DebuggerStepThrough]
            get
            {
                return _HasValue;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _HasValue = value;
            }
        }
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        T _NonNullValue;
        public T NonNullValue
        {
            [DebuggerStepThrough]
            get
            {
                if (!_NonNullValue_Accessed)
                {
                    LazinateNonNullValue();
                } 
                return _NonNullValue;
            }
            [DebuggerStepThrough]
            set
            {
                
                IsDirty = true;
                DescendantIsDirty = true;
                _NonNullValue = value;
                _NonNullValue_Accessed = true;
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        bool _NonNullValue_Accessed;
        private void LazinateNonNullValue()
        {
            if (LazinatorMemoryStorage.Length == 0)
            {
                _NonNullValue = default(T);
            }else
            {
                LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _NonNullValue_ByteIndex, _NonNullValue_ByteLength, true, false, null);
                
                _NonNullValue = DeserializationFactory.Instance.CreateBasedOnType<T>(childData); 
            }
            _NonNullValue_Accessed = true;
        }
        
        
        /* Serialization, deserialization, and object relationships */
        
        public WNullableStruct(IncludeChildrenMode originalIncludeChildrenMode = IncludeChildrenMode.IncludeAllChildren) : this()
        {
            OriginalIncludeChildrenMode = originalIncludeChildrenMode;
        }
        
        public WNullableStruct(LazinatorMemory serializedBytes, ILazinator parent = null, IncludeChildrenMode originalIncludeChildrenMode = IncludeChildrenMode.IncludeAllChildren, int? lazinatorObjectVersion = null) : this()
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
            
            ReadGenericIDIfApplicable(ContainsOpenGenericParameters, LazinatorUniqueID, span, ref bytesSoFar);
            
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
            WNullableStruct<T> clone;
            if (cloneBufferOptions == CloneBufferOptions.NoBuffer)
            {
                clone = new WNullableStruct<T>(includeChildrenMode);
                clone = (WNullableStruct<T>)AssignCloneProperties(clone, includeChildrenMode);
            }
            else
            {
                LazinatorMemory bytes = EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, false, IsDirty, DescendantIsDirty, false, LazinatorMemoryStorage, false, this);
                clone = new WNullableStruct<T>(bytes);
            }
            return clone;
        }
        
        ILazinator AssignCloneProperties(ILazinator clone, IncludeChildrenMode includeChildrenMode)
        {
            clone.FreeInMemoryObjects();
            WNullableStruct<T> typedClone = (WNullableStruct<T>) clone;
            typedClone.HasValue = HasValue;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                typedClone.NonNullValue = (T) NonNullValue.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer);
            }
            
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
            get => _DescendantHasChanged || (true && (NonNullValue.HasChanged || NonNullValue.DescendantHasChanged));
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
            get => _DescendantIsDirty || (true && (NonNullValue.IsDirty || NonNullValue.DescendantIsDirty));
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
            bool isMatch_NonNullValue = matchCriterion == null || matchCriterion(NonNullValue);
            bool shouldExplore_NonNullValue = exploreCriterion == null || exploreCriterion(NonNullValue);
            if (isMatch_NonNullValue)
            {
                yield return ("NonNullValue", NonNullValue);
            }
            if ((!stopExploringBelowMatch || !isMatch_NonNullValue) && shouldExplore_NonNullValue)
            {
                foreach (var toYield in NonNullValue.EnumerateLazinatorDescendants(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
                {
                    yield return ("NonNullValue" + "." + toYield.propertyName, toYield.descendant);
                }
            }
            yield break;
        }
        
        
        public IEnumerable<(string propertyName, object descendant)> EnumerateNonLazinatorProperties()
        {
            yield return ("HasValue", (object)HasValue);
            yield break;
        }
        
        public ILazinator ForEachLazinator(Func<ILazinator, ILazinator> changeFunc, bool exploreOnlyDeserializedChildren, bool changeThisLevel)
        {
            var deserialized_NonNullValue = NonNullValue;
            _NonNullValue = (T) _NonNullValue.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, true);
            if (changeThisLevel && changeFunc != null)
            {
                return changeFunc(this);
            }
            return this;
        }
        
        public void FreeInMemoryObjects()
        {
            _NonNullValue = default;
            _NonNullValue_Accessed = false;
            IsDirty = false;
            DescendantIsDirty = false;
            HasChanged = false;
            DescendantHasChanged = false;
        }
        
        /* Conversion */
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public int LazinatorUniqueID => 35;
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        bool ContainsOpenGenericParameters => true;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public LazinatorGenericIDType LazinatorGenericID => LazinatorGenericIDType.GetCachedForType<WNullableStruct<T>>(() => DeserializationFactory.Instance.GetUniqueIDListForGenericType(35, new Type[] { typeof(T) }));
        
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public int LazinatorObjectVersion
        {
            get => -1;
            set => ThrowHelper.ThrowVersioningDisabledException("WNullableStruct<T>");
        }
        
        
        void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            ReadOnlySpan<byte> span = LazinatorMemoryStorage.InitialMemory.Span;
            ConvertFromBytesForPrimitiveProperties(span, includeChildrenMode, serializedVersionNumber, ref bytesSoFar);
            int lengthForLengths = 0;
            bytesSoFar += ConvertFromBytesForChildProperties(span, includeChildrenMode, serializedVersionNumber, bytesSoFar + lengthForLengths, ref bytesSoFar);
        }
        
        void ConvertFromBytesForPrimitiveProperties(ReadOnlySpan<byte> span, IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            TabbedText.WriteLine($"Reading HasValue at byte location {bytesSoFar}"); 
            _HasValue = span.ToBoolean(ref bytesSoFar);
        }
        
        int ConvertFromBytesForChildProperties(ReadOnlySpan<byte> span, IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, int indexOfFirstChild, ref int bytesSoFar)
        {
            int totalChildrenBytes = 0;
            TabbedText.WriteLine($"Reading length of NonNullValue at byte location {bytesSoFar} to determine location: {indexOfFirstChild + totalChildrenBytes}"); 
            _NonNullValue_ByteIndex = indexOfFirstChild + totalChildrenBytes;
            
            totalChildrenBytes = span.Length - bytesSoFar;
            return totalChildrenBytes;
        }
        
        public void SerializeToExistingBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            TabbedText.WriteLine($"Initiating serialization of Lazinator.Wrappers.WNullableStruct<T> ");
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
                    UpdateDeserializedChildren(ref writer, startPosition);
                }
                
                if (_NonNullValue_Accessed && _NonNullValue.IsStruct && (_NonNullValue.IsDirty || _NonNullValue.DescendantIsDirty))
                {
                    _NonNullValue_Accessed = false;
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
            NonNullValue.UpdateStoredBuffer(ref writer, startPosition + _NonNullValue_ByteIndex, _NonNullValue_ByteLength, IncludeChildrenMode.IncludeAllChildren, true);
        }
        
        
        void WritePropertiesIntoBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer, bool includeUniqueID)
        {
            int startPosition = writer.Position;
            TabbedText.WriteLine($"Writing properties for Lazinator.Wrappers.WNullableStruct<T> starting at {writer.Position}.");
            TabbedText.WriteLine($"Includes? uniqueID {(LazinatorGenericID.IsEmpty ? LazinatorUniqueID.ToString() : String.Join("","",LazinatorGenericID.TypeAndInnerTypeIDs.ToArray()))} {includeUniqueID}, Lazinator version {Lazinator.Support.LazinatorVersionInfo.LazinatorIntVersion} True, Object version {LazinatorObjectVersion} False, IncludeChildrenMode {includeChildrenMode} True");
            TabbedText.WriteLine($"IsDirty {IsDirty} DescendantIsDirty {DescendantIsDirty} HasParentClass {LazinatorParents.Any()}");
            if (includeUniqueID)
            {
                if (!ContainsOpenGenericParameters)
                {
                    CompressedIntegralTypes.WriteCompressedInt(ref writer, LazinatorUniqueID);
                }
                else
                {
                    WriteLazinatorGenericID(ref writer, LazinatorGenericID);
                }
            }
            CompressedIntegralTypes.WriteCompressedInt(ref writer, Lazinator.Support.LazinatorVersionInfo.LazinatorIntVersion);
            writer.Write((byte)includeChildrenMode);
            // write properties
            
            WritePrimitivePropertiesIntoBuffer(ref writer, includeChildrenMode, verifyCleanness, updateStoredBuffer, includeUniqueID);
            int lengthForLengths = 0;
            Span<byte> lengthsSpan = writer.FreeSpan.Slice(0, lengthForLengths);
            writer.Skip(lengthForLengths);TabbedText.WriteLine($"Byte {writer.Position}, Leaving {lengthForLengths} bytes to store lengths of child objects");
            WriteChildrenPropertiesIntoBuffer(ref writer, includeChildrenMode, verifyCleanness, updateStoredBuffer, includeUniqueID, startPosition, ref lengthsSpan);
            TabbedText.WriteLine($"Byte {writer.Position} (end of WNullableStruct<T>) ");
        }
        
        void WritePrimitivePropertiesIntoBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer, bool includeUniqueID)
        {
            TabbedText.WriteLine($"Byte {writer.Position}, HasValue value {_HasValue}");
            TabbedText.Tabs++;
            WriteUncompressedPrimitives.WriteBool(ref writer, _HasValue);
            TabbedText.Tabs--;
        }
        
        void WriteChildrenPropertiesIntoBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer, bool includeUniqueID, int startOfObjectPosition, ref Span<byte> lengthsSpan)
        {
            int startOfChildPosition = 0;
            TabbedText.WriteLine($"Byte {writer.Position}, NonNullValue value {_NonNullValue}");
            TabbedText.Tabs++;
            startOfChildPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                if ((includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode) && !_NonNullValue_Accessed)
                {
                    var deserialized = NonNullValue;
                }
                var serializedBytesCopy = LazinatorMemoryStorage;
                var byteIndexCopy = _NonNullValue_ByteIndex;
                var byteLengthCopy = _NonNullValue_ByteLength;
                WriteChild(ref writer, ref _NonNullValue, includeChildrenMode, _NonNullValue_Accessed, () => GetChildSlice(serializedBytesCopy, byteIndexCopy, byteLengthCopy, true, false, null), verifyCleanness, updateStoredBuffer, false, true, null);
                
            }
            if (updateStoredBuffer)
            {
                _NonNullValue_ByteIndex = startOfChildPosition - startOfObjectPosition;
                
            }
            TabbedText.Tabs--;
        }
        
    }
}
