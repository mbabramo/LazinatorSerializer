//1f064767-fa02-c56e-7dfe-e6a8fdf14705
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.31
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LazinatorTests.Examples.Abstract
{
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
    
    
    public partial class Concrete6 : Concrete5, ILazinator
    {
        /* Clone overrides */
        
        public override ILazinator CloneLazinator()
        {
            return CloneLazinator(OriginalIncludeChildrenMode);
        }
        
        public override ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode)
        {
            MemoryInBuffer bytes = EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, false, false, IsDirty, DescendantIsDirty, false, LazinatorObjectBytes, (StreamManuallyDelegate)EncodeToNewBuffer);
            var clone = new Concrete6()
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
        protected int _IntList6_ByteIndex;
        private int _Concrete6_EndByteIndex;
        protected virtual int _IntList6_ByteLength => _Concrete6_EndByteIndex - _IntList6_ByteIndex;
        
        private System.Collections.Generic.List<int> _IntList6;
        public System.Collections.Generic.List<int> IntList6
        {
            [DebuggerStepThrough]
            get
            {
                if (!_IntList6_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _IntList6 = default(System.Collections.Generic.List<int>);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _IntList6_ByteIndex, _IntList6_ByteLength);
                        _IntList6 = ConvertFromBytes_System_Collections_Generic_List_int(childData, DeserializationFactory, null);
                    }
                    _IntList6_Accessed = true;
                    IsDirty = true;
                }
                return _IntList6;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _IntList6 = value;
                _IntList6_Accessed = true;
            }
        }
        protected bool _IntList6_Accessed;
        
        /* Conversion */
        
        public override int LazinatorUniqueID => 249;
        
        public override int LazinatorObjectVersion { get; set; } = 0;
        
        public override void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            base.ConvertFromBytesAfterHeader(OriginalIncludeChildrenMode, serializedVersionNumber, ref bytesSoFar);
            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
            _IntList6_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            _Concrete6_EndByteIndex = bytesSoFar;
        }
        
        public override void SerializeExistingBuffer(BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
            base.SerializeExistingBuffer(writer, includeChildrenMode, verifyCleanness);
            // write properties
            WriteNonLazinatorObject(
            nonLazinatorObject: _IntList6, isBelievedDirty: _IntList6_Accessed,
            isAccessed: _IntList6_Accessed, writer: writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorObjectBytes, _IntList6_ByteIndex, _IntList6_ByteLength),
            verifyCleanness: false,
            binaryWriterAction: (w, v) =>
            ConvertToBytes_System_Collections_Generic_List_int(w, IntList6,
            includeChildrenMode, v));
        }
        
        /* Conversion of supported collections and tuples */
        
        private static System.Collections.Generic.List<int> ConvertFromBytes_System_Collections_Generic_List_int(ReadOnlyMemory<byte> storage, DeserializationFactory deserializationFactory, InformParentOfDirtinessDelegate informParentOfDirtinessDelegate)
        {
            if (storage.Length == 0)
            {
                return default(System.Collections.Generic.List<int>);
            }
            ReadOnlySpan<byte> span = storage.Span;
            
            int bytesSoFar = 0;
            int collectionLength = span.ToDecompressedInt(ref bytesSoFar);
            
            System.Collections.Generic.List<int> collection = new System.Collections.Generic.List<int>(collectionLength);
            for (int i = 0; i < collectionLength; i++)
            {
                int item = span.ToDecompressedInt(ref bytesSoFar);
                collection.Add(item);
            }
            
            return collection;
        }
        
        private static void ConvertToBytes_System_Collections_Generic_List_int(BinaryBufferWriter writer, System.Collections.Generic.List<int> itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
            if (itemToConvert == default(System.Collections.Generic.List<int>))
            {
                return;
            }
            CompressedIntegralTypes.WriteCompressedInt(writer, itemToConvert.Count);
            int itemToConvertCount = itemToConvert.Count;
            for (int itemIndex = 0; itemIndex < itemToConvertCount; itemIndex++)
            {
                CompressedIntegralTypes.WriteCompressedInt(writer, itemToConvert[itemIndex]);
            }
        }
        
    }
}
