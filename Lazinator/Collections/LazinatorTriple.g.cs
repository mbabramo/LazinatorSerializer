//9f269b38-8b33-2b1b-1019-df11a77c6f7e
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.123
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Lazinator.Collections
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
    public partial class LazinatorTriple<T, U, V> : ILazinator
    {
        /* Serialization, deserialization, and object relationships */
        
        protected ILazinator _LazinatorParentClass;
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
            var clone = new LazinatorTriple<T, U, V>()
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
        
        protected int _Item1_ByteIndex;
        protected int _Item2_ByteIndex;
        protected int _Item3_ByteIndex;
        protected virtual int _Item1_ByteLength => _Item2_ByteIndex - _Item1_ByteIndex;
        protected virtual int _Item2_ByteLength => _Item3_ByteIndex - _Item2_ByteIndex;
        private int _LazinatorTriple_T_U_V_EndByteIndex = 0;
        protected virtual int _Item3_ByteLength => _LazinatorTriple_T_U_V_EndByteIndex - _Item3_ByteIndex;
        
        private T _Item1;
        public virtual T Item1
        {
            [DebuggerStepThrough]
            get
            {
                if (!_Item1_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _Item1 = default(T);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _Item1_ByteIndex, _Item1_ByteLength, false, false, null);
                        if (childData.Length == 0)
                        {
                            _Item1 = default;
                        }
                        else _Item1 = new T()
                        {
                            LazinatorParentClass = this,
                            LazinatorObjectBytes = childData,
                        };
                    }
                    _Item1_Accessed = true;
                }
                return _Item1;
            }
            [DebuggerStepThrough]
            set
            {
                if (!System.Collections.Generic.EqualityComparer<T>.Default.Equals(value, default(T)))
                {
                    value.LazinatorParentClass = this;
                    value.IsDirty = true;
                }
                IsDirty = true;
                _Item1 = value;
                _Item1_Accessed = true;
            }
        }
        protected bool _Item1_Accessed;
        private U _Item2;
        public virtual U Item2
        {
            [DebuggerStepThrough]
            get
            {
                if (!_Item2_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _Item2 = default(U);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _Item2_ByteIndex, _Item2_ByteLength, false, false, null);
                        if (childData.Length == 0)
                        {
                            _Item2 = default;
                        }
                        else _Item2 = new U()
                        {
                            LazinatorParentClass = this,
                            LazinatorObjectBytes = childData,
                        };
                    }
                    _Item2_Accessed = true;
                }
                return _Item2;
            }
            [DebuggerStepThrough]
            set
            {
                if (!System.Collections.Generic.EqualityComparer<U>.Default.Equals(value, default(U)))
                {
                    value.LazinatorParentClass = this;
                    value.IsDirty = true;
                }
                IsDirty = true;
                _Item2 = value;
                _Item2_Accessed = true;
            }
        }
        protected bool _Item2_Accessed;
        private V _Item3;
        public virtual V Item3
        {
            [DebuggerStepThrough]
            get
            {
                if (!_Item3_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _Item3 = default(V);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _Item3_ByteIndex, _Item3_ByteLength, false, false, null);
                        if (childData.Length == 0)
                        {
                            _Item3 = default;
                        }
                        else _Item3 = new V()
                        {
                            LazinatorParentClass = this,
                            LazinatorObjectBytes = childData,
                        };
                    }
                    _Item3_Accessed = true;
                }
                return _Item3;
            }
            [DebuggerStepThrough]
            set
            {
                if (!System.Collections.Generic.EqualityComparer<V>.Default.Equals(value, default(V)))
                {
                    value.LazinatorParentClass = this;
                    value.IsDirty = true;
                }
                IsDirty = true;
                _Item3 = value;
                _Item3_Accessed = true;
            }
        }
        protected bool _Item3_Accessed;
        
        public IEnumerable<ILazinator> GetDirtyNodes() => EnumerateLazinatorNodes(x => x.IsDirty, false, x => x.IsDirty || x.DescendantIsDirty, true);
        
        public IEnumerable<ILazinator> EnumerateLazinatorNodes(Func<ILazinator, bool> matchCriterion, bool stopExploringBelowMatch, Func<ILazinator, bool> exploreCriterion, bool exploreOnlyDeserializedChildren)
        {
            bool match = (matchCriterion == null) ? true : matchCriterion(this);
            if (match)
            {
                yield return this;
            }
            bool explore = (!match || !stopExploringBelowMatch) && ((exploreCriterion == null) ? true : exploreCriterion(this));
            if (explore)
            {
                foreach (ILazinator dirty in EnumerateLazinatorNodes_Helper(exploreOnlyDeserializedChildren, matchCriterion, exploreCriterion, stopExploringBelowMatch))
                {
                    yield return dirty;
                }
            }
        }
        
        protected virtual IEnumerable<ILazinator> EnumerateLazinatorNodes_Helper(bool exploreOnlyDeserializedChildren, Func<ILazinator, bool> matchCriterion, Func<ILazinator, bool> exploreCriterion, bool stopExploringBelowMatch)
        {
            if (!exploreOnlyDeserializedChildren || (_Item1_Accessed && !System.Collections.Generic.EqualityComparer<T>.Default.Equals(_Item1, default(T))))
            {
                foreach (ILazinator toYield in _Item1.EnumerateLazinatorNodes(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren))
                {
                    yield return toYield;
                }
            }
            if (!exploreOnlyDeserializedChildren || (_Item2_Accessed && !System.Collections.Generic.EqualityComparer<U>.Default.Equals(_Item2, default(U))))
            {
                foreach (ILazinator toYield in _Item2.EnumerateLazinatorNodes(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren))
                {
                    yield return toYield;
                }
            }
            if (!exploreOnlyDeserializedChildren || (_Item3_Accessed && !System.Collections.Generic.EqualityComparer<V>.Default.Equals(_Item3, default(V))))
            {
                foreach (ILazinator toYield in _Item3.EnumerateLazinatorNodes(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren))
                {
                    yield return toYield;
                }
            }
            yield break;
        }
        
        protected virtual void ResetAccessedProperties()
        {
            
        }
        
        /* Conversion */
        
        public virtual int LazinatorUniqueID => 106;
        
        protected virtual bool ContainsOpenGenericParameters => true;
        protected virtual LazinatorGenericIDType _LazinatorGenericID { get; set; }
        public virtual LazinatorGenericIDType LazinatorGenericID
        {
            get
            {
                if (_LazinatorGenericID.IsEmpty)
                {
                    _LazinatorGenericID = DeserializationFactory.Instance.GetUniqueIDListForGenericType(106, new Type[] { typeof(T), typeof(U), typeof(V) });
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
            _Item1_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            _Item2_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            _Item3_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            _LazinatorTriple_T_U_V_EndByteIndex = bytesSoFar;
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
                _DescendantIsDirty = includeChildrenMode != IncludeChildrenMode.IncludeAllChildren && ((_Item1_Accessed && !System.Collections.Generic.EqualityComparer<T>.Default.Equals(_Item1, default(T)) && (Item1.IsDirty || Item1.DescendantIsDirty)) || (_Item2_Accessed && !System.Collections.Generic.EqualityComparer<U>.Default.Equals(_Item2, default(U)) && (Item2.IsDirty || Item2.DescendantIsDirty)) || (_Item3_Accessed && !System.Collections.Generic.EqualityComparer<V>.Default.Equals(_Item3, default(V)) && (Item3.IsDirty || Item3.DescendantIsDirty)));
                
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
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                WriteChild(writer, _Item1, includeChildrenMode, _Item1_Accessed, () => GetChildSlice(LazinatorObjectBytes, _Item1_ByteIndex, _Item1_ByteLength, false, false, null), verifyCleanness, updateStoredBuffer, false, false, this);
            }
            if (updateStoredBuffer)
            {
                _Item1_ByteIndex = startOfObjectPosition - startPosition;
            }
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                WriteChild(writer, _Item2, includeChildrenMode, _Item2_Accessed, () => GetChildSlice(LazinatorObjectBytes, _Item2_ByteIndex, _Item2_ByteLength, false, false, null), verifyCleanness, updateStoredBuffer, false, false, this);
            }
            if (updateStoredBuffer)
            {
                _Item2_ByteIndex = startOfObjectPosition - startPosition;
            }
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                WriteChild(writer, _Item3, includeChildrenMode, _Item3_Accessed, () => GetChildSlice(LazinatorObjectBytes, _Item3_ByteIndex, _Item3_ByteLength, false, false, null), verifyCleanness, updateStoredBuffer, false, false, this);
            }
            if (updateStoredBuffer)
            {
                _Item3_ByteIndex = startOfObjectPosition - startPosition;
            }
            if (updateStoredBuffer)
            {
                _LazinatorTriple_T_U_V_EndByteIndex = writer.Position - startPosition;
            }
        }
        
    }
}
