/*Location3032*//*Location3016*///8bf50f71-5f6f-d238-a31b-e3ff54a33bfc
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.380
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

#nullable disable
namespace LazinatorTests.Examples.RemoteHierarchy
{/*Location3017*/
    using Lazinator.Attributes;/*Location3018*/
    using Lazinator.Buffers;/*Location3019*/
    using Lazinator.Core;/*Location3020*/
    using Lazinator.Exceptions;/*Location3021*/
    using Lazinator.Support;/*Location3022*/
    using Lazinator.Wrappers;/*Location3023*/
    using LazinatorCollections.Remote;/*Location3024*/
    using System;/*Location3025*/
    using System.Buffers;/*Location3026*/
    using System.Collections.Generic;/*Location3027*/
    using System.Diagnostics;/*Location3028*/
    using System.IO;/*Location3029*/
    using System.Linq;/*Location3030*/
    using System.Runtime.InteropServices;/*Location3031*/
    using static Lazinator.Core.LazinatorUtilities;
    
    [Autogenerated]
    public partial class RemoteHierarchy : ILazinator
    {
        /*Location3033*/public bool IsStruct => false;
        
        /*Location3034*//* Property definitions */
        
        /*Location3035*/        protected int _RemoteLevel1Item_ByteIndex;
        /*Location3036*/private int _RemoteHierarchy_EndByteIndex;
        /*Location3037*/protected virtual int _RemoteLevel1Item_ByteLength => _RemoteHierarchy_EndByteIndex - _RemoteLevel1Item_ByteIndex;
        
        /*Location3038*/
        protected int _TopOfHierarchyInt;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public int TopOfHierarchyInt
        {
            get
            {
                return _TopOfHierarchyInt;
            }
            set
            {
                IsDirty = true;
                _TopOfHierarchyInt = value;
            }
        }
        /*Location3039*/
        protected Remote<WGuid, RemoteLevel1> _RemoteLevel1Item;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public Remote<WGuid, RemoteLevel1> RemoteLevel1Item
        {
            get
            {
                if (!_RemoteLevel1Item_Accessed)
                {
                    Lazinate_RemoteLevel1Item();
                } 
                return _RemoteLevel1Item;
            }
            set
            {
                if (_RemoteLevel1Item != null)
                {
                    _RemoteLevel1Item.LazinatorParents = _RemoteLevel1Item.LazinatorParents.WithRemoved(this);
                }
                if (value != null)
                {
                    value.LazinatorParents = value.LazinatorParents.WithAdded(this);
                }
                
                IsDirty = true;
                DescendantIsDirty = true;
                _RemoteLevel1Item = value;
                _RemoteLevel1Item_Accessed = true;
            }
        }
        protected bool _RemoteLevel1Item_Accessed;
        private void Lazinate_RemoteLevel1Item()
        {
            if (LazinatorObjectBytes.Length == 0)
            {
                _RemoteLevel1Item = null;
            }
            else
            {
                LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _RemoteLevel1Item_ByteIndex, _RemoteLevel1Item_ByteLength, false, false, null);
                
                _RemoteLevel1Item = DeserializationFactory.Instance.CreateBaseOrDerivedType(254, (c, p) => new Remote<WGuid, RemoteLevel1>(c, p), childData, this); 
            }
            
            _RemoteLevel1Item_Accessed = true;
        }
        
        /*Location3042*/
        /* Serialization, deserialization, and object relationships */
        
        public RemoteHierarchy(IncludeChildrenMode originalIncludeChildrenMode)
        {
            OriginalIncludeChildrenMode = originalIncludeChildrenMode;
        }
        
