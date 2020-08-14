/*Location3433*//*Location3419*///f3d7c781-a82c-8c19-6b67-95ff819c09ca
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.380
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

#nullable disable
namespace LazinatorTests.Examples.Abstract
{/*Location3420*/
    using Lazinator.Attributes;/*Location3421*/
    using Lazinator.Buffers;/*Location3422*/
    using Lazinator.Core;/*Location3423*/
    using Lazinator.Exceptions;/*Location3424*/
    using Lazinator.Support;/*Location3425*/
    using System;/*Location3426*/
    using System.Buffers;/*Location3427*/
    using System.Collections.Generic;/*Location3428*/
    using System.Diagnostics;/*Location3429*/
    using System.IO;/*Location3430*/
    using System.Linq;/*Location3431*/
    using System.Runtime.InteropServices;/*Location3432*/
    using static Lazinator.Core.LazinatorUtilities;
    
    [Autogenerated]
    public partial class DerivedGenericContainer<T> : ILazinator
    {
        /*Location3434*/public bool IsStruct => false;
        
        /*Location3435*//* Property definitions */
        
        /*Location3436*/        protected int _Item_ByteIndex;
        /*Location3437*/private int _DerivedGenericContainer_T_EndByteIndex;
        /*Location3438*/protected virtual int _Item_ByteLength => _DerivedGenericContainer_T_EndByteIndex - _Item_ByteIndex;
        
        /*Location3439*/
        protected AbstractGeneric1<T> _Item;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public virtual AbstractGeneric1<T> Item
        {
            get
            {
                if (!_Item_Accessed)
                {
                    Lazinate_Item();
                } 
                return _Item;
            }
            set
            {
                if (_Item != null)
                {
                    _Item.LazinatorParents = _Item.LazinatorParents.WithRemoved(this);
                }
                if (value != null)
                {
                    value.LazinatorParents = value.LazinatorParents.WithAdded(this);
                }
                
                IsDirty = true;
                DescendantIsDirty = true;
                _Item = value;
                _Item_Accessed = true;
            }
        }
        protected bool _Item_Accessed;
        private void Lazinate_Item()
        {
            if (LazinatorObjectBytes.Length == 0)
            {
                _Item = null;
            }
            else
            {
                LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _Item_ByteIndex, _Item_ByteLength, false, false, null);
                
                _Item = DeserializationFactory.Instance.CreateAbstractType<AbstractGeneric1<T>>(childData, this); 
            }
            
            _Item_Accessed = true;
        }
        
        /*Location3441*/
        /* Serialization, deserialization, and object relationships */
        
