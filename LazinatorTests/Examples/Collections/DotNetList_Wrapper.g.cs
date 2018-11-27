//015cb272-2f79-5947-3c77-01131f07a371
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.330
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
    using Lazinator.Wrappers;
    using System;
    using System.Buffers;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;
    using static Lazinator.Core.LazinatorUtilities;
    
    [Autogenerated]
    public partial class DotNetList_Wrapper : ILazinator
    {
        public bool IsStruct => false;
        
        /* Property definitions */
        
        protected int _MyListInt_ByteIndex;
        protected int _MyListNullableByte_ByteIndex;
        protected int _MyListNullableInt_ByteIndex;
        protected virtual int _MyListInt_ByteLength => _MyListNullableByte_ByteIndex - _MyListInt_ByteIndex;
        protected virtual int _MyListNullableByte_ByteLength => _MyListNullableInt_ByteIndex - _MyListNullableByte_ByteIndex;
        private int _DotNetList_Wrapper_EndByteIndex;
        protected virtual int _MyListNullableInt_ByteLength => _DotNetList_Wrapper_EndByteIndex - _MyListNullableInt_ByteIndex;
        
        
        protected List<WInt> _MyListInt;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public List<WInt> MyListInt
        {
            get
            {
                if (!_MyListInt_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _MyListInt = default(List<WInt>);
                        _MyListInt_Dirty = true; 
                    }
                    else
                    {
                        LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _MyListInt_ByteIndex, _MyListInt_ByteLength, false, false, null);
                        _MyListInt = ConvertFromBytes_List_GWInt_g(childData);
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
        
        protected List<WNullableByte> _MyListNullableByte;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public List<WNullableByte> MyListNullableByte
        {
            get
            {
                if (!_MyListNullableByte_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _MyListNullableByte = default(List<WNullableByte>);
                    }
                    else
                    {
                        LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _MyListNullableByte_ByteIndex, _MyListNullableByte_ByteLength, false, false, null);
                        _MyListNullableByte = ConvertFromBytes_List_GWNullableByte_g(childData);
                    }
                    _MyListNullableByte_Accessed = true;
                }
                IsDirty = true; 
                return _MyListNullableByte;
            }
            set
            {
                IsDirty = true;
                DescendantIsDirty = true;
                _MyListNullableByte = value;
                _MyListNullableByte_Accessed = true;
            }
        }
        protected bool _MyListNullableByte_Accessed;
        
        protected List<WNullableInt> _MyListNullableInt;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public List<WNullableInt> MyListNullableInt
        {
            get
            {
                if (!_MyListNullableInt_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _MyListNullableInt = default(List<WNullableInt>);
                    }
                    else
                    {
                        LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _MyListNullableInt_ByteIndex, _MyListNullableInt_ByteLength, false, false, null);
                        _MyListNullableInt = ConvertFromBytes_List_GWNullableInt_g(childData);
                    }
                    _MyListNullableInt_Accessed = true;
                }
                IsDirty = true; 
                return _MyListNullableInt;
            }
            set
            {
                IsDirty = true;
                DescendantIsDirty = true;
                _MyListNullableInt = value;
                _MyListNullableInt_Accessed = true;
            }
        }
        protected bool _MyListNullableInt_Accessed;
        
        /* Serialization, deserialization, and object relationships */
        
        public DotNetList_Wrapper() : base()
        {
        }
        
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
            var clone = new DotNetList_Wrapper()
            {
                OriginalIncludeChildrenMode = includeChildrenMode
            };
            clone = CompleteClone(this, clone, includeChildrenMode, cloneBufferOptions);
            return clone;
        }
        
        public virtual ILazinator AssignCloneProperties(ILazinator clone, IncludeChildrenMode includeChildrenMode)
        {
            clone.FreeInMemoryObjects();
            DotNetList_Wrapper typedClone = (DotNetList_Wrapper) clone;
            typedClone.MyListInt = CloneOrChange_List_GWInt_g(MyListInt, l => l?.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer), false);
            typedClone.MyListNullableByte = CloneOrChange_List_GWNullableByte_g(MyListNullableByte, l => l?.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer), false);
            typedClone.MyListNullableInt = CloneOrChange_List_GWNullableInt_g(MyListNullableInt, l => l?.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer), false);
            
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
            yield return ("MyListInt", (object)MyListInt);
            yield return ("MyListNullableByte", (object)MyListNullableByte);
            yield return ("MyListNullableInt", (object)MyListNullableInt);
            yield break;
        }
        
        public virtual ILazinator ForEachLazinator(Func<ILazinator, ILazinator> changeFunc, bool exploreOnlyDeserializedChildren)
        {
            if ((!exploreOnlyDeserializedChildren && MyListInt != null) || (_MyListInt_Accessed && _MyListInt != null))
            {
                _MyListInt = (List<WInt>) CloneOrChange_List_GWInt_g(_MyListInt, l => l?.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren), true);
            }
            if ((!exploreOnlyDeserializedChildren && MyListNullableByte != null) || (_MyListNullableByte_Accessed && _MyListNullableByte != null))
            {
                _MyListNullableByte = (List<WNullableByte>) CloneOrChange_List_GWNullableByte_g(_MyListNullableByte, l => l?.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren), true);
            }
            if ((!exploreOnlyDeserializedChildren && MyListNullableInt != null) || (_MyListNullableInt_Accessed && _MyListNullableInt != null))
            {
                _MyListNullableInt = (List<WNullableInt>) CloneOrChange_List_GWNullableInt_g(_MyListNullableInt, l => l?.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren), true);
            }
            return changeFunc(this);
        }
        
        public virtual void FreeInMemoryObjects()
        {
            _MyListInt = default;
            _MyListNullableByte = default;
            _MyListNullableInt = default;
            _MyListInt_Accessed = _MyListNullableByte_Accessed = _MyListNullableInt_Accessed = false;
            IsDirty = false;
            DescendantIsDirty = false;
            HasChanged = false;
            DescendantHasChanged = false;
        }
        
        /* Conversion */
        
        public virtual int LazinatorUniqueID => 263;
        
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
            _MyListInt_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            _MyListNullableByte_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            _MyListNullableInt_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            _DotNetList_Wrapper_EndByteIndex = bytesSoFar;
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
                    if (_MyListInt_Accessed && _MyListInt != null)
                    {
                        _MyListInt = (List<WInt>) CloneOrChange_List_GWInt_g(_MyListInt, l => l.RemoveBufferInHierarchy(), true);
                    }
                    if (_MyListNullableByte_Accessed && _MyListNullableByte != null)
                    {
                        _MyListNullableByte = (List<WNullableByte>) CloneOrChange_List_GWNullableByte_g(_MyListNullableByte, l => l.RemoveBufferInHierarchy(), true);
                    }
                    if (_MyListNullableInt_Accessed && _MyListNullableInt != null)
                    {
                        _MyListNullableInt = (List<WNullableInt>) CloneOrChange_List_GWNullableInt_g(_MyListNullableInt, l => l.RemoveBufferInHierarchy(), true);
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
            ConvertToBytes_List_GWInt_g(ref w, _MyListInt,
            includeChildrenMode, v, updateStoredBuffer));
            if (updateStoredBuffer)
            {
                _MyListInt_ByteIndex = startOfObjectPosition - startPosition;
            }
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode && !_MyListNullableByte_Accessed)
            {
                var deserialized = MyListNullableByte;
            }
            WriteNonLazinatorObject(
            nonLazinatorObject: _MyListNullableByte, isBelievedDirty: _MyListNullableByte_Accessed || (includeChildrenMode != OriginalIncludeChildrenMode),
            isAccessed: _MyListNullableByte_Accessed, writer: ref writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorMemoryStorage, _MyListNullableByte_ByteIndex, _MyListNullableByte_ByteLength, false, false, null),
            verifyCleanness: false,
            binaryWriterAction: (ref BinaryBufferWriter w, bool v) =>
            ConvertToBytes_List_GWNullableByte_g(ref w, _MyListNullableByte,
            includeChildrenMode, v, updateStoredBuffer));
            if (updateStoredBuffer)
            {
                _MyListNullableByte_ByteIndex = startOfObjectPosition - startPosition;
            }
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode && !_MyListNullableInt_Accessed)
            {
                var deserialized = MyListNullableInt;
            }
            WriteNonLazinatorObject(
            nonLazinatorObject: _MyListNullableInt, isBelievedDirty: _MyListNullableInt_Accessed || (includeChildrenMode != OriginalIncludeChildrenMode),
            isAccessed: _MyListNullableInt_Accessed, writer: ref writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorMemoryStorage, _MyListNullableInt_ByteIndex, _MyListNullableInt_ByteLength, false, false, null),
            verifyCleanness: false,
            binaryWriterAction: (ref BinaryBufferWriter w, bool v) =>
            ConvertToBytes_List_GWNullableInt_g(ref w, _MyListNullableInt,
            includeChildrenMode, v, updateStoredBuffer));
            if (updateStoredBuffer)
            {
                _MyListNullableInt_ByteIndex = startOfObjectPosition - startPosition;
            }
            if (updateStoredBuffer)
            {
                _DotNetList_Wrapper_EndByteIndex = writer.Position - startPosition;
            }
        }
        
        /* Conversion of supported collections and tuples */
        
        private static List<WInt> ConvertFromBytes_List_GWInt_g(LazinatorMemory storage)
        {
            if (storage.Length == 0)
            {
                return default(List<WInt>);
            }
            ReadOnlySpan<byte> span = storage.Span;
            
            int bytesSoFar = 0;
            int collectionLength = span.ToDecompressedInt(ref bytesSoFar);
            
            List<WInt> collection = new List<WInt>(collectionLength);
            for (int itemIndex = 0; itemIndex < collectionLength; itemIndex++)
            {
                int lengthCollectionMember = span.ToByte(ref bytesSoFar);
                LazinatorMemory childData = storage.Slice(bytesSoFar, lengthCollectionMember);
                var item = new WInt();
                item.DeserializeLazinator(childData);
                collection.Add(item);
                bytesSoFar += lengthCollectionMember;
            }
            
            return collection;
        }
        
        private static void ConvertToBytes_List_GWInt_g(ref BinaryBufferWriter writer, List<WInt> itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            if (itemToConvert == default(List<WInt>))
            {
                return;
            }
            CompressedIntegralTypes.WriteCompressedInt(ref writer, itemToConvert.Count);
            int itemToConvertCount = itemToConvert.Count;
            for (int itemIndex = 0; itemIndex < itemToConvertCount; itemIndex++)
            {
                void action(ref BinaryBufferWriter w) 
                {
                    var copy = itemToConvert[itemIndex];
                    copy.SerializeExistingBuffer(ref w, includeChildrenMode, verifyCleanness, updateStoredBuffer);
                    itemToConvert[itemIndex] = copy;
                }
                WriteToBinaryWithByteLengthPrefix(ref writer, action);
            }
        }
        
        private static List<WInt> CloneOrChange_List_GWInt_g(List<WInt> itemToClone, Func<ILazinator, ILazinator> cloneOrChangeFunc, bool avoidCloningIfPossible)
        {
            if (itemToClone == null)
            {
                return default;
            }
            int collectionLength = itemToClone.Count;
            List<WInt> collection = avoidCloningIfPossible ? itemToClone : new List<WInt>(collectionLength);
            int itemToCloneCount = itemToClone.Count;
            for (int itemIndex = 0; itemIndex < itemToCloneCount; itemIndex++)
            {
                if (avoidCloningIfPossible)
                {
                    if (true)
                    {
                        itemToClone[itemIndex] = (WInt) cloneOrChangeFunc(itemToClone[itemIndex]);
                    }
                    continue;
                }
                var itemCopied = (WInt) cloneOrChangeFunc(itemToClone[itemIndex]);
                collection.Add(itemCopied);
            }
            return collection;
        }
        
        private static List<WNullableByte> ConvertFromBytes_List_GWNullableByte_g(LazinatorMemory storage)
        {
            if (storage.Length == 0)
            {
                return default(List<WNullableByte>);
            }
            ReadOnlySpan<byte> span = storage.Span;
            
            int bytesSoFar = 0;
            int collectionLength = span.ToDecompressedInt(ref bytesSoFar);
            
            List<WNullableByte> collection = new List<WNullableByte>(collectionLength);
            for (int itemIndex = 0; itemIndex < collectionLength; itemIndex++)
            {
                int lengthCollectionMember = span.ToByte(ref bytesSoFar);
                LazinatorMemory childData = storage.Slice(bytesSoFar, lengthCollectionMember);
                var item = new WNullableByte();
                item.DeserializeLazinator(childData);
                collection.Add(item);
                bytesSoFar += lengthCollectionMember;
            }
            
            return collection;
        }
        
        private static void ConvertToBytes_List_GWNullableByte_g(ref BinaryBufferWriter writer, List<WNullableByte> itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            if (itemToConvert == default(List<WNullableByte>))
            {
                return;
            }
            CompressedIntegralTypes.WriteCompressedInt(ref writer, itemToConvert.Count);
            int itemToConvertCount = itemToConvert.Count;
            for (int itemIndex = 0; itemIndex < itemToConvertCount; itemIndex++)
            {
                void action(ref BinaryBufferWriter w) 
                {
                    var copy = itemToConvert[itemIndex];
                    copy.SerializeExistingBuffer(ref w, includeChildrenMode, verifyCleanness, updateStoredBuffer);
                    itemToConvert[itemIndex] = copy;
                }
                WriteToBinaryWithByteLengthPrefix(ref writer, action);
            }
        }
        
        private static List<WNullableByte> CloneOrChange_List_GWNullableByte_g(List<WNullableByte> itemToClone, Func<ILazinator, ILazinator> cloneOrChangeFunc, bool avoidCloningIfPossible)
        {
            if (itemToClone == null)
            {
                return default;
            }
            int collectionLength = itemToClone.Count;
            List<WNullableByte> collection = avoidCloningIfPossible ? itemToClone : new List<WNullableByte>(collectionLength);
            int itemToCloneCount = itemToClone.Count;
            for (int itemIndex = 0; itemIndex < itemToCloneCount; itemIndex++)
            {
                if (avoidCloningIfPossible)
                {
                    if (true)
                    {
                        itemToClone[itemIndex] = (WNullableByte) cloneOrChangeFunc(itemToClone[itemIndex]);
                    }
                    continue;
                }
                var itemCopied = (WNullableByte) cloneOrChangeFunc(itemToClone[itemIndex]);
                collection.Add(itemCopied);
            }
            return collection;
        }
        
        private static List<WNullableInt> ConvertFromBytes_List_GWNullableInt_g(LazinatorMemory storage)
        {
            if (storage.Length == 0)
            {
                return default(List<WNullableInt>);
            }
            ReadOnlySpan<byte> span = storage.Span;
            
            int bytesSoFar = 0;
            int collectionLength = span.ToDecompressedInt(ref bytesSoFar);
            
            List<WNullableInt> collection = new List<WNullableInt>(collectionLength);
            for (int itemIndex = 0; itemIndex < collectionLength; itemIndex++)
            {
                int lengthCollectionMember = span.ToByte(ref bytesSoFar);
                LazinatorMemory childData = storage.Slice(bytesSoFar, lengthCollectionMember);
                var item = new WNullableInt();
                item.DeserializeLazinator(childData);
                collection.Add(item);
                bytesSoFar += lengthCollectionMember;
            }
            
            return collection;
        }
        
        private static void ConvertToBytes_List_GWNullableInt_g(ref BinaryBufferWriter writer, List<WNullableInt> itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            if (itemToConvert == default(List<WNullableInt>))
            {
                return;
            }
            CompressedIntegralTypes.WriteCompressedInt(ref writer, itemToConvert.Count);
            int itemToConvertCount = itemToConvert.Count;
            for (int itemIndex = 0; itemIndex < itemToConvertCount; itemIndex++)
            {
                void action(ref BinaryBufferWriter w) 
                {
                    var copy = itemToConvert[itemIndex];
                    copy.SerializeExistingBuffer(ref w, includeChildrenMode, verifyCleanness, updateStoredBuffer);
                    itemToConvert[itemIndex] = copy;
                }
                WriteToBinaryWithByteLengthPrefix(ref writer, action);
            }
        }
        
        private static List<WNullableInt> CloneOrChange_List_GWNullableInt_g(List<WNullableInt> itemToClone, Func<ILazinator, ILazinator> cloneOrChangeFunc, bool avoidCloningIfPossible)
        {
            if (itemToClone == null)
            {
                return default;
            }
            int collectionLength = itemToClone.Count;
            List<WNullableInt> collection = avoidCloningIfPossible ? itemToClone : new List<WNullableInt>(collectionLength);
            int itemToCloneCount = itemToClone.Count;
            for (int itemIndex = 0; itemIndex < itemToCloneCount; itemIndex++)
            {
                if (avoidCloningIfPossible)
                {
                    if (true)
                    {
                        itemToClone[itemIndex] = (WNullableInt) cloneOrChangeFunc(itemToClone[itemIndex]);
                    }
                    continue;
                }
                var itemCopied = (WNullableInt) cloneOrChangeFunc(itemToClone[itemIndex]);
                collection.Add(itemCopied);
            }
            return collection;
        }
        
    }
}
