//de8c0940-273a-9440-3452-f8624bc14ee4
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Buffers;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using Lazinator.Buffers; 
using Lazinator.Collections;
using Lazinator.Core; 
using static Lazinator.Core.LazinatorUtilities;
using Lazinator.Exceptions;
using Lazinator.Support;
using Lazinator.Wrappers;

namespace LazinatorTests.Examples.Collections
{
    public partial class ArrayMultidimensional_Values : ILazinator
    {
        /* Boilerplate for every non-abstract ILazinator object */
        
        public virtual ILazinator LazinatorParentClass { get; set; }
        
        protected internal IncludeChildrenMode OriginalIncludeChildrenMode;
        
        public virtual void Deserialize()
        {
            int bytesSoFar = 0;
            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
            if (span.Length == 0)
            {
                return;
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
        }
        
        public virtual MemoryInBuffer SerializeNewBuffer(IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
            return EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, true, verifyCleanness, IsDirty, DescendantIsDirty, false, LazinatorObjectBytes, (StreamManuallyDelegate) EncodeToNewBuffer);
        }
        
        protected internal virtual MemoryInBuffer EncodeToNewBuffer(IncludeChildrenMode includeChildrenMode, bool verifyCleanness) => LazinatorUtilities.EncodeToNewBinaryBufferWriter(this, includeChildrenMode, verifyCleanness);
        
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
        
        private bool _IsDirty;
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
            InformParentOfDirtinessDelegate();
        }
        
        private bool _DescendantIsDirty;
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
        
        public virtual DeserializationFactory DeserializationFactory { get; set; }
        
        private MemoryInBuffer _HierarchyBytes;
        public virtual MemoryInBuffer HierarchyBytes
        {
            get => _HierarchyBytes;
            set
            {
                _HierarchyBytes = value;
                LazinatorObjectBytes = value.FilledMemory;
            }
        }
        
        private ReadOnlyMemory<byte> _LazinatorObjectBytes;
        public virtual ReadOnlyMemory<byte> LazinatorObjectBytes
        {
            get => _LazinatorObjectBytes;
            set
            {
                _LazinatorObjectBytes = value;
                Deserialize();
            }
        }
        
        /* Field boilerplate */
        
        internal int _MyArrayInt_ByteIndex;
        internal int _MyCrazyJaggedArray_ByteIndex;
        internal int _MyThreeDimArrayInt_ByteIndex;
        internal int _MyArrayInt_ByteLength => _MyCrazyJaggedArray_ByteIndex - _MyArrayInt_ByteIndex;
        internal int _MyCrazyJaggedArray_ByteLength => _MyThreeDimArrayInt_ByteIndex - _MyCrazyJaggedArray_ByteIndex;
        internal int _MyThreeDimArrayInt_ByteLength => LazinatorObjectBytes.Length - _MyThreeDimArrayInt_ByteIndex;
        
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
                        _MyArrayInt = ConvertFromBytes_Array2_int(childData, DeserializationFactory, () => { MyArrayInt_Dirty = true; });
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
        internal bool _MyArrayInt_Accessed;
        
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
                    IsDirty = true;
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
                        _MyCrazyJaggedArray = ConvertFromBytes_Array_Array3_Array4_int(childData, DeserializationFactory, null);
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
        internal bool _MyCrazyJaggedArray_Accessed;
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
                        _MyThreeDimArrayInt = ConvertFromBytes_Array3_int(childData, DeserializationFactory, null);
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
        internal bool _MyThreeDimArrayInt_Accessed;
        
        /* Conversion */
        
        public virtual int LazinatorUniqueID => 200;
        
        public virtual int LazinatorObjectVersion { get; set; } = 0;
        
