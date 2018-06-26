//56399613-2986-da11-e65b-1cd94eae137b
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.153
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
    using Lazinator.Wrappers;
    using LazinatorTests.Examples;
    using LazinatorTests.Examples.Abstract;
    using System;
    using System.Buffers;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;
    using static Lazinator.Core.LazinatorUtilities;
    
    [Autogenerated]
    public partial class OpenGenericStayingOpenContainer : ILazinator
    {
        /* Serialization, deserialization, and object relationships */
        
        public OpenGenericStayingOpenContainer() : base()
        {
        }
        
        public virtual LazinatorParentsCollection LazinatorParentsReference { get; set; }
        
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
            var clone = new OpenGenericStayingOpenContainer()
            {
                LazinatorParentsReference = LazinatorParentsReference,
                OriginalIncludeChildrenMode = includeChildrenMode,
                HierarchyBytes = bytes,
            };
            clone.LazinatorParentsReference = default;
            return clone;
        }
        
        public virtual bool HasChanged { get; set; }
        
        protected bool _IsDirty;
        public virtual bool IsDirty
        {
            [DebuggerStepThrough]
            get => _IsDirty;
            [DebuggerStepThrough]
            set
            {
                _IsDirty = value;
                if (_IsDirty)
                {
                    LazinatorParentsReference.InformParentsOfDirtiness();
                    HasChanged = true;
                }
            }
        }
        
        protected bool _DescendantHasChanged;
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
                    if (_DescendantIsDirty)
                    {
                        _DescendantHasChanged = true;
                        LazinatorParentsReference.InformParentsOfDirtiness();
                    }
                }
                if (_DescendantIsDirty)
                {
                    _DescendantHasChanged = true;
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
        
        protected int _ClosedGenericBase_ByteIndex;
        protected int _ClosedGenericFloat_ByteIndex;
        protected int _ClosedGenericFromBaseWithBase_ByteIndex;
        protected int _ClosedGenericInterface_ByteIndex;
        protected int _ClosedGenericNonexclusiveInterface_ByteIndex;
        protected virtual int _ClosedGenericBase_ByteLength => _ClosedGenericFloat_ByteIndex - _ClosedGenericBase_ByteIndex;
        protected virtual int _ClosedGenericFloat_ByteLength => _ClosedGenericFromBaseWithBase_ByteIndex - _ClosedGenericFloat_ByteIndex;
        protected virtual int _ClosedGenericFromBaseWithBase_ByteLength => _ClosedGenericInterface_ByteIndex - _ClosedGenericFromBaseWithBase_ByteIndex;
        protected virtual int _ClosedGenericInterface_ByteLength => _ClosedGenericNonexclusiveInterface_ByteIndex - _ClosedGenericInterface_ByteIndex;
        private int _OpenGenericStayingOpenContainer_EndByteIndex;
        protected virtual int _ClosedGenericNonexclusiveInterface_ByteLength => _OpenGenericStayingOpenContainer_EndByteIndex - _ClosedGenericNonexclusiveInterface_ByteIndex;
        
        private OpenGeneric<Base> _ClosedGenericBase;
        public OpenGeneric<Base> ClosedGenericBase
        {
            get
            {
                if (!_ClosedGenericBase_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _ClosedGenericBase = default(OpenGeneric<Base>);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _ClosedGenericBase_ByteIndex, _ClosedGenericBase_ByteLength, false, false, null);
                        
                        _ClosedGenericBase = DeserializationFactory.Instance.CreateBaseOrDerivedType(233, () => new OpenGeneric<Base>(), childData, this); 
                    }
                    _ClosedGenericBase_Accessed = true;
                } 
                return _ClosedGenericBase;
            }
            set
            {
                if (value != null)
                {
                    value.LazinatorParentsReference = value.LazinatorParentsReference.WithAdded(this);
                    value.IsDirty = true;
                }
                IsDirty = true;
                _ClosedGenericBase = value;
                _ClosedGenericBase_Accessed = true;
            }
        }
        protected bool _ClosedGenericBase_Accessed;
        private OpenGeneric<WFloat> _ClosedGenericFloat;
        public OpenGeneric<WFloat> ClosedGenericFloat
        {
            get
            {
                if (!_ClosedGenericFloat_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _ClosedGenericFloat = default(OpenGeneric<WFloat>);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _ClosedGenericFloat_ByteIndex, _ClosedGenericFloat_ByteLength, false, false, null);
                        
                        _ClosedGenericFloat = DeserializationFactory.Instance.CreateBaseOrDerivedType(233, () => new OpenGeneric<WFloat>(), childData, this); 
                    }
                    _ClosedGenericFloat_Accessed = true;
                } 
                return _ClosedGenericFloat;
            }
            set
            {
                if (value != null)
                {
                    value.LazinatorParentsReference = value.LazinatorParentsReference.WithAdded(this);
                    value.IsDirty = true;
                }
                IsDirty = true;
                _ClosedGenericFloat = value;
                _ClosedGenericFloat_Accessed = true;
            }
        }
        protected bool _ClosedGenericFloat_Accessed;
        private GenericFromBase<Base> _ClosedGenericFromBaseWithBase;
        public GenericFromBase<Base> ClosedGenericFromBaseWithBase
        {
            get
            {
                if (!_ClosedGenericFromBaseWithBase_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _ClosedGenericFromBaseWithBase = default(GenericFromBase<Base>);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _ClosedGenericFromBaseWithBase_ByteIndex, _ClosedGenericFromBaseWithBase_ByteLength, false, false, null);
                        
                        _ClosedGenericFromBaseWithBase = DeserializationFactory.Instance.CreateBaseOrDerivedType(267, () => new GenericFromBase<Base>(), childData, this); 
                    }
                    _ClosedGenericFromBaseWithBase_Accessed = true;
                } 
                return _ClosedGenericFromBaseWithBase;
            }
            set
            {
                if (value != null)
                {
                    value.LazinatorParentsReference = value.LazinatorParentsReference.WithAdded(this);
                    value.IsDirty = true;
                }
                IsDirty = true;
                _ClosedGenericFromBaseWithBase = value;
                _ClosedGenericFromBaseWithBase_Accessed = true;
            }
        }
        protected bool _ClosedGenericFromBaseWithBase_Accessed;
        private OpenGeneric<IExampleChild> _ClosedGenericInterface;
        public OpenGeneric<IExampleChild> ClosedGenericInterface
        {
            get
            {
                if (!_ClosedGenericInterface_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _ClosedGenericInterface = default(OpenGeneric<IExampleChild>);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _ClosedGenericInterface_ByteIndex, _ClosedGenericInterface_ByteLength, false, false, null);
                        
                        _ClosedGenericInterface = DeserializationFactory.Instance.CreateBaseOrDerivedType(233, () => new OpenGeneric<IExampleChild>(), childData, this); 
                    }
                    _ClosedGenericInterface_Accessed = true;
                } 
                return _ClosedGenericInterface;
            }
            set
            {
                if (value != null)
                {
                    value.LazinatorParentsReference = value.LazinatorParentsReference.WithAdded(this);
                    value.IsDirty = true;
                }
                IsDirty = true;
                _ClosedGenericInterface = value;
                _ClosedGenericInterface_Accessed = true;
            }
        }
        protected bool _ClosedGenericInterface_Accessed;
        private OpenGeneric<IExampleNonexclusiveInterface> _ClosedGenericNonexclusiveInterface;
        public OpenGeneric<IExampleNonexclusiveInterface> ClosedGenericNonexclusiveInterface
        {
            get
            {
                if (!_ClosedGenericNonexclusiveInterface_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _ClosedGenericNonexclusiveInterface = default(OpenGeneric<IExampleNonexclusiveInterface>);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _ClosedGenericNonexclusiveInterface_ByteIndex, _ClosedGenericNonexclusiveInterface_ByteLength, false, false, null);
                        
                        _ClosedGenericNonexclusiveInterface = DeserializationFactory.Instance.CreateBaseOrDerivedType(233, () => new OpenGeneric<IExampleNonexclusiveInterface>(), childData, this); 
                    }
                    _ClosedGenericNonexclusiveInterface_Accessed = true;
                } 
                return _ClosedGenericNonexclusiveInterface;
            }
            set
            {
                if (value != null)
                {
                    value.LazinatorParentsReference = value.LazinatorParentsReference.WithAdded(this);
                    value.IsDirty = true;
                }
                IsDirty = true;
                _ClosedGenericNonexclusiveInterface = value;
                _ClosedGenericNonexclusiveInterface_Accessed = true;
            }
        }
        protected bool _ClosedGenericNonexclusiveInterface_Accessed;
        
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
            if (enumerateNulls && (ClosedGenericBase == null))
            {
                yield return ("ClosedGenericBase", default);
            }
            else if ((!exploreOnlyDeserializedChildren && ClosedGenericBase != null) || (_ClosedGenericBase_Accessed && _ClosedGenericBase != null))
            {
                foreach (ILazinator toYield in ClosedGenericBase.EnumerateLazinatorNodes(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
                {
                    yield return ("ClosedGenericBase", toYield);
                }
            }
            if (enumerateNulls && (ClosedGenericFloat == null))
            {
                yield return ("ClosedGenericFloat", default);
            }
            else if ((!exploreOnlyDeserializedChildren && ClosedGenericFloat != null) || (_ClosedGenericFloat_Accessed && _ClosedGenericFloat != null))
            {
                foreach (ILazinator toYield in ClosedGenericFloat.EnumerateLazinatorNodes(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
                {
                    yield return ("ClosedGenericFloat", toYield);
                }
            }
            if (enumerateNulls && (ClosedGenericFromBaseWithBase == null))
            {
                yield return ("ClosedGenericFromBaseWithBase", default);
            }
            else if ((!exploreOnlyDeserializedChildren && ClosedGenericFromBaseWithBase != null) || (_ClosedGenericFromBaseWithBase_Accessed && _ClosedGenericFromBaseWithBase != null))
            {
                foreach (ILazinator toYield in ClosedGenericFromBaseWithBase.EnumerateLazinatorNodes(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
                {
                    yield return ("ClosedGenericFromBaseWithBase", toYield);
                }
            }
            if (enumerateNulls && (ClosedGenericInterface == null))
            {
                yield return ("ClosedGenericInterface", default);
            }
            else if ((!exploreOnlyDeserializedChildren && ClosedGenericInterface != null) || (_ClosedGenericInterface_Accessed && _ClosedGenericInterface != null))
            {
                foreach (ILazinator toYield in ClosedGenericInterface.EnumerateLazinatorNodes(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
                {
                    yield return ("ClosedGenericInterface", toYield);
                }
            }
            if (enumerateNulls && (ClosedGenericNonexclusiveInterface == null))
            {
                yield return ("ClosedGenericNonexclusiveInterface", default);
            }
            else if ((!exploreOnlyDeserializedChildren && ClosedGenericNonexclusiveInterface != null) || (_ClosedGenericNonexclusiveInterface_Accessed && _ClosedGenericNonexclusiveInterface != null))
            {
                foreach (ILazinator toYield in ClosedGenericNonexclusiveInterface.EnumerateLazinatorNodes(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
                {
                    yield return ("ClosedGenericNonexclusiveInterface", toYield);
                }
            }
            yield break;
        }
        
        
        public virtual IEnumerable<(string propertyName, object descendant)> EnumerateNonLazinatorProperties()
        {
            yield break;
        }
        
        protected virtual void ResetAccessedProperties()
        {
            _ClosedGenericBase_Accessed = _ClosedGenericFloat_Accessed = _ClosedGenericFromBaseWithBase_Accessed = _ClosedGenericInterface_Accessed = _ClosedGenericNonexclusiveInterface_Accessed = false;
        }
        
        /* Conversion */
        
        public virtual int LazinatorUniqueID => 234;
        
        protected virtual bool ContainsOpenGenericParameters => false;
        protected virtual LazinatorGenericIDType _LazinatorGenericID { get; set; }
        public virtual LazinatorGenericIDType LazinatorGenericID
        {
            get => default;
            set { }
        }
        
        public virtual int LazinatorObjectVersion { get; set; } = 0;
        
        
        public virtual void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
            _ClosedGenericBase_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            _ClosedGenericFloat_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            _ClosedGenericFromBaseWithBase_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            _ClosedGenericInterface_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            _ClosedGenericNonexclusiveInterface_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            _OpenGenericStayingOpenContainer_EndByteIndex = bytesSoFar;
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
                _DescendantIsDirty = includeChildrenMode != IncludeChildrenMode.IncludeAllChildren && ((_ClosedGenericBase_Accessed && _ClosedGenericBase != null && (ClosedGenericBase.IsDirty || ClosedGenericBase.DescendantIsDirty)) || (_ClosedGenericFloat_Accessed && _ClosedGenericFloat != null && (ClosedGenericFloat.IsDirty || ClosedGenericFloat.DescendantIsDirty)) || (_ClosedGenericFromBaseWithBase_Accessed && _ClosedGenericFromBaseWithBase != null && (ClosedGenericFromBaseWithBase.IsDirty || ClosedGenericFromBaseWithBase.DescendantIsDirty)) || (_ClosedGenericInterface_Accessed && _ClosedGenericInterface != null && (ClosedGenericInterface.IsDirty || ClosedGenericInterface.DescendantIsDirty)) || (_ClosedGenericNonexclusiveInterface_Accessed && _ClosedGenericNonexclusiveInterface != null && (ClosedGenericNonexclusiveInterface.IsDirty || ClosedGenericNonexclusiveInterface.DescendantIsDirty)));
                
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
                WriteChild(writer, _ClosedGenericBase, includeChildrenMode, _ClosedGenericBase_Accessed, () => GetChildSlice(LazinatorObjectBytes, _ClosedGenericBase_ByteIndex, _ClosedGenericBase_ByteLength, false, false, null), verifyCleanness, updateStoredBuffer, false, false, this);
            }
            if (updateStoredBuffer)
            {
                _ClosedGenericBase_ByteIndex = startOfObjectPosition - startPosition;
            }
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                WriteChild(writer, _ClosedGenericFloat, includeChildrenMode, _ClosedGenericFloat_Accessed, () => GetChildSlice(LazinatorObjectBytes, _ClosedGenericFloat_ByteIndex, _ClosedGenericFloat_ByteLength, false, false, null), verifyCleanness, updateStoredBuffer, false, false, this);
            }
            if (updateStoredBuffer)
            {
                _ClosedGenericFloat_ByteIndex = startOfObjectPosition - startPosition;
            }
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                WriteChild(writer, _ClosedGenericFromBaseWithBase, includeChildrenMode, _ClosedGenericFromBaseWithBase_Accessed, () => GetChildSlice(LazinatorObjectBytes, _ClosedGenericFromBaseWithBase_ByteIndex, _ClosedGenericFromBaseWithBase_ByteLength, false, false, null), verifyCleanness, updateStoredBuffer, false, false, this);
            }
            if (updateStoredBuffer)
            {
                _ClosedGenericFromBaseWithBase_ByteIndex = startOfObjectPosition - startPosition;
            }
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                WriteChild(writer, _ClosedGenericInterface, includeChildrenMode, _ClosedGenericInterface_Accessed, () => GetChildSlice(LazinatorObjectBytes, _ClosedGenericInterface_ByteIndex, _ClosedGenericInterface_ByteLength, false, false, null), verifyCleanness, updateStoredBuffer, false, false, this);
            }
            if (updateStoredBuffer)
            {
                _ClosedGenericInterface_ByteIndex = startOfObjectPosition - startPosition;
            }
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                WriteChild(writer, _ClosedGenericNonexclusiveInterface, includeChildrenMode, _ClosedGenericNonexclusiveInterface_Accessed, () => GetChildSlice(LazinatorObjectBytes, _ClosedGenericNonexclusiveInterface_ByteIndex, _ClosedGenericNonexclusiveInterface_ByteLength, false, false, null), verifyCleanness, updateStoredBuffer, false, false, this);
            }
            if (updateStoredBuffer)
            {
                _ClosedGenericNonexclusiveInterface_ByteIndex = startOfObjectPosition - startPosition;
            }
            if (updateStoredBuffer)
            {
                _OpenGenericStayingOpenContainer_EndByteIndex = writer.Position - startPosition;
            }
        }
        
    }
}
