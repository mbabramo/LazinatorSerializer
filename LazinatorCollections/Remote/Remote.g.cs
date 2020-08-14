/*Location7725*//*Location7711*///84860dde-5f17-8999-3ead-6d5dda561d6c
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.380
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

#nullable disable
namespace LazinatorCollections.Remote
{/*Location7712*/
    using Lazinator.Attributes;/*Location7713*/
    using Lazinator.Buffers;/*Location7714*/
    using Lazinator.Core;/*Location7715*/
    using Lazinator.Exceptions;/*Location7716*/
    using Lazinator.Support;/*Location7717*/
    using System;/*Location7718*/
    using System.Buffers;/*Location7719*/
    using System.Collections.Generic;/*Location7720*/
    using System.Diagnostics;/*Location7721*/
    using System.IO;/*Location7722*/
    using System.Linq;/*Location7723*/
    using System.Runtime.InteropServices;/*Location7724*/
    using static Lazinator.Core.LazinatorUtilities;
    
    [Autogenerated]
    public partial class Remote<TKey, TValue> : ILazinator
    {
        /*Location7726*/[DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsStruct => false;
        
        /*Location7727*//* Property definitions */
        
        /*Location7728*/        protected int _Key_ByteIndex;
        /*Location7729*/        protected int _Local_ByteIndex;
        /*Location7730*/protected virtual int _Key_ByteLength => _Local_ByteIndex - _Key_ByteIndex;
        /*Location7731*/private int _Remote_TKey_TValue_EndByteIndex = 0;
        /*Location7732*/protected virtual int _Local_ByteLength => _Remote_TKey_TValue_EndByteIndex - _Local_ByteIndex;
        
        /*Location7733*/
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected bool _StoreLocally;
        public bool StoreLocally
        {
            [DebuggerStepThrough]
            get
            {
                return _StoreLocally;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _StoreLocally = value;
            }
        }
        /*Location7734*/
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected TKey _Key;
        public virtual TKey Key
        {
            [DebuggerStepThrough]
            get
            {
                if (!_Key_Accessed)
                {
                    Lazinate_Key();
                } 
                return _Key;
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
                    if (_Key != null)
                    {
                        _Key.LazinatorParents = _Key.LazinatorParents.WithRemoved(this);
                    }
                    if (value != null)
                    {
                        value.LazinatorParents = value.LazinatorParents.WithAdded(this);
                    }
                }
                
                IsDirty = true;
                DescendantIsDirty = true;
                _Key = value;
                _Key_Accessed = true;
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected bool _Key_Accessed;
        private void Lazinate_Key()
        {
            if (LazinatorObjectBytes.Length == 0)
            {
                _Key = default(TKey);
                if (_Key != null)
                { // Key is a struct
                    _Key.LazinatorParents = new LazinatorParentsCollection(this);
                }
            }
            else
            {
                LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _Key_ByteIndex, _Key_ByteLength, false, false, null);
                
                _Key = DeserializationFactory.Instance.CreateBasedOnType<TKey>(childData, this); 
            }
            
            _Key_Accessed = true;
        }
        
        /*Location7735*/
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected TValue _Local;
        public virtual TValue Local
        {
            [DebuggerStepThrough]
            get
            {
                if (!_Local_Accessed)
                {
                    Lazinate_Local();
                } 
                return _Local;
            }
            [DebuggerStepThrough]
            set
            {
                if (_Local != null)
                {
                    _Local.LazinatorParents = _Local.LazinatorParents.WithRemoved(this);
                }
                if (value != null)
                {
                    value.LazinatorParents = value.LazinatorParents.WithAdded(this);
                }
                
                IsDirty = true;
                DescendantIsDirty = true;
                _Local = value;
                _Local_Accessed = true;
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected bool _Local_Accessed;
        private void Lazinate_Local()
        {
            if (LazinatorObjectBytes.Length == 0)
            {
                _Local = default(TValue);
                if (_Local != null)
                { // Local is a struct
                    _Local.LazinatorParents = new LazinatorParentsCollection(this);
                }
            }
            else
            {
                LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _Local_ByteIndex, _Local_ByteLength, false, false, null);
                
                _Local = DeserializationFactory.Instance.CreateBasedOnType<TValue>(childData, this); 
            }
            
            _Local_Accessed = true;
        }
        
        /*Location7739*/
        /* Serialization, deserialization, and object relationships */
        
        public Remote(LazinatorConstructorEnum constructorEnum)
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
            var clone = new Remote<TKey, TValue>(LazinatorConstructorEnum.LazinatorConstructor)
            {
                OriginalIncludeChildrenMode = includeChildrenMode
            };
            clone = CompleteClone(this, clone, includeChildrenMode, cloneBufferOptions);
            return clone;
        }
        
        public virtual ILazinator AssignCloneProperties(ILazinator clone, IncludeChildrenMode includeChildrenMode)
        {
            clone.FreeInMemoryObjects();
            Remote<TKey, TValue> typedClone = (Remote<TKey, TValue>) clone;
            /*Location7736*/typedClone.StoreLocally = StoreLocally;
            /*Location7737*/if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                if (Key == null)
                {
                    typedClone.Key = default(TKey);
                }
                else
                {
                    typedClone.Key = (TKey) Key.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer);
                }
                
            }
            
