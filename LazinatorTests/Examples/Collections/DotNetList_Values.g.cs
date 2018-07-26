//dfbe57d6-5ce9-cd52-aa9f-1799bbbe1d66
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.198
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
        
        /* Serialization, deserialization, and object relationships */
        
        public virtual LazinatorParentsCollection LazinatorParents { get; set; }
        
        protected IncludeChildrenMode OriginalIncludeChildrenMode;
        
        public virtual int Deserialize()
        {
            ResetAccessedProperties();
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
        
        public virtual MemoryInBuffer SerializeNewBuffer(IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
            return EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, verifyCleanness, IsDirty, DescendantIsDirty, false, LazinatorObjectBytes, (StreamManuallyDelegate) EncodeToNewBuffer);
        }
        
        protected virtual MemoryInBuffer EncodeToNewBuffer(IncludeChildrenMode includeChildrenMode, bool verifyCleanness) => LazinatorUtilities.EncodeToNewBinaryBufferWriter(this, includeChildrenMode, verifyCleanness);
        
        public virtual ILazinator CloneLazinator()
        {
            return CloneLazinator(OriginalIncludeChildrenMode);
        }
        
        public virtual ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode)
        {
            MemoryInBuffer bytes = EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, false, IsDirty, DescendantIsDirty, false, LazinatorObjectBytes, (StreamManuallyDelegate)EncodeToNewBuffer);
            var clone = new DotNetList_Values()
            {
                LazinatorParents = LazinatorParents,
                OriginalIncludeChildrenMode = includeChildrenMode,
                HierarchyBytes = bytes,
            };
            clone.LazinatorParents = default;
            return clone;
        }
        
        public virtual bool HasChanged { get; set; }
        
        protected bool _IsDirty;
        public virtual bool IsDirty
        {
            [DebuggerStepThrough]
            get => _IsDirty || _LazinatorObjectBytes.Length == 0;
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
        
        private MemoryInBuffer _HierarchyBytes;
        public virtual MemoryInBuffer HierarchyBytes
        {
            set
            {
                _HierarchyBytes = value;
                LazinatorObjectBytes = value.FilledMemory;
            }
        }
        
        protected ReadOnlyMemory<byte> _LazinatorObjectBytes;
        public virtual ReadOnlyMemory<byte> LazinatorObjectBytes
        {
            get => _LazinatorObjectBytes;
            set
            {
                _LazinatorObjectBytes = value;
                int length = Deserialize();
                _LazinatorObjectBytes = _LazinatorObjectBytes.Slice(0, length);
            }
        }
        
        public virtual void LazinatorConvertToBytes()
        {
            if (!IsDirty && !DescendantIsDirty && _LazinatorObjectBytes.Length > 0)
            {
                return;
            }
            MemoryInBuffer bytes = EncodeOrRecycleToNewBuffer(IncludeChildrenMode.IncludeAllChildren, OriginalIncludeChildrenMode, false, IsDirty, DescendantIsDirty, false, LazinatorObjectBytes, (StreamManuallyDelegate)EncodeToNewBuffer);
            _LazinatorObjectBytes = bytes.FilledMemory;
        }
        
        public virtual int GetByteLength()
        {
            LazinatorConvertToBytes();
            return _LazinatorObjectBytes.Length;
        }
        
        public virtual uint GetBinaryHashCode32()
        {
            LazinatorConvertToBytes();
            return FarmhashByteSpans.Hash32(LazinatorObjectBytes.Span);
        }
        
        public virtual ulong GetBinaryHashCode64()
        {
            LazinatorConvertToBytes();
            return FarmhashByteSpans.Hash64(LazinatorObjectBytes.Span);
        }
        
        public virtual Guid GetBinaryHashCode128()
        {
            LazinatorConvertToBytes();
            return FarmhashByteSpans.Hash128(LazinatorObjectBytes.Span);
        }
        
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
        public LinkedList<int> MyLinkedListInt
        {
            get
            {
                if (!_MyLinkedListInt_Accessed)
                {
                    if (_LazinatorObjectBytes.Length == 0)
                    {
                        _MyLinkedListInt = default(LinkedList<int>);
                        _MyLinkedListInt_Dirty = true; 
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _MyLinkedListInt_ByteIndex, _MyLinkedListInt_ByteLength, false, false, null);
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
        public bool MyLinkedListInt_Dirty
        {
            get => _MyLinkedListInt_Dirty;
            set
            {
                if (_MyLinkedListInt_Dirty != value)
                {
                    _MyLinkedListInt_Dirty = value;
                    if (value && !IsDirty)
                    {
                        IsDirty = true;
                    }
                }
            }
        }
        protected List<int> _MyListInt;
        public List<int> MyListInt
        {
            get
            {
                if (!_MyListInt_Accessed)
                {
                    if (_LazinatorObjectBytes.Length == 0)
                    {
                        _MyListInt = default(List<int>);
                        _MyListInt_Dirty = true; 
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _MyListInt_ByteIndex, _MyListInt_ByteLength, false, false, null);
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
        public bool MyListInt_Dirty
        {
            get => _MyListInt_Dirty;
            set
            {
                if (_MyListInt_Dirty != value)
                {
                    _MyListInt_Dirty = value;
                    if (value && !IsDirty)
                    {
                        IsDirty = true;
                    }
                }
            }
        }
        protected List<int> _MyListInt2;
        public List<int> MyListInt2
        {
            get
            {
                if (!_MyListInt2_Accessed)
                {
                    if (_LazinatorObjectBytes.Length == 0)
                    {
                        _MyListInt2 = default(List<int>);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _MyListInt2_ByteIndex, _MyListInt2_ByteLength, false, false, null);
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
        public SortedSet<int> MySortedSetInt
        {
            get
            {
                if (!_MySortedSetInt_Accessed)
                {
                    if (_LazinatorObjectBytes.Length == 0)
                    {
                        _MySortedSetInt = default(SortedSet<int>);
                        _MySortedSetInt_Dirty = true; 
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _MySortedSetInt_ByteIndex, _MySortedSetInt_ByteLength, false, false, null);
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
        public bool MySortedSetInt_Dirty
        {
            get => _MySortedSetInt_Dirty;
            set
            {
                if (_MySortedSetInt_Dirty != value)
                {
                    _MySortedSetInt_Dirty = value;
                    if (value && !IsDirty)
                    {
                        IsDirty = true;
                    }
                }
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
        
        protected virtual void ResetAccessedProperties()
        {
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
                
                _IsDirty = false;
                if (includeChildrenMode == IncludeChildrenMode.IncludeAllChildren)
                {
                    _DescendantIsDirty = false;
                }
                else
                {
                    throw new Exception("Cannot update stored buffer when serializing only some children.");
                }
                
                _LazinatorObjectBytes = writer.Slice(startPosition);
            }
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
            if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren && !_MyLinkedListInt_Accessed)
            {
                var deserialized = MyLinkedListInt;
            }
            WriteNonLazinatorObject(
            nonLazinatorObject: _MyLinkedListInt, isBelievedDirty: MyLinkedListInt_Dirty,
            isAccessed: _MyLinkedListInt_Accessed, writer: ref writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorObjectBytes, _MyLinkedListInt_ByteIndex, _MyLinkedListInt_ByteLength, false, false, null),
            verifyCleanness: verifyCleanness,
            binaryWriterAction: (ref BinaryBufferWriter w, bool v) =>
            ConvertToBytes_LinkedList_Gint_g(ref w, _MyLinkedListInt,
            includeChildrenMode, v, updateStoredBuffer));
            if (updateStoredBuffer)
            {
                _MyLinkedListInt_ByteIndex = startOfObjectPosition - startPosition;
            }
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren && !_MyListInt_Accessed)
            {
                var deserialized = MyListInt;
            }
            WriteNonLazinatorObject(
            nonLazinatorObject: _MyListInt, isBelievedDirty: MyListInt_Dirty,
            isAccessed: _MyListInt_Accessed, writer: ref writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorObjectBytes, _MyListInt_ByteIndex, _MyListInt_ByteLength, false, false, null),
            verifyCleanness: verifyCleanness,
            binaryWriterAction: (ref BinaryBufferWriter w, bool v) =>
            ConvertToBytes_List_Gint_g(ref w, _MyListInt,
            includeChildrenMode, v, updateStoredBuffer));
            if (updateStoredBuffer)
            {
                _MyListInt_ByteIndex = startOfObjectPosition - startPosition;
            }
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren && !_MyListInt2_Accessed)
            {
                var deserialized = MyListInt2;
            }
            WriteNonLazinatorObject(
            nonLazinatorObject: _MyListInt2, isBelievedDirty: _MyListInt2_Accessed,
            isAccessed: _MyListInt2_Accessed, writer: ref writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorObjectBytes, _MyListInt2_ByteIndex, _MyListInt2_ByteLength, false, false, null),
            verifyCleanness: false,
            binaryWriterAction: (ref BinaryBufferWriter w, bool v) =>
            ConvertToBytes_List_Gint_g(ref w, _MyListInt2,
            includeChildrenMode, v, updateStoredBuffer));
            if (updateStoredBuffer)
            {
                _MyListInt2_ByteIndex = startOfObjectPosition - startPosition;
            }
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren && !_MySortedSetInt_Accessed)
            {
                var deserialized = MySortedSetInt;
            }
            WriteNonLazinatorObject(
            nonLazinatorObject: _MySortedSetInt, isBelievedDirty: MySortedSetInt_Dirty,
            isAccessed: _MySortedSetInt_Accessed, writer: ref writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorObjectBytes, _MySortedSetInt_ByteIndex, _MySortedSetInt_ByteLength, false, false, null),
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
        
        private static LinkedList<int> ConvertFromBytes_LinkedList_Gint_g(ReadOnlyMemory<byte> storage)
        {
            if (storage.Length == 0)
            {
                return default(LinkedList<int>);
            }
            ReadOnlySpan<byte> span = storage.Span;
            
            int bytesSoFar = 0;
            int collectionLength = span.ToDecompressedInt(ref bytesSoFar);
            
            LinkedList<int> collection = new LinkedList<int>();
            for (int i = 0; i < collectionLength; i++)
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
        
        private static List<int> ConvertFromBytes_List_Gint_g(ReadOnlyMemory<byte> storage)
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
        
        private static SortedSet<int> ConvertFromBytes_SortedSet_Gint_g(ReadOnlyMemory<byte> storage)
        {
            if (storage.Length == 0)
            {
                return default(SortedSet<int>);
            }
            ReadOnlySpan<byte> span = storage.Span;
            
            int bytesSoFar = 0;
            int collectionLength = span.ToDecompressedInt(ref bytesSoFar);
            
            SortedSet<int> collection = new SortedSet<int>();
            for (int i = 0; i < collectionLength; i++)
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
        
    }
}
