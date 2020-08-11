//dbc67c8b-fcc9-4b0c-9bf6-1d06103ccae3
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.380
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

#nullable enable
namespace LazinatorTests.Examples.ExampleHierarchy
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
    public partial class NullableEnabledContext : ILazinator
    {
        public bool IsStruct => false;
        
        /* Property definitions */
        
        protected int _ExplicitlyNullable_ByteIndex;
        protected int _ExplicitlyNullableInterface_ByteIndex;
        protected int _NonNullableClass_ByteIndex;
        protected int _NonNullableInterface_ByteIndex;
        protected virtual int _ExplicitlyNullable_ByteLength => _ExplicitlyNullableInterface_ByteIndex - _ExplicitlyNullable_ByteIndex;
        protected virtual int _ExplicitlyNullableInterface_ByteLength => _NonNullableClass_ByteIndex - _ExplicitlyNullableInterface_ByteIndex;
        protected virtual int _NonNullableClass_ByteLength => _NonNullableInterface_ByteIndex - _NonNullableClass_ByteIndex;
        private int _NullableEnabledContext_EndByteIndex;
        protected virtual int _NonNullableInterface_ByteLength => _NullableEnabledContext_EndByteIndex - _NonNullableInterface_ByteIndex;
        
        
        protected Example? _ExplicitlyNullable;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public Example? ExplicitlyNullable
        {
            get
            {
                if (!_ExplicitlyNullable_Accessed)
                {
                    Lazinate_ExplicitlyNullable();
                } 
                return _ExplicitlyNullable;
            }
            set
            {
                if (_ExplicitlyNullable != null)
                {
                    _ExplicitlyNullable.LazinatorParents = _ExplicitlyNullable.LazinatorParents.WithRemoved(this);
                }
                if (value != null)
                {
                    value.LazinatorParents = value.LazinatorParents.WithAdded(this);
                }
                
                IsDirty = true;
                DescendantIsDirty = true;
                _ExplicitlyNullable = value;
                _ExplicitlyNullable_Accessed = true;
            }
        }
        protected bool _ExplicitlyNullable_Accessed;
        private void Lazinate_ExplicitlyNullable()
        {
            if (LazinatorObjectBytes.Length == 0)
            {
                _ExplicitlyNullable = null;
            }
            else
            {
                LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _ExplicitlyNullable_ByteIndex, _ExplicitlyNullable_ByteLength, false, false, null);
                
                _ExplicitlyNullable = DeserializationFactory.Instance.CreateBaseOrDerivedType(1012, () => new Example(LazinatorConstructorEnum.LazinatorConstructor), childData, this); 
            }
            _ExplicitlyNullable_Accessed = true;
        }
        
        
        protected IExample? _ExplicitlyNullableInterface;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public IExample? ExplicitlyNullableInterface
        {
            get
            {
                if (!_ExplicitlyNullableInterface_Accessed)
                {
                    Lazinate_ExplicitlyNullableInterface();
                } 
                return _ExplicitlyNullableInterface;
            }
            set
            {
                if (_ExplicitlyNullableInterface != null)
                {
                    _ExplicitlyNullableInterface.LazinatorParents = _ExplicitlyNullableInterface.LazinatorParents.WithRemoved(this);
                }
                if (value != null)
                {
                    value.LazinatorParents = value.LazinatorParents.WithAdded(this);
                }
                
                IsDirty = true;
                DescendantIsDirty = true;
                _ExplicitlyNullableInterface = value;
                _ExplicitlyNullableInterface_Accessed = true;
            }
        }
        protected bool _ExplicitlyNullableInterface_Accessed;
        private void Lazinate_ExplicitlyNullableInterface()
        {
            if (LazinatorObjectBytes.Length == 0)
            {
                _ExplicitlyNullableInterface = null;
            }
            else
            {
                LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _ExplicitlyNullableInterface_ByteIndex, _ExplicitlyNullableInterface_ByteLength, false, false, null);
                
                _ExplicitlyNullableInterface = DeserializationFactory.Instance.CreateBasedOnType<IExample?>(childData, this); 
            }
            _ExplicitlyNullableInterface_Accessed = true;
        }
        
        
        protected Example _NonNullableClass;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public Example NonNullableClass
        {
            get
            {
                if (!_NonNullableClass_Accessed)
                {
                    Lazinate_NonNullableClass();
                } 
                return _NonNullableClass;
            }
            set
            {
                if (_NonNullableClass != null)
                {
                    _NonNullableClass.LazinatorParents = _NonNullableClass.LazinatorParents.WithRemoved(this);
                }
                if (value != null)
                {
                    value.LazinatorParents = value.LazinatorParents.WithAdded(this);
                }
                
                IsDirty = true;
                DescendantIsDirty = true;
                _NonNullableClass = value;
                _NonNullableClass_Accessed = true;
            }
        }
        protected bool _NonNullableClass_Accessed;
        private void Lazinate_NonNullableClass()
        {
            if (LazinatorObjectBytes.Length == 0)
            {
            }
            else
            {
                LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _NonNullableClass_ByteIndex, _NonNullableClass_ByteLength, false, false, null);
                
                _NonNullableClass = DeserializationFactory.Instance.CreateBaseOrDerivedType(1012, () => new Example(LazinatorConstructorEnum.LazinatorConstructor), childData, this); 
            }
            _NonNullableClass_Accessed = true;
        }
        
        
        protected IExample _NonNullableInterface;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public IExample NonNullableInterface
        {
            get
            {
                if (!_NonNullableInterface_Accessed)
                {
                    Lazinate_NonNullableInterface();
                } 
                return _NonNullableInterface;
            }
            set
            {
                if (_NonNullableInterface != null)
                {
                    _NonNullableInterface.LazinatorParents = _NonNullableInterface.LazinatorParents.WithRemoved(this);
                }
                if (value != null)
                {
                    value.LazinatorParents = value.LazinatorParents.WithAdded(this);
                }
                
                IsDirty = true;
                DescendantIsDirty = true;
                _NonNullableInterface_Accessed = true;
            }
        }
        protected bool _NonNullableInterface_Accessed;
        private void Lazinate_NonNullableInterface()
        {
            if (LazinatorObjectBytes.Length == 0)
            {
            }
            else
            {
                LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _NonNullableInterface_ByteIndex, _NonNullableInterface_ByteLength, false, false, null);
                
                _NonNullableInterface = DeserializationFactory.Instance.CreateBasedOnType<IExample>(childData, this); 
            }
            _NonNullableInterface_Accessed = true;
        }
        
        
        /* Serialization, deserialization, and object relationships */
        
        public NullableEnabledContext(LazinatorConstructorEnum constructorEnum)
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
            
            ReadGenericIDIfApplicable(ContainsOpenGenericParameters, LazinatorUniqueID, span, ref bytesSoFar);
            
            int lazinatorLibraryVersion = span.ToDecompressedInt(ref bytesSoFar);
            
            int serializedVersionNumber = span.ToDecompressedInt(ref bytesSoFar);
            
            OriginalIncludeChildrenMode = (IncludeChildrenMode)span.ToByte(ref bytesSoFar);
            
            ConvertFromBytesAfterHeader(OriginalIncludeChildrenMode, serializedVersionNumber, ref bytesSoFar);
            return bytesSoFar;
        }
        
        public virtual LazinatorMemory SerializeLazinator(IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer) 
        {
            if (LazinatorMemoryStorage.IsEmpty || includeChildrenMode != OriginalIncludeChildrenMode || (verifyCleanness || IsDirty || (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && DescendantIsDirty)))
            {
                return EncodeToNewBuffer(includeChildrenMode, verifyCleanness, updateStoredBuffer);
            }
            BinaryBufferWriter writer = new BinaryBufferWriter(LazinatorMemoryStorage.Length);
            writer.Write(LazinatorMemoryStorage.Span);
            return writer.LazinatorMemory;
        }
        
        protected virtual LazinatorMemory EncodeToNewBuffer(IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer) 
        {
            int bufferSize = LazinatorMemoryStorage.Length == 0 ? ExpandableBytes.DefaultMinBufferSize : LazinatorMemoryStorage.Length;
            BinaryBufferWriter writer = new BinaryBufferWriter(bufferSize);
            SerializeExistingBuffer(ref writer, includeChildrenMode, verifyCleanness, updateStoredBuffer);
            return writer.LazinatorMemory;
        }
        
        public virtual ILazinator? CloneLazinator(IncludeChildrenMode includeChildrenMode = IncludeChildrenMode.IncludeAllChildren, CloneBufferOptions cloneBufferOptions = CloneBufferOptions.IndependentBuffers)
        {
            var clone = new NullableEnabledContext(LazinatorConstructorEnum.LazinatorConstructor)
            {
                OriginalIncludeChildrenMode = includeChildrenMode
            };
            clone = CompleteClone(this, clone, includeChildrenMode, cloneBufferOptions);
            return clone;
        }
        
        public virtual ILazinator? AssignCloneProperties(ILazinator? clone, IncludeChildrenMode includeChildrenMode)
        {
            clone.FreeInMemoryObjects();
            NullableEnabledContext typedClone = (NullableEnabledContext) clone;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                if (ExplicitlyNullable == null)
                {
                    typedClone.ExplicitlyNullable = null;
                }
                else
                {
                    typedClone.ExplicitlyNullable = (Example?) ExplicitlyNullable.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer);
                }
            }
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                if (ExplicitlyNullableInterface == null)
                {
                    typedClone.ExplicitlyNullableInterface = null;
                }
                else
                {
                    typedClone.ExplicitlyNullableInterface = (IExample?) ExplicitlyNullableInterface.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer);
                }
            }
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                if (NonNullableClass == null)
                {
                }
                else
                {
                    typedClone.NonNullableClass = (Example) NonNullableClass.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer);
                }
            }
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                if (NonNullableInterface == null)
                {
                }
                else
                {
                    typedClone.NonNullableInterface = (IExample) NonNullableInterface.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer);
                }
            }
            
            return typedClone;
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
        protected virtual ReadOnlyMemory<byte> LazinatorObjectBytes => LazinatorMemoryStorage.IsEmpty ? LazinatorMemory.EmptyReadOnlyMemory : LazinatorMemoryStorage.Memory;
        
        public virtual void UpdateStoredBuffer()
        {
            if (!IsDirty && !DescendantIsDirty && LazinatorObjectBytes.Length > 0 && OriginalIncludeChildrenMode == IncludeChildrenMode.IncludeAllChildren)
            {
                return;
            }
            var previousBuffer = LazinatorMemoryStorage;
            if (LazinatorMemoryStorage.IsEmpty || IncludeChildrenMode.IncludeAllChildren != OriginalIncludeChildrenMode || (IsDirty || DescendantIsDirty))
            {
                LazinatorMemoryStorage = EncodeToNewBuffer(IncludeChildrenMode.IncludeAllChildren, false, true);
            }
            else
            {
                BinaryBufferWriter writer = new BinaryBufferWriter(LazinatorMemoryStorage.Length);
                writer.Write(LazinatorMemoryStorage.Span);
                LazinatorMemoryStorage = writer.LazinatorMemory;
            }
            OriginalIncludeChildrenMode = IncludeChildrenMode.IncludeAllChildren;
            if (!LazinatorParents.Any())
            {
                previousBuffer.Dispose();
            }
        }
        
        public virtual int GetByteLength()
        {
            UpdateStoredBuffer();
            return LazinatorObjectBytes.Length;
        }
        
        public virtual bool NonBinaryHash32 => false;
        
        
        public IEnumerable<ILazinator?> EnumerateLazinatorNodes(Func<ILazinator?, bool>? matchCriterion, bool stopExploringBelowMatch, Func<ILazinator?, bool>? exploreCriterion, bool exploreOnlyDeserializedChildren, bool enumerateNulls)
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
        
        public virtual IEnumerable<(string propertyName, ILazinator? descendant)> EnumerateLazinatorDescendants(Func<ILazinator?, bool>? matchCriterion, bool stopExploringBelowMatch, Func<ILazinator?, bool>? exploreCriterion, bool exploreOnlyDeserializedChildren, bool enumerateNulls)
        {
            if (enumerateNulls && (!exploreOnlyDeserializedChildren || _ExplicitlyNullable_Accessed) && ExplicitlyNullable == null)
            {
                yield return ("ExplicitlyNullable", default);
            }
            else
            {
                if ((!exploreOnlyDeserializedChildren && ExplicitlyNullable != null) || (_ExplicitlyNullable_Accessed && _ExplicitlyNullable != null))
                {
                    bool isMatch = matchCriterion == null || matchCriterion(ExplicitlyNullable);
                    bool shouldExplore = exploreCriterion == null || exploreCriterion(ExplicitlyNullable);
                    if (isMatch)
                    {
                        yield return ("ExplicitlyNullable", ExplicitlyNullable);
                    }
                    if ((!stopExploringBelowMatch || !isMatch) && shouldExplore)
                    {
                        foreach (var toYield in ExplicitlyNullable.EnumerateLazinatorDescendants(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
                        {
                            yield return ("ExplicitlyNullable" + "." + toYield.propertyName, toYield.descendant);
                        }
                    }
                }
            }
            if (enumerateNulls && (!exploreOnlyDeserializedChildren || _ExplicitlyNullableInterface_Accessed) && ExplicitlyNullableInterface == null)
            {
                yield return ("ExplicitlyNullableInterface", default);
            }
            else
            {
                if ((!exploreOnlyDeserializedChildren && ExplicitlyNullableInterface != null) || (_ExplicitlyNullableInterface_Accessed && _ExplicitlyNullableInterface != null))
                {
                    bool isMatch = matchCriterion == null || matchCriterion(ExplicitlyNullableInterface);
                    bool shouldExplore = exploreCriterion == null || exploreCriterion(ExplicitlyNullableInterface);
                    if (isMatch)
                    {
                        yield return ("ExplicitlyNullableInterface", ExplicitlyNullableInterface);
                    }
                    if ((!stopExploringBelowMatch || !isMatch) && shouldExplore)
                    {
                        foreach (var toYield in ExplicitlyNullableInterface.EnumerateLazinatorDescendants(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
                        {
                            yield return ("ExplicitlyNullableInterface" + "." + toYield.propertyName, toYield.descendant);
                        }
                    }
                }
            }
            if (enumerateNulls && (!exploreOnlyDeserializedChildren || _NonNullableClass_Accessed) && NonNullableClass == null)
            {
                yield return ("NonNullableClass", default);
            }
            else
            {
                if ((!exploreOnlyDeserializedChildren && NonNullableClass != null) || (_NonNullableClass_Accessed && _NonNullableClass != null))
                {
                    bool isMatch = matchCriterion == null || matchCriterion(NonNullableClass);
                    bool shouldExplore = exploreCriterion == null || exploreCriterion(NonNullableClass);
                    if (isMatch)
                    {
                        yield return ("NonNullableClass", NonNullableClass);
                    }
                    if ((!stopExploringBelowMatch || !isMatch) && shouldExplore)
                    {
                        foreach (var toYield in NonNullableClass.EnumerateLazinatorDescendants(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
                        {
                            yield return ("NonNullableClass" + "." + toYield.propertyName, toYield.descendant);
                        }
                    }
                }
            }
            if (enumerateNulls && (!exploreOnlyDeserializedChildren || _NonNullableInterface_Accessed) && NonNullableInterface == null)
            {
                yield return ("NonNullableInterface", default);
            }
            else
            {
                if ((!exploreOnlyDeserializedChildren && NonNullableInterface != null) || (_NonNullableInterface_Accessed && _NonNullableInterface != null))
                {
                    bool isMatch = matchCriterion == null || matchCriterion(NonNullableInterface);
                    bool shouldExplore = exploreCriterion == null || exploreCriterion(NonNullableInterface);
                    if (isMatch)
                    {
                        yield return ("NonNullableInterface", NonNullableInterface);
                    }
                    if ((!stopExploringBelowMatch || !isMatch) && shouldExplore)
                    {
                        foreach (var toYield in NonNullableInterface.EnumerateLazinatorDescendants(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
                        {
                            yield return ("NonNullableInterface" + "." + toYield.propertyName, toYield.descendant);
                        }
                    }
                }
            }
            yield break;
        }
        
        
        public virtual IEnumerable<(string propertyName, object descendant)> EnumerateNonLazinatorProperties()
        {
            yield break;
        }
        
        public virtual ILazinator? ForEachLazinator(Func<ILazinator?, ILazinator?>? changeFunc, bool exploreOnlyDeserializedChildren, bool changeThisLevel)
        {
            if ((!exploreOnlyDeserializedChildren && ExplicitlyNullable != null) || (_ExplicitlyNullable_Accessed && _ExplicitlyNullable != null))
            {
                _ExplicitlyNullable = (Example?) _ExplicitlyNullable.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, true);
            }
            if ((!exploreOnlyDeserializedChildren && ExplicitlyNullableInterface != null) || (_ExplicitlyNullableInterface_Accessed && _ExplicitlyNullableInterface != null))
            {
                _ExplicitlyNullableInterface = (IExample?) _ExplicitlyNullableInterface.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, true);
            }
            if ((!exploreOnlyDeserializedChildren && NonNullableClass != null) || (_NonNullableClass_Accessed && _NonNullableClass != null))
            {
                _NonNullableClass = (Example) _NonNullableClass.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, true);
            }
            if ((!exploreOnlyDeserializedChildren && NonNullableInterface != null) || (_NonNullableInterface_Accessed && _NonNullableInterface != null))
            {
                _NonNullableInterface = (IExample) _NonNullableInterface.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, true);
            }
            if (changeThisLevel)
            {
                return changeFunc(this);
            }
            return this;
        }
        
        public virtual void FreeInMemoryObjects()
        {
            _ExplicitlyNullable = default;
            _ExplicitlyNullableInterface = default;
            _NonNullableClass = default;
            _NonNullableInterface = default;
            _ExplicitlyNullable_Accessed = _ExplicitlyNullableInterface_Accessed = _NonNullableClass_Accessed = _NonNullableInterface_Accessed = false;
            IsDirty = false;
            DescendantIsDirty = false;
            HasChanged = false;
            DescendantHasChanged = false;
        }
        
        /* Conversion */
        
        public virtual int LazinatorUniqueID => 1085;
        
        protected virtual bool ContainsOpenGenericParameters => false;
        public virtual LazinatorGenericIDType LazinatorGenericID => default;
        
        
        public virtual int LazinatorObjectVersion { get; set; } = 0;
        
        
        public virtual void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
            _ExplicitlyNullable_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            _ExplicitlyNullableInterface_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            _NonNullableClass_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            _NonNullableInterface_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            _NullableEnabledContext_EndByteIndex = bytesSoFar;
        }
        
        public virtual void SerializeExistingBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
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
        
        public virtual void UpdateStoredBuffer(ref BinaryBufferWriter writer, int startPosition, int length, IncludeChildrenMode includeChildrenMode, bool updateDeserializedChildren)
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
        
        protected virtual void UpdateDeserializedChildren(ref BinaryBufferWriter writer, int startPosition)
        {
            if (_ExplicitlyNullable_Accessed && _ExplicitlyNullable != null)
            {
                _ExplicitlyNullable.UpdateStoredBuffer(ref writer, startPosition + _ExplicitlyNullable_ByteIndex + sizeof(int), _ExplicitlyNullable_ByteLength - sizeof(int), IncludeChildrenMode.IncludeAllChildren, true);
            }
            if (_ExplicitlyNullableInterface_Accessed && _ExplicitlyNullableInterface != null)
            {
                _ExplicitlyNullableInterface.UpdateStoredBuffer(ref writer, startPosition + _ExplicitlyNullableInterface_ByteIndex + sizeof(int), _ExplicitlyNullableInterface_ByteLength - sizeof(int), IncludeChildrenMode.IncludeAllChildren, true);
            }
            if (_NonNullableClass_Accessed && _NonNullableClass != null)
            {
                _NonNullableClass.UpdateStoredBuffer(ref writer, startPosition + _NonNullableClass_ByteIndex + sizeof(int), _NonNullableClass_ByteLength - sizeof(int), IncludeChildrenMode.IncludeAllChildren, true);
            }
            if (_NonNullableInterface_Accessed && _NonNullableInterface != null)
            {
                _NonNullableInterface.UpdateStoredBuffer(ref writer, startPosition + _NonNullableInterface_ByteIndex + sizeof(int), _NonNullableInterface_ByteLength - sizeof(int), IncludeChildrenMode.IncludeAllChildren, true);
            }
        }
        
        
        protected virtual void WritePropertiesIntoBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer, bool includeUniqueID)
        {
            int startPosition = writer.Position;
            int startOfObjectPosition = 0;
            // header information
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
            writer.Write((byte)includeChildrenMode);
            // write properties
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                if ((includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode) && !_ExplicitlyNullable_Accessed)
                {
                    var deserialized = ExplicitlyNullable;
                }
                WriteChild(ref writer, ref _ExplicitlyNullable, includeChildrenMode, _ExplicitlyNullable_Accessed, () => GetChildSlice(LazinatorMemoryStorage, _ExplicitlyNullable_ByteIndex, _ExplicitlyNullable_ByteLength, false, false, null), verifyCleanness, updateStoredBuffer, false, false, this);
            }
            if (updateStoredBuffer)
            {
                _ExplicitlyNullable_ByteIndex = startOfObjectPosition - startPosition;
            }
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                if ((includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode) && !_ExplicitlyNullableInterface_Accessed)
                {
                    var deserialized = ExplicitlyNullableInterface;
                }
                WriteChild(ref writer, ref _ExplicitlyNullableInterface, includeChildrenMode, _ExplicitlyNullableInterface_Accessed, () => GetChildSlice(LazinatorMemoryStorage, _ExplicitlyNullableInterface_ByteIndex, _ExplicitlyNullableInterface_ByteLength, false, false, null), verifyCleanness, updateStoredBuffer, false, false, this);
            }
            if (updateStoredBuffer)
            {
                _ExplicitlyNullableInterface_ByteIndex = startOfObjectPosition - startPosition;
            }
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                if ((includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode) && !_NonNullableClass_Accessed)
                {
                    var deserialized = NonNullableClass;
                }
                WriteChild(ref writer, ref _NonNullableClass, includeChildrenMode, _NonNullableClass_Accessed, () => GetChildSlice(LazinatorMemoryStorage, _NonNullableClass_ByteIndex, _NonNullableClass_ByteLength, false, false, null), verifyCleanness, updateStoredBuffer, false, false, this);
            }
            if (updateStoredBuffer)
            {
                _NonNullableClass_ByteIndex = startOfObjectPosition - startPosition;
            }
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                if ((includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode) && !_NonNullableInterface_Accessed)
                {
                    var deserialized = NonNullableInterface;
                }
                WriteChild(ref writer, ref _NonNullableInterface, includeChildrenMode, _NonNullableInterface_Accessed, () => GetChildSlice(LazinatorMemoryStorage, _NonNullableInterface_ByteIndex, _NonNullableInterface_ByteLength, false, false, null), verifyCleanness, updateStoredBuffer, false, false, this);
            }
            if (updateStoredBuffer)
            {
                _NonNullableInterface_ByteIndex = startOfObjectPosition - startPosition;
            }
            if (updateStoredBuffer)
            {
                _NullableEnabledContext_EndByteIndex = writer.Position - startPosition;
            }
        }
        
    }
}
