/*Location9859*//*Location9845*///f8675080-e3e2-ddb1-a44b-686d4f41d33b
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.380
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

#nullable disable
namespace LazinatorCollections.Tuples
{/*Location9846*/
    using Lazinator.Attributes;/*Location9847*/
    using Lazinator.Buffers;/*Location9848*/
    using Lazinator.Core;/*Location9849*/
    using Lazinator.Exceptions;/*Location9850*/
    using Lazinator.Support;/*Location9851*/
    using System;/*Location9852*/
    using System.Buffers;/*Location9853*/
    using System.Collections.Generic;/*Location9854*/
    using System.Diagnostics;/*Location9855*/
    using System.IO;/*Location9856*/
    using System.Linq;/*Location9857*/
    using System.Runtime.InteropServices;/*Location9858*/
    using static Lazinator.Core.LazinatorUtilities;
    
    [Autogenerated]
    public partial class LazinatorTuple<T, U> : ILazinator
    {
        /*Location9860*/[DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsStruct => false;
        
        /*Location9861*//* Property definitions */
        
        /*Location9862*/        protected int _Item1_ByteIndex;
        /*Location9863*/        protected int _Item2_ByteIndex;
        /*Location9864*/protected virtual int _Item1_ByteLength => _Item2_ByteIndex - _Item1_ByteIndex;
        /*Location9865*/private int _LazinatorTuple_T_U_EndByteIndex = 0;
        /*Location9866*/protected virtual int _Item2_ByteLength => _LazinatorTuple_T_U_EndByteIndex - _Item2_ByteIndex;
        
        /*Location9867*/
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected T _Item1;
        public virtual T Item1
        {
            [DebuggerStepThrough]
            get
            {
                if (!_Item1_Accessed)
                {
                    Lazinate_Item1();
                } 
                return _Item1;
            }
            [DebuggerStepThrough]
            set
            {
                if (value != null && value.IsStruct)
                {
                    value.LazinatorParents = new LazinatorParentsCollection(this);
                }
                else
                {
                    if (_Item1 != null)
                    {
                        _Item1.LazinatorParents = _Item1.LazinatorParents.WithRemoved(this);
                    }
                    if (value != null)
                    {
                        value.LazinatorParents = value.LazinatorParents.WithAdded(this);
                    }
                }
                
                IsDirty = true;
                DescendantIsDirty = true;
                _Item1 = value;
                _Item1_Accessed = true;
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected bool _Item1_Accessed;
        private void Lazinate_Item1()
        {
            if (LazinatorObjectBytes.Length == 0)
            {
                _Item1 = default(T);
                if (_Item1 != null)
                { // Item1 is a struct
                    _Item1.LazinatorParents = new LazinatorParentsCollection(this);
                }
            }
            else
            {
                LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _Item1_ByteIndex, _Item1_ByteLength, false, false, null);
                
                _Item1 = DeserializationFactory.Instance.CreateBasedOnType<T>(childData, this); 
            }
            
            _Item1_Accessed = true;
        }
        
        /*Location9868*/
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected U _Item2;
        public virtual U Item2
        {
            [DebuggerStepThrough]
            get
            {
                if (!_Item2_Accessed)
                {
                    Lazinate_Item2();
                } 
                return _Item2;
            }
            [DebuggerStepThrough]
            set
            {
                if (value != null && value.IsStruct)
                {
                    value.LazinatorParents = new LazinatorParentsCollection(this);
                }
                else
                {
                    if (_Item2 != null)
                    {
                        _Item2.LazinatorParents = _Item2.LazinatorParents.WithRemoved(this);
                    }
                    if (value != null)
                    {
                        value.LazinatorParents = value.LazinatorParents.WithAdded(this);
                    }
                }
                
                IsDirty = true;
                DescendantIsDirty = true;
                _Item2 = value;
                _Item2_Accessed = true;
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected bool _Item2_Accessed;
        private void Lazinate_Item2()
        {
            if (LazinatorObjectBytes.Length == 0)
            {
                _Item2 = default(U);
                if (_Item2 != null)
                { // Item2 is a struct
                    _Item2.LazinatorParents = new LazinatorParentsCollection(this);
                }
            }
            else
            {
                LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _Item2_ByteIndex, _Item2_ByteLength, false, false, null);
                
                _Item2 = DeserializationFactory.Instance.CreateBasedOnType<U>(childData, this); 
            }
            
            _Item2_Accessed = true;
        }
        
        /*Location9871*/
        /* Serialization, deserialization, and object relationships */
        
        public LazinatorTuple(IncludeChildrenMode originalIncludeChildrenMode)
        {
            OriginalIncludeChildrenMode = originalIncludeChildrenMode;
        }
        
        public LazinatorTuple(LazinatorMemory serializedBytes, ILazinator parent = null)
        {
            LazinatorParents = new LazinatorParentsCollection(parent);
            DeserializeLazinator(serializedBytes);
            HasChanged = false;
            DescendantHasChanged = false;
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
            LazinatorTuple<T, U> clone;
            if (cloneBufferOptions == CloneBufferOptions.NoBuffer)
            {
                clone = new LazinatorTuple<T, U>(includeChildrenMode);
                clone.LazinatorObjectVersion = LazinatorObjectVersion;
                clone = (LazinatorTuple<T, U>)AssignCloneProperties(clone, includeChildrenMode);
            }
            else
            {
                LazinatorMemory bytes = EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, false, IsDirty, DescendantIsDirty, false, LazinatorMemoryStorage, false, this);
                clone = new LazinatorTuple<T, U>(bytes);
            }
            return clone;
        }
        
        public virtual ILazinator AssignCloneProperties(ILazinator clone, IncludeChildrenMode includeChildrenMode)
        {
            clone.FreeInMemoryObjects();
            LazinatorTuple<T, U> typedClone = (LazinatorTuple<T, U>) clone;
            /*Location9869*/if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                if (Item1 == null)
                {
                    typedClone.Item1 = default(T);
                }
                else
                {
                    typedClone.Item1 = (T) Item1.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer);
                }
                
            }
            
            /*Location9870*/if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                if (Item2 == null)
                {
                    typedClone.Item2 = default(U);
                }
                else
                {
                    typedClone.Item2 = (U) Item2.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer);
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
        
        /*Location9872*/
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
        
        /*Location9873*/public virtual IEnumerable<(string propertyName, ILazinator descendant)> EnumerateLazinatorDescendants(Func<ILazinator, bool> matchCriterion, bool stopExploringBelowMatch, Func<ILazinator, bool> exploreCriterion, bool exploreOnlyDeserializedChildren, bool enumerateNulls)
        {
            /*Location9874*/if (enumerateNulls && (!exploreOnlyDeserializedChildren || _Item1_Accessed) && Item1 == null)
            {
                yield return ("Item1", default);
            }
            else
            {
                if ((!exploreOnlyDeserializedChildren && Item1 != null) || (_Item1_Accessed && _Item1 != null))
                {
                    bool isMatch_Item1 = matchCriterion == null || matchCriterion(Item1);
                    bool shouldExplore_Item1 = exploreCriterion == null || exploreCriterion(Item1);
                    if (isMatch_Item1)
                    {
                        yield return ("Item1", Item1);
                    }
                    if ((!stopExploringBelowMatch || !isMatch_Item1) && shouldExplore_Item1)
                    {
                        foreach (var toYield in Item1.EnumerateLazinatorDescendants(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
                        {
                            yield return ("Item1" + "." + toYield.propertyName, toYield.descendant);
                        }
                    }
                }
                
            }
            
            /*Location9875*/if (enumerateNulls && (!exploreOnlyDeserializedChildren || _Item2_Accessed) && Item2 == null)
            {
                yield return ("Item2", default);
            }
            else
            {
                if ((!exploreOnlyDeserializedChildren && Item2 != null) || (_Item2_Accessed && _Item2 != null))
                {
                    bool isMatch_Item2 = matchCriterion == null || matchCriterion(Item2);
                    bool shouldExplore_Item2 = exploreCriterion == null || exploreCriterion(Item2);
                    if (isMatch_Item2)
                    {
                        yield return ("Item2", Item2);
                    }
                    if ((!stopExploringBelowMatch || !isMatch_Item2) && shouldExplore_Item2)
                    {
                        foreach (var toYield in Item2.EnumerateLazinatorDescendants(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
                        {
                            yield return ("Item2" + "." + toYield.propertyName, toYield.descendant);
                        }
                    }
                }
                
            }
            
            /*Location9876*/yield break;
        }
        /*Location9877*/
        
        public virtual IEnumerable<(string propertyName, object descendant)> EnumerateNonLazinatorProperties()
        {
            /*Location9878*/yield break;
        }
        /*Location9879*/
        public virtual ILazinator ForEachLazinator(Func<ILazinator, ILazinator> changeFunc, bool exploreOnlyDeserializedChildren, bool changeThisLevel)
        {
            /*Location9880*/if ((!exploreOnlyDeserializedChildren && Item1 != null) || (_Item1_Accessed && _Item1 != null))
            {
                _Item1 = (T) _Item1.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, true);
            }
            /*Location9881*/if ((!exploreOnlyDeserializedChildren && Item2 != null) || (_Item2_Accessed && _Item2 != null))
            {
                _Item2 = (U) _Item2.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, true);
            }
            /*Location9882*/if (changeThisLevel && changeFunc != null)
            {
                return changeFunc(this);
            }
            return this;
        }
        /*Location9883*/
        public virtual void FreeInMemoryObjects()
        {
            _Item1 = default;
            _Item2 = default;
            _Item1_Accessed = _Item2_Accessed = false;
            IsDirty = false;
            DescendantIsDirty = false;
            HasChanged = false;
            DescendantHasChanged = false;
        }
        /*Location9884*/
        /* Conversion */
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public virtual int LazinatorUniqueID => 205;
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected virtual bool ContainsOpenGenericParameters => true;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public virtual LazinatorGenericIDType LazinatorGenericID => LazinatorGenericIDType.GetCachedForType<LazinatorTuple<T, U>>(() => DeserializationFactory.Instance.GetUniqueIDListForGenericType(205, new Type[] { typeof(T), typeof(U) }));
        
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public virtual int LazinatorObjectVersion { get; set; } = 0;
        
        
        /*Location9885*/public virtual void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
            /*Location9886*/_Item1_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            
            /*Location9887*/_Item2_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            
            /*Location9888*/_LazinatorTuple_T_U_EndByteIndex = bytesSoFar;
            /*Location9889*/        }
            
            /*Location9890*/public virtual void SerializeExistingBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
            {
                /*Location9891*/if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren)
                {
                    updateStoredBuffer = false;
                }
                /*Location9892*/int startPosition = writer.Position;
                WritePropertiesIntoBuffer(ref writer, includeChildrenMode, verifyCleanness, updateStoredBuffer, true);
                /*Location9893*/if (updateStoredBuffer)
                {
                    UpdateStoredBuffer(ref writer, startPosition, writer.Position - startPosition, includeChildrenMode, false);
                    /*Location9894*/}
                    /*Location9895*/}
                    /*Location9896*/
                    public virtual void UpdateStoredBuffer(ref BinaryBufferWriter writer, int startPosition, int length, IncludeChildrenMode includeChildrenMode, bool updateDeserializedChildren)
                    {
                        /*Location9897*/_IsDirty = false;
                        if (includeChildrenMode == IncludeChildrenMode.IncludeAllChildren)
                        {
                            _DescendantIsDirty = false;/*Location9898*/
                            if (updateDeserializedChildren)
                            {
                                UpdateDeserializedChildren(ref writer, startPosition);
                                /*Location9899*/}
                                /*Location9900*/
                                if (_Item1_Accessed && _Item1 != null && _Item1.IsStruct && (_Item1.IsDirty || _Item1.DescendantIsDirty))
                                {
                                    _Item1_Accessed = false;
                                }
                                if (_Item2_Accessed && _Item2 != null && _Item2.IsStruct && (_Item2.IsDirty || _Item2.DescendantIsDirty))
                                {
                                    _Item2_Accessed = false;
                                }/*Location9901*/
                            }
                            else
                            {
                                ThrowHelper.ThrowCannotUpdateStoredBuffer();
                            }
                            /*Location9902*/
                            var newBuffer = writer.Slice(startPosition, length);
                            LazinatorMemoryStorage = newBuffer;
                            /*Location9903*/}
                            /*Location9904*/
                            protected virtual void UpdateDeserializedChildren(ref BinaryBufferWriter writer, int startPosition)
                            {
                                /*Location9905*/if (_Item1_Accessed && _Item1 != null)
                                {
                                    Item1.UpdateStoredBuffer(ref writer, startPosition + _Item1_ByteIndex + sizeof(int), _Item1_ByteLength - sizeof(int), IncludeChildrenMode.IncludeAllChildren, true);
                                }
                                
                                /*Location9906*/if (_Item2_Accessed && _Item2 != null)
                                {
                                    Item2.UpdateStoredBuffer(ref writer, startPosition + _Item2_ByteIndex + sizeof(int), _Item2_ByteLength - sizeof(int), IncludeChildrenMode.IncludeAllChildren, true);
                                }
                                
                                /*Location9907*/}
                                
                                /*Location9908*/
                                protected virtual void WritePropertiesIntoBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer, bool includeUniqueID)
                                {
                                    int startPosition = writer.Position;
                                    int startOfObjectPosition = 0;
                                    // header information
                                    /*Location9909*/if (includeUniqueID)
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
                                    /*Location9910*/CompressedIntegralTypes.WriteCompressedInt(ref writer, Lazinator.Support.LazinatorVersionInfo.LazinatorIntVersion);
                                    CompressedIntegralTypes.WriteCompressedInt(ref writer, LazinatorObjectVersion);
                                    writer.Write((byte)includeChildrenMode);
                                    /*Location9911*/// write properties
                                    /*Location9912*/startOfObjectPosition = writer.Position;
                                    /*Location9913*/if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
                                    {
                                        if ((includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode) && !_Item1_Accessed)
                                        {
                                            var deserialized = Item1;
                                        }
                                        WriteChild(ref writer, ref _Item1, includeChildrenMode, _Item1_Accessed, () => GetChildSlice(LazinatorMemoryStorage, _Item1_ByteIndex, _Item1_ByteLength, false, false, null), verifyCleanness, updateStoredBuffer, false, false, this);
                                    }
                                    
                                    /*Location9914*/if (updateStoredBuffer)
                                    {
                                        _Item1_ByteIndex = startOfObjectPosition - startPosition;
                                    }
                                    /*Location9915*/startOfObjectPosition = writer.Position;
                                    /*Location9916*/if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
                                    {
                                        if ((includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode) && !_Item2_Accessed)
                                        {
                                            var deserialized = Item2;
                                        }
                                        WriteChild(ref writer, ref _Item2, includeChildrenMode, _Item2_Accessed, () => GetChildSlice(LazinatorMemoryStorage, _Item2_ByteIndex, _Item2_ByteLength, false, false, null), verifyCleanness, updateStoredBuffer, false, false, this);
                                    }
                                    
                                    /*Location9917*/if (updateStoredBuffer)
                                    {
                                        _Item2_ByteIndex = startOfObjectPosition - startPosition;
                                    }
                                    /*Location9918*/if (updateStoredBuffer)
                                    {
                                        /*Location9919*/_LazinatorTuple_T_U_EndByteIndex = writer.Position - startPosition;
                                        /*Location9920*/}
                                        /*Location9921*/}
                                        /*Location9922*/
                                    }
                                }
