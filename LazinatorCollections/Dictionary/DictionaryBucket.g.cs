/*Location9703*//*Location9688*///38c76b1b-020f-46ac-db22-290e61551795
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.380
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

#nullable disable
namespace LazinatorCollections.Dictionary
{/*Location9689*/
    using Lazinator.Attributes;/*Location9690*/
    using Lazinator.Buffers;/*Location9691*/
    using Lazinator.Core;/*Location9692*/
    using Lazinator.Exceptions;/*Location9693*/
    using Lazinator.Support;/*Location9694*/
    using LazinatorCollections;/*Location9695*/
    using System;/*Location9696*/
    using System.Buffers;/*Location9697*/
    using System.Collections.Generic;/*Location9698*/
    using System.Diagnostics;/*Location9699*/
    using System.IO;/*Location9700*/
    using System.Linq;/*Location9701*/
    using System.Runtime.InteropServices;/*Location9702*/
    using static Lazinator.Core.LazinatorUtilities;
    
    [Autogenerated]
    public partial class DictionaryBucket<TKey, TValue> : ILazinator
    {
        /*Location9704*/[DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsStruct => false;
        
        /*Location9705*//* Property definitions */
        
        /*Location9706*/        protected int _Keys_ByteIndex;
        /*Location9707*/        protected int _Values_ByteIndex;
        /*Location9708*/protected virtual int _Keys_ByteLength => _Values_ByteIndex - _Keys_ByteIndex;
        /*Location9709*/private int _DictionaryBucket_TKey_TValue_EndByteIndex;
        /*Location9710*/protected virtual int _Values_ByteLength => _DictionaryBucket_TKey_TValue_EndByteIndex - _Values_ByteIndex;
        
        /*Location9711*/
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected bool _Initialized;
        public bool Initialized
        {
            [DebuggerStepThrough]
            get
            {
                return _Initialized;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _Initialized = value;
            }
        }
        /*Location9712*/
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected LazinatorList<TKey> _Keys;
        public virtual LazinatorList<TKey> Keys
        {
            [DebuggerStepThrough]
            get
            {
                if (!_Keys_Accessed)
                {
                    Lazinate_Keys();
                } 
                return _Keys;
            }
            [DebuggerStepThrough]
            set
            {
                if (_Keys != null)
                {
                    _Keys.LazinatorParents = _Keys.LazinatorParents.WithRemoved(this);
                }
                if (value != null)
                {
                    value.LazinatorParents = value.LazinatorParents.WithAdded(this);
                }
                
                IsDirty = true;
                DescendantIsDirty = true;
                _Keys = value;
                _Keys_Accessed = true;
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected bool _Keys_Accessed;
        private void Lazinate_Keys()
        {
            if (LazinatorObjectBytes.Length == 0)
            {
                _Keys = null;
            }
            else
            {
                LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _Keys_ByteIndex, _Keys_ByteLength, false, false, null);
                
                _Keys = DeserializationFactory.Instance.CreateBaseOrDerivedType(201, (c, p) => new LazinatorList<TKey>(c, p), childData, this); 
            }
            
            _Keys_Accessed = true;
        }
        
        /*Location9713*/
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected LazinatorList<TValue> _Values;
        public virtual LazinatorList<TValue> Values
        {
            [DebuggerStepThrough]
            get
            {
                if (!_Values_Accessed)
                {
                    Lazinate_Values();
                } 
                return _Values;
            }
            [DebuggerStepThrough]
            set
            {
                if (_Values != null)
                {
                    _Values.LazinatorParents = _Values.LazinatorParents.WithRemoved(this);
                }
                if (value != null)
                {
                    value.LazinatorParents = value.LazinatorParents.WithAdded(this);
                }
                
                IsDirty = true;
                DescendantIsDirty = true;
                _Values = value;
                _Values_Accessed = true;
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected bool _Values_Accessed;
        private void Lazinate_Values()
        {
            if (LazinatorObjectBytes.Length == 0)
            {
                _Values = null;
            }
            else
            {
                LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _Values_ByteIndex, _Values_ByteLength, false, false, null);
                
                _Values = DeserializationFactory.Instance.CreateBaseOrDerivedType(201, (c, p) => new LazinatorList<TValue>(c, p), childData, this); 
            }
            
            _Values_Accessed = true;
        }
        
        /*Location9717*/
        /* Serialization, deserialization, and object relationships */
        
        public DictionaryBucket(IncludeChildrenMode originalIncludeChildrenMode)
        {
            OriginalIncludeChildrenMode = originalIncludeChildrenMode;
        }
        
        public DictionaryBucket(LazinatorMemory serializedBytes, ILazinator parent = null)
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
            DictionaryBucket<TKey, TValue> clone;
            if (cloneBufferOptions == CloneBufferOptions.NoBuffer)
            {
                clone = new DictionaryBucket<TKey, TValue>(includeChildrenMode);
                clone.LazinatorObjectVersion = LazinatorObjectVersion;
                clone = (DictionaryBucket<TKey, TValue>)AssignCloneProperties(clone, includeChildrenMode);
            }
            else
            {
                LazinatorMemory bytes = EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, false, IsDirty, DescendantIsDirty, false, LazinatorMemoryStorage, false, this);
                clone = new DictionaryBucket<TKey, TValue>(bytes);
            }
            return clone;
        }
        
        public virtual ILazinator AssignCloneProperties(ILazinator clone, IncludeChildrenMode includeChildrenMode)
        {
            clone.FreeInMemoryObjects();
            DictionaryBucket<TKey, TValue> typedClone = (DictionaryBucket<TKey, TValue>) clone;
            /*Location9714*/typedClone.Initialized = Initialized;
            /*Location9715*/if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                if (Keys == null)
                {
                    typedClone.Keys = null;
                }
                else
                {
                    typedClone.Keys = (LazinatorList<TKey>) Keys.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer);
                }
                
            }
            
