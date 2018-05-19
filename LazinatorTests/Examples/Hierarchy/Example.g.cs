//fddbc229-d718-83fd-b103-c26576c24cc4
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.31
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LazinatorTests.Examples
{
    using System;
    using System.Buffers;
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
    
    
    public partial class Example : ILazinator
    {
        /* Boilerplate for every non-abstract ILazinator object */
        
        public virtual ILazinator LazinatorParentClass { get; set; }
        
        protected IncludeChildrenMode OriginalIncludeChildrenMode;
        
        public virtual void Deserialize()
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
                        OnDirty();
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
        
        private bool _DescendantIsDirty;
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
            if (_ExcludableChild_Accessed)
            {
                ExcludableChild.MarkHierarchyClean();
            }
            if (_IncludableChild_Accessed)
            {
                IncludableChild.MarkHierarchyClean();
            }
            if (_MyChild1_Accessed)
            {
                MyChild1.MarkHierarchyClean();
            }
            if (_MyChild2_Accessed)
            {
                MyChild2.MarkHierarchyClean();
            }
            if (_MyChild2Previous_Accessed)
            {
                MyChild2Previous.MarkHierarchyClean();
            }
            if (_MyInterfaceImplementer_Accessed)
            {
                MyInterfaceImplementer.MarkHierarchyClean();
            }
            if (_WrappedInt_Accessed)
            {
                WrappedInt.MarkHierarchyClean();
            }
            _MyNonLazinatorChild_Dirty = false;
        }
        
        public virtual DeserializationFactory DeserializationFactory { get; set; }
        
        private MemoryInBuffer _HierarchyBytes;
        public virtual MemoryInBuffer HierarchyBytes
        {
            get => _HierarchyBytes;
            set
            {
                _HierarchyBytes = value;
                LazinatorObjectBytes = value.FilledMemory;
            }
        }
        
        private ReadOnlyMemory<byte> _LazinatorObjectBytes;
        public virtual ReadOnlyMemory<byte> LazinatorObjectBytes
        {
            get => _LazinatorObjectBytes;
            set
            {
                _LazinatorObjectBytes = value;
                Deserialize();
            }
        }
        
        public virtual void LazinatorConvertToBytes()
        {
            if (!IsDirty && !DescendantIsDirty)
            {
                return;
            }
            MemoryInBuffer bytes = EncodeOrRecycleToNewBuffer(IncludeChildrenMode.IncludeAllChildren, OriginalIncludeChildrenMode, false, false, IsDirty, DescendantIsDirty, false, LazinatorObjectBytes, (StreamManuallyDelegate)EncodeToNewBuffer);
            _IsDirty = false;
            LazinatorObjectBytes = bytes.FilledMemory;
            _ExcludableChild_Accessed = _IncludableChild_Accessed = _MyChild1_Accessed = _MyChild2_Accessed = _MyChild2Previous_Accessed = _MyInterfaceImplementer_Accessed = _WrappedInt_Accessed = false;
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
        
        /* Field boilerplate */
        
        protected int _ExcludableChild_ByteIndex;
        protected int _IncludableChild_ByteIndex;
        protected int _MyChild1_ByteIndex;
        protected int _MyChild2_ByteIndex;
        protected int _MyChild2Previous_ByteIndex;
        protected int _MyInterfaceImplementer_ByteIndex;
        protected int _MyNonLazinatorChild_ByteIndex;
        protected int _WrappedInt_ByteIndex;
        protected virtual int _ExcludableChild_ByteLength => _IncludableChild_ByteIndex - _ExcludableChild_ByteIndex;
        protected virtual int _IncludableChild_ByteLength => _MyChild1_ByteIndex - _IncludableChild_ByteIndex;
        protected virtual int _MyChild1_ByteLength => _MyChild2_ByteIndex - _MyChild1_ByteIndex;
        protected virtual int _MyChild2_ByteLength => _MyChild2Previous_ByteIndex - _MyChild2_ByteIndex;
        protected virtual int _MyChild2Previous_ByteLength => _MyInterfaceImplementer_ByteIndex - _MyChild2Previous_ByteIndex;
        protected virtual int _MyInterfaceImplementer_ByteLength => _MyNonLazinatorChild_ByteIndex - _MyInterfaceImplementer_ByteIndex;
        protected virtual int _MyNonLazinatorChild_ByteLength => _WrappedInt_ByteIndex - _MyNonLazinatorChild_ByteIndex;
        private int _Example_EndByteIndex;
        protected virtual int _WrappedInt_ByteLength => _Example_EndByteIndex - _WrappedInt_ByteIndex;
        
        private bool _MyBool;
        public bool MyBool
        {
            [DebuggerStepThrough]
            get
            {
                return _MyBool;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _MyBool = value;
            }
        }
        private char _MyChar;
        public char MyChar
        {
            [DebuggerStepThrough]
            get
            {
                return _MyChar;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _MyChar = value;
            }
        }
        private global::System.DateTime _MyDateTime;
        [Newtonsoft.Json.JsonProperty("MyDT")]
        public global::System.DateTime MyDateTime
        {
            [DebuggerStepThrough]
            get
            {
                return _MyDateTime;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _MyDateTime = value;
            }
        }
        private global::LazinatorTests.Examples.Example.EnumWithinClass _MyEnumWithinClass;
        public global::LazinatorTests.Examples.Example.EnumWithinClass MyEnumWithinClass
        {
            [DebuggerStepThrough]
            get
            {
                return _MyEnumWithinClass;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _MyEnumWithinClass = value;
            }
        }
        private string _MyNewString;
        public string MyNewString
        {
            [DebuggerStepThrough]
            get
            {
                return _MyNewString;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _MyNewString = value;
            }
        }
        private decimal? _MyNullableDecimal;
        public decimal? MyNullableDecimal
        {
            [DebuggerStepThrough]
            get
            {
                return _MyNullableDecimal;
            }
            [DebuggerStepThrough]
            internal set
            {
                IsDirty = true;
                _MyNullableDecimal = value;
            }
        }
        private double? _MyNullableDouble;
        public virtual double? MyNullableDouble
        {
            [DebuggerStepThrough]
            get
            {
                return _MyNullableDouble;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _MyNullableDouble = value;
            }
        }
        private global::System.TimeSpan? _MyNullableTimeSpan;
        public global::System.TimeSpan? MyNullableTimeSpan
        {
            [DebuggerStepThrough]
            get
            {
                return _MyNullableTimeSpan;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _MyNullableTimeSpan = value;
            }
        }
        private string _MyOldString;
        public string MyOldString
        {
            [DebuggerStepThrough]
            get
            {
                return _MyOldString;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _MyOldString = value;
            }
        }
        private string _MyString;
        public string MyString
        {
            [DebuggerStepThrough]
            get
            {
                return _MyString;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _MyString = value;
            }
        }
        private global::LazinatorTests.Examples.TestEnum _MyTestEnum;
        public global::LazinatorTests.Examples.TestEnum MyTestEnum
        {
            [DebuggerStepThrough]
            get
            {
                return _MyTestEnum;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _MyTestEnum = value;
            }
        }
        private global::LazinatorTests.Examples.TestEnumByte? _MyTestEnumByteNullable;
        public global::LazinatorTests.Examples.TestEnumByte? MyTestEnumByteNullable
        {
            [DebuggerStepThrough]
            get
            {
                return _MyTestEnumByteNullable;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _MyTestEnumByteNullable = value;
            }
        }
        private uint _MyUint;
        public uint MyUint
        {
            [DebuggerStepThrough]
            get
            {
                return _MyUint;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _MyUint = value;
            }
        }
        private global::LazinatorTests.Examples.ExampleChild _ExcludableChild;
        public global::LazinatorTests.Examples.ExampleChild ExcludableChild
        {
            [DebuggerStepThrough]
            get
            {
                if (!_ExcludableChild_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _ExcludableChild = default(global::LazinatorTests.Examples.ExampleChild);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _ExcludableChild_ByteIndex, _ExcludableChild_ByteLength);
                        
                        if (DeserializationFactory == null)
                        {
                            LazinatorDeserializationException.ThrowNoDeserializationFactory();
                        }
                        _ExcludableChild = DeserializationFactory.Create(213, () => new global::LazinatorTests.Examples.ExampleChild(), childData, this); 
                    }
                    _ExcludableChild_Accessed = true;
                }
                return _ExcludableChild;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _ExcludableChild = value;
                if (_ExcludableChild != null)
                {
                    _ExcludableChild.IsDirty = true;
                }
                _ExcludableChild_Accessed = true;
            }
        }
        protected bool _ExcludableChild_Accessed;
        private global::LazinatorTests.Examples.ExampleChild _IncludableChild;
        public global::LazinatorTests.Examples.ExampleChild IncludableChild
        {
            [DebuggerStepThrough]
            get
            {
                if (!_IncludableChild_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _IncludableChild = default(global::LazinatorTests.Examples.ExampleChild);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _IncludableChild_ByteIndex, _IncludableChild_ByteLength);
                        
                        if (DeserializationFactory == null)
                        {
                            LazinatorDeserializationException.ThrowNoDeserializationFactory();
                        }
                        _IncludableChild = DeserializationFactory.Create(213, () => new global::LazinatorTests.Examples.ExampleChild(), childData, this); 
                    }
                    _IncludableChild_Accessed = true;
                }
                return _IncludableChild;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _IncludableChild = value;
                if (_IncludableChild != null)
                {
                    _IncludableChild.IsDirty = true;
                }
                _IncludableChild_Accessed = true;
            }
        }
        protected bool _IncludableChild_Accessed;
        private global::LazinatorTests.Examples.ExampleChild _MyChild1;
        public global::LazinatorTests.Examples.ExampleChild MyChild1
        {
            [DebuggerStepThrough]
            get
            {
                if (!_MyChild1_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _MyChild1 = default(global::LazinatorTests.Examples.ExampleChild);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _MyChild1_ByteIndex, _MyChild1_ByteLength);
                        
                        if (DeserializationFactory == null)
                        {
                            LazinatorDeserializationException.ThrowNoDeserializationFactory();
                        }
                        _MyChild1 = DeserializationFactory.Create(213, () => new global::LazinatorTests.Examples.ExampleChild(), childData, this); 
                    }
                    _MyChild1_Accessed = true;
                }
                return _MyChild1;
            }
            [DebuggerStepThrough]
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
        protected bool _MyChild1_Accessed;
        private global::LazinatorTests.Examples.ExampleChild _MyChild2;
        public global::LazinatorTests.Examples.ExampleChild MyChild2
        {
            [DebuggerStepThrough]
            get
            {
                if (!_MyChild2_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _MyChild2 = default(global::LazinatorTests.Examples.ExampleChild);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _MyChild2_ByteIndex, _MyChild2_ByteLength);
                        
                        if (DeserializationFactory == null)
                        {
                            LazinatorDeserializationException.ThrowNoDeserializationFactory();
                        }
                        _MyChild2 = DeserializationFactory.Create(213, () => new global::LazinatorTests.Examples.ExampleChild(), childData, this); 
                    }
                    _MyChild2_Accessed = true;
                }
                return _MyChild2;
            }
            [DebuggerStepThrough]
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
        protected bool _MyChild2_Accessed;
        private global::LazinatorTests.Examples.ExampleChild _MyChild2Previous;
        public global::LazinatorTests.Examples.ExampleChild MyChild2Previous
        {
            [DebuggerStepThrough]
            get
            {
                if (!_MyChild2Previous_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _MyChild2Previous = default(global::LazinatorTests.Examples.ExampleChild);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _MyChild2Previous_ByteIndex, _MyChild2Previous_ByteLength);
                        
                        if (DeserializationFactory == null)
                        {
                            LazinatorDeserializationException.ThrowNoDeserializationFactory();
                        }
                        _MyChild2Previous = DeserializationFactory.Create(213, () => new global::LazinatorTests.Examples.ExampleChild(), childData, this); 
                    }
                    _MyChild2Previous_Accessed = true;
                }
                return _MyChild2Previous;
            }
            [DebuggerStepThrough]
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
        protected bool _MyChild2Previous_Accessed;
        private global::LazinatorTests.Examples.IExampleNonexclusiveInterface _MyInterfaceImplementer;
        public global::LazinatorTests.Examples.IExampleNonexclusiveInterface MyInterfaceImplementer
        {
            [DebuggerStepThrough]
            get
            {
                if (!_MyInterfaceImplementer_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _MyInterfaceImplementer = default(global::LazinatorTests.Examples.IExampleNonexclusiveInterface);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _MyInterfaceImplementer_ByteIndex, _MyInterfaceImplementer_ByteLength);
                        
                        if (DeserializationFactory == null)
                        {
                            LazinatorDeserializationException.ThrowNoDeserializationFactory();
                        }
                        _MyInterfaceImplementer = (global::LazinatorTests.Examples.IExampleNonexclusiveInterface)DeserializationFactory.FactoryCreate(childData, this); 
                    }
                    _MyInterfaceImplementer_Accessed = true;
                }
                return _MyInterfaceImplementer;
            }
            [DebuggerStepThrough]
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
        protected bool _MyInterfaceImplementer_Accessed;
        private global::LazinatorTests.Examples.NonLazinatorClass _MyNonLazinatorChild;
        public global::LazinatorTests.Examples.NonLazinatorClass MyNonLazinatorChild
        {
            [DebuggerStepThrough]
            get
            {
                if (!_MyNonLazinatorChild_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _MyNonLazinatorChild = default(global::LazinatorTests.Examples.NonLazinatorClass);
                        _MyNonLazinatorChild_Dirty = true;
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _MyNonLazinatorChild_ByteIndex, _MyNonLazinatorChild_ByteLength);
                        _MyNonLazinatorChild = LazinatorTests.Examples.NonLazinatorDirectConverter.ConvertFromBytes_LazinatorTests__Examples__NonLazinatorClass(childData, DeserializationFactory, () => { MyNonLazinatorChild_Dirty = true; });
                    }
                    _MyNonLazinatorChild_Accessed = true;
                }
                return _MyNonLazinatorChild;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _MyNonLazinatorChild = value;
                _MyNonLazinatorChild_Dirty = true;
                _MyNonLazinatorChild_Accessed = true;
            }
        }
        protected bool _MyNonLazinatorChild_Accessed;
        
        private bool _MyNonLazinatorChild_Dirty;
        public bool MyNonLazinatorChild_Dirty
        {
            [DebuggerStepThrough]
            get => _MyNonLazinatorChild_Dirty;
            [DebuggerStepThrough]
            set
            {
                if (_MyNonLazinatorChild_Dirty != value)
                {
                    _MyNonLazinatorChild_Dirty = value;
                    if (value && !IsDirty)
                    {
                        IsDirty = true;
                    }
                }
            }
        }
        private global::Lazinator.Wrappers.LazinatorWrapperInt _WrappedInt;
        public global::Lazinator.Wrappers.LazinatorWrapperInt WrappedInt
        {
            [DebuggerStepThrough]
            get
            {
                if (!_WrappedInt_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _WrappedInt = default(global::Lazinator.Wrappers.LazinatorWrapperInt);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _WrappedInt_ByteIndex, _WrappedInt_ByteLength, true);
                        _WrappedInt = new global::Lazinator.Wrappers.LazinatorWrapperInt()
                        {
                            DeserializationFactory = DeserializationFactory,
                            LazinatorParentClass = this,
                            LazinatorObjectBytes = childData,
                        };
                    }
                    _WrappedInt_Accessed = true;
                }
                return _WrappedInt;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _WrappedInt = value;
                _WrappedInt_Accessed = true;
            }
        }
        protected bool _WrappedInt_Accessed;
        public global::Lazinator.Wrappers.LazinatorWrapperInt WrappedInt_Copy
        {
            [DebuggerStepThrough]
            get
            {
                if (!_WrappedInt_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        return default(global::Lazinator.Wrappers.LazinatorWrapperInt);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _WrappedInt_ByteIndex, _WrappedInt_ByteLength);
                        return new global::Lazinator.Wrappers.LazinatorWrapperInt()
                        {
                            DeserializationFactory = DeserializationFactory,
                            LazinatorObjectBytes = childData,
                        };
                    }
                }
                return _WrappedInt;
            }
        }
        
        /* Conversion */
        
        public virtual int LazinatorUniqueID => 212;
        
        public virtual int LazinatorObjectVersion { get; set; } = 3;
        
        public virtual void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
            _MyBool = span.ToBoolean(ref bytesSoFar);
            _MyChar = span.ToChar(ref bytesSoFar);
            _MyDateTime = span.ToDecompressedDateTime(ref bytesSoFar);
            _MyEnumWithinClass = (global::LazinatorTests.Examples.Example.EnumWithinClass)span.ToDecompressedInt(ref bytesSoFar);
            if (serializedVersionNumber >= 3) 
            {
                _MyNewString = span.ToString_BrotliCompressedWithLength(ref bytesSoFar);
            }
            _MyNullableDecimal = span.ToDecompressedNullableDecimal(ref bytesSoFar);
            _MyNullableDouble = span.ToNullableDouble(ref bytesSoFar);
            _MyNullableTimeSpan = span.ToDecompressedNullableTimeSpan(ref bytesSoFar);
            if (serializedVersionNumber < 3) 
            {
                _MyOldString = span.ToString_BrotliCompressedWithLength(ref bytesSoFar);
            }
            _MyString = span.ToString_BrotliCompressedWithLength(ref bytesSoFar);
            _MyTestEnum = (global::LazinatorTests.Examples.TestEnum)span.ToDecompressedInt(ref bytesSoFar);
            _MyTestEnumByteNullable = (global::LazinatorTests.Examples.TestEnumByte?)span.ToDecompressedNullableByte(ref bytesSoFar);
            _MyUint = span.ToDecompressedUint(ref bytesSoFar);
            _ExcludableChild_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren && includeChildrenMode != IncludeChildrenMode.ExcludeOnlyExcludableChildren) 
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            _IncludableChild_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren) 
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            _MyChild1_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            _MyChild2_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            _MyChild2Previous_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren && serializedVersionNumber < 3) 
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            _MyInterfaceImplementer_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            _MyNonLazinatorChild_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            _WrappedInt_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                bytesSoFar = span.ToByte(ref bytesSoFar) + bytesSoFar;
            }
            _Example_EndByteIndex = bytesSoFar;
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
            CompressedIntegralTypes.WriteCompressedInt(writer, (int) _MyEnumWithinClass);
            if (LazinatorObjectVersion >= 3) 
            {
                EncodeCharAndString.WriteBrotliCompressedWithIntPrefix(writer, _MyNewString);
            }
            CompressedDecimal.WriteCompressedNullableDecimal(writer, _MyNullableDecimal);
            WriteUncompressedPrimitives.WriteNullableDouble(writer, _MyNullableDouble);
            CompressedIntegralTypes.WriteCompressedNullableTimeSpan(writer, _MyNullableTimeSpan);
            if (LazinatorObjectVersion < 3) 
            {
                EncodeCharAndString.WriteBrotliCompressedWithIntPrefix(writer, _MyOldString);
            }
            EncodeCharAndString.WriteBrotliCompressedWithIntPrefix(writer, _MyString);
            CompressedIntegralTypes.WriteCompressedInt(writer, (int) _MyTestEnum);
            CompressedIntegralTypes.WriteCompressedNullableByte(writer, (byte?) _MyTestEnumByteNullable);
            CompressedIntegralTypes.WriteCompressedUint(writer, _MyUint);
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren && includeChildrenMode != IncludeChildrenMode.ExcludeOnlyExcludableChildren) 
            {
                WriteChildWithLength(writer, _ExcludableChild, includeChildrenMode, _ExcludableChild_Accessed, () => GetChildSlice(LazinatorObjectBytes, _ExcludableChild_ByteIndex, _ExcludableChild_ByteLength), verifyCleanness, false);
            }
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren) 
            {
                WriteChildWithLength(writer, _IncludableChild, includeChildrenMode, _IncludableChild_Accessed, () => GetChildSlice(LazinatorObjectBytes, _IncludableChild_ByteIndex, _IncludableChild_ByteLength), verifyCleanness, false);
            }
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                WriteChildWithLength(writer, _MyChild1, includeChildrenMode, _MyChild1_Accessed, () => GetChildSlice(LazinatorObjectBytes, _MyChild1_ByteIndex, _MyChild1_ByteLength), verifyCleanness, false);
            }
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                WriteChildWithLength(writer, _MyChild2, includeChildrenMode, _MyChild2_Accessed, () => GetChildSlice(LazinatorObjectBytes, _MyChild2_ByteIndex, _MyChild2_ByteLength), verifyCleanness, false);
            }
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren && LazinatorObjectVersion < 3) 
            {
                WriteChildWithLength(writer, _MyChild2Previous, includeChildrenMode, _MyChild2Previous_Accessed, () => GetChildSlice(LazinatorObjectBytes, _MyChild2Previous_ByteIndex, _MyChild2Previous_ByteLength), verifyCleanness, false);
            }
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                WriteChildWithLength(writer, _MyInterfaceImplementer, includeChildrenMode, _MyInterfaceImplementer_Accessed, () => GetChildSlice(LazinatorObjectBytes, _MyInterfaceImplementer_ByteIndex, _MyInterfaceImplementer_ByteLength), verifyCleanness, false);
            }
            WriteNonLazinatorObject(
            nonLazinatorObject: _MyNonLazinatorChild, isBelievedDirty: MyNonLazinatorChild_Dirty,
            isAccessed: _MyNonLazinatorChild_Accessed, writer: writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorObjectBytes, _MyNonLazinatorChild_ByteIndex, _MyNonLazinatorChild_ByteLength),
            verifyCleanness: verifyCleanness,
            binaryWriterAction: (w, v) =>
            LazinatorTests.Examples.NonLazinatorDirectConverter.ConvertToBytes_LazinatorTests__Examples__NonLazinatorClass(w, MyNonLazinatorChild,
            includeChildrenMode, v));
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                WriteChildWithLength(writer, _WrappedInt, includeChildrenMode, _WrappedInt_Accessed, () => GetChildSlice(LazinatorObjectBytes, _WrappedInt_ByteIndex, _WrappedInt_ByteLength), verifyCleanness, true);
            }
        }
        
    }
}
