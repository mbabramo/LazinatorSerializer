//8cccd096-0975-ad50-b035-3f1601580fa0
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using Lazinator.Buffers; 
using Lazinator.Collections;
using Lazinator.Core; 
using static Lazinator.Core.LazinatorUtilities;
using Lazinator.Exceptions;
using Lazinator.Support;
using Lazinator.Wrappers;

namespace LazinatorTests.Examples
{
    public partial class Example : ILazinator
    {
        /* Boilerplate for every base class implementing ILazinator */
        
        public ILazinator LazinatorParentClass { get; set; }
        
        protected internal IncludeChildrenMode OriginalIncludeChildrenMode;
        
        public void Deserialize()
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
            if (serializedVersionNumber < LazinatorObjectVersion)
            {
                LazinatorObjectVersionUpgrade(serializedVersionNumber);
            }
        }
        
        public MemoryInBuffer SerializeNewBuffer(IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
            return EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, true, verifyCleanness, IsDirty, DescendantIsDirty, false, LazinatorObjectBytes, (StreamManuallyDelegate) EncodeToNewBuffer);
        }
        
        internal MemoryInBuffer EncodeToNewBuffer(IncludeChildrenMode includeChildrenMode, bool verifyCleanness) => LazinatorUtilities.EncodeToNewBuffer(this, includeChildrenMode, verifyCleanness);
        
        public ILazinator Clone()
        {
            return Clone(OriginalIncludeChildrenMode);
        }
        
        public ILazinator Clone(IncludeChildrenMode includeChildrenMode)
        {
            MemoryInBuffer bytes = EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, false, false, IsDirty, DescendantIsDirty, false, LazinatorObjectBytes, (StreamManuallyDelegate)EncodeToNewBuffer);
            var clone = new Example()
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
        public bool IsDirty
        {
            //[DebuggerStepThrough]
            get => _IsDirty;
            //[DebuggerStepThrough]
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
        
        public InformParentOfDirtinessDelegate InformParentOfDirtinessDelegate { get; set; }
        public void InformParentOfDirtiness()
        {
            if (InformParentOfDirtinessDelegate == null)
            {
                if (LazinatorParentClass != null)
                {
                    LazinatorParentClass.DescendantIsDirty = true;
                }
            }
            else
            InformParentOfDirtinessDelegate();
        }
        
        private bool _DescendantIsDirty;
        public bool DescendantIsDirty
        {
            //[DebuggerStepThrough]
            get => _DescendantIsDirty;
            //[DebuggerStepThrough]
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
        
        public DeserializationFactory DeserializationFactory { get; set; }
        
        private MemoryInBuffer _HierarchyBytes;
        public MemoryInBuffer HierarchyBytes
        {
            get => _HierarchyBytes;
            set
            {
                _HierarchyBytes = value;
                LazinatorObjectBytes = value.FilledMemory;
            }
        }
        
        private ReadOnlyMemory<byte> _LazinatorObjectBytes;
        public ReadOnlyMemory<byte> LazinatorObjectBytes
        {
            get => _LazinatorObjectBytes;
            set
            {
                _LazinatorObjectBytes = value;
                Deserialize();
            }
        }
        
        /* Field boilerplate */
        
        internal int _MyChild1_ByteIndex;
        internal int _MyChild2_ByteIndex;
        internal int _MyChild2Previous_ByteIndex;
        internal int _MyInterfaceImplementer_ByteIndex;
        internal int _MyNonLazinatorChild_ByteIndex;
        internal int _MyChild1_ByteLength => _MyChild2_ByteIndex - _MyChild1_ByteIndex;
        internal int _MyChild2_ByteLength => _MyChild2Previous_ByteIndex - _MyChild2_ByteIndex;
        internal int _MyChild2Previous_ByteLength => _MyInterfaceImplementer_ByteIndex - _MyChild2Previous_ByteIndex;
        internal int _MyInterfaceImplementer_ByteLength => _MyNonLazinatorChild_ByteIndex - _MyInterfaceImplementer_ByteIndex;
        internal int _MyNonLazinatorChild_ByteLength => LazinatorObjectBytes.Length - _MyNonLazinatorChild_ByteIndex;
        
        private bool _MyBool;
        public bool MyBool
        {
            //[DebuggerStepThrough]
            get
            {
                return _MyBool;
            }
            //[DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _MyBool = value;
            }
        }
        private char _MyChar;
        public char MyChar
        {
            //[DebuggerStepThrough]
            get
            {
                return _MyChar;
            }
            //[DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _MyChar = value;
            }
        }
        private DateTime _MyDateTime;
        public DateTime MyDateTime
        {
            //[DebuggerStepThrough]
            get
            {
                return _MyDateTime;
            }
            //[DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _MyDateTime = value;
            }
        }
        private string _MyNewString;
        public string MyNewString
        {
            //[DebuggerStepThrough]
            get
            {
                return _MyNewString;
            }
            //[DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _MyNewString = value;
            }
        }
        private string _MyOldString;
        public string MyOldString
        {
            //[DebuggerStepThrough]
            get
            {
                return _MyOldString;
            }
            //[DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _MyOldString = value;
            }
        }
        private string _MyString;
        public string MyString
        {
            //[DebuggerStepThrough]
            get
            {
                return _MyString;
            }
            //[DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _MyString = value;
            }
        }
        private TestEnum _MyTestEnum;
        public TestEnum MyTestEnum
        {
            //[DebuggerStepThrough]
            get
            {
                return _MyTestEnum;
            }
            //[DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _MyTestEnum = value;
            }
        }
        private uint _MyUint;
        public uint MyUint
        {
            //[DebuggerStepThrough]
            get
            {
                return _MyUint;
            }
            //[DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _MyUint = value;
            }
        }
        private decimal? _MyNullableDecimal;
        public decimal? MyNullableDecimal
        {
            //[DebuggerStepThrough]
            get
            {
                return _MyNullableDecimal;
            }
            //[DebuggerStepThrough]
            internal set
            {
                IsDirty = true;
                _MyNullableDecimal = value;
            }
        }
        private double? _MyNullableDouble;
        public double? MyNullableDouble
        {
            //[DebuggerStepThrough]
            get
            {
                return _MyNullableDouble;
            }
            //[DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _MyNullableDouble = value;
            }
        }
        private TimeSpan? _MyNullableTimeSpan;
        public TimeSpan? MyNullableTimeSpan
        {
            //[DebuggerStepThrough]
            get
            {
                return _MyNullableTimeSpan;
            }
            //[DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _MyNullableTimeSpan = value;
            }
        }
        private TestEnumByte? _MyTestEnumByteNullable;
        public TestEnumByte? MyTestEnumByteNullable
        {
            //[DebuggerStepThrough]
            get
            {
                return _MyTestEnumByteNullable;
            }
            //[DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _MyTestEnumByteNullable = value;
            }
        }
        private ExampleChild _MyChild1;
        public ExampleChild MyChild1
        {
            //[DebuggerStepThrough]
            get
            {
                if (!_MyChild1_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _MyChild1 = default(ExampleChild);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _MyChild1_ByteIndex, _MyChild1_ByteLength);
                        _MyChild1 = DeserializationFactory.Create(113, () => new ExampleChild(), childData, this); 
                    }
                    _MyChild1_Accessed = true;
                }
                return _MyChild1;
            }
            //[DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _MyChild1 = value;
                if (_MyChild1 != null)
                {
                    _MyChild1.IsDirty = true;
                }
                _MyChild1_Accessed = true;
            }
        }
        internal bool _MyChild1_Accessed;
        private ExampleChild _MyChild2;
        public ExampleChild MyChild2
        {
            //[DebuggerStepThrough]
            get
            {
                if (!_MyChild2_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _MyChild2 = default(ExampleChild);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _MyChild2_ByteIndex, _MyChild2_ByteLength);
                        _MyChild2 = DeserializationFactory.Create(113, () => new ExampleChild(), childData, this); 
                    }
                    _MyChild2_Accessed = true;
                }
                return _MyChild2;
            }
            //[DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _MyChild2 = value;
                if (_MyChild2 != null)
                {
                    _MyChild2.IsDirty = true;
                }
                _MyChild2_Accessed = true;
            }
        }
        internal bool _MyChild2_Accessed;
        private ExampleChild _MyChild2Previous;
        public ExampleChild MyChild2Previous
        {
            //[DebuggerStepThrough]
            get
            {
                if (!_MyChild2Previous_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _MyChild2Previous = default(ExampleChild);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _MyChild2Previous_ByteIndex, _MyChild2Previous_ByteLength);
                        _MyChild2Previous = DeserializationFactory.Create(113, () => new ExampleChild(), childData, this); 
                    }
                    _MyChild2Previous_Accessed = true;
                }
                return _MyChild2Previous;
            }
            //[DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _MyChild2Previous = value;
                if (_MyChild2Previous != null)
                {
                    _MyChild2Previous.IsDirty = true;
                }
                _MyChild2Previous_Accessed = true;
            }
        }
        internal bool _MyChild2Previous_Accessed;
        private IExampleNonexclusiveInterface _MyInterfaceImplementer;
        public IExampleNonexclusiveInterface MyInterfaceImplementer
        {
            //[DebuggerStepThrough]
            get
            {
                if (!_MyInterfaceImplementer_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _MyInterfaceImplementer = default(IExampleNonexclusiveInterface);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _MyInterfaceImplementer_ByteIndex, _MyInterfaceImplementer_ByteLength);
                        _MyInterfaceImplementer = (IExampleNonexclusiveInterface)DeserializationFactory.FactoryCreate(childData, this); 
                    }
                    _MyInterfaceImplementer_Accessed = true;
                }
                return _MyInterfaceImplementer;
            }
            //[DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _MyInterfaceImplementer = value;
                if (_MyInterfaceImplementer != null)
                {
                    _MyInterfaceImplementer.IsDirty = true;
                }
                _MyInterfaceImplementer_Accessed = true;
            }
        }
        internal bool _MyInterfaceImplementer_Accessed;
        private NonLazinatorClass _MyNonLazinatorChild;
        public NonLazinatorClass MyNonLazinatorChild
        {
            //[DebuggerStepThrough]
            get
            {
                if (!_MyNonLazinatorChild_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _MyNonLazinatorChild = default(NonLazinatorClass);
                        _MyNonLazinatorChild_Dirty = true;
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _MyNonLazinatorChild_ByteIndex, _MyNonLazinatorChild_ByteLength);
                        _MyNonLazinatorChild = ConvertFromBytes_NonLazinatorClass(childData, DeserializationFactory, () => { MyNonLazinatorChild_Dirty = true; });
                    }
                    _MyNonLazinatorChild_Accessed = true;
                }
                return _MyNonLazinatorChild;
            }
            //[DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _MyNonLazinatorChild = value;
                _MyNonLazinatorChild_Dirty = true;
                _MyNonLazinatorChild_Accessed = true;
            }
        }
        internal bool _MyNonLazinatorChild_Accessed;
        
        private bool _MyNonLazinatorChild_Dirty;
        public bool MyNonLazinatorChild_Dirty
        {
            //[DebuggerStepThrough]
            get => _MyNonLazinatorChild_Dirty;
            //[DebuggerStepThrough]
            set
            {
                if (_MyNonLazinatorChild_Dirty != value)
                {
                    _MyNonLazinatorChild_Dirty = value;
                    if (value && !IsDirty)
                    IsDirty = true;
                }
            }
        }
        
        /* Conversion */
        
        public virtual int LazinatorUniqueID => 112;
        
        public virtual int LazinatorObjectVersion { get; set; } = 3;
        
        public virtual void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
            _MyBool = span.ToBoolean(ref bytesSoFar);
            _MyChar = span.ToChar(ref bytesSoFar);
            _MyDateTime = span.ToDecompressedDateTime(ref bytesSoFar);
            if (serializedVersionNumber >= 3) 
            {
                _MyNewString = span.ToString_VarIntLength(ref bytesSoFar);
            }
            if (serializedVersionNumber < 3) 
            {
                _MyOldString = span.ToString_VarIntLength(ref bytesSoFar);
            }
            _MyString = span.ToString_VarIntLength(ref bytesSoFar);
            _MyTestEnum = (TestEnum)span.ToDecompressedInt(ref bytesSoFar);
            _MyUint = span.ToDecompressedUint(ref bytesSoFar);
            _MyNullableDecimal = span.ToDecompressedNullableDecimal(ref bytesSoFar);
            _MyNullableDouble = span.ToNullableDouble(ref bytesSoFar);
            _MyNullableTimeSpan = span.ToDecompressedNullableTimeSpan(ref bytesSoFar);
            _MyTestEnumByteNullable = (TestEnumByte?)span.ToDecompressedNullableByte(ref bytesSoFar);
            _MyChild1_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren) 
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            _MyChild2_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren) 
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            _MyChild2Previous_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && serializedVersionNumber < 3) 
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            _MyInterfaceImplementer_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren) 
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            _MyNonLazinatorChild_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren) 
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
        }
        
        public virtual void SerializeExistingBuffer(BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
            // header information
            CompressedIntegralTypes.WriteCompressedInt(writer, LazinatorUniqueID);
            CompressedIntegralTypes.WriteCompressedInt(writer, Lazinator.Support.LazinatorVersionInfo.LazinatorIntVersion);
            CompressedIntegralTypes.WriteCompressedInt(writer, LazinatorObjectVersion);
            writer.Write((byte)includeChildrenMode);
            // write properties
            WriteUncompressedPrimitives.WriteBool(writer, _MyBool);
            EncodeCharAndString.WriteCharInTwoBytes(writer, _MyChar);
            CompressedIntegralTypes.WriteCompressedDateTime(writer, _MyDateTime);
            if (LazinatorObjectVersion >= 3) 
            {
                EncodeCharAndString.WriteStringWithVarIntPrefix(writer, _MyNewString);
            }
            if (LazinatorObjectVersion < 3) 
            {
                EncodeCharAndString.WriteStringWithVarIntPrefix(writer, _MyOldString);
            }
            EncodeCharAndString.WriteStringWithVarIntPrefix(writer, _MyString);
            CompressedIntegralTypes.WriteCompressedInt(writer, (int)_MyTestEnum);
            CompressedIntegralTypes.WriteCompressedUint(writer, _MyUint);
            CompressedDecimal.WriteCompressedNullableDecimal(writer, _MyNullableDecimal);
            WriteUncompressedPrimitives.WriteNullableDouble(writer, _MyNullableDouble);
            CompressedIntegralTypes.WriteCompressedNullableTimeSpan(writer, _MyNullableTimeSpan);
            CompressedIntegralTypes.WriteCompressedNullableByte(writer, (byte?)_MyTestEnumByteNullable);
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren) 
            {
                WriteChildWithLength(writer, _MyChild1, includeChildrenMode, _MyChild1_Accessed, () => GetChildSlice(LazinatorObjectBytes, _MyChild1_ByteIndex, _MyChild1_ByteLength), verifyCleanness);
            }
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren) 
            {
                WriteChildWithLength(writer, _MyChild2, includeChildrenMode, _MyChild2_Accessed, () => GetChildSlice(LazinatorObjectBytes, _MyChild2_ByteIndex, _MyChild2_ByteLength), verifyCleanness);
            }
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && LazinatorObjectVersion < 3) 
            {
                WriteChildWithLength(writer, _MyChild2Previous, includeChildrenMode, _MyChild2Previous_Accessed, () => GetChildSlice(LazinatorObjectBytes, _MyChild2Previous_ByteIndex, _MyChild2Previous_ByteLength), verifyCleanness);
            }
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren) 
            {
                WriteChildWithLength(writer, _MyInterfaceImplementer, includeChildrenMode, _MyInterfaceImplementer_Accessed, () => GetChildSlice(LazinatorObjectBytes, _MyInterfaceImplementer_ByteIndex, _MyInterfaceImplementer_ByteLength), verifyCleanness);
            }
            WriteNonLazinatorObject(
            nonLazinatorObject: _MyNonLazinatorChild, isBelievedDirty: MyNonLazinatorChild_Dirty,
            isAccessed: _MyNonLazinatorChild_Accessed, writer: writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorObjectBytes, _MyNonLazinatorChild_ByteIndex, _MyNonLazinatorChild_ByteLength),
            verifyCleanness: verifyCleanness,
            binaryWriterAction: (w, v) =>
            ConvertToBytes_NonLazinatorClass(w, MyNonLazinatorChild,
            includeChildrenMode, v));
        }
        
    }
}