            /*Location9716*/if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                if (Values == null)
                {
                    typedClone.Values = null;
                }
                else
                {
                    typedClone.Values = (LazinatorList<TValue>) Values.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer);
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
        
        /*Location9718*/
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
        
        /*Location9719*/public virtual IEnumerable<(string propertyName, ILazinator descendant)> EnumerateLazinatorDescendants(Func<ILazinator, bool> matchCriterion, bool stopExploringBelowMatch, Func<ILazinator, bool> exploreCriterion, bool exploreOnlyDeserializedChildren, bool enumerateNulls)
        {
            /*Location9720*/if (enumerateNulls && (!exploreOnlyDeserializedChildren || _Keys_Accessed) && Keys == null)
            {
                yield return ("Keys", default);
            }
            else
            {
                if ((!exploreOnlyDeserializedChildren && Keys != null) || (_Keys_Accessed && _Keys != null))
                {
                    bool isMatch_Keys = matchCriterion == null || matchCriterion(Keys);
                    bool shouldExplore_Keys = exploreCriterion == null || exploreCriterion(Keys);
                    if (isMatch_Keys)
                    {
                        yield return ("Keys", Keys);
                    }
                    if ((!stopExploringBelowMatch || !isMatch_Keys) && shouldExplore_Keys)
                    {
                        foreach (var toYield in Keys.EnumerateLazinatorDescendants(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
                        {
                            yield return ("Keys" + "." + toYield.propertyName, toYield.descendant);
                        }
                    }
                }
                
            }
            
            /*Location9721*/if (enumerateNulls && (!exploreOnlyDeserializedChildren || _Values_Accessed) && Values == null)
            {
                yield return ("Values", default);
            }
            else
            {
                if ((!exploreOnlyDeserializedChildren && Values != null) || (_Values_Accessed && _Values != null))
                {
                    bool isMatch_Values = matchCriterion == null || matchCriterion(Values);
                    bool shouldExplore_Values = exploreCriterion == null || exploreCriterion(Values);
                    if (isMatch_Values)
                    {
                        yield return ("Values", Values);
                    }
                    if ((!stopExploringBelowMatch || !isMatch_Values) && shouldExplore_Values)
                    {
                        foreach (var toYield in Values.EnumerateLazinatorDescendants(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
                        {
                            yield return ("Values" + "." + toYield.propertyName, toYield.descendant);
                        }
                    }
                }
                
            }
            
            /*Location9722*/yield break;
        }
        /*Location9723*/
        
        public virtual IEnumerable<(string propertyName, object descendant)> EnumerateNonLazinatorProperties()
        {
            /*Location9724*/yield return ("Initialized", (object)Initialized);
            /*Location9725*/yield break;
        }
        /*Location9726*/
        public virtual ILazinator ForEachLazinator(Func<ILazinator, ILazinator> changeFunc, bool exploreOnlyDeserializedChildren, bool changeThisLevel)
        {
            /*Location9727*/if ((!exploreOnlyDeserializedChildren && Keys != null) || (_Keys_Accessed && _Keys != null))
            {
                _Keys = (LazinatorList<TKey>) _Keys.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, true);
            }
            /*Location9728*/if ((!exploreOnlyDeserializedChildren && Values != null) || (_Values_Accessed && _Values != null))
            {
                _Values = (LazinatorList<TValue>) _Values.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, true);
            }
            /*Location9729*/if (changeThisLevel && changeFunc != null)
            {
                return changeFunc(this);
            }
            return this;
        }
        /*Location9730*/
        public virtual void FreeInMemoryObjects()
        {
            _Keys = default;
            _Values = default;
            _Keys_Accessed = _Values_Accessed = false;
            IsDirty = false;
            DescendantIsDirty = false;
            HasChanged = false;
            DescendantHasChanged = false;
        }
        /*Location9731*/
        /* Conversion */
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public virtual int LazinatorUniqueID => 210;
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected virtual bool ContainsOpenGenericParameters => true;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public virtual LazinatorGenericIDType LazinatorGenericID => LazinatorGenericIDType.GetCachedForType<DictionaryBucket<TKey, TValue>>(() => DeserializationFactory.Instance.GetUniqueIDListForGenericType(210, new Type[] { typeof(TKey), typeof(TValue) }));
        
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public virtual int LazinatorObjectVersion { get; set; } = 0;
        
        
        /*Location9732*/public virtual void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
            /*Location9733*/_Initialized = span.ToBoolean(ref bytesSoFar);
            /*Location9734*/_Keys_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            
            /*Location9735*/_Values_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            
            /*Location9736*/_DictionaryBucket_TKey_TValue_EndByteIndex = bytesSoFar;
            /*Location9737*/        }
            
            /*Location9738*/public virtual void SerializeExistingBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
            {
                /*Location9739*/if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren)
                {
                    updateStoredBuffer = false;
                }
                /*Location9740*/int startPosition = writer.Position;
                WritePropertiesIntoBuffer(ref writer, includeChildrenMode, verifyCleanness, updateStoredBuffer, true);
                /*Location9741*/if (updateStoredBuffer)
                {
                    UpdateStoredBuffer(ref writer, startPosition, writer.Position - startPosition, includeChildrenMode, false);
                    /*Location9742*/}
                    /*Location9743*/}
                    /*Location9744*/
                    public virtual void UpdateStoredBuffer(ref BinaryBufferWriter writer, int startPosition, int length, IncludeChildrenMode includeChildrenMode, bool updateDeserializedChildren)
                    {
                        /*Location9745*/_IsDirty = false;
                        if (includeChildrenMode == IncludeChildrenMode.IncludeAllChildren)
                        {
                            _DescendantIsDirty = false;/*Location9746*/
                            if (updateDeserializedChildren)
                            {
                                UpdateDeserializedChildren(ref writer, startPosition);
                                /*Location9747*/}
                                /*Location9748*//*Location9749*/
                            }
                            else
                            {
                                ThrowHelper.ThrowCannotUpdateStoredBuffer();
                            }
                            /*Location9750*/
                            var newBuffer = writer.Slice(startPosition, length);
                            LazinatorMemoryStorage = newBuffer;
                            /*Location9751*/}
                            /*Location9752*/
                            protected virtual void UpdateDeserializedChildren(ref BinaryBufferWriter writer, int startPosition)
                            {
                                /*Location9753*/if (_Keys_Accessed && _Keys != null)
                                {
                                    Keys.UpdateStoredBuffer(ref writer, startPosition + _Keys_ByteIndex + sizeof(int), _Keys_ByteLength - sizeof(int), IncludeChildrenMode.IncludeAllChildren, true);
                                }
                                
                                /*Location9754*/if (_Values_Accessed && _Values != null)
                                {
                                    Values.UpdateStoredBuffer(ref writer, startPosition + _Values_ByteIndex + sizeof(int), _Values_ByteLength - sizeof(int), IncludeChildrenMode.IncludeAllChildren, true);
                                }
                                
                                /*Location9755*/}
                                
