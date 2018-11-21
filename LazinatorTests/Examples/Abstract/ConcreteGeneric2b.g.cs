//f3e4b308-edfc-5471-57e3-2c32d0cdf6e0
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.320
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
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;
    using static Lazinator.Core.LazinatorUtilities;
    
    [Autogenerated]
    public partial class ConcreteGeneric2b : ILazinator
    {
        /* Property definitions */
        
        protected int _LazinatorExample_ByteIndex;
        protected override int _MyT_ByteLength => _LazinatorExample_ByteIndex - _MyT_ByteIndex;
        private int _ConcreteGeneric2b_EndByteIndex;
        protected virtual int _LazinatorExample_ByteLength => _ConcreteGeneric2b_EndByteIndex - _LazinatorExample_ByteIndex;
        
        
        protected global::LazinatorTests.Examples.Abstract.AbstractGeneric1<global::LazinatorTests.Examples.Example>.EnumWithinAbstractGeneric _MyEnumWithinAbstractGeneric;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public override global::LazinatorTests.Examples.Abstract.AbstractGeneric1<global::LazinatorTests.Examples.Example>.EnumWithinAbstractGeneric MyEnumWithinAbstractGeneric
        {
            get
            {
                return _MyEnumWithinAbstractGeneric;
            }
            set
            {
                IsDirty = true;
                _MyEnumWithinAbstractGeneric = value;
            }
        }
        
        protected global::LazinatorTests.Examples.Abstract.AbstractGeneric1<int>.EnumWithinAbstractGeneric _MyEnumWithinAbstractGeneric2;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public override global::LazinatorTests.Examples.Abstract.AbstractGeneric1<int>.EnumWithinAbstractGeneric MyEnumWithinAbstractGeneric2
        {
            get
            {
                return _MyEnumWithinAbstractGeneric2;
            }
            set
            {
                IsDirty = true;
                _MyEnumWithinAbstractGeneric2 = value;
            }
        }
        
        protected int _MyUnofficialInt;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public override int MyUnofficialInt
        {
            get
            {
                return _MyUnofficialInt;
            }
            set
            {
                IsDirty = true;
                _MyUnofficialInt = value;
            }
        }
        
        protected string _AnotherProperty;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public string AnotherProperty
        {
            get
            {
                return _AnotherProperty;
            }
            set
            {
                IsDirty = true;
                _AnotherProperty = value;
            }
        }
        
        protected Example _MyT;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public override Example MyT
        {
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
                        LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _MyT_ByteIndex, _MyT_ByteLength, false, false, null);
                        
                        _MyT = DeserializationFactory.Instance.CreateBaseOrDerivedType(212, () => new Example(), childData, this); 
                    }
                    _MyT_Accessed = true;
                } 
                return _MyT;
            }
            set
            {
                if (_MyT != null)
                {
                    _MyT.LazinatorParents = _MyT.LazinatorParents.WithRemoved(this);
                }
                if (value != null)
                {
                    value.LazinatorParents = value.LazinatorParents.WithAdded(this);
                }
                
                IsDirty = true;
                DescendantIsDirty = true;
                _MyT = value;
                _MyT_Accessed = true;
            }
        }
        
        protected Example _LazinatorExample;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public Example LazinatorExample
        {
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
                        LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _LazinatorExample_ByteIndex, _LazinatorExample_ByteLength, false, false, null);
                        
                        _LazinatorExample = DeserializationFactory.Instance.CreateBaseOrDerivedType(212, () => new Example(), childData, this); 
                    }
                    _LazinatorExample_Accessed = true;
                } 
                return _LazinatorExample;
            }
            set
            {
                if (_LazinatorExample != null)
                {
                    _LazinatorExample.LazinatorParents = _LazinatorExample.LazinatorParents.WithRemoved(this);
                }
                if (value != null)
                {
                    value.LazinatorParents = value.LazinatorParents.WithAdded(this);
                }
                
                IsDirty = true;
                DescendantIsDirty = true;
                _LazinatorExample = value;
                _LazinatorExample_Accessed = true;
            }
        }
        protected bool _LazinatorExample_Accessed;
        
        /* Serialization, deserialization, and object relationships */
        
        public ConcreteGeneric2b() : base()
        {
        }
        
        public override LazinatorParentsCollection LazinatorParents { get; set; }
        
        public override IncludeChildrenMode OriginalIncludeChildrenMode { get; set; }
        
        public override int Deserialize()
        {
            FreeInMemoryObjects();
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
        
        public override LazinatorMemory SerializeLazinator(IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer) => EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, verifyCleanness, IsDirty, DescendantIsDirty, false, LazinatorMemoryStorage, updateStoredBuffer, this);
        
        public override ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode = IncludeChildrenMode.IncludeAllChildren, CloneBufferOptions cloneBufferOptions = CloneBufferOptions.IndependentBuffers)
        {
            var clone = new ConcreteGeneric2b()
            {
                OriginalIncludeChildrenMode = includeChildrenMode
            };
            clone = CompleteClone(this, clone, includeChildrenMode, cloneBufferOptions);
            return clone;
        }
        
        public override ILazinator AssignCloneProperties(ILazinator clone, IncludeChildrenMode includeChildrenMode)
        {
            clone.FreeInMemoryObjects();
            ConcreteGeneric2b typedClone = (ConcreteGeneric2b) clone;
            typedClone.MyEnumWithinAbstractGeneric = MyEnumWithinAbstractGeneric;
            typedClone.MyEnumWithinAbstractGeneric2 = MyEnumWithinAbstractGeneric2;
            typedClone.MyUnofficialInt = MyUnofficialInt;
            typedClone.AnotherProperty = AnotherProperty;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                typedClone.MyT = (MyT == null) ? default(Example) : (Example) MyT.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer);
            }
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                typedClone.LazinatorExample = (LazinatorExample == null) ? default(Example) : (Example) LazinatorExample.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer);
            }
            
            return typedClone;
        }
        
        public override bool HasChanged { get; set; }
        
        protected bool _IsDirty;
        public override bool IsDirty
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
        
        protected bool _DescendantHasChanged;
        public override bool DescendantHasChanged
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
                    if (_DescendantIsDirty)
                    {
                        LazinatorParents.InformParentsOfDirtiness();
                        _DescendantHasChanged = true;
                    }
                }
            }
        }
        
        public override void DeserializeLazinator(LazinatorMemory serializedBytes)
        {
            LazinatorMemoryStorage = serializedBytes;
            int length = Deserialize();
            if (length != LazinatorMemoryStorage.Length)
            {
                LazinatorMemoryStorage = LazinatorMemoryStorage.Slice(0, length);
            }
        }
        
        public override LazinatorMemory LazinatorMemoryStorage
        {
            get;
            set;
        }
        protected override ReadOnlyMemory<byte> LazinatorObjectBytes => LazinatorMemoryStorage?.Memory ?? LazinatorUtilities.EmptyReadOnlyMemory;
        
        public override void UpdateStoredBuffer(bool disposePreviousBuffer = false)
        {
            if (!IsDirty && !DescendantIsDirty && LazinatorObjectBytes.Length > 0 && OriginalIncludeChildrenMode == IncludeChildrenMode.IncludeAllChildren)
            {
                return;
            }
            var previousBuffer = LazinatorMemoryStorage;
            EncodeOrRecycleToNewBuffer(IncludeChildrenMode.IncludeAllChildren, OriginalIncludeChildrenMode, false, IsDirty, DescendantIsDirty, false, previousBuffer, true, this);
            OriginalIncludeChildrenMode = IncludeChildrenMode.IncludeAllChildren;
            if (disposePreviousBuffer)
            {
                previousBuffer.Dispose();
            }
        }
        
        public override int GetByteLength()
        {
            UpdateStoredBuffer();
            return LazinatorObjectBytes.Length;
        }
        
        public override bool NonBinaryHash32 => false;
        
        
        public override IEnumerable<ILazinator> EnumerateLazinatorNodes(Func<ILazinator, bool> matchCriterion, bool stopExploringBelowMatch, Func<ILazinator, bool> exploreCriterion, bool exploreOnlyDeserializedChildren, bool enumerateNulls)
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
        
        public override IEnumerable<(string propertyName, ILazinator descendant)> EnumerateLazinatorDescendants(Func<ILazinator, bool> matchCriterion, bool stopExploringBelowMatch, Func<ILazinator, bool> exploreCriterion, bool exploreOnlyDeserializedChildren, bool enumerateNulls)
        {
            if (enumerateNulls && (!exploreOnlyDeserializedChildren || _MyT_Accessed) && (MyT == null))
            {
                yield return ("MyT", default);
            }
            else if ((!exploreOnlyDeserializedChildren && MyT != null) || (_MyT_Accessed && _MyT != null))
            {
                bool isMatch = matchCriterion == null || matchCriterion(MyT);
                bool shouldExplore = exploreCriterion == null || exploreCriterion(MyT);
                if (isMatch)
                {
                    yield return ("MyT", MyT);
                }
                if ((!stopExploringBelowMatch || !isMatch) && shouldExplore)
                {
                    foreach (var toYield in MyT.EnumerateLazinatorDescendants(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
                    {
                        yield return ("MyT" + "." + toYield.propertyName, toYield.descendant);
                    }
                }
            }
            if (enumerateNulls && (!exploreOnlyDeserializedChildren || _LazinatorExample_Accessed) && (LazinatorExample == null))
            {
                yield return ("LazinatorExample", default);
            }
            else if ((!exploreOnlyDeserializedChildren && LazinatorExample != null) || (_LazinatorExample_Accessed && _LazinatorExample != null))
            {
                bool isMatch = matchCriterion == null || matchCriterion(LazinatorExample);
                bool shouldExplore = exploreCriterion == null || exploreCriterion(LazinatorExample);
                if (isMatch)
                {
                    yield return ("LazinatorExample", LazinatorExample);
                }
                if ((!stopExploringBelowMatch || !isMatch) && shouldExplore)
                {
                    foreach (var toYield in LazinatorExample.EnumerateLazinatorDescendants(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
                    {
                        yield return ("LazinatorExample" + "." + toYield.propertyName, toYield.descendant);
                    }
                }
            }
            yield break;
        }
        
        
        public override IEnumerable<(string propertyName, object descendant)> EnumerateNonLazinatorProperties()
        {
            yield return ("MyEnumWithinAbstractGeneric", (object)MyEnumWithinAbstractGeneric);
            yield return ("MyEnumWithinAbstractGeneric2", (object)MyEnumWithinAbstractGeneric2);
            yield return ("MyUnofficialInt", (object)MyUnofficialInt);
            yield return ("AnotherProperty", (object)AnotherProperty);
            yield break;
        }
        
        public override ILazinator ForEachLazinator(Func<ILazinator, ILazinator> changeFunc, bool exploreOnlyDeserializedChildren)
        {
            if ((!exploreOnlyDeserializedChildren && MyT != null) || (_MyT_Accessed && _MyT != null))
            {
                _MyT = (Example) _MyT.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren);
            }
            if ((!exploreOnlyDeserializedChildren && LazinatorExample != null) || (_LazinatorExample_Accessed && _LazinatorExample != null))
            {
                _LazinatorExample = (Example) _LazinatorExample.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren);
            }
            return changeFunc(this);
        }
        
        public override void FreeInMemoryObjects()
        {
            _MyT = default;
            _LazinatorExample = default;
            _MyT_Accessed = _LazinatorExample_Accessed = false;
            IsDirty = false;
            DescendantIsDirty = false;
            HasChanged = false;
            DescendantHasChanged = false;
        }
        
        /* Conversion */
        
        public override int LazinatorUniqueID => 242;
        
        protected override bool ContainsOpenGenericParameters => false;
        protected override LazinatorGenericIDType _LazinatorGenericID { get; set; }
        public override LazinatorGenericIDType LazinatorGenericID
        {
            get => default;
            set { }
        }
        
        public override int LazinatorObjectVersion { get; set; } = 0;
        
        
        public override void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
            _MyEnumWithinAbstractGeneric = (global::LazinatorTests.Examples.Abstract.AbstractGeneric1<global::LazinatorTests.Examples.Example>.EnumWithinAbstractGeneric)span.ToDecompressedInt(ref bytesSoFar);
            _MyEnumWithinAbstractGeneric2 = (global::LazinatorTests.Examples.Abstract.AbstractGeneric1<int>.EnumWithinAbstractGeneric)span.ToDecompressedInt(ref bytesSoFar);
            _MyUnofficialInt = span.ToDecompressedInt(ref bytesSoFar);
            _AnotherProperty = span.ToString_VarIntLengthUtf8(ref bytesSoFar);
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
        
        public override void SerializeExistingBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
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
        
        public override void UpdateStoredBuffer(ref BinaryBufferWriter writer, int startPosition, int length, IncludeChildrenMode includeChildrenMode, bool updateDeserializedChildren)
        {
            _IsDirty = false;
            if (includeChildrenMode == IncludeChildrenMode.IncludeAllChildren)
            {
                _DescendantIsDirty = false;
                if (updateDeserializedChildren)
                {
                    if (_MyT_Accessed && _MyT != null)
                    {
                        _MyT.UpdateStoredBuffer(ref writer, startPosition + _MyT_ByteIndex + sizeof(int), _MyT_ByteLength - sizeof(int), IncludeChildrenMode.IncludeAllChildren, true);
                    }
                    if (_LazinatorExample_Accessed && _LazinatorExample != null)
                    {
                        _LazinatorExample.UpdateStoredBuffer(ref writer, startPosition + _LazinatorExample_ByteIndex + sizeof(int), _LazinatorExample_ByteLength - sizeof(int), IncludeChildrenMode.IncludeAllChildren, true);
                    }
                }
                
            }
            else
            {
                throw new LazinatorSerializationException("Cannot update stored buffer when serializing only some children.");
            }
            
            var newBuffer = writer.Slice(startPosition, length);
            LazinatorMemoryStorage = newBuffer;
        }
        
        protected override void WritePropertiesIntoBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer, bool includeUniqueID)
        {
            int startPosition = writer.Position;
            int startOfObjectPosition = 0;
            // header information
            if (includeUniqueID)
            {
                if (LazinatorGenericID.IsEmpty)
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
            CompressedIntegralTypes.WriteCompressedInt(ref writer, (int) _MyEnumWithinAbstractGeneric);
            CompressedIntegralTypes.WriteCompressedInt(ref writer, (int) _MyEnumWithinAbstractGeneric2);
            CompressedIntegralTypes.WriteCompressedInt(ref writer, _MyUnofficialInt);
            EncodeCharAndString.WriteStringUtf8WithVarIntPrefix(ref writer, _AnotherProperty);
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode && !_MyT_Accessed)
                {
                    var deserialized = MyT;
                }
                WriteChild(ref writer, ref _MyT, includeChildrenMode, _MyT_Accessed, () => GetChildSlice(LazinatorMemoryStorage, _MyT_ByteIndex, _MyT_ByteLength, false, false, null), verifyCleanness, updateStoredBuffer, false, false, this);
            }
            if (updateStoredBuffer)
            {
                _MyT_ByteIndex = startOfObjectPosition - startPosition;
            }
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode && !_LazinatorExample_Accessed)
                {
                    var deserialized = LazinatorExample;
                }
                WriteChild(ref writer, ref _LazinatorExample, includeChildrenMode, _LazinatorExample_Accessed, () => GetChildSlice(LazinatorMemoryStorage, _LazinatorExample_ByteIndex, _LazinatorExample_ByteLength, false, false, null), verifyCleanness, updateStoredBuffer, false, false, this);
            }
            if (updateStoredBuffer)
            {
                _LazinatorExample_ByteIndex = startOfObjectPosition - startPosition;
            }
            if (updateStoredBuffer)
            {
                _ConcreteGeneric2b_EndByteIndex = writer.Position - startPosition;
            }
        }
        
    }
}
