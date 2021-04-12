//9b055a6b-e1a6-4d68-90d8-632aff90333e
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.393
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

#nullable disable
namespace LazinatorTests.Examples
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
    public partial struct ExampleStructContainingStruct : ILazinator
    {
        public bool IsStruct => true;
        
        /* Property definitions */
        
        int _MyExampleNullableStruct_ByteIndex;
        int _MyExampleStructContainingClasses_ByteIndex;
        int _MyExampleNullableStruct_ByteLength => _MyExampleStructContainingClasses_ByteIndex - _MyExampleNullableStruct_ByteIndex;
        int _MyExampleStructContainingClasses_ByteLength => (int) (LazinatorMemoryStorage.Length - _MyExampleStructContainingClasses_ByteIndex);
        
        
        ExampleStructContainingClasses? _MyExampleNullableStruct;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ExampleStructContainingClasses? MyExampleNullableStruct
        {
            get
            {
                if (!_MyExampleNullableStruct_Accessed)
                {
                    LazinateMyExampleNullableStruct();
                } 
                return _MyExampleNullableStruct;
            }
            set
            {
                
                IsDirty = true;
                DescendantIsDirty = true;
                _MyExampleNullableStruct = value;
                _MyExampleNullableStruct_Accessed = true;
            }
        }
        bool _MyExampleNullableStruct_Accessed;
        private void LazinateMyExampleNullableStruct()
        {
            if (LazinatorMemoryStorage.Length == 0)
            {
                _MyExampleNullableStruct = default(ExampleStructContainingClasses?);
            }
            else
            {
                LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _MyExampleNullableStruct_ByteIndex, _MyExampleNullableStruct_ByteLength, null);if (childData.Length == 0)
                {
                    _MyExampleNullableStruct = default;
                }
                else 
                {
                    _MyExampleNullableStruct = new ExampleStructContainingClasses(childData);
                    
                }
            }
            _MyExampleNullableStruct_Accessed = true;
        }
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ExampleStructContainingClasses? MyExampleNullableStruct_Copy
        {
            get
            {
                if (!_MyExampleNullableStruct_Accessed)
                {
                    if (LazinatorMemoryStorage.Length == 0)
                    {
                        return null;
                    }
                    else
                    {
                        LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _MyExampleNullableStruct_ByteIndex, _MyExampleNullableStruct_ByteLength, null);
                        var toReturn = new ExampleStructContainingClasses(childData);
                        toReturn.IsDirty = false;
                        return toReturn;
                    }
                }
                if (_MyExampleNullableStruct == null)
                {
                    return null;
                }
                var cleanCopy = _MyExampleNullableStruct.Value;
                cleanCopy.IsDirty = false;
                cleanCopy.DescendantIsDirty = false;
                return cleanCopy;
            }
        }
        
        ExampleStructContainingClasses _MyExampleStructContainingClasses;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ExampleStructContainingClasses MyExampleStructContainingClasses
        {
            get
            {
                if (!_MyExampleStructContainingClasses_Accessed)
                {
                    LazinateMyExampleStructContainingClasses();
                } 
                return _MyExampleStructContainingClasses;
            }
            set
            {
                
                IsDirty = true;
                DescendantIsDirty = true;
                _MyExampleStructContainingClasses = value;
                _MyExampleStructContainingClasses_Accessed = true;
            }
        }
        bool _MyExampleStructContainingClasses_Accessed;
        private void LazinateMyExampleStructContainingClasses()
        {
            if (LazinatorMemoryStorage.Length == 0)
            {
                _MyExampleStructContainingClasses = default(ExampleStructContainingClasses);
            }
            else
            {
                LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _MyExampleStructContainingClasses_ByteIndex, _MyExampleStructContainingClasses_ByteLength, null);_MyExampleStructContainingClasses = new ExampleStructContainingClasses(childData);
                
            }
            _MyExampleStructContainingClasses_Accessed = true;
        }
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ExampleStructContainingClasses MyExampleStructContainingClasses_Copy
        {
            get
            {
                if (!_MyExampleStructContainingClasses_Accessed)
                {
                    if (LazinatorMemoryStorage.Length == 0)
                    {
                        return default(ExampleStructContainingClasses);
                    }
                    else
                    {
                        LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _MyExampleStructContainingClasses_ByteIndex, _MyExampleStructContainingClasses_ByteLength, null);
                        var toReturn = new ExampleStructContainingClasses(childData);
                        toReturn.IsDirty = false;
                        return toReturn;
                    }
                }
                var cleanCopy = _MyExampleStructContainingClasses;
                cleanCopy.IsDirty = false;
                cleanCopy.DescendantIsDirty = false;
                return cleanCopy;
            }
        }
        
        /* Serialization, deserialization, and object relationships */
        
        public ExampleStructContainingStruct(IncludeChildrenMode originalIncludeChildrenMode = IncludeChildrenMode.IncludeAllChildren) : this()
        {
            OriginalIncludeChildrenMode = originalIncludeChildrenMode;
        }
        
        public ExampleStructContainingStruct(LazinatorMemory serializedBytes, ILazinator parent = null, IncludeChildrenMode originalIncludeChildrenMode = IncludeChildrenMode.IncludeAllChildren, int? lazinatorObjectVersion = null) : this()
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
            get => _DescendantHasChanged || (_MyExampleNullableStruct_Accessed && (MyExampleNullableStruct.Value.HasChanged || MyExampleNullableStruct.Value.DescendantHasChanged)) || (_MyExampleStructContainingClasses_Accessed && (MyExampleStructContainingClasses.HasChanged || MyExampleStructContainingClasses.DescendantHasChanged));
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
            get => _DescendantIsDirty || (_MyExampleNullableStruct_Accessed && (MyExampleNullableStruct.Value.IsDirty || MyExampleNullableStruct.Value.DescendantIsDirty)) || (_MyExampleStructContainingClasses_Accessed && (MyExampleStructContainingClasses.IsDirty || MyExampleStructContainingClasses.DescendantIsDirty));
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
            ReadOnlySpan<byte> span = LazinatorMemoryStorage.InitialMemory.Span;
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
                LazinatorMemoryStorage.WriteToBinaryBuffer(ref writer);
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
            LazinatorMemoryStorage.WriteToBinaryBuffer(ref writer);
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
            ExampleStructContainingStruct clone;
            if (cloneBufferOptions == CloneBufferOptions.NoBuffer)
            {
                clone = new ExampleStructContainingStruct(includeChildrenMode);
                clone.LazinatorObjectVersion = LazinatorObjectVersion;
                clone = (ExampleStructContainingStruct)AssignCloneProperties(clone, includeChildrenMode);
            }
            else
            {
                LazinatorMemory bytes = EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, IsDirty, DescendantIsDirty, false, LazinatorMemoryStorage, this);
                clone = new ExampleStructContainingStruct(bytes);
            }
            return clone;
        }
        
        ILazinator AssignCloneProperties(ILazinator clone, IncludeChildrenMode includeChildrenMode)
        {
            clone.FreeInMemoryObjects();
            ExampleStructContainingStruct typedClone = (ExampleStructContainingStruct) clone;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                if (MyExampleNullableStruct == null)
                {
                    typedClone.MyExampleNullableStruct = null;
                }
                else
                {
                    typedClone.MyExampleNullableStruct = (ExampleStructContainingClasses?) MyExampleNullableStruct.Value.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer);
                }
            }
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                typedClone.MyExampleStructContainingClasses = (ExampleStructContainingClasses) MyExampleStructContainingClasses.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer);
            }
            
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
            if (enumerateNulls && (!exploreOnlyDeserializedChildren || _MyExampleNullableStruct_Accessed) && MyExampleNullableStruct == null)
            {
                yield return ("MyExampleNullableStruct", default);
            }
            else
            {
                if ((!exploreOnlyDeserializedChildren && MyExampleNullableStruct != null) || (_MyExampleNullableStruct_Accessed && _MyExampleNullableStruct != null))
                {
                    bool isMatch_MyExampleNullableStruct = matchCriterion == null || matchCriterion(MyExampleNullableStruct);
                    bool shouldExplore_MyExampleNullableStruct = exploreCriterion == null || exploreCriterion(MyExampleNullableStruct);
                    if (isMatch_MyExampleNullableStruct)
                    {
                        yield return ("MyExampleNullableStruct", MyExampleNullableStruct);
                    }
                    if ((!stopExploringBelowMatch || !isMatch_MyExampleNullableStruct) && shouldExplore_MyExampleNullableStruct)
                    {
                        foreach (var toYield in MyExampleNullableStruct.Value.EnumerateLazinatorDescendants(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
                        {
                            yield return ("MyExampleNullableStruct" + "." + toYield.propertyName, toYield.descendant);
                        }
                    }
                }
            }
            bool isMatch_MyExampleStructContainingClasses = matchCriterion == null || matchCriterion(MyExampleStructContainingClasses);
            bool shouldExplore_MyExampleStructContainingClasses = exploreCriterion == null || exploreCriterion(MyExampleStructContainingClasses);
            if (isMatch_MyExampleStructContainingClasses)
            {
                yield return ("MyExampleStructContainingClasses", MyExampleStructContainingClasses);
            }
            if ((!stopExploringBelowMatch || !isMatch_MyExampleStructContainingClasses) && shouldExplore_MyExampleStructContainingClasses)
            {
                foreach (var toYield in MyExampleStructContainingClasses.EnumerateLazinatorDescendants(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
                {
                    yield return ("MyExampleStructContainingClasses" + "." + toYield.propertyName, toYield.descendant);
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
            if ((!exploreOnlyDeserializedChildren && MyExampleNullableStruct != null) || (_MyExampleNullableStruct_Accessed && _MyExampleNullableStruct != null))
            {
                _MyExampleNullableStruct = (ExampleStructContainingClasses?) _MyExampleNullableStruct.Value.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, true);
            }
            var deserialized_MyExampleStructContainingClasses = MyExampleStructContainingClasses;
            _MyExampleStructContainingClasses = (ExampleStructContainingClasses) _MyExampleStructContainingClasses.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, true);
            if (changeThisLevel && changeFunc != null)
            {
                return changeFunc(this);
            }
            return this;
        }
        
        public void FreeInMemoryObjects()
        {
            _MyExampleNullableStruct = default;
            _MyExampleStructContainingClasses = default;
            _MyExampleNullableStruct_Accessed = _MyExampleStructContainingClasses_Accessed = false;
            IsDirty = false;
            DescendantIsDirty = false;
            HasChanged = false;
            DescendantHasChanged = false;
        }
        
        /* Conversion */
        
        public int LazinatorUniqueID => 1018;
        
        bool ContainsOpenGenericParameters => false;
        public LazinatorGenericIDType LazinatorGenericID => default;
        
        
        private bool _LazinatorObjectVersionChanged;
        private int _LazinatorObjectVersionOverride;
        public int LazinatorObjectVersion
        {
            get => _LazinatorObjectVersionChanged ? _LazinatorObjectVersionOverride : 0;
            set
            {
                _LazinatorObjectVersionOverride = value;
                _LazinatorObjectVersionChanged = true;
            }
        }
        
        
        int ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            ReadOnlySpan<byte> span = LazinatorMemoryStorage.InitialMemory.Span;
            ConvertFromBytesForPrimitiveProperties(span, includeChildrenMode, serializedVersionNumber, ref bytesSoFar);
            int lengthForLengths = 0;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                lengthForLengths += 8;
            }
            int totalChildrenSize = ConvertFromBytesForChildProperties(span, includeChildrenMode, serializedVersionNumber, bytesSoFar + lengthForLengths, ref bytesSoFar);
            return bytesSoFar + totalChildrenSize;
        }
        
        void ConvertFromBytesForPrimitiveProperties(ReadOnlySpan<byte> span, IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
        }
        
        int ConvertFromBytesForChildProperties(ReadOnlySpan<byte> span, IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, int indexOfFirstChild, ref int bytesSoFar)
        {
            int totalChildrenBytes = 0;
            _MyExampleNullableStruct_ByteIndex = indexOfFirstChild + totalChildrenBytes;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                totalChildrenBytes += span.ToInt32(ref bytesSoFar);
            }
            _MyExampleStructContainingClasses_ByteIndex = indexOfFirstChild + totalChildrenBytes;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                totalChildrenBytes += span.ToInt32(ref bytesSoFar);
            }
            return totalChildrenBytes;
        }
        
        public void SerializeToExistingBuffer(ref BufferWriter writer, in LazinatorSerializationOptions options)
        {
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
                
                _MyExampleNullableStruct_Accessed = false;
                _MyExampleStructContainingClasses_Accessed = false;
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
            if (_MyExampleNullableStruct_Accessed && _MyExampleNullableStruct != null)
            {
                MyExampleNullableStruct.Value.UpdateStoredBuffer(ref writer, startPosition + _MyExampleNullableStruct_ByteIndex, _MyExampleNullableStruct_ByteLength, IncludeChildrenMode.IncludeAllChildren, true);
            }
            MyExampleStructContainingClasses.UpdateStoredBuffer(ref writer, startPosition + _MyExampleStructContainingClasses_ByteIndex, _MyExampleStructContainingClasses_ByteLength, IncludeChildrenMode.IncludeAllChildren, true);
            
        }
        
        
        void WritePropertiesIntoBuffer(ref BufferWriter writer, in LazinatorSerializationOptions options, bool includeUniqueID)
        {
            int startPosition = writer.ActiveMemoryPosition;
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
            
            long previousLengthsPosition = writer.SetLengthsPosition(lengthForLengths);
            WriteChildrenPropertiesIntoBuffer(ref writer, options, includeUniqueID, startPosition);
            writer.ResetLengthsPosition(previousLengthsPosition);
            
        }
        
        void WriteChildrenPropertiesIntoBuffer(ref BufferWriter writer, LazinatorSerializationOptions options, bool includeUniqueID, int startOfObjectPosition)
        {
            if (options.SplittingPossible)
            {
                options = options.WithoutSplittingPossible();
            }
            int startOfChildPosition = 0;
            int lengthValue = 0;
            startOfChildPosition = writer.ActiveMemoryPosition;
            if (options.IncludeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && options.IncludeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                if ((options.IncludeChildrenMode != IncludeChildrenMode.IncludeAllChildren || options.IncludeChildrenMode != OriginalIncludeChildrenMode) && !_MyExampleNullableStruct_Accessed)
                {
                    var deserialized = MyExampleNullableStruct;
                }
                if (_MyExampleNullableStruct == null)
                {
                    WriteNullChild_LengthsSeparate(ref writer, false);
                }
                else
                {
                    var serializedBytesCopy = LazinatorMemoryStorage;
                    var byteIndexCopy = _MyExampleNullableStruct_ByteIndex;
                    var byteLengthCopy = _MyExampleNullableStruct_ByteLength;
                    var copy = _MyExampleNullableStruct.Value;
                    WriteChild(ref writer, ref copy, options, _MyExampleNullableStruct_Accessed, () => GetChildSlice(serializedBytesCopy, byteIndexCopy, byteLengthCopy, null), null);
                    _MyExampleNullableStruct = copy;
                    lengthValue = writer.ActiveMemoryPosition - startOfChildPosition;
                    if (lengthValue > int.MaxValue)
                    {
                        ThrowHelper.ThrowTooLargeException(int.MaxValue);
                    }
                    writer.RecordLength((int) lengthValue);
                }
            }
            if (options.UpdateStoredBuffer)
            {
                _MyExampleNullableStruct_ByteIndex = startOfChildPosition - startOfObjectPosition;
                
            }
            startOfChildPosition = writer.ActiveMemoryPosition;
            if (options.IncludeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && options.IncludeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                if ((options.IncludeChildrenMode != IncludeChildrenMode.IncludeAllChildren || options.IncludeChildrenMode != OriginalIncludeChildrenMode) && !_MyExampleStructContainingClasses_Accessed)
                {
                    var deserialized = MyExampleStructContainingClasses;
                }
                var serializedBytesCopy = LazinatorMemoryStorage;
                var byteIndexCopy = _MyExampleStructContainingClasses_ByteIndex;
                var byteLengthCopy = _MyExampleStructContainingClasses_ByteLength;
                WriteChild(ref writer, ref _MyExampleStructContainingClasses, options, _MyExampleStructContainingClasses_Accessed, () => GetChildSlice(serializedBytesCopy, byteIndexCopy, byteLengthCopy, null), null);
                lengthValue = writer.ActiveMemoryPosition - startOfChildPosition;
                if (lengthValue > int.MaxValue)
                {
                    ThrowHelper.ThrowTooLargeException(int.MaxValue);
                }
                writer.RecordLength((int) lengthValue);
            }
            if (options.UpdateStoredBuffer)
            {
                _MyExampleStructContainingClasses_ByteIndex = startOfChildPosition - startOfObjectPosition;
                
            }
            
        }
    }
}
