/*Location7886*//*Location7872*///68409ad1-f77c-dbc7-8830-e7ce2dbb3c67
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.380
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

#nullable disable
namespace LazinatorCollections
{/*Location7873*/
    using Lazinator.Attributes;/*Location7874*/
    using Lazinator.Buffers;/*Location7875*/
    using Lazinator.Core;/*Location7876*/
    using Lazinator.Exceptions;/*Location7877*/
    using Lazinator.Support;/*Location7878*/
    using System;/*Location7879*/
    using System.Buffers;/*Location7880*/
    using System.Collections.Generic;/*Location7881*/
    using System.Diagnostics;/*Location7882*/
    using System.IO;/*Location7883*/
    using System.Linq;/*Location7884*/
    using System.Runtime.InteropServices;/*Location7885*/
    using static Lazinator.Core.LazinatorUtilities;
    
    [Autogenerated]
    public partial class LazinatorLinkedList<T> : ILazinator
    {
        /*Location7887*/[DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsStruct => false;
        
        /*Location7888*//* Property definitions */
        
        /*Location7889*/        protected int _FirstNode_ByteIndex;
        /*Location7890*/private int _LazinatorLinkedList_T_EndByteIndex;
        /*Location7891*/protected virtual int _FirstNode_ByteLength => _LazinatorLinkedList_T_EndByteIndex - _FirstNode_ByteIndex;
        
        /*Location7892*/
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
            protected set
            {
                IsDirty = true;
                _AllowDuplicates = value;
            }
        }
        /*Location7893*/
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected int _Count;
        public int Count
        {
            [DebuggerStepThrough]
            get
            {
                return _Count;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _Count = value;
            }
        }
        /*Location7894*/
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected LazinatorLinkedListNode<T> _FirstNode;
        public virtual LazinatorLinkedListNode<T> FirstNode
        {
            [DebuggerStepThrough]
            get
            {
                if (!_FirstNode_Accessed)
                {
                    Lazinate_FirstNode();
                } 
                return _FirstNode;
            }
            [DebuggerStepThrough]
            set
            {
                if (_FirstNode != null)
                {
                    _FirstNode.LazinatorParents = _FirstNode.LazinatorParents.WithRemoved(this);
                }
                if (value != null)
                {
                    value.LazinatorParents = value.LazinatorParents.WithAdded(this);
                }
                
                IsDirty = true;
                DescendantIsDirty = true;
                _FirstNode = value;
                _FirstNode_Accessed = true;
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected bool _FirstNode_Accessed;
        private void Lazinate_FirstNode()
        {
            if (LazinatorObjectBytes.Length == 0)
            {
                _FirstNode = null;
            }
            else
            {
                LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _FirstNode_ByteIndex, _FirstNode_ByteLength, false, false, null);
                
                _FirstNode = DeserializationFactory.Instance.CreateBaseOrDerivedType(224, () => new LazinatorLinkedListNode<T>(LazinatorConstructorEnum.LazinatorConstructor), childData, this); 
            }
            
            _FirstNode_Accessed = true;
        }
        
        /*Location7898*/
        /* Serialization, deserialization, and object relationships */
        
        public LazinatorLinkedList(LazinatorConstructorEnum constructorEnum)
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
            var clone = new LazinatorLinkedList<T>(LazinatorConstructorEnum.LazinatorConstructor)
            {
                OriginalIncludeChildrenMode = includeChildrenMode
            };
            clone = CompleteClone(this, clone, includeChildrenMode, cloneBufferOptions);
            return clone;
        }
        
