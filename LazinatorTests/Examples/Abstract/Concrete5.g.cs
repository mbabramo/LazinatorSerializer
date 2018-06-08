//9f2574b7-e3d3-e1f5-18c8-7de7c00cecca
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.79
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
    public partial class Concrete5 : Abstract4, ILazinator
    {
        /* Clone overrides */
        
        public Concrete5() : base()
        {
        }
        
        public override ILazinator CloneLazinator()
        {
            return CloneLazinator(OriginalIncludeChildrenMode);
        }
        
        public override ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode)
        {
            MemoryInBuffer bytes = EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, false, false, IsDirty, DescendantIsDirty, false, LazinatorObjectBytes, (StreamManuallyDelegate)EncodeToNewBuffer);
            var clone = new Concrete5()
            {
                LazinatorParentClass = LazinatorParentClass,
                InformParentOfDirtinessDelegate = InformParentOfDirtinessDelegate,
                OriginalIncludeChildrenMode = includeChildrenMode,
                HierarchyBytes = bytes
            };
            return clone;
        }
        
        /* Properties */
        protected int _IntList5_ByteIndex;
        protected override int _IntList4_ByteLength => _IntList5_ByteIndex - _IntList4_ByteIndex;
        private int _Concrete5_EndByteIndex;
        protected virtual int _IntList5_ByteLength => _Concrete5_EndByteIndex - _IntList5_ByteIndex;
        
        private string _String4;
        public override string String4
        {
            [DebuggerStepThrough]
            get
            {
                return _String4;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _String4 = value;
            }
        }
        private string _String5;
        public string String5
        {
            [DebuggerStepThrough]
            get
            {
                return _String5;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _String5 = value;
            }
        }
        private List<int> _IntList4;
        public override List<int> IntList4
        {
            [DebuggerStepThrough]
            get
            {
                if (!_IntList4_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _IntList4 = default(List<int>);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _IntList4_ByteIndex, _IntList4_ByteLength, false, false, null);
                        _IntList4 = ConvertFromBytes_List_Gint_g(childData, null);
                    }
                    _IntList4_Accessed = true;
                }
                IsDirty = true;
                return _IntList4;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _IntList4 = value;
                _IntList4_Accessed = true;
            }
        }
        private List<int> _IntList5;
        public List<int> IntList5
        {
            [DebuggerStepThrough]
            get
            {
                if (!_IntList5_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _IntList5 = default(List<int>);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _IntList5_ByteIndex, _IntList5_ByteLength, false, false, null);
                        _IntList5 = ConvertFromBytes_List_Gint_g(childData, null);
                    }
                    _IntList5_Accessed = true;
                }
                IsDirty = true;
                return _IntList5;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _IntList5 = value;
                _IntList5_Accessed = true;
            }
        }
        protected bool _IntList5_Accessed;
        
        protected override void ResetAccessedProperties()
        {
            base.ResetAccessedProperties();
            _IntList4_Accessed = _IntList5_Accessed = false;
        }
        
        /* Conversion */
        
        public override int LazinatorUniqueID => 239;
        
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
            _String4 = span.ToString_BrotliCompressedWithLength(ref bytesSoFar);
            _String5 = span.ToString_BrotliCompressedWithLength(ref bytesSoFar);
            _IntList4_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            _IntList5_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            _Concrete5_EndByteIndex = bytesSoFar;
        }
        
        public override void SerializeExistingBuffer(BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
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
            EncodeCharAndString.WriteBrotliCompressedWithIntPrefix(writer, _String4);
            EncodeCharAndString.WriteBrotliCompressedWithIntPrefix(writer, _String5);
            startOfObjectPosition = writer.Position;
            WriteNonLazinatorObject(
            nonLazinatorObject: _IntList4, isBelievedDirty: _IntList4_Accessed,
            isAccessed: _IntList4_Accessed, writer: writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorObjectBytes, _IntList4_ByteIndex, _IntList4_ByteLength, false, false, null),
            verifyCleanness: false,
            binaryWriterAction: (w, v) =>
            ConvertToBytes_List_Gint_g(w, IntList4,
            includeChildrenMode, v, updateStoredBuffer));
            _IntList4_ByteIndex = startOfObjectPosition - startPosition;
            startOfObjectPosition = writer.Position;
            WriteNonLazinatorObject(
            nonLazinatorObject: _IntList5, isBelievedDirty: _IntList5_Accessed,
            isAccessed: _IntList5_Accessed, writer: writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorObjectBytes, _IntList5_ByteIndex, _IntList5_ByteLength, false, false, null),
            verifyCleanness: false,
            binaryWriterAction: (w, v) =>
            ConvertToBytes_List_Gint_g(w, IntList5,
            includeChildrenMode, v, updateStoredBuffer));
            _IntList5_ByteIndex = startOfObjectPosition - startPosition;
            _Concrete5_EndByteIndex = writer.Position;
        }
        
        /* Conversion of supported collections and tuples */
        
        private static List<int> ConvertFromBytes_List_Gint_g(ReadOnlyMemory<byte> storage, InformParentOfDirtinessDelegate informParentOfDirtinessDelegate)
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
        
        private static void ConvertToBytes_List_Gint_g(BinaryBufferWriter writer, List<int> itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
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
