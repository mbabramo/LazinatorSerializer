//92689e78-6937-9056-8fec-04663ecfd8f5
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.220
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
    public partial class ArrayMultidimensional_Values : ILazinator
    {
        public bool IsStruct => false;
        
        /* Serialization, deserialization, and object relationships */
        
        public virtual LazinatorParentsCollection LazinatorParents { get; set; }
        
        protected IncludeChildrenMode OriginalIncludeChildrenMode;
        
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
        
        public virtual LazinatorMemory SerializeLazinator(IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            return EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, verifyCleanness, IsDirty, DescendantIsDirty, false, LazinatorMemoryStorage, (EncodeManuallyDelegate) EncodeToNewBuffer, updateStoredBuffer);
        }
        
        protected virtual LazinatorMemory EncodeToNewBuffer(IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer) => LazinatorUtilities.EncodeToNewBinaryBufferWriter(this, includeChildrenMode, verifyCleanness, updateStoredBuffer);
        
        public virtual ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode = IncludeChildrenMode.IncludeAllChildren, bool updateStoredBuffer = false, bool disposeCloneIndependently = false)
        {
            LazinatorMemory bytes = EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, false, IsDirty, DescendantIsDirty, false, LazinatorMemoryStorage, (EncodeManuallyDelegate)EncodeToNewBuffer, updateStoredBuffer);
            var clone = new ArrayMultidimensional_Values()
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
        
        public virtual bool HasChanged { get; set; }
        
        protected bool _IsDirty;
        public virtual bool IsDirty
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
        }
        
        protected LazinatorMemory _LazinatorMemoryStorage;
        public virtual LazinatorMemory LazinatorMemoryStorage
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
        protected virtual ReadOnlyMemory<byte> LazinatorObjectBytes => LazinatorMemoryStorage?.Memory ?? LazinatorUtilities.EmptyReadOnlyMemory;
        
        public virtual void EnsureLazinatorMemoryUpToDate()
        {
            
            if (!IsDirty && !DescendantIsDirty && LazinatorObjectBytes.Length > 0)
            {
                return;
            }
            EncodeOrRecycleToNewBuffer(IncludeChildrenMode.IncludeAllChildren, OriginalIncludeChildrenMode, false, IsDirty, DescendantIsDirty, false, LazinatorMemoryStorage, (EncodeManuallyDelegate)EncodeToNewBuffer, true);
        }
        
        public virtual int GetByteLength()
        {
            EnsureLazinatorMemoryUpToDate();
            return LazinatorObjectBytes.Length;
        }
        
        public virtual uint GetBinaryHashCode32()
        {
            EnsureLazinatorMemoryUpToDate();
            return FarmhashByteSpans.Hash32(LazinatorObjectBytes.Span);
        }
        
        public virtual ulong GetBinaryHashCode64()
        {
            EnsureLazinatorMemoryUpToDate();
            return FarmhashByteSpans.Hash64(LazinatorObjectBytes.Span);
        }
        
        public virtual Guid GetBinaryHashCode128()
        {
            EnsureLazinatorMemoryUpToDate();
            return FarmhashByteSpans.Hash128(LazinatorObjectBytes.Span);
        }
        
        /* Property definitions */
        
        protected int _MyArrayInt_ByteIndex;
        protected int _MyCrazyJaggedArray_ByteIndex;
        protected int _MyThreeDimArrayInt_ByteIndex;
        protected virtual int _MyArrayInt_ByteLength => _MyCrazyJaggedArray_ByteIndex - _MyArrayInt_ByteIndex;
        protected virtual int _MyCrazyJaggedArray_ByteLength => _MyThreeDimArrayInt_ByteIndex - _MyCrazyJaggedArray_ByteIndex;
        private int _ArrayMultidimensional_Values_EndByteIndex;
        protected virtual int _MyThreeDimArrayInt_ByteLength => _ArrayMultidimensional_Values_EndByteIndex - _MyThreeDimArrayInt_ByteIndex;
        
        protected int[,] _MyArrayInt;
        public int[,] MyArrayInt
        {
            get
            {
                if (!_MyArrayInt_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _MyArrayInt = default(int[,]);
                        _MyArrayInt_Dirty = true; 
                    }
                    else
                    {
                        LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _MyArrayInt_ByteIndex, _MyArrayInt_ByteLength, false, false, null);
                        _MyArrayInt = ConvertFromBytes_int_B_c_b(childData);
                    }
                    _MyArrayInt_Accessed = true;
                } 
                return _MyArrayInt;
            }
            set
            {
                IsDirty = true;
                DescendantIsDirty = true;
                _MyArrayInt = value;
                _MyArrayInt_Dirty = true;
                _MyArrayInt_Accessed = true;
            }
        }
        protected bool _MyArrayInt_Accessed;
        
        private bool _MyArrayInt_Dirty;
        public bool MyArrayInt_Dirty
        {
            get => _MyArrayInt_Dirty;
            set
            {
                if (_MyArrayInt_Dirty != value)
                {
                    _MyArrayInt_Dirty = value;
                    if (value && !IsDirty)
                    {
                        IsDirty = true;
                    }
                }
            }
        }
        protected int[][,,][,,,] _MyCrazyJaggedArray;
        public int[][,,][,,,] MyCrazyJaggedArray
        {
            get
            {
                if (!_MyCrazyJaggedArray_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _MyCrazyJaggedArray = default(int[][,,][,,,]);
                    }
                    else
                    {
                        LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _MyCrazyJaggedArray_ByteIndex, _MyCrazyJaggedArray_ByteLength, false, false, null);
                        _MyCrazyJaggedArray = ConvertFromBytes_int_B_b_B_c_c_b_B_c_c_c_b(childData);
                    }
                    _MyCrazyJaggedArray_Accessed = true;
                }
                IsDirty = true; 
                return _MyCrazyJaggedArray;
            }
            set
            {
                IsDirty = true;
                DescendantIsDirty = true;
                _MyCrazyJaggedArray = value;
                _MyCrazyJaggedArray_Accessed = true;
            }
        }
        protected bool _MyCrazyJaggedArray_Accessed;
        protected int[,,] _MyThreeDimArrayInt;
        public int[,,] MyThreeDimArrayInt
        {
            get
            {
                if (!_MyThreeDimArrayInt_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _MyThreeDimArrayInt = default(int[,,]);
                    }
                    else
                    {
                        LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _MyThreeDimArrayInt_ByteIndex, _MyThreeDimArrayInt_ByteLength, false, false, null);
                        _MyThreeDimArrayInt = ConvertFromBytes_int_B_c_c_b(childData);
                    }
                    _MyThreeDimArrayInt_Accessed = true;
                }
                IsDirty = true; 
                return _MyThreeDimArrayInt;
            }
            set
            {
                IsDirty = true;
                DescendantIsDirty = true;
                _MyThreeDimArrayInt = value;
                _MyThreeDimArrayInt_Accessed = true;
            }
        }
        protected bool _MyThreeDimArrayInt_Accessed;
        
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
            yield return ("MyArrayInt", (object)MyArrayInt);
            yield return ("MyCrazyJaggedArray", (object)MyCrazyJaggedArray);
            yield return ("MyThreeDimArrayInt", (object)MyThreeDimArrayInt);
            yield break;
        }
        
        public virtual void FreeInMemoryObjects()
        {
            _MyArrayInt = default;
            _MyCrazyJaggedArray = default;
            _MyThreeDimArrayInt = default;
            _MyArrayInt_Accessed = _MyCrazyJaggedArray_Accessed = _MyThreeDimArrayInt_Accessed = false;
            IsDirty = false;
            DescendantIsDirty = false;
            HasChanged = false;
            DescendantHasChanged = false;
        }
        
        /* Conversion */
        
        public virtual int LazinatorUniqueID => 200;
        
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
            _MyArrayInt_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            _MyCrazyJaggedArray_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            _MyThreeDimArrayInt_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            _ArrayMultidimensional_Values_EndByteIndex = bytesSoFar;
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
                
                var newBuffer = writer.Slice(startPosition);
                if (_LazinatorMemoryStorage != null)
                {
                    _LazinatorMemoryStorage.DisposeWithThis(newBuffer);
                }
                _LazinatorMemoryStorage = newBuffer;
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
            if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren && !_MyArrayInt_Accessed)
            {
                var deserialized = MyArrayInt;
            }
            WriteNonLazinatorObject(
            nonLazinatorObject: _MyArrayInt, isBelievedDirty: MyArrayInt_Dirty,
            isAccessed: _MyArrayInt_Accessed, writer: ref writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorMemoryStorage, _MyArrayInt_ByteIndex, _MyArrayInt_ByteLength, false, false, null),
            verifyCleanness: verifyCleanness,
            binaryWriterAction: (ref BinaryBufferWriter w, bool v) =>
            ConvertToBytes_int_B_c_b(ref w, _MyArrayInt,
            includeChildrenMode, v, updateStoredBuffer));
            if (updateStoredBuffer)
            {
                _MyArrayInt_ByteIndex = startOfObjectPosition - startPosition;
            }
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren && !_MyCrazyJaggedArray_Accessed)
            {
                var deserialized = MyCrazyJaggedArray;
            }
            WriteNonLazinatorObject(
            nonLazinatorObject: _MyCrazyJaggedArray, isBelievedDirty: _MyCrazyJaggedArray_Accessed,
            isAccessed: _MyCrazyJaggedArray_Accessed, writer: ref writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorMemoryStorage, _MyCrazyJaggedArray_ByteIndex, _MyCrazyJaggedArray_ByteLength, false, false, null),
            verifyCleanness: false,
            binaryWriterAction: (ref BinaryBufferWriter w, bool v) =>
            ConvertToBytes_int_B_b_B_c_c_b_B_c_c_c_b(ref w, _MyCrazyJaggedArray,
            includeChildrenMode, v, updateStoredBuffer));
            if (updateStoredBuffer)
            {
                _MyCrazyJaggedArray_ByteIndex = startOfObjectPosition - startPosition;
            }
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren && !_MyThreeDimArrayInt_Accessed)
            {
                var deserialized = MyThreeDimArrayInt;
            }
            WriteNonLazinatorObject(
            nonLazinatorObject: _MyThreeDimArrayInt, isBelievedDirty: _MyThreeDimArrayInt_Accessed,
            isAccessed: _MyThreeDimArrayInt_Accessed, writer: ref writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorMemoryStorage, _MyThreeDimArrayInt_ByteIndex, _MyThreeDimArrayInt_ByteLength, false, false, null),
            verifyCleanness: false,
            binaryWriterAction: (ref BinaryBufferWriter w, bool v) =>
            ConvertToBytes_int_B_c_c_b(ref w, _MyThreeDimArrayInt,
            includeChildrenMode, v, updateStoredBuffer));
            if (updateStoredBuffer)
            {
                _MyThreeDimArrayInt_ByteIndex = startOfObjectPosition - startPosition;
            }
            if (updateStoredBuffer)
            {
                _ArrayMultidimensional_Values_EndByteIndex = writer.Position - startPosition;
            }
        }
        
        /* Conversion of supported collections and tuples */
        
        private static int[,] ConvertFromBytes_int_B_c_b(LazinatorMemory storage)
        {
            if (storage.Length == 0)
            {
                return default(int[,]);
            }
            ReadOnlySpan<byte> span = storage.Span;
            
            int bytesSoFar = 0;
            int collectionLength0 = span.ToDecompressedInt(ref bytesSoFar);
            int collectionLength1 = span.ToDecompressedInt(ref bytesSoFar);
            
            int[,] collection = new int[collectionLength0, collectionLength1];
            for (int i0 = 0; i0 < collectionLength0; i0++)
            for (int i1 = 0; i1 < collectionLength1; i1++)
            {
                int item = span.ToDecompressedInt(ref bytesSoFar);
                collection[i0, i1] = item;
            }
            
            return collection;
        }
        
        private static void ConvertToBytes_int_B_c_b(ref BinaryBufferWriter writer, int[,] itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            if (itemToConvert == default(int[,]))
            {
                return;
            }
            CompressedIntegralTypes.WriteCompressedInt(ref writer, itemToConvert.GetLength(0));
            CompressedIntegralTypes.WriteCompressedInt(ref writer, itemToConvert.GetLength(1));
            int length0 = itemToConvert.GetLength(0);
            int length1 = itemToConvert.GetLength(1);
            for (int itemIndex0 = 0; itemIndex0 < length0; itemIndex0++)
            for (int itemIndex1 = 0; itemIndex1 < length1; itemIndex1++)
            {
                CompressedIntegralTypes.WriteCompressedInt(ref writer, itemToConvert[itemIndex0, itemIndex1]);
            }
        }
        
        private static int[][,,][,,,] ConvertFromBytes_int_B_b_B_c_c_b_B_c_c_c_b(LazinatorMemory storage)
        {
            if (storage.Length == 0)
            {
                return default(int[][,,][,,,]);
            }
            ReadOnlySpan<byte> span = storage.Span;
            
            int bytesSoFar = 0;
            int collectionLength = span.ToDecompressedInt(ref bytesSoFar);
            
            int[][,,][,,,] collection = new int[collectionLength][,,][,,,];
            for (int i = 0; i < collectionLength; i++)
            {
                int lengthCollectionMember = span.ToInt32(ref bytesSoFar);
                if (lengthCollectionMember == 0)
                {
                    collection[i] = default(int[,,][,,,]);
                }
                else
                {
                    LazinatorMemory childData = storage.Slice(bytesSoFar, lengthCollectionMember);
                    var item = ConvertFromBytes_int_B_c_c_b_B_c_c_c_b(childData);
                    collection[i] = item;
                }
                bytesSoFar += lengthCollectionMember;
            }
            
            return collection;
        }
        
        private static void ConvertToBytes_int_B_b_B_c_c_b_B_c_c_c_b(ref BinaryBufferWriter writer, int[][,,][,,,] itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            if (itemToConvert == default(int[][,,][,,,]))
            {
                return;
            }
            CompressedIntegralTypes.WriteCompressedInt(ref writer, itemToConvert.Length);
            int itemToConvertCount = itemToConvert.Length;
            for (int itemIndex = 0; itemIndex < itemToConvertCount; itemIndex++)
            {
                if (itemToConvert[itemIndex] == default(int[,,][,,,]))
                {
                    writer.Write((uint)0);
                }
                else 
                {
                    
                    void action(ref BinaryBufferWriter w) => ConvertToBytes_int_B_c_c_b_B_c_c_c_b(ref w, itemToConvert[itemIndex], includeChildrenMode, verifyCleanness, updateStoredBuffer);
                    WriteToBinaryWithIntLengthPrefix(ref writer, action);
                }
                
            }
        }
        
        private static int[,,][,,,] ConvertFromBytes_int_B_c_c_b_B_c_c_c_b(LazinatorMemory storage)
        {
            if (storage.Length == 0)
            {
                return default(int[,,][,,,]);
            }
            ReadOnlySpan<byte> span = storage.Span;
            
            int bytesSoFar = 0;
            int collectionLength0 = span.ToDecompressedInt(ref bytesSoFar);
            int collectionLength1 = span.ToDecompressedInt(ref bytesSoFar);
            int collectionLength2 = span.ToDecompressedInt(ref bytesSoFar);
            
            int[,,][,,,] collection = new int[collectionLength0, collectionLength1, collectionLength2][,,,];
            for (int i0 = 0; i0 < collectionLength0; i0++)
            for (int i1 = 0; i1 < collectionLength1; i1++)
            for (int i2 = 0; i2 < collectionLength2; i2++)
            {
                int lengthCollectionMember = span.ToInt32(ref bytesSoFar);
                if (lengthCollectionMember == 0)
                {
                    collection[i0, i1, i2] = default(int[,,,]);
                }
                else
                {
                    LazinatorMemory childData = storage.Slice(bytesSoFar, lengthCollectionMember);
                    var item = ConvertFromBytes_int_B_c_c_c_b(childData);
                    collection[i0, i1, i2] = item;
                }
                bytesSoFar += lengthCollectionMember;
            }
            
            return collection;
        }
        
        private static void ConvertToBytes_int_B_c_c_b_B_c_c_c_b(ref BinaryBufferWriter writer, int[,,][,,,] itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            if (itemToConvert == default(int[,,][,,,]))
            {
                return;
            }
            CompressedIntegralTypes.WriteCompressedInt(ref writer, itemToConvert.GetLength(0));
            CompressedIntegralTypes.WriteCompressedInt(ref writer, itemToConvert.GetLength(1));
            CompressedIntegralTypes.WriteCompressedInt(ref writer, itemToConvert.GetLength(2));
            int length0 = itemToConvert.GetLength(0);
            int length1 = itemToConvert.GetLength(1);
            int length2 = itemToConvert.GetLength(2);
            for (int itemIndex0 = 0; itemIndex0 < length0; itemIndex0++)
            for (int itemIndex1 = 0; itemIndex1 < length1; itemIndex1++)
            for (int itemIndex2 = 0; itemIndex2 < length2; itemIndex2++)
            {
                if (itemToConvert[itemIndex0, itemIndex1, itemIndex2] == default(int[,,,]))
                {
                    writer.Write((uint)0);
                }
                else 
                {
                    
                    void action(ref BinaryBufferWriter w) => ConvertToBytes_int_B_c_c_c_b(ref w, itemToConvert[itemIndex0, itemIndex1, itemIndex2], includeChildrenMode, verifyCleanness, updateStoredBuffer);
                    WriteToBinaryWithIntLengthPrefix(ref writer, action);
                }
                
            }
        }
        
        private static int[,,,] ConvertFromBytes_int_B_c_c_c_b(LazinatorMemory storage)
        {
            if (storage.Length == 0)
            {
                return default(int[,,,]);
            }
            ReadOnlySpan<byte> span = storage.Span;
            
            int bytesSoFar = 0;
            int collectionLength0 = span.ToDecompressedInt(ref bytesSoFar);
            int collectionLength1 = span.ToDecompressedInt(ref bytesSoFar);
            int collectionLength2 = span.ToDecompressedInt(ref bytesSoFar);
            int collectionLength3 = span.ToDecompressedInt(ref bytesSoFar);
            
            int[,,,] collection = new int[collectionLength0, collectionLength1, collectionLength2, collectionLength3];
            for (int i0 = 0; i0 < collectionLength0; i0++)
            for (int i1 = 0; i1 < collectionLength1; i1++)
            for (int i2 = 0; i2 < collectionLength2; i2++)
            for (int i3 = 0; i3 < collectionLength3; i3++)
            {
                int item = span.ToDecompressedInt(ref bytesSoFar);
                collection[i0, i1, i2, i3] = item;
            }
            
            return collection;
        }
        
        private static void ConvertToBytes_int_B_c_c_c_b(ref BinaryBufferWriter writer, int[,,,] itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            if (itemToConvert == default(int[,,,]))
            {
                return;
            }
            CompressedIntegralTypes.WriteCompressedInt(ref writer, itemToConvert.GetLength(0));
            CompressedIntegralTypes.WriteCompressedInt(ref writer, itemToConvert.GetLength(1));
            CompressedIntegralTypes.WriteCompressedInt(ref writer, itemToConvert.GetLength(2));
            CompressedIntegralTypes.WriteCompressedInt(ref writer, itemToConvert.GetLength(3));
            int length0 = itemToConvert.GetLength(0);
            int length1 = itemToConvert.GetLength(1);
            int length2 = itemToConvert.GetLength(2);
            int length3 = itemToConvert.GetLength(3);
            for (int itemIndex0 = 0; itemIndex0 < length0; itemIndex0++)
            for (int itemIndex1 = 0; itemIndex1 < length1; itemIndex1++)
            for (int itemIndex2 = 0; itemIndex2 < length2; itemIndex2++)
            for (int itemIndex3 = 0; itemIndex3 < length3; itemIndex3++)
            {
                CompressedIntegralTypes.WriteCompressedInt(ref writer, itemToConvert[itemIndex0, itemIndex1, itemIndex2, itemIndex3]);
            }
        }
        
        private static int[,,] ConvertFromBytes_int_B_c_c_b(LazinatorMemory storage)
        {
            if (storage.Length == 0)
            {
                return default(int[,,]);
            }
            ReadOnlySpan<byte> span = storage.Span;
            
            int bytesSoFar = 0;
            int collectionLength0 = span.ToDecompressedInt(ref bytesSoFar);
            int collectionLength1 = span.ToDecompressedInt(ref bytesSoFar);
            int collectionLength2 = span.ToDecompressedInt(ref bytesSoFar);
            
            int[,,] collection = new int[collectionLength0, collectionLength1, collectionLength2];
            for (int i0 = 0; i0 < collectionLength0; i0++)
            for (int i1 = 0; i1 < collectionLength1; i1++)
            for (int i2 = 0; i2 < collectionLength2; i2++)
            {
                int item = span.ToDecompressedInt(ref bytesSoFar);
                collection[i0, i1, i2] = item;
            }
            
            return collection;
        }
        
        private static void ConvertToBytes_int_B_c_c_b(ref BinaryBufferWriter writer, int[,,] itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            if (itemToConvert == default(int[,,]))
            {
                return;
            }
            CompressedIntegralTypes.WriteCompressedInt(ref writer, itemToConvert.GetLength(0));
            CompressedIntegralTypes.WriteCompressedInt(ref writer, itemToConvert.GetLength(1));
            CompressedIntegralTypes.WriteCompressedInt(ref writer, itemToConvert.GetLength(2));
            int length0 = itemToConvert.GetLength(0);
            int length1 = itemToConvert.GetLength(1);
            int length2 = itemToConvert.GetLength(2);
            for (int itemIndex0 = 0; itemIndex0 < length0; itemIndex0++)
            for (int itemIndex1 = 0; itemIndex1 < length1; itemIndex1++)
            for (int itemIndex2 = 0; itemIndex2 < length2; itemIndex2++)
            {
                CompressedIntegralTypes.WriteCompressedInt(ref writer, itemToConvert[itemIndex0, itemIndex1, itemIndex2]);
            }
        }
        
    }
}
