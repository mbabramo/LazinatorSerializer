//2cec189d-1c21-d8d6-664e-354b9e63014e
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.72
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
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.InteropServices;
    using static Lazinator.Core.LazinatorUtilities;
    
    [Autogenerated]
    public partial class ClassWithSubclass : ILazinator
    {
        /* Serialization, deserialization, and object relationships */
        
        public ClassWithSubclass() : base()
        {
        }
        
        public virtual ILazinator LazinatorParentClass { get; set; }
        
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
            
            int uniqueID = span.ToDecompressedInt(ref bytesSoFar);
            if (uniqueID != LazinatorUniqueID)
            {
                throw new FormatException("Wrong self-serialized type initialized.");
            }
            
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
                DeserializationFactory = DeserializationFactory,
                LazinatorParentClass = LazinatorParentClass,
                InformParentOfDirtinessDelegate = InformParentOfDirtinessDelegate,
                OriginalIncludeChildrenMode = includeChildrenMode,
                HierarchyBytes = bytes
            };
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
        
        public virtual void MarkHierarchyClean()
        {
            _IsDirty = false;
            _DescendantIsDirty = false;
            if (_SubclassInstance1_Accessed)
            {
                SubclassInstance1.MarkHierarchyClean();
            }
            if (_SubclassInstance2_Accessed)
            {
                SubclassInstance2.MarkHierarchyClean();
            }
        }
        
        public virtual DeserializationFactory DeserializationFactory { get; set; }
        
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
        
        /* Field definitions */
        
        protected int _SubclassInstance1_ByteIndex;
        protected int _SubclassInstance2_ByteIndex;
        protected virtual int _SubclassInstance1_ByteLength => _SubclassInstance2_ByteIndex - _SubclassInstance1_ByteIndex;
        private int _ClassWithSubclass_EndByteIndex;
        protected virtual int _SubclassInstance2_ByteLength => _ClassWithSubclass_EndByteIndex - _SubclassInstance2_ByteIndex;
        
        private int _IntWithinSuperclass;
        public int IntWithinSuperclass
        {
            [DebuggerStepThrough]
            get
            {
                return _IntWithinSuperclass;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _IntWithinSuperclass = value;
            }
        }
        private global::LazinatorTests.Examples.Subclasses.ClassWithSubclass.SubclassWithinClass _SubclassInstance1;
        public global::LazinatorTests.Examples.Subclasses.ClassWithSubclass.SubclassWithinClass SubclassInstance1
        {
            [DebuggerStepThrough]
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
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _SubclassInstance1_ByteIndex, _SubclassInstance1_ByteLength);
                        
                        if (DeserializationFactory == null)
                        {
                            DeserializationFactory = DeserializationFactory.GetInstance();
                        }
                        _SubclassInstance1 = DeserializationFactory.FactoryCreateBaseOrDerivedType(258, () => new global::LazinatorTests.Examples.Subclasses.ClassWithSubclass.SubclassWithinClass(), childData, this); 
                    }
                    _SubclassInstance1_Accessed = true;
                }
                return _SubclassInstance1;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _SubclassInstance1 = value;
                if (_SubclassInstance1 != null)
                {
                    _SubclassInstance1.IsDirty = true;
                }
                _SubclassInstance1_Accessed = true;
            }
        }
        protected bool _SubclassInstance1_Accessed;
        private global::LazinatorTests.Examples.Subclasses.ClassWithSubclass.SubclassWithinClass _SubclassInstance2;
        public global::LazinatorTests.Examples.Subclasses.ClassWithSubclass.SubclassWithinClass SubclassInstance2
        {
            [DebuggerStepThrough]
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
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _SubclassInstance2_ByteIndex, _SubclassInstance2_ByteLength);
                        
                        if (DeserializationFactory == null)
                        {
                            DeserializationFactory = DeserializationFactory.GetInstance();
                        }
                        _SubclassInstance2 = DeserializationFactory.FactoryCreateBaseOrDerivedType(258, () => new global::LazinatorTests.Examples.Subclasses.ClassWithSubclass.SubclassWithinClass(), childData, this); 
                    }
                    _SubclassInstance2_Accessed = true;
                }
                return _SubclassInstance2;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _SubclassInstance2 = value;
                if (_SubclassInstance2 != null)
                {
                    _SubclassInstance2.IsDirty = true;
                }
                _SubclassInstance2_Accessed = true;
            }
        }
        protected bool _SubclassInstance2_Accessed;
        
        protected virtual void ResetAccessedProperties()
        {
            _SubclassInstance1_Accessed = _SubclassInstance2_Accessed = false;
        }
        
        /* Conversion */
        
        public virtual int LazinatorUniqueID => 257;
        
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
        
        public virtual void SerializeExistingBuffer(BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
            int startPosition = writer.Position;
            WritePropertiesIntoBuffer(writer, includeChildrenMode, verifyCleanness, true);
            
            _IsDirty = false;
            _DescendantIsDirty = includeChildrenMode != IncludeChildrenMode.IncludeAllChildren && ((_SubclassInstance1_Accessed && SubclassInstance1 != null && (SubclassInstance1.IsDirty || SubclassInstance1.DescendantIsDirty)) || (_SubclassInstance2_Accessed && SubclassInstance2 != null && (SubclassInstance2.IsDirty || SubclassInstance2.DescendantIsDirty)));
            
            _LazinatorObjectBytes = writer.Slice(startPosition);
        }
        protected virtual void WritePropertiesIntoBuffer(BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool includeUniqueID)
        {
            // header information
            if (includeUniqueID)
            {
                CompressedIntegralTypes.WriteCompressedInt(writer, LazinatorUniqueID);
            }
            CompressedIntegralTypes.WriteCompressedInt(writer, Lazinator.Support.LazinatorVersionInfo.LazinatorIntVersion);
            CompressedIntegralTypes.WriteCompressedInt(writer, LazinatorObjectVersion);
            writer.Write((byte)includeChildrenMode);
            // write properties
            CompressedIntegralTypes.WriteCompressedInt(writer, _IntWithinSuperclass);
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                WriteChildWithLength(writer, _SubclassInstance1, includeChildrenMode, _SubclassInstance1_Accessed, () => GetChildSlice(LazinatorObjectBytes, _SubclassInstance1_ByteIndex, _SubclassInstance1_ByteLength), verifyCleanness, false, false, this);
            }
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                WriteChildWithLength(writer, _SubclassInstance2, includeChildrenMode, _SubclassInstance2_Accessed, () => GetChildSlice(LazinatorObjectBytes, _SubclassInstance2_ByteIndex, _SubclassInstance2_ByteLength), verifyCleanness, false, false, this);
            }
        }
        
    }
}