        public virtual ILazinator AssignCloneProperties(ILazinator clone, IncludeChildrenMode includeChildrenMode)
        {
            clone.FreeInMemoryObjects();
            LazinatorLinkedList<T> typedClone = (LazinatorLinkedList<T>) clone;
            /*Location7895*/typedClone.AllowDuplicates = AllowDuplicates;
            /*Location7896*/typedClone.Count = Count;
            /*Location7897*/if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                if (FirstNode == null)
                {
                    typedClone.FirstNode = null;
                }
                else
                {
                    typedClone.FirstNode = (LazinatorLinkedListNode<T>) FirstNode.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer);
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
        
        /*Location7899*/
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
        
        /*Location7900*/public virtual IEnumerable<(string propertyName, ILazinator descendant)> EnumerateLazinatorDescendants(Func<ILazinator, bool> matchCriterion, bool stopExploringBelowMatch, Func<ILazinator, bool> exploreCriterion, bool exploreOnlyDeserializedChildren, bool enumerateNulls)
        {
            /*Location7901*/if (enumerateNulls && (!exploreOnlyDeserializedChildren || _FirstNode_Accessed) && FirstNode == null)
            {
                yield return ("FirstNode", default);
            }
            else
            {
                if ((!exploreOnlyDeserializedChildren && FirstNode != null) || (_FirstNode_Accessed && _FirstNode != null))
                {
                    bool isMatch_FirstNode = matchCriterion == null || matchCriterion(FirstNode);
                    bool shouldExplore_FirstNode = exploreCriterion == null || exploreCriterion(FirstNode);
                    if (isMatch_FirstNode)
                    {
                        yield return ("FirstNode", FirstNode);
                    }
                    if ((!stopExploringBelowMatch || !isMatch_FirstNode) && shouldExplore_FirstNode)
                    {
                        foreach (var toYield in FirstNode.EnumerateLazinatorDescendants(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
                        {
                            yield return ("FirstNode" + "." + toYield.propertyName, toYield.descendant);
                        }
                    }
                }
                
            }
            
            /*Location7902*/yield break;
        }
        /*Location7903*/
        
        public virtual IEnumerable<(string propertyName, object descendant)> EnumerateNonLazinatorProperties()
        {
            /*Location7904*/yield return ("AllowDuplicates", (object)AllowDuplicates);
            /*Location7905*/yield return ("Count", (object)Count);
            /*Location7906*/yield break;
        }
        /*Location7907*/
        public virtual ILazinator ForEachLazinator(Func<ILazinator, ILazinator> changeFunc, bool exploreOnlyDeserializedChildren, bool changeThisLevel)
        {
            /*Location7908*/if ((!exploreOnlyDeserializedChildren && FirstNode != null) || (_FirstNode_Accessed && _FirstNode != null))
            {
                _FirstNode = (LazinatorLinkedListNode<T>) _FirstNode.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, true);
            }
            /*Location7909*/if (changeThisLevel && changeFunc != null)
            {
                return changeFunc(this);
            }
            return this;
        }
        /*Location7910*/
        public virtual void FreeInMemoryObjects()
        {
            _FirstNode = default;
            _FirstNode_Accessed = false;
            IsDirty = false;
            DescendantIsDirty = false;
            HasChanged = false;
            DescendantHasChanged = false;
        }
        /*Location7911*/
        /* Conversion */
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public virtual int LazinatorUniqueID => 225;
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected virtual bool ContainsOpenGenericParameters => true;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public virtual LazinatorGenericIDType LazinatorGenericID => LazinatorGenericIDType.GetCachedForType<LazinatorLinkedList<T>>(() => DeserializationFactory.Instance.GetUniqueIDListForGenericType(225, new Type[] { typeof(T) }));
        
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public virtual int LazinatorObjectVersion { get; set; } = 0;
        
        
        /*Location7912*/public virtual void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
            /*Location7913*/_AllowDuplicates = span.ToBoolean(ref bytesSoFar);
            /*Location7914*/_Count = span.ToDecompressedInt(ref bytesSoFar);
            /*Location7915*/_FirstNode_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            
            /*Location7916*/_LazinatorLinkedList_T_EndByteIndex = bytesSoFar;
            /*Location7917*/        }
            
            /*Location7918*/public virtual void SerializeExistingBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
            {
                /*Location7919*/if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren)
                {
                    updateStoredBuffer = false;
                }
                /*Location7920*/int startPosition = writer.Position;
                WritePropertiesIntoBuffer(ref writer, includeChildrenMode, verifyCleanness, updateStoredBuffer, true);
                /*Location7921*/if (updateStoredBuffer)
                {
                    UpdateStoredBuffer(ref writer, startPosition, writer.Position - startPosition, includeChildrenMode, false);
                    /*Location7922*/}
                    /*Location7923*/}
                    /*Location7924*/
                    public virtual void UpdateStoredBuffer(ref BinaryBufferWriter writer, int startPosition, int length, IncludeChildrenMode includeChildrenMode, bool updateDeserializedChildren)
                    {
                        /*Location7925*/_IsDirty = false;
                        if (includeChildrenMode == IncludeChildrenMode.IncludeAllChildren)
                        {
                            _DescendantIsDirty = false;/*Location7926*/
                            if (updateDeserializedChildren)
                            {
                                UpdateDeserializedChildren(ref writer, startPosition);
                                /*Location7927*/}
                                /*Location7928*//*Location7929*/
                            }
                            else
                            {
                                ThrowHelper.ThrowCannotUpdateStoredBuffer();
                            }
                            /*Location7930*/
                            var newBuffer = writer.Slice(startPosition, length);
                            LazinatorMemoryStorage = newBuffer;
                            /*Location7931*/}
                            /*Location7932*/
                            protected virtual void UpdateDeserializedChildren(ref BinaryBufferWriter writer, int startPosition)
                            {
                                /*Location7933*/if (_FirstNode_Accessed && _FirstNode != null)
                                {
                                    FirstNode.UpdateStoredBuffer(ref writer, startPosition + _FirstNode_ByteIndex + sizeof(int), _FirstNode_ByteLength - sizeof(int), IncludeChildrenMode.IncludeAllChildren, true);
                                }
                                
                                /*Location7934*/}
                                
