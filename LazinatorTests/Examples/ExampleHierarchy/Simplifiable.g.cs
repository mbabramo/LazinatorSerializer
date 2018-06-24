//44502902-647f-4a9c-93f8-c4a5507dbc57
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.153
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LazinatorTests.Examples
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
    using System.Linq;
    using System.Runtime.InteropServices;
    using static Lazinator.Core.LazinatorUtilities;
    
    [Autogenerated]
    public partial class Simplifiable : ILazinator
    {
        /* Serialization, deserialization, and object relationships */
        
        protected ILazinator _LazinatorParentClass;
        public Simplifiable() : base()
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
            var clone = new Simplifiable()
            {
                LazinatorParentClass = LazinatorParentClass,
                OriginalIncludeChildrenMode = includeChildrenMode,
                HierarchyBytes = bytes,
            };
            clone.LazinatorParentClass = null;
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
                if (_IsDirty != value)
                {
                    _IsDirty = value;
                    if (_IsDirty)
                    {
                        InformParentOfDirtiness();
                    }
                }
                if (_IsDirty)
                {
                    HasChanged = true;
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
                        if (LazinatorParentClass != null)
                        {
                            LazinatorParentClass.DescendantIsDirty = true;
                        }
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
        
        protected int _Example_ByteIndex;
        protected int _Example2_ByteIndex;
        protected int _Example3_ByteIndex;
        protected virtual int _Example_ByteLength => _Example2_ByteIndex - _Example_ByteIndex;
        protected virtual int _Example2_ByteLength => _Example3_ByteIndex - _Example2_ByteIndex;
        private int _Simplifiable_EndByteIndex;
        protected virtual int _Example3_ByteLength => _Simplifiable_EndByteIndex - _Example3_ByteIndex;
        
        private bool _MyIntsAre3;
        public bool MyIntsAre3
        {
            get
            {
                return _MyIntsAre3;
            }
            private set
            {
                IsDirty = true;
                _MyIntsAre3 = value;
            }
        }
        private char? _Example2Char;
        public char? Example2Char
        {
            get
            {
                return _Example2Char;
            }
            private set
            {
                IsDirty = true;
                _Example2Char = value;
            }
        }
        private bool _Example3IsNull;
        public bool Example3IsNull
        {
            get
            {
                return _Example3IsNull;
            }
            private set
            {
                IsDirty = true;
                _Example3IsNull = value;
            }
        }
        private bool _ExampleHasDefaultValue;
        public bool ExampleHasDefaultValue
        {
            get
            {
                return _ExampleHasDefaultValue;
            }
            private set
            {
                IsDirty = true;
                _ExampleHasDefaultValue = value;
            }
        }
        private int _MyInt;
        public int MyInt
        {
            get
            {
                return _MyInt;
            }
            set
            {
                IsDirty = true;
                _MyInt = value;
            }
        }
        private int _MyOtherInt;
        public int MyOtherInt
        {
            get
            {
                return _MyOtherInt;
            }
            set
            {
                IsDirty = true;
                _MyOtherInt = value;
            }
        }
        private Example _Example;
        public Example Example
        {
            get
            {
                if (!_Example_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _Example = default(Example);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _Example_ByteIndex, _Example_ByteLength, false, false, null);
                        
                        _Example = DeserializationFactory.Instance.CreateBaseOrDerivedType(212, () => new Example(), childData, this); 
                    }
                    _Example_Accessed = true;
                } 
                return _Example;
            }
            set
            {
                if (value != null)
                {
                    if (value.LazinatorParentClass != null)
                    {
                        throw new MovedLazinatorException($"The property Example cannot be set to a Lazinator object with a defined LazinatorParentClass, because AutoChangeParent is set to false in the configuration file and no attribute providing an exception is present.");
                    }
                    value.LazinatorParentClass = this;
                    value.IsDirty = true;
                }
                IsDirty = true;
                _Example = value;
                _Example_Accessed = true;
            }
        }
        protected bool _Example_Accessed;
        private Example _Example2;
        public Example Example2
        {
            get
            {
                if (!_Example2_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _Example2 = default(Example);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _Example2_ByteIndex, _Example2_ByteLength, false, false, null);
                        
                        _Example2 = DeserializationFactory.Instance.CreateBaseOrDerivedType(212, () => new Example(), childData, this); 
                    }
                    _Example2_Accessed = true;
                } 
                return _Example2;
            }
            set
            {
                if (value != null)
                {
                    if (value.LazinatorParentClass != null)
                    {
                        throw new MovedLazinatorException($"The property Example2 cannot be set to a Lazinator object with a defined LazinatorParentClass, because AutoChangeParent is set to false in the configuration file and no attribute providing an exception is present.");
                    }
                    value.LazinatorParentClass = this;
                    value.IsDirty = true;
                }
                IsDirty = true;
                _Example2 = value;
                _Example2_Accessed = true;
            }
        }
        protected bool _Example2_Accessed;
        private Example _Example3;
        public Example Example3
        {
            get
            {
                if (!_Example3_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _Example3 = default(Example);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _Example3_ByteIndex, _Example3_ByteLength, false, false, null);
                        
                        _Example3 = DeserializationFactory.Instance.CreateBaseOrDerivedType(212, () => new Example(), childData, this); 
                    }
                    _Example3_Accessed = true;
                } 
                return _Example3;
            }
            set
            {
                if (value != null)
                {
                    if (value.LazinatorParentClass != null)
                    {
                        throw new MovedLazinatorException($"The property Example3 cannot be set to a Lazinator object with a defined LazinatorParentClass, because AutoChangeParent is set to false in the configuration file and no attribute providing an exception is present.");
                    }
                    value.LazinatorParentClass = this;
                    value.IsDirty = true;
                }
                IsDirty = true;
                _Example3 = value;
                _Example3_Accessed = true;
            }
        }
        protected bool _Example3_Accessed;
        
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
            if (enumerateNulls && (Example == null))
            {
                yield return ("Example", default);
            }
            else if ((!exploreOnlyDeserializedChildren && Example != null) || (_Example_Accessed && _Example != null))
            {
                foreach (ILazinator toYield in Example.EnumerateLazinatorNodes(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
                {
                    yield return ("Example", toYield);
                }
            }
            if (enumerateNulls && (Example2 == null))
            {
                yield return ("Example2", default);
            }
            else if ((!exploreOnlyDeserializedChildren && Example2 != null) || (_Example2_Accessed && _Example2 != null))
            {
                foreach (ILazinator toYield in Example2.EnumerateLazinatorNodes(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
                {
                    yield return ("Example2", toYield);
                }
            }
            if (enumerateNulls && (Example3 == null))
            {
                yield return ("Example3", default);
            }
            else if ((!exploreOnlyDeserializedChildren && Example3 != null) || (_Example3_Accessed && _Example3 != null))
            {
                foreach (ILazinator toYield in Example3.EnumerateLazinatorNodes(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
                {
                    yield return ("Example3", toYield);
                }
            }
            yield break;
        }
        
        
        public virtual IEnumerable<(string propertyName, object descendant)> EnumerateNonLazinatorProperties()
        {
            yield return ("MyIntsAre3", (object)MyIntsAre3);
            yield return ("Example2Char", (object)Example2Char);
            yield return ("Example3IsNull", (object)Example3IsNull);
            yield return ("ExampleHasDefaultValue", (object)ExampleHasDefaultValue);
            yield return ("MyInt", (object)MyInt);
            yield return ("MyOtherInt", (object)MyOtherInt);
            yield break;
        }
        
        protected virtual void ResetAccessedProperties()
        {
            _Example_Accessed = _Example2_Accessed = _Example3_Accessed = false;
        }
        
        /* Conversion */
        
        public virtual int LazinatorUniqueID => 273;
        
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
            _MyIntsAre3 = span.ToBoolean(ref bytesSoFar);
            _Example2Char = span.ToNullableChar(ref bytesSoFar);
            _Example3IsNull = span.ToBoolean(ref bytesSoFar);
            _ExampleHasDefaultValue = span.ToBoolean(ref bytesSoFar);
            
            if (MyIntsAre3)
            {
                SetMyIntsTo3();
            }
            else
            {
                _MyInt = span.ToDecompressedInt(ref bytesSoFar);
            }
            
            if (MyIntsAre3)
            {
                // is set above
            }
            else
            {
                _MyOtherInt = span.ToDecompressedInt(ref bytesSoFar);
            }
            _Example_ByteIndex = bytesSoFar;
            if (ExampleHasDefaultValue)
            {
                SetExampleToDefaultValue();
            }
            else
            {
                if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
                {
                    bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
                }
            }
            _Example2_ByteIndex = bytesSoFar;
            if (Example2Char != null)
            {
                Example2 = new Example() { MyChar = (char)Example2Char };
            }
            else
            {
                if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren && serializedVersionNumber >= 4) 
                {
                    bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
                }
            }
            _Example3_ByteIndex = bytesSoFar;
            if (Example3IsNull)
            {
                Example3 = null;
            }
            else
            {
                if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
                {
                    bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
                }
            }
            _Simplifiable_EndByteIndex = bytesSoFar;
        }
        
        public virtual void SerializeExistingBuffer(BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            TabbedText.WriteLine($"Initiating serialization of LazinatorTests.Examples.Simplifiable ");
            if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren)
            {
                updateStoredBuffer = false;
            }
            PreSerialization(verifyCleanness, updateStoredBuffer);
            int startPosition = writer.Position;
            WritePropertiesIntoBuffer(writer, includeChildrenMode, verifyCleanness, updateStoredBuffer, true);
            if (updateStoredBuffer)
            {
                
                _IsDirty = false;
                _DescendantIsDirty = includeChildrenMode != IncludeChildrenMode.IncludeAllChildren && ((_Example_Accessed && _Example != null && (Example.IsDirty || Example.DescendantIsDirty)) || (_Example2_Accessed && _Example2 != null && (Example2.IsDirty || Example2.DescendantIsDirty)) || (_Example3_Accessed && _Example3 != null && (Example3.IsDirty || Example3.DescendantIsDirty)));
                
                _LazinatorObjectBytes = writer.Slice(startPosition);
            }
        }
        protected virtual void WritePropertiesIntoBuffer(BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer, bool includeUniqueID)
        {
            int startPosition = writer.Position;
            int startOfObjectPosition = 0;
            // header information
            TabbedText.WriteLine($"Writing properties for LazinatorTests.Examples.Simplifiable starting at {writer.Position}.");
            TabbedText.WriteLine($"Includes? uniqueID {(LazinatorGenericID.IsEmpty ? LazinatorUniqueID.ToString() : String.Join("","",LazinatorGenericID.TypeAndInnerTypeIDs.ToArray()))} {includeUniqueID}, Lazinator version {Lazinator.Support.LazinatorVersionInfo.LazinatorIntVersion} True, Object version {LazinatorObjectVersion} True, IncludeChildrenMode {includeChildrenMode} True");
            TabbedText.WriteLine($"IsDirty {IsDirty} DescendantIsDirty {DescendantIsDirty} HasParentClass {LazinatorParentClass != null}");
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
            TabbedText.WriteLine($"Byte {writer.Position}, MyIntsAre3 value {_MyIntsAre3}");
            TabbedText.Tabs++;
            WriteUncompressedPrimitives.WriteBool(writer, _MyIntsAre3);
            TabbedText.Tabs--;
            TabbedText.WriteLine($"Byte {writer.Position}, Example2Char value {_Example2Char}");
            TabbedText.Tabs++;
            EncodeCharAndString.WriteNullableChar(writer, _Example2Char);
            TabbedText.Tabs--;
            TabbedText.WriteLine($"Byte {writer.Position}, Example3IsNull value {_Example3IsNull}");
            TabbedText.Tabs++;
            WriteUncompressedPrimitives.WriteBool(writer, _Example3IsNull);
            TabbedText.Tabs--;
            TabbedText.WriteLine($"Byte {writer.Position}, ExampleHasDefaultValue value {_ExampleHasDefaultValue}");
            TabbedText.Tabs++;
            WriteUncompressedPrimitives.WriteBool(writer, _ExampleHasDefaultValue);
            TabbedText.Tabs--;
            TabbedText.WriteLine($"Byte {writer.Position}, MyInt value {_MyInt}");
            TabbedText.Tabs++;
            if (!(MyIntsAre3))
            {
                CompressedIntegralTypes.WriteCompressedInt(writer, _MyInt);
            }
            TabbedText.Tabs--;
            TabbedText.WriteLine($"Byte {writer.Position}, MyOtherInt value {_MyOtherInt}");
            TabbedText.Tabs++;
            if (!(MyIntsAre3))
            {
                CompressedIntegralTypes.WriteCompressedInt(writer, _MyOtherInt);
            }
            TabbedText.Tabs--;
            TabbedText.WriteLine($"Byte {writer.Position}, Example (accessed? {_Example_Accessed}) (backing var null? {_Example == null}) ");
            TabbedText.Tabs++;
            if (!(ExampleHasDefaultValue))
            {
                startOfObjectPosition = writer.Position;
                if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
                {
                    WriteChild(writer, _Example, includeChildrenMode, _Example_Accessed, () => GetChildSlice(LazinatorObjectBytes, _Example_ByteIndex, _Example_ByteLength, false, false, null), verifyCleanness, updateStoredBuffer, false, false, this);
                }
            }
            if (updateStoredBuffer)
            {
                _Example_ByteIndex = startOfObjectPosition - startPosition;
            }
            TabbedText.Tabs--;
            TabbedText.WriteLine($"Byte {writer.Position}, Example2 (accessed? {_Example2_Accessed}) (backing var null? {_Example2 == null}) ");
            TabbedText.Tabs++;
            if (!(Example2Char != null))
            {
                startOfObjectPosition = writer.Position;
                if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren && LazinatorObjectVersion >= 4) 
                {
                    WriteChild(writer, _Example2, includeChildrenMode, _Example2_Accessed, () => GetChildSlice(LazinatorObjectBytes, _Example2_ByteIndex, _Example2_ByteLength, false, false, null), verifyCleanness, updateStoredBuffer, false, false, this);
                }
            }
            if (updateStoredBuffer)
            {
                _Example2_ByteIndex = startOfObjectPosition - startPosition;
            }
            TabbedText.Tabs--;
            TabbedText.WriteLine($"Byte {writer.Position}, Example3 (accessed? {_Example3_Accessed}) (backing var null? {_Example3 == null}) ");
            TabbedText.Tabs++;
            if (!(Example3IsNull))
            {
                startOfObjectPosition = writer.Position;
                if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
                {
                    WriteChild(writer, _Example3, includeChildrenMode, _Example3_Accessed, () => GetChildSlice(LazinatorObjectBytes, _Example3_ByteIndex, _Example3_ByteLength, false, false, null), verifyCleanness, updateStoredBuffer, false, false, this);
                }
            }
            if (updateStoredBuffer)
            {
                _Example3_ByteIndex = startOfObjectPosition - startPosition;
            }
            TabbedText.Tabs--;
            if (updateStoredBuffer)
            {
                _Simplifiable_EndByteIndex = writer.Position - startPosition;
            }
            TabbedText.WriteLine($"Byte {writer.Position} (end of Simplifiable) ");
        }
        
    }
}
