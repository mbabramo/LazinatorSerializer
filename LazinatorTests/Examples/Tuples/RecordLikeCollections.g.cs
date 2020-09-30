//b477314b-e2fb-436d-5924-791b770953a3
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.393
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

#nullable disable
namespace LazinatorTests.Examples.Tuples
{
    using Lazinator.Attributes;
    using Lazinator.Buffers;
    using Lazinator.Core;
    using Lazinator.Exceptions;
    using Lazinator.Support;
    using LazinatorTests.Examples;
    using LazinatorTests.Examples.Structs;
    using System;
    using System.Buffers;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;
    using static Lazinator.Core.LazinatorUtilities;
    
    [Autogenerated]
    public partial class RecordLikeCollections : ILazinator
    {
        public bool IsStruct => false;
        
        /* Property definitions */
        
        protected int _MyDictionaryWithRecordLikeContainers_ByteIndex;
        protected int _MyDictionaryWithRecordLikeTypeValues_ByteIndex;
        protected virtual int _MyDictionaryWithRecordLikeContainers_ByteLength => _MyDictionaryWithRecordLikeTypeValues_ByteIndex - _MyDictionaryWithRecordLikeContainers_ByteIndex;
        private int _RecordLikeCollections_EndByteIndex;
        protected virtual int _MyDictionaryWithRecordLikeTypeValues_ByteLength => _RecordLikeCollections_EndByteIndex - _MyDictionaryWithRecordLikeTypeValues_ByteIndex;
        
        
        protected int _MyInt;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public int MyInt
        {
            get
            {
                return _MyInt;
            }
            set
            {
                IsDirty = true;
                _MyInt = value;
            }
        }
        
        protected Dictionary<Int32, RecordLikeContainer> _MyDictionaryWithRecordLikeContainers;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public Dictionary<Int32, RecordLikeContainer> MyDictionaryWithRecordLikeContainers
        {
            get
            {
                if (!_MyDictionaryWithRecordLikeContainers_Accessed)
                {
                    Lazinate_MyDictionaryWithRecordLikeContainers();
                }
                IsDirty = true; 
                return _MyDictionaryWithRecordLikeContainers;
            }
            set
            {
                IsDirty = true;
                DescendantIsDirty = true;
                _MyDictionaryWithRecordLikeContainers = value;
                _MyDictionaryWithRecordLikeContainers_Accessed = true;
            }
        }
        protected bool _MyDictionaryWithRecordLikeContainers_Accessed;
        private void Lazinate_MyDictionaryWithRecordLikeContainers()
        {
            if (LazinatorMemoryStorage.Length == 0)
            {
                _MyDictionaryWithRecordLikeContainers = default(Dictionary<Int32, RecordLikeContainer>);
            }
            else
            {
                LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _MyDictionaryWithRecordLikeContainers_ByteIndex, _MyDictionaryWithRecordLikeContainers_ByteLength, false, false, null);
                _MyDictionaryWithRecordLikeContainers = ConvertFromBytes_Dictionary_Gint_c_C32RecordLikeContainer_g(childData);
            }
            
            _MyDictionaryWithRecordLikeContainers_Accessed = true;
        }
        
        
        protected Dictionary<Int32, RecordLikeTypeWithLazinator> _MyDictionaryWithRecordLikeTypeValues;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public Dictionary<Int32, RecordLikeTypeWithLazinator> MyDictionaryWithRecordLikeTypeValues
        {
            get
            {
                if (!_MyDictionaryWithRecordLikeTypeValues_Accessed)
                {
                    Lazinate_MyDictionaryWithRecordLikeTypeValues();
                }
                IsDirty = true; 
                return _MyDictionaryWithRecordLikeTypeValues;
            }
            set
            {
                IsDirty = true;
                DescendantIsDirty = true;
                _MyDictionaryWithRecordLikeTypeValues = value;
                _MyDictionaryWithRecordLikeTypeValues_Accessed = true;
            }
        }
        protected bool _MyDictionaryWithRecordLikeTypeValues_Accessed;
        private void Lazinate_MyDictionaryWithRecordLikeTypeValues()
        {
            if (LazinatorMemoryStorage.Length == 0)
            {
                _MyDictionaryWithRecordLikeTypeValues = default(Dictionary<Int32, RecordLikeTypeWithLazinator>);
            }
            else
            {
                LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _MyDictionaryWithRecordLikeTypeValues_ByteIndex, _MyDictionaryWithRecordLikeTypeValues_ByteLength, false, false, null);
                _MyDictionaryWithRecordLikeTypeValues = ConvertFromBytes_Dictionary_Gint_c_C32RecordLikeTypeWithLazinator_g(childData);
            }
            
