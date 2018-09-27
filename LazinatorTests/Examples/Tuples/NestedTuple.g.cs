//2ef644ea-dfe8-8ad5-572d-a69928143713
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.222
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
    public partial class NestedTuple : ILazinator
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
        
        public virtual ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode = IncludeChildrenMode.IncludeAllChildren, CloneBufferOptions cloneBufferOptions = CloneBufferOptions.LinkedBuffer)
        {
            var clone = new NestedTuple()
            {
                OriginalIncludeChildrenMode = includeChildrenMode
            };
            if (clone.LazinatorObjectVersion != LazinatorObjectVersion)
            {
                clone.LazinatorObjectVersion = LazinatorObjectVersion;
            }
            
            if (cloneBufferOptions == CloneBufferOptions.NoBuffer)
            {
                AssignCloneProperties(clone, includeChildrenMode);
            }
            else
            {
                LazinatorMemory bytes = EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, false, IsDirty, DescendantIsDirty, false, LazinatorMemoryStorage, (EncodeManuallyDelegate)EncodeToNewBuffer, cloneBufferOptions == CloneBufferOptions.SharedBuffer);
                clone.DeserializeLazinator(bytes);
                if (cloneBufferOptions == CloneBufferOptions.IndependentBuffers)
                {
                    clone.LazinatorMemoryStorage.DisposeIndependently();
                }
            }
            clone.LazinatorParents = default;
            return clone;
        }
        
        protected virtual void AssignCloneProperties(ILazinator clone, IncludeChildrenMode includeChildrenMode)
        {
            NestedTuple typedClone = (NestedTuple) clone;
            typedClone.MyNestedTuple = Clone_Tuple_Guint_C63_c_C32_PExampleChild_c_C32_Puint_c_C32_Pint_C32a_c_C32string_C32b_p_C63_c_C32Tuple_Gshort_c_C32long_g_p_p_c_C32NonLazinatorClass_g(MyNestedTuple);
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
            if (!IsDirty && !DescendantIsDirty && LazinatorObjectBytes.Length > 0 && OriginalIncludeChildrenMode == IncludeChildrenMode.IncludeAllChildren)
            {
                return;
            }
            EncodeOrRecycleToNewBuffer(IncludeChildrenMode.IncludeAllChildren, OriginalIncludeChildrenMode, false, IsDirty, DescendantIsDirty, false, LazinatorMemoryStorage, (EncodeManuallyDelegate)EncodeToNewBuffer, true);
            OriginalIncludeChildrenMode = IncludeChildrenMode.IncludeAllChildren;
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
        
        protected int _MyNestedTuple_ByteIndex;
        private int _NestedTuple_EndByteIndex;
        protected virtual int _MyNestedTuple_ByteLength => _NestedTuple_EndByteIndex - _MyNestedTuple_ByteIndex;
        
        protected Tuple<uint?, (ExampleChild, (uint, (int a, string b)?, Tuple<short, long>)), NonLazinatorClass> _MyNestedTuple;
        public Tuple<uint?, (ExampleChild, (uint, (int a, string b)?, Tuple<short, long>)), NonLazinatorClass> MyNestedTuple
        {
            get
            {
                if (!_MyNestedTuple_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _MyNestedTuple = default(Tuple<uint?, (ExampleChild, (uint, (int a, string b)?, Tuple<short, long>)), NonLazinatorClass>);
                    }
                    else
                    {
                        LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _MyNestedTuple_ByteIndex, _MyNestedTuple_ByteLength, false, false, null);
                        _MyNestedTuple = ConvertFromBytes_Tuple_Guint_C63_c_C32_PExampleChild_c_C32_Puint_c_C32_Pint_C32a_c_C32string_C32b_p_C63_c_C32Tuple_Gshort_c_C32long_g_p_p_c_C32NonLazinatorClass_g(childData);
                    }
                    _MyNestedTuple_Accessed = true;
                }
                IsDirty = true; 
                return _MyNestedTuple;
            }
            set
            {
                IsDirty = true;
                DescendantIsDirty = true;
                _MyNestedTuple = value;
                _MyNestedTuple_Accessed = true;
            }
        }
        protected bool _MyNestedTuple_Accessed;
        
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
            yield return ("MyNestedTuple", (object)MyNestedTuple);
            yield break;
        }
        
        public virtual void FreeInMemoryObjects()
        {
            _MyNestedTuple = default;
            _MyNestedTuple_Accessed = false;
            IsDirty = false;
            DescendantIsDirty = false;
            HasChanged = false;
            DescendantHasChanged = false;
        }
        
        /* Conversion */
        
        public virtual int LazinatorUniqueID => 224;
        
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
            _MyNestedTuple_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            _NestedTuple_EndByteIndex = bytesSoFar;
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
            if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren && !_MyNestedTuple_Accessed)
            {
                var deserialized = MyNestedTuple;
            }
            WriteNonLazinatorObject(
            nonLazinatorObject: _MyNestedTuple, isBelievedDirty: _MyNestedTuple_Accessed || (includeChildrenMode != OriginalIncludeChildrenMode),
            isAccessed: _MyNestedTuple_Accessed, writer: ref writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorMemoryStorage, _MyNestedTuple_ByteIndex, _MyNestedTuple_ByteLength, false, false, null),
            verifyCleanness: false,
            binaryWriterAction: (ref BinaryBufferWriter w, bool v) =>
            ConvertToBytes_Tuple_Guint_C63_c_C32_PExampleChild_c_C32_Puint_c_C32_Pint_C32a_c_C32string_C32b_p_C63_c_C32Tuple_Gshort_c_C32long_g_p_p_c_C32NonLazinatorClass_g(ref w, _MyNestedTuple,
            includeChildrenMode, v, updateStoredBuffer));
            if (updateStoredBuffer)
            {
                _MyNestedTuple_ByteIndex = startOfObjectPosition - startPosition;
            }
            if (updateStoredBuffer)
            {
                _NestedTuple_EndByteIndex = writer.Position - startPosition;
            }
        }
        
        /* Conversion of supported collections and tuples */
        
        private static Tuple<uint?, (ExampleChild, (uint, (int a, string b)?, Tuple<short, long>)), NonLazinatorClass> ConvertFromBytes_Tuple_Guint_C63_c_C32_PExampleChild_c_C32_Puint_c_C32_Pint_C32a_c_C32string_C32b_p_C63_c_C32Tuple_Gshort_c_C32long_g_p_p_c_C32NonLazinatorClass_g(LazinatorMemory storage)
        {
            if (storage.Length == 0)
            {
                return default;
            }
            ReadOnlySpan<byte> span = storage.ReadOnlySpan;
            
            int bytesSoFar = 0;
            
            uint? item1 = span.ToDecompressedNullableUint(ref bytesSoFar);
            
            (ExampleChild, (uint, (int a, string b)?, Tuple<short, long>)) item2 = default;
            int lengthCollectionMember_item2 = span.ToInt32(ref bytesSoFar);
            if (lengthCollectionMember_item2 != 0)
            {
                LazinatorMemory childData = storage.Slice(bytesSoFar, lengthCollectionMember_item2);
                item2 = ConvertFromBytes__PExampleChild_c_C32_Puint_c_C32_Pint_C32a_c_C32string_C32b_p_C63_c_C32Tuple_Gshort_c_C32long_g_p_p(childData);
            }
            bytesSoFar += lengthCollectionMember_item2;
            
            NonLazinatorClass item3 = default;
            int lengthCollectionMember_item3 = span.ToInt32(ref bytesSoFar);
            if (lengthCollectionMember_item3 != 0)
            {
                LazinatorMemory childData = storage.Slice(bytesSoFar, lengthCollectionMember_item3);
                item3 = NonLazinatorDirectConverter.ConvertFromBytes_NonLazinatorClass(childData);
            }
            bytesSoFar += lengthCollectionMember_item3;
            
            var tupleType = new Tuple<uint?, (ExampleChild, (uint, (int a, string b)?, Tuple<short, long>)), NonLazinatorClass>(item1, item2, item3);
            
            return tupleType;
        }
        
        private static void ConvertToBytes_Tuple_Guint_C63_c_C32_PExampleChild_c_C32_Puint_c_C32_Pint_C32a_c_C32string_C32b_p_C63_c_C32Tuple_Gshort_c_C32long_g_p_p_c_C32NonLazinatorClass_g(ref BinaryBufferWriter writer, Tuple<uint?, (ExampleChild, (uint, (int a, string b)?, Tuple<short, long>)), NonLazinatorClass> itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            if (itemToConvert == null)
            {
                return;
            }
            
            CompressedIntegralTypes.WriteCompressedNullableUint(ref writer, itemToConvert.Item1);
            
            void actionItem2(ref BinaryBufferWriter w) => ConvertToBytes__PExampleChild_c_C32_Puint_c_C32_Pint_C32a_c_C32string_C32b_p_C63_c_C32Tuple_Gshort_c_C32long_g_p_p(ref w, itemToConvert.Item2, includeChildrenMode, verifyCleanness, updateStoredBuffer);
            WriteToBinaryWithIntLengthPrefix(ref writer, actionItem2);
            
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
        
        private static Tuple<uint?, (ExampleChild, (uint, (int a, string b)?, Tuple<short, long>)), NonLazinatorClass> Clone_Tuple_Guint_C63_c_C32_PExampleChild_c_C32_Puint_c_C32_Pint_C32a_c_C32string_C32b_p_C63_c_C32Tuple_Gshort_c_C32long_g_p_p_c_C32NonLazinatorClass_g(Tuple<uint?, (ExampleChild, (uint, (int a, string b)?, Tuple<short, long>)), NonLazinatorClass> itemToConvert)
        {
            return new Tuple<uint?, (ExampleChild, (uint, (int a, string b)?, Tuple<short, long>)), NonLazinatorClass>((uint?) (itemToConvert?.Item1),((ExampleChild, (uint, (int a, string b)?, Tuple<short, long>))) Clone__PExampleChild_c_C32_Puint_c_C32_Pint_C32a_c_C32string_C32b_p_C63_c_C32Tuple_Gshort_c_C32long_g_p_p((itemToConvert?.Item2 ?? default)),(NonLazinatorClass) (itemToConvert?.Item3));
        }
        
        private static (ExampleChild, (uint, (int a, string b)?, Tuple<short, long>)) ConvertFromBytes__PExampleChild_c_C32_Puint_c_C32_Pint_C32a_c_C32string_C32b_p_C63_c_C32Tuple_Gshort_c_C32long_g_p_p(LazinatorMemory storage)
        {
            if (storage.Length == 0)
            {
                return default;
            }
            ReadOnlySpan<byte> span = storage.ReadOnlySpan;
            
            int bytesSoFar = 0;
            
            ExampleChild item1 = default;
            int lengthCollectionMember_item1 = span.ToInt32(ref bytesSoFar);
            if (lengthCollectionMember_item1 != 0)
            {
                LazinatorMemory childData = storage.Slice(bytesSoFar, lengthCollectionMember_item1);
                item1 = DeserializationFactory.Instance.CreateBasedOnType<ExampleChild>(childData);
            }
            bytesSoFar += lengthCollectionMember_item1;
            
            (uint, (int a, string b)?, Tuple<short, long>) item2 = default;
            int lengthCollectionMember_item2 = span.ToInt32(ref bytesSoFar);
            if (lengthCollectionMember_item2 != 0)
            {
                LazinatorMemory childData = storage.Slice(bytesSoFar, lengthCollectionMember_item2);
                item2 = ConvertFromBytes__Puint_c_C32_Pint_C32a_c_C32string_C32b_p_C63_c_C32Tuple_Gshort_c_C32long_g_p(childData);
            }
            bytesSoFar += lengthCollectionMember_item2;
            
            var tupleType = (item1, item2);
            
            return tupleType;
        }
        
        private static void ConvertToBytes__PExampleChild_c_C32_Puint_c_C32_Pint_C32a_c_C32string_C32b_p_C63_c_C32Tuple_Gshort_c_C32long_g_p_p(ref BinaryBufferWriter writer, (ExampleChild, (uint, (int a, string b)?, Tuple<short, long>)) itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            
            if (itemToConvert.Item1 == null)
            {
                writer.Write((uint)0);
            }
            else
            {
                void actionItem1(ref BinaryBufferWriter w) => itemToConvert.Item1.SerializeExistingBuffer(ref w, includeChildrenMode, verifyCleanness, updateStoredBuffer);
                WriteToBinaryWithIntLengthPrefix(ref writer, actionItem1);
            };
            
            void actionItem2(ref BinaryBufferWriter w) => ConvertToBytes__Puint_c_C32_Pint_C32a_c_C32string_C32b_p_C63_c_C32Tuple_Gshort_c_C32long_g_p(ref w, itemToConvert.Item2, includeChildrenMode, verifyCleanness, updateStoredBuffer);
            WriteToBinaryWithIntLengthPrefix(ref writer, actionItem2);
        }
        
        private static (ExampleChild, (uint, (int a, string b)?, Tuple<short, long>)) Clone__PExampleChild_c_C32_Puint_c_C32_Pint_C32a_c_C32string_C32b_p_C63_c_C32Tuple_Gshort_c_C32long_g_p_p((ExampleChild, (uint, (int a, string b)?, Tuple<short, long>)) itemToConvert)
        {
            return ((ExampleChild) (itemToConvert.Item1)?.CloneLazinator(IncludeChildrenMode.IncludeAllChildren, CloneBufferOptions.NoBuffer),((uint, (int a, string b)?, Tuple<short, long>)) Clone__Puint_c_C32_Pint_C32a_c_C32string_C32b_p_C63_c_C32Tuple_Gshort_c_C32long_g_p((itemToConvert.Item2)));
        }
        
        private static (uint, (int a, string b)?, Tuple<short, long>) ConvertFromBytes__Puint_c_C32_Pint_C32a_c_C32string_C32b_p_C63_c_C32Tuple_Gshort_c_C32long_g_p(LazinatorMemory storage)
        {
            if (storage.Length == 0)
            {
                return default;
            }
            ReadOnlySpan<byte> span = storage.ReadOnlySpan;
            
            int bytesSoFar = 0;
            
            uint item1 = span.ToDecompressedUint(ref bytesSoFar);
            
            (int a, string b)? item2 = default;
            int lengthCollectionMember_item2 = span.ToInt32(ref bytesSoFar);
            if (lengthCollectionMember_item2 != 0)
            {
                LazinatorMemory childData = storage.Slice(bytesSoFar, lengthCollectionMember_item2);
                item2 = ConvertFromBytes__Pint_C32a_c_C32string_C32b_p_C63(childData);
            }
            bytesSoFar += lengthCollectionMember_item2;
            
            Tuple<short, long> item3 = default;
            int lengthCollectionMember_item3 = span.ToInt32(ref bytesSoFar);
            if (lengthCollectionMember_item3 != 0)
            {
                LazinatorMemory childData = storage.Slice(bytesSoFar, lengthCollectionMember_item3);
                item3 = ConvertFromBytes_Tuple_Gshort_c_C32long_g(childData);
            }
            bytesSoFar += lengthCollectionMember_item3;
            
            var tupleType = (item1, item2, item3);
            
            return tupleType;
        }
        
        private static void ConvertToBytes__Puint_c_C32_Pint_C32a_c_C32string_C32b_p_C63_c_C32Tuple_Gshort_c_C32long_g_p(ref BinaryBufferWriter writer, (uint, (int a, string b)?, Tuple<short, long>) itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            
            CompressedIntegralTypes.WriteCompressedUint(ref writer, itemToConvert.Item1);
            
            if (itemToConvert.Item2 == null)
            {
                writer.Write((uint)0);
            }
            else
            {
                void actionItem2(ref BinaryBufferWriter w) => ConvertToBytes__Pint_C32a_c_C32string_C32b_p_C63(ref w, itemToConvert.Item2, includeChildrenMode, verifyCleanness, updateStoredBuffer);
                WriteToBinaryWithIntLengthPrefix(ref writer, actionItem2);
            }
            
            if (itemToConvert.Item3 == null)
            {
                writer.Write((uint)0);
            }
            else
            {
                void actionItem3(ref BinaryBufferWriter w) => ConvertToBytes_Tuple_Gshort_c_C32long_g(ref w, itemToConvert.Item3, includeChildrenMode, verifyCleanness, updateStoredBuffer);
                WriteToBinaryWithIntLengthPrefix(ref writer, actionItem3);
            }
        }
        
        private static (uint, (int a, string b)?, Tuple<short, long>) Clone__Puint_c_C32_Pint_C32a_c_C32string_C32b_p_C63_c_C32Tuple_Gshort_c_C32long_g_p((uint, (int a, string b)?, Tuple<short, long>) itemToConvert)
        {
            return ((uint) (itemToConvert.Item1),((int a, string b)?) Clone__Pint_C32a_c_C32string_C32b_p_C63((itemToConvert.Item2)),(Tuple<short, long>) Clone_Tuple_Gshort_c_C32long_g((itemToConvert.Item3)));
        }
        
        private static (int a, string b)? ConvertFromBytes__Pint_C32a_c_C32string_C32b_p_C63(LazinatorMemory storage)
        {
            if (storage.Length == 0)
            {
                return default;
            }
            ReadOnlySpan<byte> span = storage.ReadOnlySpan;
            
            int bytesSoFar = 0;
            
            int item1 = span.ToDecompressedInt(ref bytesSoFar);
            
            string item2 = span.ToString_VarIntLengthUtf8(ref bytesSoFar);
            
            var tupleType = (item1, item2);
            
            return tupleType;
        }
        
        private static void ConvertToBytes__Pint_C32a_c_C32string_C32b_p_C63(ref BinaryBufferWriter writer, (int a, string b)? itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            if (itemToConvert == null)
            {
                return;
            }
            
            CompressedIntegralTypes.WriteCompressedInt(ref writer, itemToConvert.Value.Item1);
            
            EncodeCharAndString.WriteStringUtf8WithVarIntPrefix(ref writer, itemToConvert.Value.Item2);
        }
        
        private static (int a, string b)? Clone__Pint_C32a_c_C32string_C32b_p_C63((int a, string b)? itemToConvert)
        {
            return ((int) (itemToConvert?.Item1 ?? default),(string) (itemToConvert?.Item2 ?? default));
        }
        
        private static Tuple<short, long> ConvertFromBytes_Tuple_Gshort_c_C32long_g(LazinatorMemory storage)
        {
            if (storage.Length == 0)
            {
                return default;
            }
            ReadOnlySpan<byte> span = storage.ReadOnlySpan;
            
            int bytesSoFar = 0;
            
            short item1 = span.ToDecompressedShort(ref bytesSoFar);
            
            long item2 = span.ToDecompressedLong(ref bytesSoFar);
            
            var tupleType = new Tuple<short, long>(item1, item2);
            
            return tupleType;
        }
        
        private static void ConvertToBytes_Tuple_Gshort_c_C32long_g(ref BinaryBufferWriter writer, Tuple<short, long> itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            if (itemToConvert == null)
            {
                return;
            }
            
            CompressedIntegralTypes.WriteCompressedShort(ref writer, itemToConvert.Item1);
            
            CompressedIntegralTypes.WriteCompressedLong(ref writer, itemToConvert.Item2);
        }
        
        private static Tuple<short, long> Clone_Tuple_Gshort_c_C32long_g(Tuple<short, long> itemToConvert)
        {
            return new Tuple<short, long>((short) (itemToConvert?.Item1 ?? default),(long) (itemToConvert?.Item2 ?? default));
        }
        
    }
}
