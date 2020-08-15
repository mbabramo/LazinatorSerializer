/*Location2572*//*Location2558*///87136fa0-de4a-a0a3-e5a7-023c178a374f
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.380
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

#nullable disable
namespace LazinatorTests.Examples.Hierarchy
{/*Location2559*/
    using Lazinator.Attributes;/*Location2560*/
    using Lazinator.Buffers;/*Location2561*/
    using Lazinator.Core;/*Location2562*/
    using Lazinator.Exceptions;/*Location2563*/
    using Lazinator.Support;/*Location2564*/
    using System;/*Location2565*/
    using System.Buffers;/*Location2566*/
    using System.Collections.Generic;/*Location2567*/
    using System.Diagnostics;/*Location2568*/
    using System.IO;/*Location2569*/
    using System.Linq;/*Location2570*/
    using System.Runtime.InteropServices;/*Location2571*/
    using static Lazinator.Core.LazinatorUtilities;
    
    [Autogenerated]
    public partial class RecursiveExample : ILazinator
    {
        /*Location2573*/public bool IsStruct => false;
        
        /*Location2574*//* Property definitions */
        
        /*Location2575*/        protected int _RecursiveClass_ByteIndex;
        /*Location2576*/        protected int _RecursiveInterface_ByteIndex;
        /*Location2577*/protected virtual int _RecursiveClass_ByteLength => _RecursiveInterface_ByteIndex - _RecursiveClass_ByteIndex;
        /*Location2578*/private int _RecursiveExample_EndByteIndex;
        /*Location2579*/protected virtual int _RecursiveInterface_ByteLength => _RecursiveExample_EndByteIndex - _RecursiveInterface_ByteIndex;
        
        /*Location2580*/
        protected RecursiveExample _RecursiveClass;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public RecursiveExample RecursiveClass
        {
            get
            {
                if (!_RecursiveClass_Accessed)
                {
                    Lazinate_RecursiveClass();
                } 
                return _RecursiveClass;
            }
            set
            {
                if (_RecursiveClass != null)
                {
                    _RecursiveClass.LazinatorParents = _RecursiveClass.LazinatorParents.WithRemoved(this);
                }
                if (value != null)
                {
                    value.LazinatorParents = value.LazinatorParents.WithAdded(this);
                }
                
                IsDirty = true;
                DescendantIsDirty = true;
                _RecursiveClass = value;
                _RecursiveClass_Accessed = true;
            }
        }
        protected bool _RecursiveClass_Accessed;
        private void Lazinate_RecursiveClass()
        {
            if (LazinatorObjectBytes.Length == 0)
            {
                _RecursiveClass = null;
            }
            else
            {
                LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _RecursiveClass_ByteIndex, _RecursiveClass_ByteLength, false, false, null);
                
                _RecursiveClass = DeserializationFactory.Instance.CreateBaseOrDerivedType(1047, (c, p) => new RecursiveExample(c, p), childData, this); 
            }
            
            _RecursiveClass_Accessed = true;
        }
        
        /*Location2581*/
        protected IRecursiveExample _RecursiveInterface;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public IRecursiveExample RecursiveInterface
        {
            get
            {
                if (!_RecursiveInterface_Accessed)
                {
                    Lazinate_RecursiveInterface();
                } 
                return _RecursiveInterface;
            }
            set
            {
                if (_RecursiveInterface != null)
                {
                    _RecursiveInterface.LazinatorParents = _RecursiveInterface.LazinatorParents.WithRemoved(this);
                }
                if (value != null)
                {
                    value.LazinatorParents = value.LazinatorParents.WithAdded(this);
                }
                
                IsDirty = true;
                DescendantIsDirty = true;
                _RecursiveInterface = value;
                _RecursiveInterface_Accessed = true;
            }
        }
        protected bool _RecursiveInterface_Accessed;
        private void Lazinate_RecursiveInterface()
        {
            if (LazinatorObjectBytes.Length == 0)
            {
                _RecursiveInterface = null;
            }
            else
            {
                LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _RecursiveInterface_ByteIndex, _RecursiveInterface_ByteLength, false, false, null);
                
                _RecursiveInterface = DeserializationFactory.Instance.CreateBasedOnType<IRecursiveExample>(childData, this); 
            }
            
