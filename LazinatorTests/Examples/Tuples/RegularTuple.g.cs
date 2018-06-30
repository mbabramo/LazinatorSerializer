//c83431fe-1589-e15c-6af3-920c19f785f1
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.180
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
    public partial class RegularTuple : ILazinator
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
            var clone = new RegularTuple()
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
        
        protected int _MyTupleSerialized_ByteIndex;
        protected int _MyTupleSerialized2_ByteIndex;
        protected int _MyTupleSerialized3_ByteIndex;
        protected int _MyTupleSerialized4_ByteIndex;
        protected virtual int _MyTupleSerialized_ByteLength => _MyTupleSerialized2_ByteIndex - _MyTupleSerialized_ByteIndex;
        protected virtual int _MyTupleSerialized2_ByteLength => _MyTupleSerialized3_ByteIndex - _MyTupleSerialized2_ByteIndex;
        protected virtual int _MyTupleSerialized3_ByteLength => _MyTupleSerialized4_ByteIndex - _MyTupleSerialized3_ByteIndex;
        private int _RegularTuple_EndByteIndex;
        protected virtual int _MyTupleSerialized4_ByteLength => _RegularTuple_EndByteIndex - _MyTupleSerialized4_ByteIndex;
        
        protected Tuple<uint, ExampleChild, NonLazinatorClass> _MyTupleSerialized;
        public Tuple<uint, ExampleChild, NonLazinatorClass> MyTupleSerialized
        {
            get
            {
                if (!_MyTupleSerialized_Accessed)
                {
                    if (_LazinatorObjectBytes.Length == 0)
                    {
                        _MyTupleSerialized = default(Tuple<uint, ExampleChild, NonLazinatorClass>);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _MyTupleSerialized_ByteIndex, _MyTupleSerialized_ByteLength, false, false, null);
                        _MyTupleSerialized = ConvertFromBytes_Tuple_Guint_c_C32ExampleChild_c_C32NonLazinatorClass_g(childData);
                    }
                    _MyTupleSerialized_Accessed = true;
                }
                IsDirty = true; 
                return _MyTupleSerialized;
            }
            set
            {
                IsDirty = true;
                DescendantIsDirty = true;
                _MyTupleSerialized = value;
                _MyTupleSerialized_Accessed = true;
            }
        }
        protected bool _MyTupleSerialized_Accessed;
        protected Tuple<uint, ExampleChild, NonLazinatorClass> _MyTupleSerialized2;
        public Tuple<uint, ExampleChild, NonLazinatorClass> MyTupleSerialized2
        {
            get
            {
                if (!_MyTupleSerialized2_Accessed)
                {
                    if (_LazinatorObjectBytes.Length == 0)
                    {
                        _MyTupleSerialized2 = default(Tuple<uint, ExampleChild, NonLazinatorClass>);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _MyTupleSerialized2_ByteIndex, _MyTupleSerialized2_ByteLength, false, false, null);
                        _MyTupleSerialized2 = ConvertFromBytes_Tuple_Guint_c_C32ExampleChild_c_C32NonLazinatorClass_g(childData);
                    }
                    _MyTupleSerialized2_Accessed = true;
                }
                IsDirty = true; 
                return _MyTupleSerialized2;
            }
            set
            {
                IsDirty = true;
                DescendantIsDirty = true;
                _MyTupleSerialized2 = value;
                _MyTupleSerialized2_Accessed = true;
            }
        }
        protected bool _MyTupleSerialized2_Accessed;
        protected Tuple<uint?, ExampleChild, NonLazinatorClass> _MyTupleSerialized3;
        public Tuple<uint?, ExampleChild, NonLazinatorClass> MyTupleSerialized3
        {
            get
            {
                if (!_MyTupleSerialized3_Accessed)
                {
                    if (_LazinatorObjectBytes.Length == 0)
                    {
                        _MyTupleSerialized3 = default(Tuple<uint?, ExampleChild, NonLazinatorClass>);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _MyTupleSerialized3_ByteIndex, _MyTupleSerialized3_ByteLength, false, false, null);
                        _MyTupleSerialized3 = ConvertFromBytes_Tuple_Guint_C63_c_C32ExampleChild_c_C32NonLazinatorClass_g(childData);
                    }
                    _MyTupleSerialized3_Accessed = true;
                }
                IsDirty = true; 
                return _MyTupleSerialized3;
            }
            set
            {
                IsDirty = true;
                DescendantIsDirty = true;
                _MyTupleSerialized3 = value;
                _MyTupleSerialized3_Accessed = true;
            }
        }
        protected bool _MyTupleSerialized3_Accessed;
        protected Tuple<int, ExampleStruct> _MyTupleSerialized4;
        public Tuple<int, ExampleStruct> MyTupleSerialized4
        {
            get
            {
                if (!_MyTupleSerialized4_Accessed)
                {
                    if (_LazinatorObjectBytes.Length == 0)
                    {
                        _MyTupleSerialized4 = default(Tuple<int, ExampleStruct>);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _MyTupleSerialized4_ByteIndex, _MyTupleSerialized4_ByteLength, false, false, null);
                        _MyTupleSerialized4 = ConvertFromBytes_Tuple_Gint_c_C32ExampleStruct_g(childData);
                    }
                    _MyTupleSerialized4_Accessed = true;
                }
                IsDirty = true; 
                return _MyTupleSerialized4;
            }
            set
            {
                IsDirty = true;
                DescendantIsDirty = true;
                _MyTupleSerialized4 = value;
                _MyTupleSerialized4_Accessed = true;
            }
        }
        protected bool _MyTupleSerialized4_Accessed;
        
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
            yield return ("MyTupleSerialized", (object)MyTupleSerialized);
            yield return ("MyTupleSerialized2", (object)MyTupleSerialized2);
            yield return ("MyTupleSerialized3", (object)MyTupleSerialized3);
            yield return ("MyTupleSerialized4", (object)MyTupleSerialized4);
            yield break;
        }
        
        protected virtual void ResetAccessedProperties()
        {
            _MyTupleSerialized_Accessed = _MyTupleSerialized2_Accessed = _MyTupleSerialized3_Accessed = _MyTupleSerialized4_Accessed = false;
            IsDirty = false;
            DescendantIsDirty = false;
            HasChanged = false;
            DescendantHasChanged = false;
        }
        
        /* Conversion */
        
        public virtual int LazinatorUniqueID => 227;
        
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
            _MyTupleSerialized_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            _MyTupleSerialized2_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            _MyTupleSerialized3_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            _MyTupleSerialized4_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            _RegularTuple_EndByteIndex = bytesSoFar;
        }
        
        public virtual void SerializeExistingBuffer(BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
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
        protected virtual void WritePropertiesIntoBuffer(BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer, bool includeUniqueID)
        {
            int startPosition = writer.Position;
            int startOfObjectPosition = 0;
            // header information
            if (includeUniqueID)
            {
                if (LazinatorGenericID.IsEmpty)
                {
                    CompressedIntegralTypes.WriteCompressedInt(writer, LazinatorUniqueID);
                }
                else
                {
                    WriteLazinatorGenericID(writer, LazinatorGenericID);
                }
            }
            CompressedIntegralTypes.WriteCompressedInt(writer, Lazinator.Support.LazinatorVersionInfo.LazinatorIntVersion);
            CompressedIntegralTypes.WriteCompressedInt(writer, LazinatorObjectVersion);
            writer.Write((byte)includeChildrenMode);
            // write properties
            startOfObjectPosition = writer.Position;
            WriteNonLazinatorObject(
            nonLazinatorObject: _MyTupleSerialized, isBelievedDirty: _MyTupleSerialized_Accessed,
            isAccessed: _MyTupleSerialized_Accessed, writer: writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorObjectBytes, _MyTupleSerialized_ByteIndex, _MyTupleSerialized_ByteLength, false, false, null),
            verifyCleanness: false,
            binaryWriterAction: (w, v) =>
            ConvertToBytes_Tuple_Guint_c_C32ExampleChild_c_C32NonLazinatorClass_g(w, _MyTupleSerialized,
            includeChildrenMode, v, updateStoredBuffer));
            if (updateStoredBuffer)
            {
                _MyTupleSerialized_ByteIndex = startOfObjectPosition - startPosition;
            }
            startOfObjectPosition = writer.Position;
            WriteNonLazinatorObject(
            nonLazinatorObject: _MyTupleSerialized2, isBelievedDirty: _MyTupleSerialized2_Accessed,
            isAccessed: _MyTupleSerialized2_Accessed, writer: writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorObjectBytes, _MyTupleSerialized2_ByteIndex, _MyTupleSerialized2_ByteLength, false, false, null),
            verifyCleanness: false,
            binaryWriterAction: (w, v) =>
            ConvertToBytes_Tuple_Guint_c_C32ExampleChild_c_C32NonLazinatorClass_g(w, _MyTupleSerialized2,
            includeChildrenMode, v, updateStoredBuffer));
            if (updateStoredBuffer)
            {
                _MyTupleSerialized2_ByteIndex = startOfObjectPosition - startPosition;
            }
            startOfObjectPosition = writer.Position;
            WriteNonLazinatorObject(
            nonLazinatorObject: _MyTupleSerialized3, isBelievedDirty: _MyTupleSerialized3_Accessed,
            isAccessed: _MyTupleSerialized3_Accessed, writer: writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorObjectBytes, _MyTupleSerialized3_ByteIndex, _MyTupleSerialized3_ByteLength, false, false, null),
            verifyCleanness: false,
            binaryWriterAction: (w, v) =>
            ConvertToBytes_Tuple_Guint_C63_c_C32ExampleChild_c_C32NonLazinatorClass_g(w, _MyTupleSerialized3,
            includeChildrenMode, v, updateStoredBuffer));
            if (updateStoredBuffer)
            {
                _MyTupleSerialized3_ByteIndex = startOfObjectPosition - startPosition;
            }
            startOfObjectPosition = writer.Position;
            WriteNonLazinatorObject(
            nonLazinatorObject: _MyTupleSerialized4, isBelievedDirty: _MyTupleSerialized4_Accessed,
            isAccessed: _MyTupleSerialized4_Accessed, writer: writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorObjectBytes, _MyTupleSerialized4_ByteIndex, _MyTupleSerialized4_ByteLength, false, false, null),
            verifyCleanness: false,
            binaryWriterAction: (w, v) =>
            ConvertToBytes_Tuple_Gint_c_C32ExampleStruct_g(w, _MyTupleSerialized4,
            includeChildrenMode, v, updateStoredBuffer));
            if (updateStoredBuffer)
            {
                _MyTupleSerialized4_ByteIndex = startOfObjectPosition - startPosition;
            }
            if (updateStoredBuffer)
            {
                _RegularTuple_EndByteIndex = writer.Position - startPosition;
            }
        }
        
        /* Conversion of supported collections and tuples */
        
        private static Tuple<uint, ExampleChild, NonLazinatorClass> ConvertFromBytes_Tuple_Guint_c_C32ExampleChild_c_C32NonLazinatorClass_g(ReadOnlyMemory<byte> storage)
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
            
            var tupleType = new Tuple<uint, ExampleChild, NonLazinatorClass>(item1, item2, item3);
            
            return tupleType;
        }
        
        private static void ConvertToBytes_Tuple_Guint_c_C32ExampleChild_c_C32NonLazinatorClass_g(BinaryBufferWriter writer, Tuple<uint, ExampleChild, NonLazinatorClass> itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            if (itemToConvert == null)
            {
                return;
            }
            
            CompressedIntegralTypes.WriteCompressedUint(writer, itemToConvert.Item1);
            
            if (itemToConvert.Item2 == null)
            {
                writer.Write((uint)0);
            }
            else
            {
                void actionItem2(BinaryBufferWriter w) => itemToConvert.Item2.SerializeExistingBuffer(writer, includeChildrenMode, verifyCleanness, updateStoredBuffer);
                WriteToBinaryWithIntLengthPrefix(writer, actionItem2);
            };
            
            if (itemToConvert.Item3 == null)
            {
                writer.Write((uint)0);
            }
            else
            {
                void actionItem3(BinaryBufferWriter w) => NonLazinatorDirectConverter.ConvertToBytes_NonLazinatorClass(writer, itemToConvert.Item3, includeChildrenMode, verifyCleanness, updateStoredBuffer);
                WriteToBinaryWithIntLengthPrefix(writer, actionItem3);
            }
        }
        
        private static Tuple<uint?, ExampleChild, NonLazinatorClass> ConvertFromBytes_Tuple_Guint_C63_c_C32ExampleChild_c_C32NonLazinatorClass_g(ReadOnlyMemory<byte> storage)
        {
            if (storage.Length == 0)
            {
                return default;
            }
            ReadOnlySpan<byte> span = storage.Span;
            
            int bytesSoFar = 0;
            
            uint? item1 = span.ToDecompressedNullableUint(ref bytesSoFar);
            
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
            
            var tupleType = new Tuple<uint?, ExampleChild, NonLazinatorClass>(item1, item2, item3);
            
            return tupleType;
        }
        
        private static void ConvertToBytes_Tuple_Guint_C63_c_C32ExampleChild_c_C32NonLazinatorClass_g(BinaryBufferWriter writer, Tuple<uint?, ExampleChild, NonLazinatorClass> itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            if (itemToConvert == null)
            {
                return;
            }
            
            CompressedIntegralTypes.WriteCompressedNullableUint(writer, itemToConvert.Item1);
            
            if (itemToConvert.Item2 == null)
            {
                writer.Write((uint)0);
            }
            else
            {
                void actionItem2(BinaryBufferWriter w) => itemToConvert.Item2.SerializeExistingBuffer(writer, includeChildrenMode, verifyCleanness, updateStoredBuffer);
                WriteToBinaryWithIntLengthPrefix(writer, actionItem2);
            };
            
            if (itemToConvert.Item3 == null)
            {
                writer.Write((uint)0);
            }
            else
            {
                void actionItem3(BinaryBufferWriter w) => NonLazinatorDirectConverter.ConvertToBytes_NonLazinatorClass(writer, itemToConvert.Item3, includeChildrenMode, verifyCleanness, updateStoredBuffer);
                WriteToBinaryWithIntLengthPrefix(writer, actionItem3);
            }
        }
        
        private static Tuple<int, ExampleStruct> ConvertFromBytes_Tuple_Gint_c_C32ExampleStruct_g(ReadOnlyMemory<byte> storage)
        {
            if (storage.Length == 0)
            {
                return default;
            }
            ReadOnlySpan<byte> span = storage.Span;
            
            int bytesSoFar = 0;
            
            int item1 = span.ToDecompressedInt(ref bytesSoFar);
            
            ExampleStruct item2 = default;
            int lengthCollectionMember_item2 = span.ToInt32(ref bytesSoFar);
            if (lengthCollectionMember_item2 != 0)
            {
                ReadOnlyMemory<byte> childData = storage.Slice(bytesSoFar, lengthCollectionMember_item2);
                item2 = new ExampleStruct()
                {
                    LazinatorObjectBytes = childData,
                };
            }
            bytesSoFar += lengthCollectionMember_item2;
            
            var tupleType = new Tuple<int, ExampleStruct>(item1, item2);
            
            return tupleType;
        }
        
        private static void ConvertToBytes_Tuple_Gint_c_C32ExampleStruct_g(BinaryBufferWriter writer, Tuple<int, ExampleStruct> itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            if (itemToConvert == null)
            {
                return;
            }
            
            CompressedIntegralTypes.WriteCompressedInt(writer, itemToConvert.Item1);
            
            void actionItem2(BinaryBufferWriter w) => itemToConvert.Item2.SerializeExistingBuffer(writer, includeChildrenMode, verifyCleanness, updateStoredBuffer);
            WriteToBinaryWithIntLengthPrefix(writer, actionItem2);
        }
        
    }
}
