//c9557c9f-2810-918b-0080-80b52edeeb80
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.35
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LazinatorTests.Examples.Abstract
{
    using Lazinator.Attributes;
    using Lazinator.Buffers;
    using Lazinator.Core;
    using Lazinator.Exceptions;
    using Lazinator.Support;
    using System;
    using System.Buffers;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.InteropServices;
    using static Lazinator.Core.LazinatorUtilities;
    
    [Autogenerated]
    public partial class Concrete6 : Concrete5, ILazinator
    {
        /* Clone overrides */
        
        public Concrete6() : base()
        {
        }
        
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
        
        private List<int> _IntList6;
        public List<int> IntList6
        {
            [DebuggerStepThrough]
            get
            {
                if (!_IntList6_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _IntList6 = default(List<int>);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _IntList6_ByteIndex, _IntList6_ByteLength);
                        _IntList6 = ConvertFromBytes_List_Gint_g(childData, DeserializationFactory, null);
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
            ConvertToBytes_List_Gint_g(w, IntList6,
            includeChildrenMode, v));
        }
        
        /* Conversion of supported collections and tuples */
        
        private static List<int> ConvertFromBytes_List_Gint_g(ReadOnlyMemory<byte> storage, DeserializationFactory deserializationFactory, InformParentOfDirtinessDelegate informParentOfDirtinessDelegate)
        {
            if (storage.Length == 0)
            {
                return default(List<int>);
            }
            ReadOnlySpan<byte> span = storage.Span;
            
            int bytesSoFar = 0;
            int collectionLength = span.ToDecompressedInt(ref bytesSoFar);
            
            List<int> collection = new List<int>(collectionLength);
            for (int i = 0; i < collectionLength; i++)
            {
                int item = span.ToDecompressedInt(ref bytesSoFar);
                collection.Add(item);
            }
            
            return collection;
        }
        
        private static void ConvertToBytes_List_Gint_g(BinaryBufferWriter writer, List<int> itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
            if (itemToConvert == default(List<int>))
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
