//c655ab59-8e36-ed4e-5bd3-877918ab68cf
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.105
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
                LazinatorParentClass = LazinatorParentClass,
                OriginalIncludeChildrenMode = includeChildrenMode,
                HierarchyBytes = bytes
            };
            clone.LazinatorParentClass = null;
            return clone;
        }
        
        /* Properties */
        protected int _MyLevel2ListNestedNonLazinatorType_ByteIndex;
        private int _Derived_DotNetList_Nested_NonSelfSerializable_EndByteIndex;
        protected virtual int _MyLevel2ListNestedNonLazinatorType_ByteLength => _Derived_DotNetList_Nested_NonSelfSerializable_EndByteIndex - _MyLevel2ListNestedNonLazinatorType_ByteIndex;
        
        private int _MyLevel2Int;
        public int MyLevel2Int
        {
            get
            {
                return _MyLevel2Int;
            }
            set
            {
                IsDirty = true;
                _MyLevel2Int = value;
                LazinatorUtilities.ConfirmDescendantDirtinessConsistency(this);
            }
        }
        private List<List<NonLazinatorClass>> _MyLevel2ListNestedNonLazinatorType;
        public List<List<NonLazinatorClass>> MyLevel2ListNestedNonLazinatorType
        {
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
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _MyLevel2ListNestedNonLazinatorType_ByteIndex, _MyLevel2ListNestedNonLazinatorType_ByteLength, false, false, null);
                        _MyLevel2ListNestedNonLazinatorType = ConvertFromBytes_List_GList_GNonLazinatorClass_g_g(childData, null);
                    }
                    _MyLevel2ListNestedNonLazinatorType_Accessed = true;
                }
                IsDirty = true;
                return _MyLevel2ListNestedNonLazinatorType;
            }
            set
            {
                IsDirty = true;
                _MyLevel2ListNestedNonLazinatorType = value;
                _MyLevel2ListNestedNonLazinatorType_Accessed = true;
                LazinatorUtilities.ConfirmDescendantDirtinessConsistency(this);
            }
        }
        protected bool _MyLevel2ListNestedNonLazinatorType_Accessed;
        
        protected override void ResetAccessedProperties()
        {
            base.ResetAccessedProperties();
            _MyLevel2ListNestedNonLazinatorType_Accessed = false;
        }
        
        /* Conversion */
        
        public override int LazinatorUniqueID => 260;
        
        protected override bool ContainsOpenGenericParameters => false;
        public override System.Collections.Generic.List<int> LazinatorGenericID
        {
            get => null;
            set { }
        }
        
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
        
        public override void SerializeExistingBuffer(BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren)
            {
                updateStoredBuffer = false;
            }
            int startPosition = writer.Position;
            WritePropertiesIntoBuffer(writer, includeChildrenMode, verifyCleanness, updateStoredBuffer, true);
            if (updateStoredBuffer)
            {
                
                _IsDirty = false;
                _DescendantIsDirty = false;
                
                _LazinatorObjectBytes = writer.Slice(startPosition);
            }
        }
        
        protected override void WritePropertiesIntoBuffer(BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer, bool includeUniqueID)
        {
            int startPosition = writer.Position;
            int startOfObjectPosition = 0;
            base.WritePropertiesIntoBuffer(writer, includeChildrenMode, verifyCleanness, updateStoredBuffer, includeUniqueID);
            // write properties
            CompressedIntegralTypes.WriteCompressedInt(writer, _MyLevel2Int);
            startOfObjectPosition = writer.Position;
            WriteNonLazinatorObject(
            nonLazinatorObject: _MyLevel2ListNestedNonLazinatorType, isBelievedDirty: _MyLevel2ListNestedNonLazinatorType_Accessed,
            isAccessed: _MyLevel2ListNestedNonLazinatorType_Accessed, writer: writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorObjectBytes, _MyLevel2ListNestedNonLazinatorType_ByteIndex, _MyLevel2ListNestedNonLazinatorType_ByteLength, false, false, null),
            verifyCleanness: false,
            binaryWriterAction: (w, v) =>
            ConvertToBytes_List_GList_GNonLazinatorClass_g_g(w, MyLevel2ListNestedNonLazinatorType,
            includeChildrenMode, v, updateStoredBuffer));
            if (updateStoredBuffer)
            {
                _MyLevel2ListNestedNonLazinatorType_ByteIndex = startOfObjectPosition - startPosition;
            }
            if (updateStoredBuffer)
            {
                _Derived_DotNetList_Nested_NonSelfSerializable_EndByteIndex = writer.Position - startPosition;
            }
        }
        
        /* Conversion of supported collections and tuples */
        
        private static List<List<NonLazinatorClass>> ConvertFromBytes_List_GList_GNonLazinatorClass_g_g(ReadOnlyMemory<byte> storage, InformParentOfDirtinessDelegate informParentOfDirtinessDelegate)
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
                    var item = ConvertFromBytes_List_GNonLazinatorClass_g(childData, informParentOfDirtinessDelegate);
                    collection.Add(item);
                }
                bytesSoFar += lengthCollectionMember;
            }
            
            return collection;
        }
        
        private static void ConvertToBytes_List_GList_GNonLazinatorClass_g_g(BinaryBufferWriter writer, List<List<NonLazinatorClass>> itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
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
                    
                    void action(BinaryBufferWriter w) => ConvertToBytes_List_GNonLazinatorClass_g(writer, itemToConvert[itemIndex], includeChildrenMode, verifyCleanness, updateStoredBuffer);
                    WriteToBinaryWithIntLengthPrefix(writer, action);
                }
                
            }
        }
        
        private static List<NonLazinatorClass> ConvertFromBytes_List_GNonLazinatorClass_g(ReadOnlyMemory<byte> storage, InformParentOfDirtinessDelegate informParentOfDirtinessDelegate)
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
                    var item = NonLazinatorDirectConverter.ConvertFromBytes_NonLazinatorClass(childData, informParentOfDirtinessDelegate);
                    collection.Add(item);
                }
                bytesSoFar += lengthCollectionMember;
            }
            
            return collection;
        }
        
        private static void ConvertToBytes_List_GNonLazinatorClass_g(BinaryBufferWriter writer, List<NonLazinatorClass> itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
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
                    
                    void action(BinaryBufferWriter w) => NonLazinatorDirectConverter.ConvertToBytes_NonLazinatorClass(writer, itemToConvert[itemIndex], includeChildrenMode, verifyCleanness, updateStoredBuffer);
                    WriteToBinaryWithIntLengthPrefix(writer, action);
                }
                
            }
        }
        
    }
}
