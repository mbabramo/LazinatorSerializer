//e122c5c4-7ada-6e7b-da0a-f15e46f2770c
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.359
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Lazinator.Collections.Avl.ListTree
{
    using Lazinator.Attributes;
    using Lazinator.Buffers;
    using Lazinator.Collections.Avl.ValueTree;
    using Lazinator.Collections.Factories;
    using Lazinator.Collections.Interfaces;
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
    public partial class AvlListTree<T> : ILazinator
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsStruct => false;
        
        /* Property definitions */
        
        protected int _InteriorCollectionFactory_ByteIndex;
        protected int _UnderlyingTree_ByteIndex;
        protected virtual int _InteriorCollectionFactory_ByteLength => _UnderlyingTree_ByteIndex - _InteriorCollectionFactory_ByteIndex;
        private int _AvlListTree_T_EndByteIndex;
        protected virtual int _UnderlyingTree_ByteLength => _AvlListTree_T_EndByteIndex - _UnderlyingTree_ByteIndex;
        
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected bool _AllowDuplicates;
        public bool AllowDuplicates
        {
            [DebuggerStepThrough]
            get
            {
                return _AllowDuplicates;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _AllowDuplicates = value;
            }
        }
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected bool _Unbalanced;
        public bool Unbalanced
        {
            [DebuggerStepThrough]
            get
            {
                return _Unbalanced;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _Unbalanced = value;
            }
        }
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected IAvlListTreeInteriorCollectionFactory<T> _InteriorCollectionFactory;
        public virtual IAvlListTreeInteriorCollectionFactory<T> InteriorCollectionFactory
        {
            [DebuggerStepThrough]
            get
            {
                if (!_InteriorCollectionFactory_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _InteriorCollectionFactory = default(IAvlListTreeInteriorCollectionFactory<T>);
                    }
                    else
                    {
                        LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _InteriorCollectionFactory_ByteIndex, _InteriorCollectionFactory_ByteLength, false, false, null);
                        
                        _InteriorCollectionFactory = DeserializationFactory.Instance.CreateBasedOnType<IAvlListTreeInteriorCollectionFactory<T>>(childData, this); 
                    }
                    _InteriorCollectionFactory_Accessed = true;
                } 
                return _InteriorCollectionFactory;
            }
            [DebuggerStepThrough]
            set
            {
                if (_InteriorCollectionFactory != null)
                {
                    _InteriorCollectionFactory.LazinatorParents = _InteriorCollectionFactory.LazinatorParents.WithRemoved(this);
                }
                if (value != null)
                {
                    value.LazinatorParents = value.LazinatorParents.WithAdded(this);
                }
                
                IsDirty = true;
                DescendantIsDirty = true;
                _InteriorCollectionFactory = value;
                _InteriorCollectionFactory_Accessed = true;
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected bool _InteriorCollectionFactory_Accessed;
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected AvlIndexableTree<IMultivalueContainer<T>> _UnderlyingTree;
        public virtual AvlIndexableTree<IMultivalueContainer<T>> UnderlyingTree
        {
            [DebuggerStepThrough]
            get
            {
                if (!_UnderlyingTree_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _UnderlyingTree = default(AvlIndexableTree<IMultivalueContainer<T>>);
                    }
                    else
                    {
                        LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _UnderlyingTree_ByteIndex, _UnderlyingTree_ByteLength, false, false, null);
                        
                        _UnderlyingTree = DeserializationFactory.Instance.CreateBaseOrDerivedType(166, () => new AvlIndexableTree<IMultivalueContainer<T>>(), childData, this); 
                    }
                    _UnderlyingTree_Accessed = true;
                } 
                return _UnderlyingTree;
            }
            [DebuggerStepThrough]
            set
            {
                if (_UnderlyingTree != null)
                {
                    _UnderlyingTree.LazinatorParents = _UnderlyingTree.LazinatorParents.WithRemoved(this);
                }
                if (value != null)
                {
                    value.LazinatorParents = value.LazinatorParents.WithAdded(this);
                }
                
                IsDirty = true;
                DescendantIsDirty = true;
                _UnderlyingTree = value;
                _UnderlyingTree_Accessed = true;
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected bool _UnderlyingTree_Accessed;
        
        /* Serialization, deserialization, and object relationships */
        
        public AvlListTree() : base()
        {
        }
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public virtual LazinatorParentsCollection LazinatorParents { get; set; }
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
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
        
        public virtual ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode = IncludeChildrenMode.IncludeAllChildren, CloneBufferOptions cloneBufferOptions = CloneBufferOptions.IndependentBuffers)
        {
            var clone = new AvlListTree<T>()
            {
                OriginalIncludeChildrenMode = includeChildrenMode
            };
            clone = CompleteClone(this, clone, includeChildrenMode, cloneBufferOptions);
            return clone;
        }
        
        public virtual ILazinator AssignCloneProperties(ILazinator clone, IncludeChildrenMode includeChildrenMode)
        {
            clone.FreeInMemoryObjects();
            AvlListTree<T> typedClone = (AvlListTree<T>) clone;
            typedClone.AllowDuplicates = AllowDuplicates;
            typedClone.Unbalanced = Unbalanced;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                if (InteriorCollectionFactory == null)
                {
                    typedClone.InteriorCollectionFactory = default(IAvlListTreeInteriorCollectionFactory<T>);
                }
                else
                {
                    typedClone.InteriorCollectionFactory = (IAvlListTreeInteriorCollectionFactory<T>) InteriorCollectionFactory.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer);
                }
            }
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                if (UnderlyingTree == null)
                {
                    typedClone.UnderlyingTree = default(AvlIndexableTree<IMultivalueContainer<T>>);
                }
                else
                {
                    typedClone.UnderlyingTree = (AvlIndexableTree<IMultivalueContainer<T>>) UnderlyingTree.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer);
                }
            }
            
            return typedClone;
        }
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public virtual bool HasChanged { get; set; }
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected bool _IsDirty;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
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
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected bool _DescendantHasChanged;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
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
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected bool _DescendantIsDirty;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
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
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public virtual LazinatorMemory LazinatorMemoryStorage
        {
            get;
            set;
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
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
            if (enumerateNulls && (!exploreOnlyDeserializedChildren || _InteriorCollectionFactory_Accessed) && InteriorCollectionFactory == null)
            {
                yield return ("InteriorCollectionFactory", default);
            }
            else
            {
                if ((!exploreOnlyDeserializedChildren && InteriorCollectionFactory != null) || (_InteriorCollectionFactory_Accessed && _InteriorCollectionFactory != null))
                {
                    bool isMatch = matchCriterion == null || matchCriterion(InteriorCollectionFactory);
                    bool shouldExplore = exploreCriterion == null || exploreCriterion(InteriorCollectionFactory);
                    if (isMatch)
                    {
                        yield return ("InteriorCollectionFactory", InteriorCollectionFactory);
                    }
                    if ((!stopExploringBelowMatch || !isMatch) && shouldExplore)
                    {
                        foreach (var toYield in InteriorCollectionFactory.EnumerateLazinatorDescendants(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
                        {
                            yield return ("InteriorCollectionFactory" + "." + toYield.propertyName, toYield.descendant);
                        }
                    }
                }
            }
            if (enumerateNulls && (!exploreOnlyDeserializedChildren || _UnderlyingTree_Accessed) && UnderlyingTree == null)
            {
                yield return ("UnderlyingTree", default);
            }
            else
            {
                if ((!exploreOnlyDeserializedChildren && UnderlyingTree != null) || (_UnderlyingTree_Accessed && _UnderlyingTree != null))
                {
                    bool isMatch = matchCriterion == null || matchCriterion(UnderlyingTree);
                    bool shouldExplore = exploreCriterion == null || exploreCriterion(UnderlyingTree);
                    if (isMatch)
                    {
                        yield return ("UnderlyingTree", UnderlyingTree);
                    }
                    if ((!stopExploringBelowMatch || !isMatch) && shouldExplore)
                    {
                        foreach (var toYield in UnderlyingTree.EnumerateLazinatorDescendants(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
                        {
                            yield return ("UnderlyingTree" + "." + toYield.propertyName, toYield.descendant);
                        }
                    }
                }
            }
            yield break;
        }
        
        
        public virtual IEnumerable<(string propertyName, object descendant)> EnumerateNonLazinatorProperties()
        {
            yield return ("AllowDuplicates", (object)AllowDuplicates);
            yield return ("Unbalanced", (object)Unbalanced);
            yield break;
        }
        
        public virtual ILazinator ForEachLazinator(Func<ILazinator, ILazinator> changeFunc, bool exploreOnlyDeserializedChildren, bool changeThisLevel)
        {
            if ((!exploreOnlyDeserializedChildren && InteriorCollectionFactory != null) || (_InteriorCollectionFactory_Accessed && _InteriorCollectionFactory != null))
            {
                _InteriorCollectionFactory = (IAvlListTreeInteriorCollectionFactory<T>) _InteriorCollectionFactory.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, true);
            }
            if ((!exploreOnlyDeserializedChildren && UnderlyingTree != null) || (_UnderlyingTree_Accessed && _UnderlyingTree != null))
            {
                _UnderlyingTree = (AvlIndexableTree<IMultivalueContainer<T>>) _UnderlyingTree.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, true);
            }
            if (changeThisLevel)
            {
                return changeFunc(this);
            }
            return this;
        }
        
        public virtual void FreeInMemoryObjects()
        {
            _InteriorCollectionFactory = default;
            _UnderlyingTree = default;
            _InteriorCollectionFactory_Accessed = _UnderlyingTree_Accessed = false;
            IsDirty = false;
            DescendantIsDirty = false;
            HasChanged = false;
            DescendantHasChanged = false;
        }
        
        /* Conversion */
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public virtual int LazinatorUniqueID => 193;
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected virtual bool ContainsOpenGenericParameters => true;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public virtual LazinatorGenericIDType LazinatorGenericID => LazinatorGenericIDType.GetCachedForType<AvlListTree<T>>(() => DeserializationFactory.Instance.GetUniqueIDListForGenericType(193, new Type[] { typeof(T) }));
        
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public virtual int LazinatorObjectVersion { get; set; } = 0;
        
        
        public virtual void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
            _AllowDuplicates = span.ToBoolean(ref bytesSoFar);
            _Unbalanced = span.ToBoolean(ref bytesSoFar);
            _InteriorCollectionFactory_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            _UnderlyingTree_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            _AvlListTree_T_EndByteIndex = bytesSoFar;
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
                throw new LazinatorSerializationException("Cannot update stored buffer when serializing only some children.");
            }
            
            var newBuffer = writer.Slice(startPosition, length);
            LazinatorMemoryStorage = newBuffer;
        }
        
        protected virtual void UpdateDeserializedChildren(ref BinaryBufferWriter writer, int startPosition)
        {
            if (_InteriorCollectionFactory_Accessed && _InteriorCollectionFactory != null)
            {
                _InteriorCollectionFactory.UpdateStoredBuffer(ref writer, startPosition + _InteriorCollectionFactory_ByteIndex + sizeof(int), _InteriorCollectionFactory_ByteLength - sizeof(int), IncludeChildrenMode.IncludeAllChildren, true);
            }
            if (_UnderlyingTree_Accessed && _UnderlyingTree != null)
            {
                _UnderlyingTree.UpdateStoredBuffer(ref writer, startPosition + _UnderlyingTree_ByteIndex + sizeof(int), _UnderlyingTree_ByteLength - sizeof(int), IncludeChildrenMode.IncludeAllChildren, true);
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
            WriteUncompressedPrimitives.WriteBool(ref writer, _AllowDuplicates);
            WriteUncompressedPrimitives.WriteBool(ref writer, _Unbalanced);
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                if ((includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode) && !_InteriorCollectionFactory_Accessed)
                {
                    var deserialized = InteriorCollectionFactory;
                }
                WriteChild(ref writer, ref _InteriorCollectionFactory, includeChildrenMode, _InteriorCollectionFactory_Accessed, () => GetChildSlice(LazinatorMemoryStorage, _InteriorCollectionFactory_ByteIndex, _InteriorCollectionFactory_ByteLength, false, false, null), verifyCleanness, updateStoredBuffer, false, false, this);
            }
            if (updateStoredBuffer)
            {
                _InteriorCollectionFactory_ByteIndex = startOfObjectPosition - startPosition;
            }
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                if ((includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode) && !_UnderlyingTree_Accessed)
                {
                    var deserialized = UnderlyingTree;
                }
                WriteChild(ref writer, ref _UnderlyingTree, includeChildrenMode, _UnderlyingTree_Accessed, () => GetChildSlice(LazinatorMemoryStorage, _UnderlyingTree_ByteIndex, _UnderlyingTree_ByteLength, false, false, null), verifyCleanness, updateStoredBuffer, false, false, this);
            }
            if (updateStoredBuffer)
            {
                _UnderlyingTree_ByteIndex = startOfObjectPosition - startPosition;
            }
            if (updateStoredBuffer)
            {
                _AvlListTree_T_EndByteIndex = writer.Position - startPosition;
            }
        }
        
    }
}
