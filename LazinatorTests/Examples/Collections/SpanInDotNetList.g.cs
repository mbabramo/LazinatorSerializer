/*Location5703*//*Location5689*///d54bbffd-87ef-a6fe-fc22-143f62b5c394
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.380
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

#nullable disable
namespace LazinatorTests.Examples.Collections
{/*Location5690*/
    using Lazinator.Attributes;/*Location5691*/
    using Lazinator.Buffers;/*Location5692*/
    using Lazinator.Core;/*Location5693*/
    using Lazinator.Exceptions;/*Location5694*/
    using Lazinator.Support;/*Location5695*/
    using System;/*Location5696*/
    using System.Buffers;/*Location5697*/
    using System.Collections.Generic;/*Location5698*/
    using System.Diagnostics;/*Location5699*/
    using System.IO;/*Location5700*/
    using System.Linq;/*Location5701*/
    using System.Runtime.InteropServices;/*Location5702*/
    using static Lazinator.Core.LazinatorUtilities;
    
    [Autogenerated]
    public partial class SpanInDotNetList : ILazinator
    {
        /*Location5704*/public bool IsStruct => false;
        
        /*Location5705*//* Property definitions */
        
        /*Location5706*/        protected int _SpanList_ByteIndex;
        /*Location5707*/private int _SpanInDotNetList_EndByteIndex;
        /*Location5708*/protected virtual int _SpanList_ByteLength => _SpanInDotNetList_EndByteIndex - _SpanList_ByteIndex;
        
        /*Location5709*/
        protected int _SomeInt;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public int SomeInt
        {
            get
            {
                return _SomeInt;
            }
            set
            {
                IsDirty = true;
                _SomeInt = value;
            }
        }
        /*Location5710*/
        protected List<SpanAndMemory> _SpanList;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public List<SpanAndMemory> SpanList
        {
            get
            {
                if (!_SpanList_Accessed)
                {
                    Lazinate_SpanList();
                }
                IsDirty = true; 
                return _SpanList;
            }
            set
            {
                IsDirty = true;
                DescendantIsDirty = true;
                _SpanList = value;
                _SpanList_Accessed = true;
            }
        }
        protected bool _SpanList_Accessed;
        private void Lazinate_SpanList()
        {
            if (LazinatorObjectBytes.Length == 0)
            {
                _SpanList = default(List<SpanAndMemory>);
            }
            else
            {
                LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _SpanList_ByteIndex, _SpanList_ByteLength, false, false, null);
                _SpanList = ConvertFromBytes_List_GSpanAndMemory_g(childData);
            }
            
            _SpanList_Accessed = true;
        }
        
        /*Location5713*/
        /* Serialization, deserialization, and object relationships */
        
