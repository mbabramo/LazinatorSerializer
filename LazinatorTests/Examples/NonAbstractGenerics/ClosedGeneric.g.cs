//71f457e4-504d-b1f6-58f2-1f6998160475
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.132
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
    using System.Linq;
    using System.Runtime.InteropServices;
    using static Lazinator.Core.LazinatorUtilities;
    
    [Autogenerated]
    public partial class ClosedGeneric : OpenGeneric<ExampleChild>, ILazinator
    {
        /* Clone overrides */
        
        public ClosedGeneric() : base()
        {
        }
        
        public override ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode)
        {
            MemoryInBuffer bytes = EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, false, IsDirty, DescendantIsDirty, false, LazinatorObjectBytes, (StreamManuallyDelegate)EncodeToNewBuffer);
            var clone = new ClosedGeneric()
            {
                LazinatorParentClass = LazinatorParentClass,
                OriginalIncludeChildrenMode = includeChildrenMode,
                HierarchyBytes = bytes
            };
            clone.LazinatorParentClass = null;
            return clone;
        }
        
        /* Properties */
        
        private int _AnotherPropertyAdded;
        public int AnotherPropertyAdded
        {
            get
            {
                return _AnotherPropertyAdded;
            }
            set
            {
                IsDirty = true;
                _AnotherPropertyAdded = value;
            }
        }
        
        protected override IEnumerable<ILazinator> EnumerateLazinatorNodes_Helper(Func<ILazinator, bool> matchCriterion, bool stopExploringBelowMatch, Func<ILazinator, bool> exploreCriterion, bool exploreOnlyDeserializedChildren)
        {
            base.EnumerateLazinatorNodes_Helper(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren);
            yield break;
        }
        
        protected override void ResetAccessedProperties()
        {
            base.ResetAccessedProperties();
            
        }
        
        /* Conversion */
        
        public override int LazinatorUniqueID => 250;
        
        protected override bool ContainsOpenGenericParameters => false;
        public override LazinatorGenericIDType LazinatorGenericID
        {
            get => default;
            set { }
        }
        
        public override int LazinatorObjectVersion { get; set; } = 0;
        
        
        public override void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            base.ConvertFromBytesAfterHeader(OriginalIncludeChildrenMode, serializedVersionNumber, ref bytesSoFar);
            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
            _AnotherPropertyAdded = span.ToDecompressedInt(ref bytesSoFar);
        }
        
        public override void SerializeExistingBuffer(BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
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
                _DescendantIsDirty = false;
                
                _LazinatorObjectBytes = writer.Slice(startPosition);
            }
        }
        
        protected override void WritePropertiesIntoBuffer(BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer, bool includeUniqueID)
        {
            base.WritePropertiesIntoBuffer(writer, includeChildrenMode, verifyCleanness, updateStoredBuffer, includeUniqueID);
            // write properties
            CompressedIntegralTypes.WriteCompressedInt(writer, _AnotherPropertyAdded);
        }
        
    }
}
