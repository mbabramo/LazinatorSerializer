//d0f7071e-2120-4b42-4b16-14301058d81c
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.72
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LazinatorTests.Examples.NonAbstractGenerics
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
    public partial class ClosedGeneric : OpenGeneric<ExampleChild>, ILazinator
    {
        /* Clone overrides */
        
        public ClosedGeneric() : base()
        {
        }
        
        public override ILazinator CloneLazinator()
        {
            return CloneLazinator(OriginalIncludeChildrenMode);
        }
        
        public override ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode)
        {
            MemoryInBuffer bytes = EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, false, false, IsDirty, DescendantIsDirty, false, LazinatorObjectBytes, (StreamManuallyDelegate)EncodeToNewBuffer);
            var clone = new ClosedGeneric()
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
        protected override int _MyListT_ByteLength => _MyT_ByteIndex - _MyListT_ByteIndex;
        private int _ClosedGeneric_EndByteIndex = 0;
        protected override int _MyT_ByteLength => _ClosedGeneric_EndByteIndex - _MyT_ByteIndex;
        
        private int _AnotherPropertyAdded;
        public int AnotherPropertyAdded
        {
            [DebuggerStepThrough]
            get
            {
                return _AnotherPropertyAdded;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _AnotherPropertyAdded = value;
            }
        }
        private List<ExampleChild> _MyListT;
        public override List<ExampleChild> MyListT
        {
            [DebuggerStepThrough]
            get
            {
                if (!_MyListT_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _MyListT = default(List<ExampleChild>);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _MyListT_ByteIndex, _MyListT_ByteLength);
                        _MyListT = ConvertFromBytes_List_GExampleChild_g(childData, DeserializationFactory, null);
                    }
                    _MyListT_Accessed = true;
                    IsDirty = true;
                }
                return _MyListT;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _MyListT = value;
                _MyListT_Accessed = true;
            }
        }
        private ExampleChild _MyT;
        public override ExampleChild MyT
        {
            [DebuggerStepThrough]
            get
            {
                if (!_MyT_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _MyT = default(ExampleChild);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _MyT_ByteIndex, _MyT_ByteLength);
                        
                        if (DeserializationFactory == null)
                        {
                            DeserializationFactory = DeserializationFactory.GetInstance();
                        }
                        _MyT = DeserializationFactory.FactoryCreateBaseOrDerivedType(213, () => new ExampleChild(), childData, this); 
                    }
                    _MyT_Accessed = true;
                }
                return _MyT;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _MyT = value;
                if (_MyT != null)
                {
                    _MyT.IsDirty = true;
                }
                _MyT_Accessed = true;
            }
        }
        
        protected override void ResetAccessedProperties()
        {
            base.ResetAccessedProperties();
            _MyListT_Accessed = _MyT_Accessed = false;
        }
        
        /* Conversion */
        
        public override int LazinatorUniqueID => 250;
        
        public override int LazinatorObjectVersion { get; set; } = 0;
        
        public override void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            base.ConvertFromBytesAfterHeader(OriginalIncludeChildrenMode, serializedVersionNumber, ref bytesSoFar);
            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
            _AnotherPropertyAdded = span.ToDecompressedInt(ref bytesSoFar);
            _MyListT_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            _MyT_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            _ClosedGeneric_EndByteIndex = bytesSoFar;
        }
        
        public override void SerializeExistingBuffer(BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
            int startPosition = writer.Position;
            WritePropertiesIntoBuffer(writer, includeChildrenMode, verifyCleanness, true);
            
            _IsDirty = false;
            _DescendantIsDirty = includeChildrenMode != IncludeChildrenMode.IncludeAllChildren && ((_MyT_Accessed && MyT != null && (MyT.IsDirty || MyT.DescendantIsDirty)));
            
            _LazinatorObjectBytes = writer.Slice(startPosition);
        }
        
        protected override void WritePropertiesIntoBuffer(BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool includeUniqueID)
        {
            base.WritePropertiesIntoBuffer(writer, includeChildrenMode, verifyCleanness, includeUniqueID);
            // write properties
            CompressedIntegralTypes.WriteCompressedInt(writer, _AnotherPropertyAdded);
            WriteNonLazinatorObject(
            nonLazinatorObject: _MyListT, isBelievedDirty: _MyListT_Accessed,
            isAccessed: _MyListT_Accessed, writer: writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorObjectBytes, _MyListT_ByteIndex, _MyListT_ByteLength),
            verifyCleanness: false,
            binaryWriterAction: (w, v) =>
            ConvertToBytes_List_GExampleChild_g(w, MyListT,
            includeChildrenMode, v));
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                WriteChildWithLength(writer, _MyT, includeChildrenMode, _MyT_Accessed, () => GetChildSlice(LazinatorObjectBytes, _MyT_ByteIndex, _MyT_ByteLength), verifyCleanness, false, false, this);
            }
        }
        
        /* Conversion of supported collections and tuples */
        
        private static List<ExampleChild> ConvertFromBytes_List_GExampleChild_g(ReadOnlyMemory<byte> storage, DeserializationFactory deserializationFactory, InformParentOfDirtinessDelegate informParentOfDirtinessDelegate)
        {
            if (storage.Length == 0)
            {
                return default(List<ExampleChild>);
            }
            ReadOnlySpan<byte> span = storage.Span;
            
            int bytesSoFar = 0;
            int collectionLength = span.ToDecompressedInt(ref bytesSoFar);
            
            List<ExampleChild> collection = new List<ExampleChild>(collectionLength);
            for (int i = 0; i < collectionLength; i++)
            {
                int lengthCollectionMember = span.ToInt32(ref bytesSoFar);
                if (lengthCollectionMember == 0)
                {
                    collection.Add(default(ExampleChild));
                }
                else
                {
                    ReadOnlyMemory<byte> childData = storage.Slice(bytesSoFar, lengthCollectionMember);
                    if (deserializationFactory == null)
                    {
                        deserializationFactory = DeserializationFactory.GetInstance();
                    }
                    var item = (ExampleChild)deserializationFactory.FactoryCreateFromBytesIncludingIDSpecifyingDelegate(childData, informParentOfDirtinessDelegate);
                    collection.Add(item);
                }
                bytesSoFar += lengthCollectionMember;
            }
            
            return collection;
        }
        
        private static void ConvertToBytes_List_GExampleChild_g(BinaryBufferWriter writer, List<ExampleChild> itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
            if (itemToConvert == default(List<ExampleChild>))
            {
                return;
            }
            CompressedIntegralTypes.WriteCompressedInt(writer, itemToConvert.Count);
            int itemToConvertCount = itemToConvert.Count;
            for (int itemIndex = 0; itemIndex < itemToConvertCount; itemIndex++)
            {
                if (itemToConvert[itemIndex] == default(ExampleChild))
                {
                    writer.Write((uint)0);
                }
                else 
                {
                    
                    void action(BinaryBufferWriter w) => itemToConvert[itemIndex].SerializeExistingBuffer(writer, includeChildrenMode, verifyCleanness);
                    WriteToBinaryWithIntLengthPrefix(writer, action);
                }
                
            }
        }
        
    }
}