        public DerivedGenericContainer(LazinatorConstructorEnum constructorEnum)
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
            var clone = new DerivedGenericContainer<T>(LazinatorConstructorEnum.LazinatorConstructor)
            {
                OriginalIncludeChildrenMode = includeChildrenMode
            };
            clone = CompleteClone(this, clone, includeChildrenMode, cloneBufferOptions);
            return clone;
        }
        
        public virtual ILazinator AssignCloneProperties(ILazinator clone, IncludeChildrenMode includeChildrenMode)
        {
            clone.FreeInMemoryObjects();
            DerivedGenericContainer<T> typedClone = (DerivedGenericContainer<T>) clone;
            /*Location3440*/if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                if (Item == null)
                {
                    typedClone.Item = null;
                }
                else
                {
                    typedClone.Item = (AbstractGeneric1<T>) Item.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer);
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
        
        /*Location3442*/
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
        
        /*Location3443*/public virtual IEnumerable<(string propertyName, ILazinator descendant)> EnumerateLazinatorDescendants(Func<ILazinator, bool> matchCriterion, bool stopExploringBelowMatch, Func<ILazinator, bool> exploreCriterion, bool exploreOnlyDeserializedChildren, bool enumerateNulls)
        {
            /*Location3444*/if (enumerateNulls && (!exploreOnlyDeserializedChildren || _Item_Accessed) && Item == null)
            {
                yield return ("Item", default);
            }
            else
            {
                if ((!exploreOnlyDeserializedChildren && Item != null) || (_Item_Accessed && _Item != null))
                {
                    bool isMatch_Item = matchCriterion == null || matchCriterion(Item);
                    bool shouldExplore_Item = exploreCriterion == null || exploreCriterion(Item);
                    if (isMatch_Item)
                    {
                        yield return ("Item", Item);
                    }
                    if ((!stopExploringBelowMatch || !isMatch_Item) && shouldExplore_Item)
                    {
                        foreach (var toYield in Item.EnumerateLazinatorDescendants(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
                        {
                            yield return ("Item" + "." + toYield.propertyName, toYield.descendant);
                        }
                    }
                }
                
            }
            
            /*Location3445*/yield break;
        }
        /*Location3446*/
        
        public virtual IEnumerable<(string propertyName, object descendant)> EnumerateNonLazinatorProperties()
        {
            /*Location3447*/yield break;
        }
        /*Location3448*/
        public virtual ILazinator ForEachLazinator(Func<ILazinator, ILazinator> changeFunc, bool exploreOnlyDeserializedChildren, bool changeThisLevel)
        {
            /*Location3449*/if ((!exploreOnlyDeserializedChildren && Item != null) || (_Item_Accessed && _Item != null))
            {
                _Item = (AbstractGeneric1<T>) _Item.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, true);
            }
            /*Location3450*/if (changeThisLevel && changeFunc != null)
            {
                return changeFunc(this);
            }
            return this;
        }
        /*Location3451*/
        public virtual void FreeInMemoryObjects()
        {
            _Item = default;
            _Item_Accessed = false;
            IsDirty = false;
            DescendantIsDirty = false;
            HasChanged = false;
            DescendantHasChanged = false;
        }
        /*Location3452*/
        /* Conversion */
        
        public virtual int LazinatorUniqueID => 1065;
        
        protected virtual bool ContainsOpenGenericParameters => true;
        public virtual LazinatorGenericIDType LazinatorGenericID => LazinatorGenericIDType.GetCachedForType<DerivedGenericContainer<T>>(() => DeserializationFactory.Instance.GetUniqueIDListForGenericType(1065, new Type[] { typeof(T) }));
        
        
        public virtual int LazinatorObjectVersion { get; set; } = 0;
        
        
        /*Location3453*/public virtual void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
            /*Location3454*/_Item_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            
            /*Location3455*/_DerivedGenericContainer_T_EndByteIndex = bytesSoFar;
            /*Location3456*/        }
            
            /*Location3457*/public virtual void SerializeExistingBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
            {
                /*Location3458*/if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren)
                {
                    updateStoredBuffer = false;
                }
                /*Location3459*/int startPosition = writer.Position;
                WritePropertiesIntoBuffer(ref writer, includeChildrenMode, verifyCleanness, updateStoredBuffer, true);
                /*Location3460*/if (updateStoredBuffer)
                {
                    UpdateStoredBuffer(ref writer, startPosition, writer.Position - startPosition, includeChildrenMode, false);
                    /*Location3461*/}
                    /*Location3462*/}
                    /*Location3463*/
                    public virtual void UpdateStoredBuffer(ref BinaryBufferWriter writer, int startPosition, int length, IncludeChildrenMode includeChildrenMode, bool updateDeserializedChildren)
                    {
                        /*Location3464*/_IsDirty = false;
                        if (includeChildrenMode == IncludeChildrenMode.IncludeAllChildren)
                        {
                            _DescendantIsDirty = false;/*Location3465*/
                            if (updateDeserializedChildren)
                            {
                                UpdateDeserializedChildren(ref writer, startPosition);
                                /*Location3466*/}
                                /*Location3467*//*Location3468*/
                            }
                            else
                            {
                                ThrowHelper.ThrowCannotUpdateStoredBuffer();
                            }
                            /*Location3469*/
                            var newBuffer = writer.Slice(startPosition, length);
                            LazinatorMemoryStorage = newBuffer;
                            /*Location3470*/}
                            /*Location3471*/
                            protected virtual void UpdateDeserializedChildren(ref BinaryBufferWriter writer, int startPosition)
                            {
                                /*Location3472*/if (_Item_Accessed && _Item != null)
                                {
                                    Item.UpdateStoredBuffer(ref writer, startPosition + _Item_ByteIndex + sizeof(int), _Item_ByteLength - sizeof(int), IncludeChildrenMode.IncludeAllChildren, true);
                                }
                                
                                /*Location3473*/}
                                
                                /*Location3474*/
                                protected virtual void WritePropertiesIntoBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer, bool includeUniqueID)
                                {
                                    int startPosition = writer.Position;
                                    int startOfObjectPosition = 0;
                                    // header information
                                    /*Location3475*/if (includeUniqueID)
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
                                    /*Location3476*/CompressedIntegralTypes.WriteCompressedInt(ref writer, Lazinator.Support.LazinatorVersionInfo.LazinatorIntVersion);
                                    CompressedIntegralTypes.WriteCompressedInt(ref writer, LazinatorObjectVersion);
                                    writer.Write((byte)includeChildrenMode);
                                    /*Location3477*/// write properties
                                    /*Location3478*/startOfObjectPosition = writer.Position;
                                    /*Location3479*/if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
                                    {
                                        if ((includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode) && !_Item_Accessed)
                                        {
                                            var deserialized = Item;
                                        }
                                        WriteChild(ref writer, ref _Item, includeChildrenMode, _Item_Accessed, () => GetChildSlice(LazinatorMemoryStorage, _Item_ByteIndex, _Item_ByteLength, false, false, null), verifyCleanness, updateStoredBuffer, false, false, this);
                                    }
                                    
                                    /*Location3480*/if (updateStoredBuffer)
                                    {
                                        _Item_ByteIndex = startOfObjectPosition - startPosition;
                                    }
                                    /*Location3481*/if (updateStoredBuffer)
                                    {
                                        /*Location3482*/_DerivedGenericContainer_T_EndByteIndex = writer.Position - startPosition;
                                        /*Location3483*/}
                                        /*Location3484*/}
                                        /*Location3485*/
                                    }
                                }