            /*Location7738*/if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                if (Local == null)
                {
                    typedClone.Local = default(TValue);
                }
                else
                {
                    typedClone.Local = (TValue) Local.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer);
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
        
        /*Location7740*/
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
        
        /*Location7741*/public virtual IEnumerable<(string propertyName, ILazinator descendant)> EnumerateLazinatorDescendants(Func<ILazinator, bool> matchCriterion, bool stopExploringBelowMatch, Func<ILazinator, bool> exploreCriterion, bool exploreOnlyDeserializedChildren, bool enumerateNulls)
        {
            /*Location7742*/if (enumerateNulls && (!exploreOnlyDeserializedChildren || _Key_Accessed) && Key == null)
            {
                yield return ("Key", default);
            }
            else
            {
                if ((!exploreOnlyDeserializedChildren && Key != null) || (_Key_Accessed && _Key != null))
                {
                    bool isMatch_Key = matchCriterion == null || matchCriterion(Key);
                    bool shouldExplore_Key = exploreCriterion == null || exploreCriterion(Key);
                    if (isMatch_Key)
                    {
                        yield return ("Key", Key);
                    }
                    if ((!stopExploringBelowMatch || !isMatch_Key) && shouldExplore_Key)
                    {
                        foreach (var toYield in Key.EnumerateLazinatorDescendants(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
                        {
                            yield return ("Key" + "." + toYield.propertyName, toYield.descendant);
                        }
                    }
                }
                
            }
            
            /*Location7743*/if (enumerateNulls && (!exploreOnlyDeserializedChildren || _Local_Accessed) && Local == null)
            {
                yield return ("Local", default);
            }
            else
            {
                if ((!exploreOnlyDeserializedChildren && Local != null) || (_Local_Accessed && _Local != null))
                {
                    bool isMatch_Local = matchCriterion == null || matchCriterion(Local);
                    bool shouldExplore_Local = exploreCriterion == null || exploreCriterion(Local);
                    if (isMatch_Local)
                    {
                        yield return ("Local", Local);
                    }
                    if ((!stopExploringBelowMatch || !isMatch_Local) && shouldExplore_Local)
                    {
                        foreach (var toYield in Local.EnumerateLazinatorDescendants(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
                        {
                            yield return ("Local" + "." + toYield.propertyName, toYield.descendant);
                        }
                    }
                }
                
            }
            
            /*Location7744*/yield break;
        }
        /*Location7745*/
        
        public virtual IEnumerable<(string propertyName, object descendant)> EnumerateNonLazinatorProperties()
        {
            /*Location7746*/yield return ("StoreLocally", (object)StoreLocally);
            /*Location7747*/yield break;
        }
        /*Location7748*/
        public virtual ILazinator ForEachLazinator(Func<ILazinator, ILazinator> changeFunc, bool exploreOnlyDeserializedChildren, bool changeThisLevel)
        {
            /*Location7749*/if ((!exploreOnlyDeserializedChildren && Key != null) || (_Key_Accessed && _Key != null))
            {
                _Key = (TKey) _Key.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, true);
            }
            /*Location7750*/if ((!exploreOnlyDeserializedChildren && Local != null) || (_Local_Accessed && _Local != null))
            {
                _Local = (TValue) _Local.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, true);
            }
            /*Location7751*/if (changeThisLevel && changeFunc != null)
            {
                return changeFunc(this);
            }
            return this;
        }
        /*Location7752*/
        public virtual void FreeInMemoryObjects()
        {
            _Key = default;
            _Local = default;
            _Key_Accessed = _Local_Accessed = false;
            IsDirty = false;
            DescendantIsDirty = false;
            HasChanged = false;
            DescendantHasChanged = false;
        }
        /*Location7753*/
        /* Conversion */
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public virtual int LazinatorUniqueID => 254;
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected virtual bool ContainsOpenGenericParameters => true;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public virtual LazinatorGenericIDType LazinatorGenericID => LazinatorGenericIDType.GetCachedForType<Remote<TKey, TValue>>(() => DeserializationFactory.Instance.GetUniqueIDListForGenericType(254, new Type[] { typeof(TKey), typeof(TValue) }));
        
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public virtual int LazinatorObjectVersion { get; set; } = 0;
        
        
        /*Location7754*/public virtual void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
            /*Location7755*/_StoreLocally = span.ToBoolean(ref bytesSoFar);
            /*Location7756*/_Key_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            
            /*Location7757*/_Local_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            
            /*Location7758*/_Remote_TKey_TValue_EndByteIndex = bytesSoFar;
            /*Location7759*/        }
            
            /*Location7760*/public virtual void SerializeExistingBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
            {
                /*Location7761*/if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren)
                {
                    updateStoredBuffer = false;
                }
                /*Location7762*/int startPosition = writer.Position;
                WritePropertiesIntoBuffer(ref writer, includeChildrenMode, verifyCleanness, updateStoredBuffer, true);
                /*Location7763*/if (updateStoredBuffer)
                {
                    UpdateStoredBuffer(ref writer, startPosition, writer.Position - startPosition, includeChildrenMode, false);
                    /*Location7764*/}
                    /*Location7765*/}
                    /*Location7766*/
                    public virtual void UpdateStoredBuffer(ref BinaryBufferWriter writer, int startPosition, int length, IncludeChildrenMode includeChildrenMode, bool updateDeserializedChildren)
                    {
                        /*Location7767*/_IsDirty = false;
                        if (includeChildrenMode == IncludeChildrenMode.IncludeAllChildren)
                        {
                            _DescendantIsDirty = false;/*Location7768*/
                            if (updateDeserializedChildren)
                            {
                                UpdateDeserializedChildren(ref writer, startPosition);
                                /*Location7769*/}
                                /*Location7770*/
                                if (_Key_Accessed && _Key != null && _Key.IsStruct && (_Key.IsDirty || _Key.DescendantIsDirty))
                                {
                                    _Key_Accessed = false;
                                }
                                if (_Local_Accessed && _Local != null && _Local.IsStruct && (_Local.IsDirty || _Local.DescendantIsDirty))
                                {
                                    _Local_Accessed = false;
                                }/*Location7771*/
                            }
                            else
                            {
                                ThrowHelper.ThrowCannotUpdateStoredBuffer();
                            }
                            /*Location7772*/
                            var newBuffer = writer.Slice(startPosition, length);
                            LazinatorMemoryStorage = newBuffer;
                            /*Location7773*/}
                            /*Location7774*/
                            protected virtual void UpdateDeserializedChildren(ref BinaryBufferWriter writer, int startPosition)
                            {
                                /*Location7775*/if (_Key_Accessed && _Key != null)
                                {
                                    Key.UpdateStoredBuffer(ref writer, startPosition + _Key_ByteIndex + sizeof(int), _Key_ByteLength - sizeof(int), IncludeChildrenMode.IncludeAllChildren, true);
                                }
                                
                                /*Location7776*/if (_Local_Accessed && _Local != null)
                                {
                                    Local.UpdateStoredBuffer(ref writer, startPosition + _Local_ByteIndex + sizeof(int), _Local_ByteLength - sizeof(int), IncludeChildrenMode.IncludeAllChildren, true);
                                }
                                
                                /*Location7777*/}
                                
