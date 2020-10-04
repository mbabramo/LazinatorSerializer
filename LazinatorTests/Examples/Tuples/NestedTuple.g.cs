//bf3c18ef-62c3-d689-439d-af5de3c62413
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.393
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

#nullable disable
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
        
        /* Property definitions */
        
        protected int _MyNestedTuple_ByteIndex;
        private int _NestedTuple_EndByteIndex;
        protected virtual int _MyNestedTuple_ByteLength => _NestedTuple_EndByteIndex - _MyNestedTuple_ByteIndex;
        
        
        protected Tuple<UInt32?, (ExampleChild, (UInt32, (Int32 a, String b)?, Tuple<Int16, Int64>)), NonLazinatorClass> _MyNestedTuple;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public Tuple<UInt32?, (ExampleChild, (UInt32, (Int32 a, String b)?, Tuple<Int16, Int64>)), NonLazinatorClass> MyNestedTuple
        {
            get
            {
                if (!_MyNestedTuple_Accessed)
                {
                    Lazinate_MyNestedTuple();
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
        private void Lazinate_MyNestedTuple()
        {
            if (LazinatorMemoryStorage.Length == 0)
            {
                _MyNestedTuple = default(Tuple<UInt32?, (ExampleChild, (UInt32, (Int32 a, String b)?, Tuple<Int16, Int64>)), NonLazinatorClass>);
            }
            else
            {
                LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _MyNestedTuple_ByteIndex, _MyNestedTuple_ByteLength, false, false, null);
                _MyNestedTuple = ConvertFromBytes_Tuple_Guint_n_c_C32_PExampleChild_c_C32_Puint_c_C32_Pint_C32a_c_C32string_C32b_p_n_c_C32Tuple_Gshort_c_C32long_g_p_p_c_C32NonLazinatorClass_g(childData);
            }
            
            _MyNestedTuple_Accessed = true;
        }
        
        
        /* Serialization, deserialization, and object relationships */
        
        public NestedTuple(IncludeChildrenMode originalIncludeChildrenMode = IncludeChildrenMode.IncludeAllChildren)
        {
            OriginalIncludeChildrenMode = originalIncludeChildrenMode;
        }
        
        public NestedTuple(LazinatorMemory serializedBytes, ILazinator parent = null)
        {
            LazinatorParents = new LazinatorParentsCollection(parent);
            DeserializeLazinator(serializedBytes);
            HasChanged = false;
            DescendantHasChanged = false;
        }
        
        public virtual LazinatorParentsCollection LazinatorParents { get; set; }
        
        public virtual IncludeChildrenMode OriginalIncludeChildrenMode { get; set; }
        
        public virtual int Deserialize()
        {
            FreeInMemoryObjects();
            int bytesSoFar = 0;
            ReadOnlySpan<byte> span = LazinatorMemoryStorage.InitialMemory.Span;
            if (span.Length == 0)
            {
                return 0;
            }
            
            ReadGenericIDIfApplicable(ContainsOpenGenericParameters, LazinatorUniqueID, span, ref bytesSoFar);
            
            int lazinatorLibraryVersion = span.ToDecompressedInt32(ref bytesSoFar);
            
            int serializedVersionNumber = span.ToDecompressedInt32(ref bytesSoFar);
            
            OriginalIncludeChildrenMode = (IncludeChildrenMode)span.ToByte(ref bytesSoFar);
            
            ConvertFromBytesAfterHeader(OriginalIncludeChildrenMode, serializedVersionNumber, ref bytesSoFar);
            return bytesSoFar;
        }
        
        public virtual LazinatorMemory SerializeLazinator(IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer) 
        {
            if (LazinatorMemoryStorage.IsEmpty || includeChildrenMode != OriginalIncludeChildrenMode || (verifyCleanness || IsDirty || (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && DescendantIsDirty)))
            {
                return EncodeToNewBuffer(includeChildrenMode, verifyCleanness, updateStoredBuffer);
            }
            BinaryBufferWriter writer = new BinaryBufferWriter(LazinatorMemoryStorage.Length);
            LazinatorMemoryStorage.WriteToBinaryBuffer(ref writer);
            return writer.LazinatorMemory;
        }
        
        protected virtual LazinatorMemory EncodeToNewBuffer(IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer) 
        {
            int bufferSize = LazinatorMemoryStorage.Length == 0 ? ExpandableBytes.DefaultMinBufferSize : LazinatorMemoryStorage.Length;
            BinaryBufferWriter writer = new BinaryBufferWriter(bufferSize);
            SerializeExistingBuffer(ref writer, includeChildrenMode, verifyCleanness, updateStoredBuffer);
            return writer.LazinatorMemory;
        }
        
        public virtual ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode = IncludeChildrenMode.IncludeAllChildren, CloneBufferOptions cloneBufferOptions = CloneBufferOptions.IndependentBuffers)
        {
            NestedTuple clone;
            if (cloneBufferOptions == CloneBufferOptions.NoBuffer)
            {
                clone = new NestedTuple(includeChildrenMode);
                clone.LazinatorObjectVersion = LazinatorObjectVersion;
                clone = (NestedTuple)AssignCloneProperties(clone, includeChildrenMode);
            }
            else
            {
                LazinatorMemory bytes = EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, false, IsDirty, DescendantIsDirty, false, LazinatorMemoryStorage, false, this);
                clone = new NestedTuple(bytes);
            }
            return clone;
        }
        
        public virtual ILazinator AssignCloneProperties(ILazinator clone, IncludeChildrenMode includeChildrenMode)
        {
            clone.FreeInMemoryObjects();
            NestedTuple typedClone = (NestedTuple) clone;
            typedClone.MyNestedTuple = CloneOrChange_Tuple_Guint_n_c_C32_PExampleChild_c_C32_Puint_c_C32_Pint_C32a_c_C32string_C32b_p_n_c_C32Tuple_Gshort_c_C32long_g_p_p_c_C32NonLazinatorClass_g(MyNestedTuple, l => l?.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer), false);
            
            return typedClone;
        }
        
        public virtual bool HasChanged { get; set; }
        
        protected bool _IsDirty;
        public virtual bool IsDirty
        {
            [DebuggerStepThrough]
            get => _IsDirty|| LazinatorMemoryStorage.Length == 0;
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
        
        public virtual void UpdateStoredBuffer()
        {
            if (!IsDirty && !DescendantIsDirty && LazinatorMemoryStorage.Length > 0 && OriginalIncludeChildrenMode == IncludeChildrenMode.IncludeAllChildren)
            {
                return;
            }
            var previousBuffer = LazinatorMemoryStorage;
            if (LazinatorMemoryStorage.IsEmpty || IncludeChildrenMode.IncludeAllChildren != OriginalIncludeChildrenMode || (IsDirty || DescendantIsDirty))
            {
                LazinatorMemoryStorage = EncodeToNewBuffer(IncludeChildrenMode.IncludeAllChildren, false, true);
            }
            else
            {
                BinaryBufferWriter writer = new BinaryBufferWriter(LazinatorMemoryStorage.Length);
                LazinatorMemoryStorage.WriteToBinaryBuffer(ref writer);
                LazinatorMemoryStorage = writer.LazinatorMemory;
            }
            OriginalIncludeChildrenMode = IncludeChildrenMode.IncludeAllChildren;
            if (!LazinatorParents.Any())
            {
                previousBuffer.Dispose();
            }
        }
        
        public virtual int GetByteLength()
        {
            UpdateStoredBuffer();
            return LazinatorMemoryStorage.Length;
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
            yield break;
        }
        
        
        public virtual IEnumerable<(string propertyName, object descendant)> EnumerateNonLazinatorProperties()
        {
            yield return ("MyNestedTuple", (object)MyNestedTuple);
            yield break;
        }
        
        public virtual ILazinator ForEachLazinator(Func<ILazinator, ILazinator> changeFunc, bool exploreOnlyDeserializedChildren, bool changeThisLevel)
        {
            if ((!exploreOnlyDeserializedChildren && MyNestedTuple != null) || (_MyNestedTuple_Accessed && _MyNestedTuple != null))
            {
                _MyNestedTuple = (Tuple<UInt32?, (ExampleChild, (UInt32, (Int32 a, String b)?, Tuple<Int16, Int64>)), NonLazinatorClass>) CloneOrChange_Tuple_Guint_n_c_C32_PExampleChild_c_C32_Puint_c_C32_Pint_C32a_c_C32string_C32b_p_n_c_C32Tuple_Gshort_c_C32long_g_p_p_c_C32NonLazinatorClass_g(_MyNestedTuple, l => l?.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, true), true);
            }
            if (changeThisLevel && changeFunc != null)
            {
                return changeFunc(this);
            }
            return this;
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
        
        public virtual int LazinatorUniqueID => 1024;
        
        protected virtual bool ContainsOpenGenericParameters => false;
        public virtual LazinatorGenericIDType LazinatorGenericID => default;
        
        
        public virtual int LazinatorObjectVersion { get; set; } = 0;
        
        
        public virtual void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            ReadOnlySpan<byte> span = LazinatorMemoryStorage.InitialMemory.Span;
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
                
            }
            else
            {
                ThrowHelper.ThrowCannotUpdateStoredBuffer();
            }
            
            var newBuffer = writer.Slice(startPosition, length);
            LazinatorMemoryStorage = newBuffer;
        }
        
        protected virtual void UpdateDeserializedChildren(ref BinaryBufferWriter writer, int startPosition)
        {
            if (_MyNestedTuple_Accessed && _MyNestedTuple != null)
            {
                _MyNestedTuple = (Tuple<UInt32?, (ExampleChild, (UInt32, (Int32 a, String b)?, Tuple<Int16, Int64>)), NonLazinatorClass>) CloneOrChange_Tuple_Guint_n_c_C32_PExampleChild_c_C32_Puint_c_C32_Pint_C32a_c_C32string_C32b_p_n_c_C32Tuple_Gshort_c_C32long_g_p_p_c_C32NonLazinatorClass_g(_MyNestedTuple, l => l.RemoveBufferInHierarchy(), true);
            }
        }
        
        
        protected virtual void WritePropertiesIntoBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer, bool includeUniqueID)
        {
            int startPosition = writer.Position;
            int startOfObjectPosition = 0;
            // header information
            if (includeUniqueID)
            {
                if (!ContainsOpenGenericParameters)
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
            if ((includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode) && !_MyNestedTuple_Accessed)
            {
                var deserialized = MyNestedTuple;
            }
            WriteNonLazinatorObject(
            nonLazinatorObject: _MyNestedTuple, isBelievedDirty: _MyNestedTuple_Accessed || (includeChildrenMode != OriginalIncludeChildrenMode),
            isAccessed: _MyNestedTuple_Accessed, writer: ref writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorMemoryStorage, _MyNestedTuple_ByteIndex, _MyNestedTuple_ByteLength, false, false, null),
            verifyCleanness: false,
            binaryWriterAction: (ref BinaryBufferWriter w, bool v) =>
            ConvertToBytes_Tuple_Guint_n_c_C32_PExampleChild_c_C32_Puint_c_C32_Pint_C32a_c_C32string_C32b_p_n_c_C32Tuple_Gshort_c_C32long_g_p_p_c_C32NonLazinatorClass_g(ref w, _MyNestedTuple,
            includeChildrenMode, v, updateStoredBuffer));
            if (updateStoredBuffer)
            {
                _MyNestedTuple_ByteIndex = startOfObjectPosition - startPosition;if (_MyNestedTuple_Accessed && _MyNestedTuple != null)
                {
                    _MyNestedTuple = (Tuple<UInt32?, (ExampleChild, (UInt32, (Int32 a, String b)?, Tuple<Int16, Int64>)), NonLazinatorClass>) CloneOrChange_Tuple_Guint_n_c_C32_PExampleChild_c_C32_Puint_c_C32_Pint_C32a_c_C32string_C32b_p_n_c_C32Tuple_Gshort_c_C32long_g_p_p_c_C32NonLazinatorClass_g(_MyNestedTuple, l => l.RemoveBufferInHierarchy(), true);
                }
                
            }
            if (updateStoredBuffer)
            {
                _NestedTuple_EndByteIndex = writer.Position - startPosition;
            }
        }
        
        /* Conversion of supported collections and tuples */
        
        private static Tuple<UInt32?, (ExampleChild, (UInt32, (Int32 a, String b)?, Tuple<Int16, Int64>)), NonLazinatorClass> ConvertFromBytes_Tuple_Guint_n_c_C32_PExampleChild_c_C32_Puint_c_C32_Pint_C32a_c_C32string_C32b_p_n_c_C32Tuple_Gshort_c_C32long_g_p_p_c_C32NonLazinatorClass_g(LazinatorMemory storage)
        {
            if (storage.Length == 0)
            {
                return default;
            }
            ReadOnlySpan<byte> span = storage.InitialReadOnlyMemory.Span;
            
            int bytesSoFar = 0;
            
            uint? item1 = span.ToDecompressedNullableUInt32(ref bytesSoFar);
            
            (ExampleChild, (UInt32, (Int32 a, String b)?, Tuple<Int16, Int64>)) item2 = default((ExampleChild, (UInt32, (Int32 a, String b)?, Tuple<Int16, Int64>)));
            int lengthCollectionMember_item2 = span.ToInt32(ref bytesSoFar);
            if (lengthCollectionMember_item2 != 0)
            {
                LazinatorMemory childData = storage.Slice(bytesSoFar, lengthCollectionMember_item2);
                item2 = ConvertFromBytes__PExampleChild_c_C32_Puint_c_C32_Pint_C32a_c_C32string_C32b_p_n_c_C32Tuple_Gshort_c_C32long_g_p_p(childData);
            }
            bytesSoFar += lengthCollectionMember_item2;
            
            NonLazinatorClass item3 = default(NonLazinatorClass);
            int lengthCollectionMember_item3 = span.ToInt32(ref bytesSoFar);
            if (lengthCollectionMember_item3 != 0)
            {
                LazinatorMemory childData = storage.Slice(bytesSoFar, lengthCollectionMember_item3);
                item3 = NonLazinatorDirectConverter.ConvertFromBytes_NonLazinatorClass(childData);
            }
            bytesSoFar += lengthCollectionMember_item3;
            
            var itemToCreate = new Tuple<UInt32?, (ExampleChild, (UInt32, (Int32 a, String b)?, Tuple<Int16, Int64>)), NonLazinatorClass>(item1, item2, item3);
            
            return itemToCreate;
        }
        
        private static void ConvertToBytes_Tuple_Guint_n_c_C32_PExampleChild_c_C32_Puint_c_C32_Pint_C32a_c_C32string_C32b_p_n_c_C32Tuple_Gshort_c_C32long_g_p_p_c_C32NonLazinatorClass_g(ref BinaryBufferWriter writer, Tuple<UInt32?, (ExampleChild, (UInt32, (Int32 a, String b)?, Tuple<Int16, Int64>)), NonLazinatorClass> itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            if (itemToConvert == null)
            {
                return;
            }
            
            CompressedIntegralTypes.WriteCompressedNullableUInt(ref writer, itemToConvert.Item1);
            
            void actionItem2(ref BinaryBufferWriter w) => ConvertToBytes__PExampleChild_c_C32_Puint_c_C32_Pint_C32a_c_C32string_C32b_p_n_c_C32Tuple_Gshort_c_C32long_g_p_p(ref w, itemToConvert.Item2, includeChildrenMode, verifyCleanness, updateStoredBuffer);
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
        
        private static Tuple<UInt32?, (ExampleChild, (UInt32, (Int32 a, String b)?, Tuple<Int16, Int64>)), NonLazinatorClass> CloneOrChange_Tuple_Guint_n_c_C32_PExampleChild_c_C32_Puint_c_C32_Pint_C32a_c_C32string_C32b_p_n_c_C32Tuple_Gshort_c_C32long_g_p_p_c_C32NonLazinatorClass_g(Tuple<UInt32?, (ExampleChild, (UInt32, (Int32 a, String b)?, Tuple<Int16, Int64>)), NonLazinatorClass> itemToConvert, Func<ILazinator, ILazinator> cloneOrChangeFunc, bool avoidCloningIfPossible)
        {
            if (itemToConvert == null)
            {
                return default(Tuple<UInt32?, (ExampleChild, (UInt32, (Int32 a, String b)?, Tuple<Int16, Int64>)), NonLazinatorClass>);
            }
            
            return new Tuple<UInt32?, (ExampleChild, (UInt32, (Int32 a, String b)?, Tuple<Int16, Int64>)), NonLazinatorClass>((uint?) (itemToConvert?.Item1), ((ExampleChild, (UInt32, (Int32 a, String b)?, Tuple<Int16, Int64>))) CloneOrChange__PExampleChild_c_C32_Puint_c_C32_Pint_C32a_c_C32string_C32b_p_n_c_C32Tuple_Gshort_c_C32long_g_p_p((itemToConvert?.Item2 ?? default), cloneOrChangeFunc, avoidCloningIfPossible), (NonLazinatorClass) (itemToConvert?.Item3));
        }
        
        private static (ExampleChild, (UInt32, (Int32 a, String b)?, Tuple<Int16, Int64>)) ConvertFromBytes__PExampleChild_c_C32_Puint_c_C32_Pint_C32a_c_C32string_C32b_p_n_c_C32Tuple_Gshort_c_C32long_g_p_p(LazinatorMemory storage)
        {
            if (storage.Length == 0)
            {
                return default;
            }
            ReadOnlySpan<byte> span = storage.InitialReadOnlyMemory.Span;
            
            int bytesSoFar = 0;
            
            ExampleChild item1 = default(ExampleChild);
            int lengthCollectionMember_item1 = span.ToInt32(ref bytesSoFar);
            if (lengthCollectionMember_item1 != 0)
            {
                LazinatorMemory childData = storage.Slice(bytesSoFar, lengthCollectionMember_item1);
                item1 = DeserializationFactory.Instance.CreateBasedOnType<ExampleChild>(childData);
            }
            bytesSoFar += lengthCollectionMember_item1;
            
            (UInt32, (Int32 a, String b)?, Tuple<Int16, Int64>) item2 = default((UInt32, (Int32 a, String b)?, Tuple<Int16, Int64>));
            int lengthCollectionMember_item2 = span.ToInt32(ref bytesSoFar);
            if (lengthCollectionMember_item2 != 0)
            {
                LazinatorMemory childData = storage.Slice(bytesSoFar, lengthCollectionMember_item2);
                item2 = ConvertFromBytes__Puint_c_C32_Pint_C32a_c_C32string_C32b_p_n_c_C32Tuple_Gshort_c_C32long_g_p(childData);
            }
            bytesSoFar += lengthCollectionMember_item2;
            
            var itemToCreate = (item1, item2);
            
            return itemToCreate;
        }
        
        private static void ConvertToBytes__PExampleChild_c_C32_Puint_c_C32_Pint_C32a_c_C32string_C32b_p_n_c_C32Tuple_Gshort_c_C32long_g_p_p(ref BinaryBufferWriter writer, (ExampleChild, (UInt32, (Int32 a, String b)?, Tuple<Int16, Int64>)) itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
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
            
            void actionItem2(ref BinaryBufferWriter w) => ConvertToBytes__Puint_c_C32_Pint_C32a_c_C32string_C32b_p_n_c_C32Tuple_Gshort_c_C32long_g_p(ref w, itemToConvert.Item2, includeChildrenMode, verifyCleanness, updateStoredBuffer);
            WriteToBinaryWithIntLengthPrefix(ref writer, actionItem2);
        }
        
        private static (ExampleChild, (UInt32, (Int32 a, String b)?, Tuple<Int16, Int64>)) CloneOrChange__PExampleChild_c_C32_Puint_c_C32_Pint_C32a_c_C32string_C32b_p_n_c_C32Tuple_Gshort_c_C32long_g_p_p((ExampleChild, (UInt32, (Int32 a, String b)?, Tuple<Int16, Int64>)) itemToConvert, Func<ILazinator, ILazinator> cloneOrChangeFunc, bool avoidCloningIfPossible)
        {
            return ((ExampleChild) (cloneOrChangeFunc((itemToConvert.Item1))), ((UInt32, (Int32 a, String b)?, Tuple<Int16, Int64>)) CloneOrChange__Puint_c_C32_Pint_C32a_c_C32string_C32b_p_n_c_C32Tuple_Gshort_c_C32long_g_p((itemToConvert.Item2), cloneOrChangeFunc, avoidCloningIfPossible));
        }
        
        private static (UInt32, (Int32 a, String b)?, Tuple<Int16, Int64>) ConvertFromBytes__Puint_c_C32_Pint_C32a_c_C32string_C32b_p_n_c_C32Tuple_Gshort_c_C32long_g_p(LazinatorMemory storage)
        {
            if (storage.Length == 0)
            {
                return default;
            }
            ReadOnlySpan<byte> span = storage.InitialReadOnlyMemory.Span;
            
            int bytesSoFar = 0;
            
            uint item1 = span.ToDecompressedUInt32(ref bytesSoFar);
            
            (Int32 a, String b)? item2 = default((Int32 a, String b)?);
            int lengthCollectionMember_item2 = span.ToInt32(ref bytesSoFar);
            if (lengthCollectionMember_item2 != 0)
            {
                LazinatorMemory childData = storage.Slice(bytesSoFar, lengthCollectionMember_item2);
                item2 = ConvertFromBytes__Pint_C32a_c_C32string_C32b_p_n(childData);
            }
            bytesSoFar += lengthCollectionMember_item2;
            
            Tuple<Int16, Int64> item3 = default(Tuple<Int16, Int64>);
            int lengthCollectionMember_item3 = span.ToInt32(ref bytesSoFar);
            if (lengthCollectionMember_item3 != 0)
            {
                LazinatorMemory childData = storage.Slice(bytesSoFar, lengthCollectionMember_item3);
                item3 = ConvertFromBytes_Tuple_Gshort_c_C32long_g(childData);
            }
            bytesSoFar += lengthCollectionMember_item3;
            
            var itemToCreate = (item1, item2, item3);
            
            return itemToCreate;
        }
        
        private static void ConvertToBytes__Puint_c_C32_Pint_C32a_c_C32string_C32b_p_n_c_C32Tuple_Gshort_c_C32long_g_p(ref BinaryBufferWriter writer, (UInt32, (Int32 a, String b)?, Tuple<Int16, Int64>) itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            
            CompressedIntegralTypes.WriteCompressedUInt(ref writer, itemToConvert.Item1);
            
            if (itemToConvert.Item2 == null)
            {
                writer.Write((uint)0);
            }
            else
            {
                void actionItem2(ref BinaryBufferWriter w) => ConvertToBytes__Pint_C32a_c_C32string_C32b_p_n(ref w, itemToConvert.Item2, includeChildrenMode, verifyCleanness, updateStoredBuffer);
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
        
        private static (UInt32, (Int32 a, String b)?, Tuple<Int16, Int64>) CloneOrChange__Puint_c_C32_Pint_C32a_c_C32string_C32b_p_n_c_C32Tuple_Gshort_c_C32long_g_p((UInt32, (Int32 a, String b)?, Tuple<Int16, Int64>) itemToConvert, Func<ILazinator, ILazinator> cloneOrChangeFunc, bool avoidCloningIfPossible)
        {
            return ((uint) (itemToConvert.Item1), ((Int32 a, String b)?) CloneOrChange__Pint_C32a_c_C32string_C32b_p_n((itemToConvert.Item2), cloneOrChangeFunc, avoidCloningIfPossible), (Tuple<Int16, Int64>) CloneOrChange_Tuple_Gshort_c_C32long_g((itemToConvert.Item3), cloneOrChangeFunc, avoidCloningIfPossible));
        }
        
        private static (Int32 a, String b)? ConvertFromBytes__Pint_C32a_c_C32string_C32b_p_n(LazinatorMemory storage)
        {
            if (storage.Length == 0)
            {
                return default;
            }
            ReadOnlySpan<byte> span = storage.InitialReadOnlyMemory.Span;
            
            int bytesSoFar = 0;
            
            int item1 = span.ToDecompressedInt32(ref bytesSoFar);
            
            string item2 = span.ToString_BrotliCompressedWithLength(ref bytesSoFar);
            
            var itemToCreate = (item1, item2);
            
            return itemToCreate;
        }
        
        private static void ConvertToBytes__Pint_C32a_c_C32string_C32b_p_n(ref BinaryBufferWriter writer, (Int32 a, String b)? itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            if (itemToConvert == null)
            {
                return;
            }
            
            CompressedIntegralTypes.WriteCompressedInt(ref writer, itemToConvert.Value.Item1);
            
            EncodeCharAndString.WriteBrotliCompressedWithIntPrefix(ref writer, itemToConvert.Value.Item2);
        }
        
        private static (Int32 a, String b)? CloneOrChange__Pint_C32a_c_C32string_C32b_p_n((Int32 a, String b)? itemToConvert, Func<ILazinator, ILazinator> cloneOrChangeFunc, bool avoidCloningIfPossible)
        {
            if (itemToConvert == null)
            {
                return default((Int32 a, String b)?);
            }
            
            return ((int) (itemToConvert?.Item1 ?? default), (string) (itemToConvert?.Item2));
        }
        
        private static Tuple<Int16, Int64> ConvertFromBytes_Tuple_Gshort_c_C32long_g(LazinatorMemory storage)
        {
            if (storage.Length == 0)
            {
                return default;
            }
            ReadOnlySpan<byte> span = storage.InitialReadOnlyMemory.Span;
            
            int bytesSoFar = 0;
            
            short item1 = span.ToDecompressedInt16(ref bytesSoFar);
            
            long item2 = span.ToDecompressedInt64(ref bytesSoFar);
            
            var itemToCreate = new Tuple<Int16, Int64>(item1, item2);
            
            return itemToCreate;
        }
        
        private static void ConvertToBytes_Tuple_Gshort_c_C32long_g(ref BinaryBufferWriter writer, Tuple<Int16, Int64> itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            if (itemToConvert == null)
            {
                return;
            }
            
            CompressedIntegralTypes.WriteCompressedShort(ref writer, itemToConvert.Item1);
            
            CompressedIntegralTypes.WriteCompressedLong(ref writer, itemToConvert.Item2);
        }
        
        private static Tuple<Int16, Int64> CloneOrChange_Tuple_Gshort_c_C32long_g(Tuple<Int16, Int64> itemToConvert, Func<ILazinator, ILazinator> cloneOrChangeFunc, bool avoidCloningIfPossible)
        {
            if (itemToConvert == null)
            {
                return default(Tuple<Int16, Int64>);
            }
            
            return new Tuple<Int16, Int64>((short) (itemToConvert?.Item1 ?? default), (long) (itemToConvert?.Item2 ?? default));
        }
        
    }
}
