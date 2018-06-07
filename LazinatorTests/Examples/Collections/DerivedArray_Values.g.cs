//3f7ebe06-0f48-8c50-7236-ced11d27ab6b
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.77
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
    using System;
    using System.Buffers;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.InteropServices;
    using static Lazinator.Core.LazinatorUtilities;
    
    [Autogenerated]
    public partial class DerivedArray_Values : Array_Values, ILazinator
    {
        /* Clone overrides */
        
        public DerivedArray_Values() : base()
        {
        }
        
        public override ILazinator CloneLazinator()
        {
            return CloneLazinator(OriginalIncludeChildrenMode);
        }
        
        public override ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode)
        {
            MemoryInBuffer bytes = EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, false, false, IsDirty, DescendantIsDirty, false, LazinatorObjectBytes, (StreamManuallyDelegate)EncodeToNewBuffer);
            var clone = new DerivedArray_Values()
            {
                LazinatorParentClass = LazinatorParentClass,
                InformParentOfDirtinessDelegate = InformParentOfDirtinessDelegate,
                OriginalIncludeChildrenMode = includeChildrenMode,
                HierarchyBytes = bytes
            };
            return clone;
        }
        
        /* Properties */
        protected int _MyArrayInt_DerivedLevel_ByteIndex;
        private int _DerivedArray_Values_EndByteIndex;
        protected virtual int _MyArrayInt_DerivedLevel_ByteLength => _DerivedArray_Values_EndByteIndex - _MyArrayInt_DerivedLevel_ByteIndex;
        
        private int[] _MyArrayInt_DerivedLevel;
        public int[] MyArrayInt_DerivedLevel
        {
            [DebuggerStepThrough]
            get
            {
                if (!_MyArrayInt_DerivedLevel_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _MyArrayInt_DerivedLevel = default(int[]);
                        _MyArrayInt_DerivedLevel_Dirty = true;
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _MyArrayInt_DerivedLevel_ByteIndex, _MyArrayInt_DerivedLevel_ByteLength, false, false, null);
                        _MyArrayInt_DerivedLevel = ConvertFromBytes_int_B_b(childData, () => { MyArrayInt_DerivedLevel_Dirty = true; });
                    }
                    _MyArrayInt_DerivedLevel_Accessed = true;
                }
                return _MyArrayInt_DerivedLevel;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _MyArrayInt_DerivedLevel = value;
                _MyArrayInt_DerivedLevel_Dirty = true;
                _MyArrayInt_DerivedLevel_Accessed = true;
            }
        }
        protected bool _MyArrayInt_DerivedLevel_Accessed;
        
        private bool _MyArrayInt_DerivedLevel_Dirty;
        public bool MyArrayInt_DerivedLevel_Dirty
        {
            [DebuggerStepThrough]
            get => _MyArrayInt_DerivedLevel_Dirty;
            [DebuggerStepThrough]
            set
            {
                if (_MyArrayInt_DerivedLevel_Dirty != value)
                {
                    _MyArrayInt_DerivedLevel_Dirty = value;
                    if (value && !IsDirty)
                    {
                        IsDirty = true;
                    }
                }
            }
        }
        
        protected override void ResetAccessedProperties()
        {
            base.ResetAccessedProperties();
            _MyArrayInt_DerivedLevel_Accessed = false;
        }
        
        /* Conversion */
        
        public override int LazinatorUniqueID => 261;
        
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
            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;_MyArrayInt_DerivedLevel_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            _DerivedArray_Values_EndByteIndex = bytesSoFar;
        }
        
        public override void SerializeExistingBuffer(BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
            int startPosition = writer.Position;
            WritePropertiesIntoBuffer(writer, includeChildrenMode, verifyCleanness, true);
            
            _IsDirty = false;
            _DescendantIsDirty = false;
            
            _LazinatorObjectBytes = writer.Slice(startPosition);
        }
        
        protected override void WritePropertiesIntoBuffer(BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool includeUniqueID)
        {
            base.WritePropertiesIntoBuffer(writer, includeChildrenMode, verifyCleanness, includeUniqueID);
            // write properties
            WriteNonLazinatorObject(
            nonLazinatorObject: _MyArrayInt_DerivedLevel, isBelievedDirty: MyArrayInt_DerivedLevel_Dirty,
            isAccessed: _MyArrayInt_DerivedLevel_Accessed, writer: writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorObjectBytes, _MyArrayInt_DerivedLevel_ByteIndex, _MyArrayInt_DerivedLevel_ByteLength, false, false, null),
            verifyCleanness: verifyCleanness,
            binaryWriterAction: (w, v) =>
            ConvertToBytes_int_B_b(w, MyArrayInt_DerivedLevel,
            includeChildrenMode, v));
        }
        
        /* Conversion of supported collections and tuples */
        
        private static int[] ConvertFromBytes_int_B_b(ReadOnlyMemory<byte> storage, InformParentOfDirtinessDelegate informParentOfDirtinessDelegate)
        {
            if (storage.Length == 0)
            {
                return default(int[]);
            }
            ReadOnlySpan<byte> span = storage.Span;
            
            int bytesSoFar = 0;
            int collectionLength = span.ToDecompressedInt(ref bytesSoFar);
            
            int[] collection = new int[collectionLength];
            for (int i = 0; i < collectionLength; i++)
            {
                int item = span.ToDecompressedInt(ref bytesSoFar);
                collection[i] = item;
            }
            
            return collection;
        }
        
        private static void ConvertToBytes_int_B_b(BinaryBufferWriter writer, int[] itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
            if (itemToConvert == default(int[]))
            {
                return;
            }
            CompressedIntegralTypes.WriteCompressedInt(writer, itemToConvert.Length);
            int itemToConvertCount = itemToConvert.Length;
            for (int itemIndex = 0; itemIndex < itemToConvertCount; itemIndex++)
            {
                CompressedIntegralTypes.WriteCompressedInt(writer, itemToConvert[itemIndex]);
            }
        }
        
    }
}
