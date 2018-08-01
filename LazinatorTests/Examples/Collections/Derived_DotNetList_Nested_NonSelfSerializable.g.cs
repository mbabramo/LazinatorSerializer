//a0b1a629-619f-15a2-1fe6-8bb576a48c84
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.208
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
    using System.Linq;
    using System.Runtime.InteropServices;
    using static Lazinator.Core.LazinatorUtilities;
    
    [Autogenerated]
    public partial class Derived_DotNetList_Nested_NonSelfSerializable : DotNetList_Nested_NonSelfSerializable, ILazinator
    {
        /* Clone overrides */
        
        public Derived_DotNetList_Nested_NonSelfSerializable() : base()
        {
        }
        
        public override ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode, bool updateStoredBuffer = false)
        {
            LazinatorMemory bytes = EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, false, IsDirty, DescendantIsDirty, false, LazinatorObjectBytes, (EncodeManuallyDelegate)EncodeToNewBuffer, updateStoredBuffer);
            var clone = new Derived_DotNetList_Nested_NonSelfSerializable()
            {
                LazinatorParents = LazinatorParents,
                OriginalIncludeChildrenMode = includeChildrenMode
            };
            clone.DeserializeLazinator(bytes);
            clone.LazinatorParents = default;
            return clone;
        }
        
        /* Properties */
        protected int _MyLevel2ListNestedNonLazinatorType_ByteIndex;
        private int _Derived_DotNetList_Nested_NonSelfSerializable_EndByteIndex;
        protected virtual int _MyLevel2ListNestedNonLazinatorType_ByteLength => _Derived_DotNetList_Nested_NonSelfSerializable_EndByteIndex - _MyLevel2ListNestedNonLazinatorType_ByteIndex;
        
        protected int _MyLevel2Int;
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
            }
        }
        protected List<List<NonLazinatorClass>> _MyLevel2ListNestedNonLazinatorType;
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
                        LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _MyLevel2ListNestedNonLazinatorType_ByteIndex, _MyLevel2ListNestedNonLazinatorType_ByteLength, false, false, null);
                        _MyLevel2ListNestedNonLazinatorType = ConvertFromBytes_List_GList_GNonLazinatorClass_g_g(childData);
                    }
                    _MyLevel2ListNestedNonLazinatorType_Accessed = true;
                }
                IsDirty = true; 
                return _MyLevel2ListNestedNonLazinatorType;
            }
            set
            {
                IsDirty = true;
                DescendantIsDirty = true;
                _MyLevel2ListNestedNonLazinatorType = value;
                _MyLevel2ListNestedNonLazinatorType_Accessed = true;
            }
        }
        protected bool _MyLevel2ListNestedNonLazinatorType_Accessed;
        
        public override IEnumerable<(string propertyName, ILazinator descendant)> EnumerateLazinatorDescendants(Func<ILazinator, bool> matchCriterion, bool stopExploringBelowMatch, Func<ILazinator, bool> exploreCriterion, bool exploreOnlyDeserializedChildren, bool enumerateNulls)
        {
            foreach (var inheritedYield in base.EnumerateLazinatorDescendants(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
            {
                yield return inheritedYield;
            }
            yield break;
        }
        
        
        public override IEnumerable<(string propertyName, object descendant)> EnumerateNonLazinatorProperties()
        {
            foreach (var inheritedYield in base.EnumerateNonLazinatorProperties())
            {
                yield return inheritedYield;
            }
            yield return ("MyLevel2Int", (object)MyLevel2Int);
            yield return ("MyLevel2ListNestedNonLazinatorType", (object)MyLevel2ListNestedNonLazinatorType);
            yield break;
        }
        
        protected override void ResetAccessedProperties()
        {
            base.ResetAccessedProperties();
            _MyLevel2ListNestedNonLazinatorType_Accessed = false;
            IsDirty = false;
            DescendantIsDirty = false;
            HasChanged = false;
            DescendantHasChanged = false;
        }
        
        /* Conversion */
        
        public override int LazinatorUniqueID => 260;
        
        protected override bool ContainsOpenGenericParameters => false;
        public override LazinatorGenericIDType LazinatorGenericID
        {
            get => default;
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
        
        public override void SerializeExistingBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
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
                if (_LazinatorMemoryStorage != null && LazinatorParents.Any())
                {
                    _LazinatorMemoryStorage.DisposeWhenOriginalSourceDisposed(newBuffer);
                }
                _LazinatorMemoryStorage = newBuffer;
            }
        }
        
        protected override void WritePropertiesIntoBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer, bool includeUniqueID)
        {
            int startPosition = writer.Position;
            int startOfObjectPosition = 0;
            base.WritePropertiesIntoBuffer(ref writer, includeChildrenMode, verifyCleanness, updateStoredBuffer, includeUniqueID);
            // write properties
            CompressedIntegralTypes.WriteCompressedInt(ref writer, _MyLevel2Int);
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren && !_MyLevel2ListNestedNonLazinatorType_Accessed)
            {
                var deserialized = MyLevel2ListNestedNonLazinatorType;
            }
            WriteNonLazinatorObject(
            nonLazinatorObject: _MyLevel2ListNestedNonLazinatorType, isBelievedDirty: _MyLevel2ListNestedNonLazinatorType_Accessed,
            isAccessed: _MyLevel2ListNestedNonLazinatorType_Accessed, writer: ref writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorMemoryStorage, _MyLevel2ListNestedNonLazinatorType_ByteIndex, _MyLevel2ListNestedNonLazinatorType_ByteLength, false, false, null),
            verifyCleanness: false,
            binaryWriterAction: (ref BinaryBufferWriter w, bool v) =>
            ConvertToBytes_List_GList_GNonLazinatorClass_g_g(ref w, _MyLevel2ListNestedNonLazinatorType,
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
        
        private static List<List<NonLazinatorClass>> ConvertFromBytes_List_GList_GNonLazinatorClass_g_g(LazinatorMemory storage)
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
                    LazinatorMemory childData = storage.Slice(bytesSoFar, lengthCollectionMember);
                    var item = ConvertFromBytes_List_GNonLazinatorClass_g(childData);
                    collection.Add(item);
                }
                bytesSoFar += lengthCollectionMember;
            }
            
            return collection;
        }
        
        private static void ConvertToBytes_List_GList_GNonLazinatorClass_g_g(ref BinaryBufferWriter writer, List<List<NonLazinatorClass>> itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            if (itemToConvert == default(List<List<NonLazinatorClass>>))
            {
                return;
            }
            CompressedIntegralTypes.WriteCompressedInt(ref writer, itemToConvert.Count);
            int itemToConvertCount = itemToConvert.Count;
            for (int itemIndex = 0; itemIndex < itemToConvertCount; itemIndex++)
            {
                if (itemToConvert[itemIndex] == default(List<NonLazinatorClass>))
                {
                    writer.Write((uint)0);
                }
                else 
                {
                    
                    void action(ref BinaryBufferWriter w) => ConvertToBytes_List_GNonLazinatorClass_g(ref w, itemToConvert[itemIndex], includeChildrenMode, verifyCleanness, updateStoredBuffer);
                    WriteToBinaryWithIntLengthPrefix(ref writer, action);
                }
                
            }
        }
        
        private static List<NonLazinatorClass> ConvertFromBytes_List_GNonLazinatorClass_g(LazinatorMemory storage)
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
                    LazinatorMemory childData = storage.Slice(bytesSoFar, lengthCollectionMember);
                    var item = NonLazinatorDirectConverter.ConvertFromBytes_NonLazinatorClass(childData);
                    collection.Add(item);
                }
                bytesSoFar += lengthCollectionMember;
            }
            
            return collection;
        }
        
        private static void ConvertToBytes_List_GNonLazinatorClass_g(ref BinaryBufferWriter writer, List<NonLazinatorClass> itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            if (itemToConvert == default(List<NonLazinatorClass>))
            {
                return;
            }
            CompressedIntegralTypes.WriteCompressedInt(ref writer, itemToConvert.Count);
            int itemToConvertCount = itemToConvert.Count;
            for (int itemIndex = 0; itemIndex < itemToConvertCount; itemIndex++)
            {
                if (itemToConvert[itemIndex] == default(NonLazinatorClass))
                {
                    writer.Write((uint)0);
                }
                else 
                {
                    
                    void action(ref BinaryBufferWriter w) => NonLazinatorDirectConverter.ConvertToBytes_NonLazinatorClass(ref w, itemToConvert[itemIndex], includeChildrenMode, verifyCleanness, updateStoredBuffer);
                    WriteToBinaryWithIntLengthPrefix(ref writer, action);
                }
                
            }
        }
        
    }
}
