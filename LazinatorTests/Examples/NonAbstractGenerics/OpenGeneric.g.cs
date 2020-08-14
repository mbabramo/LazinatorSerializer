/*Location3713*//*Location3699*///f8d4fe92-6559-6511-3ccc-3c2c092baf00
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.380
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

#nullable disable
namespace LazinatorTests.Examples.NonAbstractGenerics
{/*Location3700*/
    using Lazinator.Attributes;/*Location3701*/
    using Lazinator.Buffers;/*Location3702*/
    using Lazinator.Core;/*Location3703*/
    using Lazinator.Exceptions;/*Location3704*/
    using Lazinator.Support;/*Location3705*/
    using System;/*Location3706*/
    using System.Buffers;/*Location3707*/
    using System.Collections.Generic;/*Location3708*/
    using System.Diagnostics;/*Location3709*/
    using System.IO;/*Location3710*/
    using System.Linq;/*Location3711*/
    using System.Runtime.InteropServices;/*Location3712*/
    using static Lazinator.Core.LazinatorUtilities;
    
    [Autogenerated]
    public partial class OpenGeneric<T> : ILazinator
    {
        /*Location3714*/public bool IsStruct => false;
        
        /*Location3715*//* Property definitions */
        
        /*Location3716*/        protected int _MyListT_ByteIndex;
        /*Location3717*/        protected int _MyT_ByteIndex;
        /*Location3718*/protected virtual int _MyListT_ByteLength => _MyT_ByteIndex - _MyListT_ByteIndex;
        /*Location3719*/private int _OpenGeneric_T_EndByteIndex = 0;
        /*Location3720*/protected virtual int _MyT_ByteLength => _OpenGeneric_T_EndByteIndex - _MyT_ByteIndex;
        
        /*Location3721*/
        protected List<T> _MyListT;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public virtual List<T> MyListT
        {
            get
            {
                if (!_MyListT_Accessed)
                {
                    Lazinate_MyListT();
                }
                IsDirty = true; 
                return _MyListT;
            }
            set
            {
                IsDirty = true;
                DescendantIsDirty = true;
                _MyListT = value;
                _MyListT_Accessed = true;
            }
        }
        protected bool _MyListT_Accessed;
        private void Lazinate_MyListT()
        {
            if (LazinatorObjectBytes.Length == 0)
            {
                _MyListT = default(List<T>);
            }
            else
            {
                LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _MyListT_ByteIndex, _MyListT_ByteLength, false, false, null);
                _MyListT = ConvertFromBytes_List_GT_g(childData);
            }
            
            _MyListT_Accessed = true;
        }
        
        /*Location3722*/
        protected T _MyT;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public virtual T MyT
        {
            get
            {
                if (!_MyT_Accessed)
                {
                    Lazinate_MyT();
                } 
                return _MyT;
            }
            set
            {
                if (value != null && value.IsStruct)
                {
                    value.LazinatorParents = new LazinatorParentsCollection(this);
                }
                else
                {
                    if (_MyT != null)
                    {
                        _MyT.LazinatorParents = _MyT.LazinatorParents.WithRemoved(this);
                    }
                    if (value != null)
                    {
                        value.LazinatorParents = value.LazinatorParents.WithAdded(this);
                    }
                }
                
                IsDirty = true;
                DescendantIsDirty = true;
                _MyT = value;
                _MyT_Accessed = true;
            }
        }
        protected bool _MyT_Accessed;
        private void Lazinate_MyT()
        {
            if (LazinatorObjectBytes.Length == 0)
            {
                _MyT = default(T);
                if (_MyT != null)
                { // MyT is a struct
                    _MyT.LazinatorParents = new LazinatorParentsCollection(this);
                }
            }
            else
            {
                LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _MyT_ByteIndex, _MyT_ByteLength, false, false, null);
                
                _MyT = DeserializationFactory.Instance.CreateBasedOnType<T>(childData, this); 
            }
            
