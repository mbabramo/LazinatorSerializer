//e1fe3842-1dc4-b836-23b6-a4e78d051095
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.24
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

namespace LazinatorTests.Examples.NonAbstractGenerics
{
    public partial class ClosedGeneric : OpenGeneric<ExampleChild>, ILazinator
    {
        /* Clone overrides */
        
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
        internal int _MyT_ByteIndex;
        internal int _MyListT_ByteIndex;
        internal int _MyListT_EndByteIndex;
        internal int _MyT_ByteLength => _MyListT_ByteIndex - _MyT_ByteIndex;
        internal int _MyListT_ByteLength => _MyListT_EndByteIndex - _MyListT_ByteIndex;
        
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
        private LazinatorTests.Examples.ExampleChild _MyT;
        public override LazinatorTests.Examples.ExampleChild MyT
        {
            [DebuggerStepThrough]
            get
            {
                if (!_MyT_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _MyT = default(LazinatorTests.Examples.ExampleChild);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _MyT_ByteIndex, _MyT_ByteLength);
                        
                        if (DeserializationFactory == null)
                        {
                            LazinatorDeserializationException.ThrowNoDeserializationFactory();
                        }
                        _MyT = DeserializationFactory.Create(213, () => new LazinatorTests.Examples.ExampleChild(), childData, this); 
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
        internal bool _MyT_Accessed;
        private System.Collections.Generic.List<LazinatorTests.Examples.ExampleChild> _MyListT;
        public override System.Collections.Generic.List<LazinatorTests.Examples.ExampleChild> MyListT
        {
            [DebuggerStepThrough]
            get
            {
                if (!_MyListT_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _MyListT = default(System.Collections.Generic.List<LazinatorTests.Examples.ExampleChild>);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _MyListT_ByteIndex, _MyListT_ByteLength);
                        _MyListT = ConvertFromBytes_System_Collections_Generic_List_ExampleChild(childData, DeserializationFactory, null);
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
        internal bool _MyListT_Accessed;
        
        /* Conversion */
        
        public override int LazinatorUniqueID => 250;
        
        public override int LazinatorObjectVersion { get; set; } = 0;
        
        public override void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            base.ConvertFromBytesAfterHeader(OriginalIncludeChildrenMode, serializedVersionNumber, ref bytesSoFar);
            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
            _AnotherPropertyAdded = span.ToDecompressedInt(ref bytesSoFar);
            _MyT_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            _MyListT_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            _MyListT_EndByteIndex = bytesSoFar;
        }
        
        public override void SerializeExistingBuffer(BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
            base.SerializeExistingBuffer(writer, includeChildrenMode, verifyCleanness);
            // write properties
            CompressedIntegralTypes.WriteCompressedInt(writer, _AnotherPropertyAdded);
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                WriteChildWithLength(writer, _MyT, includeChildrenMode, _MyT_Accessed, () => GetChildSlice(LazinatorObjectBytes, _MyT_ByteIndex, _MyT_ByteLength), verifyCleanness, false);
            }
            WriteNonLazinatorObject(
            nonLazinatorObject: _MyListT, isBelievedDirty: _MyListT_Accessed,
            isAccessed: _MyListT_Accessed, writer: writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorObjectBytes, _MyListT_ByteIndex, _MyListT_ByteLength),
            verifyCleanness: false,
            binaryWriterAction: (w, v) =>
            ConvertToBytes_System_Collections_Generic_List_ExampleChild(w, MyListT,
            includeChildrenMode, v));
        }
        
        /* Conversion of supported collections and tuples */
        
        private static System.Collections.Generic.List<LazinatorTests.Examples.ExampleChild> ConvertFromBytes_System_Collections_Generic_List_ExampleChild(ReadOnlyMemory<byte> storage, DeserializationFactory deserializationFactory, InformParentOfDirtinessDelegate informParentOfDirtinessDelegate)
        {
            if (storage.Length == 0)
            {
                return default(System.Collections.Generic.List<LazinatorTests.Examples.ExampleChild>);
            }
            ReadOnlySpan<byte> span = storage.Span;
            
            int bytesSoFar = 0;
            int collectionLength = span.ToDecompressedInt(ref bytesSoFar);
            
            System.Collections.Generic.List<LazinatorTests.Examples.ExampleChild> collection = new System.Collections.Generic.List<LazinatorTests.Examples.ExampleChild>(collectionLength);
            for (int i = 0; i < collectionLength; i++)
            {
                int lengthCollectionMember = span.ToInt32(ref bytesSoFar);
                if (lengthCollectionMember == 0)
                {
                    collection.Add(default(LazinatorTests.Examples.ExampleChild));
                }
                else
                {
                    ReadOnlyMemory<byte> childData = storage.Slice(bytesSoFar, lengthCollectionMember);
                    if (deserializationFactory == null)
                    {
                        throw new MissingDeserializationFactoryException();
                    }
                    var item = (LazinatorTests.Examples.ExampleChild)deserializationFactory.FactoryCreate(childData, informParentOfDirtinessDelegate);
                    collection.Add(item);
                }
                bytesSoFar += lengthCollectionMember;
            }
            
            return collection;
        }
        
        private static void ConvertToBytes_System_Collections_Generic_List_ExampleChild(BinaryBufferWriter writer, System.Collections.Generic.List<LazinatorTests.Examples.ExampleChild> itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
            if (itemToConvert == default(System.Collections.Generic.List<LazinatorTests.Examples.ExampleChild>))
            {
                return;
            }
            CompressedIntegralTypes.WriteCompressedInt(writer, itemToConvert.Count);
            int itemToConvertCount = itemToConvert.Count;
            for (int itemIndex = 0; itemIndex < itemToConvertCount; itemIndex++)
            {
                if (itemToConvert[itemIndex] == default(LazinatorTests.Examples.ExampleChild))
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