        public SpanInDotNetList(LazinatorConstructorEnum constructorEnum)
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
            var clone = new SpanInDotNetList(LazinatorConstructorEnum.LazinatorConstructor)
            {
                OriginalIncludeChildrenMode = includeChildrenMode
            };
            clone = CompleteClone(this, clone, includeChildrenMode, cloneBufferOptions);
            return clone;
        }
        
        public virtual ILazinator AssignCloneProperties(ILazinator clone, IncludeChildrenMode includeChildrenMode)
        {
            clone.FreeInMemoryObjects();
            SpanInDotNetList typedClone = (SpanInDotNetList) clone;
            /*Location5711*/typedClone.SomeInt = SomeInt;
            /*Location5712*/typedClone.SpanList = CloneOrChange_List_GSpanAndMemory_g(SpanList, l => l.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer), false);
            
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
        
        /*Location5714*/
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
        
        /*Location5715*/public virtual IEnumerable<(string propertyName, ILazinator descendant)> EnumerateLazinatorDescendants(Func<ILazinator, bool> matchCriterion, bool stopExploringBelowMatch, Func<ILazinator, bool> exploreCriterion, bool exploreOnlyDeserializedChildren, bool enumerateNulls)
        {
            /*Location5716*/yield break;
        }
        /*Location5717*/
        
        public virtual IEnumerable<(string propertyName, object descendant)> EnumerateNonLazinatorProperties()
        {
            /*Location5718*/yield return ("SomeInt", (object)SomeInt);
            /*Location5719*/yield return ("SpanList", (object)SpanList);
            /*Location5720*/yield break;
        }
        /*Location5721*/
        public virtual ILazinator ForEachLazinator(Func<ILazinator, ILazinator> changeFunc, bool exploreOnlyDeserializedChildren, bool changeThisLevel)
        {
            /*Location5722*/if ((!exploreOnlyDeserializedChildren && SpanList != null) || (_SpanList_Accessed && _SpanList != null))
            {
                _SpanList = (List<SpanAndMemory>) CloneOrChange_List_GSpanAndMemory_g(_SpanList, l => l.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, true), true);
            }
            /*Location5723*/if (changeThisLevel && changeFunc != null)
            {
                return changeFunc(this);
            }
            return this;
        }
        /*Location5724*/
        public virtual void FreeInMemoryObjects()
        {
            _SpanList = default;
            _SpanList_Accessed = false;
            IsDirty = false;
            DescendantIsDirty = false;
            HasChanged = false;
            DescendantHasChanged = false;
        }
        /*Location5725*/
        /* Conversion */
        
        public virtual int LazinatorUniqueID => 1080;
        
        protected virtual bool ContainsOpenGenericParameters => false;
        public virtual LazinatorGenericIDType LazinatorGenericID => default;
        
        
        public virtual int LazinatorObjectVersion { get; set; } = 0;
        
        
        /*Location5726*/public virtual void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
            /*Location5727*/_SomeInt = span.ToDecompressedInt(ref bytesSoFar);
            /*Location5728*/_SpanList_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            /*Location5729*/_SpanInDotNetList_EndByteIndex = bytesSoFar;
            /*Location5730*/        }
            
            /*Location5731*/public virtual void SerializeExistingBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
            {
                /*Location5732*/if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren)
                {
                    updateStoredBuffer = false;
                }
                /*Location5733*/int startPosition = writer.Position;
                WritePropertiesIntoBuffer(ref writer, includeChildrenMode, verifyCleanness, updateStoredBuffer, true);
                /*Location5734*/if (updateStoredBuffer)
                {
                    UpdateStoredBuffer(ref writer, startPosition, writer.Position - startPosition, includeChildrenMode, false);
                    /*Location5735*/}
                    /*Location5736*/}
                    /*Location5737*/
                    public virtual void UpdateStoredBuffer(ref BinaryBufferWriter writer, int startPosition, int length, IncludeChildrenMode includeChildrenMode, bool updateDeserializedChildren)
                    {
                        /*Location5738*/_IsDirty = false;
                        if (includeChildrenMode == IncludeChildrenMode.IncludeAllChildren)
                        {
                            _DescendantIsDirty = false;/*Location5739*/
                            if (updateDeserializedChildren)
                            {
                                UpdateDeserializedChildren(ref writer, startPosition);
                                /*Location5740*/}
                                /*Location5741*//*Location5742*/
                            }
                            else
                            {
                                ThrowHelper.ThrowCannotUpdateStoredBuffer();
                            }
                            /*Location5743*/
                            var newBuffer = writer.Slice(startPosition, length);
                            LazinatorMemoryStorage = newBuffer;
                            /*Location5744*/}
                            /*Location5745*/
                            protected virtual void UpdateDeserializedChildren(ref BinaryBufferWriter writer, int startPosition)
                            {
                                /*Location5746*/if (_SpanList_Accessed && _SpanList != null)
                                {
                                    _SpanList = (List<SpanAndMemory>) CloneOrChange_List_GSpanAndMemory_g(_SpanList, l => l.RemoveBufferInHierarchy(), true);
                                }
                                /*Location5747*/}
                                
                                /*Location5748*/
                                protected virtual void WritePropertiesIntoBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer, bool includeUniqueID)
                                {
                                    int startPosition = writer.Position;
                                    int startOfObjectPosition = 0;
                                    // header information
                                    /*Location5749*/if (includeUniqueID)
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
                                    /*Location5750*/CompressedIntegralTypes.WriteCompressedInt(ref writer, Lazinator.Support.LazinatorVersionInfo.LazinatorIntVersion);
                                    CompressedIntegralTypes.WriteCompressedInt(ref writer, LazinatorObjectVersion);
                                    writer.Write((byte)includeChildrenMode);
                                    /*Location5751*/// write properties
                                    /*Location5752*/CompressedIntegralTypes.WriteCompressedInt(ref writer, _SomeInt);
                                    /*Location5753*/startOfObjectPosition = writer.Position;
                                    /*Location5754*/if ((includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode) && !_SpanList_Accessed)
                                    {
                                        var deserialized = SpanList;
                                    }
                                    /*Location5755*/WriteNonLazinatorObject(
                                    nonLazinatorObject: _SpanList, isBelievedDirty: _SpanList_Accessed || (includeChildrenMode != OriginalIncludeChildrenMode),
                                    isAccessed: _SpanList_Accessed, writer: ref writer,
                                    getChildSliceForFieldFn: () => GetChildSlice(LazinatorMemoryStorage, _SpanList_ByteIndex, _SpanList_ByteLength, false, false, null),
                                    verifyCleanness: false,
                                    binaryWriterAction: (ref BinaryBufferWriter w, bool v) =>
                                    ConvertToBytes_List_GSpanAndMemory_g(ref w, _SpanList,
                                    includeChildrenMode, v, updateStoredBuffer));
                                    /*Location5756*/if (updateStoredBuffer)
                                    {
                                        _SpanList_ByteIndex = startOfObjectPosition - startPosition;
                                    }
                                    /*Location5757*/if (updateStoredBuffer)
                                    {
                                        /*Location5758*/_SpanInDotNetList_EndByteIndex = writer.Position - startPosition;
                                        /*Location5759*/}
                                        /*Location5760*/}
                                        /*Location5761*/
                                        /* Conversion of supported collections and tuples */
                                        /*Location5762*/
                                        private static List<SpanAndMemory> ConvertFromBytes_List_GSpanAndMemory_g(LazinatorMemory storage)
                                        {
                                            if (storage.Length == 0)
                                            {
                                                return default(List<SpanAndMemory>);
                                            }
                                            ReadOnlySpan<byte> span = storage.Span;
                                            
                                            int bytesSoFar = 0;
                                            int collectionLength = span.ToDecompressedInt(ref bytesSoFar);
                                            
                                            List<SpanAndMemory> collection = new List<SpanAndMemory>(collectionLength);
                                            for (int itemIndex = 0; itemIndex < collectionLength; itemIndex++)
                                            {
                                                int lengthCollectionMember = span.ToInt32(ref bytesSoFar);
                                                if (lengthCollectionMember == 0)
                                                {
                                                    collection.Add(null);
                                                }
                                                else
                                                {
                                                    LazinatorMemory childData = storage.Slice(bytesSoFar, lengthCollectionMember);
                                                    var item = DeserializationFactory.Instance.CreateBasedOnType<SpanAndMemory>(childData);
                                                    collection.Add(item);
                                                }
                                                bytesSoFar += lengthCollectionMember;
                                            }
                                            
                                            return collection;
                                        }/*Location5763*/
                                        
                                        private static void ConvertToBytes_List_GSpanAndMemory_g(ref BinaryBufferWriter writer, List<SpanAndMemory> itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
                                        {
                                            if (itemToConvert == default(List<SpanAndMemory>))
                                            {
                                                return;
                                            }
                                            CompressedIntegralTypes.WriteCompressedInt(ref writer, itemToConvert.Count);
                                            int itemToConvertCount = itemToConvert.Count;
                                            for (int itemIndex = 0; itemIndex < itemToConvertCount; itemIndex++)
                                            {
                                                if (itemToConvert[itemIndex] == null)
                                                {
                                                    writer.Write((uint)0);
                                                }
                                                else 
                                                {
                                                    
                                                    void action(ref BinaryBufferWriter w) => itemToConvert[itemIndex].SerializeExistingBuffer(ref w, includeChildrenMode, verifyCleanness, updateStoredBuffer);
                                                    WriteToBinaryWithIntLengthPrefix(ref writer, action);
                                                }
                                                
                                            }
                                        }
                                        /*Location5764*/
                                        private static List<SpanAndMemory> CloneOrChange_List_GSpanAndMemory_g(List<SpanAndMemory> itemToClone, Func<ILazinator, ILazinator> cloneOrChangeFunc, bool avoidCloningIfPossible)
                                        {
                                            if (itemToClone == null)
                                            {
                                                return default;
                                            }
                                            
                                            int collectionLength = itemToClone.Count;
                                            List<SpanAndMemory> collection = avoidCloningIfPossible ? itemToClone : new List<SpanAndMemory>(collectionLength);
                                            int itemToCloneCount = itemToClone.Count;
                                            for (int itemIndex = 0; itemIndex < itemToCloneCount; itemIndex++)
                                            {
                                                if (avoidCloningIfPossible)
                                                {
                                                    if (itemToClone[itemIndex] != null)
                                                    {
                                                        itemToClone[itemIndex] = (SpanAndMemory) (cloneOrChangeFunc(itemToClone[itemIndex]));
                                                    }
                                                    continue;
                                                }
                                                if (itemToClone[itemIndex] == null)
                                                {
                                                    collection.Add(null);
                                                }
                                                else
                                                {
                                                    var itemCopied = (SpanAndMemory) (cloneOrChangeFunc(itemToClone[itemIndex]));
                                                    collection.Add(itemCopied);
                                                }
                                                
                                            }
                                            return collection;
                                        }
                                        /*Location5765*/
                                    }
                                }