            _MyT_Accessed = true;
        }
        
        /*Location3725*/
        /* Serialization, deserialization, and object relationships */
        
        public OpenGeneric(LazinatorConstructorEnum constructorEnum)
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
        
        public virtual ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode = IncludeChildrenMode.IncludeAllChildren, CloneBufferOptions cloneBufferOptions = CloneBufferOptions.IndependentBuffers)
        {
            var clone = new OpenGeneric<T>(LazinatorConstructorEnum.LazinatorConstructor)
            {
                OriginalIncludeChildrenMode = includeChildrenMode
            };
            clone = CompleteClone(this, clone, includeChildrenMode, cloneBufferOptions);
            return clone;
        }
        
        public virtual ILazinator AssignCloneProperties(ILazinator clone, IncludeChildrenMode includeChildrenMode)
        {
            clone.FreeInMemoryObjects();
            OpenGeneric<T> typedClone = (OpenGeneric<T>) clone;
            /*Location3723*/typedClone.MyListT = CloneOrChange_List_GT_g(MyListT, l => l.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer), false);
            /*Location3724*/if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                if (MyT == null)
                {
                    typedClone.MyT = default(T);
                }
                else
                {
                    typedClone.MyT = (T) MyT.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer);
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
        
        /*Location3726*/
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
        
        /*Location3727*/public virtual IEnumerable<(string propertyName, ILazinator descendant)> EnumerateLazinatorDescendants(Func<ILazinator, bool> matchCriterion, bool stopExploringBelowMatch, Func<ILazinator, bool> exploreCriterion, bool exploreOnlyDeserializedChildren, bool enumerateNulls)
        {
            /*Location3728*/if (enumerateNulls && (!exploreOnlyDeserializedChildren || _MyT_Accessed) && MyT == null)
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
            
            /*Location3729*/yield break;
        }
        /*Location3730*/
        
        public virtual IEnumerable<(string propertyName, object descendant)> EnumerateNonLazinatorProperties()
        {
            /*Location3731*/yield return ("MyListT", (object)MyListT);
            /*Location3732*/yield break;
        }
        /*Location3733*/
        public virtual ILazinator ForEachLazinator(Func<ILazinator, ILazinator> changeFunc, bool exploreOnlyDeserializedChildren, bool changeThisLevel)
        {
            /*Location3734*/if ((!exploreOnlyDeserializedChildren && MyT != null) || (_MyT_Accessed && _MyT != null))
            {
                _MyT = (T) _MyT.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, true);
            }
            /*Location3735*/if ((!exploreOnlyDeserializedChildren && MyListT != null) || (_MyListT_Accessed && _MyListT != null))
            {
                _MyListT = (List<T>) CloneOrChange_List_GT_g(_MyListT, l => l.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, true), true);
            }
            /*Location3736*/if (changeThisLevel && changeFunc != null)
            {
                return changeFunc(this);
            }
            return this;
        }
        /*Location3737*/
        public virtual void FreeInMemoryObjects()
        {
            _MyListT = default;
            _MyT = default;
            _MyListT_Accessed = _MyT_Accessed = false;
            IsDirty = false;
            DescendantIsDirty = false;
            HasChanged = false;
            DescendantHasChanged = false;
        }
        /*Location3738*/
        /* Conversion */
        
        public virtual int LazinatorUniqueID => 1033;
        
        protected virtual bool ContainsOpenGenericParameters => true;
        public virtual LazinatorGenericIDType LazinatorGenericID => LazinatorGenericIDType.GetCachedForType<OpenGeneric<T>>(() => DeserializationFactory.Instance.GetUniqueIDListForGenericType(1033, new Type[] { typeof(T) }));
        
        
        public virtual int LazinatorObjectVersion { get; set; } = 0;
        
        
        /*Location3739*/public virtual void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
            /*Location3740*/_MyListT_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            /*Location3741*/_MyT_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            
            /*Location3742*/_OpenGeneric_T_EndByteIndex = bytesSoFar;
            /*Location3743*/        }
            
            /*Location3744*/public virtual void SerializeExistingBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
            {
                /*Location3745*/if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren)
                {
                    updateStoredBuffer = false;
                }
                /*Location3746*/int startPosition = writer.Position;
                WritePropertiesIntoBuffer(ref writer, includeChildrenMode, verifyCleanness, updateStoredBuffer, true);
                /*Location3747*/if (updateStoredBuffer)
                {
                    UpdateStoredBuffer(ref writer, startPosition, writer.Position - startPosition, includeChildrenMode, false);
                    /*Location3748*/}
                    /*Location3749*/}
                    /*Location3750*/
                    public virtual void UpdateStoredBuffer(ref BinaryBufferWriter writer, int startPosition, int length, IncludeChildrenMode includeChildrenMode, bool updateDeserializedChildren)
                    {
                        /*Location3751*/_IsDirty = false;
                        if (includeChildrenMode == IncludeChildrenMode.IncludeAllChildren)
                        {
                            _DescendantIsDirty = false;/*Location3752*/
                            if (updateDeserializedChildren)
                            {
                                UpdateDeserializedChildren(ref writer, startPosition);
                                /*Location3753*/}
                                /*Location3754*/
                                if (_MyT_Accessed && _MyT != null && _MyT.IsStruct && (_MyT.IsDirty || _MyT.DescendantIsDirty))
                                {
                                    _MyT_Accessed = false;
                                }/*Location3755*/
                            }
                            else
                            {
                                ThrowHelper.ThrowCannotUpdateStoredBuffer();
                            }
                            /*Location3756*/
                            var newBuffer = writer.Slice(startPosition, length);
                            LazinatorMemoryStorage = newBuffer;
                            /*Location3757*/}
                            /*Location3758*/
                            protected virtual void UpdateDeserializedChildren(ref BinaryBufferWriter writer, int startPosition)
                            {
                                /*Location3759*/if (_MyT_Accessed && _MyT != null)
                                {
                                    MyT.UpdateStoredBuffer(ref writer, startPosition + _MyT_ByteIndex + sizeof(int), _MyT_ByteLength - sizeof(int), IncludeChildrenMode.IncludeAllChildren, true);
                                }
                                
                                /*Location3760*/if (_MyListT_Accessed && _MyListT != null)
                                {
                                    _MyListT = (List<T>) CloneOrChange_List_GT_g(_MyListT, l => l.RemoveBufferInHierarchy(), true);
                                }
                                /*Location3761*/}
                                
                                /*Location3762*/
                                protected virtual void WritePropertiesIntoBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer, bool includeUniqueID)
                                {
                                    int startPosition = writer.Position;
                                    int startOfObjectPosition = 0;
                                    // header information
                                    /*Location3763*/if (includeUniqueID)
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
                                    /*Location3764*/CompressedIntegralTypes.WriteCompressedInt(ref writer, Lazinator.Support.LazinatorVersionInfo.LazinatorIntVersion);
                                    CompressedIntegralTypes.WriteCompressedInt(ref writer, LazinatorObjectVersion);
                                    writer.Write((byte)includeChildrenMode);
                                    /*Location3765*/// write properties
                                    /*Location3766*/startOfObjectPosition = writer.Position;
                                    /*Location3767*/if ((includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode) && !_MyListT_Accessed)
                                    {
                                        var deserialized = MyListT;
                                    }
                                    /*Location3768*/WriteNonLazinatorObject(
                                    nonLazinatorObject: _MyListT, isBelievedDirty: _MyListT_Accessed || (includeChildrenMode != OriginalIncludeChildrenMode),
                                    isAccessed: _MyListT_Accessed, writer: ref writer,
                                    getChildSliceForFieldFn: () => GetChildSlice(LazinatorMemoryStorage, _MyListT_ByteIndex, _MyListT_ByteLength, false, false, null),
                                    verifyCleanness: false,
                                    binaryWriterAction: (ref BinaryBufferWriter w, bool v) =>
                                    ConvertToBytes_List_GT_g(ref w, _MyListT,
                                    includeChildrenMode, v, updateStoredBuffer));
                                    /*Location3769*/if (updateStoredBuffer)
                                    {
                                        _MyListT_ByteIndex = startOfObjectPosition - startPosition;
                                    }
                                    /*Location3770*/startOfObjectPosition = writer.Position;
                                    /*Location3771*/if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
                                    {
                                        if ((includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode) && !_MyT_Accessed)
                                        {
                                            var deserialized = MyT;
                                        }
                                        WriteChild(ref writer, ref _MyT, includeChildrenMode, _MyT_Accessed, () => GetChildSlice(LazinatorMemoryStorage, _MyT_ByteIndex, _MyT_ByteLength, false, false, null), verifyCleanness, updateStoredBuffer, false, false, this);
                                    }
                                    
                                    /*Location3772*/if (updateStoredBuffer)
                                    {
                                        _MyT_ByteIndex = startOfObjectPosition - startPosition;
                                    }
                                    /*Location3773*/if (updateStoredBuffer)
                                    {
                                        /*Location3774*/_OpenGeneric_T_EndByteIndex = writer.Position - startPosition;
                                        /*Location3775*/}
                                        /*Location3776*/}
                                        /*Location3777*/
                                        /* Conversion of supported collections and tuples */
                                        /*Location3778*/
                                        private static List<T> ConvertFromBytes_List_GT_g(LazinatorMemory storage)
                                        {
                                            if (storage.Length == 0)
                                            {
                                                return default(List<T>);
                                            }
                                            ReadOnlySpan<byte> span = storage.Span;
                                            
                                            int bytesSoFar = 0;
                                            int collectionLength = span.ToDecompressedInt(ref bytesSoFar);
                                            
                                            List<T> collection = new List<T>(collectionLength);
                                            for (int itemIndex = 0; itemIndex < collectionLength; itemIndex++)
                                            {
                                                int lengthCollectionMember = span.ToInt32(ref bytesSoFar);
                                                if (lengthCollectionMember == 0)
                                                {
                                                    collection.Add(default(T));
                                                }
                                                else
                                                {
                                                    LazinatorMemory childData = storage.Slice(bytesSoFar, lengthCollectionMember);
                                                    var item = DeserializationFactory.Instance.CreateBasedOnType<T>(childData);
                                                    collection.Add(item);
                                                }
                                                bytesSoFar += lengthCollectionMember;
                                            }
                                            
                                            return collection;
                                        }/*Location3779*/
                                        
                                        private static void ConvertToBytes_List_GT_g(ref BinaryBufferWriter writer, List<T> itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
                                        {
                                            if (itemToConvert == default(List<T>))
                                            {
                                                return;
                                            }
                                            CompressedIntegralTypes.WriteCompressedInt(ref writer, itemToConvert.Count);
                                            int itemToConvertCount = itemToConvert.Count;
                                            for (int itemIndex = 0; itemIndex < itemToConvertCount; itemIndex++)
                                            {
                                                if (System.Collections.Generic.EqualityComparer<T>.Default.Equals(itemToConvert[itemIndex], default(T)))
                                                {
                                                    writer.Write((uint)0);
                                                }
                                                else 
                                                {
                                                    
                                                    void action(ref BinaryBufferWriter w) 
                                                    {
                                                        var copy = itemToConvert[itemIndex];
                                                        copy.SerializeExistingBuffer(ref w, includeChildrenMode, verifyCleanness, updateStoredBuffer);
                                                        itemToConvert[itemIndex] = copy;
                                                    }
                                                    WriteToBinaryWithIntLengthPrefix(ref writer, action);
                                                }
                                                
                                            }
                                        }
                                        /*Location3780*/
                                        private static List<T> CloneOrChange_List_GT_g(List<T> itemToClone, Func<ILazinator, ILazinator> cloneOrChangeFunc, bool avoidCloningIfPossible)
                                        {
                                            if (itemToClone == null)
                                            {
                                                return default;
                                            }
                                            
                                            int collectionLength = itemToClone.Count;
                                            List<T> collection = avoidCloningIfPossible ? itemToClone : new List<T>(collectionLength);
                                            int itemToCloneCount = itemToClone.Count;
                                            for (int itemIndex = 0; itemIndex < itemToCloneCount; itemIndex++)
                                            {
                                                if (avoidCloningIfPossible)
                                                {
                                                    if (itemToClone[itemIndex] != null)
                                                    {
                                                        itemToClone[itemIndex] = (T) (cloneOrChangeFunc(itemToClone[itemIndex]));
                                                    }
                                                    continue;
                                                }
                                                if (itemToClone[itemIndex] == null)
                                                {
                                                    collection.Add(default(T));
                                                }
                                                else
                                                {
                                                    var itemCopied = (T) (cloneOrChangeFunc(itemToClone[itemIndex]));
                                                    collection.Add(itemCopied);
                                                }
                                                
                                            }
                                            return collection;
                                        }
                                        /*Location3781*/
                                    }
                                }
