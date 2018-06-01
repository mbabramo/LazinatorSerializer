//5f77d183-6ff5-e0eb-1fd5-14f16c853d88
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.70
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
    using LazinatorTests.Examples;
    using System;
    using System.Buffers;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.InteropServices;
    using static Lazinator.Core.LazinatorUtilities;
    
    [Autogenerated]
    public partial class Derived_DotNetList_Nested_NonSelfSerializable : DotNetList_Nested_NonSelfSerializable, ILazinator
    {
        /* Clone overrides */
        
        public Derived_DotNetList_Nested_NonSelfSerializable() : base()
        {
        }
        
        public override ILazinator CloneLazinator()
        {
            return CloneLazinator(OriginalIncludeChildrenMode);
        }
        
        public override ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode)
        {
            MemoryInBuffer bytes = EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, false, false, IsDirty, DescendantIsDirty, false, LazinatorObjectBytes, (StreamManuallyDelegate)EncodeToNewBuffer);
            var clone = new Derived_DotNetList_Nested_NonSelfSerializable()
            {
                DeserializationFactory = DeserializationFactory,
                LazinatorParentClass = LazinatorParentClass,
                InformParentOfDirtinessDelegate = InformParentOfDirtinessDelegate,
                OriginalIncludeChildrenMode = includeChildrenMode,
                HierarchyBytes = bytes
            };
            return clone;
        }
        
        /* Properties */
        protected int _MyLevel2ListNestedNonLazinatorType_ByteIndex;
        private int _Derived_DotNetList_Nested_NonSelfSerializable_EndByteIndex;
        protected virtual int _MyLevel2ListNestedNonLazinatorType_ByteLength => _Derived_DotNetList_Nested_NonSelfSerializable_EndByteIndex - _MyLevel2ListNestedNonLazinatorType_ByteIndex;
        
        private int _MyLevel2Int;
        public int MyLevel2Int
        {
            [DebuggerStepThrough]
            get
            {
                return _MyLevel2Int;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _MyLevel2Int = value;
            }
        }
        private List<List<NonLazinatorClass>> _MyLevel2ListNestedNonLazinatorType;
        public List<List<NonLazinatorClass>> MyLevel2ListNestedNonLazinatorType
        {
            [DebuggerStepThrough]
            get
            {
                if (!_MyLevel2ListNestedNonLazinatorType_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _MyLevel2ListNestedNonLazinatorType = default(List<List<NonLazinatorClass>>);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _MyLevel2ListNestedNonLazinatorType_ByteIndex, _MyLevel2ListNestedNonLazinatorType_ByteLength);
                        _MyLevel2ListNestedNonLazinatorType = ConvertFromBytes_List_GList_GNonLazinatorClass_g_g(childData, DeserializationFactory, null);
                    }
                    _MyLevel2ListNestedNonLazinatorType_Accessed = true;
                    IsDirty = true;
                }
                return _MyLevel2ListNestedNonLazinatorType;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _MyLevel2ListNestedNonLazinatorType = value;
                _MyLevel2ListNestedNonLazinatorType_Accessed = true;
            }
        }
        protected bool _MyLevel2ListNestedNonLazinatorType_Accessed;
        
        /* Conversion */
        
        public override int LazinatorUniqueID => 260;
        
        public override int LazinatorObjectVersion { get; set; } = 0;
        
        public override void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            base.ConvertFromBytesAfterHeader(OriginalIncludeChildrenMode, serializedVersionNumber, ref bytesSoFar);
            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
            _MyLevel2Int = span.ToDecompressedInt(ref bytesSoFar);
            _MyLevel2ListNestedNonLazinatorType_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            _Derived_DotNetList_Nested_NonSelfSerializable_EndByteIndex = bytesSoFar;
        }
        
        public override void SerializeExistingBuffer(BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
            int startPosition = writer.Position;
            base.SerializeExistingBuffer(writer, includeChildrenMode, verifyCleanness);
            // write properties
            CompressedIntegralTypes.WriteCompressedInt(writer, _MyLevel2Int);
            WriteNonLazinatorObject(
            nonLazinatorObject: _MyLevel2ListNestedNonLazinatorType, isBelievedDirty: _MyLevel2ListNestedNonLazinatorType_Accessed,
            isAccessed: _MyLevel2ListNestedNonLazinatorType_Accessed, writer: writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorObjectBytes, _MyLevel2ListNestedNonLazinatorType_ByteIndex, _MyLevel2ListNestedNonLazinatorType_ByteLength),
            verifyCleanness: false,
            binaryWriterAction: (w, v) =>
            ConvertToBytes_List_GList_GNonLazinatorClass_g_g(w, MyLevel2ListNestedNonLazinatorType,
            includeChildrenMode, v));
            
            _IsDirty = false;
            _DescendantIsDirty = false;
            
            _LazinatorObjectBytes = writer.Slice(startPosition);
        }
        
        /* Conversion of supported collections and tuples */
        
        private static List<List<NonLazinatorClass>> ConvertFromBytes_List_GList_GNonLazinatorClass_g_g(ReadOnlyMemory<byte> storage, DeserializationFactory deserializationFactory, InformParentOfDirtinessDelegate informParentOfDirtinessDelegate)
        {
            if (storage.Length == 0)
            {
                return default(List<List<NonLazinatorClass>>);
            }
            ReadOnlySpan<byte> span = storage.Span;
            
            int bytesSoFar = 0;
            int collectionLength = span.ToDecompressedInt(ref bytesSoFar);
            
            List<List<NonLazinatorClass>> collection = new List<List<NonLazinatorClass>>(collectionLength);
            for (int i = 0; i < collectionLength; i++)
            {
                int lengthCollectionMember = span.ToInt32(ref bytesSoFar);
                if (lengthCollectionMember == 0)
                {
                    collection.Add(default(List<NonLazinatorClass>));
                }
                else
                {
                    ReadOnlyMemory<byte> childData = storage.Slice(bytesSoFar, lengthCollectionMember);
                    var item = ConvertFromBytes_List_GNonLazinatorClass_g(childData, deserializationFactory, informParentOfDirtinessDelegate);
                    collection.Add(item);
                }
                bytesSoFar += lengthCollectionMember;
            }
            
            return collection;
        }
        
        private static void ConvertToBytes_List_GList_GNonLazinatorClass_g_g(BinaryBufferWriter writer, List<List<NonLazinatorClass>> itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
            if (itemToConvert == default(List<List<NonLazinatorClass>>))
            {
                return;
            }
            CompressedIntegralTypes.WriteCompressedInt(writer, itemToConvert.Count);
            int itemToConvertCount = itemToConvert.Count;
            for (int itemIndex = 0; itemIndex < itemToConvertCount; itemIndex++)
            {
                if (itemToConvert[itemIndex] == default(List<NonLazinatorClass>))
                {
                    writer.Write((uint)0);
                }
                else 
                {
                    
                    void action(BinaryBufferWriter w) => ConvertToBytes_List_GNonLazinatorClass_g(writer, itemToConvert[itemIndex], includeChildrenMode, verifyCleanness);
                    WriteToBinaryWithIntLengthPrefix(writer, action);
                }
                
            }
        }
        
        private static List<NonLazinatorClass> ConvertFromBytes_List_GNonLazinatorClass_g(ReadOnlyMemory<byte> storage, DeserializationFactory deserializationFactory, InformParentOfDirtinessDelegate informParentOfDirtinessDelegate)
        {
            if (storage.Length == 0)
            {
                return default(List<NonLazinatorClass>);
            }
            ReadOnlySpan<byte> span = storage.Span;
            
            int bytesSoFar = 0;
            int collectionLength = span.ToDecompressedInt(ref bytesSoFar);
            
            List<NonLazinatorClass> collection = new List<NonLazinatorClass>(collectionLength);
            for (int i = 0; i < collectionLength; i++)
            {
                int lengthCollectionMember = span.ToInt32(ref bytesSoFar);
                if (lengthCollectionMember == 0)
                {
                    collection.Add(default(NonLazinatorClass));
                }
                else
                {
                    ReadOnlyMemory<byte> childData = storage.Slice(bytesSoFar, lengthCollectionMember);
                    var item = NonLazinatorDirectConverter.ConvertFromBytes_NonLazinatorClass(childData, deserializationFactory, informParentOfDirtinessDelegate);
                    collection.Add(item);
                }
                bytesSoFar += lengthCollectionMember;
            }
            
            return collection;
        }
        
        private static void ConvertToBytes_List_GNonLazinatorClass_g(BinaryBufferWriter writer, List<NonLazinatorClass> itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
            if (itemToConvert == default(List<NonLazinatorClass>))
            {
                return;
            }
            CompressedIntegralTypes.WriteCompressedInt(writer, itemToConvert.Count);
            int itemToConvertCount = itemToConvert.Count;
            for (int itemIndex = 0; itemIndex < itemToConvertCount; itemIndex++)
            {
                if (itemToConvert[itemIndex] == default(NonLazinatorClass))
                {
                    writer.Write((uint)0);
                }
                else 
                {
                    
                    void action(BinaryBufferWriter w) => NonLazinatorDirectConverter.ConvertToBytes_NonLazinatorClass(writer, itemToConvert[itemIndex], includeChildrenMode, verifyCleanness);
                    WriteToBinaryWithIntLengthPrefix(writer, action);
                }
                
            }
        }
        
    }
}