        public virtual void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
            _MyArrayInt_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren) 
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            _MyCrazyJaggedArray_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren) 
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            _MyThreeDimArrayInt_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren) 
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
        }
        
        public virtual void SerializeExistingBuffer(BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
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
            ConvertToBytes_Array2_int(w, MyArrayInt,
            includeChildrenMode, v));
            WriteNonLazinatorObject(
            nonLazinatorObject: _MyCrazyJaggedArray, isBelievedDirty: _MyCrazyJaggedArray_Accessed,
            isAccessed: _MyCrazyJaggedArray_Accessed, writer: writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorObjectBytes, _MyCrazyJaggedArray_ByteIndex, _MyCrazyJaggedArray_ByteLength),
            verifyCleanness: false,
            binaryWriterAction: (w, v) =>
            ConvertToBytes_Array_Array3_Array4_int(w, MyCrazyJaggedArray,
            includeChildrenMode, v));
            WriteNonLazinatorObject(
            nonLazinatorObject: _MyThreeDimArrayInt, isBelievedDirty: _MyThreeDimArrayInt_Accessed,
            isAccessed: _MyThreeDimArrayInt_Accessed, writer: writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorObjectBytes, _MyThreeDimArrayInt_ByteIndex, _MyThreeDimArrayInt_ByteLength),
            verifyCleanness: false,
            binaryWriterAction: (w, v) =>
            ConvertToBytes_Array3_int(w, MyThreeDimArrayInt,
            includeChildrenMode, v));
        }
        
        /* Conversion of supported collections and tuples */
        
        private static int[,] ConvertFromBytes_Array2_int(ReadOnlyMemory<byte> storage, DeserializationFactory deserializationFactory, InformParentOfDirtinessDelegate informParentOfDirtinessDelegate)
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
        
        private static void ConvertToBytes_Array2_int(BinaryBufferWriter writer, int[,] itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
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
        
        private static int[][,,][,,,] ConvertFromBytes_Array_Array3_Array4_int(ReadOnlyMemory<byte> storage, DeserializationFactory deserializationFactory, InformParentOfDirtinessDelegate informParentOfDirtinessDelegate)
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
                    var item = ConvertFromBytes_Array3_Array4_int(childData, deserializationFactory, informParentOfDirtinessDelegate);
                    collection[i] = item;
                }
                bytesSoFar += lengthCollectionMember;
            }
            
            return collection;
        }
        
        private static void ConvertToBytes_Array_Array3_Array4_int(BinaryBufferWriter writer, int[][,,][,,,] itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
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
                    
                    void action(BinaryBufferWriter w) => ConvertToBytes_Array3_Array4_int(writer, itemToConvert[itemIndex], includeChildrenMode, verifyCleanness);
                    WriteToBinaryWithUintLengthPrefix(writer, action);
                }
                
            }
        }
        
        private static int[,,][,,,] ConvertFromBytes_Array3_Array4_int(ReadOnlyMemory<byte> storage, DeserializationFactory deserializationFactory, InformParentOfDirtinessDelegate informParentOfDirtinessDelegate)
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
                    var item = ConvertFromBytes_Array4_int(childData, deserializationFactory, informParentOfDirtinessDelegate);
                    collection[i0, i1, i2] = item;
                }
                bytesSoFar += lengthCollectionMember;
            }
            
            return collection;
        }
        
        private static void ConvertToBytes_Array3_Array4_int(BinaryBufferWriter writer, int[,,][,,,] itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
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
                    
                    void action(BinaryBufferWriter w) => ConvertToBytes_Array4_int(writer, itemToConvert[itemIndex0, itemIndex1, itemIndex2], includeChildrenMode, verifyCleanness);
                    WriteToBinaryWithUintLengthPrefix(writer, action);
                }
                
            }
        }
        
        private static int[,,,] ConvertFromBytes_Array4_int(ReadOnlyMemory<byte> storage, DeserializationFactory deserializationFactory, InformParentOfDirtinessDelegate informParentOfDirtinessDelegate)
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
        
        private static void ConvertToBytes_Array4_int(BinaryBufferWriter writer, int[,,,] itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
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
        
        private static int[,,] ConvertFromBytes_Array3_int(ReadOnlyMemory<byte> storage, DeserializationFactory deserializationFactory, InformParentOfDirtinessDelegate informParentOfDirtinessDelegate)
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
        
        private static void ConvertToBytes_Array3_int(BinaryBufferWriter writer, int[,,] itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
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
