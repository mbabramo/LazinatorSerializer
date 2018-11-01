//ed3d094d-abd3-e973-e16e-4af2d6892324
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.274
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
        public bool IsStruct => false;
        
        /* Serialization, deserialization, and object relationships */
        
        public Simplifiable() : base()
        {
        }
        
        public virtual LazinatorParentsCollection LazinatorParents { get; set; }
        
        public virtual IncludeChildrenMode OriginalIncludeChildrenMode { get; set; }
        
        public virtual int Deserialize()
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
        
        public virtual LazinatorMemory SerializeLazinator(IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            return EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, verifyCleanness, IsDirty, DescendantIsDirty, false, LazinatorMemoryStorage, (EncodeManuallyDelegate) EncodeToNewBuffer, updateStoredBuffer);
        }
        
        protected virtual LazinatorMemory EncodeToNewBuffer(IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer) => LazinatorUtilities.EncodeToNewBinaryBufferWriter(this, includeChildrenMode, verifyCleanness, updateStoredBuffer);
        
        public virtual ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode = IncludeChildrenMode.IncludeAllChildren, CloneBufferOptions cloneBufferOptions = CloneBufferOptions.LinkedBuffer)
        {
            var clone = new Simplifiable()
            {
                OriginalIncludeChildrenMode = includeChildrenMode
            };
            if (clone.LazinatorObjectVersion != LazinatorObjectVersion)
            {
                clone.LazinatorObjectVersion = LazinatorObjectVersion;
            }
            
            if (cloneBufferOptions == CloneBufferOptions.NoBuffer)
            {
                AssignCloneProperties(clone, includeChildrenMode);
            }
            else
            {
                LazinatorMemory bytes = EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, false, IsDirty, DescendantIsDirty, false, LazinatorMemoryStorage, (EncodeManuallyDelegate)EncodeToNewBuffer, cloneBufferOptions == CloneBufferOptions.SharedBuffer);
                clone.DeserializeLazinator(bytes);
                if (cloneBufferOptions == CloneBufferOptions.IndependentBuffers)
                {
                    clone.LazinatorMemoryStorage.DisposeIndependently();
                }
            }
            clone.LazinatorParents = default;
            return clone;
        }
        
        protected virtual void AssignCloneProperties(ILazinator clone, IncludeChildrenMode includeChildrenMode)
        {
            clone.FreeInMemoryObjects();
            Simplifiable typedClone = (Simplifiable) clone;
            typedClone.MyIntsAre3 = MyIntsAre3;
            typedClone.Example2Char = Example2Char;
            typedClone.Example3IsNull = Example3IsNull;
            typedClone.ExampleHasDefaultValue = ExampleHasDefaultValue;
            typedClone.MyInt = MyInt;
            typedClone.MyOtherInt = MyOtherInt;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                typedClone.ANonSkippableEarlierExample = (ANonSkippableEarlierExample == null) ? default(Example) : (Example) ANonSkippableEarlierExample.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer);
            }
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                typedClone.Example = (Example == null) ? default(Example) : (Example) Example.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer);
            }
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren && LazinatorObjectVersion >= 4) 
            {
                typedClone.Example2 = (Example2 == null) ? default(Example) : (Example) Example2.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer);
            }
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                typedClone.Example3 = (Example3 == null) ? default(Example) : (Example) Example3.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer);
            }
        }
        
        public virtual bool HasChanged { get; set; }
        
        protected bool _IsDirty;
        public virtual bool IsDirty
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
                        LazinatorParents.InformParentsOfDirtiness();
                        _DescendantHasChanged = true;
                    }
                }
            }
        }
        
        public virtual void DeserializeLazinator(LazinatorMemory serializedBytes)
        {
            LazinatorMemoryStorage = serializedBytes;
            int length = Deserialize();
            if (length != LazinatorMemoryStorage.Length)
            {
                LazinatorMemoryStorage = LazinatorMemoryStorage.Slice(0, length);
            }
        }
        
        public virtual LazinatorMemory LazinatorMemoryStorage
        {
            get;
            set;
        }
        protected virtual ReadOnlyMemory<byte> LazinatorObjectBytes => LazinatorMemoryStorage?.Memory ?? LazinatorUtilities.EmptyReadOnlyMemory;
        
        public virtual void EnsureLazinatorMemoryUpToDate()
        {
            if (!IsDirty && !DescendantIsDirty && LazinatorObjectBytes.Length > 0 && OriginalIncludeChildrenMode == IncludeChildrenMode.IncludeAllChildren)
            {
                return;
            }
            EncodeOrRecycleToNewBuffer(IncludeChildrenMode.IncludeAllChildren, OriginalIncludeChildrenMode, false, IsDirty, DescendantIsDirty, false, LazinatorMemoryStorage, (EncodeManuallyDelegate)EncodeToNewBuffer, true);
            OriginalIncludeChildrenMode = IncludeChildrenMode.IncludeAllChildren;
        }
        
        public virtual int GetByteLength()
        {
            EnsureLazinatorMemoryUpToDate();
            return LazinatorObjectBytes.Length;
        }
        
        public virtual uint GetBinaryHashCode32()
        {
            EnsureLazinatorMemoryUpToDate();
            return FarmhashByteSpans.Hash32(LazinatorObjectBytes.Span);
        }
        
        public virtual ulong GetBinaryHashCode64()
        {
            EnsureLazinatorMemoryUpToDate();
            return FarmhashByteSpans.Hash64(LazinatorObjectBytes.Span);
        }
        
        public virtual Guid GetBinaryHashCode128()
        {
            EnsureLazinatorMemoryUpToDate();
            return FarmhashByteSpans.Hash128(LazinatorObjectBytes.Span);
        }
        
        /* Property definitions */
        
        protected int _ANonSkippableEarlierExample_ByteIndex;
        protected int _Example_ByteIndex;
        protected int _Example2_ByteIndex;
        protected int _Example3_ByteIndex;
        protected virtual int _ANonSkippableEarlierExample_ByteLength => _Example_ByteIndex - _ANonSkippableEarlierExample_ByteIndex;
        protected virtual int _Example_ByteLength => _Example2_ByteIndex - _Example_ByteIndex;
        protected virtual int _Example2_ByteLength => _Example3_ByteIndex - _Example2_ByteIndex;
        private int _Simplifiable_EndByteIndex;
        protected virtual int _Example3_ByteLength => _Simplifiable_EndByteIndex - _Example3_ByteIndex;
        
        
        protected bool _MyIntsAre3;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
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
        
        protected char? _Example2Char;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
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
        
        protected bool _Example3IsNull;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
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
        
        protected bool _ExampleHasDefaultValue;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
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
        
        protected int _MyInt;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
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
        
        protected int _MyOtherInt;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
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
        
        protected Example _ANonSkippableEarlierExample;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public Example ANonSkippableEarlierExample
        {
            get
            {
                if (!_ANonSkippableEarlierExample_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _ANonSkippableEarlierExample = default(Example);
                    }
                    else
                    {
                        LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _ANonSkippableEarlierExample_ByteIndex, _ANonSkippableEarlierExample_ByteLength, false, false, null);
                        
                        _ANonSkippableEarlierExample = DeserializationFactory.Instance.CreateBaseOrDerivedType(212, () => new Example(), childData, this); 
                    }
                    _ANonSkippableEarlierExample_Accessed = true;
                } 
                return _ANonSkippableEarlierExample;
            }
            set
            {
                if (_ANonSkippableEarlierExample != null)
                {
                    _ANonSkippableEarlierExample.LazinatorParents = _ANonSkippableEarlierExample.LazinatorParents.WithRemoved(this);
                }
                if (value != null)
                {
                    value.LazinatorParents = value.LazinatorParents.WithAdded(this);
                }
                
                IsDirty = true;
                DescendantIsDirty = true;
                _ANonSkippableEarlierExample = value;
                _ANonSkippableEarlierExample_Accessed = true;
            }
        }
        protected bool _ANonSkippableEarlierExample_Accessed;
        
        protected Example _Example;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
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
                        LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _Example_ByteIndex, _Example_ByteLength, false, false, null);
                        
                        _Example = DeserializationFactory.Instance.CreateBaseOrDerivedType(212, () => new Example(), childData, this); 
                    }
                    _Example_Accessed = true;
                } 
                return _Example;
            }
            set
            {
                if (_Example != null)
                {
                    _Example.LazinatorParents = _Example.LazinatorParents.WithRemoved(this);
                }
                if (value != null)
                {
                    value.LazinatorParents = value.LazinatorParents.WithAdded(this);
                }
                
                IsDirty = true;
                DescendantIsDirty = true;
                _Example = value;
                _Example_Accessed = true;
            }
        }
        protected bool _Example_Accessed;
        
        protected Example _Example2;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
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
                        LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _Example2_ByteIndex, _Example2_ByteLength, false, false, null);
                        
                        _Example2 = DeserializationFactory.Instance.CreateBaseOrDerivedType(212, () => new Example(), childData, this); 
                    }
                    _Example2_Accessed = true;
                } 
                return _Example2;
            }
            set
            {
                if (_Example2 != null)
                {
                    _Example2.LazinatorParents = _Example2.LazinatorParents.WithRemoved(this);
                }
                if (value != null)
                {
                    value.LazinatorParents = value.LazinatorParents.WithAdded(this);
                }
                
                IsDirty = true;
                DescendantIsDirty = true;
                _Example2 = value;
                _Example2_Accessed = true;
            }
        }
        protected bool _Example2_Accessed;
        
        protected Example _Example3;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
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
                        LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _Example3_ByteIndex, _Example3_ByteLength, false, false, null);
                        
                        _Example3 = DeserializationFactory.Instance.CreateBaseOrDerivedType(212, () => new Example(), childData, this); 
                    }
                    _Example3_Accessed = true;
                } 
                return _Example3;
            }
            set
            {
                if (_Example3 != null)
                {
                    _Example3.LazinatorParents = _Example3.LazinatorParents.WithRemoved(this);
                }
                if (value != null)
                {
                    value.LazinatorParents = value.LazinatorParents.WithAdded(this);
                }
                
                IsDirty = true;
                DescendantIsDirty = true;
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
            if (enumerateNulls && (!exploreOnlyDeserializedChildren || _ANonSkippableEarlierExample_Accessed) && (ANonSkippableEarlierExample == null))
            {
                yield return ("ANonSkippableEarlierExample", default);
            }
            else if ((!exploreOnlyDeserializedChildren && ANonSkippableEarlierExample != null) || (_ANonSkippableEarlierExample_Accessed && _ANonSkippableEarlierExample != null))
            {
                bool isMatch = matchCriterion == null || matchCriterion(ANonSkippableEarlierExample);
                bool shouldExplore = exploreCriterion == null || exploreCriterion(ANonSkippableEarlierExample);
                if (isMatch)
                {
                    yield return ("ANonSkippableEarlierExample", ANonSkippableEarlierExample);
                }
                if ((!stopExploringBelowMatch || !isMatch) && shouldExplore)
                {
                    foreach (var toYield in ANonSkippableEarlierExample.EnumerateLazinatorDescendants(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
                    {
                        yield return ("ANonSkippableEarlierExample" + "." + toYield.propertyName, toYield.descendant);
                    }
                }
            }
            if (enumerateNulls && (!exploreOnlyDeserializedChildren || _Example_Accessed) && (Example == null))
            {
                yield return ("Example", default);
            }
            else if ((!exploreOnlyDeserializedChildren && Example != null) || (_Example_Accessed && _Example != null))
            {
                bool isMatch = matchCriterion == null || matchCriterion(Example);
                bool shouldExplore = exploreCriterion == null || exploreCriterion(Example);
                if (isMatch)
                {
                    yield return ("Example", Example);
                }
                if ((!stopExploringBelowMatch || !isMatch) && shouldExplore)
                {
                    foreach (var toYield in Example.EnumerateLazinatorDescendants(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
                    {
                        yield return ("Example" + "." + toYield.propertyName, toYield.descendant);
                    }
                }
            }
            if (enumerateNulls && (!exploreOnlyDeserializedChildren || _Example2_Accessed) && (Example2 == null))
            {
                yield return ("Example2", default);
            }
            else if ((!exploreOnlyDeserializedChildren && Example2 != null) || (_Example2_Accessed && _Example2 != null))
            {
                bool isMatch = matchCriterion == null || matchCriterion(Example2);
                bool shouldExplore = exploreCriterion == null || exploreCriterion(Example2);
                if (isMatch)
                {
                    yield return ("Example2", Example2);
                }
                if ((!stopExploringBelowMatch || !isMatch) && shouldExplore)
                {
                    foreach (var toYield in Example2.EnumerateLazinatorDescendants(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
                    {
                        yield return ("Example2" + "." + toYield.propertyName, toYield.descendant);
                    }
                }
            }
            if (enumerateNulls && (!exploreOnlyDeserializedChildren || _Example3_Accessed) && (Example3 == null))
            {
                yield return ("Example3", default);
            }
            else if ((!exploreOnlyDeserializedChildren && Example3 != null) || (_Example3_Accessed && _Example3 != null))
            {
                bool isMatch = matchCriterion == null || matchCriterion(Example3);
                bool shouldExplore = exploreCriterion == null || exploreCriterion(Example3);
                if (isMatch)
                {
                    yield return ("Example3", Example3);
                }
                if ((!stopExploringBelowMatch || !isMatch) && shouldExplore)
                {
                    foreach (var toYield in Example3.EnumerateLazinatorDescendants(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
                    {
                        yield return ("Example3" + "." + toYield.propertyName, toYield.descendant);
                    }
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
        
        public virtual void FreeInMemoryObjects()
        {
            _ANonSkippableEarlierExample = default;
            _Example = default;
            _Example2 = default;
            _Example3 = default;
            _ANonSkippableEarlierExample_Accessed = _Example_Accessed = _Example2_Accessed = _Example3_Accessed = false;
            IsDirty = false;
            DescendantIsDirty = false;
            HasChanged = false;
            DescendantHasChanged = false;
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
            _ANonSkippableEarlierExample_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
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
        
        public virtual void SerializeExistingBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren)
            {
                updateStoredBuffer = false;
            }
            PreSerialization(verifyCleanness, updateStoredBuffer);
            int startPosition = writer.Position;
            WritePropertiesIntoBuffer(ref writer, includeChildrenMode, verifyCleanness, updateStoredBuffer, true);
            if (updateStoredBuffer)
            {
                UpdateStoredBuffer(ref writer, startPosition, includeChildrenMode, false);
            }
        }
        
        public virtual void UpdateStoredBuffer(ref BinaryBufferWriter writer, int startPosition, IncludeChildrenMode includeChildrenMode, bool updateDeserializedChildren)
        {
            _IsDirty = false;
            if (includeChildrenMode == IncludeChildrenMode.IncludeAllChildren)
            {
                _DescendantIsDirty = false;
                if (updateDeserializedChildren)
                {
                    if (_ANonSkippableEarlierExample_Accessed && _ANonSkippableEarlierExample != null)
                    {
                        _ANonSkippableEarlierExample.UpdateStoredBuffer(ref writer, startPosition + _ANonSkippableEarlierExample_ByteIndex + sizeof(int), IncludeChildrenMode.IncludeAllChildren, true);
                    }
                    if (_Example_Accessed && _Example != null)
                    {
                        _Example.UpdateStoredBuffer(ref writer, startPosition + _Example_ByteIndex + sizeof(int), IncludeChildrenMode.IncludeAllChildren, true);
                    }
                    if (_Example2_Accessed && _Example2 != null)
                    {
                        _Example2.UpdateStoredBuffer(ref writer, startPosition + _Example2_ByteIndex + sizeof(int), IncludeChildrenMode.IncludeAllChildren, true);
                    }
                    if (_Example3_Accessed && _Example3 != null)
                    {
                        _Example3.UpdateStoredBuffer(ref writer, startPosition + _Example3_ByteIndex + sizeof(int), IncludeChildrenMode.IncludeAllChildren, true);
                    }
                }
                
            }
            else
            {
                throw new Exception("Cannot update stored buffer when serializing only some children.");
            }
            
            var newBuffer = writer.Slice(startPosition);
            LazinatorMemoryStorage = ReplaceBuffer(LazinatorMemoryStorage, newBuffer, LazinatorParents, startPosition == 0, IsStruct);
        }
        
        protected virtual void WritePropertiesIntoBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer, bool includeUniqueID)
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
            WriteUncompressedPrimitives.WriteBool(ref writer, _MyIntsAre3);
            EncodeCharAndString.WriteNullableChar(ref writer, _Example2Char);
            WriteUncompressedPrimitives.WriteBool(ref writer, _Example3IsNull);
            WriteUncompressedPrimitives.WriteBool(ref writer, _ExampleHasDefaultValue);
            if (!(MyIntsAre3))
            {
                CompressedIntegralTypes.WriteCompressedInt(ref writer, _MyInt);
            }
            if (!(MyIntsAre3))
            {
                CompressedIntegralTypes.WriteCompressedInt(ref writer, _MyOtherInt);
            }
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode && !_ANonSkippableEarlierExample_Accessed)
                {
                    var deserialized = ANonSkippableEarlierExample;
                }
                WriteChild(ref writer, ref _ANonSkippableEarlierExample, includeChildrenMode, _ANonSkippableEarlierExample_Accessed, () => GetChildSlice(LazinatorMemoryStorage, _ANonSkippableEarlierExample_ByteIndex, _ANonSkippableEarlierExample_ByteLength, false, false, null), verifyCleanness, updateStoredBuffer, false, false, this);
            }
            if (updateStoredBuffer)
            {
                _ANonSkippableEarlierExample_ByteIndex = startOfObjectPosition - startPosition;
            }
            startOfObjectPosition = writer.Position;
            if (!(ExampleHasDefaultValue))
            {
                if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
                {
                    if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode && !_Example_Accessed)
                    {
                        var deserialized = Example;
                    }
                    WriteChild(ref writer, ref _Example, includeChildrenMode, _Example_Accessed, () => GetChildSlice(LazinatorMemoryStorage, _Example_ByteIndex, _Example_ByteLength, false, false, null), verifyCleanness, updateStoredBuffer, false, false, this);
                }
            }
            if (updateStoredBuffer)
            {
                _Example_ByteIndex = startOfObjectPosition - startPosition;
            }
            startOfObjectPosition = writer.Position;
            if (!(Example2Char != null))
            {
                if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren && LazinatorObjectVersion >= 4) 
                {
                    if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode && !_Example2_Accessed)
                    {
                        var deserialized = Example2;
                    }
                    WriteChild(ref writer, ref _Example2, includeChildrenMode, _Example2_Accessed, () => GetChildSlice(LazinatorMemoryStorage, _Example2_ByteIndex, _Example2_ByteLength, false, false, null), verifyCleanness, updateStoredBuffer, false, false, this);
                }
            }
            if (updateStoredBuffer)
            {
                _Example2_ByteIndex = startOfObjectPosition - startPosition;
            }
            startOfObjectPosition = writer.Position;
            if (!(Example3IsNull))
            {
                if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
                {
                    if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode && !_Example3_Accessed)
                    {
                        var deserialized = Example3;
                    }
                    WriteChild(ref writer, ref _Example3, includeChildrenMode, _Example3_Accessed, () => GetChildSlice(LazinatorMemoryStorage, _Example3_ByteIndex, _Example3_ByteLength, false, false, null), verifyCleanness, updateStoredBuffer, false, false, this);
                }
            }
            if (updateStoredBuffer)
            {
                _Example3_ByteIndex = startOfObjectPosition - startPosition;
            }
            if (updateStoredBuffer)
            {
                _Simplifiable_EndByteIndex = writer.Position - startPosition;
            }
        }
        
    }
}
