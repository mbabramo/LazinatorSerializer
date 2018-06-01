//61296149-68b8-16ed-adb1-481595c4d0ab
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.68
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
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.InteropServices;
    using static Lazinator.Core.LazinatorUtilities;
    
    [Autogenerated]
    public partial class ArrayMultidimensional_Values : ILazinator
    {
        /* Serialization, deserialization, and object relationships */
        
        public virtual ILazinator LazinatorParentClass { get; set; }
        
        protected IncludeChildrenMode OriginalIncludeChildrenMode;
        
        public virtual int Deserialize()
        {
            int bytesSoFar = 0;
            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
            if (span.Length == 0)
            {
                return 0;
            }
            
            int uniqueID = span.ToDecompressedInt(ref bytesSoFar);
            if (uniqueID != LazinatorUniqueID)
            {
                throw new FormatException("Wrong self-serialized type initialized.");
            }
            
            int lazinatorLibraryVersion = span.ToDecompressedInt(ref bytesSoFar);
            
            int serializedVersionNumber = span.ToDecompressedInt(ref bytesSoFar);
            
            OriginalIncludeChildrenMode = (IncludeChildrenMode)span.ToByte(ref bytesSoFar);
            
            ConvertFromBytesAfterHeader(OriginalIncludeChildrenMode, serializedVersionNumber, ref bytesSoFar);
            return bytesSoFar;
        }
        
        public virtual MemoryInBuffer SerializeNewBuffer(IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
            return EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, true, verifyCleanness, IsDirty, DescendantIsDirty, false, LazinatorObjectBytes, (StreamManuallyDelegate) EncodeToNewBuffer);
        }
        
        protected virtual MemoryInBuffer EncodeToNewBuffer(IncludeChildrenMode includeChildrenMode, bool verifyCleanness) => LazinatorUtilities.EncodeToNewBinaryBufferWriter(this, includeChildrenMode, verifyCleanness);
        
        public virtual ILazinator CloneLazinator()
        {
            return CloneLazinator(OriginalIncludeChildrenMode);
        }
        
        public virtual ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode)
        {
            MemoryInBuffer bytes = EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, false, false, IsDirty, DescendantIsDirty, false, LazinatorObjectBytes, (StreamManuallyDelegate)EncodeToNewBuffer);
            var clone = new ArrayMultidimensional_Values()
            {
                DeserializationFactory = DeserializationFactory,
                LazinatorParentClass = LazinatorParentClass,
                InformParentOfDirtinessDelegate = InformParentOfDirtinessDelegate,
                OriginalIncludeChildrenMode = includeChildrenMode,
                HierarchyBytes = bytes
            };
            return clone;
        }
        
        protected bool _IsDirty;
        public virtual bool IsDirty
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
                        InformParentOfDirtiness();
                    }
                }
            }
        }
        
        public virtual InformParentOfDirtinessDelegate InformParentOfDirtinessDelegate { get; set; }
        public virtual void InformParentOfDirtiness()
        {
            if (InformParentOfDirtinessDelegate == null)
            {
                if (LazinatorParentClass != null)
                {
                    LazinatorParentClass.DescendantIsDirty = true;
                }
            }
            else
            {
                InformParentOfDirtinessDelegate();
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
                    if (_DescendantIsDirty && LazinatorParentClass != null)
                    {
                        LazinatorParentClass.DescendantIsDirty = true;
                    }
                }
            }
        }
        
        public virtual void MarkHierarchyClean()
        {
            _IsDirty = false;
            _DescendantIsDirty = false;
            _MyArrayInt_Dirty = false;
        }
        
        public virtual DeserializationFactory DeserializationFactory { get; set; }
        
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
                Deserialize();
            }
        }
        
        public virtual void LazinatorConvertToBytes()
        {
            if (!IsDirty && !DescendantIsDirty)
            {
                return;
            }
            MemoryInBuffer bytes = EncodeOrRecycleToNewBuffer(IncludeChildrenMode.IncludeAllChildren, OriginalIncludeChildrenMode, false, false, IsDirty, DescendantIsDirty, false, LazinatorObjectBytes, (StreamManuallyDelegate)EncodeToNewBuffer);
            LazinatorObjectBytes = bytes.FilledMemory;
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
        
        /* Field definitions */
        
        protected int _MyArrayInt_ByteIndex;
        protected int _MyCrazyJaggedArray_ByteIndex;
        protected int _MyThreeDimArrayInt_ByteIndex;
        protected virtual int _MyArrayInt_ByteLength => _MyCrazyJaggedArray_ByteIndex - _MyArrayInt_ByteIndex;
        protected virtual int _MyCrazyJaggedArray_ByteLength => _MyThreeDimArrayInt_ByteIndex - _MyCrazyJaggedArray_ByteIndex;
        private int _ArrayMultidimensional_Values_EndByteIndex;
        protected virtual int _MyThreeDimArrayInt_ByteLength => _ArrayMultidimensional_Values_EndByteIndex - _MyThreeDimArrayInt_ByteIndex;
        
        private int[,] _MyArrayInt;
        public int[,] MyArrayInt
        {
            [DebuggerStepThrough]
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
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _MyArrayInt_ByteIndex, _MyArrayInt_ByteLength);
                        _MyArrayInt = ConvertFromBytes_int_B_c_b(childData, DeserializationFactory, () => { MyArrayInt_Dirty = true; });
                    }
                    _MyArrayInt_Accessed = true;
                }
                return _MyArrayInt;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _MyArrayInt = value;
                _MyArrayInt_Dirty = true;
                _MyArrayInt_Accessed = true;
            }
        }
        protected bool _MyArrayInt_Accessed;
        
        private bool _MyArrayInt_Dirty;
        public bool MyArrayInt_Dirty
        {
            [DebuggerStepThrough]
            get => _MyArrayInt_Dirty;
            [DebuggerStepThrough]
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
        private int[][,,][,,,] _MyCrazyJaggedArray;
        public int[][,,][,,,] MyCrazyJaggedArray
        {
            [DebuggerStepThrough]
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
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _MyCrazyJaggedArray_ByteIndex, _MyCrazyJaggedArray_ByteLength);
                        _MyCrazyJaggedArray = ConvertFromBytes_int_B_b_B_c_c_b_B_c_c_c_b(childData, DeserializationFactory, null);
                    }
                    _MyCrazyJaggedArray_Accessed = true;
                    IsDirty = true;
                }
                return _MyCrazyJaggedArray;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _MyCrazyJaggedArray = value;
                _MyCrazyJaggedArray_Accessed = true;
            }
        }
        protected bool _MyCrazyJaggedArray_Accessed;
        private int[,,] _MyThreeDimArrayInt;
        public int[,,] MyThreeDimArrayInt
        {
            [DebuggerStepThrough]
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
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _MyThreeDimArrayInt_ByteIndex, _MyThreeDimArrayInt_ByteLength);
                        _MyThreeDimArrayInt = ConvertFromBytes_int_B_c_c_b(childData, DeserializationFactory, null);
                    }
                    _MyThreeDimArrayInt_Accessed = true;
                    IsDirty = true;
                }
                return _MyThreeDimArrayInt;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _MyThreeDimArrayInt = value;
                _MyThreeDimArrayInt_Accessed = true;
            }
        }
        protected bool _MyThreeDimArrayInt_Accessed;
        
        /* Conversion */
        
        public virtual int LazinatorUniqueID => 200;
        
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
        
        public virtual void SerializeExistingBuffer(BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
            int startPosition = writer.Position;
            // header information
            CompressedIntegralTypes.WriteCompressedInt(writer, LazinatorUniqueID);
            CompressedIntegralTypes.WriteCompressedInt(writer, Lazinator.Support.LazinatorVersionInfo.LazinatorIntVersion);
            CompressedIntegralTypes.WriteCompressedInt(writer, LazinatorObjectVersion);
            writer.Write((byte)includeChildrenMode);
            // write properties
            WriteNonLazinatorObject(
            nonLazinatorObject: _MyArrayInt, isBelievedDirty: MyArrayInt_Dirty,
            isAccessed: _MyArrayInt_Accessed, writer: writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorObjectBytes, _MyArrayInt_ByteIndex, _MyArrayInt_ByteLength),
            verifyCleanness: verifyCleanness,
            binaryWriterAction: (w, v) =>
            ConvertToBytes_int_B_c_b(w, MyArrayInt,
            includeChildrenMode, v));
            WriteNonLazinatorObject(
            nonLazinatorObject: _MyCrazyJaggedArray, isBelievedDirty: _MyCrazyJaggedArray_Accessed,
            isAccessed: _MyCrazyJaggedArray_Accessed, writer: writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorObjectBytes, _MyCrazyJaggedArray_ByteIndex, _MyCrazyJaggedArray_ByteLength),
            verifyCleanness: false,
            binaryWriterAction: (w, v) =>
            ConvertToBytes_int_B_b_B_c_c_b_B_c_c_c_b(w, MyCrazyJaggedArray,
            includeChildrenMode, v));
            WriteNonLazinatorObject(
            nonLazinatorObject: _MyThreeDimArrayInt, isBelievedDirty: _MyThreeDimArrayInt_Accessed,
            isAccessed: _MyThreeDimArrayInt_Accessed, writer: writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorObjectBytes, _MyThreeDimArrayInt_ByteIndex, _MyThreeDimArrayInt_ByteLength),
            verifyCleanness: false,
            binaryWriterAction: (w, v) =>
            ConvertToBytes_int_B_c_c_b(w, MyThreeDimArrayInt,
            includeChildrenMode, v));
            
            _IsDirty = false;
            _DescendantIsDirty = false;
            
            _LazinatorObjectBytes = writer.Slice(startPosition);
        }
        
        /* Conversion of supported collections and tuples */
        
        private static int[,] ConvertFromBytes_int_B_c_b(ReadOnlyMemory<byte> storage, DeserializationFactory deserializationFactory, InformParentOfDirtinessDelegate informParentOfDirtinessDelegate)
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
        
        private static void ConvertToBytes_int_B_c_b(BinaryBufferWriter writer, int[,] itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
            if (itemToConvert == default(int[,]))
            {
                return;
            }
            CompressedIntegralTypes.WriteCompressedInt(writer, itemToConvert.GetLength(0));
            CompressedIntegralTypes.WriteCompressedInt(writer, itemToConvert.GetLength(1));
            int length0 = itemToConvert.GetLength(0);
            int length1 = itemToConvert.GetLength(1);
            for (int itemIndex0 = 0; itemIndex0 < length0; itemIndex0++)
            for (int itemIndex1 = 0; itemIndex1 < length1; itemIndex1++)
            {
                CompressedIntegralTypes.WriteCompressedInt(writer, itemToConvert[itemIndex0, itemIndex1]);
            }
        }
        
        private static int[][,,][,,,] ConvertFromBytes_int_B_b_B_c_c_b_B_c_c_c_b(ReadOnlyMemory<byte> storage, DeserializationFactory deserializationFactory, InformParentOfDirtinessDelegate informParentOfDirtinessDelegate)
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
                    ReadOnlyMemory<byte> childData = storage.Slice(bytesSoFar, lengthCollectionMember);
                    var item = ConvertFromBytes_int_B_c_c_b_B_c_c_c_b(childData, deserializationFactory, informParentOfDirtinessDelegate);
                    collection[i] = item;
                }
                bytesSoFar += lengthCollectionMember;
            }
            
            return collection;
        }
        
        private static void ConvertToBytes_int_B_b_B_c_c_b_B_c_c_c_b(BinaryBufferWriter writer, int[][,,][,,,] itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
            if (itemToConvert == default(int[][,,][,,,]))
            {
                return;
            }
            CompressedIntegralTypes.WriteCompressedInt(writer, itemToConvert.Length);
            int itemToConvertCount = itemToConvert.Length;
            for (int itemIndex = 0; itemIndex < itemToConvertCount; itemIndex++)
            {
                if (itemToConvert[itemIndex] == default(int[,,][,,,]))
                {
                    writer.Write((uint)0);
                }
                else 
                {
                    
                    void action(BinaryBufferWriter w) => ConvertToBytes_int_B_c_c_b_B_c_c_c_b(writer, itemToConvert[itemIndex], includeChildrenMode, verifyCleanness);
                    WriteToBinaryWithIntLengthPrefix(writer, action);
                }
                
            }
        }
        
        private static int[,,][,,,] ConvertFromBytes_int_B_c_c_b_B_c_c_c_b(ReadOnlyMemory<byte> storage, DeserializationFactory deserializationFactory, InformParentOfDirtinessDelegate informParentOfDirtinessDelegate)
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
                    ReadOnlyMemory<byte> childData = storage.Slice(bytesSoFar, lengthCollectionMember);
                    var item = ConvertFromBytes_int_B_c_c_c_b(childData, deserializationFactory, informParentOfDirtinessDelegate);
                    collection[i0, i1, i2] = item;
                }
                bytesSoFar += lengthCollectionMember;
            }
            
            return collection;
        }
        
        private static void ConvertToBytes_int_B_c_c_b_B_c_c_c_b(BinaryBufferWriter writer, int[,,][,,,] itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
            if (itemToConvert == default(int[,,][,,,]))
            {
                return;
            }
            CompressedIntegralTypes.WriteCompressedInt(writer, itemToConvert.GetLength(0));
            CompressedIntegralTypes.WriteCompressedInt(writer, itemToConvert.GetLength(1));
            CompressedIntegralTypes.WriteCompressedInt(writer, itemToConvert.GetLength(2));
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
                    
                    void action(BinaryBufferWriter w) => ConvertToBytes_int_B_c_c_c_b(writer, itemToConvert[itemIndex0, itemIndex1, itemIndex2], includeChildrenMode, verifyCleanness);
                    WriteToBinaryWithIntLengthPrefix(writer, action);
                }
                
            }
        }
        
        private static int[,,,] ConvertFromBytes_int_B_c_c_c_b(ReadOnlyMemory<byte> storage, DeserializationFactory deserializationFactory, InformParentOfDirtinessDelegate informParentOfDirtinessDelegate)
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
        
        private static void ConvertToBytes_int_B_c_c_c_b(BinaryBufferWriter writer, int[,,,] itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
            if (itemToConvert == default(int[,,,]))
            {
                return;
            }
            CompressedIntegralTypes.WriteCompressedInt(writer, itemToConvert.GetLength(0));
            CompressedIntegralTypes.WriteCompressedInt(writer, itemToConvert.GetLength(1));
            CompressedIntegralTypes.WriteCompressedInt(writer, itemToConvert.GetLength(2));
            CompressedIntegralTypes.WriteCompressedInt(writer, itemToConvert.GetLength(3));
            int length0 = itemToConvert.GetLength(0);
            int length1 = itemToConvert.GetLength(1);
            int length2 = itemToConvert.GetLength(2);
            int length3 = itemToConvert.GetLength(3);
            for (int itemIndex0 = 0; itemIndex0 < length0; itemIndex0++)
            for (int itemIndex1 = 0; itemIndex1 < length1; itemIndex1++)
            for (int itemIndex2 = 0; itemIndex2 < length2; itemIndex2++)
            for (int itemIndex3 = 0; itemIndex3 < length3; itemIndex3++)
            {
                CompressedIntegralTypes.WriteCompressedInt(writer, itemToConvert[itemIndex0, itemIndex1, itemIndex2, itemIndex3]);
            }
        }
        
        private static int[,,] ConvertFromBytes_int_B_c_c_b(ReadOnlyMemory<byte> storage, DeserializationFactory deserializationFactory, InformParentOfDirtinessDelegate informParentOfDirtinessDelegate)
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
        
        private static void ConvertToBytes_int_B_c_c_b(BinaryBufferWriter writer, int[,,] itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
            if (itemToConvert == default(int[,,]))
            {
                return;
            }
            CompressedIntegralTypes.WriteCompressedInt(writer, itemToConvert.GetLength(0));
            CompressedIntegralTypes.WriteCompressedInt(writer, itemToConvert.GetLength(1));
            CompressedIntegralTypes.WriteCompressedInt(writer, itemToConvert.GetLength(2));
            int length0 = itemToConvert.GetLength(0);
            int length1 = itemToConvert.GetLength(1);
            int length2 = itemToConvert.GetLength(2);
            for (int itemIndex0 = 0; itemIndex0 < length0; itemIndex0++)
            for (int itemIndex1 = 0; itemIndex1 < length1; itemIndex1++)
            for (int itemIndex2 = 0; itemIndex2 < length2; itemIndex2++)
            {
                CompressedIntegralTypes.WriteCompressedInt(writer, itemToConvert[itemIndex0, itemIndex1, itemIndex2]);
            }
        }
        
    }
}
