//c782722a-0200-69ed-a0e1-609e54640d4b
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.262
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
    using Lazinator.Wrappers;
    using System;
    using System.Buffers;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;
    using static Lazinator.Core.LazinatorUtilities;
    
    [Autogenerated]
    public partial class ConcreteFromGenericFromBase : GenericFromBase<WNullableDecimal>, ILazinator
    {
        /* Clone overrides */
        
        public ConcreteFromGenericFromBase() : base()
        {
        }
        
        public override ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode = IncludeChildrenMode.IncludeAllChildren, CloneBufferOptions cloneBufferOptions = CloneBufferOptions.LinkedBuffer)
        {
            var clone = new ConcreteFromGenericFromBase()
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
        
        protected override void AssignCloneProperties(ILazinator clone, IncludeChildrenMode includeChildrenMode)
        {
            base.AssignCloneProperties(clone, includeChildrenMode);
            ConcreteFromGenericFromBase typedClone = (ConcreteFromGenericFromBase) clone;
        }
        
        /* Properties */
        
        
        public override IEnumerable<(string propertyName, ILazinator descendant)> EnumerateLazinatorDescendants(Func<ILazinator, bool> matchCriterion, bool stopExploringBelowMatch, Func<ILazinator, bool> exploreCriterion, bool exploreOnlyDeserializedChildren, bool enumerateNulls)
        {
            foreach (var inheritedYield in base.EnumerateLazinatorDescendants(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
            {
                yield return inheritedYield;
            }
            yield break;
        }
        
        
        public override IEnumerable<(string propertyName, object descendant)> EnumerateNonLazinatorProperties()
        {
            foreach (var inheritedYield in base.EnumerateNonLazinatorProperties())
            {
                yield return inheritedYield;
            }
            yield break;
        }
        
        public override void FreeInMemoryObjects()
        {
            base.FreeInMemoryObjects();
            
            IsDirty = false;
            DescendantIsDirty = false;
            HasChanged = false;
            DescendantHasChanged = false;
        }
        
        /* Conversion */
        
        public override int LazinatorUniqueID => 272;
        
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
        }
        
        public override void SerializeExistingBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren)
            {
                updateStoredBuffer = false;
            }
            int startPosition = writer.Position;
            WritePropertiesIntoBuffer(ref writer, includeChildrenMode, verifyCleanness, updateStoredBuffer, true);
            if (updateStoredBuffer)
            {
                UpdateStoredBuffer(ref writer, startPosition, includeChildrenMode, false);
            }
        }
        
        protected override void WritePropertiesIntoBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer, bool includeUniqueID)
        {
            base.WritePropertiesIntoBuffer(ref writer, includeChildrenMode, verifyCleanness, updateStoredBuffer, includeUniqueID);
            // write properties
        }
        
    }
}
