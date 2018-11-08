//c1bc6589-414e-7b42-c9fe-b17b83f49b77
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.283
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
            return EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, verifyCleanness, IsDirty, DescendantIsDirty, false, LazinatorMemoryStorage, (EncodeManuallyDelegate) EncodeToNewBuffer, updateStoredBuffer);
        }
        
        LazinatorMemory EncodeToNewBuffer(IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer) => LazinatorUtilities.EncodeToNewBinaryBufferWriter(this, includeChildrenMode, verifyCleanness, updateStoredBuffer);
        
        public ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode = IncludeChildrenMode.IncludeAllChildren, CloneBufferOptions cloneBufferOptions = CloneBufferOptions.LinkedBuffer)
        {
            var clone = new ExampleStructContainingStruct()
            {
                OriginalIncludeChildrenMode = includeChildrenMode
            };
            if (clone.LazinatorObjectVersion != LazinatorObjectVersion)
            {
                clone.LazinatorObjectVersion = LazinatorObjectVersion;
            }
            
            if (cloneBufferOptions == CloneBufferOptions.NoBuffer)
            {
                AssignCloneProperties(ref clone, includeChildrenMode);
            }
            else
            {
                LazinatorMemory bytes = EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, false, IsDirty, DescendantIsDirty, false, LazinatorMemoryStorage, (EncodeManuallyDelegate)EncodeToNewBuffer, cloneBufferOptions == CloneBufferOptions.SharedBuffer);
                clone.DeserializeLazinator(bytes);
                if (cloneBufferOptions == CloneBufferOptions.IndependentBuffers)
                {
                    clone.LazinatorMemoryStorage.DisposeIndependently();
                }
            }
            clone.LazinatorParents = default;
            return clone;
        }
        
        void AssignCloneProperties(ref ExampleStructContainingStruct clone, IncludeChildrenMode includeChildrenMode)
        {
            clone.FreeInMemoryObjects();
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                clone.MyExampleStruct = (System.Collections.Generic.EqualityComparer<ExampleStruct>.Default.Equals(MyExampleStruct, default(ExampleStruct))) ? default(ExampleStruct) : (ExampleStruct) MyExampleStruct.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer);
            }
            
            clone.IsDirty = false;}
            
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
                get => _DescendantHasChanged || (_MyExampleStruct_Accessed && (MyExampleStruct.HasChanged || MyExampleStruct.DescendantHasChanged));
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
                get => _DescendantIsDirty || (_MyExampleStruct_Accessed && (MyExampleStruct.IsDirty || MyExampleStruct.DescendantIsDirty));
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
            
            public void EnsureLazinatorMemoryUpToDate()
            {
                if (LazinatorMemoryStorage == null)
                {
                    throw new NotSupportedException("Cannot use EnsureLazinatorMemoryUpToDate on a struct that has not been deserialized. Clone the struct instead."); 
                }
                if (!IsDirty && !DescendantIsDirty && LazinatorObjectBytes.Length > 0 && OriginalIncludeChildrenMode == IncludeChildrenMode.IncludeAllChildren)
                {
                    return;
                }
                LazinatorMemoryStorage = EncodeOrRecycleToNewBuffer(IncludeChildrenMode.IncludeAllChildren, OriginalIncludeChildrenMode, false, IsDirty, DescendantIsDirty, false, LazinatorMemoryStorage, (EncodeManuallyDelegate)EncodeToNewBuffer, true);
                OriginalIncludeChildrenMode = IncludeChildrenMode.IncludeAllChildren;
            }
            
            public int GetByteLength()
            {
                EnsureLazinatorMemoryUpToDate();
                return LazinatorObjectBytes.Length;
            }
            
            public uint GetBinaryHashCode32()
            {
                if (LazinatorMemoryStorage == null)
                {
                    var result = SerializeLazinator(IncludeChildrenMode.IncludeAllChildren, false, false);
                    return FarmhashByteSpans.Hash32(result.Span);
                }
                else
                {
                    EnsureLazinatorMemoryUpToDate();
                    return FarmhashByteSpans.Hash32(LazinatorObjectBytes.Span);
                }
            }
            
            public ulong GetBinaryHashCode64()
            {
                if (LazinatorMemoryStorage == null)
                {
                    var result = SerializeLazinator(IncludeChildrenMode.IncludeAllChildren, false, false);
                    return FarmhashByteSpans.Hash64(result.Span);
                }
                else
                {
                    EnsureLazinatorMemoryUpToDate();
                    return FarmhashByteSpans.Hash64(LazinatorObjectBytes.Span);
                }
            }
            
            public Guid GetBinaryHashCode128()
            {
                if (LazinatorMemoryStorage == null)
                {
                    var result = SerializeLazinator(IncludeChildrenMode.IncludeAllChildren, false, false);
                    return FarmhashByteSpans.Hash128(result.Span);
                }
                else
                {
                    EnsureLazinatorMemoryUpToDate();
                    return FarmhashByteSpans.Hash128(LazinatorObjectBytes.Span);
                }
            }
            
            /* Property definitions */
            
            int _MyExampleStruct_ByteIndex;
            private int _ExampleStructContainingStruct_EndByteIndex;
            int _MyExampleStruct_ByteLength => _ExampleStructContainingStruct_EndByteIndex - _MyExampleStruct_ByteIndex;
            
            
            ExampleStruct _MyExampleStruct;
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            public ExampleStruct MyExampleStruct
            {
                get
                {
                    if (!_MyExampleStruct_Accessed)
                    {
                        if (LazinatorObjectBytes.Length == 0)
                        {
                            _MyExampleStruct = default(ExampleStruct);
                        }
                        else
                        {
                            LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _MyExampleStruct_ByteIndex, _MyExampleStruct_ByteLength, false, false, null);
                            _MyExampleStruct = new ExampleStruct();
                            _MyExampleStruct.DeserializeLazinator(childData);
                        }
                        _MyExampleStruct_Accessed = true;
                    } 
                    return _MyExampleStruct;
                }
                set
                {
                    
                    IsDirty = true;
                    DescendantIsDirty = true;
                    _MyExampleStruct = value;
                    _MyExampleStruct_Accessed = true;
                }
            }
            bool _MyExampleStruct_Accessed;
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            public ExampleStruct MyExampleStruct_Copy
            {
                get
                {
                    if (!_MyExampleStruct_Accessed)
                    {
                        if (LazinatorObjectBytes.Length == 0)
                        {
                            return default(ExampleStruct);
                        }
                        else
                        {
                            LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _MyExampleStruct_ByteIndex, _MyExampleStruct_ByteLength, false, false, null);
                            var toReturn = new ExampleStruct();
                            toReturn.DeserializeLazinator(childData);
                            toReturn.IsDirty = false;
                            return toReturn;
                        }
                    }
                    var cleanCopy = _MyExampleStruct;
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
                if (enumerateNulls && (!exploreOnlyDeserializedChildren || _MyExampleStruct_Accessed) && (System.Collections.Generic.EqualityComparer<ExampleStruct>.Default.Equals(MyExampleStruct, default(ExampleStruct))))
                {
                    yield return ("MyExampleStruct", default);
                }
                else if ((!exploreOnlyDeserializedChildren && !System.Collections.Generic.EqualityComparer<ExampleStruct>.Default.Equals(MyExampleStruct, default(ExampleStruct))) || (_MyExampleStruct_Accessed && !System.Collections.Generic.EqualityComparer<ExampleStruct>.Default.Equals(_MyExampleStruct, default(ExampleStruct))))
                {
                    bool isMatch = matchCriterion == null || matchCriterion(MyExampleStruct);
                    bool shouldExplore = exploreCriterion == null || exploreCriterion(MyExampleStruct);
                    if (isMatch)
                    {
                        yield return ("MyExampleStruct", MyExampleStruct);
                    }
                    if ((!stopExploringBelowMatch || !isMatch) && shouldExplore)
                    {
                        foreach (var toYield in MyExampleStruct.EnumerateLazinatorDescendants(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
                        {
                            yield return ("MyExampleStruct" + "." + toYield.propertyName, toYield.descendant);
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
                if ((!exploreOnlyDeserializedChildren && !System.Collections.Generic.EqualityComparer<ExampleStruct>.Default.Equals(MyExampleStruct, default(ExampleStruct))) || (_MyExampleStruct_Accessed && !System.Collections.Generic.EqualityComparer<ExampleStruct>.Default.Equals(_MyExampleStruct, default(ExampleStruct))))
                {
                    MyExampleStruct = (ExampleStruct) MyExampleStruct.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren);
                }
                return changeFunc(this);
            }
            
            public void FreeInMemoryObjects()
            {
                _MyExampleStruct = default;
                _MyExampleStruct_Accessed = false;
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
                _MyExampleStruct_ByteIndex = bytesSoFar;
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
                        if (_MyExampleStruct_Accessed && !System.Collections.Generic.EqualityComparer<ExampleStruct>.Default.Equals(_MyExampleStruct, default(ExampleStruct)))
                        {
                            _MyExampleStruct.UpdateStoredBuffer(ref writer, startPosition + _MyExampleStruct_ByteIndex + sizeof(int), _MyExampleStruct_ByteLength - sizeof(int), IncludeChildrenMode.IncludeAllChildren, true);
                        }
                    }
                    
                    _MyExampleStruct_Accessed = false;
                }
                else
                {
                    throw new Exception("Cannot update stored buffer when serializing only some children.");
                }
                
                var newBuffer = writer.Slice(startPosition, length);
                LazinatorMemoryStorage = ReplaceBuffer(LazinatorMemoryStorage, newBuffer, LazinatorParents, startPosition == 0, IsStruct);
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
                    if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode && !_MyExampleStruct_Accessed)
                    {
                        var deserialized = MyExampleStruct;
                    }
                    var serializedBytesCopy = LazinatorMemoryStorage;
                    var byteIndexCopy = _MyExampleStruct_ByteIndex;
                    var byteLengthCopy = _MyExampleStruct_ByteLength;
                    WriteChild(ref writer, ref _MyExampleStruct, includeChildrenMode, _MyExampleStruct_Accessed, () => GetChildSlice(serializedBytesCopy, byteIndexCopy, byteLengthCopy, false, false, null), verifyCleanness, updateStoredBuffer, false, false, null);
                }
                if (updateStoredBuffer)
                {
                    _MyExampleStruct_ByteIndex = startOfObjectPosition - startPosition;
                }
                if (updateStoredBuffer)
                {
                    _ExampleStructContainingStruct_EndByteIndex = writer.Position - startPosition;
                }
            }
            
        }
    }
