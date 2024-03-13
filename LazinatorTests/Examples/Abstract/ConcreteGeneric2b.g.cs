//773afe35-d6ad-249b-2e0b-fbab63e1b181
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.426, on 2024/03/13 03:12:22.047 PM.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

#nullable disable
namespace LazinatorTests.Examples.Abstract
{
    #pragma warning disable 8019
    using Lazinator.Attributes;
    using Lazinator.Buffers;
    using Lazinator.Core;
    using Lazinator.Exceptions;
    using Lazinator.Support;
    using LazinatorTests.Examples;
    using static Lazinator.Buffers.WriteUncompressedPrimitives;
    using System;
    using System.Buffers;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;
    using static Lazinator.Core.LazinatorUtilities;
    #pragma warning restore 8019
    
    [Autogenerated]
    public partial class ConcreteGeneric2b : ILazinator
    {
        /* Property definitions */
        
        protected int _LazinatorExample_ByteIndex;
        protected override int _MyT_ByteLength => _LazinatorExample_ByteIndex - _MyT_ByteIndex;
        private int _ConcreteGeneric2b_EndByteIndex;
        protected virtual  int _LazinatorExample_ByteLength => _ConcreteGeneric2b_EndByteIndex - _LazinatorExample_ByteIndex;
        protected virtual int _OverallEndByteIndex => _ConcreteGeneric2b_EndByteIndex;
        
        
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
        
