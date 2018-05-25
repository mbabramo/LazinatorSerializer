//901ebe95-9653-878d-ff3c-d7e42107debc
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.36
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
    using LazinatorTests.Examples;
    using System;
    using System.Buffers;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.InteropServices;
    using static Lazinator.Core.LazinatorUtilities;
    
    [Autogenerated]
    public partial class ConcreteGeneric2b : ILazinator
    {
        /* Serialization, deserialization, and object relationships */
        
        public ConcreteGeneric2b() : base()
        {
        }
        
        public override ILazinator LazinatorParentClass { get; set; }
        
        protected IncludeChildrenMode OriginalIncludeChildrenMode;
        
        public override void Deserialize()
        {
            int bytesSoFar = 0;
            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
            if (span.Length == 0)
            {
                return;
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
        }
        
        public override MemoryInBuffer SerializeNewBuffer(IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
            return EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, true, verifyCleanness, IsDirty, DescendantIsDirty, false, LazinatorObjectBytes, (StreamManuallyDelegate) EncodeToNewBuffer);
        }
        
        protected override MemoryInBuffer EncodeToNewBuffer(IncludeChildrenMode includeChildrenMode, bool verifyCleanness) => LazinatorUtilities.EncodeToNewBinaryBufferWriter(this, includeChildrenMode, verifyCleanness);
        
        public override ILazinator CloneLazinator()
        {
            return CloneLazinator(OriginalIncludeChildrenMode);
        }
        
        public override ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode)
        {
            MemoryInBuffer bytes = EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, false, false, IsDirty, DescendantIsDirty, false, LazinatorObjectBytes, (StreamManuallyDelegate)EncodeToNewBuffer);
            var clone = new ConcreteGeneric2b()
            {
                DeserializationFactory = DeserializationFactory,
                LazinatorParentClass = LazinatorParentClass,
                InformParentOfDirtinessDelegate = InformParentOfDirtinessDelegate,
                OriginalIncludeChildrenMode = includeChildrenMode,
                HierarchyBytes = bytes
            };
            return clone;
        }
        
        private bool _IsDirty;
        public override bool IsDirty
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
        
        public override InformParentOfDirtinessDelegate InformParentOfDirtinessDelegate { get; set; }
        public override void InformParentOfDirtiness()
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
        
        private bool _DescendantIsDirty;
        public override bool DescendantIsDirty
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
        
        public override void MarkHierarchyClean()
        {
            _IsDirty = false;
            _DescendantIsDirty = false;
            if (_MyT_Accessed)
            {
                MyT.MarkHierarchyClean();
            }
            if (_LazinatorExample_Accessed)
            {
                LazinatorExample.MarkHierarchyClean();
            }
        }
        
        public override DeserializationFactory DeserializationFactory { get; set; }
        
        private MemoryInBuffer _HierarchyBytes;
        public override MemoryInBuffer HierarchyBytes
        {
            get => _HierarchyBytes;
            set
            {
                _HierarchyBytes = value;
                LazinatorObjectBytes = value.FilledMemory;
            }
        }
        
        private ReadOnlyMemory<byte> _LazinatorObjectBytes;
        public override ReadOnlyMemory<byte> LazinatorObjectBytes
        {
            get => _LazinatorObjectBytes;
            set
            {
                _LazinatorObjectBytes = value;
                Deserialize();
            }
        }
        
        public override void LazinatorConvertToBytes()
        {
            if (!IsDirty && !DescendantIsDirty)
            {
                return;
            }
            MemoryInBuffer bytes = EncodeOrRecycleToNewBuffer(IncludeChildrenMode.IncludeAllChildren, OriginalIncludeChildrenMode, false, false, IsDirty, DescendantIsDirty, false, LazinatorObjectBytes, (StreamManuallyDelegate)EncodeToNewBuffer);
            _IsDirty = false;
            LazinatorObjectBytes = bytes.FilledMemory;
            _MyT_Accessed = _LazinatorExample_Accessed = false;
        }
        
        public override uint GetBinaryHashCode32()
        {
            LazinatorConvertToBytes();
            return FarmhashByteSpans.Hash32(LazinatorObjectBytes.Span);
        }
        
        public override ulong GetBinaryHashCode64()
        {
            LazinatorConvertToBytes();
            return FarmhashByteSpans.Hash64(LazinatorObjectBytes.Span);
        }
        
        public override Guid GetBinaryHashCode128()
        {
            LazinatorConvertToBytes();
            return FarmhashByteSpans.Hash128(LazinatorObjectBytes.Span);
        }
        
        /* Field definitions */
        
        protected int _LazinatorExample_ByteIndex;
        protected override int _MyT_ByteLength => _LazinatorExample_ByteIndex - _MyT_ByteIndex;
        private int _ConcreteGeneric2b_EndByteIndex;
        protected virtual int _LazinatorExample_ByteLength => _ConcreteGeneric2b_EndByteIndex - _LazinatorExample_ByteIndex;
        
        private global::LazinatorTests.Examples.Abstract.AbstractGeneric1<global::LazinatorTests.Examples.Example>.EnumWithinAbstractGeneric _MyEnumWithinAbstractGeneric;
        public override global::LazinatorTests.Examples.Abstract.AbstractGeneric1<global::LazinatorTests.Examples.Example>.EnumWithinAbstractGeneric MyEnumWithinAbstractGeneric
        {
            [DebuggerStepThrough]
            get
            {
                return _MyEnumWithinAbstractGeneric;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _MyEnumWithinAbstractGeneric = value;
            }
        }
        private global::LazinatorTests.Examples.Abstract.AbstractGeneric1<int>.EnumWithinAbstractGeneric _MyEnumWithinAbstractGeneric2;
        public override global::LazinatorTests.Examples.Abstract.AbstractGeneric1<int>.EnumWithinAbstractGeneric MyEnumWithinAbstractGeneric2
        {
            [DebuggerStepThrough]
            get
            {
                return _MyEnumWithinAbstractGeneric2;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _MyEnumWithinAbstractGeneric2 = value;
            }
        }
        private string _AnotherProperty;
        public string AnotherProperty
        {
            [DebuggerStepThrough]
            get
            {
                return _AnotherProperty;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _AnotherProperty = value;
            }
        }
        private Example _MyT;
        public override Example MyT
        {
            [DebuggerStepThrough]
            get
            {
                if (!_MyT_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _MyT = default(Example);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _MyT_ByteIndex, _MyT_ByteLength);
                        
                        if (DeserializationFactory == null)
                        {
                            LazinatorDeserializationException.ThrowNoDeserializationFactory();
                        }
                        _MyT = DeserializationFactory.Create(212, () => new Example(), childData, this); 
                    }
                    _MyT_Accessed = true;
                }
                return _MyT;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _MyT = value;
                if (_MyT != null)
                {
                    _MyT.IsDirty = true;
                }
                _MyT_Accessed = true;
            }
        }
        private Example _LazinatorExample;
        public Example LazinatorExample
        {
            [DebuggerStepThrough]
            get
            {
                if (!_LazinatorExample_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _LazinatorExample = default(Example);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _LazinatorExample_ByteIndex, _LazinatorExample_ByteLength);
                        
                        if (DeserializationFactory == null)
                        {
                            LazinatorDeserializationException.ThrowNoDeserializationFactory();
                        }
                        _LazinatorExample = DeserializationFactory.Create(212, () => new Example(), childData, this); 
                    }
                    _LazinatorExample_Accessed = true;
                }
                return _LazinatorExample;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _LazinatorExample = value;
                if (_LazinatorExample != null)
                {
                    _LazinatorExample.IsDirty = true;
                }
                _LazinatorExample_Accessed = true;
            }
        }
        protected bool _LazinatorExample_Accessed;
        
        /* Conversion */
        
        public override int LazinatorUniqueID => 242;
        
        public override int LazinatorObjectVersion { get; set; } = 0;
        
        public override void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
            _MyEnumWithinAbstractGeneric = (global::LazinatorTests.Examples.Abstract.AbstractGeneric1<global::LazinatorTests.Examples.Example>.EnumWithinAbstractGeneric)span.ToDecompressedInt(ref bytesSoFar);
            _MyEnumWithinAbstractGeneric2 = (global::LazinatorTests.Examples.Abstract.AbstractGeneric1<int>.EnumWithinAbstractGeneric)span.ToDecompressedInt(ref bytesSoFar);
            _AnotherProperty = span.ToString_BrotliCompressedWithLength(ref bytesSoFar);
            _MyT_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            _LazinatorExample_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            _ConcreteGeneric2b_EndByteIndex = bytesSoFar;
        }
        
        public override void SerializeExistingBuffer(BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
            // header information
            CompressedIntegralTypes.WriteCompressedInt(writer, LazinatorUniqueID);
            CompressedIntegralTypes.WriteCompressedInt(writer, Lazinator.Support.LazinatorVersionInfo.LazinatorIntVersion);
            CompressedIntegralTypes.WriteCompressedInt(writer, LazinatorObjectVersion);
            writer.Write((byte)includeChildrenMode);
            // write properties
            CompressedIntegralTypes.WriteCompressedInt(writer, (int) _MyEnumWithinAbstractGeneric);
            CompressedIntegralTypes.WriteCompressedInt(writer, (int) _MyEnumWithinAbstractGeneric2);
            EncodeCharAndString.WriteBrotliCompressedWithIntPrefix(writer, _AnotherProperty);
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                WriteChildWithLength(writer, _MyT, includeChildrenMode, _MyT_Accessed, () => GetChildSlice(LazinatorObjectBytes, _MyT_ByteIndex, _MyT_ByteLength), verifyCleanness, false);
            }
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                WriteChildWithLength(writer, _LazinatorExample, includeChildrenMode, _LazinatorExample_Accessed, () => GetChildSlice(LazinatorObjectBytes, _LazinatorExample_ByteIndex, _LazinatorExample_ByteLength), verifyCleanness, false);
            }
        }
        
    }
}