            _RecursiveInterface_Accessed = true;
        }
        
        /*Location2584*/
        /* Serialization, deserialization, and object relationships */
        
        public RecursiveExample(LazinatorConstructorEnum constructorEnum)
        {
        }
        
        public RecursiveExample(LazinatorMemory serializedBytes, ILazinator parent = null)
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
            var clone = new RecursiveExample(LazinatorConstructorEnum.LazinatorConstructor)
            {
                OriginalIncludeChildrenMode = includeChildrenMode
            };
            clone = CompleteClone(this, clone, includeChildrenMode, cloneBufferOptions);
            return clone;
        }
        
        public virtual ILazinator AssignCloneProperties(ILazinator clone, IncludeChildrenMode includeChildrenMode)
        {
            clone.FreeInMemoryObjects();
            RecursiveExample typedClone = (RecursiveExample) clone;
            /*Location2582*/if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                if (RecursiveClass == null)
                {
                    typedClone.RecursiveClass = null;
                }
                else
                {
                    typedClone.RecursiveClass = (RecursiveExample) RecursiveClass.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer);
                }
                
            }
            
            /*Location2583*/if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                if (RecursiveInterface == null)
                {
                    typedClone.RecursiveInterface = null;
                }
                else
                {
                    typedClone.RecursiveInterface = (IRecursiveExample) RecursiveInterface.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer);
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
        
        /*Location2585*/
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
        
        /*Location2586*/public virtual IEnumerable<(string propertyName, ILazinator descendant)> EnumerateLazinatorDescendants(Func<ILazinator, bool> matchCriterion, bool stopExploringBelowMatch, Func<ILazinator, bool> exploreCriterion, bool exploreOnlyDeserializedChildren, bool enumerateNulls)
        {
            /*Location2587*/if (enumerateNulls && (!exploreOnlyDeserializedChildren || _RecursiveClass_Accessed) && RecursiveClass == null)
            {
                yield return ("RecursiveClass", default);
            }
            else
            {
                if ((!exploreOnlyDeserializedChildren && RecursiveClass != null) || (_RecursiveClass_Accessed && _RecursiveClass != null))
                {
                    bool isMatch_RecursiveClass = matchCriterion == null || matchCriterion(RecursiveClass);
                    bool shouldExplore_RecursiveClass = exploreCriterion == null || exploreCriterion(RecursiveClass);
                    if (isMatch_RecursiveClass)
                    {
                        yield return ("RecursiveClass", RecursiveClass);
                    }
                    if ((!stopExploringBelowMatch || !isMatch_RecursiveClass) && shouldExplore_RecursiveClass)
                    {
                        foreach (var toYield in RecursiveClass.EnumerateLazinatorDescendants(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
                        {
                            yield return ("RecursiveClass" + "." + toYield.propertyName, toYield.descendant);
                        }
                    }
                }
                
            }
            
            /*Location2588*/if (enumerateNulls && (!exploreOnlyDeserializedChildren || _RecursiveInterface_Accessed) && RecursiveInterface == null)
            {
                yield return ("RecursiveInterface", default);
            }
            else
            {
                if ((!exploreOnlyDeserializedChildren && RecursiveInterface != null) || (_RecursiveInterface_Accessed && _RecursiveInterface != null))
                {
                    bool isMatch_RecursiveInterface = matchCriterion == null || matchCriterion(RecursiveInterface);
                    bool shouldExplore_RecursiveInterface = exploreCriterion == null || exploreCriterion(RecursiveInterface);
                    if (isMatch_RecursiveInterface)
                    {
                        yield return ("RecursiveInterface", RecursiveInterface);
                    }
                    if ((!stopExploringBelowMatch || !isMatch_RecursiveInterface) && shouldExplore_RecursiveInterface)
                    {
                        foreach (var toYield in RecursiveInterface.EnumerateLazinatorDescendants(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
                        {
                            yield return ("RecursiveInterface" + "." + toYield.propertyName, toYield.descendant);
                        }
                    }
                }
                
            }
            
            /*Location2589*/yield break;
        }
        /*Location2590*/
        
        public virtual IEnumerable<(string propertyName, object descendant)> EnumerateNonLazinatorProperties()
        {
            /*Location2591*/yield break;
        }
        /*Location2592*/
        public virtual ILazinator ForEachLazinator(Func<ILazinator, ILazinator> changeFunc, bool exploreOnlyDeserializedChildren, bool changeThisLevel)
        {
            /*Location2593*/if ((!exploreOnlyDeserializedChildren && RecursiveClass != null) || (_RecursiveClass_Accessed && _RecursiveClass != null))
            {
                _RecursiveClass = (RecursiveExample) _RecursiveClass.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, true);
            }
            /*Location2594*/if ((!exploreOnlyDeserializedChildren && RecursiveInterface != null) || (_RecursiveInterface_Accessed && _RecursiveInterface != null))
            {
                _RecursiveInterface = (IRecursiveExample) _RecursiveInterface.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, true);
            }
            /*Location2595*/if (changeThisLevel && changeFunc != null)
            {
                return changeFunc(this);
            }
            return this;
        }
        /*Location2596*/
        public virtual void FreeInMemoryObjects()
        {
            _RecursiveClass = default;
            _RecursiveInterface = default;
            _RecursiveClass_Accessed = _RecursiveInterface_Accessed = false;
            IsDirty = false;
            DescendantIsDirty = false;
            HasChanged = false;
            DescendantHasChanged = false;
        }
        /*Location2597*/
        /* Conversion */
        
        public virtual int LazinatorUniqueID => 1047;
        
        protected virtual bool ContainsOpenGenericParameters => false;
        public virtual LazinatorGenericIDType LazinatorGenericID => default;
        
        
        public virtual int LazinatorObjectVersion { get; set; } = 0;
        
        
        /*Location2598*/public virtual void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
            /*Location2599*/_RecursiveClass_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            
            /*Location2600*/_RecursiveInterface_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            
            /*Location2601*/_RecursiveExample_EndByteIndex = bytesSoFar;
            /*Location2602*/        }
            
            /*Location2603*/public virtual void SerializeExistingBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
            {
                /*Location2604*/if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren)
                {
                    updateStoredBuffer = false;
                }
                /*Location2605*/int startPosition = writer.Position;
                WritePropertiesIntoBuffer(ref writer, includeChildrenMode, verifyCleanness, updateStoredBuffer, true);
                /*Location2606*/if (updateStoredBuffer)
                {
                    UpdateStoredBuffer(ref writer, startPosition, writer.Position - startPosition, includeChildrenMode, false);
                    /*Location2607*/}
                    /*Location2608*/}
                    /*Location2609*/
                    public virtual void UpdateStoredBuffer(ref BinaryBufferWriter writer, int startPosition, int length, IncludeChildrenMode includeChildrenMode, bool updateDeserializedChildren)
                    {
                        /*Location2610*/_IsDirty = false;
                        if (includeChildrenMode == IncludeChildrenMode.IncludeAllChildren)
                        {
                            _DescendantIsDirty = false;/*Location2611*/
                            if (updateDeserializedChildren)
                            {
                                UpdateDeserializedChildren(ref writer, startPosition);
                                /*Location2612*/}
                                /*Location2613*//*Location2614*/
                            }
                            else
                            {
                                ThrowHelper.ThrowCannotUpdateStoredBuffer();
                            }
                            /*Location2615*/
                            var newBuffer = writer.Slice(startPosition, length);
                            LazinatorMemoryStorage = newBuffer;
                            /*Location2616*/}
                            /*Location2617*/
                            protected virtual void UpdateDeserializedChildren(ref BinaryBufferWriter writer, int startPosition)
                            {
                                /*Location2618*/if (_RecursiveClass_Accessed && _RecursiveClass != null)
                                {
                                    RecursiveClass.UpdateStoredBuffer(ref writer, startPosition + _RecursiveClass_ByteIndex + sizeof(int), _RecursiveClass_ByteLength - sizeof(int), IncludeChildrenMode.IncludeAllChildren, true);
                                }
                                
                                /*Location2619*/if (_RecursiveInterface_Accessed && _RecursiveInterface != null)
                                {
                                    RecursiveInterface.UpdateStoredBuffer(ref writer, startPosition + _RecursiveInterface_ByteIndex + sizeof(int), _RecursiveInterface_ByteLength - sizeof(int), IncludeChildrenMode.IncludeAllChildren, true);
                                }
                                
                                /*Location2620*/}
                                
                                /*Location2621*/
                                protected virtual void WritePropertiesIntoBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer, bool includeUniqueID)
                                {
                                    int startPosition = writer.Position;
                                    int startOfObjectPosition = 0;
                                    // header information
                                    /*Location2622*/if (includeUniqueID)
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
                                    /*Location2623*/CompressedIntegralTypes.WriteCompressedInt(ref writer, Lazinator.Support.LazinatorVersionInfo.LazinatorIntVersion);
                                    CompressedIntegralTypes.WriteCompressedInt(ref writer, LazinatorObjectVersion);
                                    writer.Write((byte)includeChildrenMode);
                                    /*Location2624*/// write properties
                                    /*Location2625*/startOfObjectPosition = writer.Position;
                                    /*Location2626*/if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
                                    {
                                        if ((includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode) && !_RecursiveClass_Accessed)
                                        {
                                            var deserialized = RecursiveClass;
                                        }
                                        WriteChild(ref writer, ref _RecursiveClass, includeChildrenMode, _RecursiveClass_Accessed, () => GetChildSlice(LazinatorMemoryStorage, _RecursiveClass_ByteIndex, _RecursiveClass_ByteLength, false, false, null), verifyCleanness, updateStoredBuffer, false, false, this);
                                    }
                                    
                                    /*Location2627*/if (updateStoredBuffer)
                                    {
                                        _RecursiveClass_ByteIndex = startOfObjectPosition - startPosition;
                                    }
                                    /*Location2628*/startOfObjectPosition = writer.Position;
                                    /*Location2629*/if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
                                    {
                                        if ((includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode) && !_RecursiveInterface_Accessed)
                                        {
                                            var deserialized = RecursiveInterface;
                                        }
                                        WriteChild(ref writer, ref _RecursiveInterface, includeChildrenMode, _RecursiveInterface_Accessed, () => GetChildSlice(LazinatorMemoryStorage, _RecursiveInterface_ByteIndex, _RecursiveInterface_ByteLength, false, false, null), verifyCleanness, updateStoredBuffer, false, false, this);
                                    }
                                    
                                    /*Location2630*/if (updateStoredBuffer)
                                    {
                                        _RecursiveInterface_ByteIndex = startOfObjectPosition - startPosition;
                                    }
                                    /*Location2631*/if (updateStoredBuffer)
                                    {
                                        /*Location2632*/_RecursiveExample_EndByteIndex = writer.Position - startPosition;
                                        /*Location2633*/}
                                        /*Location2634*/}
                                        /*Location2635*/
                                    }
                                }
