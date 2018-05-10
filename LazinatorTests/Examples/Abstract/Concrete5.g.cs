//b3ff31a4-02b0-a9e8-b70c-e1ff024903a3
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool.
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

namespace LazinatorTests.Examples.Abstract
{
    public partial class Concrete5 : Abstract4, ILazinator
    {
        /* Clone overrides */
        
        public override ILazinator CloneLazinator()
        {
            return CloneLazinator(OriginalIncludeChildrenMode);
        }
        
        public override ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode)
        {
            MemoryInBuffer bytes = EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, false, false, IsDirty, DescendantIsDirty, false, LazinatorObjectBytes, (StreamManuallyDelegate)EncodeToNewBuffer);
            var clone = new Concrete5()
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
        
        /* Conversion */
        
        public override int LazinatorUniqueID => 239;
        
        public override int LazinatorObjectVersion { get; set; } = 0;
        
        public override void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            base.ConvertFromBytesAfterHeader(OriginalIncludeChildrenMode, serializedVersionNumber, ref bytesSoFar);
            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
            _String4 = span.ToString_BrotliCompressedWithLength(ref bytesSoFar);
            _String5 = span.ToString_BrotliCompressedWithLength(ref bytesSoFar);
        }
        
        public override void SerializeExistingBuffer(BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
            base.SerializeExistingBuffer(writer, includeChildrenMode, verifyCleanness);
            // write properties
            EncodeCharAndString.WriteBrotliCompressedWithIntPrefix(writer, _String4);
            EncodeCharAndString.WriteBrotliCompressedWithIntPrefix(writer, _String5);
        }
        
    }
}
