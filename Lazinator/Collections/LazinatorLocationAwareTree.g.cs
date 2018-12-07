//9ab95e60-19f3-c91f-1ec3-a61c5b784b5d
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.342
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Lazinator.Collections
{
    using Lazinator.Attributes;
    using Lazinator.Buffers;
    using Lazinator.Collections.Dictionary;
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
    public partial class LazinatorGeneralLocationAwareTree<T> : LazinatorGeneralTree<T>, ILazinator
    {
        /* Property definitions */
        
        protected int _Locations_ByteIndex;
        private int _LazinatorLocationAwareTree_T_EndByteIndex;
        protected virtual int _Locations_ByteLength => _LazinatorLocationAwareTree_T_EndByteIndex - _Locations_ByteIndex;
        
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected LazinatorDictionary<T, LazinatorList<WInt>> _Locations;
        public virtual LazinatorDictionary<T, LazinatorList<WInt>> Locations
        {
            [DebuggerStepThrough]
            get
            {
                if (!_Locations_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _Locations = default(LazinatorDictionary<T, LazinatorList<WInt>>);
                    }
                    else
                    {
                        LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _Locations_ByteIndex, _Locations_ByteLength, false, false, null);
                        
                        _Locations = DeserializationFactory.Instance.CreateBaseOrDerivedType(99, () => new LazinatorDictionary<T, LazinatorList<WInt>>(), childData, this); 
                    }
                    _Locations_Accessed = true;
                } 
                return _Locations;
            }
            [DebuggerStepThrough]
            set
            {
                if (_Locations != null)
                {
                    _Locations.LazinatorParents = _Locations.LazinatorParents.WithRemoved(this);
                }
                if (value != null)
                {
                    value.LazinatorParents = value.LazinatorParents.WithAdded(this);
                }
                
                IsDirty = true;
                DescendantIsDirty = true;
                _Locations = value;
                _Locations_Accessed = true;
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected bool _Locations_Accessed;
        /* Clone overrides */
        
        public LazinatorGeneralLocationAwareTree() : base()
        {
        }
        
        public override ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode = IncludeChildrenMode.IncludeAllChildren, CloneBufferOptions cloneBufferOptions = CloneBufferOptions.IndependentBuffers)
        {
            var clone = new LazinatorGeneralLocationAwareTree<T>()
            {
                OriginalIncludeChildrenMode = includeChildrenMode
            };
            clone = CompleteClone(this, clone, includeChildrenMode, cloneBufferOptions);
            return clone;
        }
        
        public override ILazinator AssignCloneProperties(ILazinator clone, IncludeChildrenMode includeChildrenMode)
        {
            base.AssignCloneProperties(clone, includeChildrenMode);
            LazinatorGeneralLocationAwareTree<T> typedClone = (LazinatorGeneralLocationAwareTree<T>) clone;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                if (Locations == null)
                {
                    typedClone.Locations = default(LazinatorDictionary<T, LazinatorList<WInt>>);
                }
                else
                {
                    typedClone.Locations = (LazinatorDictionary<T, LazinatorList<WInt>>) Locations.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer);
                }
            }
            
            return typedClone;
        }
        
        /* Properties */
        
        public override IEnumerable<(string propertyName, ILazinator descendant)> EnumerateLazinatorDescendants(Func<ILazinator, bool> matchCriterion, bool stopExploringBelowMatch, Func<ILazinator, bool> exploreCriterion, bool exploreOnlyDeserializedChildren, bool enumerateNulls)
        {
            foreach (var inheritedYield in base.EnumerateLazinatorDescendants(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
            {
                yield return inheritedYield;
            }
            if (enumerateNulls && (!exploreOnlyDeserializedChildren || _Locations_Accessed) && Locations == null)
            {
                yield return ("Locations", default);
            }
            else
            {
                if ((!exploreOnlyDeserializedChildren && Locations != null) || (_Locations_Accessed && _Locations != null))
                {
                    bool isMatch = matchCriterion == null || matchCriterion(Locations);
                    bool shouldExplore = exploreCriterion == null || exploreCriterion(Locations);
                    if (isMatch)
                    {
                        yield return ("Locations", Locations);
                    }
                    if ((!stopExploringBelowMatch || !isMatch) && shouldExplore)
                    {
                        foreach (var toYield in Locations.EnumerateLazinatorDescendants(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
                        {
                            yield return ("Locations" + "." + toYield.propertyName, toYield.descendant);
                        }
                    }
                }
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
        
        public override ILazinator ForEachLazinator(Func<ILazinator, ILazinator> changeFunc, bool exploreOnlyDeserializedChildren, bool changeThisLevel)
        {
            base.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, false);
            if ((!exploreOnlyDeserializedChildren && Locations != null) || (_Locations_Accessed && _Locations != null))
            {
                _Locations = (LazinatorDictionary<T, LazinatorList<WInt>>) _Locations.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, true);
            }
            if (changeThisLevel)
            {
                return changeFunc(this);
            }
            return this;
        }
        
        public override void FreeInMemoryObjects()
        {
            base.FreeInMemoryObjects();
            _Locations = default;
            _Locations_Accessed = false;
            IsDirty = false;
            DescendantIsDirty = false;
            HasChanged = false;
            DescendantHasChanged = false;
        }
        
        /* Conversion */
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public override int LazinatorUniqueID => 116;
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected override bool ContainsOpenGenericParameters => true;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public override LazinatorGenericIDType LazinatorGenericID
        {
            get
            {
                if (_LazinatorGenericID.IsEmpty)
                {
                    _LazinatorGenericID = DeserializationFactory.Instance.GetUniqueIDListForGenericType(116, new Type[] { typeof(T) });
                }
                return _LazinatorGenericID;
            }
            set
            {
                _LazinatorGenericID = value;
            }
        }
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public override int LazinatorObjectVersion { get; set; } = 0;
        
        
        public override void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            base.ConvertFromBytesAfterHeader(OriginalIncludeChildrenMode, serializedVersionNumber, ref bytesSoFar);
            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
            _Locations_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            _LazinatorLocationAwareTree_T_EndByteIndex = bytesSoFar;
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
                
                if (_Item_Accessed && _Item != null && _Item.IsStruct && (_Item.IsDirty || _Item.DescendantIsDirty))
                {
                    _Item_Accessed = false;
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
            if (_Locations_Accessed && _Locations != null)
            {
                _Locations.UpdateStoredBuffer(ref writer, startPosition + _Locations_ByteIndex + sizeof(int), _Locations_ByteLength - sizeof(int), IncludeChildrenMode.IncludeAllChildren, true);
            }
        }
        
        
        protected override void WritePropertiesIntoBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer, bool includeUniqueID)
        {
            int startPosition = writer.Position;
            int startOfObjectPosition = 0;
            base.WritePropertiesIntoBuffer(ref writer, includeChildrenMode, verifyCleanness, updateStoredBuffer, includeUniqueID);
            // write properties
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                if ((includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode) && !_Locations_Accessed)
                {
                    var deserialized = Locations;
                }
                WriteChild(ref writer, ref _Locations, includeChildrenMode, _Locations_Accessed, () => GetChildSlice(LazinatorMemoryStorage, _Locations_ByteIndex, _Locations_ByteLength, false, false, null), verifyCleanness, updateStoredBuffer, false, false, this);
            }
            if (updateStoredBuffer)
            {
                _Locations_ByteIndex = startOfObjectPosition - startPosition;
            }
            if (updateStoredBuffer)
            {
                _LazinatorLocationAwareTree_T_EndByteIndex = writer.Position - startPosition;
            }
        }
        
    }
}