                                /*Location9756*/
                                protected virtual void WritePropertiesIntoBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer, bool includeUniqueID)
                                {
                                    int startPosition = writer.Position;
                                    int startOfObjectPosition = 0;
                                    // header information
                                    /*Location9757*/if (includeUniqueID)
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
                                    /*Location9758*/CompressedIntegralTypes.WriteCompressedInt(ref writer, Lazinator.Support.LazinatorVersionInfo.LazinatorIntVersion);
                                    CompressedIntegralTypes.WriteCompressedInt(ref writer, LazinatorObjectVersion);
                                    writer.Write((byte)includeChildrenMode);
                                    /*Location9759*/// write properties
                                    /*Location9760*/WriteUncompressedPrimitives.WriteBool(ref writer, _Initialized);
                                    /*Location9761*/startOfObjectPosition = writer.Position;
                                    /*Location9762*/if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
                                    {
                                        if ((includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode) && !_Keys_Accessed)
                                        {
                                            var deserialized = Keys;
                                        }
                                        WriteChild(ref writer, ref _Keys, includeChildrenMode, _Keys_Accessed, () => GetChildSlice(LazinatorMemoryStorage, _Keys_ByteIndex, _Keys_ByteLength, false, false, null), verifyCleanness, updateStoredBuffer, false, false, this);
                                    }
                                    
                                    /*Location9763*/if (updateStoredBuffer)
                                    {
                                        _Keys_ByteIndex = startOfObjectPosition - startPosition;
                                    }
                                    /*Location9764*/startOfObjectPosition = writer.Position;
                                    /*Location9765*/if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
                                    {
                                        if ((includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode) && !_Values_Accessed)
                                        {
                                            var deserialized = Values;
                                        }
                                        WriteChild(ref writer, ref _Values, includeChildrenMode, _Values_Accessed, () => GetChildSlice(LazinatorMemoryStorage, _Values_ByteIndex, _Values_ByteLength, false, false, null), verifyCleanness, updateStoredBuffer, false, false, this);
                                    }
                                    
                                    /*Location9766*/if (updateStoredBuffer)
                                    {
                                        _Values_ByteIndex = startOfObjectPosition - startPosition;
                                    }
                                    /*Location9767*/if (updateStoredBuffer)
                                    {
                                        /*Location9768*/_DictionaryBucket_TKey_TValue_EndByteIndex = writer.Position - startPosition;
                                        /*Location9769*/}
                                        /*Location9770*/}
                                        /*Location9771*/
                                    }
                                }
