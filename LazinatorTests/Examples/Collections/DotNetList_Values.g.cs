//8196952a-d113-1093-1b7d-41e71637b5e4
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.321
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LazinatorTests.Examples.Collections
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
    public partial class DotNetList_Values : ILazinator
    {
        public bool IsStruct => false;
        
        /* Property definitions */
        
        protected int _MyLinkedListInt_ByteIndex;
        protected int _MyListInt_ByteIndex;
        protected int _MyListInt2_ByteIndex;
        protected int _MySortedSetInt_ByteIndex;
        protected virtual int _MyLinkedListInt_ByteLength => _MyListInt_ByteIndex - _MyLinkedListInt_ByteIndex;
        protected virtual int _MyListInt_ByteLength => _MyListInt2_ByteIndex - _MyListInt_ByteIndex;
        protected virtual int _MyListInt2_ByteLength => _MySortedSetInt_ByteIndex - _MyListInt2_ByteIndex;
        private int _DotNetList_Values_EndByteIndex;
        protected virtual int _MySortedSetInt_ByteLength => _DotNetList_Values_EndByteIndex - _MySortedSetInt_ByteIndex;
        
        
        protected LinkedList<int> _MyLinkedListInt;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public LinkedList<int> MyLinkedListInt
        {
            get
            {
                if (!_MyLinkedListInt_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _MyLinkedListInt = default(LinkedList<int>);
                        _MyLinkedListInt_Dirty = true; 
                    }
                    else
                    {
                        LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _MyLinkedListInt_ByteIndex, _MyLinkedListInt_ByteLength, false, false, null);
                        _MyLinkedListInt = ConvertFromBytes_LinkedList_Gint_g(childData);
                    }
                    _MyLinkedListInt_Accessed = true;
                } 
                return _MyLinkedListInt;
            }
            set
            {
                IsDirty = true;
                DescendantIsDirty = true;
                _MyLinkedListInt = value;
                _MyLinkedListInt_Dirty = true;
                _MyLinkedListInt_Accessed = true;
            }
        }
        protected bool _MyLinkedListInt_Accessed;
        
        private bool _MyLinkedListInt_Dirty;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool MyLinkedListInt_Dirty
        {
            get => _MyLinkedListInt_Dirty;
            set
            {
                if (_MyLinkedListInt_Dirty != value)
                {
                    _MyLinkedListInt_Dirty = value;
                }
                if (value && !IsDirty)
                {
                    IsDirty = true;
                }
            }
        }
        
        protected List<int> _MyListInt;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public List<int> MyListInt
        {
            get
            {
                if (!_MyListInt_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _MyListInt = default(List<int>);
                        _MyListInt_Dirty = true; 
                    }
                    else
                    {
                        LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _MyListInt_ByteIndex, _MyListInt_ByteLength, false, false, null);
                        _MyListInt = ConvertFromBytes_List_Gint_g(childData);
                    }
                    _MyListInt_Accessed = true;
                } 
                return _MyListInt;
            }
            set
            {
                IsDirty = true;
                DescendantIsDirty = true;
                _MyListInt = value;
                _MyListInt_Dirty = true;
                _MyListInt_Accessed = true;
            }
        }
        protected bool _MyListInt_Accessed;
        
        private bool _MyListInt_Dirty;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool MyListInt_Dirty
        {
            get => _MyListInt_Dirty;
            set
            {
                if (_MyListInt_Dirty != value)
                {
                    _MyListInt_Dirty = value;
                }
                if (value && !IsDirty)
                {
                    IsDirty = true;
                }
            }
        }
        
        protected List<int> _MyListInt2;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public List<int> MyListInt2
        {
            get
            {
                if (!_MyListInt2_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _MyListInt2 = default(List<int>);
                    }
                    else
                    {
                        LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _MyListInt2_ByteIndex, _MyListInt2_ByteLength, false, false, null);
                        _MyListInt2 = ConvertFromBytes_List_Gint_g(childData);
                    }
                    _MyListInt2_Accessed = true;
                }
                IsDirty = true; 
                return _MyListInt2;
            }
            set
            {
                IsDirty = true;
                DescendantIsDirty = true;
                _MyListInt2 = value;
                _MyListInt2_Accessed = true;
            }
        }
        protected bool _MyListInt2_Accessed;
        
        protected SortedSet<int> _MySortedSetInt;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public SortedSet<int> MySortedSetInt
        {
            get
            {
                if (!_MySortedSetInt_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _MySortedSetInt = default(SortedSet<int>);
                        _MySortedSetInt_Dirty = true; 
                    }
                    else
                    {
                        LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _MySortedSetInt_ByteIndex, _MySortedSetInt_ByteLength, false, false, null);
                        _MySortedSetInt = ConvertFromBytes_SortedSet_Gint_g(childData);
                    }
                    _MySortedSetInt_Accessed = true;
                } 
                return _MySortedSetInt;
            }
            set
            {
                IsDirty = true;
                DescendantIsDirty = true;
                _MySortedSetInt = value;
                _MySortedSetInt_Dirty = true;
                _MySortedSetInt_Accessed = true;
            }
        }
        protected bool _MySortedSetInt_Accessed;
        
        private bool _MySortedSetInt_Dirty;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool MySortedSetInt_Dirty
        {
            get => _MySortedSetInt_Dirty;
            set
            {
                if (_MySortedSetInt_Dirty != value)
                {
                    _MySortedSetInt_Dirty = value;
                }
                if (value && !IsDirty)
                {
                    IsDirty = true;
                }
            }
        }
        
        /* Serialization, deserialization, and object relationships */
        
        public virtual LazinatorParentsCollection LazinatorParents { get; set; }
        
        public virtual IncludeChildrenMode OriginalIncludeChildrenMode { get; set; }
        
        public virtual int Deserialize()
        {
            FreeInMemoryObjects();
            int bytesSoFar = 0;
            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
            if (span.Length == 0)
            {
                return 0;
            }
            
            LazinatorGenericID = GetGenericIDIfApplicable(ContainsOpenGenericParameters, LazinatorUniqueID, span, ref bytesSoFar);
            
            int lazinatorLibraryVersion = span.ToDecompressedInt(ref bytesSoFar);
            
            int serializedVersionNumber = span.ToDecompressedInt(ref bytesSoFar);
            
            OriginalIncludeChildrenMode = (IncludeChildrenMode)span.ToByte(ref bytesSoFar);
            
            ConvertFromBytesAfterHeader(OriginalIncludeChildrenMode, serializedVersionNumber, ref bytesSoFar);
            return bytesSoFar;
        }
        
        public virtual LazinatorMemory SerializeLazinator(IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer) => EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, verifyCleanness, IsDirty, DescendantIsDirty, false, LazinatorMemoryStorage, updateStoredBuffer, this);
        
        public virtual ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode = IncludeChildrenMode.IncludeAllChildren, CloneBufferOptions cloneBufferOptions = CloneBufferOptions.IndependentBuffers)
        {
            var clone = new DotNetList_Values()
            {
                OriginalIncludeChildrenMode = includeChildrenMode
            };
            clone = CompleteClone(this, clone, includeChildrenMode, cloneBufferOptions);
            return clone;
        }
        
        public virtual ILazinator AssignCloneProperties(ILazinator clone, IncludeChildrenMode includeChildrenMode)
        {
            clone.FreeInMemoryObjects();
            DotNetList_Values typedClone = (DotNetList_Values) clone;
            typedClone.MyLinkedListInt = CloneOrChange_LinkedList_Gint_g(MyLinkedListInt, l => l?.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer), false);
            typedClone.MyListInt = CloneOrChange_List_Gint_g(MyListInt, l => l?.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer), false);
            typedClone.MyListInt2 = CloneOrChange_List_Gint_g(MyListInt2, l => l?.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer), false);
            typedClone.MySortedSetInt = CloneOrChange_SortedSet_Gint_g(MySortedSetInt, l => l?.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer), false);
            
            return typedClone;
        }
        
        public virtual bool HasChanged { get; set; }
        
        protected bool _IsDirty;
        public virtual bool IsDirty
        {
            [DebuggerStepThrough]
            get => _IsDirty|| LazinatorObjectBytes.Length == 0;
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
        
        protected bool _DescendantHasChanged;
        public virtual bool DescendantHasChanged
        {
            [DebuggerStepThrough]
            get => _DescendantHasChanged;
            [DebuggerStepThrough]
            set
            {
                _DescendantHasChanged = value;
            }
        }
        
        protected bool _DescendantIsDirty;
        public virtual bool DescendantIsDirty
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
        
        public virtual void DeserializeLazinator(LazinatorMemory serializedBytes)
        {
            LazinatorMemoryStorage = serializedBytes;
            int length = Deserialize();
            if (length != LazinatorMemoryStorage.Length)
            {
                LazinatorMemoryStorage = LazinatorMemoryStorage.Slice(0, length);
            }
        }
        
        public virtual LazinatorMemory LazinatorMemoryStorage
        {
            get;
            set;
        }
        protected virtual ReadOnlyMemory<byte> LazinatorObjectBytes => LazinatorMemoryStorage.IsEmpty ? LazinatorUtilities.EmptyReadOnlyMemory : LazinatorMemoryStorage.Memory;
        
        public virtual void UpdateStoredBuffer(bool disposePreviousBuffer = false)
        {
            if (!IsDirty && !DescendantIsDirty && LazinatorObjectBytes.Length > 0 && OriginalIncludeChildrenMode == IncludeChildrenMode.IncludeAllChildren)
            {
                return;
            }
            var previousBuffer = LazinatorMemoryStorage;
            LazinatorMemoryStorage = EncodeOrRecycleToNewBuffer(IncludeChildrenMode.IncludeAllChildren, OriginalIncludeChildrenMode, false, IsDirty, DescendantIsDirty, false, previousBuffer, true, this);
            OriginalIncludeChildrenMode = IncludeChildrenMode.IncludeAllChildren;
            if (disposePreviousBuffer)
            {
                previousBuffer.Dispose();
            }
        }
        
        public virtual int GetByteLength()
        {
            UpdateStoredBuffer();
            return LazinatorObjectBytes.Length;
        }
        
        public virtual bool NonBinaryHash32 => false;
        
        
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
        
        public virtual IEnumerable<(string propertyName, ILazinator descendant)> EnumerateLazinatorDescendants(Func<ILazinator, bool> matchCriterion, bool stopExploringBelowMatch, Func<ILazinator, bool> exploreCriterion, bool exploreOnlyDeserializedChildren, bool enumerateNulls)
        {
            yield break;
        }
        
        
        public virtual IEnumerable<(string propertyName, object descendant)> EnumerateNonLazinatorProperties()
        {
            yield return ("MyLinkedListInt", (object)MyLinkedListInt);
            yield return ("MyListInt", (object)MyListInt);
            yield return ("MyListInt2", (object)MyListInt2);
            yield return ("MySortedSetInt", (object)MySortedSetInt);
            yield break;
        }
        
        public virtual ILazinator ForEachLazinator(Func<ILazinator, ILazinator> changeFunc, bool exploreOnlyDeserializedChildren)
        {
            if ((!exploreOnlyDeserializedChildren && MyLinkedListInt != null) || (_MyLinkedListInt_Accessed && _MyLinkedListInt != null))
            {
                _MyLinkedListInt = (LinkedList<int>) CloneOrChange_LinkedList_Gint_g(_MyLinkedListInt, l => l?.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren), true);
            }
            if ((!exploreOnlyDeserializedChildren && MyListInt != null) || (_MyListInt_Accessed && _MyListInt != null))
            {
                _MyListInt = (List<int>) CloneOrChange_List_Gint_g(_MyListInt, l => l?.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren), true);
            }
            if ((!exploreOnlyDeserializedChildren && MyListInt2 != null) || (_MyListInt2_Accessed && _MyListInt2 != null))
            {
                _MyListInt2 = (List<int>) CloneOrChange_List_Gint_g(_MyListInt2, l => l?.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren), true);
            }
            if ((!exploreOnlyDeserializedChildren && MySortedSetInt != null) || (_MySortedSetInt_Accessed && _MySortedSetInt != null))
            {
                _MySortedSetInt = (SortedSet<int>) CloneOrChange_SortedSet_Gint_g(_MySortedSetInt, l => l?.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren), true);
            }
            return changeFunc(this);
        }
        
        public virtual void FreeInMemoryObjects()
        {
            _MyLinkedListInt = default;
            _MyListInt = default;
            _MyListInt2 = default;
            _MySortedSetInt = default;
            _MyLinkedListInt_Accessed = _MyListInt_Accessed = _MyListInt2_Accessed = _MySortedSetInt_Accessed = false;
            IsDirty = false;
            DescendantIsDirty = false;
            HasChanged = false;
            DescendantHasChanged = false;
        }
        
        /* Conversion */
        
        public virtual int LazinatorUniqueID => 209;
        
        protected virtual bool ContainsOpenGenericParameters => false;
        protected virtual LazinatorGenericIDType _LazinatorGenericID { get; set; }
        public virtual LazinatorGenericIDType LazinatorGenericID
        {
            get => default;
            set { }
        }
        
        public virtual int LazinatorObjectVersion { get; set; } = 0;
        
        
        public virtual void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
            _MyLinkedListInt_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            _MyListInt_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            _MyListInt2_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            _MySortedSetInt_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            _DotNetList_Values_EndByteIndex = bytesSoFar;
        }
        
        public virtual void SerializeExistingBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
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
        
        public virtual void UpdateStoredBuffer(ref BinaryBufferWriter writer, int startPosition, int length, IncludeChildrenMode includeChildrenMode, bool updateDeserializedChildren)
        {
            _IsDirty = false;
            if (includeChildrenMode == IncludeChildrenMode.IncludeAllChildren)
            {
                _DescendantIsDirty = false;
                if (updateDeserializedChildren)
                {
                    if (_MyLinkedListInt_Accessed && _MyLinkedListInt != null)
                    {
                        _MyLinkedListInt = (LinkedList<int>) CloneOrChange_LinkedList_Gint_g(_MyLinkedListInt, l => l.RemoveBufferInHierarchy(), true);
                    }
                    if (_MyListInt_Accessed && _MyListInt != null)
                    {
                        _MyListInt = (List<int>) CloneOrChange_List_Gint_g(_MyListInt, l => l.RemoveBufferInHierarchy(), true);
                    }
                    if (_MyListInt2_Accessed && _MyListInt2 != null)
                    {
                        _MyListInt2 = (List<int>) CloneOrChange_List_Gint_g(_MyListInt2, l => l.RemoveBufferInHierarchy(), true);
                    }
                    if (_MySortedSetInt_Accessed && _MySortedSetInt != null)
                    {
                        _MySortedSetInt = (SortedSet<int>) CloneOrChange_SortedSet_Gint_g(_MySortedSetInt, l => l.RemoveBufferInHierarchy(), true);
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
        
        protected virtual void WritePropertiesIntoBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer, bool includeUniqueID)
        {
            int startPosition = writer.Position;
            int startOfObjectPosition = 0;
            // header information
            if (includeUniqueID)
            {
                if (LazinatorGenericID.IsEmpty)
                {
                    CompressedIntegralTypes.WriteCompressedInt(ref writer, LazinatorUniqueID);
                }
                else
                {
                    WriteLazinatorGenericID(ref writer, LazinatorGenericID);
                }
            }
            CompressedIntegralTypes.WriteCompressedInt(ref writer, Lazinator.Support.LazinatorVersionInfo.LazinatorIntVersion);
            CompressedIntegralTypes.WriteCompressedInt(ref writer, LazinatorObjectVersion);
            writer.Write((byte)includeChildrenMode);
            // write properties
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode && !_MyLinkedListInt_Accessed)
            {
                var deserialized = MyLinkedListInt;
            }
            WriteNonLazinatorObject(
            nonLazinatorObject: _MyLinkedListInt, isBelievedDirty: MyLinkedListInt_Dirty || (includeChildrenMode != OriginalIncludeChildrenMode),
            isAccessed: _MyLinkedListInt_Accessed, writer: ref writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorMemoryStorage, _MyLinkedListInt_ByteIndex, _MyLinkedListInt_ByteLength, false, false, null),
            verifyCleanness: verifyCleanness,
            binaryWriterAction: (ref BinaryBufferWriter w, bool v) =>
            ConvertToBytes_LinkedList_Gint_g(ref w, _MyLinkedListInt,
            includeChildrenMode, v, updateStoredBuffer));
            if (updateStoredBuffer)
            {
                _MyLinkedListInt_ByteIndex = startOfObjectPosition - startPosition;
            }
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode && !_MyListInt_Accessed)
            {
                var deserialized = MyListInt;
            }
            WriteNonLazinatorObject(
            nonLazinatorObject: _MyListInt, isBelievedDirty: MyListInt_Dirty || (includeChildrenMode != OriginalIncludeChildrenMode),
            isAccessed: _MyListInt_Accessed, writer: ref writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorMemoryStorage, _MyListInt_ByteIndex, _MyListInt_ByteLength, false, false, null),
            verifyCleanness: verifyCleanness,
            binaryWriterAction: (ref BinaryBufferWriter w, bool v) =>
            ConvertToBytes_List_Gint_g(ref w, _MyListInt,
            includeChildrenMode, v, updateStoredBuffer));
            if (updateStoredBuffer)
            {
                _MyListInt_ByteIndex = startOfObjectPosition - startPosition;
            }
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode && !_MyListInt2_Accessed)
            {
                var deserialized = MyListInt2;
            }
            WriteNonLazinatorObject(
            nonLazinatorObject: _MyListInt2, isBelievedDirty: _MyListInt2_Accessed || (includeChildrenMode != OriginalIncludeChildrenMode),
            isAccessed: _MyListInt2_Accessed, writer: ref writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorMemoryStorage, _MyListInt2_ByteIndex, _MyListInt2_ByteLength, false, false, null),
            verifyCleanness: false,
            binaryWriterAction: (ref BinaryBufferWriter w, bool v) =>
            ConvertToBytes_List_Gint_g(ref w, _MyListInt2,
            includeChildrenMode, v, updateStoredBuffer));
            if (updateStoredBuffer)
            {
                _MyListInt2_ByteIndex = startOfObjectPosition - startPosition;
            }
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode && !_MySortedSetInt_Accessed)
            {
                var deserialized = MySortedSetInt;
            }
            WriteNonLazinatorObject(
            nonLazinatorObject: _MySortedSetInt, isBelievedDirty: MySortedSetInt_Dirty || (includeChildrenMode != OriginalIncludeChildrenMode),
            isAccessed: _MySortedSetInt_Accessed, writer: ref writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorMemoryStorage, _MySortedSetInt_ByteIndex, _MySortedSetInt_ByteLength, false, false, null),
            verifyCleanness: verifyCleanness,
            binaryWriterAction: (ref BinaryBufferWriter w, bool v) =>
            ConvertToBytes_SortedSet_Gint_g(ref w, _MySortedSetInt,
            includeChildrenMode, v, updateStoredBuffer));
            if (updateStoredBuffer)
            {
                _MySortedSetInt_ByteIndex = startOfObjectPosition - startPosition;
            }
            if (updateStoredBuffer)
            {
                _DotNetList_Values_EndByteIndex = writer.Position - startPosition;
            }
        }
        
        /* Conversion of supported collections and tuples */
        
        private static LinkedList<int> ConvertFromBytes_LinkedList_Gint_g(LazinatorMemory storage)
        {
            if (storage.Length == 0)
            {
                return default(LinkedList<int>);
            }
            ReadOnlySpan<byte> span = storage.Span;
            
            int bytesSoFar = 0;
            int collectionLength = span.ToDecompressedInt(ref bytesSoFar);
            
            LinkedList<int> collection = new LinkedList<int>();
            for (int itemIndex = 0; itemIndex < collectionLength; itemIndex++)
            {
                int item = span.ToDecompressedInt(ref bytesSoFar);
                collection.AddLast(item);
            }
            
            return collection;
        }
        
        private static void ConvertToBytes_LinkedList_Gint_g(ref BinaryBufferWriter writer, LinkedList<int> itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            if (itemToConvert == default(LinkedList<int>))
            {
                return;
            }
            CompressedIntegralTypes.WriteCompressedInt(ref writer, itemToConvert.Count);
            int itemToConvertCount = itemToConvert.Count;
            for (int itemIndex = 0; itemIndex < itemToConvertCount; itemIndex++)
            {
                CompressedIntegralTypes.WriteCompressedInt(ref writer, System.Linq.Enumerable.ElementAt(itemToConvert, itemIndex));
            }
        }
        
        private static LinkedList<int> CloneOrChange_LinkedList_Gint_g(LinkedList<int> itemToClone, Func<ILazinator, ILazinator> cloneOrChangeFunc, bool avoidCloningIfPossible)
        {
            if (itemToClone == null)
            {
                return default;
            }
            int collectionLength = itemToClone.Count;
            LinkedList<int> collection = new LinkedList<int>();
            int itemToCloneCount = itemToClone.Count;
            for (int itemIndex = 0; itemIndex < itemToCloneCount; itemIndex++)
            {
                var itemCopied = (int) System.Linq.Enumerable.ElementAt(itemToClone, itemIndex);
                collection.AddLast(itemCopied);
            }
            return collection;
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
            for (int itemIndex = 0; itemIndex < collectionLength; itemIndex++)
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
        
        private static List<int> CloneOrChange_List_Gint_g(List<int> itemToClone, Func<ILazinator, ILazinator> cloneOrChangeFunc, bool avoidCloningIfPossible)
        {
            if (itemToClone == null)
            {
                return default;
            }
            int collectionLength = itemToClone.Count;
            List<int> collection = new List<int>(collectionLength);
            int itemToCloneCount = itemToClone.Count;
            for (int itemIndex = 0; itemIndex < itemToCloneCount; itemIndex++)
            {
                var itemCopied = (int) itemToClone[itemIndex];
                collection.Add(itemCopied);
            }
            return collection;
        }
        
        private static SortedSet<int> ConvertFromBytes_SortedSet_Gint_g(LazinatorMemory storage)
        {
            if (storage.Length == 0)
            {
                return default(SortedSet<int>);
            }
            ReadOnlySpan<byte> span = storage.Span;
            
            int bytesSoFar = 0;
            int collectionLength = span.ToDecompressedInt(ref bytesSoFar);
            
            SortedSet<int> collection = new SortedSet<int>();
            for (int itemIndex = 0; itemIndex < collectionLength; itemIndex++)
            {
                int item = span.ToDecompressedInt(ref bytesSoFar);
                collection.Add(item);
            }
            
            return collection;
        }
        
        private static void ConvertToBytes_SortedSet_Gint_g(ref BinaryBufferWriter writer, SortedSet<int> itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            if (itemToConvert == default(SortedSet<int>))
            {
                return;
            }
            CompressedIntegralTypes.WriteCompressedInt(ref writer, itemToConvert.Count);
            int itemToConvertCount = itemToConvert.Count;
            var sortedSet = System.Linq.Enumerable.ToList(itemToConvert);
            for (int itemIndex = 0; itemIndex < itemToConvertCount; itemIndex++)
            {
                CompressedIntegralTypes.WriteCompressedInt(ref writer, sortedSet[itemIndex]);
            }
        }
        
        private static SortedSet<int> CloneOrChange_SortedSet_Gint_g(SortedSet<int> itemToClone, Func<ILazinator, ILazinator> cloneOrChangeFunc, bool avoidCloningIfPossible)
        {
            if (itemToClone == null)
            {
                return default;
            }
            int collectionLength = itemToClone.Count;
            SortedSet<int> collection = new SortedSet<int>();
            int itemToCloneCount = itemToClone.Count;
            var sortedSet = System.Linq.Enumerable.ToList(itemToClone);
            for (int itemIndex = 0; itemIndex < itemToCloneCount; itemIndex++)
            {
                var itemCopied = (int) sortedSet[itemIndex];
                collection.Add(itemCopied);
            }
            return collection;
        }
        
    }
}
