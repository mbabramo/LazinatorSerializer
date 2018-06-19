//fe63ee03-7fde-00cc-7ff9-19e91b00f2da
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.123
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Lazinator.Collections.Dictionary
{
    using Lazinator.Attributes;
    using Lazinator.Buffers;
    using Lazinator.Collections;
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
    internal partial class DictionaryBucket<TKey, TValue> : ILazinator
    {
        /* Serialization, deserialization, and object relationships */
        
        protected ILazinator _LazinatorParentClass;
        public DictionaryBucket() : base()
        {
        }
        
        public virtual ILazinator LazinatorParentClass 
        { 
            get => _LazinatorParentClass;
            set
            {
                _LazinatorParentClass = value;
                if (value != null && (IsDirty || DescendantIsDirty))
                {
                    value.DescendantIsDirty = true;
                }
            }
        }
        
        protected IncludeChildrenMode OriginalIncludeChildrenMode;
        
        public virtual int Deserialize()
        {
            ResetAccessedProperties();
            int bytesSoFar = 0;
            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
            if (span.Length == 0)
            {
                return 0;
            }
            
            LazinatorGenericID = GetGenericIDIfApplicable(ContainsOpenGenericParameters, LazinatorUniqueID, span, ref bytesSoFar);
            
            int lazinatorLibraryVersion = span.ToDecompressedInt(ref bytesSoFar);
            
            int serializedVersionNumber = span.ToDecompressedInt(ref bytesSoFar);
            
            OriginalIncludeChildrenMode = (IncludeChildrenMode)span.ToByte(ref bytesSoFar);
            
            ConvertFromBytesAfterHeader(OriginalIncludeChildrenMode, serializedVersionNumber, ref bytesSoFar);
            return bytesSoFar;
        }
        
        public virtual MemoryInBuffer SerializeNewBuffer(IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
            return EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, verifyCleanness, IsDirty, DescendantIsDirty, false, LazinatorObjectBytes, (StreamManuallyDelegate) EncodeToNewBuffer);
        }
        
        protected virtual MemoryInBuffer EncodeToNewBuffer(IncludeChildrenMode includeChildrenMode, bool verifyCleanness) => LazinatorUtilities.EncodeToNewBinaryBufferWriter(this, includeChildrenMode, verifyCleanness);
        
        public virtual ILazinator CloneLazinator()
        {
            return CloneLazinator(OriginalIncludeChildrenMode);
        }
        
        public virtual ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode)
        {
            MemoryInBuffer bytes = EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, false, IsDirty, DescendantIsDirty, false, LazinatorObjectBytes, (StreamManuallyDelegate)EncodeToNewBuffer);
            var clone = new DictionaryBucket<TKey, TValue>()
            {
                LazinatorParentClass = LazinatorParentClass,
                OriginalIncludeChildrenMode = includeChildrenMode,
                HierarchyBytes = bytes,
            };
            clone.LazinatorParentClass = null;
            return clone;
        }
        
        protected bool _IsDirty;
        public virtual bool IsDirty
        {
            [DebuggerStepThrough]
            get => _IsDirty;
            [DebuggerStepThrough]
            set
            {
                if (_IsDirty != value)
                {
                    _IsDirty = value;
                    if (_IsDirty)
                    {
                        InformParentOfDirtiness();
                    }
                }
            }
        }
        
        public virtual InformParentOfDirtinessDelegate InformParentOfDirtinessDelegate { get; set; }
        public virtual void InformParentOfDirtiness()
        {
            if (InformParentOfDirtinessDelegate == null)
            {
                if (LazinatorParentClass != null)
                {
                    LazinatorParentClass.DescendantIsDirty = true;
                }
            }
            else
            {
                InformParentOfDirtinessDelegate();
            }
        }
        
        protected bool _DescendantIsDirty;
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
                    if (_DescendantIsDirty && LazinatorParentClass != null)
                    {
                        LazinatorParentClass.DescendantIsDirty = true;
                    }
                }
            }
        }
        
        private MemoryInBuffer _HierarchyBytes;
        public virtual MemoryInBuffer HierarchyBytes
        {
            set
            {
                _HierarchyBytes = value;
                LazinatorObjectBytes = value.FilledMemory;
            }
        }
        
        protected ReadOnlyMemory<byte> _LazinatorObjectBytes;
        public virtual ReadOnlyMemory<byte> LazinatorObjectBytes
        {
            get => _LazinatorObjectBytes;
            set
            {
                _LazinatorObjectBytes = value;
                int length = Deserialize();
                _LazinatorObjectBytes = _LazinatorObjectBytes.Slice(0, length);
            }
        }
        
        public virtual void LazinatorConvertToBytes()
        {
            if (!IsDirty && !DescendantIsDirty && _LazinatorObjectBytes.Length > 0)
            {
                return;
            }
            MemoryInBuffer bytes = EncodeOrRecycleToNewBuffer(IncludeChildrenMode.IncludeAllChildren, OriginalIncludeChildrenMode, false, IsDirty, DescendantIsDirty, false, LazinatorObjectBytes, (StreamManuallyDelegate)EncodeToNewBuffer);
            _LazinatorObjectBytes = bytes.FilledMemory;
        }
        
        public virtual int GetByteLength()
        {
            LazinatorConvertToBytes();
            return _LazinatorObjectBytes.Length;
        }
        
        public virtual uint GetBinaryHashCode32()
        {
            LazinatorConvertToBytes();
            return FarmhashByteSpans.Hash32(LazinatorObjectBytes.Span);
        }
        
        public virtual ulong GetBinaryHashCode64()
        {
            LazinatorConvertToBytes();
            return FarmhashByteSpans.Hash64(LazinatorObjectBytes.Span);
        }
        
        public virtual Guid GetBinaryHashCode128()
        {
            LazinatorConvertToBytes();
            return FarmhashByteSpans.Hash128(LazinatorObjectBytes.Span);
        }
        
        /* Property definitions */
        
        protected int _Keys_ByteIndex;
        protected int _Values_ByteIndex;
        protected virtual int _Keys_ByteLength => _Values_ByteIndex - _Keys_ByteIndex;
        private int _DictionaryBucket_TKey_TValue_EndByteIndex;
        protected virtual int _Values_ByteLength => _DictionaryBucket_TKey_TValue_EndByteIndex - _Values_ByteIndex;
        
        private bool _Initialized;
        public bool Initialized
        {
            [DebuggerStepThrough]
            get
            {
                return _Initialized;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _Initialized = value;
            }
        }
        private LazinatorList<TKey> _Keys;
        public virtual LazinatorList<TKey> Keys
        {
            [DebuggerStepThrough]
            get
            {
                if (!_Keys_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _Keys = default(LazinatorList<TKey>);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _Keys_ByteIndex, _Keys_ByteLength, false, false, null);
                        
                        _Keys = DeserializationFactory.Instance.CreateBaseOrDerivedType(51, () => new LazinatorList<TKey>(), childData, this); 
                    }
                    _Keys_Accessed = true;
                }
                return _Keys;
            }
            [DebuggerStepThrough]
            set
            {
                if (value != null)
                {
                    value.LazinatorParentClass = this;
                    value.IsDirty = true;
                }
                IsDirty = true;
                _Keys = value;
                _Keys_Accessed = true;
            }
        }
        protected bool _Keys_Accessed;
        private LazinatorList<TValue> _Values;
        public virtual LazinatorList<TValue> Values
        {
            [DebuggerStepThrough]
            get
            {
                if (!_Values_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _Values = default(LazinatorList<TValue>);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _Values_ByteIndex, _Values_ByteLength, false, false, null);
                        
                        _Values = DeserializationFactory.Instance.CreateBaseOrDerivedType(51, () => new LazinatorList<TValue>(), childData, this); 
                    }
                    _Values_Accessed = true;
                }
                return _Values;
            }
            [DebuggerStepThrough]
            set
            {
                if (value != null)
                {
                    value.LazinatorParentClass = this;
                    value.IsDirty = true;
                }
                IsDirty = true;
                _Values = value;
                _Values_Accessed = true;
            }
        }
        protected bool _Values_Accessed;
        
        public IEnumerable<ILazinator> GetDirtyNodes() => EnumerateLazinatorNodes(true, null, null, false);
        
        public IEnumerable<ILazinator> EnumerateLazinatorNodes(bool exploreOnlyDeserializedChildren, Func<ILazinator, bool> exploreCriterion, Func<ILazinator, bool> yieldCriterion, bool onlyHighestMatch)
        {
            if (IsDirty)
            {
                bool yield = (yieldCriterion == null) ? true : yieldCriterion(this);
                if (yield)
                {
                    yield return this;
                    if (onlyHighestMatch)
                    {
                        yield break;
                    }
                }
            }
            bool explore = (exploreCriterion == null) ? true : exploreCriterion(this);
            if (explore && DescendantIsDirty)
            {
                foreach (ILazinator dirty in EnumerateLazinatorNodes_Helper(exploreOnlyDeserializedChildren, exploreCriterion, yieldCriterion, onlyHighestMatch))
                {
                    yield return dirty;
                }
            }
        }
        
        protected virtual IEnumerable<ILazinator> EnumerateLazinatorNodes_Helper(bool exploreOnlyDeserializedChildren, Func<ILazinator, bool> exploreCriterion, Func<ILazinator, bool> yieldCriterion, bool onlyHighestMatch)
        {
            if (_Keys_Accessed && _Keys != null && (_Keys.IsDirty || _Keys.DescendantIsDirty))
            {
                foreach (ILazinator toYield in _Keys.EnumerateLazinatorNodes(exploreOnlyDeserializedChildren, exploreCriterion, yieldCriterion, onlyHighestMatch))
                {
                    yield return toYield;
                }
            }
            if (_Values_Accessed && _Values != null && (_Values.IsDirty || _Values.DescendantIsDirty))
            {
                foreach (ILazinator toYield in _Values.EnumerateLazinatorNodes(exploreOnlyDeserializedChildren, exploreCriterion, yieldCriterion, onlyHighestMatch))
                {
                    yield return toYield;
                }
            }
            yield break;
        }
        
        protected virtual void ResetAccessedProperties()
        {
            _Keys_Accessed = _Values_Accessed = false;
        }
        
        /* Conversion */
        
        public virtual int LazinatorUniqueID => 98;
        
        protected virtual bool ContainsOpenGenericParameters => true;
        protected virtual LazinatorGenericIDType _LazinatorGenericID { get; set; }
        public virtual LazinatorGenericIDType LazinatorGenericID
        {
            get
            {
                if (_LazinatorGenericID.IsEmpty)
                {
                    _LazinatorGenericID = DeserializationFactory.Instance.GetUniqueIDListForGenericType(98, new Type[] { typeof(TKey), typeof(TValue) });
                }
                return _LazinatorGenericID;
            }
            set
            {
                _LazinatorGenericID = value;
            }
        }
        
        public virtual int LazinatorObjectVersion { get; set; } = 0;
        
        
        public virtual void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
            _Initialized = span.ToBoolean(ref bytesSoFar);
            _Keys_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            _Values_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            _DictionaryBucket_TKey_TValue_EndByteIndex = bytesSoFar;
        }
        
        public virtual void SerializeExistingBuffer(BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
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
                _DescendantIsDirty = includeChildrenMode != IncludeChildrenMode.IncludeAllChildren && ((_Keys_Accessed && _Keys != null && (Keys.IsDirty || Keys.DescendantIsDirty)) || (_Values_Accessed && _Values != null && (Values.IsDirty || Values.DescendantIsDirty)));
                
                _LazinatorObjectBytes = writer.Slice(startPosition);
            }
        }
        protected virtual void WritePropertiesIntoBuffer(BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer, bool includeUniqueID)
        {
            int startPosition = writer.Position;
            int startOfObjectPosition = 0;
            // header information
            if (includeUniqueID)
            {
                if (LazinatorGenericID.IsEmpty)
                {
                    CompressedIntegralTypes.WriteCompressedInt(writer, LazinatorUniqueID);
                }
                else
                {
                    WriteLazinatorGenericID(writer, LazinatorGenericID);
                }
            }
            CompressedIntegralTypes.WriteCompressedInt(writer, Lazinator.Support.LazinatorVersionInfo.LazinatorIntVersion);
            CompressedIntegralTypes.WriteCompressedInt(writer, LazinatorObjectVersion);
            writer.Write((byte)includeChildrenMode);
            // write properties
            WriteUncompressedPrimitives.WriteBool(writer, _Initialized);
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                WriteChild(writer, _Keys, includeChildrenMode, _Keys_Accessed, () => GetChildSlice(LazinatorObjectBytes, _Keys_ByteIndex, _Keys_ByteLength, false, false, null), verifyCleanness, updateStoredBuffer, false, false, this);
            }
            if (updateStoredBuffer)
            {
                _Keys_ByteIndex = startOfObjectPosition - startPosition;
            }
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                WriteChild(writer, _Values, includeChildrenMode, _Values_Accessed, () => GetChildSlice(LazinatorObjectBytes, _Values_ByteIndex, _Values_ByteLength, false, false, null), verifyCleanness, updateStoredBuffer, false, false, this);
            }
            if (updateStoredBuffer)
            {
                _Values_ByteIndex = startOfObjectPosition - startPosition;
            }
            if (updateStoredBuffer)
            {
                _DictionaryBucket_TKey_TValue_EndByteIndex = writer.Position - startPosition;
            }
        }
        
    }
}