                                /*Location7778*/
                                protected virtual void WritePropertiesIntoBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer, bool includeUniqueID)
                                {
                                    int startPosition = writer.Position;
                                    int startOfObjectPosition = 0;
                                    // header information
                                    /*Location7779*/if (includeUniqueID)
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
                                    /*Location7780*/CompressedIntegralTypes.WriteCompressedInt(ref writer, Lazinator.Support.LazinatorVersionInfo.LazinatorIntVersion);
                                    CompressedIntegralTypes.WriteCompressedInt(ref writer, LazinatorObjectVersion);
                                    writer.Write((byte)includeChildrenMode);
                                    /*Location7781*/// write properties
                                    /*Location7782*/WriteUncompressedPrimitives.WriteBool(ref writer, _StoreLocally);
                                    /*Location7783*/startOfObjectPosition = writer.Position;
                                    /*Location7784*/if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
                                    {
                                        if ((includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode) && !_Key_Accessed)
                                        {
                                            var deserialized = Key;
                                        }
                                        WriteChild(ref writer, ref _Key, includeChildrenMode, _Key_Accessed, () => GetChildSlice(LazinatorMemoryStorage, _Key_ByteIndex, _Key_ByteLength, false, false, null), verifyCleanness, updateStoredBuffer, false, false, this);
                                    }
                                    
                                    /*Location7785*/if (updateStoredBuffer)
                                    {
                                        _Key_ByteIndex = startOfObjectPosition - startPosition;
                                    }
                                    /*Location7786*/startOfObjectPosition = writer.Position;
                                    /*Location7787*/if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
                                    {
                                        if ((includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode) && !_Local_Accessed)
                                        {
                                            var deserialized = Local;
                                        }
                                        WriteChild(ref writer, ref _Local, includeChildrenMode, _Local_Accessed, () => GetChildSlice(LazinatorMemoryStorage, _Local_ByteIndex, _Local_ByteLength, false, false, null), verifyCleanness, updateStoredBuffer, false, false, this);
                                    }
                                    
                                    /*Location7788*/if (updateStoredBuffer)
                                    {
                                        _Local_ByteIndex = startOfObjectPosition - startPosition;
                                    }
                                    /*Location7789*/if (updateStoredBuffer)
                                    {
                                        /*Location7790*/_Remote_TKey_TValue_EndByteIndex = writer.Position - startPosition;
                                        /*Location7791*/}
                                        /*Location7792*/}
                                        /*Location7793*/
                                    }
                                }
