//f4ff970c-35b4-3c6a-20cb-ae9abcc05092
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.123
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LazinatorTests.Examples.Subclasses
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
    public partial class ClassWithSubclass : ILazinator
    {
        /* Serialization, deserialization, and object relationships */
        
        protected ILazinator _LazinatorParentClass;
        public ClassWithSubclass() : base()
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
            return EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, true, verifyCleanness, IsDirty, DescendantIsDirty, false, LazinatorObjectBytes, (StreamManuallyDelegate) EncodeToNewBuffer);
        }
        
        protected virtual MemoryInBuffer EncodeToNewBuffer(IncludeChildrenMode includeChildrenMode, bool verifyCleanness) => LazinatorUtilities.EncodeToNewBinaryBufferWriter(this, includeChildrenMode, verifyCleanness);
        
        public virtual ILazinator CloneLazinator()
        {
            return CloneLazinator(OriginalIncludeChildrenMode);
        }
        
        public virtual ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode)
        {
            MemoryInBuffer bytes = EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, false, false, IsDirty, DescendantIsDirty, false, LazinatorObjectBytes, (StreamManuallyDelegate)EncodeToNewBuffer);
            var clone = new ClassWithSubclass()
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
            MemoryInBuffer bytes = EncodeOrRecycleToNewBuffer(IncludeChildrenMode.IncludeAllChildren, OriginalIncludeChildrenMode, false, false, IsDirty, DescendantIsDirty, false, LazinatorObjectBytes, (StreamManuallyDelegate)EncodeToNewBuffer);
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
        
        protected int _SubclassInstance1_ByteIndex;
        protected int _SubclassInstance2_ByteIndex;
        protected virtual int _SubclassInstance1_ByteLength => _SubclassInstance2_ByteIndex - _SubclassInstance1_ByteIndex;
        private int _ClassWithSubclass_EndByteIndex;
        protected virtual int _SubclassInstance2_ByteLength => _ClassWithSubclass_EndByteIndex - _SubclassInstance2_ByteIndex;
        
        private int _IntWithinSuperclass;
        public int IntWithinSuperclass
        {
            get
            {
                return _IntWithinSuperclass;
            }
            set
            {
                IsDirty = true;
                _IntWithinSuperclass = value;
            }
        }
        private global::LazinatorTests.Examples.Subclasses.ClassWithSubclass.SubclassWithinClass _SubclassInstance1;
        public global::LazinatorTests.Examples.Subclasses.ClassWithSubclass.SubclassWithinClass SubclassInstance1
        {
            get
            {
                if (!_SubclassInstance1_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _SubclassInstance1 = default(global::LazinatorTests.Examples.Subclasses.ClassWithSubclass.SubclassWithinClass);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _SubclassInstance1_ByteIndex, _SubclassInstance1_ByteLength, false, false, null);
                        
                        _SubclassInstance1 = DeserializationFactory.Instance.CreateBaseOrDerivedType(258, () => new global::LazinatorTests.Examples.Subclasses.ClassWithSubclass.SubclassWithinClass(), childData, this); 
                    }
                    _SubclassInstance1_Accessed = true;
                }
                return _SubclassInstance1;
            }
            set
            {
                if (value != null)
                {
                    if (value.LazinatorParentClass != null)
                    {
                        throw new MovedLazinatorException($"The property SubclassInstance1 cannot be set to a Lazinator object with a defined LazinatorParentClass, because AutoChangeParent is set to false in the configuration file and no attribute providing an exception is present.");
                    }
                    value.LazinatorParentClass = this;
                    value.IsDirty = true;
                }
                IsDirty = true;
                _SubclassInstance1 = value;
                _SubclassInstance1_Accessed = true;
            }
        }
        protected bool _SubclassInstance1_Accessed;
        private global::LazinatorTests.Examples.Subclasses.ClassWithSubclass.SubclassWithinClass _SubclassInstance2;
        public global::LazinatorTests.Examples.Subclasses.ClassWithSubclass.SubclassWithinClass SubclassInstance2
        {
            get
            {
                if (!_SubclassInstance2_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _SubclassInstance2 = default(global::LazinatorTests.Examples.Subclasses.ClassWithSubclass.SubclassWithinClass);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _SubclassInstance2_ByteIndex, _SubclassInstance2_ByteLength, false, false, null);
                        
                        _SubclassInstance2 = DeserializationFactory.Instance.CreateBaseOrDerivedType(258, () => new global::LazinatorTests.Examples.Subclasses.ClassWithSubclass.SubclassWithinClass(), childData, this); 
                    }
                    _SubclassInstance2_Accessed = true;
                }
                return _SubclassInstance2;
            }
            set
            {
                if (value != null)
                {
                    if (value.LazinatorParentClass != null)
                    {
                        throw new MovedLazinatorException($"The property SubclassInstance2 cannot be set to a Lazinator object with a defined LazinatorParentClass, because AutoChangeParent is set to false in the configuration file and no attribute providing an exception is present.");
                    }
                    value.LazinatorParentClass = this;
                    value.IsDirty = true;
                }
                IsDirty = true;
                _SubclassInstance2 = value;
                _SubclassInstance2_Accessed = true;
            }
        }
        protected bool _SubclassInstance2_Accessed;
        
        public IEnumerable<ILazinator> GetDirtyNodes() => GetDirtyNodes(null, null, false);
        
        public IEnumerable<ILazinator> GetDirtyNodes(Func<ILazinator, bool> exploreCriterion, Func<ILazinator, bool> yieldCriterion, bool onlyHighestDirty)
        {
            if (IsDirty)
            {
                bool yield = (yieldCriterion == null) ? true : yieldCriterion(this);
                if (yield)
                {
                    yield return this;
                    if (onlyHighestDirty)
                    {
                        yield break;
                    }
                }
            }
            bool explore = (exploreCriterion == null) ? true : exploreCriterion(this);
            if (explore && DescendantIsDirty)
            {
                foreach (ILazinator dirty in GetDirtyNodes_Helper(exploreCriterion, yieldCriterion, onlyHighestDirty))
                {
                    yield return dirty;
                }
            }
        }
        
        protected virtual IEnumerable<ILazinator> GetDirtyNodes_Helper(Func<ILazinator, bool> exploreCriterion, Func<ILazinator, bool> yieldCriterion, bool onlyHighestDirty)
        {
            if (_SubclassInstance1_Accessed && SubclassInstance1 != null && (_SubclassInstance1.IsDirty || _SubclassInstance1.DescendantIsDirty))
            {
                foreach (ILazinator toYield in _SubclassInstance1.GetDirtyNodes(exploreCriterion, yieldCriterion, onlyHighestDirty))
                {
                    yield return toYield;
                }
            }
            if (_SubclassInstance2_Accessed && SubclassInstance2 != null && (_SubclassInstance2.IsDirty || _SubclassInstance2.DescendantIsDirty))
            {
                foreach (ILazinator toYield in _SubclassInstance2.GetDirtyNodes(exploreCriterion, yieldCriterion, onlyHighestDirty))
                {
                    yield return toYield;
                }
            }
            yield break;
        }
        
        protected virtual void ResetAccessedProperties()
        {
            _SubclassInstance1_Accessed = _SubclassInstance2_Accessed = false;
        }
        
        /* Conversion */
        
        public virtual int LazinatorUniqueID => 257;
        
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
            _IntWithinSuperclass = span.ToDecompressedInt(ref bytesSoFar);
            _SubclassInstance1_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            _SubclassInstance2_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            _ClassWithSubclass_EndByteIndex = bytesSoFar;
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
                _DescendantIsDirty = includeChildrenMode != IncludeChildrenMode.IncludeAllChildren && ((_SubclassInstance1_Accessed && SubclassInstance1 != null && (SubclassInstance1.IsDirty || SubclassInstance1.DescendantIsDirty)) || (_SubclassInstance2_Accessed && SubclassInstance2 != null && (SubclassInstance2.IsDirty || SubclassInstance2.DescendantIsDirty)));
                
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
            CompressedIntegralTypes.WriteCompressedInt(writer, _IntWithinSuperclass);
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                WriteChild(writer, _SubclassInstance1, includeChildrenMode, _SubclassInstance1_Accessed, () => GetChildSlice(LazinatorObjectBytes, _SubclassInstance1_ByteIndex, _SubclassInstance1_ByteLength, false, false, null), verifyCleanness, updateStoredBuffer, false, false, this);
            }
            if (updateStoredBuffer)
            {
                _SubclassInstance1_ByteIndex = startOfObjectPosition - startPosition;
            }
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                WriteChild(writer, _SubclassInstance2, includeChildrenMode, _SubclassInstance2_Accessed, () => GetChildSlice(LazinatorObjectBytes, _SubclassInstance2_ByteIndex, _SubclassInstance2_ByteLength, false, false, null), verifyCleanness, updateStoredBuffer, false, false, this);
            }
            if (updateStoredBuffer)
            {
                _SubclassInstance2_ByteIndex = startOfObjectPosition - startPosition;
            }
            if (updateStoredBuffer)
            {
                _ClassWithSubclass_EndByteIndex = writer.Position - startPosition;
            }
        }
        
    }
}
