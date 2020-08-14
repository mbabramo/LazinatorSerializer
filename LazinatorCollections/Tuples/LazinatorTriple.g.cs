/*Location8943*//*Location8929*///1c9e518c-74dc-f8f1-8f97-d12aedc5a69e
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
{/*Location8930*/
    using Lazinator.Attributes;/*Location8931*/
    using Lazinator.Buffers;/*Location8932*/
    using Lazinator.Core;/*Location8933*/
    using Lazinator.Exceptions;/*Location8934*/
    using Lazinator.Support;/*Location8935*/
    using System;/*Location8936*/
    using System.Buffers;/*Location8937*/
    using System.Collections.Generic;/*Location8938*/
    using System.Diagnostics;/*Location8939*/
    using System.IO;/*Location8940*/
    using System.Linq;/*Location8941*/
    using System.Runtime.InteropServices;/*Location8942*/
    using static Lazinator.Core.LazinatorUtilities;
    
    [Autogenerated]
    public partial class LazinatorTriple<T, U, V> : ILazinator
    {
        /*Location8944*/[DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsStruct => false;
        
        /*Location8945*//* Property definitions */
        
        /*Location8946*/        protected int _Item1_ByteIndex;
        /*Location8947*/        protected int _Item2_ByteIndex;
        /*Location8948*/        protected int _Item3_ByteIndex;
        /*Location8949*/protected virtual int _Item1_ByteLength => _Item2_ByteIndex - _Item1_ByteIndex;
        /*Location8950*/protected virtual int _Item2_ByteLength => _Item3_ByteIndex - _Item2_ByteIndex;
        /*Location8951*/private int _LazinatorTriple_T_U_V_EndByteIndex = 0;
        /*Location8952*/protected virtual int _Item3_ByteLength => _LazinatorTriple_T_U_V_EndByteIndex - _Item3_ByteIndex;
        
        /*Location8953*/
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
        
        /*Location8954*/
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
        
        /*Location8955*/
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected V _Item3;
        public virtual V Item3
        {
            [DebuggerStepThrough]
            get
            {
                if (!_Item3_Accessed)
                {
                    Lazinate_Item3();
                } 
                return _Item3;
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
                    if (_Item3 != null)
                    {
                        _Item3.LazinatorParents = _Item3.LazinatorParents.WithRemoved(this);
                    }
                    if (value != null)
                    {
                        value.LazinatorParents = value.LazinatorParents.WithAdded(this);
                    }
                }
                
                IsDirty = true;
                DescendantIsDirty = true;
                _Item3 = value;
                _Item3_Accessed = true;
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected bool _Item3_Accessed;
        private void Lazinate_Item3()
        {
            if (LazinatorObjectBytes.Length == 0)
            {
                _Item3 = default(V);
                if (_Item3 != null)
                { // Item3 is a struct
                    _Item3.LazinatorParents = new LazinatorParentsCollection(this);
                }
            }
            else
            {
                LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _Item3_ByteIndex, _Item3_ByteLength, false, false, null);
                
                _Item3 = DeserializationFactory.Instance.CreateBasedOnType<V>(childData, this); 
            }
            
            _Item3_Accessed = true;
        }
        
        /*Location8959*/
        /* Serialization, deserialization, and object relationships */
        
        public LazinatorTriple(LazinatorConstructorEnum constructorEnum)
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
            var clone = new LazinatorTriple<T, U, V>(LazinatorConstructorEnum.LazinatorConstructor)
            {
                OriginalIncludeChildrenMode = includeChildrenMode
            };
            clone = CompleteClone(this, clone, includeChildrenMode, cloneBufferOptions);
            return clone;
        }
        
        public virtual ILazinator AssignCloneProperties(ILazinator clone, IncludeChildrenMode includeChildrenMode)
        {
            clone.FreeInMemoryObjects();
            LazinatorTriple<T, U, V> typedClone = (LazinatorTriple<T, U, V>) clone;
            /*Location8956*/if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
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
            
            /*Location8957*/if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
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
            
            /*Location8958*/if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                if (Item3 == null)
                {
                    typedClone.Item3 = default(V);
                }
                else
                {
                    typedClone.Item3 = (V) Item3.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer);
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
        
        /*Location8960*/
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
        
        /*Location8961*/public virtual IEnumerable<(string propertyName, ILazinator descendant)> EnumerateLazinatorDescendants(Func<ILazinator, bool> matchCriterion, bool stopExploringBelowMatch, Func<ILazinator, bool> exploreCriterion, bool exploreOnlyDeserializedChildren, bool enumerateNulls)
        {
            /*Location8962*/if (enumerateNulls && (!exploreOnlyDeserializedChildren || _Item1_Accessed) && Item1 == null)
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
            
            /*Location8963*/if (enumerateNulls && (!exploreOnlyDeserializedChildren || _Item2_Accessed) && Item2 == null)
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
            
            /*Location8964*/if (enumerateNulls && (!exploreOnlyDeserializedChildren || _Item3_Accessed) && Item3 == null)
            {
                yield return ("Item3", default);
            }
            else
            {
                if ((!exploreOnlyDeserializedChildren && Item3 != null) || (_Item3_Accessed && _Item3 != null))
                {
                    bool isMatch_Item3 = matchCriterion == null || matchCriterion(Item3);
                    bool shouldExplore_Item3 = exploreCriterion == null || exploreCriterion(Item3);
                    if (isMatch_Item3)
                    {
                        yield return ("Item3", Item3);
                    }
                    if ((!stopExploringBelowMatch || !isMatch_Item3) && shouldExplore_Item3)
                    {
                        foreach (var toYield in Item3.EnumerateLazinatorDescendants(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
                        {
                            yield return ("Item3" + "." + toYield.propertyName, toYield.descendant);
                        }
                    }
                }
                
            }
            
            /*Location8965*/yield break;
        }
        /*Location8966*/
        
        public virtual IEnumerable<(string propertyName, object descendant)> EnumerateNonLazinatorProperties()
        {
            /*Location8967*/yield break;
        }
        /*Location8968*/
        public virtual ILazinator ForEachLazinator(Func<ILazinator, ILazinator> changeFunc, bool exploreOnlyDeserializedChildren, bool changeThisLevel)
        {
            /*Location8969*/if ((!exploreOnlyDeserializedChildren && Item1 != null) || (_Item1_Accessed && _Item1 != null))
            {
                _Item1 = (T) _Item1.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, true);
            }
            /*Location8970*/if ((!exploreOnlyDeserializedChildren && Item2 != null) || (_Item2_Accessed && _Item2 != null))
            {
                _Item2 = (U) _Item2.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, true);
            }
            /*Location8971*/if ((!exploreOnlyDeserializedChildren && Item3 != null) || (_Item3_Accessed && _Item3 != null))
            {
                _Item3 = (V) _Item3.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, true);
            }
            /*Location8972*/if (changeThisLevel && changeFunc != null)
            {
                return changeFunc(this);
            }
            return this;
        }
        /*Location8973*/
        public virtual void FreeInMemoryObjects()
        {
            _Item1 = default;
            _Item2 = default;
            _Item3 = default;
            _Item1_Accessed = _Item2_Accessed = _Item3_Accessed = false;
            IsDirty = false;
            DescendantIsDirty = false;
            HasChanged = false;
            DescendantHasChanged = false;
        }
        /*Location8974*/
        /* Conversion */
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public virtual int LazinatorUniqueID => 206;
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected virtual bool ContainsOpenGenericParameters => true;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public virtual LazinatorGenericIDType LazinatorGenericID => LazinatorGenericIDType.GetCachedForType<LazinatorTriple<T, U, V>>(() => DeserializationFactory.Instance.GetUniqueIDListForGenericType(206, new Type[] { typeof(T), typeof(U), typeof(V) }));
        
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public virtual int LazinatorObjectVersion { get; set; } = 0;
        
        
        /*Location8975*/public virtual void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
            /*Location8976*/_Item1_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            
            /*Location8977*/_Item2_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            
            /*Location8978*/_Item3_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            
            /*Location8979*/_LazinatorTriple_T_U_V_EndByteIndex = bytesSoFar;
            /*Location8980*/        }
            
            /*Location8981*/public virtual void SerializeExistingBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
            {
                /*Location8982*/if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren)
                {
                    updateStoredBuffer = false;
                }
                /*Location8983*/int startPosition = writer.Position;
                WritePropertiesIntoBuffer(ref writer, includeChildrenMode, verifyCleanness, updateStoredBuffer, true);
                /*Location8984*/if (updateStoredBuffer)
                {
                    UpdateStoredBuffer(ref writer, startPosition, writer.Position - startPosition, includeChildrenMode, false);
                    /*Location8985*/}
                    /*Location8986*/}
                    /*Location8987*/
                    public virtual void UpdateStoredBuffer(ref BinaryBufferWriter writer, int startPosition, int length, IncludeChildrenMode includeChildrenMode, bool updateDeserializedChildren)
                    {
                        /*Location8988*/_IsDirty = false;
                        if (includeChildrenMode == IncludeChildrenMode.IncludeAllChildren)
                        {
                            _DescendantIsDirty = false;/*Location8989*/
                            if (updateDeserializedChildren)
                            {
                                UpdateDeserializedChildren(ref writer, startPosition);
                                /*Location8990*/}
                                /*Location8991*/
                                if (_Item1_Accessed && _Item1 != null && _Item1.IsStruct && (_Item1.IsDirty || _Item1.DescendantIsDirty))
                                {
                                    _Item1_Accessed = false;
                                }
                                if (_Item2_Accessed && _Item2 != null && _Item2.IsStruct && (_Item2.IsDirty || _Item2.DescendantIsDirty))
                                {
                                    _Item2_Accessed = false;
                                }
                                if (_Item3_Accessed && _Item3 != null && _Item3.IsStruct && (_Item3.IsDirty || _Item3.DescendantIsDirty))
                                {
                                    _Item3_Accessed = false;
                                }/*Location8992*/
                            }
                            else
                            {
                                ThrowHelper.ThrowCannotUpdateStoredBuffer();
                            }
                            /*Location8993*/
                            var newBuffer = writer.Slice(startPosition, length);
                            LazinatorMemoryStorage = newBuffer;
                            /*Location8994*/}
                            /*Location8995*/
                            protected virtual void UpdateDeserializedChildren(ref BinaryBufferWriter writer, int startPosition)
                            {
                                /*Location8996*/if (_Item1_Accessed && _Item1 != null)
                                {
                                    Item1.UpdateStoredBuffer(ref writer, startPosition + _Item1_ByteIndex + sizeof(int), _Item1_ByteLength - sizeof(int), IncludeChildrenMode.IncludeAllChildren, true);
                                }
                                
                                /*Location8997*/if (_Item2_Accessed && _Item2 != null)
                                {
                                    Item2.UpdateStoredBuffer(ref writer, startPosition + _Item2_ByteIndex + sizeof(int), _Item2_ByteLength - sizeof(int), IncludeChildrenMode.IncludeAllChildren, true);
                                }
                                
                                /*Location8998*/if (_Item3_Accessed && _Item3 != null)
                                {
                                    Item3.UpdateStoredBuffer(ref writer, startPosition + _Item3_ByteIndex + sizeof(int), _Item3_ByteLength - sizeof(int), IncludeChildrenMode.IncludeAllChildren, true);
                                }
                                
                                /*Location8999*/}
                                
                                /*Location9000*/
                                protected virtual void WritePropertiesIntoBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer, bool includeUniqueID)
                                {
                                    int startPosition = writer.Position;
                                    int startOfObjectPosition = 0;
                                    // header information
                                    /*Location9001*/if (includeUniqueID)
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
                                    /*Location9002*/CompressedIntegralTypes.WriteCompressedInt(ref writer, Lazinator.Support.LazinatorVersionInfo.LazinatorIntVersion);
                                    CompressedIntegralTypes.WriteCompressedInt(ref writer, LazinatorObjectVersion);
                                    writer.Write((byte)includeChildrenMode);
                                    /*Location9003*/// write properties
                                    /*Location9004*/startOfObjectPosition = writer.Position;
                                    /*Location9005*/if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
                                    {
                                        if ((includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode) && !_Item1_Accessed)
                                        {
                                            var deserialized = Item1;
                                        }
                                        WriteChild(ref writer, ref _Item1, includeChildrenMode, _Item1_Accessed, () => GetChildSlice(LazinatorMemoryStorage, _Item1_ByteIndex, _Item1_ByteLength, false, false, null), verifyCleanness, updateStoredBuffer, false, false, this);
                                    }
                                    
                                    /*Location9006*/if (updateStoredBuffer)
                                    {
                                        _Item1_ByteIndex = startOfObjectPosition - startPosition;
                                    }
                                    /*Location9007*/startOfObjectPosition = writer.Position;
                                    /*Location9008*/if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
                                    {
                                        if ((includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode) && !_Item2_Accessed)
                                        {
                                            var deserialized = Item2;
                                        }
                                        WriteChild(ref writer, ref _Item2, includeChildrenMode, _Item2_Accessed, () => GetChildSlice(LazinatorMemoryStorage, _Item2_ByteIndex, _Item2_ByteLength, false, false, null), verifyCleanness, updateStoredBuffer, false, false, this);
                                    }
                                    
                                    /*Location9009*/if (updateStoredBuffer)
                                    {
                                        _Item2_ByteIndex = startOfObjectPosition - startPosition;
                                    }
                                    /*Location9010*/startOfObjectPosition = writer.Position;
                                    /*Location9011*/if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
                                    {
                                        if ((includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode) && !_Item3_Accessed)
                                        {
                                            var deserialized = Item3;
                                        }
                                        WriteChild(ref writer, ref _Item3, includeChildrenMode, _Item3_Accessed, () => GetChildSlice(LazinatorMemoryStorage, _Item3_ByteIndex, _Item3_ByteLength, false, false, null), verifyCleanness, updateStoredBuffer, false, false, this);
                                    }
                                    
                                    /*Location9012*/if (updateStoredBuffer)
                                    {
                                        _Item3_ByteIndex = startOfObjectPosition - startPosition;
                                    }
                                    /*Location9013*/if (updateStoredBuffer)
                                    {
                                        /*Location9014*/_LazinatorTriple_T_U_V_EndByteIndex = writer.Position - startPosition;
                                        /*Location9015*/}
                                        /*Location9016*/}
                                        /*Location9017*/
                                    }
                                }