        public RemoteHierarchy(LazinatorMemory serializedBytes, ILazinator parent = null)
        {
            LazinatorParents = new LazinatorParentsCollection(parent);
            DeserializeLazinator(serializedBytes);
            HasChanged = false;
            DescendantHasChanged = false;
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
            RemoteHierarchy clone;
            if (cloneBufferOptions == CloneBufferOptions.NoBuffer)
            {
                clone = new RemoteHierarchy(includeChildrenMode);
                clone.LazinatorObjectVersion = LazinatorObjectVersion;
                clone = (RemoteHierarchy)AssignCloneProperties(clone, includeChildrenMode);
            }
            else
            {
                LazinatorMemory bytes = EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, false, IsDirty, DescendantIsDirty, false, LazinatorMemoryStorage, false, this);
                clone = new RemoteHierarchy(bytes);
            }
            return clone;
        }
        
        public virtual ILazinator AssignCloneProperties(ILazinator clone, IncludeChildrenMode includeChildrenMode)
        {
            clone.FreeInMemoryObjects();
            RemoteHierarchy typedClone = (RemoteHierarchy) clone;
            /*Location3040*/typedClone.TopOfHierarchyInt = TopOfHierarchyInt;
            /*Location3041*/if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                if (RemoteLevel1Item == null)
                {
                    typedClone.RemoteLevel1Item = null;
                }
                else
                {
                    typedClone.RemoteLevel1Item = (Remote<WGuid, RemoteLevel1>) RemoteLevel1Item.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer);
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
        
        /*Location3043*/
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
        
        /*Location3044*/public virtual IEnumerable<(string propertyName, ILazinator descendant)> EnumerateLazinatorDescendants(Func<ILazinator, bool> matchCriterion, bool stopExploringBelowMatch, Func<ILazinator, bool> exploreCriterion, bool exploreOnlyDeserializedChildren, bool enumerateNulls)
        {
            /*Location3045*/if (enumerateNulls && (!exploreOnlyDeserializedChildren || _RemoteLevel1Item_Accessed) && RemoteLevel1Item == null)
            {
                yield return ("RemoteLevel1Item", default);
            }
            else
            {
                if ((!exploreOnlyDeserializedChildren && RemoteLevel1Item != null) || (_RemoteLevel1Item_Accessed && _RemoteLevel1Item != null))
                {
                    bool isMatch_RemoteLevel1Item = matchCriterion == null || matchCriterion(RemoteLevel1Item);
                    bool shouldExplore_RemoteLevel1Item = exploreCriterion == null || exploreCriterion(RemoteLevel1Item);
                    if (isMatch_RemoteLevel1Item)
                    {
                        yield return ("RemoteLevel1Item", RemoteLevel1Item);
                    }
                    if ((!stopExploringBelowMatch || !isMatch_RemoteLevel1Item) && shouldExplore_RemoteLevel1Item)
                    {
                        foreach (var toYield in RemoteLevel1Item.EnumerateLazinatorDescendants(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
                        {
                            yield return ("RemoteLevel1Item" + "." + toYield.propertyName, toYield.descendant);
                        }
                    }
                }
                
            }
            
            /*Location3046*/yield break;
        }
        /*Location3047*/
        
        public virtual IEnumerable<(string propertyName, object descendant)> EnumerateNonLazinatorProperties()
        {
            /*Location3048*/yield return ("TopOfHierarchyInt", (object)TopOfHierarchyInt);
            /*Location3049*/yield break;
        }
        /*Location3050*/
        public virtual ILazinator ForEachLazinator(Func<ILazinator, ILazinator> changeFunc, bool exploreOnlyDeserializedChildren, bool changeThisLevel)
        {
            /*Location3051*/if ((!exploreOnlyDeserializedChildren && RemoteLevel1Item != null) || (_RemoteLevel1Item_Accessed && _RemoteLevel1Item != null))
            {
                _RemoteLevel1Item = (Remote<WGuid, RemoteLevel1>) _RemoteLevel1Item.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, true);
            }
            /*Location3052*/if (changeThisLevel && changeFunc != null)
            {
                return changeFunc(this);
            }
            return this;
        }
        /*Location3053*/
        public virtual void FreeInMemoryObjects()
        {
            _RemoteLevel1Item = default;
            _RemoteLevel1Item_Accessed = false;
            IsDirty = false;
            DescendantIsDirty = false;
            HasChanged = false;
            DescendantHasChanged = false;
        }
        /*Location3054*/
        /* Conversion */
        
        public virtual int LazinatorUniqueID => 1082;
        
        protected virtual bool ContainsOpenGenericParameters => false;
        public virtual LazinatorGenericIDType LazinatorGenericID => default;
        
        
        public virtual int LazinatorObjectVersion { get; set; } = 0;
        
        
        /*Location3055*/public virtual void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
            /*Location3056*/_TopOfHierarchyInt = span.ToDecompressedInt(ref bytesSoFar);
            /*Location3057*/_RemoteLevel1Item_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            
            /*Location3058*/_RemoteHierarchy_EndByteIndex = bytesSoFar;
            /*Location3059*/        }
            
            /*Location3060*/public virtual void SerializeExistingBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
            {
                /*Location3061*/if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren)
                {
                    updateStoredBuffer = false;
                }
                /*Location3062*/int startPosition = writer.Position;
                WritePropertiesIntoBuffer(ref writer, includeChildrenMode, verifyCleanness, updateStoredBuffer, true);
                /*Location3063*/if (updateStoredBuffer)
                {
                    UpdateStoredBuffer(ref writer, startPosition, writer.Position - startPosition, includeChildrenMode, false);
                    /*Location3064*/}
                    /*Location3065*/}
                    /*Location3066*/
                    public virtual void UpdateStoredBuffer(ref BinaryBufferWriter writer, int startPosition, int length, IncludeChildrenMode includeChildrenMode, bool updateDeserializedChildren)
                    {
                        /*Location3067*/_IsDirty = false;
                        if (includeChildrenMode == IncludeChildrenMode.IncludeAllChildren)
                        {
                            _DescendantIsDirty = false;/*Location3068*/
                            if (updateDeserializedChildren)
                            {
                                UpdateDeserializedChildren(ref writer, startPosition);
                                /*Location3069*/}
                                /*Location3070*//*Location3071*/
                            }
                            else
                            {
                                ThrowHelper.ThrowCannotUpdateStoredBuffer();
                            }
                            /*Location3072*/
                            var newBuffer = writer.Slice(startPosition, length);
                            LazinatorMemoryStorage = newBuffer;
                            /*Location3073*/}
                            /*Location3074*/
                            protected virtual void UpdateDeserializedChildren(ref BinaryBufferWriter writer, int startPosition)
                            {
                                /*Location3075*/if (_RemoteLevel1Item_Accessed && _RemoteLevel1Item != null)
                                {
                                    RemoteLevel1Item.UpdateStoredBuffer(ref writer, startPosition + _RemoteLevel1Item_ByteIndex + sizeof(int), _RemoteLevel1Item_ByteLength - sizeof(int), IncludeChildrenMode.IncludeAllChildren, true);
                                }
                                
                                /*Location3076*/}
                                
                                /*Location3077*/
                                protected virtual void WritePropertiesIntoBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer, bool includeUniqueID)
                                {
                                    int startPosition = writer.Position;
                                    int startOfObjectPosition = 0;
                                    // header information
                                    /*Location3078*/if (includeUniqueID)
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
                                    /*Location3079*/CompressedIntegralTypes.WriteCompressedInt(ref writer, Lazinator.Support.LazinatorVersionInfo.LazinatorIntVersion);
                                    CompressedIntegralTypes.WriteCompressedInt(ref writer, LazinatorObjectVersion);
                                    writer.Write((byte)includeChildrenMode);
                                    /*Location3080*/// write properties
                                    /*Location3081*/CompressedIntegralTypes.WriteCompressedInt(ref writer, _TopOfHierarchyInt);
                                    /*Location3082*/startOfObjectPosition = writer.Position;
                                    /*Location3083*/if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
                                    {
                                        if ((includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode) && !_RemoteLevel1Item_Accessed)
                                        {
                                            var deserialized = RemoteLevel1Item;
                                        }
                                        WriteChild(ref writer, ref _RemoteLevel1Item, includeChildrenMode, _RemoteLevel1Item_Accessed, () => GetChildSlice(LazinatorMemoryStorage, _RemoteLevel1Item_ByteIndex, _RemoteLevel1Item_ByteLength, false, false, null), verifyCleanness, updateStoredBuffer, false, false, this);
                                    }
                                    
                                    /*Location3084*/if (updateStoredBuffer)
                                    {
                                        _RemoteLevel1Item_ByteIndex = startOfObjectPosition - startPosition;
                                    }
                                    /*Location3085*/if (updateStoredBuffer)
                                    {
                                        /*Location3086*/_RemoteHierarchy_EndByteIndex = writer.Position - startPosition;
                                        /*Location3087*/}
                                        /*Location3088*/}
                                        /*Location3089*/
                                    }
                                }
