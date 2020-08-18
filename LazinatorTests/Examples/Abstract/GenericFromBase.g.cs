/*Location4101*//*Location4087*///c02b8d8c-0f31-8bc5-767a-44c8e8c9912c
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
{/*Location4088*/
    using Lazinator.Attributes;/*Location4089*/
    using Lazinator.Buffers;/*Location4090*/
    using Lazinator.Core;/*Location4091*/
    using Lazinator.Exceptions;/*Location4092*/
    using Lazinator.Support;/*Location4093*/
    using System;/*Location4094*/
    using System.Buffers;/*Location4095*/
    using System.Collections.Generic;/*Location4096*/
    using System.Diagnostics;/*Location4097*/
    using System.IO;/*Location4098*/
    using System.Linq;/*Location4099*/
    using System.Runtime.InteropServices;/*Location4100*/
    using static Lazinator.Core.LazinatorUtilities;
    
    [Autogenerated]
    public partial class GenericFromBase<T> : Base, ILazinator
    {
        /*Location4102*//* Property definitions */
        
        /*Location4103*/        protected int _MyT_ByteIndex;
        /*Location4104*/private int _GenericFromBase_T_EndByteIndex = 0;
        /*Location4105*/protected virtual int _MyT_ByteLength => _GenericFromBase_T_EndByteIndex - _MyT_ByteIndex;
        
        /*Location4106*/
        protected int _MyInt;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public int MyInt
        {
            get
            {
                return _MyInt;
            }
            set
            {
                IsDirty = true;
                _MyInt = value;
            }
        }
        /*Location4107*/
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
        
        /*Location4110*/        /* Clone overrides */
        
        public GenericFromBase(IncludeChildrenMode originalIncludeChildrenMode) : base(originalIncludeChildrenMode)
        {
        }
        
        public GenericFromBase(LazinatorMemory serializedBytes, ILazinator parent = null) : base(serializedBytes, parent)
        {
        }
        
        public override ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode = IncludeChildrenMode.IncludeAllChildren, CloneBufferOptions cloneBufferOptions = CloneBufferOptions.IndependentBuffers)
        {
            GenericFromBase<T> clone;
            if (cloneBufferOptions == CloneBufferOptions.NoBuffer)
            {
                clone = new GenericFromBase<T>(includeChildrenMode);
                clone.LazinatorObjectVersion = LazinatorObjectVersion;
                clone = (GenericFromBase<T>)AssignCloneProperties(clone, includeChildrenMode);
            }
            else
            {
                LazinatorMemory bytes = EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, false, IsDirty, DescendantIsDirty, false, LazinatorMemoryStorage, false, this);
                clone = new GenericFromBase<T>(bytes);
            }
            return clone;
        }
        
        public override ILazinator AssignCloneProperties(ILazinator clone, IncludeChildrenMode includeChildrenMode)
        {
            base.AssignCloneProperties(clone, includeChildrenMode);
            GenericFromBase<T> typedClone = (GenericFromBase<T>) clone;
            /*Location4108*/typedClone.MyInt = MyInt;
            /*Location4109*/if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
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
        
        /* Properties */
        /*Location4111*/
        public override IEnumerable<(string propertyName, ILazinator descendant)> EnumerateLazinatorDescendants(Func<ILazinator, bool> matchCriterion, bool stopExploringBelowMatch, Func<ILazinator, bool> exploreCriterion, bool exploreOnlyDeserializedChildren, bool enumerateNulls)
        {
            foreach (var inheritedYield in base.EnumerateLazinatorDescendants(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
            {
                yield return inheritedYield;
            }
            /*Location4112*/if (enumerateNulls && (!exploreOnlyDeserializedChildren || _MyT_Accessed) && MyT == null)
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
            
            /*Location4113*/yield break;
        }
        /*Location4114*/
        
        public override IEnumerable<(string propertyName, object descendant)> EnumerateNonLazinatorProperties()
        {
            foreach (var inheritedYield in base.EnumerateNonLazinatorProperties())
            {
                yield return inheritedYield;
            }
            /*Location4115*/yield return ("MyInt", (object)MyInt);
            /*Location4116*/yield break;
        }
        /*Location4117*/
        public override ILazinator ForEachLazinator(Func<ILazinator, ILazinator> changeFunc, bool exploreOnlyDeserializedChildren, bool changeThisLevel)
        {
            base.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, false);
            /*Location4118*/if ((!exploreOnlyDeserializedChildren && MyT != null) || (_MyT_Accessed && _MyT != null))
            {
                _MyT = (T) _MyT.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, true);
            }
            /*Location4119*/if (changeThisLevel && changeFunc != null)
            {
                return changeFunc(this);
            }
            return this;
        }
        /*Location4120*/
        public override void FreeInMemoryObjects()
        {
            base.FreeInMemoryObjects();
            _MyT = default;
            _MyT_Accessed = false;
            IsDirty = false;
            DescendantIsDirty = false;
            HasChanged = false;
            DescendantHasChanged = false;
        }
        /*Location4121*/
        /* Conversion */
        
        public override int LazinatorUniqueID => 1067;
        
        protected override bool ContainsOpenGenericParameters => true;
        public override LazinatorGenericIDType LazinatorGenericID => LazinatorGenericIDType.GetCachedForType<GenericFromBase<T>>(() => DeserializationFactory.Instance.GetUniqueIDListForGenericType(1067, new Type[] { typeof(T) }));
        
        
        public override int LazinatorObjectVersion { get; set; } = 0;
        
        
        /*Location4122*/public override void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            base.ConvertFromBytesAfterHeader(OriginalIncludeChildrenMode, serializedVersionNumber, ref bytesSoFar);
            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
            /*Location4123*/_MyInt = span.ToDecompressedInt(ref bytesSoFar);
            /*Location4124*/_MyT_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            
            /*Location4125*/_GenericFromBase_T_EndByteIndex = bytesSoFar;
            /*Location4126*/        }
            
            /*Location4127*/public override void SerializeExistingBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
            {
                /*Location4128*/if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren)
                {
                    updateStoredBuffer = false;
                }
                /*Location4129*/int startPosition = writer.Position;
                WritePropertiesIntoBuffer(ref writer, includeChildrenMode, verifyCleanness, updateStoredBuffer, true);
                /*Location4130*/if (updateStoredBuffer)
                {
                    UpdateStoredBuffer(ref writer, startPosition, writer.Position - startPosition, includeChildrenMode, false);
                    /*Location4131*/}
                    /*Location4132*/}
                    /*Location4133*/
                    public override void UpdateStoredBuffer(ref BinaryBufferWriter writer, int startPosition, int length, IncludeChildrenMode includeChildrenMode, bool updateDeserializedChildren)
                    {
                        /*Location4134*/_IsDirty = false;
                        if (includeChildrenMode == IncludeChildrenMode.IncludeAllChildren)
                        {
                            _DescendantIsDirty = false;/*Location4135*/
                            if (updateDeserializedChildren)
                            {
                                UpdateDeserializedChildren(ref writer, startPosition);
                                /*Location4136*/}
                                /*Location4137*/
                                if (_MyT_Accessed && _MyT != null && _MyT.IsStruct && (_MyT.IsDirty || _MyT.DescendantIsDirty))
                                {
                                    _MyT_Accessed = false;
                                }/*Location4138*/
                            }
                            else
                            {
                                ThrowHelper.ThrowCannotUpdateStoredBuffer();
                            }
                            /*Location4139*/
                            var newBuffer = writer.Slice(startPosition, length);
                            LazinatorMemoryStorage = newBuffer;
                            /*Location4140*/}
                            /*Location4141*/
                            protected override void UpdateDeserializedChildren(ref BinaryBufferWriter writer, int startPosition)
                            {
                                base.UpdateDeserializedChildren(ref writer, startPosition);
                                /*Location4142*/if (_MyT_Accessed && _MyT != null)
                                {
                                    MyT.UpdateStoredBuffer(ref writer, startPosition + _MyT_ByteIndex + sizeof(int), _MyT_ByteLength - sizeof(int), IncludeChildrenMode.IncludeAllChildren, true);
                                }
                                
                                /*Location4143*/}
                                
                                /*Location4144*/
                                protected override void WritePropertiesIntoBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer, bool includeUniqueID)
                                {
                                    int startPosition = writer.Position;
                                    int startOfObjectPosition = 0;
                                    base.WritePropertiesIntoBuffer(ref writer, includeChildrenMode, verifyCleanness, updateStoredBuffer, includeUniqueID);
                                    /*Location4145*/// write properties
                                    /*Location4146*/CompressedIntegralTypes.WriteCompressedInt(ref writer, _MyInt);
                                    /*Location4147*/startOfObjectPosition = writer.Position;
                                    /*Location4148*/if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
                                    {
                                        if ((includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode) && !_MyT_Accessed)
                                        {
                                            var deserialized = MyT;
                                        }
                                        WriteChild(ref writer, ref _MyT, includeChildrenMode, _MyT_Accessed, () => GetChildSlice(LazinatorMemoryStorage, _MyT_ByteIndex, _MyT_ByteLength, false, false, null), verifyCleanness, updateStoredBuffer, false, false, this);
                                    }
                                    
                                    /*Location4149*/if (updateStoredBuffer)
                                    {
                                        _MyT_ByteIndex = startOfObjectPosition - startPosition;
                                    }
                                    /*Location4150*/if (updateStoredBuffer)
                                    {
                                        /*Location4151*/_GenericFromBase_T_EndByteIndex = writer.Position - startPosition;
                                        /*Location4152*/}
                                        /*Location4153*/}
                                        /*Location4154*/
                                    }
                                }
