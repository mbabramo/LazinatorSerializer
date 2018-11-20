//a898546f-53e1-bff5-12da-f5cfeaf897aa
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.318
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
    public partial struct ExampleStructContainingStruct : ILazinator
    {
        public bool IsStruct => true;
        
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
        
        public LazinatorMemory SerializeLazinator(IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            return EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, verifyCleanness, IsDirty, DescendantIsDirty, false, LazinatorMemoryStorage, updateStoredBuffer, (EncodeManuallyDelegate) EncodeToNewBuffer);
        }
        
        LazinatorMemory EncodeToNewBuffer(IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer) => LazinatorUtilities.EncodeToNewBinaryBufferWriter(this, includeChildrenMode, verifyCleanness, updateStoredBuffer);
        
        public ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode = IncludeChildrenMode.IncludeAllChildren, CloneBufferOptions cloneBufferOptions = CloneBufferOptions.IndependentBuffers)
        {
            var clone = new ExampleStructContainingStruct()
            {
                OriginalIncludeChildrenMode = includeChildrenMode
            };
            clone = CompleteClone(this, clone, includeChildrenMode, cloneBufferOptions);
            return clone;
        }
        
        public ILazinator AssignCloneProperties(ILazinator clone, IncludeChildrenMode includeChildrenMode)
        {
            clone.FreeInMemoryObjects();
            ExampleStructContainingStruct typedClone = (ExampleStructContainingStruct) clone;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                typedClone.MyExampleStructContainingClasses = (System.Collections.Generic.EqualityComparer<ExampleStructContainingClasses>.Default.Equals(MyExampleStructContainingClasses, default(ExampleStructContainingClasses))) ? default(ExampleStructContainingClasses) : (ExampleStructContainingClasses) MyExampleStructContainingClasses.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer);
            }
            
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
            get => _DescendantHasChanged || (_MyExampleStructContainingClasses_Accessed && (MyExampleStructContainingClasses.HasChanged || MyExampleStructContainingClasses.DescendantHasChanged));
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
            get => _DescendantIsDirty || (_MyExampleStructContainingClasses_Accessed && (MyExampleStructContainingClasses.IsDirty || MyExampleStructContainingClasses.DescendantIsDirty));
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
        ReadOnlyMemory<byte> LazinatorObjectBytes => LazinatorMemoryStorage?.Memory ?? LazinatorUtilities.EmptyReadOnlyMemory;
        
        public void UpdateStoredBuffer()
        {
            if (LazinatorMemoryStorage == null)
            {
                throw new NotSupportedException("Cannot use UpdateStoredBuffer on a struct that has not been deserialized. Clone the struct instead."); 
            }
            if (!IsDirty && !DescendantIsDirty && LazinatorObjectBytes.Length > 0 && OriginalIncludeChildrenMode == IncludeChildrenMode.IncludeAllChildren)
            {
                return;
            }
            LazinatorMemoryStorage = EncodeOrRecycleToNewBuffer(IncludeChildrenMode.IncludeAllChildren, OriginalIncludeChildrenMode, false, IsDirty, DescendantIsDirty, false, LazinatorMemoryStorage, true, (EncodeManuallyDelegate) EncodeToNewBuffer);
            OriginalIncludeChildrenMode = IncludeChildrenMode.IncludeAllChildren;
        }
        
        public int GetByteLength()
        {
            UpdateStoredBuffer();
            return LazinatorObjectBytes.Length;
        }
        
        public bool NonBinaryHash32 => false;
        
        /* Property definitions */
        
        int _MyExampleStructContainingClasses_ByteIndex;
        private int _ExampleStructContainingStruct_EndByteIndex;
        int _MyExampleStructContainingClasses_ByteLength => _ExampleStructContainingStruct_EndByteIndex - _MyExampleStructContainingClasses_ByteIndex;
        
        
        ExampleStructContainingClasses _MyExampleStructContainingClasses;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ExampleStructContainingClasses MyExampleStructContainingClasses
        {
            get
            {
                if (!_MyExampleStructContainingClasses_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _MyExampleStructContainingClasses = default(ExampleStructContainingClasses);
                    }
                    else
                    {
                        LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _MyExampleStructContainingClasses_ByteIndex, _MyExampleStructContainingClasses_ByteLength, false, false, null);
                        _MyExampleStructContainingClasses = new ExampleStructContainingClasses();
                        _MyExampleStructContainingClasses.DeserializeLazinator(childData);
                    }
                    _MyExampleStructContainingClasses_Accessed = true;
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
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ExampleStructContainingClasses MyExampleStructContainingClasses_Copy
        {
            get
            {
                if (!_MyExampleStructContainingClasses_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        return default(ExampleStructContainingClasses);
                    }
                    else
                    {
                        LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _MyExampleStructContainingClasses_ByteIndex, _MyExampleStructContainingClasses_ByteLength, false, false, null);
                        var toReturn = new ExampleStructContainingClasses();
                        toReturn.DeserializeLazinator(childData);
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
            if (enumerateNulls && (!exploreOnlyDeserializedChildren || _MyExampleStructContainingClasses_Accessed) && (System.Collections.Generic.EqualityComparer<ExampleStructContainingClasses>.Default.Equals(MyExampleStructContainingClasses, default(ExampleStructContainingClasses))))
            {
                yield return ("MyExampleStructContainingClasses", default);
            }
            else if ((!exploreOnlyDeserializedChildren && !System.Collections.Generic.EqualityComparer<ExampleStructContainingClasses>.Default.Equals(MyExampleStructContainingClasses, default(ExampleStructContainingClasses))) || (_MyExampleStructContainingClasses_Accessed && !System.Collections.Generic.EqualityComparer<ExampleStructContainingClasses>.Default.Equals(_MyExampleStructContainingClasses, default(ExampleStructContainingClasses))))
            {
                bool isMatch = matchCriterion == null || matchCriterion(MyExampleStructContainingClasses);
                bool shouldExplore = exploreCriterion == null || exploreCriterion(MyExampleStructContainingClasses);
                if (isMatch)
                {
                    yield return ("MyExampleStructContainingClasses", MyExampleStructContainingClasses);
                }
                if ((!stopExploringBelowMatch || !isMatch) && shouldExplore)
                {
                    foreach (var toYield in MyExampleStructContainingClasses.EnumerateLazinatorDescendants(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
                    {
                        yield return ("MyExampleStructContainingClasses" + "." + toYield.propertyName, toYield.descendant);
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
            if ((!exploreOnlyDeserializedChildren && !System.Collections.Generic.EqualityComparer<ExampleStructContainingClasses>.Default.Equals(MyExampleStructContainingClasses, default(ExampleStructContainingClasses))) || (_MyExampleStructContainingClasses_Accessed && !System.Collections.Generic.EqualityComparer<ExampleStructContainingClasses>.Default.Equals(_MyExampleStructContainingClasses, default(ExampleStructContainingClasses))))
            {
                _MyExampleStructContainingClasses = (ExampleStructContainingClasses) _MyExampleStructContainingClasses.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren);
            }
            return changeFunc(this);
        }
        
        public void FreeInMemoryObjects()
        {
            _MyExampleStructContainingClasses = default;
            _MyExampleStructContainingClasses_Accessed = false;
            IsDirty = false;
            DescendantIsDirty = false;
            HasChanged = false;
            DescendantHasChanged = false;
        }
        
        /* Conversion */
        
        public int LazinatorUniqueID => 218;
        
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
            _MyExampleStructContainingClasses_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            _ExampleStructContainingStruct_EndByteIndex = bytesSoFar;
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
                    if (_MyExampleStructContainingClasses_Accessed && !System.Collections.Generic.EqualityComparer<ExampleStructContainingClasses>.Default.Equals(_MyExampleStructContainingClasses, default(ExampleStructContainingClasses)))
                    {
                        _MyExampleStructContainingClasses.UpdateStoredBuffer(ref writer, startPosition + _MyExampleStructContainingClasses_ByteIndex + sizeof(int), _MyExampleStructContainingClasses_ByteLength - sizeof(int), IncludeChildrenMode.IncludeAllChildren, true);
                    }
                }
                
                _MyExampleStructContainingClasses_Accessed = false;
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
                if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode && !_MyExampleStructContainingClasses_Accessed)
                {
                    var deserialized = MyExampleStructContainingClasses;
                }
                var serializedBytesCopy = LazinatorMemoryStorage;
                var byteIndexCopy = _MyExampleStructContainingClasses_ByteIndex;
                var byteLengthCopy = _MyExampleStructContainingClasses_ByteLength;
                WriteChild(ref writer, ref _MyExampleStructContainingClasses, includeChildrenMode, _MyExampleStructContainingClasses_Accessed, () => GetChildSlice(serializedBytesCopy, byteIndexCopy, byteLengthCopy, false, false, null), verifyCleanness, updateStoredBuffer, false, false, null);
            }
            if (updateStoredBuffer)
            {
                _MyExampleStructContainingClasses_ByteIndex = startOfObjectPosition - startPosition;
            }
            if (updateStoredBuffer)
            {
                _ExampleStructContainingStruct_EndByteIndex = writer.Position - startPosition;
            }
        }
        
    }
}