            _MyDictionaryWithRecordLikeTypeValues_Accessed = true;
        }
        
        
        /* Serialization, deserialization, and object relationships */
        
        public RecordLikeCollections(IncludeChildrenMode originalIncludeChildrenMode = IncludeChildrenMode.IncludeAllChildren)
        {
            OriginalIncludeChildrenMode = originalIncludeChildrenMode;
        }
        
        public RecordLikeCollections(LazinatorMemory serializedBytes, ILazinator parent = null)
        {
            LazinatorParents = new LazinatorParentsCollection(parent);
            DeserializeLazinator(serializedBytes);
            HasChanged = false;
            DescendantHasChanged = false;
        }
        
        public virtual LazinatorParentsCollection LazinatorParents { get; set; }
        
        public virtual IncludeChildrenMode OriginalIncludeChildrenMode { get; set; }
        
        public virtual int Deserialize()
        {
            FreeInMemoryObjects();
            int bytesSoFar = 0;
            ReadOnlySpan<byte> span = LazinatorMemoryStorage.InitialSpan;
            if (span.Length == 0)
            {
                return 0;
            }
            
            ReadGenericIDIfApplicable(ContainsOpenGenericParameters, LazinatorUniqueID, span, ref bytesSoFar);
            
            int lazinatorLibraryVersion = span.ToDecompressedInt(ref bytesSoFar);
            
            int serializedVersionNumber = span.ToDecompressedInt(ref bytesSoFar);
            
            OriginalIncludeChildrenMode = (IncludeChildrenMode)span.ToByte(ref bytesSoFar);
            
            ConvertFromBytesAfterHeader(OriginalIncludeChildrenMode, serializedVersionNumber, ref bytesSoFar);
            return bytesSoFar;
        }
        
        public virtual LazinatorMemory SerializeLazinator(IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer) 
        {
            if (LazinatorMemoryStorage.IsEmpty || includeChildrenMode != OriginalIncludeChildrenMode || (verifyCleanness || IsDirty || (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && DescendantIsDirty)))
            {
                return EncodeToNewBuffer(includeChildrenMode, verifyCleanness, updateStoredBuffer);
            }
            BinaryBufferWriter writer = new BinaryBufferWriter(LazinatorMemoryStorage.Length);
            LazinatorMemoryStorage.WriteToBinaryBuffer(ref writer);
            return writer.LazinatorMemory;
        }
        
        protected virtual LazinatorMemory EncodeToNewBuffer(IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer) 
        {
            int bufferSize = LazinatorMemoryStorage.Length == 0 ? ExpandableBytes.DefaultMinBufferSize : LazinatorMemoryStorage.Length;
            BinaryBufferWriter writer = new BinaryBufferWriter(bufferSize);
            SerializeExistingBuffer(ref writer, includeChildrenMode, verifyCleanness, updateStoredBuffer);
            return writer.LazinatorMemory;
        }
        
        public virtual ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode = IncludeChildrenMode.IncludeAllChildren, CloneBufferOptions cloneBufferOptions = CloneBufferOptions.IndependentBuffers)
        {
            RecordLikeCollections clone;
            if (cloneBufferOptions == CloneBufferOptions.NoBuffer)
            {
                clone = new RecordLikeCollections(includeChildrenMode);
                clone.LazinatorObjectVersion = LazinatorObjectVersion;
                clone = (RecordLikeCollections)AssignCloneProperties(clone, includeChildrenMode);
            }
            else
            {
                LazinatorMemory bytes = EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, false, IsDirty, DescendantIsDirty, false, LazinatorMemoryStorage, false, this);
                clone = new RecordLikeCollections(bytes);
            }
            return clone;
        }
        
        public virtual ILazinator AssignCloneProperties(ILazinator clone, IncludeChildrenMode includeChildrenMode)
        {
            clone.FreeInMemoryObjects();
            RecordLikeCollections typedClone = (RecordLikeCollections) clone;
            typedClone.MyInt = MyInt;
            typedClone.MyDictionaryWithRecordLikeContainers = CloneOrChange_Dictionary_Gint_c_C32RecordLikeContainer_g(MyDictionaryWithRecordLikeContainers, l => l?.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer), false);
            typedClone.MyDictionaryWithRecordLikeTypeValues = CloneOrChange_Dictionary_Gint_c_C32RecordLikeTypeWithLazinator_g(MyDictionaryWithRecordLikeTypeValues, l => l?.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer), false);
            
            return typedClone;
        }
        
        public virtual bool HasChanged { get; set; }
        
        protected bool _IsDirty;
        public virtual bool IsDirty
        {
            [DebuggerStepThrough]
            get => _IsDirty|| LazinatorMemoryStorage.Length == 0;
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
        
        public virtual void UpdateStoredBuffer()
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
        
        public virtual int GetByteLength()
        {
            UpdateStoredBuffer();
            return LazinatorMemoryStorage.Length;
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
            yield return ("MyInt", (object)MyInt);
            yield return ("MyDictionaryWithRecordLikeContainers", (object)MyDictionaryWithRecordLikeContainers);
            yield return ("MyDictionaryWithRecordLikeTypeValues", (object)MyDictionaryWithRecordLikeTypeValues);
            yield break;
        }
        
        public virtual ILazinator ForEachLazinator(Func<ILazinator, ILazinator> changeFunc, bool exploreOnlyDeserializedChildren, bool changeThisLevel)
        {
            if ((!exploreOnlyDeserializedChildren && MyDictionaryWithRecordLikeContainers != null) || (_MyDictionaryWithRecordLikeContainers_Accessed && _MyDictionaryWithRecordLikeContainers != null))
            {
                _MyDictionaryWithRecordLikeContainers = (Dictionary<Int32, RecordLikeContainer>) CloneOrChange_Dictionary_Gint_c_C32RecordLikeContainer_g(_MyDictionaryWithRecordLikeContainers, l => l?.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, true), true);
            }
            if ((!exploreOnlyDeserializedChildren && MyDictionaryWithRecordLikeTypeValues != null) || (_MyDictionaryWithRecordLikeTypeValues_Accessed && _MyDictionaryWithRecordLikeTypeValues != null))
            {
                _MyDictionaryWithRecordLikeTypeValues = (Dictionary<Int32, RecordLikeTypeWithLazinator>) CloneOrChange_Dictionary_Gint_c_C32RecordLikeTypeWithLazinator_g(_MyDictionaryWithRecordLikeTypeValues, l => l?.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, true), true);
            }
            if (changeThisLevel && changeFunc != null)
            {
                return changeFunc(this);
            }
            return this;
        }
        
        public virtual void FreeInMemoryObjects()
        {
            _MyDictionaryWithRecordLikeContainers = default;
            _MyDictionaryWithRecordLikeTypeValues = default;
            _MyDictionaryWithRecordLikeContainers_Accessed = _MyDictionaryWithRecordLikeTypeValues_Accessed = false;
            IsDirty = false;
            DescendantIsDirty = false;
            HasChanged = false;
            DescendantHasChanged = false;
        }
        
        /* Conversion */
        
        public virtual int LazinatorUniqueID => 1081;
        
        protected virtual bool ContainsOpenGenericParameters => false;
        public virtual LazinatorGenericIDType LazinatorGenericID => default;
        
        
        public virtual int LazinatorObjectVersion { get; set; } = 0;
        
        
        public virtual void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            ReadOnlySpan<byte> span = LazinatorMemoryStorage.InitialSpan;
            _MyInt = span.ToDecompressedInt(ref bytesSoFar);
            _MyDictionaryWithRecordLikeContainers_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt(ref bytesSoFar) + bytesSoFar;
            _MyDictionaryWithRecordLikeTypeValues_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt(ref bytesSoFar) + bytesSoFar;
            _RecordLikeCollections_EndByteIndex = bytesSoFar;
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
        
        protected virtual void UpdateDeserializedChildren(ref BinaryBufferWriter writer, int startPosition)
        {
            if (_MyDictionaryWithRecordLikeContainers_Accessed && _MyDictionaryWithRecordLikeContainers != null)
            {
                _MyDictionaryWithRecordLikeContainers = (Dictionary<Int32, RecordLikeContainer>) CloneOrChange_Dictionary_Gint_c_C32RecordLikeContainer_g(_MyDictionaryWithRecordLikeContainers, l => l.RemoveBufferInHierarchy(), true);
            }
            if (_MyDictionaryWithRecordLikeTypeValues_Accessed && _MyDictionaryWithRecordLikeTypeValues != null)
            {
                _MyDictionaryWithRecordLikeTypeValues = (Dictionary<Int32, RecordLikeTypeWithLazinator>) CloneOrChange_Dictionary_Gint_c_C32RecordLikeTypeWithLazinator_g(_MyDictionaryWithRecordLikeTypeValues, l => l.RemoveBufferInHierarchy(), true);
            }
        }
        
        
        protected virtual void WritePropertiesIntoBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer, bool includeUniqueID)
        {
            int startPosition = writer.Position;
            int startOfObjectPosition = 0;
            // header information
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
            CompressedIntegralTypes.WriteCompressedInt(ref writer, LazinatorObjectVersion);
            writer.Write((byte)includeChildrenMode);
            // write properties
            CompressedIntegralTypes.WriteCompressedInt(ref writer, _MyInt);
            startOfObjectPosition = writer.Position;
            if ((includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode) && !_MyDictionaryWithRecordLikeContainers_Accessed)
            {
                var deserialized = MyDictionaryWithRecordLikeContainers;
            }
            WriteNonLazinatorObject(
            nonLazinatorObject: _MyDictionaryWithRecordLikeContainers, isBelievedDirty: _MyDictionaryWithRecordLikeContainers_Accessed || (includeChildrenMode != OriginalIncludeChildrenMode),
            isAccessed: _MyDictionaryWithRecordLikeContainers_Accessed, writer: ref writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorMemoryStorage, _MyDictionaryWithRecordLikeContainers_ByteIndex, _MyDictionaryWithRecordLikeContainers_ByteLength, false, false, null),
            verifyCleanness: false,
            binaryWriterAction: (ref BinaryBufferWriter w, bool v) =>
            ConvertToBytes_Dictionary_Gint_c_C32RecordLikeContainer_g(ref w, _MyDictionaryWithRecordLikeContainers,
            includeChildrenMode, v, updateStoredBuffer));
            if (updateStoredBuffer)
            {
                _MyDictionaryWithRecordLikeContainers_ByteIndex = startOfObjectPosition - startPosition;if (_MyDictionaryWithRecordLikeContainers_Accessed && _MyDictionaryWithRecordLikeContainers != null)
                {
                    _MyDictionaryWithRecordLikeContainers = (Dictionary<Int32, RecordLikeContainer>) CloneOrChange_Dictionary_Gint_c_C32RecordLikeContainer_g(_MyDictionaryWithRecordLikeContainers, l => l.RemoveBufferInHierarchy(), true);
                }
                
            }
            startOfObjectPosition = writer.Position;
            if ((includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode) && !_MyDictionaryWithRecordLikeTypeValues_Accessed)
            {
                var deserialized = MyDictionaryWithRecordLikeTypeValues;
            }
            WriteNonLazinatorObject(
            nonLazinatorObject: _MyDictionaryWithRecordLikeTypeValues, isBelievedDirty: _MyDictionaryWithRecordLikeTypeValues_Accessed || (includeChildrenMode != OriginalIncludeChildrenMode),
            isAccessed: _MyDictionaryWithRecordLikeTypeValues_Accessed, writer: ref writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorMemoryStorage, _MyDictionaryWithRecordLikeTypeValues_ByteIndex, _MyDictionaryWithRecordLikeTypeValues_ByteLength, false, false, null),
            verifyCleanness: false,
            binaryWriterAction: (ref BinaryBufferWriter w, bool v) =>
            ConvertToBytes_Dictionary_Gint_c_C32RecordLikeTypeWithLazinator_g(ref w, _MyDictionaryWithRecordLikeTypeValues,
            includeChildrenMode, v, updateStoredBuffer));
            if (updateStoredBuffer)
            {
                _MyDictionaryWithRecordLikeTypeValues_ByteIndex = startOfObjectPosition - startPosition;if (_MyDictionaryWithRecordLikeTypeValues_Accessed && _MyDictionaryWithRecordLikeTypeValues != null)
                {
                    _MyDictionaryWithRecordLikeTypeValues = (Dictionary<Int32, RecordLikeTypeWithLazinator>) CloneOrChange_Dictionary_Gint_c_C32RecordLikeTypeWithLazinator_g(_MyDictionaryWithRecordLikeTypeValues, l => l.RemoveBufferInHierarchy(), true);
                }
                
            }
            if (updateStoredBuffer)
            {
                _RecordLikeCollections_EndByteIndex = writer.Position - startPosition;
            }
        }
        
        /* Conversion of supported collections and tuples */
        
        private static Dictionary<Int32, RecordLikeContainer> ConvertFromBytes_Dictionary_Gint_c_C32RecordLikeContainer_g(LazinatorMemory storage)
        {
            if (storage.Length == 0)
            {
                return default(Dictionary<Int32, RecordLikeContainer>);
            }
            ReadOnlySpan<byte> span = storage.InitialSpan;
            int bytesSoFar = 0;
            int collectionLength = span.ToDecompressedInt(ref bytesSoFar);
            
            Dictionary<Int32, RecordLikeContainer> collection = new Dictionary<Int32, RecordLikeContainer>(collectionLength);
            for (int itemIndex = 0; itemIndex < collectionLength; itemIndex++)
            {
                int lengthCollectionMember = span.ToInt(ref bytesSoFar);
                LazinatorMemory childData = storage.Slice(bytesSoFar, lengthCollectionMember);
                var item = ConvertFromBytes_KeyValuePair_Gint_c_C32RecordLikeContainer_g(childData);
                collection.Add(item.Key, item.Value);
                bytesSoFar += lengthCollectionMember;
            }
            
            return collection;
        }
        
        private static void ConvertToBytes_Dictionary_Gint_c_C32RecordLikeContainer_g(ref BinaryBufferWriter writer, Dictionary<Int32, RecordLikeContainer> itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            if (itemToConvert == default(Dictionary<Int32, RecordLikeContainer>))
            {
                return;
            }
            CompressedIntegralTypes.WriteCompressedInt(ref writer, itemToConvert.Count);
            foreach (var item in itemToConvert)
            {
                void action(ref BinaryBufferWriter w) => ConvertToBytes_KeyValuePair_Gint_c_C32RecordLikeContainer_g(ref w, item, includeChildrenMode, verifyCleanness, updateStoredBuffer);
                WriteToBinaryWithIntLengthPrefix(ref writer, action);
            }
        }
        
        private static Dictionary<Int32, RecordLikeContainer> CloneOrChange_Dictionary_Gint_c_C32RecordLikeContainer_g(Dictionary<Int32, RecordLikeContainer> itemToClone, Func<ILazinator, ILazinator> cloneOrChangeFunc, bool avoidCloningIfPossible)
        {
            if (itemToClone == null)
            {
                return default;
            }
            
            int collectionLength = itemToClone.Count;
            Dictionary<Int32, RecordLikeContainer> collection = new Dictionary<Int32, RecordLikeContainer>(collectionLength);
            foreach (var item in itemToClone)
            {
                var itemCopied = (KeyValuePair<Int32, RecordLikeContainer>) CloneOrChange_KeyValuePair_Gint_c_C32RecordLikeContainer_g(item, cloneOrChangeFunc, avoidCloningIfPossible);
                collection.Add(itemCopied.Key, itemCopied.Value);
            }
            return collection;
        }
        
        private static KeyValuePair<Int32, RecordLikeContainer> ConvertFromBytes_KeyValuePair_Gint_c_C32RecordLikeContainer_g(LazinatorMemory storage)
        {
            if (storage.Length == 0)
            {
                return default;
            }
            ReadOnlySpan<byte> span = storage.InitialReadOnlySpan;
            
            int bytesSoFar = 0;
            
            int item1 = span.ToDecompressedInt(ref bytesSoFar);
            
            RecordLikeContainer item2 = default(RecordLikeContainer);
            int lengthCollectionMember_item2 = span.ToInt(ref bytesSoFar);
            if (lengthCollectionMember_item2 != 0)
            {
                LazinatorMemory childData = storage.Slice(bytesSoFar, lengthCollectionMember_item2);
                item2 = DeserializationFactory.Instance.CreateBasedOnType<RecordLikeContainer>(childData);
            }
            bytesSoFar += lengthCollectionMember_item2;
            
            var itemToCreate = new KeyValuePair<Int32, RecordLikeContainer>(item1, item2);
            
            return itemToCreate;
        }
        
        private static void ConvertToBytes_KeyValuePair_Gint_c_C32RecordLikeContainer_g(ref BinaryBufferWriter writer, KeyValuePair<Int32, RecordLikeContainer> itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            
            CompressedIntegralTypes.WriteCompressedInt(ref writer, itemToConvert.Key);
            
            if (itemToConvert.Value == null)
            {
                writer.Write((uint)0);
            }
            else
            {
                void actionValue(ref BinaryBufferWriter w) => itemToConvert.Value.SerializeExistingBuffer(ref w, includeChildrenMode, verifyCleanness, updateStoredBuffer);
                WriteToBinaryWithIntLengthPrefix(ref writer, actionValue);
            };
        }
        
        private static KeyValuePair<Int32, RecordLikeContainer> CloneOrChange_KeyValuePair_Gint_c_C32RecordLikeContainer_g(KeyValuePair<Int32, RecordLikeContainer> itemToConvert, Func<ILazinator, ILazinator> cloneOrChangeFunc, bool avoidCloningIfPossible)
        {
            return new KeyValuePair<Int32, RecordLikeContainer>((int) (itemToConvert.Key), (RecordLikeContainer) (cloneOrChangeFunc((itemToConvert.Value))));
        }
        
        private static Dictionary<Int32, RecordLikeTypeWithLazinator> ConvertFromBytes_Dictionary_Gint_c_C32RecordLikeTypeWithLazinator_g(LazinatorMemory storage)
        {
            if (storage.Length == 0)
            {
                return default(Dictionary<Int32, RecordLikeTypeWithLazinator>);
            }
            ReadOnlySpan<byte> span = storage.InitialSpan;
            int bytesSoFar = 0;
            int collectionLength = span.ToDecompressedInt(ref bytesSoFar);
            
            Dictionary<Int32, RecordLikeTypeWithLazinator> collection = new Dictionary<Int32, RecordLikeTypeWithLazinator>(collectionLength);
            for (int itemIndex = 0; itemIndex < collectionLength; itemIndex++)
            {
                int lengthCollectionMember = span.ToInt(ref bytesSoFar);
                LazinatorMemory childData = storage.Slice(bytesSoFar, lengthCollectionMember);
                var item = ConvertFromBytes_KeyValuePair_Gint_c_C32RecordLikeTypeWithLazinator_g(childData);
                collection.Add(item.Key, item.Value);
                bytesSoFar += lengthCollectionMember;
            }
            
            return collection;
        }
        
        private static void ConvertToBytes_Dictionary_Gint_c_C32RecordLikeTypeWithLazinator_g(ref BinaryBufferWriter writer, Dictionary<Int32, RecordLikeTypeWithLazinator> itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            if (itemToConvert == default(Dictionary<Int32, RecordLikeTypeWithLazinator>))
            {
                return;
            }
            CompressedIntegralTypes.WriteCompressedInt(ref writer, itemToConvert.Count);
            foreach (var item in itemToConvert)
            {
                void action(ref BinaryBufferWriter w) => ConvertToBytes_KeyValuePair_Gint_c_C32RecordLikeTypeWithLazinator_g(ref w, item, includeChildrenMode, verifyCleanness, updateStoredBuffer);
                WriteToBinaryWithIntLengthPrefix(ref writer, action);
            }
        }
        
        private static Dictionary<Int32, RecordLikeTypeWithLazinator> CloneOrChange_Dictionary_Gint_c_C32RecordLikeTypeWithLazinator_g(Dictionary<Int32, RecordLikeTypeWithLazinator> itemToClone, Func<ILazinator, ILazinator> cloneOrChangeFunc, bool avoidCloningIfPossible)
        {
            if (itemToClone == null)
            {
                return default;
            }
            
            int collectionLength = itemToClone.Count;
            Dictionary<Int32, RecordLikeTypeWithLazinator> collection = new Dictionary<Int32, RecordLikeTypeWithLazinator>(collectionLength);
            foreach (var item in itemToClone)
            {
                var itemCopied = (KeyValuePair<Int32, RecordLikeTypeWithLazinator>) CloneOrChange_KeyValuePair_Gint_c_C32RecordLikeTypeWithLazinator_g(item, cloneOrChangeFunc, avoidCloningIfPossible);
                collection.Add(itemCopied.Key, itemCopied.Value);
            }
            return collection;
        }
        
        private static KeyValuePair<Int32, RecordLikeTypeWithLazinator> ConvertFromBytes_KeyValuePair_Gint_c_C32RecordLikeTypeWithLazinator_g(LazinatorMemory storage)
        {
            if (storage.Length == 0)
            {
                return default;
            }
            ReadOnlySpan<byte> span = storage.InitialReadOnlySpan;
            
            int bytesSoFar = 0;
            
            int item1 = span.ToDecompressedInt(ref bytesSoFar);
            
            RecordLikeTypeWithLazinator item2 = default(RecordLikeTypeWithLazinator);
            int lengthCollectionMember_item2 = span.ToInt(ref bytesSoFar);
            if (lengthCollectionMember_item2 != 0)
            {
                LazinatorMemory childData = storage.Slice(bytesSoFar, lengthCollectionMember_item2);
                item2 = ConvertFromBytes_RecordLikeTypeWithLazinator(childData);
            }
            bytesSoFar += lengthCollectionMember_item2;
            
            var itemToCreate = new KeyValuePair<Int32, RecordLikeTypeWithLazinator>(item1, item2);
            
            return itemToCreate;
        }
        
        private static void ConvertToBytes_KeyValuePair_Gint_c_C32RecordLikeTypeWithLazinator_g(ref BinaryBufferWriter writer, KeyValuePair<Int32, RecordLikeTypeWithLazinator> itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            
            CompressedIntegralTypes.WriteCompressedInt(ref writer, itemToConvert.Key);
            
            void actionValue(ref BinaryBufferWriter w) => ConvertToBytes_RecordLikeTypeWithLazinator(ref w, itemToConvert.Value, includeChildrenMode, verifyCleanness, updateStoredBuffer);
            WriteToBinaryWithIntLengthPrefix(ref writer, actionValue);
        }
        
        private static KeyValuePair<Int32, RecordLikeTypeWithLazinator> CloneOrChange_KeyValuePair_Gint_c_C32RecordLikeTypeWithLazinator_g(KeyValuePair<Int32, RecordLikeTypeWithLazinator> itemToConvert, Func<ILazinator, ILazinator> cloneOrChangeFunc, bool avoidCloningIfPossible)
        {
            return new KeyValuePair<Int32, RecordLikeTypeWithLazinator>((int) (itemToConvert.Key), (RecordLikeTypeWithLazinator) CloneOrChange_RecordLikeTypeWithLazinator((itemToConvert.Value), cloneOrChangeFunc, avoidCloningIfPossible));
        }
        
        private static RecordLikeTypeWithLazinator ConvertFromBytes_RecordLikeTypeWithLazinator(LazinatorMemory storage)
        {
            if (storage.Length == 0)
            {
                return default;
            }
            ReadOnlySpan<byte> span = storage.InitialReadOnlySpan;
            
            int bytesSoFar = 0;
            
            int item1 = span.ToDecompressedInt(ref bytesSoFar);
            
            string item2 = span.ToString_BrotliCompressedWithLength(ref bytesSoFar);
            
            Example item3 = default(Example);
            int lengthCollectionMember_item3 = span.ToInt(ref bytesSoFar);
            if (lengthCollectionMember_item3 != 0)
            {
                LazinatorMemory childData = storage.Slice(bytesSoFar, lengthCollectionMember_item3);
                item3 = DeserializationFactory.Instance.CreateBasedOnType<Example>(childData);
            }
            bytesSoFar += lengthCollectionMember_item3;
            
            ExampleStructWithoutClass item4 = default(ExampleStructWithoutClass);
            int lengthCollectionMember_item4 = span.ToInt(ref bytesSoFar);
            if (lengthCollectionMember_item4 != 0)
            {
                LazinatorMemory childData = storage.Slice(bytesSoFar, lengthCollectionMember_item4);
                item4 = new ExampleStructWithoutClass();
                item4.DeserializeLazinator(childData);;
            }
            bytesSoFar += lengthCollectionMember_item4;
            
            var itemToCreate = new RecordLikeTypeWithLazinator(item1, item2, item3, item4);
            
            return itemToCreate;
        }
        
        private static void ConvertToBytes_RecordLikeTypeWithLazinator(ref BinaryBufferWriter writer, RecordLikeTypeWithLazinator itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            
            CompressedIntegralTypes.WriteCompressedInt(ref writer, itemToConvert.Age);
            
            EncodeCharAndString.WriteBrotliCompressedWithIntPrefix(ref writer, itemToConvert.Name);
            
            if (itemToConvert.Example == null)
            {
                writer.Write((uint)0);
            }
            else
            {
                void actionExample(ref BinaryBufferWriter w) => itemToConvert.Example.SerializeExistingBuffer(ref w, includeChildrenMode, verifyCleanness, updateStoredBuffer);
                WriteToBinaryWithIntLengthPrefix(ref writer, actionExample);
            };
            
            void actionExampleStruct(ref BinaryBufferWriter w) => itemToConvert.ExampleStruct.SerializeExistingBuffer(ref w, includeChildrenMode, verifyCleanness, updateStoredBuffer);
            WriteToBinaryWithIntLengthPrefix(ref writer, actionExampleStruct);
        }
        
        private static RecordLikeTypeWithLazinator CloneOrChange_RecordLikeTypeWithLazinator(RecordLikeTypeWithLazinator itemToConvert, Func<ILazinator, ILazinator> cloneOrChangeFunc, bool avoidCloningIfPossible)
        {
            return new RecordLikeTypeWithLazinator((int) (itemToConvert.Age), (string) (itemToConvert.Name), (Example) (cloneOrChangeFunc((itemToConvert.Example))), (ExampleStructWithoutClass) (cloneOrChangeFunc((itemToConvert.ExampleStruct))));
        }
        
    }
}
