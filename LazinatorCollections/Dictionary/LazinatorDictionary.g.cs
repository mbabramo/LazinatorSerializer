//325dbb4a-d36a-2194-1c93-89aa57eace3b
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.389
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

#nullable disable
namespace LazinatorCollections.Dictionary
{
    using Lazinator.Attributes;
    using Lazinator.Buffers;
    using Lazinator.Core;
    using Lazinator.Exceptions;
    using Lazinator.Support;
    using LazinatorCollections;
    using System;
    using System.Buffers;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;
    using static Lazinator.Core.LazinatorUtilities;
    
    [Autogenerated]
    public partial class LazinatorDictionary<TKey, TValue> : ILazinator
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsStruct => false;
        
        /* Property definitions */
        
        protected int _Buckets_ByteIndex;
        private int _LazinatorDictionary_TKey_TValue_EndByteIndex;
        protected virtual int _Buckets_ByteLength => _LazinatorDictionary_TKey_TValue_EndByteIndex - _Buckets_ByteIndex;
        
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected int _Count;
        public int Count
        {
            [DebuggerStepThrough]
            get
            {
                return _Count;
            }
            [DebuggerStepThrough]
            private set
            {
                IsDirty = true;
                _Count = value;
            }
        }
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected LazinatorList<DictionaryBucket<TKey, TValue>> _Buckets;
        internal virtual LazinatorList<DictionaryBucket<TKey, TValue>> Buckets
        {
            [DebuggerStepThrough]
            get
            {
                if (!_Buckets_Accessed)
                {
                    Lazinate_Buckets();
                } 
                return _Buckets;
            }
            [DebuggerStepThrough]
            set
            {
                if (_Buckets != null)
                {
                    _Buckets.LazinatorParents = _Buckets.LazinatorParents.WithRemoved(this);
                }
                if (value != null)
                {
                    value.LazinatorParents = value.LazinatorParents.WithAdded(this);
                }
                
                IsDirty = true;
                DescendantIsDirty = true;
                _Buckets = value;
                _Buckets_Accessed = true;
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected bool _Buckets_Accessed;
        private void Lazinate_Buckets()
        {
            if (LazinatorObjectBytes.Length == 0)
            {
                _Buckets = null;
            }
            else
            {
                LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _Buckets_ByteIndex, _Buckets_ByteLength, false, false, null);
                
                _Buckets = DeserializationFactory.Instance.CreateBaseOrDerivedType(201, (c, p) => new LazinatorList<DictionaryBucket<TKey, TValue>>(c, p), childData, this); 
            }
            
            _Buckets_Accessed = true;
        }
        
        
        /* Serialization, deserialization, and object relationships */
        
        public LazinatorDictionary(IncludeChildrenMode originalIncludeChildrenMode = IncludeChildrenMode.IncludeAllChildren)
        {
            OriginalIncludeChildrenMode = originalIncludeChildrenMode;
        }
        
        public LazinatorDictionary(LazinatorMemory serializedBytes, ILazinator parent = null)
        {
            LazinatorParents = new LazinatorParentsCollection(parent);
            DeserializeLazinator(serializedBytes);
            HasChanged = false;
            DescendantHasChanged = false;
        }
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public virtual LazinatorParentsCollection LazinatorParents { get; set; }
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public virtual IncludeChildrenMode OriginalIncludeChildrenMode { get; set; }
        
        public virtual int Deserialize()
        {
            FreeInMemoryObjects();
            int bytesSoFar = 0;
            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
            if (span.Length == 0)
            {
                return 0;
            }
            
            ReadGenericIDIfApplicable(ContainsOpenGenericParameters, LazinatorUniqueID, span, ref bytesSoFar);
            
            int lazinatorLibraryVersion = span.ToDecompressedInt(ref bytesSoFar);
            
            int serializedVersionNumber = span.ToDecompressedInt(ref bytesSoFar);
            
            OriginalIncludeChildrenMode = (IncludeChildrenMode)span.ToByte(ref bytesSoFar);
            
            ConvertFromBytesAfterHeader(OriginalIncludeChildrenMode, serializedVersionNumber, ref bytesSoFar);
            return bytesSoFar;
        }
        
        public virtual LazinatorMemory SerializeLazinator(IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer) 
        {
            if (LazinatorMemoryStorage.IsEmpty || includeChildrenMode != OriginalIncludeChildrenMode || (verifyCleanness || IsDirty || (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && DescendantIsDirty)))
            {
                return EncodeToNewBuffer(includeChildrenMode, verifyCleanness, updateStoredBuffer);
            }
            BinaryBufferWriter writer = new BinaryBufferWriter(LazinatorMemoryStorage.Length);
            writer.Write(LazinatorMemoryStorage.Span);
            return writer.LazinatorMemory;
        }
        
        protected virtual LazinatorMemory EncodeToNewBuffer(IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer) 
        {
            int bufferSize = LazinatorMemoryStorage.Length == 0 ? ExpandableBytes.DefaultMinBufferSize : LazinatorMemoryStorage.Length;
            BinaryBufferWriter writer = new BinaryBufferWriter(bufferSize);
            SerializeExistingBuffer(ref writer, includeChildrenMode, verifyCleanness, updateStoredBuffer);
            return writer.LazinatorMemory;
        }
        
        public virtual ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode = IncludeChildrenMode.IncludeAllChildren, CloneBufferOptions cloneBufferOptions = CloneBufferOptions.IndependentBuffers)
        {
            LazinatorDictionary<TKey, TValue> clone;
            if (cloneBufferOptions == CloneBufferOptions.NoBuffer)
            {
                clone = new LazinatorDictionary<TKey, TValue>(includeChildrenMode);
                clone.LazinatorObjectVersion = LazinatorObjectVersion;
                clone = (LazinatorDictionary<TKey, TValue>)AssignCloneProperties(clone, includeChildrenMode);
            }
            else
            {
                LazinatorMemory bytes = EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, false, IsDirty, DescendantIsDirty, false, LazinatorMemoryStorage, false, this);
                clone = new LazinatorDictionary<TKey, TValue>(bytes);
            }
            return clone;
        }
        
        public virtual ILazinator AssignCloneProperties(ILazinator clone, IncludeChildrenMode includeChildrenMode)
        {
            clone.FreeInMemoryObjects();
            LazinatorDictionary<TKey, TValue> typedClone = (LazinatorDictionary<TKey, TValue>) clone;
            typedClone.Count = Count;/*Location572*/
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                if (Buckets == null)
                {
                    typedClone.Buckets = null;/*Location574*//*Location575*/
                }
                else
                {
                    typedClone.Buckets = (LazinatorList<DictionaryBucket<TKey, TValue>>) Buckets.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer);/*Location573*/
                }
                
            }
            
            
            return typedClone;
        }
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public virtual bool HasChanged { get; set; }
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected bool _IsDirty;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public virtual bool IsDirty
        {
            [DebuggerStepThrough]
            get => _IsDirty|| LazinatorObjectBytes.Length == 0;
            [DebuggerStepThrough]
            set
            {
                if (_IsDirty != value)
                {
                    _IsDirty = value;
                    if (_IsDirty)
                    {
                        LazinatorParents.InformParentsOfDirtiness();
                        HasChanged = true;
                    }
                }
            }
        }
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected bool _DescendantHasChanged;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public virtual bool DescendantHasChanged
        {
            [DebuggerStepThrough]
            get => _DescendantHasChanged;
            [DebuggerStepThrough]
            set
            {
                _DescendantHasChanged = value;
            }
        }
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected bool _DescendantIsDirty;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public virtual bool DescendantIsDirty
        {
            [DebuggerStepThrough]
            get => _DescendantIsDirty;
            [DebuggerStepThrough]
            set
            {
                if (_DescendantIsDirty != value)
                {
                    _DescendantIsDirty = value;
                    if (_DescendantIsDirty)
                    {
                        LazinatorParents.InformParentsOfDirtiness();
                        _DescendantHasChanged = true;
                    }
                }
            }
        }
        
        public virtual void DeserializeLazinator(LazinatorMemory serializedBytes)
        {
            LazinatorMemoryStorage = serializedBytes;
            int length = Deserialize();
            if (length != LazinatorMemoryStorage.Length)
            {
                LazinatorMemoryStorage = LazinatorMemoryStorage.Slice(0, length);
            }
        }
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public virtual LazinatorMemory LazinatorMemoryStorage
        {
            get;
            set;
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected virtual ReadOnlyMemory<byte> LazinatorObjectBytes => LazinatorMemoryStorage.IsEmpty ? LazinatorMemory.EmptyReadOnlyMemory : LazinatorMemoryStorage.Memory;
        
        public virtual void UpdateStoredBuffer()
        {
            if (!IsDirty && !DescendantIsDirty && LazinatorObjectBytes.Length > 0 && OriginalIncludeChildrenMode == IncludeChildrenMode.IncludeAllChildren)
            {
                return;
            }
            var previousBuffer = LazinatorMemoryStorage;
            if (LazinatorMemoryStorage.IsEmpty || IncludeChildrenMode.IncludeAllChildren != OriginalIncludeChildrenMode || (IsDirty || DescendantIsDirty))
            {
                LazinatorMemoryStorage = EncodeToNewBuffer(IncludeChildrenMode.IncludeAllChildren, false, true);
            }
            else
            {
                BinaryBufferWriter writer = new BinaryBufferWriter(LazinatorMemoryStorage.Length);
                writer.Write(LazinatorMemoryStorage.Span);
                LazinatorMemoryStorage = writer.LazinatorMemory;
            }
            OriginalIncludeChildrenMode = IncludeChildrenMode.IncludeAllChildren;
            if (!LazinatorParents.Any())
            {
                previousBuffer.Dispose();
            }
        }
        
        public virtual int GetByteLength()
        {
            UpdateStoredBuffer();
            return LazinatorObjectBytes.Length;
        }
        
        public virtual bool NonBinaryHash32 => false;
        
        
        public IEnumerable<ILazinator> EnumerateLazinatorNodes(Func<ILazinator, bool> matchCriterion, bool stopExploringBelowMatch, Func<ILazinator, bool> exploreCriterion, bool exploreOnlyDeserializedChildren, bool enumerateNulls)
        {
            bool match = (matchCriterion == null) ? true : matchCriterion(this);
            bool explore = (!match || !stopExploringBelowMatch) && ((exploreCriterion == null) ? true : exploreCriterion(this));
            if (match)
            {
                yield return this;
            }
            if (explore)
            {
                foreach (var item in EnumerateLazinatorDescendants(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
                {
                    yield return item.descendant;
                }
            }
        }
        
        public virtual IEnumerable<(string propertyName, ILazinator descendant)> EnumerateLazinatorDescendants(Func<ILazinator, bool> matchCriterion, bool stopExploringBelowMatch, Func<ILazinator, bool> exploreCriterion, bool exploreOnlyDeserializedChildren, bool enumerateNulls)
        {
            if (enumerateNulls && (!exploreOnlyDeserializedChildren || _Buckets_Accessed) && Buckets == null)
            {
                yield return ("Buckets", default);
            }
            else
            {
                if ((!exploreOnlyDeserializedChildren && Buckets != null) || (_Buckets_Accessed && _Buckets != null))
                {
                    bool isMatch_Buckets = matchCriterion == null || matchCriterion(Buckets);
                    bool shouldExplore_Buckets = exploreCriterion == null || exploreCriterion(Buckets);
                    if (isMatch_Buckets)
                    {
                        yield return ("Buckets", Buckets);
                    }
                    if ((!stopExploringBelowMatch || !isMatch_Buckets) && shouldExplore_Buckets)
                    {
                        foreach (var toYield in Buckets.EnumerateLazinatorDescendants(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
                        {
                            yield return ("Buckets" + "." + toYield.propertyName, toYield.descendant);
                        }
                    }
                }
                
            }
            
            yield break;
        }
        
        
        public virtual IEnumerable<(string propertyName, object descendant)> EnumerateNonLazinatorProperties()
        {
            yield return ("Count", (object)Count);
            yield break;
        }
        
        public virtual ILazinator ForEachLazinator(Func<ILazinator, ILazinator> changeFunc, bool exploreOnlyDeserializedChildren, bool changeThisLevel)
        {
            if ((!exploreOnlyDeserializedChildren && Buckets != null) || (_Buckets_Accessed && _Buckets != null))
            {
                _Buckets = (LazinatorList<DictionaryBucket<TKey, TValue>>) _Buckets.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, true);
            }
            if (changeThisLevel && changeFunc != null)
            {
                return changeFunc(this);
            }
            return this;
        }
        
        public virtual void FreeInMemoryObjects()
        {
            _Buckets = default;
            _Buckets_Accessed = false;
            IsDirty = false;
            DescendantIsDirty = false;
            HasChanged = false;
            DescendantHasChanged = false;
        }
        
        /* Conversion */
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public virtual int LazinatorUniqueID => 211;
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected virtual bool ContainsOpenGenericParameters => true;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public virtual LazinatorGenericIDType LazinatorGenericID => LazinatorGenericIDType.GetCachedForType<LazinatorDictionary<TKey, TValue>>(() => DeserializationFactory.Instance.GetUniqueIDListForGenericType(211, new Type[] { typeof(TKey), typeof(TValue) }));
        
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public virtual int LazinatorObjectVersion { get; set; } = 0;
        
        
        public virtual void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
            _Count = span.ToDecompressedInt(ref bytesSoFar);
            _Buckets_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            
            _LazinatorDictionary_TKey_TValue_EndByteIndex = bytesSoFar;
        }
        
        public virtual void SerializeExistingBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
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
        
        public virtual void UpdateStoredBuffer(ref BinaryBufferWriter writer, int startPosition, int length, IncludeChildrenMode includeChildrenMode, bool updateDeserializedChildren)
        {
            _IsDirty = false;
            if (includeChildrenMode == IncludeChildrenMode.IncludeAllChildren)
            {
                _DescendantIsDirty = false;
                if (updateDeserializedChildren)
                {
                    UpdateDeserializedChildren(ref writer, startPosition);
                }
                
            }
            else
            {
                ThrowHelper.ThrowCannotUpdateStoredBuffer();
            }
            
            var newBuffer = writer.Slice(startPosition, length);
            LazinatorMemoryStorage = newBuffer;
        }
        
        protected virtual void UpdateDeserializedChildren(ref BinaryBufferWriter writer, int startPosition)
        {
            if (_Buckets_Accessed && _Buckets != null)
            {
                Buckets.UpdateStoredBuffer(ref writer, startPosition + _Buckets_ByteIndex + sizeof(int), _Buckets_ByteLength - sizeof(int), IncludeChildrenMode.IncludeAllChildren, true);
            }
            
        }
        
        
        protected virtual void WritePropertiesIntoBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer, bool includeUniqueID)
        {
            int startPosition = writer.Position;
            int startOfObjectPosition = 0;
            // header information
            if (includeUniqueID)
            {
                if (!ContainsOpenGenericParameters)
                {
                    CompressedIntegralTypes.WriteCompressedInt(ref writer, LazinatorUniqueID);
                }
                else
                {
                    WriteLazinatorGenericID(ref writer, LazinatorGenericID);
                }
            }
            CompressedIntegralTypes.WriteCompressedInt(ref writer, Lazinator.Support.LazinatorVersionInfo.LazinatorIntVersion);
            CompressedIntegralTypes.WriteCompressedInt(ref writer, LazinatorObjectVersion);
            writer.Write((byte)includeChildrenMode);
            // write properties
            CompressedIntegralTypes.WriteCompressedInt(ref writer, _Count);
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                if ((includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode) && !_Buckets_Accessed)
                {
                    var deserialized = Buckets;
                }
                WriteChild(ref writer, ref _Buckets, includeChildrenMode, _Buckets_Accessed, () => GetChildSlice(LazinatorMemoryStorage, _Buckets_ByteIndex, _Buckets_ByteLength, false, false, null), verifyCleanness, updateStoredBuffer, false, false, this);
            }
            
            if (updateStoredBuffer)
            {
                _Buckets_ByteIndex = startOfObjectPosition - startPosition;
            }
            if (updateStoredBuffer)
            {
                _LazinatorDictionary_TKey_TValue_EndByteIndex = writer.Position - startPosition;
            }
        }
        
    }
}
