//aef59eff-b2e9-a732-6ee2-4d7da61cd287
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.329
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LazinatorTests.Examples
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
    public partial struct NonLazinatorContainer : ILazinator
    {
        public bool IsStruct => true;
        
        /* Property definitions */
        
        int _NonLazinatorClass_ByteIndex;
        int _NonLazinatorInterchangeableClass_ByteIndex;
        int _NonLazinatorInterchangeableStruct_ByteIndex;
        int _NonLazinatorStruct_ByteIndex;
        int _NonLazinatorClass_ByteLength => _NonLazinatorInterchangeableClass_ByteIndex - _NonLazinatorClass_ByteIndex;
        int _NonLazinatorInterchangeableClass_ByteLength => _NonLazinatorInterchangeableStruct_ByteIndex - _NonLazinatorInterchangeableClass_ByteIndex;
        int _NonLazinatorInterchangeableStruct_ByteLength => _NonLazinatorStruct_ByteIndex - _NonLazinatorInterchangeableStruct_ByteIndex;
        private int _NonLazinatorContainer_EndByteIndex;
        int _NonLazinatorStruct_ByteLength => _NonLazinatorContainer_EndByteIndex - _NonLazinatorStruct_ByteIndex;
        
        
        NonLazinatorClass _NonLazinatorClass;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public NonLazinatorClass NonLazinatorClass
        {
            get
            {
                if (!_NonLazinatorClass_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _NonLazinatorClass = default(NonLazinatorClass);
                    }
                    else
                    {
                        LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _NonLazinatorClass_ByteIndex, _NonLazinatorClass_ByteLength, false, false, null);
                        _NonLazinatorClass = NonLazinatorDirectConverter.ConvertFromBytes_NonLazinatorClass(childData);
                    }
                    _NonLazinatorClass_Accessed = true;
                }
                IsDirty = true; 
                return _NonLazinatorClass;
            }
            set
            {
                IsDirty = true;
                DescendantIsDirty = true;
                _NonLazinatorClass = value;
                _NonLazinatorClass_Accessed = true;
            }
        }
        bool _NonLazinatorClass_Accessed;
        
        NonLazinatorInterchangeableClass _NonLazinatorInterchangeableClass;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public NonLazinatorInterchangeableClass NonLazinatorInterchangeableClass
        {
            get
            {
                if (!_NonLazinatorInterchangeableClass_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _NonLazinatorInterchangeableClass = default(NonLazinatorInterchangeableClass);
                    }
                    else
                    {
                        LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _NonLazinatorInterchangeableClass_ByteIndex, _NonLazinatorInterchangeableClass_ByteLength, false, false, null);
                        _NonLazinatorInterchangeableClass = ConvertFromBytes_NonLazinatorInterchangeableClass(childData);
                    }
                    _NonLazinatorInterchangeableClass_Accessed = true;
                }
                IsDirty = true; 
                return _NonLazinatorInterchangeableClass;
            }
            set
            {
                IsDirty = true;
                DescendantIsDirty = true;
                _NonLazinatorInterchangeableClass = value;
                _NonLazinatorInterchangeableClass_Accessed = true;
            }
        }
        bool _NonLazinatorInterchangeableClass_Accessed;
        
        NonLazinatorInterchangeableStruct _NonLazinatorInterchangeableStruct;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public NonLazinatorInterchangeableStruct NonLazinatorInterchangeableStruct
        {
            get
            {
                if (!_NonLazinatorInterchangeableStruct_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _NonLazinatorInterchangeableStruct = default(NonLazinatorInterchangeableStruct);
                    }
                    else
                    {
                        LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _NonLazinatorInterchangeableStruct_ByteIndex, _NonLazinatorInterchangeableStruct_ByteLength, false, false, null);
                        _NonLazinatorInterchangeableStruct = ConvertFromBytes_NonLazinatorInterchangeableStruct(childData);
                    }
                    _NonLazinatorInterchangeableStruct_Accessed = true;
                } 
                return _NonLazinatorInterchangeableStruct;
            }
            set
            {
                IsDirty = true;
                DescendantIsDirty = true;
                _NonLazinatorInterchangeableStruct = value;
                _NonLazinatorInterchangeableStruct_Accessed = true;
            }
        }
        bool _NonLazinatorInterchangeableStruct_Accessed;
        
        NonLazinatorStruct _NonLazinatorStruct;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public NonLazinatorStruct NonLazinatorStruct
        {
            get
            {
                if (!_NonLazinatorStruct_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _NonLazinatorStruct = default(NonLazinatorStruct);
                    }
                    else
                    {
                        LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _NonLazinatorStruct_ByteIndex, _NonLazinatorStruct_ByteLength, false, false, null);
                        _NonLazinatorStruct = NonLazinatorDirectConverter.ConvertFromBytes_NonLazinatorStruct(childData);
                    }
                    _NonLazinatorStruct_Accessed = true;
                }
                IsDirty = true; 
                return _NonLazinatorStruct;
            }
            set
            {
                IsDirty = true;
                DescendantIsDirty = true;
                _NonLazinatorStruct = value;
                _NonLazinatorStruct_Accessed = true;
            }
        }
        bool _NonLazinatorStruct_Accessed;
        
        /* Serialization, deserialization, and object relationships */
        
        public LazinatorParentsCollection LazinatorParents { get; set; }
        
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
        
        public LazinatorMemory SerializeLazinator(IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer) => EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, verifyCleanness, IsDirty, DescendantIsDirty, false, LazinatorMemoryStorage, updateStoredBuffer, (EncodeManuallyDelegate) EncodeToNewBuffer);
        
        LazinatorMemory EncodeToNewBuffer(IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer) => LazinatorUtilities.EncodeToNewBinaryBufferWriter(this, includeChildrenMode, verifyCleanness, updateStoredBuffer);
        
        public ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode = IncludeChildrenMode.IncludeAllChildren, CloneBufferOptions cloneBufferOptions = CloneBufferOptions.IndependentBuffers)
        {
            var clone = new NonLazinatorContainer()
            {
                OriginalIncludeChildrenMode = includeChildrenMode
            };
            clone = CompleteClone(this, clone, includeChildrenMode, cloneBufferOptions);
            return clone;
        }
        
        public ILazinator AssignCloneProperties(ILazinator clone, IncludeChildrenMode includeChildrenMode)
        {
            clone.FreeInMemoryObjects();
            NonLazinatorContainer typedClone = (NonLazinatorContainer) clone;
            typedClone.NonLazinatorClass = NonLazinatorDirectConverter.CloneOrChange_NonLazinatorClass(NonLazinatorClass, l => l?.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer), false);
            typedClone.NonLazinatorInterchangeableClass = CloneOrChange_NonLazinatorInterchangeableClass(NonLazinatorInterchangeableClass, l => l?.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer), false);
            typedClone.NonLazinatorInterchangeableStruct = CloneOrChange_NonLazinatorInterchangeableStruct(NonLazinatorInterchangeableStruct, l => l?.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer), false);
            typedClone.NonLazinatorStruct = NonLazinatorDirectConverter.CloneOrChange_NonLazinatorStruct(NonLazinatorStruct, l => l?.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer), false);
            
            typedClone.IsDirty = false;
            return typedClone;
        }
        
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
        
        public void DeserializeLazinator(LazinatorMemory serializedBytes)
        {
            LazinatorMemoryStorage = serializedBytes;
            int length = Deserialize();
            if (length != LazinatorMemoryStorage.Length)
            {
                LazinatorMemoryStorage = LazinatorMemoryStorage.Slice(0, length);
            }
        }
        
        public LazinatorMemory LazinatorMemoryStorage
        {
            get;
            set;
        }
        ReadOnlyMemory<byte> LazinatorObjectBytes => LazinatorMemoryStorage.IsEmpty ? LazinatorUtilities.EmptyReadOnlyMemory : LazinatorMemoryStorage.Memory;
        
        public void UpdateStoredBuffer(bool disposePreviousBuffer = false)
        {
            if (!IsDirty && !DescendantIsDirty && LazinatorObjectBytes.Length > 0 && OriginalIncludeChildrenMode == IncludeChildrenMode.IncludeAllChildren)
            {
                return;
            }
            var previousBuffer = LazinatorMemoryStorage;
            LazinatorMemoryStorage = EncodeOrRecycleToNewBuffer(IncludeChildrenMode.IncludeAllChildren, OriginalIncludeChildrenMode, false, IsDirty, DescendantIsDirty, false, previousBuffer, true, (EncodeManuallyDelegate) EncodeToNewBuffer);
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
            yield break;
        }
        
        
        public IEnumerable<(string propertyName, object descendant)> EnumerateNonLazinatorProperties()
        {
            yield return ("NonLazinatorClass", (object)NonLazinatorClass);
            yield return ("NonLazinatorInterchangeableClass", (object)NonLazinatorInterchangeableClass);
            yield return ("NonLazinatorInterchangeableStruct", (object)NonLazinatorInterchangeableStruct);
            yield return ("NonLazinatorStruct", (object)NonLazinatorStruct);
            yield break;
        }
        
        public ILazinator ForEachLazinator(Func<ILazinator, ILazinator> changeFunc, bool exploreOnlyDeserializedChildren)
        {
            if ((!exploreOnlyDeserializedChildren && NonLazinatorInterchangeableClass != null) || (_NonLazinatorInterchangeableClass_Accessed && _NonLazinatorInterchangeableClass != null))
            {
                _NonLazinatorInterchangeableClass = (NonLazinatorInterchangeableClass) CloneOrChange_NonLazinatorInterchangeableClass(_NonLazinatorInterchangeableClass, l => l?.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren), true);
            }
            if ((!exploreOnlyDeserializedChildren && true) || (true))
            {
                var deserialized = NonLazinatorInterchangeableStruct;
                _NonLazinatorInterchangeableStruct = (NonLazinatorInterchangeableStruct) CloneOrChange_NonLazinatorInterchangeableStruct(_NonLazinatorInterchangeableStruct, l => l?.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren), true);
            }
            if ((!exploreOnlyDeserializedChildren && NonLazinatorClass != null) || (_NonLazinatorClass_Accessed && _NonLazinatorClass != null))
            {
                _NonLazinatorClass = NonLazinatorDirectConverter.CloneOrChange_NonLazinatorClass(_NonLazinatorClass, l => l?.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren), true);
            }
            if ((!exploreOnlyDeserializedChildren && true) || (true))
            {
                var deserialized = NonLazinatorStruct;
                _NonLazinatorStruct = NonLazinatorDirectConverter.CloneOrChange_NonLazinatorStruct(_NonLazinatorStruct, l => l?.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren), true);
            }
            return changeFunc(this);
        }
        
        public void FreeInMemoryObjects()
        {
            _NonLazinatorClass = default;
            _NonLazinatorInterchangeableClass = default;
            _NonLazinatorInterchangeableStruct = default;
            _NonLazinatorStruct = default;
            _NonLazinatorClass_Accessed = _NonLazinatorInterchangeableClass_Accessed = _NonLazinatorInterchangeableStruct_Accessed = _NonLazinatorStruct_Accessed = false;
            IsDirty = false;
            DescendantIsDirty = false;
            HasChanged = false;
            DescendantHasChanged = false;
        }
        
        /* Conversion */
        
        public int LazinatorUniqueID => 232;
        
        bool ContainsOpenGenericParameters => false;
        public LazinatorGenericIDType LazinatorGenericID
        {
            get => default;
            set { }
        }
        
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
        
        
        public void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
            _NonLazinatorClass_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            _NonLazinatorInterchangeableClass_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            _NonLazinatorInterchangeableStruct_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            _NonLazinatorStruct_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            _NonLazinatorContainer_EndByteIndex = bytesSoFar;
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
                    if (_NonLazinatorInterchangeableClass_Accessed && _NonLazinatorInterchangeableClass != null)
                    {
                        _NonLazinatorInterchangeableClass = (NonLazinatorInterchangeableClass) CloneOrChange_NonLazinatorInterchangeableClass(_NonLazinatorInterchangeableClass, l => l.RemoveBufferInHierarchy(), true);
                    }
                    if (true)
                    {
                        _NonLazinatorInterchangeableStruct = (NonLazinatorInterchangeableStruct) CloneOrChange_NonLazinatorInterchangeableStruct(_NonLazinatorInterchangeableStruct, l => l.RemoveBufferInHierarchy(), true);
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
            if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode && !_NonLazinatorClass_Accessed)
            {
                var deserialized = NonLazinatorClass;
            }
            var serializedBytesCopy_NonLazinatorClass = LazinatorMemoryStorage;
            var byteIndexCopy_NonLazinatorClass = _NonLazinatorClass_ByteIndex;
            var byteLengthCopy_NonLazinatorClass = _NonLazinatorClass_ByteLength;
            var copy_NonLazinatorClass = _NonLazinatorClass;
            WriteNonLazinatorObject(
            nonLazinatorObject: _NonLazinatorClass, isBelievedDirty: _NonLazinatorClass_Accessed || (includeChildrenMode != OriginalIncludeChildrenMode),
            isAccessed: _NonLazinatorClass_Accessed, writer: ref writer,
            getChildSliceForFieldFn: () => GetChildSlice(serializedBytesCopy_NonLazinatorClass, byteIndexCopy_NonLazinatorClass, byteLengthCopy_NonLazinatorClass, false, false, null),
            verifyCleanness: false,
            binaryWriterAction: (ref BinaryBufferWriter w, bool v) =>
            NonLazinatorDirectConverter.ConvertToBytes_NonLazinatorClass(ref w, copy_NonLazinatorClass, includeChildrenMode, v, updateStoredBuffer));
            if (updateStoredBuffer)
            {
                _NonLazinatorClass_ByteIndex = startOfObjectPosition - startPosition;
            }
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode && !_NonLazinatorInterchangeableClass_Accessed)
            {
                var deserialized = NonLazinatorInterchangeableClass;
            }
            var serializedBytesCopy_NonLazinatorInterchangeableClass = LazinatorMemoryStorage;
            var byteIndexCopy_NonLazinatorInterchangeableClass = _NonLazinatorInterchangeableClass_ByteIndex;
            var byteLengthCopy_NonLazinatorInterchangeableClass = _NonLazinatorInterchangeableClass_ByteLength;
            var copy_NonLazinatorInterchangeableClass = _NonLazinatorInterchangeableClass;
            WriteNonLazinatorObject(
            nonLazinatorObject: _NonLazinatorInterchangeableClass, isBelievedDirty: _NonLazinatorInterchangeableClass_Accessed || (includeChildrenMode != OriginalIncludeChildrenMode),
            isAccessed: _NonLazinatorInterchangeableClass_Accessed, writer: ref writer,
            getChildSliceForFieldFn: () => GetChildSlice(serializedBytesCopy_NonLazinatorInterchangeableClass, byteIndexCopy_NonLazinatorInterchangeableClass, byteLengthCopy_NonLazinatorInterchangeableClass, false, false, null),
            verifyCleanness: false,
            binaryWriterAction: (ref BinaryBufferWriter w, bool v) =>
            ConvertToBytes_NonLazinatorInterchangeableClass(ref w, copy_NonLazinatorInterchangeableClass, includeChildrenMode, v, updateStoredBuffer));
            if (updateStoredBuffer)
            {
                _NonLazinatorInterchangeableClass_ByteIndex = startOfObjectPosition - startPosition;
            }
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode && !_NonLazinatorInterchangeableStruct_Accessed)
            {
                var deserialized = NonLazinatorInterchangeableStruct;
            }
            var serializedBytesCopy_NonLazinatorInterchangeableStruct = LazinatorMemoryStorage;
            var byteIndexCopy_NonLazinatorInterchangeableStruct = _NonLazinatorInterchangeableStruct_ByteIndex;
            var byteLengthCopy_NonLazinatorInterchangeableStruct = _NonLazinatorInterchangeableStruct_ByteLength;
            var copy_NonLazinatorInterchangeableStruct = _NonLazinatorInterchangeableStruct;
            WriteNonLazinatorObject(
            nonLazinatorObject: _NonLazinatorInterchangeableStruct, isBelievedDirty: _NonLazinatorInterchangeableStruct_Accessed || (includeChildrenMode != OriginalIncludeChildrenMode),
            isAccessed: _NonLazinatorInterchangeableStruct_Accessed, writer: ref writer,
            getChildSliceForFieldFn: () => GetChildSlice(serializedBytesCopy_NonLazinatorInterchangeableStruct, byteIndexCopy_NonLazinatorInterchangeableStruct, byteLengthCopy_NonLazinatorInterchangeableStruct, false, false, null),
            verifyCleanness: false,
            binaryWriterAction: (ref BinaryBufferWriter w, bool v) =>
            ConvertToBytes_NonLazinatorInterchangeableStruct(ref w, copy_NonLazinatorInterchangeableStruct, includeChildrenMode, v, updateStoredBuffer));
            if (updateStoredBuffer)
            {
                _NonLazinatorInterchangeableStruct_ByteIndex = startOfObjectPosition - startPosition;
            }
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode && !_NonLazinatorStruct_Accessed)
            {
                var deserialized = NonLazinatorStruct;
            }
            var serializedBytesCopy_NonLazinatorStruct = LazinatorMemoryStorage;
            var byteIndexCopy_NonLazinatorStruct = _NonLazinatorStruct_ByteIndex;
            var byteLengthCopy_NonLazinatorStruct = _NonLazinatorStruct_ByteLength;
            var copy_NonLazinatorStruct = _NonLazinatorStruct;
            WriteNonLazinatorObject(
            nonLazinatorObject: _NonLazinatorStruct, isBelievedDirty: _NonLazinatorStruct_Accessed || (includeChildrenMode != OriginalIncludeChildrenMode),
            isAccessed: _NonLazinatorStruct_Accessed, writer: ref writer,
            getChildSliceForFieldFn: () => GetChildSlice(serializedBytesCopy_NonLazinatorStruct, byteIndexCopy_NonLazinatorStruct, byteLengthCopy_NonLazinatorStruct, false, false, null),
            verifyCleanness: false,
            binaryWriterAction: (ref BinaryBufferWriter w, bool v) =>
            NonLazinatorDirectConverter.ConvertToBytes_NonLazinatorStruct(ref w, copy_NonLazinatorStruct, includeChildrenMode, v, updateStoredBuffer));
            if (updateStoredBuffer)
            {
                _NonLazinatorStruct_ByteIndex = startOfObjectPosition - startPosition;
            }
            if (updateStoredBuffer)
            {
                _NonLazinatorContainer_EndByteIndex = writer.Position - startPosition;
            }
        }
        
        private static NonLazinatorInterchangeableClass ConvertFromBytes_NonLazinatorInterchangeableClass(LazinatorMemory storage)
        {
            if (storage.Length == 0)
            {
                return default(NonLazinatorInterchangeableClass);
            }
            NonLazinatorInterchangeClass interchange = new NonLazinatorInterchangeClass();
            interchange.DeserializeLazinator(storage);
            return interchange.Interchange_NonLazinatorInterchangeableClass(false);
        }
        
        private static void ConvertToBytes_NonLazinatorInterchangeableClass(ref BinaryBufferWriter writer,
        NonLazinatorInterchangeableClass itemToConvert, IncludeChildrenMode includeChildrenMode,
        bool verifyCleanness, bool updateStoredBuffer)
        {
            if (itemToConvert == null)
            {
                return;
            }
            NonLazinatorInterchangeClass interchange = new NonLazinatorInterchangeClass(itemToConvert);
            interchange.SerializeExistingBuffer(ref writer, includeChildrenMode, verifyCleanness, updateStoredBuffer);
        }
        
        
        private static NonLazinatorInterchangeableClass CloneOrChange_NonLazinatorInterchangeableClass(NonLazinatorInterchangeableClass itemToClone, Func<ILazinator, ILazinator> cloneOrChangeFunc, bool avoidCloningIfPossible)
        {
            if (itemToClone == null)
            {
                return default(NonLazinatorInterchangeableClass);
            }
            NonLazinatorInterchangeClass interchange = new NonLazinatorInterchangeClass(itemToClone);
            return interchange.Interchange_NonLazinatorInterchangeableClass(avoidCloningIfPossible ? false : true);
        }
        
        private static NonLazinatorInterchangeableStruct ConvertFromBytes_NonLazinatorInterchangeableStruct(LazinatorMemory storage)
        {
            if (storage.Length == 0)
            {
                return default(NonLazinatorInterchangeableStruct);
            }
            NonLazinatorInterchangeStruct interchange = new NonLazinatorInterchangeStruct();
            interchange.DeserializeLazinator(storage);
            return interchange.Interchange_NonLazinatorInterchangeableStruct(false);
        }
        
        private static void ConvertToBytes_NonLazinatorInterchangeableStruct(ref BinaryBufferWriter writer,
        NonLazinatorInterchangeableStruct itemToConvert, IncludeChildrenMode includeChildrenMode,
        bool verifyCleanness, bool updateStoredBuffer)
        {
            
            NonLazinatorInterchangeStruct interchange = new NonLazinatorInterchangeStruct(itemToConvert);
            interchange.SerializeExistingBuffer(ref writer, includeChildrenMode, verifyCleanness, updateStoredBuffer);
        }
        
        
        private static NonLazinatorInterchangeableStruct CloneOrChange_NonLazinatorInterchangeableStruct(NonLazinatorInterchangeableStruct itemToClone, Func<ILazinator, ILazinator> cloneOrChangeFunc, bool avoidCloningIfPossible)
        {
            
            NonLazinatorInterchangeStruct interchange = new NonLazinatorInterchangeStruct(itemToClone);
            return interchange.Interchange_NonLazinatorInterchangeableStruct(avoidCloningIfPossible ? false : true);
        }
        
    }
}
