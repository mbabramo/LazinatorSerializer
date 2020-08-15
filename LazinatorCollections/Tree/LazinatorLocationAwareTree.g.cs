//5af2ba69-45c5-cd21-4f20-ed25a5a7fd2a
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.380
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

#nullable disable
namespace LazinatorCollections.Tree
{
    using Lazinator.Attributes;
    using Lazinator.Buffers;
    using Lazinator.Core;
    using Lazinator.Exceptions;
    using Lazinator.Support;
    using Lazinator.Wrappers;
    using LazinatorCollections;
    using LazinatorCollections.Dictionary;
    using System;
    using System.Buffers;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;
    using static Lazinator.Core.LazinatorUtilities;
    
    [Autogenerated]
    public partial class LazinatorLocationAwareTree<T> : LazinatorGeneralTree<T>, ILazinator
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
                    Lazinate_Locations();
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
        private void Lazinate_Locations()
        {
            if (LazinatorObjectBytes.Length == 0)
            {
                _Locations = null;
            }
            else
            {
                LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _Locations_ByteIndex, _Locations_ByteLength, false, false, null);
                
                _Locations = DeserializationFactory.Instance.CreateBaseOrDerivedType(211, () => new LazinatorDictionary<T, LazinatorList<WInt>>(LazinatorConstructorEnum.LazinatorConstructor), childData, this); 
            }
            
            _Locations_Accessed = true;
        }
        
        /* Clone overrides */
        
        public LazinatorLocationAwareTree(LazinatorConstructorEnum constructorEnum) : base(constructorEnum)
        {
        }
        
        public LazinatorLocationAwareTree(LazinatorMemory serializedBytes, ILazinator parent = null) : base(serializedBytes, parent)
        {
        }
        
        public override ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode = IncludeChildrenMode.IncludeAllChildren, CloneBufferOptions cloneBufferOptions = CloneBufferOptions.IndependentBuffers)
        {
            var clone = new LazinatorLocationAwareTree<T>(LazinatorConstructorEnum.LazinatorConstructor)
            {
                OriginalIncludeChildrenMode = includeChildrenMode
            };
            clone = CompleteClone(this, clone, includeChildrenMode, cloneBufferOptions);
            return clone;
        }
        
        public override ILazinator AssignCloneProperties(ILazinator clone, IncludeChildrenMode includeChildrenMode)
        {
            base.AssignCloneProperties(clone, includeChildrenMode);
            LazinatorLocationAwareTree<T> typedClone = (LazinatorLocationAwareTree<T>) clone;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                if (Locations == null)
                {
                    typedClone.Locations = null;
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
                    bool isMatch_Locations = matchCriterion == null || matchCriterion(Locations);
                    bool shouldExplore_Locations = exploreCriterion == null || exploreCriterion(Locations);
                    if (isMatch_Locations)
                    {
                        yield return ("Locations", Locations);
                    }
                    if ((!stopExploringBelowMatch || !isMatch_Locations) && shouldExplore_Locations)
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
            if (changeThisLevel && changeFunc != null)
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
        public override int LazinatorUniqueID => 218;
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected override bool ContainsOpenGenericParameters => true;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public override LazinatorGenericIDType LazinatorGenericID => LazinatorGenericIDType.GetCachedForType<LazinatorLocationAwareTree<T>>(() => DeserializationFactory.Instance.GetUniqueIDListForGenericType(218, new Type[] { typeof(T) }));
        
        
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
                ThrowHelper.ThrowCannotUpdateStoredBuffer();
            }
            
            var newBuffer = writer.Slice(startPosition, length);
            LazinatorMemoryStorage = newBuffer;
        }
        
        protected override void UpdateDeserializedChildren(ref BinaryBufferWriter writer, int startPosition)
        {
            base.UpdateDeserializedChildren(ref writer, startPosition);
            if (_Locations_Accessed && _Locations != null)
            {
                Locations.UpdateStoredBuffer(ref writer, startPosition + _Locations_ByteIndex + sizeof(int), _Locations_ByteLength - sizeof(int), IncludeChildrenMode.IncludeAllChildren, true);
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
