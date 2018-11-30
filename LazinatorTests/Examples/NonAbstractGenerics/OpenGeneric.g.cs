//d3b974e9-21e2-398a-a5ee-d47d4f520cc8
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.332
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LazinatorTests.Examples.NonAbstractGenerics
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
    public partial class OpenGeneric<T> : ILazinator
    {
        public bool IsStruct => false;
        
        /* Property definitions */
        
        protected int _MyListT_ByteIndex;
        protected int _MyT_ByteIndex;
        protected virtual int _MyListT_ByteLength => _MyT_ByteIndex - _MyListT_ByteIndex;
        private int _OpenGeneric_T_EndByteIndex = 0;
        protected virtual int _MyT_ByteLength => _OpenGeneric_T_EndByteIndex - _MyT_ByteIndex;
        
        
        protected List<T> _MyListT;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public virtual List<T> MyListT
        {
            get
            {
                if (!_MyListT_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _MyListT = default(List<T>);
                    }
                    else
                    {
                        LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _MyListT_ByteIndex, _MyListT_ByteLength, false, false, null);
                        _MyListT = ConvertFromBytes_List_GT_g(childData);
                    }
                    _MyListT_Accessed = true;
                }
                IsDirty = true; 
                return _MyListT;
            }
            set
            {
                IsDirty = true;
                DescendantIsDirty = true;
                _MyListT = value;
                _MyListT_Accessed = true;
            }
        }
        protected bool _MyListT_Accessed;
        
        protected T _MyT;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public virtual T MyT
        {
            get
            {
                if (!_MyT_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _MyT = default(T);
                        if (_MyT != null)
                        { // MyT is a struct
                            _MyT.LazinatorParents = new LazinatorParentsCollection(this);
                        }
                    }
                    else
                    {
                        LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _MyT_ByteIndex, _MyT_ByteLength, false, false, null);
                        
                        _MyT = DeserializationFactory.Instance.CreateBasedOnType<T>(childData, this); 
                    }
                    _MyT_Accessed = true;
                } 
                return _MyT;
            }
            set
            {
                if (value != null && value.IsStruct)
                {
                    value.LazinatorParents = new LazinatorParentsCollection(this);
                }
                else
                {
                    if (_MyT != null)
                    {
                        _MyT.LazinatorParents = _MyT.LazinatorParents.WithRemoved(this);
                    }
                    if (value != null)
                    {
                        value.LazinatorParents = value.LazinatorParents.WithAdded(this);
                    }
                }
                
                IsDirty = true;
                DescendantIsDirty = true;
                _MyT = value;
                _MyT_Accessed = true;
            }
        }
        protected bool _MyT_Accessed;
        
        /* Serialization, deserialization, and object relationships */
        
        public OpenGeneric() : base()
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
            var clone = new OpenGeneric<T>()
            {
                OriginalIncludeChildrenMode = includeChildrenMode
            };
            clone = CompleteClone(this, clone, includeChildrenMode, cloneBufferOptions);
            return clone;
        }
        
        public virtual ILazinator AssignCloneProperties(ILazinator clone, IncludeChildrenMode includeChildrenMode)
        {
            clone.FreeInMemoryObjects();
            OpenGeneric<T> typedClone = (OpenGeneric<T>) clone;
            typedClone.MyListT = CloneOrChange_List_GT_g(MyListT, l => l?.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer), false);
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                if (MyT == null)
                {
                    typedClone.MyT = default(T);
                }
                else
                {
                    typedClone.MyT = (T) MyT.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer);
                }
            }
            
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
            if (enumerateNulls && (!exploreOnlyDeserializedChildren || _MyT_Accessed) && MyT == null)
            {
                yield return ("MyT", default);
            }
            else
            {
                if ((!exploreOnlyDeserializedChildren && MyT != null) || (_MyT_Accessed && _MyT != null))
                {
                    bool isMatch = matchCriterion == null || matchCriterion(MyT);
                    bool shouldExplore = exploreCriterion == null || exploreCriterion(MyT);
                    if (isMatch)
                    {
                        yield return ("MyT", MyT);
                    }
                    if ((!stopExploringBelowMatch || !isMatch) && shouldExplore)
                    {
                        foreach (var toYield in MyT.EnumerateLazinatorDescendants(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
                        {
                            yield return ("MyT" + "." + toYield.propertyName, toYield.descendant);
                        }
                    }
                }
            }
            yield break;
        }
        
        
        public virtual IEnumerable<(string propertyName, object descendant)> EnumerateNonLazinatorProperties()
        {
            yield return ("MyListT", (object)MyListT);
            yield break;
        }
        
        public virtual ILazinator ForEachLazinator(Func<ILazinator, ILazinator> changeFunc, bool exploreOnlyDeserializedChildren)
        {
            if ((!exploreOnlyDeserializedChildren && MyT != null) || (_MyT_Accessed && _MyT != null))
            {
                _MyT = (T) _MyT.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren);
            }
            if ((!exploreOnlyDeserializedChildren && MyListT != null) || (_MyListT_Accessed && _MyListT != null))
            {
                _MyListT = (List<T>) CloneOrChange_List_GT_g(_MyListT, l => l?.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren), true);
            }
            return changeFunc(this);
        }
        
        public virtual void FreeInMemoryObjects()
        {
            _MyListT = default;
            _MyT = default;
            _MyListT_Accessed = _MyT_Accessed = false;
            IsDirty = false;
            DescendantIsDirty = false;
            HasChanged = false;
            DescendantHasChanged = false;
        }
        
        /* Conversion */
        
        public virtual int LazinatorUniqueID => 233;
        
        protected virtual bool ContainsOpenGenericParameters => true;
        protected virtual LazinatorGenericIDType _LazinatorGenericID { get; set; }
        public virtual LazinatorGenericIDType LazinatorGenericID
        {
            get
            {
                if (_LazinatorGenericID.IsEmpty)
                {
                    _LazinatorGenericID = DeserializationFactory.Instance.GetUniqueIDListForGenericType(233, new Type[] { typeof(T) });
                }
                return _LazinatorGenericID;
            }
            set
            {
                _LazinatorGenericID = value;
            }
        }
        
        public virtual int LazinatorObjectVersion { get; set; } = 0;
        
        
        public virtual void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
            _MyListT_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            _MyT_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            _OpenGeneric_T_EndByteIndex = bytesSoFar;
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
                
                if (_MyT_Accessed && _MyT != null && _MyT.IsStruct && (_MyT.IsDirty || _MyT.DescendantIsDirty))
                {
                    _MyT_Accessed = false;
                }
            }
            else
            {
                throw new LazinatorSerializationException("Cannot update stored buffer when serializing only some children.");
            }
            
            var newBuffer = writer.Slice(startPosition, length);
            LazinatorMemoryStorage = newBuffer;
        }
        
        protected virtual void UpdateDeserializedChildren(ref BinaryBufferWriter writer, int startPosition)
        {
            if (_MyT_Accessed && _MyT != null)
            {
                _MyT.UpdateStoredBuffer(ref writer, startPosition + _MyT_ByteIndex + sizeof(int), _MyT_ByteLength - sizeof(int), IncludeChildrenMode.IncludeAllChildren, true);
            }
            if (_MyListT_Accessed && _MyListT != null)
            {
                _MyListT = (List<T>) CloneOrChange_List_GT_g(_MyListT, l => l.RemoveBufferInHierarchy(), true);
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
            if ((includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode) && !_MyListT_Accessed)
            {
                var deserialized = MyListT;
            }
            WriteNonLazinatorObject(
            nonLazinatorObject: _MyListT, isBelievedDirty: _MyListT_Accessed || (includeChildrenMode != OriginalIncludeChildrenMode),
            isAccessed: _MyListT_Accessed, writer: ref writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorMemoryStorage, _MyListT_ByteIndex, _MyListT_ByteLength, false, false, null),
            verifyCleanness: false,
            binaryWriterAction: (ref BinaryBufferWriter w, bool v) =>
            ConvertToBytes_List_GT_g(ref w, _MyListT,
            includeChildrenMode, v, updateStoredBuffer));
            if (updateStoredBuffer)
            {
                _MyListT_ByteIndex = startOfObjectPosition - startPosition;
            }
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                if ((includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode) && !_MyT_Accessed)
                {
                    var deserialized = MyT;
                }
                WriteChild(ref writer, ref _MyT, includeChildrenMode, _MyT_Accessed, () => GetChildSlice(LazinatorMemoryStorage, _MyT_ByteIndex, _MyT_ByteLength, false, false, null), verifyCleanness, updateStoredBuffer, false, false, this);
            }
            if (updateStoredBuffer)
            {
                _MyT_ByteIndex = startOfObjectPosition - startPosition;
            }
            if (updateStoredBuffer)
            {
                _OpenGeneric_T_EndByteIndex = writer.Position - startPosition;
            }
        }
        
        /* Conversion of supported collections and tuples */
        
        private static List<T> ConvertFromBytes_List_GT_g(LazinatorMemory storage)
        {
            if (storage.Length == 0)
            {
                return default(List<T>);
            }
            ReadOnlySpan<byte> span = storage.Span;
            
            int bytesSoFar = 0;
            int collectionLength = span.ToDecompressedInt(ref bytesSoFar);
            
            List<T> collection = new List<T>(collectionLength);
            for (int itemIndex = 0; itemIndex < collectionLength; itemIndex++)
            {
                int lengthCollectionMember = span.ToInt32(ref bytesSoFar);
                if (lengthCollectionMember == 0)
                {
                    collection.Add(default(T));
                }
                else
                {
                    LazinatorMemory childData = storage.Slice(bytesSoFar, lengthCollectionMember);
                    var item = DeserializationFactory.Instance.CreateBasedOnType<T>(childData);
                    collection.Add(item);
                }
                bytesSoFar += lengthCollectionMember;
            }
            
            return collection;
        }
        
        private static void ConvertToBytes_List_GT_g(ref BinaryBufferWriter writer, List<T> itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            if (itemToConvert == default(List<T>))
            {
                return;
            }
            CompressedIntegralTypes.WriteCompressedInt(ref writer, itemToConvert.Count);
            int itemToConvertCount = itemToConvert.Count;
            for (int itemIndex = 0; itemIndex < itemToConvertCount; itemIndex++)
            {
                if (System.Collections.Generic.EqualityComparer<T>.Default.Equals(itemToConvert[itemIndex], default(T)))
                {
                    writer.Write((uint)0);
                }
                else 
                {
                    
                    void action(ref BinaryBufferWriter w) 
                    {
                        var copy = itemToConvert[itemIndex];
                        copy.SerializeExistingBuffer(ref w, includeChildrenMode, verifyCleanness, updateStoredBuffer);
                        itemToConvert[itemIndex] = copy;
                    }
                    WriteToBinaryWithIntLengthPrefix(ref writer, action);
                }
                
            }
        }
        
        private static List<T> CloneOrChange_List_GT_g(List<T> itemToClone, Func<ILazinator, ILazinator> cloneOrChangeFunc, bool avoidCloningIfPossible)
        {
            if (itemToClone == null)
            {
                return default;
            }
            int collectionLength = itemToClone.Count;
            List<T> collection = avoidCloningIfPossible ? itemToClone : new List<T>(collectionLength);
            int itemToCloneCount = itemToClone.Count;
            for (int itemIndex = 0; itemIndex < itemToCloneCount; itemIndex++)
            {
                if (avoidCloningIfPossible)
                {
                    if (itemToClone[itemIndex] != null)
                    {
                        itemToClone[itemIndex] = (T) cloneOrChangeFunc(itemToClone[itemIndex]);
                    }
                    continue;
                }
                if (itemToClone[itemIndex] == null)
                {
                    collection.Add(default(T));
                }
                else
                {
                    var itemCopied = (T) cloneOrChangeFunc(itemToClone[itemIndex]);
                    collection.Add(itemCopied);
                }
            }
            return collection;
        }
        
    }
}