                                /*Location7935*/
                                protected virtual void WritePropertiesIntoBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer, bool includeUniqueID)
                                {
                                    int startPosition = writer.Position;
                                    int startOfObjectPosition = 0;
                                    // header information
                                    /*Location7936*/if (includeUniqueID)
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
                                    /*Location7937*/CompressedIntegralTypes.WriteCompressedInt(ref writer, Lazinator.Support.LazinatorVersionInfo.LazinatorIntVersion);
                                    CompressedIntegralTypes.WriteCompressedInt(ref writer, LazinatorObjectVersion);
                                    writer.Write((byte)includeChildrenMode);
                                    /*Location7938*/// write properties
                                    /*Location7939*/WriteUncompressedPrimitives.WriteBool(ref writer, _AllowDuplicates);
                                    /*Location7940*/CompressedIntegralTypes.WriteCompressedInt(ref writer, _Count);
                                    /*Location7941*/startOfObjectPosition = writer.Position;
                                    /*Location7942*/if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
                                    {
                                        if ((includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode) && !_FirstNode_Accessed)
                                        {
                                            var deserialized = FirstNode;
                                        }
                                        WriteChild(ref writer, ref _FirstNode, includeChildrenMode, _FirstNode_Accessed, () => GetChildSlice(LazinatorMemoryStorage, _FirstNode_ByteIndex, _FirstNode_ByteLength, false, false, null), verifyCleanness, updateStoredBuffer, false, false, this);
                                    }
                                    
                                    /*Location7943*/if (updateStoredBuffer)
                                    {
                                        _FirstNode_ByteIndex = startOfObjectPosition - startPosition;
                                    }
                                    /*Location7944*/if (updateStoredBuffer)
                                    {
                                        /*Location7945*/_LazinatorLinkedList_T_EndByteIndex = writer.Position - startPosition;
                                        /*Location7946*/}
                                        /*Location7947*/}
                                        /*Location7948*/
                                    }
                                }
