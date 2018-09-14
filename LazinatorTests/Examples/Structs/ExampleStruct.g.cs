//f023f3f0-65ba-1b1f-375b-dad329e0e9d5
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.220
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
    public partial struct ExampleStruct : ILazinator
    {
        public bool IsStruct => true;
        
        /* Serialization, deserialization, and object relationships */
        
        public LazinatorParentsCollection LazinatorParents { get; set; }
        
        IncludeChildrenMode OriginalIncludeChildrenMode;
        
        public int Deserialize()
        {
            FreeInMemoryObjects();
            int bytesSoFar = 0;
            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
            if (span.Length == 0)
            {
                return 0;
            }
            
            if (LazinatorParents.Any())
            {
                throw new LazinatorDeserializationException("A Lazinator struct may include a Lazinator class or interface as a property only when the Lazinator struct has no parent class.");
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
        
        public ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode = IncludeChildrenMode.IncludeAllChildren, bool updateStoredBuffer = false, bool disposeCloneIndependently = false)
        {
            LazinatorMemory bytes = EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, false, IsDirty, DescendantIsDirty, false, LazinatorMemoryStorage, (EncodeManuallyDelegate)EncodeToNewBuffer, updateStoredBuffer);
            var clone = new ExampleStruct()
            {
                OriginalIncludeChildrenMode = includeChildrenMode
            };
            clone.DeserializeLazinator(bytes);
            if (disposeCloneIndependently)
            {
                clone.LazinatorMemoryStorage.DisposeIndependently();
            }
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
            get => _DescendantHasChanged || (_MyChild1_Accessed && _MyChild1 != null && (MyChild1.HasChanged || MyChild1.DescendantHasChanged)) || (_MyChild2_Accessed && _MyChild2 != null && (MyChild2.HasChanged || MyChild2.DescendantHasChanged));
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
            get => _DescendantIsDirty || (_MyChild1_Accessed && _MyChild1 != null && (MyChild1.IsDirty || MyChild1.DescendantIsDirty)) || (_MyChild2_Accessed && _MyChild2 != null && (MyChild2.IsDirty || MyChild2.DescendantIsDirty));
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
        }
        
        LazinatorMemory _LazinatorMemoryStorage;
        public LazinatorMemory LazinatorMemoryStorage
        {
            get => _LazinatorMemoryStorage;
            set
            {
                _LazinatorMemoryStorage = value;
                int length = Deserialize();
                if (length != _LazinatorMemoryStorage.Length)
                {
                    _LazinatorMemoryStorage = _LazinatorMemoryStorage.Slice(0, length);
                }
            }
        }
        ReadOnlyMemory<byte> LazinatorObjectBytes => LazinatorMemoryStorage?.Memory ?? LazinatorUtilities.EmptyReadOnlyMemory;
        
        public void EnsureLazinatorMemoryUpToDate()
        {
            if (_LazinatorMemoryStorage == null)
            {
                throw new NotSupportedException("Cannot use EnsureLazinatorMemoryUpToDate on a struct that has not been deserialized. Clone the struct instead."); 
            }
            if (!IsDirty && !DescendantIsDirty && LazinatorObjectBytes.Length > 0)
            {
                return;
            }
            EncodeOrRecycleToNewBuffer(IncludeChildrenMode.IncludeAllChildren, OriginalIncludeChildrenMode, false, IsDirty, DescendantIsDirty, false, LazinatorMemoryStorage, (EncodeManuallyDelegate)EncodeToNewBuffer, true);
        }
        
        public int GetByteLength()
        {
            EnsureLazinatorMemoryUpToDate();
            return LazinatorObjectBytes.Length;
        }
        
        public uint GetBinaryHashCode32()
        {
            if (_LazinatorMemoryStorage == null)
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
            if (_LazinatorMemoryStorage == null)
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
            if (_LazinatorMemoryStorage == null)
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
        
        int _MyChild1_ByteIndex;
        int _MyChild2_ByteIndex;
        int _MyLazinatorList_ByteIndex;
        int _MyListValues_ByteIndex;
        int _MyTuple_ByteIndex;
        int _MyChild1_ByteLength => _MyChild2_ByteIndex - _MyChild1_ByteIndex;
        int _MyChild2_ByteLength => _MyLazinatorList_ByteIndex - _MyChild2_ByteIndex;
        int _MyLazinatorList_ByteLength => _MyListValues_ByteIndex - _MyLazinatorList_ByteIndex;
        int _MyListValues_ByteLength => _MyTuple_ByteIndex - _MyListValues_ByteIndex;
        private int _ExampleStruct_EndByteIndex;
        int _MyTuple_ByteLength => _ExampleStruct_EndByteIndex - _MyTuple_ByteIndex;
        
        bool _MyBool;
        public bool MyBool
        {
            get
            {
                return _MyBool;
            }
            set
            {
                IsDirty = true;
                _MyBool = value;
            }
        }
        char _MyChar;
        public char MyChar
        {
            get
            {
                return _MyChar;
            }
            set
            {
                IsDirty = true;
                _MyChar = value;
            }
        }
        ExampleChild _MyChild1;
        public ExampleChild MyChild1
        {
            get
            {
                if (!_MyChild1_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _MyChild1 = default(ExampleChild);
                    }
                    else
                    {
                        LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _MyChild1_ByteIndex, _MyChild1_ByteLength, false, false, null);
                        
                        _MyChild1 = DeserializationFactory.Instance.CreateBaseOrDerivedType(213, () => new ExampleChild(), childData); 
                    }
                    _MyChild1_Accessed = true;
                } 
                return _MyChild1;
            }
            set
            {
                IsDirty = true;
                DescendantIsDirty = true;
                _MyChild1 = value;
                _MyChild1_Accessed = true;
            }
        }
        bool _MyChild1_Accessed;
        ExampleChild _MyChild2;
        public ExampleChild MyChild2
        {
            get
            {
                if (!_MyChild2_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _MyChild2 = default(ExampleChild);
                    }
                    else
                    {
                        LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _MyChild2_ByteIndex, _MyChild2_ByteLength, false, false, null);
                        
                        _MyChild2 = DeserializationFactory.Instance.CreateBaseOrDerivedType(213, () => new ExampleChild(), childData); 
                    }
                    _MyChild2_Accessed = true;
                } 
                return _MyChild2;
            }
            set
            {
                IsDirty = true;
                DescendantIsDirty = true;
                _MyChild2 = value;
                _MyChild2_Accessed = true;
            }
        }
        bool _MyChild2_Accessed;
        List<Example> _MyLazinatorList;
        public List<Example> MyLazinatorList
        {
            get
            {
                if (!_MyLazinatorList_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _MyLazinatorList = default(List<Example>);
                        _MyLazinatorList_Dirty = true; 
                    }
                    else
                    {
                        LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _MyLazinatorList_ByteIndex, _MyLazinatorList_ByteLength, false, false, null);
                        _MyLazinatorList = ConvertFromBytes_List_GExample_g(childData);
                    }
                    _MyLazinatorList_Accessed = true;
                } 
                return _MyLazinatorList;
            }
            set
            {
                IsDirty = true;
                DescendantIsDirty = true;
                _MyLazinatorList = value;
                _MyLazinatorList_Dirty = true;
                _MyLazinatorList_Accessed = true;
            }
        }
        bool _MyLazinatorList_Accessed;
        
        private bool _MyLazinatorList_Dirty;
        public bool MyLazinatorList_Dirty
        {
            get => _MyLazinatorList_Dirty;
            set
            {
                if (_MyLazinatorList_Dirty != value)
                {
                    _MyLazinatorList_Dirty = value;
                    if (value && !IsDirty)
                    {
                        IsDirty = true;
                    }
                }
            }
        }
        List<int> _MyListValues;
        public List<int> MyListValues
        {
            get
            {
                if (!_MyListValues_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _MyListValues = default(List<int>);
                    }
                    else
                    {
                        LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _MyListValues_ByteIndex, _MyListValues_ByteLength, false, false, null);
                        _MyListValues = ConvertFromBytes_List_Gint_g(childData);
                    }
                    _MyListValues_Accessed = true;
                }
                IsDirty = true; 
                return _MyListValues;
            }
            set
            {
                IsDirty = true;
                DescendantIsDirty = true;
                _MyListValues = value;
                _MyListValues_Accessed = true;
            }
        }
        bool _MyListValues_Accessed;
        (NonLazinatorClass myitem1, int? myitem2) _MyTuple;
        public (NonLazinatorClass myitem1, int? myitem2) MyTuple
        {
            get
            {
                if (!_MyTuple_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _MyTuple = default((NonLazinatorClass myitem1, int? myitem2));
                    }
                    else
                    {
                        LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _MyTuple_ByteIndex, _MyTuple_ByteLength, false, false, null);
                        _MyTuple = ConvertFromBytes__PNonLazinatorClass_C32myitem1_c_C32int_C63_C32myitem2_p(childData);
                    }
                    _MyTuple_Accessed = true;
                }
                IsDirty = true; 
                return _MyTuple;
            }
            set
            {
                IsDirty = true;
                DescendantIsDirty = true;
                _MyTuple = value;
                _MyTuple_Accessed = true;
            }
        }
        bool _MyTuple_Accessed;
        
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
            if (enumerateNulls && (!exploreOnlyDeserializedChildren || _MyChild1_Accessed) && (MyChild1 == null))
            {
                yield return ("MyChild1", default);
            }
            else if ((!exploreOnlyDeserializedChildren && MyChild1 != null) || (_MyChild1_Accessed && _MyChild1 != null))
            {
                bool isMatch = matchCriterion == null || matchCriterion(MyChild1);
                bool shouldExplore = exploreCriterion == null || exploreCriterion(MyChild1);
                if (isMatch)
                {
                    yield return ("MyChild1", MyChild1);
                }
                if ((!stopExploringBelowMatch || !isMatch) && shouldExplore)
                {
                    foreach (var toYield in MyChild1.EnumerateLazinatorDescendants(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
                    {
                        yield return ("MyChild1" + "." + toYield.propertyName, toYield.descendant);
                    }
                }
            }
            if (enumerateNulls && (!exploreOnlyDeserializedChildren || _MyChild2_Accessed) && (MyChild2 == null))
            {
                yield return ("MyChild2", default);
            }
            else if ((!exploreOnlyDeserializedChildren && MyChild2 != null) || (_MyChild2_Accessed && _MyChild2 != null))
            {
                bool isMatch = matchCriterion == null || matchCriterion(MyChild2);
                bool shouldExplore = exploreCriterion == null || exploreCriterion(MyChild2);
                if (isMatch)
                {
                    yield return ("MyChild2", MyChild2);
                }
                if ((!stopExploringBelowMatch || !isMatch) && shouldExplore)
                {
                    foreach (var toYield in MyChild2.EnumerateLazinatorDescendants(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
                    {
                        yield return ("MyChild2" + "." + toYield.propertyName, toYield.descendant);
                    }
                }
            }
            yield break;
        }
        
        
        public IEnumerable<(string propertyName, object descendant)> EnumerateNonLazinatorProperties()
        {
            yield return ("MyBool", (object)MyBool);
            yield return ("MyChar", (object)MyChar);
            yield return ("MyLazinatorList", (object)MyLazinatorList);
            yield return ("MyListValues", (object)MyListValues);
            yield return ("MyTuple", (object)MyTuple);
            yield break;
        }
        
        public void FreeInMemoryObjects()
        {
            _MyChild1 = default;
            _MyChild2 = default;
            _MyLazinatorList = default;
            _MyListValues = default;
            _MyTuple = default;
            _MyChild1_Accessed = _MyChild2_Accessed = _MyLazinatorList_Accessed = _MyListValues_Accessed = _MyTuple_Accessed = false;
            IsDirty = false;
            DescendantIsDirty = false;
            HasChanged = false;
            DescendantHasChanged = false;
        }
        
        /* Conversion */
        
        public int LazinatorUniqueID => 216;
        
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
            _MyBool = span.ToBoolean(ref bytesSoFar);
            _MyChar = span.ToChar(ref bytesSoFar);
            _MyChild1_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            _MyChild2_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            _MyLazinatorList_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            _MyListValues_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            _MyTuple_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            _ExampleStruct_EndByteIndex = bytesSoFar;
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
                
                var newBuffer = writer.Slice(startPosition);
                if (_LazinatorMemoryStorage != null)
                {
                    _LazinatorMemoryStorage.DisposeWithThis(newBuffer);
                }
                _LazinatorMemoryStorage = newBuffer;
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
            WriteUncompressedPrimitives.WriteBool(ref writer, _MyBool);
            EncodeCharAndString.WriteCharInTwoBytes(ref writer, _MyChar);
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)  
            {
                if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren && !_MyChild1_Accessed)
                {
                    var deserialized = MyChild1;
                }
                var serializedBytesCopy = LazinatorMemoryStorage;
                var byteIndexCopy = _MyChild1_ByteIndex;
                var byteLengthCopy = _MyChild1_ByteLength;
                WriteChild(ref writer, _MyChild1, includeChildrenMode, _MyChild1_Accessed, () => GetChildSlice(serializedBytesCopy, byteIndexCopy, byteLengthCopy, false, false, null), verifyCleanness, updateStoredBuffer, false, false, null);
            }
            if (updateStoredBuffer)
            {
                _MyChild1_ByteIndex = startOfObjectPosition - startPosition;
            }
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)  
            {
                if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren && !_MyChild2_Accessed)
                {
                    var deserialized = MyChild2;
                }
                var serializedBytesCopy = LazinatorMemoryStorage;
                var byteIndexCopy = _MyChild2_ByteIndex;
                var byteLengthCopy = _MyChild2_ByteLength;
                WriteChild(ref writer, _MyChild2, includeChildrenMode, _MyChild2_Accessed, () => GetChildSlice(serializedBytesCopy, byteIndexCopy, byteLengthCopy, false, false, null), verifyCleanness, updateStoredBuffer, false, false, null);
            }
            if (updateStoredBuffer)
            {
                _MyChild2_ByteIndex = startOfObjectPosition - startPosition;
            }
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren && !_MyLazinatorList_Accessed)
            {
                var deserialized = MyLazinatorList;
            }
            var serializedBytesCopy_MyLazinatorList = LazinatorMemoryStorage;
            var byteIndexCopy_MyLazinatorList = _MyLazinatorList_ByteIndex;
            var byteLengthCopy_MyLazinatorList = _MyLazinatorList_ByteLength;
            var copy_MyLazinatorList = _MyLazinatorList;
            WriteNonLazinatorObject(
            nonLazinatorObject: _MyLazinatorList, isBelievedDirty: MyLazinatorList_Dirty,
            isAccessed: _MyLazinatorList_Accessed, writer: ref writer,
            getChildSliceForFieldFn: () => GetChildSlice(serializedBytesCopy_MyLazinatorList, byteIndexCopy_MyLazinatorList, byteLengthCopy_MyLazinatorList, false, false, null),
            verifyCleanness: verifyCleanness,
            binaryWriterAction: (ref BinaryBufferWriter w, bool v) =>
            ConvertToBytes_List_GExample_g(ref w, copy_MyLazinatorList, includeChildrenMode, v, updateStoredBuffer));
            if (updateStoredBuffer)
            {
                _MyLazinatorList_ByteIndex = startOfObjectPosition - startPosition;
            }
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren && !_MyListValues_Accessed)
            {
                var deserialized = MyListValues;
            }
            var serializedBytesCopy_MyListValues = LazinatorMemoryStorage;
            var byteIndexCopy_MyListValues = _MyListValues_ByteIndex;
            var byteLengthCopy_MyListValues = _MyListValues_ByteLength;
            var copy_MyListValues = _MyListValues;
            WriteNonLazinatorObject(
            nonLazinatorObject: _MyListValues, isBelievedDirty: _MyListValues_Accessed,
            isAccessed: _MyListValues_Accessed, writer: ref writer,
            getChildSliceForFieldFn: () => GetChildSlice(serializedBytesCopy_MyListValues, byteIndexCopy_MyListValues, byteLengthCopy_MyListValues, false, false, null),
            verifyCleanness: false,
            binaryWriterAction: (ref BinaryBufferWriter w, bool v) =>
            ConvertToBytes_List_Gint_g(ref w, copy_MyListValues, includeChildrenMode, v, updateStoredBuffer));
            if (updateStoredBuffer)
            {
                _MyListValues_ByteIndex = startOfObjectPosition - startPosition;
            }
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren && !_MyTuple_Accessed)
            {
                var deserialized = MyTuple;
            }
            var serializedBytesCopy_MyTuple = LazinatorMemoryStorage;
            var byteIndexCopy_MyTuple = _MyTuple_ByteIndex;
            var byteLengthCopy_MyTuple = _MyTuple_ByteLength;
            var copy_MyTuple = _MyTuple;
            WriteNonLazinatorObject(
            nonLazinatorObject: _MyTuple, isBelievedDirty: _MyTuple_Accessed,
            isAccessed: _MyTuple_Accessed, writer: ref writer,
            getChildSliceForFieldFn: () => GetChildSlice(serializedBytesCopy_MyTuple, byteIndexCopy_MyTuple, byteLengthCopy_MyTuple, false, false, null),
            verifyCleanness: false,
            binaryWriterAction: (ref BinaryBufferWriter w, bool v) =>
            ConvertToBytes__PNonLazinatorClass_C32myitem1_c_C32int_C63_C32myitem2_p(ref w, copy_MyTuple, includeChildrenMode, v, updateStoredBuffer));
            if (updateStoredBuffer)
            {
                _MyTuple_ByteIndex = startOfObjectPosition - startPosition;
            }
            if (updateStoredBuffer)
            {
                _ExampleStruct_EndByteIndex = writer.Position - startPosition;
            }
        }
        
        /* Conversion of supported collections and tuples */
        
        private static List<Example> ConvertFromBytes_List_GExample_g(LazinatorMemory storage)
        {
            if (storage.Length == 0)
            {
                return default(List<Example>);
            }
            ReadOnlySpan<byte> span = storage.Span;
            
            int bytesSoFar = 0;
            int collectionLength = span.ToDecompressedInt(ref bytesSoFar);
            
            List<Example> collection = new List<Example>(collectionLength);
            for (int i = 0; i < collectionLength; i++)
            {
                int lengthCollectionMember = span.ToInt32(ref bytesSoFar);
                if (lengthCollectionMember == 0)
                {
                    collection.Add(default(Example));
                }
                else
                {
                    LazinatorMemory childData = storage.Slice(bytesSoFar, lengthCollectionMember);
                    var item = DeserializationFactory.Instance.CreateBasedOnType<Example>(childData);
                    collection.Add(item);
                }
                bytesSoFar += lengthCollectionMember;
            }
            
            return collection;
        }
        
        private static void ConvertToBytes_List_GExample_g(ref BinaryBufferWriter writer, List<Example> itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            if (itemToConvert == default(List<Example>))
            {
                return;
            }
            CompressedIntegralTypes.WriteCompressedInt(ref writer, itemToConvert.Count);
            int itemToConvertCount = itemToConvert.Count;
            for (int itemIndex = 0; itemIndex < itemToConvertCount; itemIndex++)
            {
                if (itemToConvert[itemIndex] == default(Example))
                {
                    writer.Write((uint)0);
                }
                else 
                {
                    
                    void action(ref BinaryBufferWriter w) => itemToConvert[itemIndex].SerializeExistingBuffer(ref w, includeChildrenMode, verifyCleanness, updateStoredBuffer);
                    WriteToBinaryWithIntLengthPrefix(ref writer, action);
                }
                
            }
        }
        
        private static List<int> ConvertFromBytes_List_Gint_g(LazinatorMemory storage)
        {
            if (storage.Length == 0)
            {
                return default(List<int>);
            }
            ReadOnlySpan<byte> span = storage.Span;
            
            int bytesSoFar = 0;
            int collectionLength = span.ToDecompressedInt(ref bytesSoFar);
            
            List<int> collection = new List<int>(collectionLength);
            for (int i = 0; i < collectionLength; i++)
            {
                int item = span.ToDecompressedInt(ref bytesSoFar);
                collection.Add(item);
            }
            
            return collection;
        }
        
        private static void ConvertToBytes_List_Gint_g(ref BinaryBufferWriter writer, List<int> itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            if (itemToConvert == default(List<int>))
            {
                return;
            }
            CompressedIntegralTypes.WriteCompressedInt(ref writer, itemToConvert.Count);
            int itemToConvertCount = itemToConvert.Count;
            for (int itemIndex = 0; itemIndex < itemToConvertCount; itemIndex++)
            {
                CompressedIntegralTypes.WriteCompressedInt(ref writer, itemToConvert[itemIndex]);
            }
        }
        
        private static (NonLazinatorClass myitem1, int? myitem2) ConvertFromBytes__PNonLazinatorClass_C32myitem1_c_C32int_C63_C32myitem2_p(LazinatorMemory storage)
        {
            if (storage.Length == 0)
            {
                return default;
            }
            ReadOnlySpan<byte> span = storage.ReadOnlySpan;
            
            int bytesSoFar = 0;
            
            NonLazinatorClass item1 = default;
            int lengthCollectionMember_item1 = span.ToInt32(ref bytesSoFar);
            if (lengthCollectionMember_item1 != 0)
            {
                LazinatorMemory childData = storage.Slice(bytesSoFar, lengthCollectionMember_item1);
                item1 = NonLazinatorDirectConverter.ConvertFromBytes_NonLazinatorClass(childData);
            }
            bytesSoFar += lengthCollectionMember_item1;
            
            int? item2 = span.ToDecompressedNullableInt(ref bytesSoFar);
            
            var tupleType = (item1, item2);
            
            return tupleType;
        }
        
        private static void ConvertToBytes__PNonLazinatorClass_C32myitem1_c_C32int_C63_C32myitem2_p(ref BinaryBufferWriter writer, (NonLazinatorClass myitem1, int? myitem2) itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            
            if (itemToConvert.Item1 == null)
            {
                writer.Write((uint)0);
            }
            else
            {
                void actionItem1(ref BinaryBufferWriter w) => NonLazinatorDirectConverter.ConvertToBytes_NonLazinatorClass(ref w, itemToConvert.Item1, includeChildrenMode, verifyCleanness, updateStoredBuffer);
                WriteToBinaryWithIntLengthPrefix(ref writer, actionItem1);
            }
            
            CompressedIntegralTypes.WriteCompressedNullableInt(ref writer, itemToConvert.Item2);
        }
        
    }
}
