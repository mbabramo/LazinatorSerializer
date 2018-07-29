//34dcc2a4-b819-9821-f003-cf5f5e277138
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.207
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LazinatorTests.Examples.Tuples
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
    public partial class StructTuple : ILazinator
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
            var clone = new StructTuple()
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
        
        protected int _EnumTuple_ByteIndex;
        protected int _MyNamedTuple_ByteIndex;
        protected int _MyNullableTuple_ByteIndex;
        protected int _MyValueTupleSerialized_ByteIndex;
        protected virtual int _EnumTuple_ByteLength => _MyNamedTuple_ByteIndex - _EnumTuple_ByteIndex;
        protected virtual int _MyNamedTuple_ByteLength => _MyNullableTuple_ByteIndex - _MyNamedTuple_ByteIndex;
        protected virtual int _MyNullableTuple_ByteLength => _MyValueTupleSerialized_ByteIndex - _MyNullableTuple_ByteIndex;
        private int _StructTuple_EndByteIndex;
        protected virtual int _MyValueTupleSerialized_ByteLength => _StructTuple_EndByteIndex - _MyValueTupleSerialized_ByteIndex;
        
        protected (TestEnum firstEnum, TestEnum anotherEnum) _EnumTuple;
        public (TestEnum firstEnum, TestEnum anotherEnum) EnumTuple
        {
            get
            {
                if (!_EnumTuple_Accessed)
                {
                    if (_LazinatorObjectBytes.Length == 0)
                    {
                        _EnumTuple = default((TestEnum firstEnum, TestEnum anotherEnum));
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _EnumTuple_ByteIndex, _EnumTuple_ByteLength, false, false, null);
                        _EnumTuple = ConvertFromBytes__PTestEnum_C32firstEnum_c_C32TestEnum_C32anotherEnum_p(childData);
                    }
                    _EnumTuple_Accessed = true;
                }
                IsDirty = true; 
                return _EnumTuple;
            }
            set
            {
                IsDirty = true;
                DescendantIsDirty = true;
                _EnumTuple = value;
                _EnumTuple_Accessed = true;
            }
        }
        protected bool _EnumTuple_Accessed;
        protected (int MyFirstItem, double MySecondItem) _MyNamedTuple;
        public (int MyFirstItem, double MySecondItem) MyNamedTuple
        {
            get
            {
                if (!_MyNamedTuple_Accessed)
                {
                    if (_LazinatorObjectBytes.Length == 0)
                    {
                        _MyNamedTuple = default((int MyFirstItem, double MySecondItem));
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _MyNamedTuple_ByteIndex, _MyNamedTuple_ByteLength, false, false, null);
                        _MyNamedTuple = ConvertFromBytes__Pint_C32MyFirstItem_c_C32double_C32MySecondItem_p(childData);
                    }
                    _MyNamedTuple_Accessed = true;
                }
                IsDirty = true; 
                return _MyNamedTuple;
            }
            set
            {
                IsDirty = true;
                DescendantIsDirty = true;
                _MyNamedTuple = value;
                _MyNamedTuple_Accessed = true;
            }
        }
        protected bool _MyNamedTuple_Accessed;
        protected (int, double)? _MyNullableTuple;
        public (int, double)? MyNullableTuple
        {
            get
            {
                if (!_MyNullableTuple_Accessed)
                {
                    if (_LazinatorObjectBytes.Length == 0)
                    {
                        _MyNullableTuple = default((int, double)?);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _MyNullableTuple_ByteIndex, _MyNullableTuple_ByteLength, false, false, null);
                        _MyNullableTuple = ConvertFromBytes__Pint_c_C32double_p_C63(childData);
                    }
                    _MyNullableTuple_Accessed = true;
                }
                IsDirty = true; 
                return _MyNullableTuple;
            }
            set
            {
                IsDirty = true;
                DescendantIsDirty = true;
                _MyNullableTuple = value;
                _MyNullableTuple_Accessed = true;
            }
        }
        protected bool _MyNullableTuple_Accessed;
        protected (uint, ExampleChild, NonLazinatorClass) _MyValueTupleSerialized;
        public (uint, ExampleChild, NonLazinatorClass) MyValueTupleSerialized
        {
            get
            {
                if (!_MyValueTupleSerialized_Accessed)
                {
                    if (_LazinatorObjectBytes.Length == 0)
                    {
                        _MyValueTupleSerialized = default((uint, ExampleChild, NonLazinatorClass));
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _MyValueTupleSerialized_ByteIndex, _MyValueTupleSerialized_ByteLength, false, false, null);
                        _MyValueTupleSerialized = ConvertFromBytes__Puint_c_C32ExampleChild_c_C32NonLazinatorClass_p(childData);
                    }
                    _MyValueTupleSerialized_Accessed = true;
                }
                IsDirty = true; 
                return _MyValueTupleSerialized;
            }
            set
            {
                IsDirty = true;
                DescendantIsDirty = true;
                _MyValueTupleSerialized = value;
                _MyValueTupleSerialized_Accessed = true;
            }
        }
        protected bool _MyValueTupleSerialized_Accessed;
        
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
            yield return ("EnumTuple", (object)EnumTuple);
            yield return ("MyNamedTuple", (object)MyNamedTuple);
            yield return ("MyNullableTuple", (object)MyNullableTuple);
            yield return ("MyValueTupleSerialized", (object)MyValueTupleSerialized);
            yield break;
        }
        
        protected virtual void ResetAccessedProperties()
        {
            _EnumTuple_Accessed = _MyNamedTuple_Accessed = _MyNullableTuple_Accessed = _MyValueTupleSerialized_Accessed = false;
            IsDirty = false;
            DescendantIsDirty = false;
            HasChanged = false;
            DescendantHasChanged = false;
        }
        
        /* Conversion */
        
        public virtual int LazinatorUniqueID => 229;
        
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
            _EnumTuple_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            _MyNamedTuple_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            _MyNullableTuple_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            _MyValueTupleSerialized_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            _StructTuple_EndByteIndex = bytesSoFar;
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
            if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren && !_EnumTuple_Accessed)
            {
                var deserialized = EnumTuple;
            }
            WriteNonLazinatorObject(
            nonLazinatorObject: _EnumTuple, isBelievedDirty: _EnumTuple_Accessed,
            isAccessed: _EnumTuple_Accessed, writer: ref writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorObjectBytes, _EnumTuple_ByteIndex, _EnumTuple_ByteLength, false, false, null),
            verifyCleanness: false,
            binaryWriterAction: (ref BinaryBufferWriter w, bool v) =>
            ConvertToBytes__PTestEnum_C32firstEnum_c_C32TestEnum_C32anotherEnum_p(ref w, _EnumTuple,
            includeChildrenMode, v, updateStoredBuffer));
            if (updateStoredBuffer)
            {
                _EnumTuple_ByteIndex = startOfObjectPosition - startPosition;
            }
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren && !_MyNamedTuple_Accessed)
            {
                var deserialized = MyNamedTuple;
            }
            WriteNonLazinatorObject(
            nonLazinatorObject: _MyNamedTuple, isBelievedDirty: _MyNamedTuple_Accessed,
            isAccessed: _MyNamedTuple_Accessed, writer: ref writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorObjectBytes, _MyNamedTuple_ByteIndex, _MyNamedTuple_ByteLength, false, false, null),
            verifyCleanness: false,
            binaryWriterAction: (ref BinaryBufferWriter w, bool v) =>
            ConvertToBytes__Pint_C32MyFirstItem_c_C32double_C32MySecondItem_p(ref w, _MyNamedTuple,
            includeChildrenMode, v, updateStoredBuffer));
            if (updateStoredBuffer)
            {
                _MyNamedTuple_ByteIndex = startOfObjectPosition - startPosition;
            }
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren && !_MyNullableTuple_Accessed)
            {
                var deserialized = MyNullableTuple;
            }
            WriteNonLazinatorObject(
            nonLazinatorObject: _MyNullableTuple, isBelievedDirty: _MyNullableTuple_Accessed,
            isAccessed: _MyNullableTuple_Accessed, writer: ref writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorObjectBytes, _MyNullableTuple_ByteIndex, _MyNullableTuple_ByteLength, false, false, null),
            verifyCleanness: false,
            binaryWriterAction: (ref BinaryBufferWriter w, bool v) =>
            ConvertToBytes__Pint_c_C32double_p_C63(ref w, _MyNullableTuple,
            includeChildrenMode, v, updateStoredBuffer));
            if (updateStoredBuffer)
            {
                _MyNullableTuple_ByteIndex = startOfObjectPosition - startPosition;
            }
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren && !_MyValueTupleSerialized_Accessed)
            {
                var deserialized = MyValueTupleSerialized;
            }
            WriteNonLazinatorObject(
            nonLazinatorObject: _MyValueTupleSerialized, isBelievedDirty: _MyValueTupleSerialized_Accessed,
            isAccessed: _MyValueTupleSerialized_Accessed, writer: ref writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorObjectBytes, _MyValueTupleSerialized_ByteIndex, _MyValueTupleSerialized_ByteLength, false, false, null),
            verifyCleanness: false,
            binaryWriterAction: (ref BinaryBufferWriter w, bool v) =>
            ConvertToBytes__Puint_c_C32ExampleChild_c_C32NonLazinatorClass_p(ref w, _MyValueTupleSerialized,
            includeChildrenMode, v, updateStoredBuffer));
            if (updateStoredBuffer)
            {
                _MyValueTupleSerialized_ByteIndex = startOfObjectPosition - startPosition;
            }
            if (updateStoredBuffer)
            {
                _StructTuple_EndByteIndex = writer.Position - startPosition;
            }
        }
        
        /* Conversion of supported collections and tuples */
        
        private static (TestEnum firstEnum, TestEnum anotherEnum) ConvertFromBytes__PTestEnum_C32firstEnum_c_C32TestEnum_C32anotherEnum_p(ReadOnlyMemory<byte> storage)
        {
            if (storage.Length == 0)
            {
                return default;
            }
            ReadOnlySpan<byte> span = storage.Span;
            
            int bytesSoFar = 0;
            
            TestEnum item1 = (TestEnum)span.ToDecompressedInt(ref bytesSoFar);
            
            TestEnum item2 = (TestEnum)span.ToDecompressedInt(ref bytesSoFar);
            
            var tupleType = (item1, item2);
            
            return tupleType;
        }
        
        private static void ConvertToBytes__PTestEnum_C32firstEnum_c_C32TestEnum_C32anotherEnum_p(ref BinaryBufferWriter writer, (TestEnum firstEnum, TestEnum anotherEnum) itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            
            CompressedIntegralTypes.WriteCompressedInt(ref writer, (int) itemToConvert.Item1);
            
            CompressedIntegralTypes.WriteCompressedInt(ref writer, (int) itemToConvert.Item2);
        }
        
        private static (int MyFirstItem, double MySecondItem) ConvertFromBytes__Pint_C32MyFirstItem_c_C32double_C32MySecondItem_p(ReadOnlyMemory<byte> storage)
        {
            if (storage.Length == 0)
            {
                return default;
            }
            ReadOnlySpan<byte> span = storage.Span;
            
            int bytesSoFar = 0;
            
            int item1 = span.ToDecompressedInt(ref bytesSoFar);
            
            double item2 = span.ToDouble(ref bytesSoFar);
            
            var tupleType = (item1, item2);
            
            return tupleType;
        }
        
        private static void ConvertToBytes__Pint_C32MyFirstItem_c_C32double_C32MySecondItem_p(ref BinaryBufferWriter writer, (int MyFirstItem, double MySecondItem) itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            
            CompressedIntegralTypes.WriteCompressedInt(ref writer, itemToConvert.Item1);
            
            WriteUncompressedPrimitives.WriteDouble(ref writer, itemToConvert.Item2);
        }
        
        private static (int, double)? ConvertFromBytes__Pint_c_C32double_p_C63(ReadOnlyMemory<byte> storage)
        {
            if (storage.Length == 0)
            {
                return default;
            }
            ReadOnlySpan<byte> span = storage.Span;
            
            int bytesSoFar = 0;
            
            int item1 = span.ToDecompressedInt(ref bytesSoFar);
            
            double item2 = span.ToDouble(ref bytesSoFar);
            
            var tupleType = (item1, item2);
            
            return tupleType;
        }
        
        private static void ConvertToBytes__Pint_c_C32double_p_C63(ref BinaryBufferWriter writer, (int, double)? itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            if (itemToConvert == null)
            {
                return;
            }
            
            CompressedIntegralTypes.WriteCompressedInt(ref writer, itemToConvert.Value.Item1);
            
            WriteUncompressedPrimitives.WriteDouble(ref writer, itemToConvert.Value.Item2);
        }
        
        private static (uint, ExampleChild, NonLazinatorClass) ConvertFromBytes__Puint_c_C32ExampleChild_c_C32NonLazinatorClass_p(ReadOnlyMemory<byte> storage)
        {
            if (storage.Length == 0)
            {
                return default;
            }
            ReadOnlySpan<byte> span = storage.Span;
            
            int bytesSoFar = 0;
            
            uint item1 = span.ToDecompressedUint(ref bytesSoFar);
            
            ExampleChild item2 = default;
            int lengthCollectionMember_item2 = span.ToInt32(ref bytesSoFar);
            if (lengthCollectionMember_item2 != 0)
            {
                ReadOnlyMemory<byte> childData = storage.Slice(bytesSoFar, lengthCollectionMember_item2);
                item2 = DeserializationFactory.Instance.CreateBasedOnType<ExampleChild>(childData);
            }
            bytesSoFar += lengthCollectionMember_item2;
            
            NonLazinatorClass item3 = default;
            int lengthCollectionMember_item3 = span.ToInt32(ref bytesSoFar);
            if (lengthCollectionMember_item3 != 0)
            {
                ReadOnlyMemory<byte> childData = storage.Slice(bytesSoFar, lengthCollectionMember_item3);
                item3 = NonLazinatorDirectConverter.ConvertFromBytes_NonLazinatorClass(childData);
            }
            bytesSoFar += lengthCollectionMember_item3;
            
            var tupleType = (item1, item2, item3);
            
            return tupleType;
        }
        
        private static void ConvertToBytes__Puint_c_C32ExampleChild_c_C32NonLazinatorClass_p(ref BinaryBufferWriter writer, (uint, ExampleChild, NonLazinatorClass) itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            
            CompressedIntegralTypes.WriteCompressedUint(ref writer, itemToConvert.Item1);
            
            if (itemToConvert.Item2 == null)
            {
                writer.Write((uint)0);
            }
            else
            {
                void actionItem2(ref BinaryBufferWriter w) => itemToConvert.Item2.SerializeExistingBuffer(ref w, includeChildrenMode, verifyCleanness, updateStoredBuffer);
                WriteToBinaryWithIntLengthPrefix(ref writer, actionItem2);
            };
            
            if (itemToConvert.Item3 == null)
            {
                writer.Write((uint)0);
            }
            else
            {
                void actionItem3(ref BinaryBufferWriter w) => NonLazinatorDirectConverter.ConvertToBytes_NonLazinatorClass(ref w, itemToConvert.Item3, includeChildrenMode, verifyCleanness, updateStoredBuffer);
                WriteToBinaryWithIntLengthPrefix(ref writer, actionItem3);
            }
        }
        
    }
}