        protected global::LazinatorTests.Examples.Abstract.AbstractGeneric1<global::System.Int32>.EnumWithinAbstractGeneric _MyEnumWithinAbstractGeneric2;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public override global::LazinatorTests.Examples.Abstract.AbstractGeneric1<global::System.Int32>.EnumWithinAbstractGeneric MyEnumWithinAbstractGeneric2
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
                    LazinateMyT();
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
        private void LazinateMyT()
        {
            if (LazinatorMemoryStorage.Length == 0)
            {
                _MyT = null;
            }
            else
            {
                LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _MyT_ByteIndex, _MyT_ByteLength, null);
                _MyT = DeserializationFactory.Instance.CreateBaseOrDerivedType(1012, (c, p) => new Example(c, p), childData, this); 
            }
            _MyT_Accessed = true;
        }
        
        
        protected Example _LazinatorExample;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public Example LazinatorExample
        {
            get
            {
                if (!_LazinatorExample_Accessed)
                {
                    LazinateLazinatorExample();
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
        private void LazinateLazinatorExample()
        {
            if (LazinatorMemoryStorage.Length == 0)
            {
                _LazinatorExample = null;
            }
            else
            {
                LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _LazinatorExample_ByteIndex, _LazinatorExample_ByteLength, null);
                _LazinatorExample = DeserializationFactory.Instance.CreateBaseOrDerivedType(1012, (c, p) => new Example(c, p), childData, this); 
            }
            _LazinatorExample_Accessed = true;
        }
        
        
        /* Serialization, deserialization, and object relationships */
        
        public ConcreteGeneric2b(IncludeChildrenMode originalIncludeChildrenMode = IncludeChildrenMode.IncludeAllChildren)
        {
            OriginalIncludeChildrenMode = originalIncludeChildrenMode;
        }
        
        public ConcreteGeneric2b(LazinatorMemory serializedBytes, ILazinator parent = null, IncludeChildrenMode originalIncludeChildrenMode = IncludeChildrenMode.IncludeAllChildren, int? lazinatorObjectVersion = null)
        {
            if (lazinatorObjectVersion != null)
            {
                LazinatorObjectVersion = (int) lazinatorObjectVersion;
            }
            OriginalIncludeChildrenMode = originalIncludeChildrenMode;
            LazinatorParents = new LazinatorParentsCollection(parent, null);
            DeserializeLazinator(serializedBytes);
            HasChanged = false;
            DescendantHasChanged = false;
        }
        
        public override LazinatorParentsCollection LazinatorParents { get; set; }
        
        public override LazinatorMemory LazinatorMemoryStorage
        {
            get;
            set;
        }
        
        public override IncludeChildrenMode OriginalIncludeChildrenMode { get; set; }
        
        public override bool HasChanged { get; set; }
        
        protected bool _IsDirty;
        public override bool IsDirty
        {
            [DebuggerStepThrough]
            get => _IsDirty|| LazinatorMemoryStorage.Length == 0;
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
        
        public override bool NonBinaryHash32 => false;
        
        protected override void DeserializeLazinator(LazinatorMemory serializedBytes)
        {
            LazinatorMemoryStorage = serializedBytes;
            int length = Deserialize();
            if (length != LazinatorMemoryStorage.Length)
            {
                LazinatorMemoryStorage = LazinatorMemoryStorage.Slice(0, length);
            }
        }
        
        protected override int Deserialize()
        {
            FreeInMemoryObjects();
            int bytesSoFar = 0;
            ReadOnlySpan<byte> span = LazinatorMemoryStorage.InitialReadOnlyMemory.Span;
            if (span.Length == 0)
            {
                return 0;
            }
            
            ReadGenericIDIfApplicable(ContainsOpenGenericParameters, LazinatorUniqueID, span, ref bytesSoFar);
            
            int lazinatorLibraryVersion = span.ToDecompressedInt32(ref bytesSoFar);
            
            int serializedVersionNumber = span.ToDecompressedInt32(ref bytesSoFar);
            
            OriginalIncludeChildrenMode = (IncludeChildrenMode)span.ToByte(ref bytesSoFar);
            
            int totalBytes = ConvertFromBytesAfterHeader(OriginalIncludeChildrenMode, serializedVersionNumber, ref bytesSoFar);
            return _OverallEndByteIndex;
        }
        
        public override void SerializeLazinator()
        {
            if (!IsDirty && !DescendantIsDirty && LazinatorMemoryStorage.Length > 0 && OriginalIncludeChildrenMode == IncludeChildrenMode.IncludeAllChildren)
            {
                return;
                
            }
            var previousBuffer = LazinatorMemoryStorage;
            if (LazinatorMemoryStorage.IsEmpty || IncludeChildrenMode.IncludeAllChildren != OriginalIncludeChildrenMode || (IsDirty || DescendantIsDirty))
            {
                LazinatorMemoryStorage = EncodeToNewBuffer(LazinatorSerializationOptions.Default);
            }
            else
            {
                BufferWriter writer = new BufferWriter(LazinatorMemoryStorage.LengthInt ?? 0);
                LazinatorMemoryStorage.WriteToBuffer(ref writer);
                LazinatorMemoryStorage = writer.LazinatorMemory;
            }
            OriginalIncludeChildrenMode = IncludeChildrenMode.IncludeAllChildren;
            if (!LazinatorParents.Any())
            {
                previousBuffer.Dispose();
            }
        }
        
        public override LazinatorMemory SerializeLazinator(in LazinatorSerializationOptions options) 
        {
            if (LazinatorMemoryStorage.IsEmpty || options.IncludeChildrenMode != OriginalIncludeChildrenMode || (options.SerializeDiffs || options.VerifyCleanness || IsDirty || (options.IncludeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && DescendantIsDirty)))
            {
                return EncodeToNewBuffer(options);
            }
            BufferWriter writer = new BufferWriter(LazinatorMemoryStorage.LengthInt ?? 0);
            LazinatorMemoryStorage.WriteToBuffer(ref writer);
            return writer.LazinatorMemory;
        }
        
        protected override LazinatorMemory EncodeToNewBuffer(in LazinatorSerializationOptions options) 
        {
            int bufferSize = LazinatorMemoryStorage.Length == 0 ? ExpandableBytes.DefaultMinBufferSize : LazinatorMemoryStorage.LengthInt ?? ExpandableBytes.DefaultMinBufferSize;
            BufferWriter writer = new BufferWriter(bufferSize);
            SerializeToExistingBuffer(ref writer, options);
            return writer.LazinatorMemory;
        }
        
        public override ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode = IncludeChildrenMode.IncludeAllChildren, CloneBufferOptions cloneBufferOptions = CloneBufferOptions.IndependentBuffers)
        {
            ConcreteGeneric2b clone;
            if (cloneBufferOptions == CloneBufferOptions.NoBuffer)
            {
                clone = new ConcreteGeneric2b(includeChildrenMode);
                clone.LazinatorObjectVersion = LazinatorObjectVersion;
                clone = (ConcreteGeneric2b)AssignCloneProperties(clone, includeChildrenMode);
            }
            else
            {
                LazinatorMemory bytes = EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, IsDirty, DescendantIsDirty, false, LazinatorMemoryStorage, this);
                clone = new ConcreteGeneric2b(bytes);
            }
            return clone;
        }
        
        protected override ILazinator AssignCloneProperties(ILazinator clone, IncludeChildrenMode includeChildrenMode)
        {
            clone.FreeInMemoryObjects();
            ConcreteGeneric2b typedClone = (ConcreteGeneric2b) clone;
            typedClone.MyEnumWithinAbstractGeneric = MyEnumWithinAbstractGeneric;
            typedClone.MyEnumWithinAbstractGeneric2 = MyEnumWithinAbstractGeneric2;
            typedClone.MyUnofficialInt = MyUnofficialInt;
            typedClone.AnotherProperty = AnotherProperty;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                if (MyT == null)
                {
                    typedClone.MyT = null;
                }
                else
                {
                    typedClone.MyT = (Example) MyT.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer);
                }
            }
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                if (LazinatorExample == null)
                {
                    typedClone.LazinatorExample = null;
                }
                else
                {
                    typedClone.LazinatorExample = (Example) LazinatorExample.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer);
                }
            }
            
            return typedClone;
        }
        
        
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
            if (enumerateNulls && (!exploreOnlyDeserializedChildren || _MyT_Accessed) && MyT == null)
            {
                yield return ("MyT", default);
            }
            else
            {
                if ((!exploreOnlyDeserializedChildren && MyT != null) || (_MyT_Accessed && _MyT != null))
                {
                    bool isMatch_MyT = matchCriterion == null || matchCriterion(MyT);
                    bool shouldExplore_MyT = exploreCriterion == null || exploreCriterion(MyT);
                    if (isMatch_MyT)
                    {
                        yield return ("MyT", MyT);
                    }
                    if ((!stopExploringBelowMatch || !isMatch_MyT) && shouldExplore_MyT)
                    {
                        foreach (var toYield in MyT.EnumerateLazinatorDescendants(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
                        {
                            yield return ("MyT" + "." + toYield.propertyName, toYield.descendant);
                        }
                    }
                }
            }
            if (enumerateNulls && (!exploreOnlyDeserializedChildren || _LazinatorExample_Accessed) && LazinatorExample == null)
            {
                yield return ("LazinatorExample", default);
            }
            else
            {
                if ((!exploreOnlyDeserializedChildren && LazinatorExample != null) || (_LazinatorExample_Accessed && _LazinatorExample != null))
                {
                    bool isMatch_LazinatorExample = matchCriterion == null || matchCriterion(LazinatorExample);
                    bool shouldExplore_LazinatorExample = exploreCriterion == null || exploreCriterion(LazinatorExample);
                    if (isMatch_LazinatorExample)
                    {
                        yield return ("LazinatorExample", LazinatorExample);
                    }
                    if ((!stopExploringBelowMatch || !isMatch_LazinatorExample) && shouldExplore_LazinatorExample)
                    {
                        foreach (var toYield in LazinatorExample.EnumerateLazinatorDescendants(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
                        {
                            yield return ("LazinatorExample" + "." + toYield.propertyName, toYield.descendant);
                        }
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
        
        public override ILazinator ForEachLazinator(Func<ILazinator, ILazinator> changeFunc, bool exploreOnlyDeserializedChildren, bool changeThisLevel)
        {
            if ((!exploreOnlyDeserializedChildren && MyT != null) || (_MyT_Accessed && _MyT != null))
            {
                _MyT = (Example) _MyT.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, true);
            }
            if ((!exploreOnlyDeserializedChildren && LazinatorExample != null) || (_LazinatorExample_Accessed && _LazinatorExample != null))
            {
                _LazinatorExample = (Example) _LazinatorExample.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, true);
            }
            if (changeThisLevel && changeFunc != null)
            {
                return changeFunc(this);
            }
            return this;
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
        
        public override int LazinatorUniqueID => 1042;
        
        protected override bool ContainsOpenGenericParameters => false;
        public override LazinatorGenericIDType LazinatorGenericID => default;
        
        
        public override int LazinatorObjectVersion { get; set; } = 0;
        
        
        protected override int ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            ReadOnlySpan<byte> span = LazinatorMemoryStorage.InitialReadOnlyMemory.Span;
            ConvertFromBytesForPrimitiveProperties(span, includeChildrenMode, serializedVersionNumber, ref bytesSoFar);
            int lengthForLengths = 0;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                lengthForLengths += 8;
            }
            int totalChildrenSize = ConvertFromBytesForChildLengths(span, includeChildrenMode, serializedVersionNumber, bytesSoFar + lengthForLengths, ref bytesSoFar);
            return bytesSoFar + totalChildrenSize;
        }
        
        protected override void ConvertFromBytesForPrimitiveProperties(ReadOnlySpan<byte> span, IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            _MyEnumWithinAbstractGeneric = (global::LazinatorTests.Examples.Abstract.AbstractGeneric1<global::LazinatorTests.Examples.Example>.EnumWithinAbstractGeneric)span.ToDecompressedInt32(ref bytesSoFar);
            _MyEnumWithinAbstractGeneric2 = (global::LazinatorTests.Examples.Abstract.AbstractGeneric1<global::System.Int32>.EnumWithinAbstractGeneric)span.ToDecompressedInt32(ref bytesSoFar);
            _MyUnofficialInt = span.ToDecompressedInt32(ref bytesSoFar);
            _AnotherProperty = span.ToString_BrotliCompressedWithLength(ref bytesSoFar);
        }
        
        protected override int ConvertFromBytesForChildLengths(ReadOnlySpan<byte> span, IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, int indexOfFirstChild, ref int bytesSoFar)
        {
            int totalChildrenBytes = 0;
            _MyT_ByteIndex = indexOfFirstChild + totalChildrenBytes;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                totalChildrenBytes += span.ToInt32(ref bytesSoFar);
            }
            _LazinatorExample_ByteIndex = indexOfFirstChild + totalChildrenBytes;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                totalChildrenBytes += span.ToInt32(ref bytesSoFar);
            }
            _ConcreteGeneric2b_EndByteIndex = indexOfFirstChild + totalChildrenBytes;
            return totalChildrenBytes;
        }
        
        public override void SerializeToExistingBuffer(ref BufferWriter writer, in LazinatorSerializationOptions options)
        {
            int startPosition = writer.ActiveMemoryPosition;
            WritePropertiesIntoBuffer(ref writer, options, true);
            if (options.UpdateStoredBuffer)
            {
                UpdateStoredBuffer(ref writer, startPosition, writer.ActiveMemoryPosition - startPosition, options.IncludeChildrenMode, false);
            }
        }
        
        public override void UpdateStoredBuffer(ref BufferWriter writer, long startPosition, long length, IncludeChildrenMode includeChildrenMode, bool updateDeserializedChildren)
        {
            _IsDirty = false;
            if (includeChildrenMode == IncludeChildrenMode.IncludeAllChildren)
            {
                _DescendantIsDirty = false;
                if (updateDeserializedChildren)
                {
                    UpdateDeserializedChildren(ref writer, startPosition);
                }
                
            }
            else
            {
                ThrowHelper.ThrowCannotUpdateStoredBuffer();
            }
            
            var newBuffer = writer.Slice(startPosition, length);
            LazinatorMemoryStorage = newBuffer;
        }
        
        protected override void UpdateDeserializedChildren(ref BufferWriter writer, long startPosition)
        {
            if (_MyT_Accessed && _MyT != null)
            {
                MyT.UpdateStoredBuffer(ref writer, startPosition + _MyT_ByteIndex, _MyT_ByteLength, IncludeChildrenMode.IncludeAllChildren, true);
            }
            if (_LazinatorExample_Accessed && _LazinatorExample != null)
            {
                LazinatorExample.UpdateStoredBuffer(ref writer, startPosition + _LazinatorExample_ByteIndex, _LazinatorExample_ByteLength, IncludeChildrenMode.IncludeAllChildren, true);
            }
            
        }
        
        
        protected override void WritePropertiesIntoBuffer(ref BufferWriter writer, in LazinatorSerializationOptions options, bool includeUniqueID)
        {
            int startPosition = writer.ActiveMemoryPosition;
            if (includeUniqueID)
            {
                if (!ContainsOpenGenericParameters)
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
            writer.Write((byte)options.IncludeChildrenMode);
            // write properties
            
            WritePrimitivePropertiesIntoBuffer(ref writer, options, includeUniqueID);
            int lengthForLengths = 0;
            if (options.IncludeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && options.IncludeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                lengthForLengths += 8;
            }
            
            var previousLengthsPosition = writer.SetLengthsPosition(lengthForLengths);
            WriteChildrenPropertiesIntoBuffer(ref writer, options, includeUniqueID, startPosition);
            writer.ResetLengthsPosition(previousLengthsPosition);
            
        }
        
        protected override void WritePrimitivePropertiesIntoBuffer(ref BufferWriter writer, in LazinatorSerializationOptions options, bool includeUniqueID)
        {
            CompressedIntegralTypes.WriteCompressedInt(ref writer, (int) _MyEnumWithinAbstractGeneric);
            CompressedIntegralTypes.WriteCompressedInt(ref writer, (int) _MyEnumWithinAbstractGeneric2);
            CompressedIntegralTypes.WriteCompressedInt(ref writer, _MyUnofficialInt);
            EncodeCharAndString.WriteBrotliCompressedWithIntPrefix(ref writer, _AnotherProperty);
        }
        protected override void WriteChildrenPropertiesIntoBuffer(ref BufferWriter writer, LazinatorSerializationOptions options, bool includeUniqueID, int startOfObjectPosition)
        {
            if (options.SplittingPossible)
            {
                options = options.WithoutSplittingPossible();
            }
            int startOfChildPosition = 0;
            int lengthValue = 0;
            startOfChildPosition = writer.ActiveMemoryPosition;
            if (options.IncludeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && options.IncludeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                if ((options.IncludeChildrenMode != IncludeChildrenMode.IncludeAllChildren || options.IncludeChildrenMode != OriginalIncludeChildrenMode) && !_MyT_Accessed)
                {
                    var deserialized = MyT;
                }
                WriteChild(ref writer, ref _MyT, options, _MyT_Accessed, () => GetChildSlice(LazinatorMemoryStorage, _MyT_ByteIndex, _MyT_ByteLength, null), this);
                lengthValue = writer.ActiveMemoryPosition - startOfChildPosition;
                if (lengthValue > int.MaxValue)
                {
                    ThrowHelper.ThrowTooLargeException(int.MaxValue);
                }
                writer.RecordLength((int) lengthValue);
            }
            if (options.UpdateStoredBuffer)
            {
                _MyT_ByteIndex = startOfChildPosition - startOfObjectPosition;
                
            }
            startOfChildPosition = writer.ActiveMemoryPosition;
            if (options.IncludeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && options.IncludeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                if ((options.IncludeChildrenMode != IncludeChildrenMode.IncludeAllChildren || options.IncludeChildrenMode != OriginalIncludeChildrenMode) && !_LazinatorExample_Accessed)
                {
                    var deserialized = LazinatorExample;
                }
                WriteChild(ref writer, ref _LazinatorExample, options, _LazinatorExample_Accessed, () => GetChildSlice(LazinatorMemoryStorage, _LazinatorExample_ByteIndex, _LazinatorExample_ByteLength, null), this);
                lengthValue = writer.ActiveMemoryPosition - startOfChildPosition;
                if (lengthValue > int.MaxValue)
                {
                    ThrowHelper.ThrowTooLargeException(int.MaxValue);
                }
                writer.RecordLength((int) lengthValue);
            }
            if (options.UpdateStoredBuffer)
            {
                _LazinatorExample_ByteIndex = startOfChildPosition - startOfObjectPosition;
                
            }
            if (options.UpdateStoredBuffer)
            {
                _ConcreteGeneric2b_EndByteIndex = writer.ActiveMemoryPosition - startOfObjectPosition;
            }
            
        }
    }
}
#nullable restore
