//dc16eb36-f6e4-4256-30bc-811462e4abe5
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.359
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LazinatorCollections.Avl.ValueTree
{
    using Lazinator.Attributes;
    using Lazinator.Buffers;
    using Lazinator.Core;
    using Lazinator.Exceptions;
    using Lazinator.Support;
    using LazinatorCollections.Tree;
    using System;
    using System.Buffers;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;
    using static Lazinator.Core.LazinatorUtilities;
    
    [Autogenerated]
    public partial class AvlAggregatedNode<T> : AvlCountedNode<T>, ILazinator
    {
        /* Property definitions */
        
        
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected long _LeftAggregatedCount;
        public long LeftAggregatedCount
        {
            [DebuggerStepThrough]
            get
            {
                return _LeftAggregatedCount;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _LeftAggregatedCount = value;
            }
        }
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected long _RightAggregatedCount;
        public long RightAggregatedCount
        {
            [DebuggerStepThrough]
            get
            {
                return _RightAggregatedCount;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _RightAggregatedCount = value;
            }
        }
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected long _SelfAggregatedCount;
        public long SelfAggregatedCount
        {
            [DebuggerStepThrough]
            get
            {
                return _SelfAggregatedCount;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _SelfAggregatedCount = value;
            }
        }
        /* Clone overrides */
        
        public AvlAggregatedNode() : base()
        {
        }
        
        public override ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode = IncludeChildrenMode.IncludeAllChildren, CloneBufferOptions cloneBufferOptions = CloneBufferOptions.IndependentBuffers)
        {
            var clone = new AvlAggregatedNode<T>()
            {
                OriginalIncludeChildrenMode = includeChildrenMode
            };
            clone = CompleteClone(this, clone, includeChildrenMode, cloneBufferOptions);
            return clone;
        }
        
        public override ILazinator AssignCloneProperties(ILazinator clone, IncludeChildrenMode includeChildrenMode)
        {
            base.AssignCloneProperties(clone, includeChildrenMode);
            AvlAggregatedNode<T> typedClone = (AvlAggregatedNode<T>) clone;
            typedClone.LeftAggregatedCount = LeftAggregatedCount;
            typedClone.RightAggregatedCount = RightAggregatedCount;
            typedClone.SelfAggregatedCount = SelfAggregatedCount;
            
            return typedClone;
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
            yield return ("LeftAggregatedCount", (object)LeftAggregatedCount);
            yield return ("RightAggregatedCount", (object)RightAggregatedCount);
            yield return ("SelfAggregatedCount", (object)SelfAggregatedCount);
            yield break;
        }
        
        public override ILazinator ForEachLazinator(Func<ILazinator, ILazinator> changeFunc, bool exploreOnlyDeserializedChildren, bool changeThisLevel)
        {
            base.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, false);
            if (changeThisLevel)
            {
                return changeFunc(this);
            }
            return this;
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
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public override int LazinatorUniqueID => 169;
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected override bool ContainsOpenGenericParameters => true;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public override LazinatorGenericIDType LazinatorGenericID => LazinatorGenericIDType.GetCachedForType<AvlAggregatedNode<T>>(() => DeserializationFactory.Instance.GetUniqueIDListForGenericType(169, new Type[] { typeof(T) }));
        
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public override int LazinatorObjectVersion { get; set; } = 0;
        
        
        public override void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            base.ConvertFromBytesAfterHeader(OriginalIncludeChildrenMode, serializedVersionNumber, ref bytesSoFar);
            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
            _LeftAggregatedCount = span.ToDecompressedLong(ref bytesSoFar);
            _RightAggregatedCount = span.ToDecompressedLong(ref bytesSoFar);
            _SelfAggregatedCount = span.ToDecompressedLong(ref bytesSoFar);
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
                UpdateStoredBuffer(ref writer, startPosition, writer.Position - startPosition, includeChildrenMode, false);
            }
        }
        
        public override void UpdateStoredBuffer(ref BinaryBufferWriter writer, int startPosition, int length, IncludeChildrenMode includeChildrenMode, bool updateDeserializedChildren)
        {
            _IsDirty = false;
            if (includeChildrenMode == IncludeChildrenMode.IncludeAllChildren)
            {
                _DescendantIsDirty = false;
                if (updateDeserializedChildren)
                {
                    UpdateDeserializedChildren(ref writer, startPosition);
                }
                
                if (_Value_Accessed && _Value != null && _Value.IsStruct && (_Value.IsDirty || _Value.DescendantIsDirty))
                {
                    _Value_Accessed = false;
                }
            }
            else
            {
                throw new LazinatorSerializationException("Cannot update stored buffer when serializing only some children.");
            }
            
            var newBuffer = writer.Slice(startPosition, length);
            LazinatorMemoryStorage = newBuffer;
        }
        
        protected override void UpdateDeserializedChildren(ref BinaryBufferWriter writer, int startPosition)
        {
            base.UpdateDeserializedChildren(ref writer, startPosition);
        }
        
        
        protected override void WritePropertiesIntoBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer, bool includeUniqueID)
        {
            base.WritePropertiesIntoBuffer(ref writer, includeChildrenMode, verifyCleanness, updateStoredBuffer, includeUniqueID);
            // write properties
            CompressedIntegralTypes.WriteCompressedLong(ref writer, _LeftAggregatedCount);
            CompressedIntegralTypes.WriteCompressedLong(ref writer, _RightAggregatedCount);
            CompressedIntegralTypes.WriteCompressedLong(ref writer, _SelfAggregatedCount);
        }
        
    }
}
