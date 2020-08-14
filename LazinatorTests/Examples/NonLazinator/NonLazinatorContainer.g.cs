/*Location4141*//*Location4127*///756e8677-2cf9-6ca4-1b41-0a2f4719fa46
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.380
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

#nullable disable
namespace LazinatorTests.Examples
{/*Location4128*/
    using Lazinator.Attributes;/*Location4129*/
    using Lazinator.Buffers;/*Location4130*/
    using Lazinator.Core;/*Location4131*/
    using Lazinator.Exceptions;/*Location4132*/
    using Lazinator.Support;/*Location4133*/
    using System;/*Location4134*/
    using System.Buffers;/*Location4135*/
    using System.Collections.Generic;/*Location4136*/
    using System.Diagnostics;/*Location4137*/
    using System.IO;/*Location4138*/
    using System.Linq;/*Location4139*/
    using System.Runtime.InteropServices;/*Location4140*/
    using static Lazinator.Core.LazinatorUtilities;
    
    [Autogenerated]
    public partial struct NonLazinatorContainer : ILazinator
    {
        /*Location4142*/public bool IsStruct => true;
        
        /*Location4143*//* Property definitions */
        
        /*Location4144*/        int _NonLazinatorClass_ByteIndex;
        /*Location4145*/        int _NonLazinatorInterchangeableClass_ByteIndex;
        /*Location4146*/        int _NonLazinatorInterchangeableStruct_ByteIndex;
        /*Location4147*/        int _NonLazinatorStruct_ByteIndex;
        /*Location4148*/int _NonLazinatorClass_ByteLength => _NonLazinatorInterchangeableClass_ByteIndex - _NonLazinatorClass_ByteIndex;
        /*Location4149*/int _NonLazinatorInterchangeableClass_ByteLength => _NonLazinatorInterchangeableStruct_ByteIndex - _NonLazinatorInterchangeableClass_ByteIndex;
        /*Location4150*/int _NonLazinatorInterchangeableStruct_ByteLength => _NonLazinatorStruct_ByteIndex - _NonLazinatorInterchangeableStruct_ByteIndex;
        /*Location4151*/private int _NonLazinatorContainer_EndByteIndex;
        /*Location4152*/int _NonLazinatorStruct_ByteLength => _NonLazinatorContainer_EndByteIndex - _NonLazinatorStruct_ByteIndex;
        
        /*Location4153*/
        NonLazinatorClass _NonLazinatorClass;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public NonLazinatorClass NonLazinatorClass
        {
            get
            {
                if (!_NonLazinatorClass_Accessed)
                {
                    Lazinate_NonLazinatorClass();
                }
                IsDirty = true; 
                return _NonLazinatorClass;
            }
            set
            {
                IsDirty = true;
                DescendantIsDirty = true;
                _NonLazinatorClass = value;
                _NonLazinatorClass_Accessed = true;
            }
        }
        bool _NonLazinatorClass_Accessed;
        private void Lazinate_NonLazinatorClass()
        {
            if (LazinatorObjectBytes.Length == 0)
            {
                _NonLazinatorClass = default(NonLazinatorClass);
            }
            else
            {
                LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _NonLazinatorClass_ByteIndex, _NonLazinatorClass_ByteLength, false, false, null);
                _NonLazinatorClass = NonLazinatorDirectConverter.ConvertFromBytes_NonLazinatorClass(childData);
            }
            
            _NonLazinatorClass_Accessed = true;
        }
        
        /*Location4154*/
        NonLazinatorInterchangeableClass _NonLazinatorInterchangeableClass;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public NonLazinatorInterchangeableClass NonLazinatorInterchangeableClass
        {
            get
            {
                if (!_NonLazinatorInterchangeableClass_Accessed)
                {
                    Lazinate_NonLazinatorInterchangeableClass();
                }
                IsDirty = true; 
                return _NonLazinatorInterchangeableClass;
            }
            set
            {
                IsDirty = true;
                DescendantIsDirty = true;
                _NonLazinatorInterchangeableClass = value;
                _NonLazinatorInterchangeableClass_Accessed = true;
            }
        }
        bool _NonLazinatorInterchangeableClass_Accessed;
        private void Lazinate_NonLazinatorInterchangeableClass()
        {
            if (LazinatorObjectBytes.Length == 0)
            {
                _NonLazinatorInterchangeableClass = default(NonLazinatorInterchangeableClass);
            }
            else
            {
                LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _NonLazinatorInterchangeableClass_ByteIndex, _NonLazinatorInterchangeableClass_ByteLength, false, false, null);
                _NonLazinatorInterchangeableClass = ConvertFromBytes_NonLazinatorInterchangeableClass(childData);
            }
            
            _NonLazinatorInterchangeableClass_Accessed = true;
        }
        
        /*Location4155*/
        NonLazinatorInterchangeableStruct _NonLazinatorInterchangeableStruct;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public NonLazinatorInterchangeableStruct NonLazinatorInterchangeableStruct
        {
            get
            {
                if (!_NonLazinatorInterchangeableStruct_Accessed)
                {
                    Lazinate_NonLazinatorInterchangeableStruct();
                } 
                return _NonLazinatorInterchangeableStruct;
            }
            set
            {
                IsDirty = true;
                DescendantIsDirty = true;
                _NonLazinatorInterchangeableStruct = value;
                _NonLazinatorInterchangeableStruct_Accessed = true;
            }
        }
        bool _NonLazinatorInterchangeableStruct_Accessed;
        private void Lazinate_NonLazinatorInterchangeableStruct()
        {
            if (LazinatorObjectBytes.Length == 0)
            {
                _NonLazinatorInterchangeableStruct = default(NonLazinatorInterchangeableStruct);
            }
            else
            {
                LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _NonLazinatorInterchangeableStruct_ByteIndex, _NonLazinatorInterchangeableStruct_ByteLength, false, false, null);
                _NonLazinatorInterchangeableStruct = ConvertFromBytes_NonLazinatorInterchangeableStruct(childData);
            }
            
            _NonLazinatorInterchangeableStruct_Accessed = true;
        }
        
        /*Location4156*/
        NonLazinatorStruct _NonLazinatorStruct;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public NonLazinatorStruct NonLazinatorStruct
        {
            get
            {
                if (!_NonLazinatorStruct_Accessed)
                {
                    Lazinate_NonLazinatorStruct();
                }
                IsDirty = true; 
                return _NonLazinatorStruct;
            }
            set
            {
                IsDirty = true;
                DescendantIsDirty = true;
                _NonLazinatorStruct = value;
                _NonLazinatorStruct_Accessed = true;
            }
        }
        bool _NonLazinatorStruct_Accessed;
        private void Lazinate_NonLazinatorStruct()
        {
            if (LazinatorObjectBytes.Length == 0)
            {
                _NonLazinatorStruct = default(NonLazinatorStruct);
            }
            else
            {
                LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _NonLazinatorStruct_ByteIndex, _NonLazinatorStruct_ByteLength, false, false, null);
                _NonLazinatorStruct = NonLazinatorDirectConverter.ConvertFromBytes_NonLazinatorStruct(childData);
            }
            
            _NonLazinatorStruct_Accessed = true;
        }
        
        /*Location4161*/
        /* Serialization, deserialization, and object relationships */
        
        public LazinatorParentsCollection LazinatorParents { get; set; }
        
        public IncludeChildrenMode OriginalIncludeChildrenMode { get; set; }
        
        public int Deserialize()
        {
            FreeInMemoryObjects();
            int bytesSoFar = 0;
            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
            if (span.Length == 0)
            {
                return 0;
            }
            
            int uniqueID = span.ToDecompressedInt(ref bytesSoFar);
            if (uniqueID != LazinatorUniqueID)
            {
                ThrowHelper.ThrowFormatException();
            }
            
            int lazinatorLibraryVersion = span.ToDecompressedInt(ref bytesSoFar);
            
            int serializedVersionNumber = span.ToDecompressedInt(ref bytesSoFar);
            
            OriginalIncludeChildrenMode = (IncludeChildrenMode)span.ToByte(ref bytesSoFar);
            
            ConvertFromBytesAfterHeader(OriginalIncludeChildrenMode, serializedVersionNumber, ref bytesSoFar);
            return bytesSoFar;
        }
        
        public LazinatorMemory SerializeLazinator(IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer) 
        {
            if (LazinatorMemoryStorage.IsEmpty || includeChildrenMode != OriginalIncludeChildrenMode || (verifyCleanness || IsDirty || (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && DescendantIsDirty)))
            {
                return EncodeToNewBuffer(includeChildrenMode, verifyCleanness, updateStoredBuffer);
            }
            BinaryBufferWriter writer = new BinaryBufferWriter(LazinatorMemoryStorage.Length);
            writer.Write(LazinatorMemoryStorage.Span);
            return writer.LazinatorMemory;
        }
        
        LazinatorMemory EncodeToNewBuffer(IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer) 
        {
            int bufferSize = LazinatorMemoryStorage.Length == 0 ? ExpandableBytes.DefaultMinBufferSize : LazinatorMemoryStorage.Length;
            BinaryBufferWriter writer = new BinaryBufferWriter(bufferSize);
            SerializeExistingBuffer(ref writer, includeChildrenMode, verifyCleanness, updateStoredBuffer);
            return writer.LazinatorMemory;
        }
        
        public ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode = IncludeChildrenMode.IncludeAllChildren, CloneBufferOptions cloneBufferOptions = CloneBufferOptions.IndependentBuffers)
        {
            var clone = new NonLazinatorContainer()
            {
                OriginalIncludeChildrenMode = includeChildrenMode
            };
            clone = CompleteClone(this, clone, includeChildrenMode, cloneBufferOptions);
            return clone;
        }
        
        public ILazinator AssignCloneProperties(ILazinator clone, IncludeChildrenMode includeChildrenMode)
        {
            clone.FreeInMemoryObjects();
            NonLazinatorContainer typedClone = (NonLazinatorContainer) clone;
            /*Location4157*/typedClone.NonLazinatorClass = NonLazinatorDirectConverter.CloneOrChange_NonLazinatorClass(NonLazinatorClass, l => l?.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer), false);
            /*Location4158*/typedClone.NonLazinatorInterchangeableClass = CloneOrChange_NonLazinatorInterchangeableClass(NonLazinatorInterchangeableClass, l => l.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer), false);
            /*Location4159*/typedClone.NonLazinatorInterchangeableStruct = CloneOrChange_NonLazinatorInterchangeableStruct(NonLazinatorInterchangeableStruct, l => l.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer), false);
            /*Location4160*/typedClone.NonLazinatorStruct = NonLazinatorDirectConverter.CloneOrChange_NonLazinatorStruct(NonLazinatorStruct, l => l?.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer), false);
            
            typedClone.IsDirty = false;
            return typedClone;
        }
        
        public bool HasChanged { get; set; }
        
        bool _IsDirty;
        public bool IsDirty
        {
            [DebuggerStepThrough]
            get => _IsDirty;
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
        
        bool _DescendantHasChanged;
        public bool DescendantHasChanged
        {
            [DebuggerStepThrough]
            get => _DescendantHasChanged;
            [DebuggerStepThrough]
            set
            {
                _DescendantHasChanged = value;
            }
        }
        
        bool _DescendantIsDirty;
        public bool DescendantIsDirty
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
        
        public void DeserializeLazinator(LazinatorMemory serializedBytes)
        {
            LazinatorMemoryStorage = serializedBytes;
            int length = Deserialize();
            if (length != LazinatorMemoryStorage.Length)
            {
                LazinatorMemoryStorage = LazinatorMemoryStorage.Slice(0, length);
            }
        }
        
        public LazinatorMemory LazinatorMemoryStorage
        {
            get;
            set;
        }
        ReadOnlyMemory<byte> LazinatorObjectBytes => LazinatorMemoryStorage.IsEmpty ? LazinatorMemory.EmptyReadOnlyMemory : LazinatorMemoryStorage.Memory;
        
        public void UpdateStoredBuffer()
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
        
        public int GetByteLength()
        {
            UpdateStoredBuffer();
            return LazinatorObjectBytes.Length;
        }
        
        public bool NonBinaryHash32 => false;
        
        /*Location4162*/
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
        
        /*Location4163*/public IEnumerable<(string propertyName, ILazinator descendant)> EnumerateLazinatorDescendants(Func<ILazinator, bool> matchCriterion, bool stopExploringBelowMatch, Func<ILazinator, bool> exploreCriterion, bool exploreOnlyDeserializedChildren, bool enumerateNulls)
        {
            /*Location4164*/yield break;
        }
        /*Location4165*/
        
        public IEnumerable<(string propertyName, object descendant)> EnumerateNonLazinatorProperties()
        {
            /*Location4166*/yield return ("NonLazinatorClass", (object)NonLazinatorClass);
            /*Location4167*/yield return ("NonLazinatorInterchangeableClass", (object)NonLazinatorInterchangeableClass);
            /*Location4168*/yield return ("NonLazinatorInterchangeableStruct", (object)NonLazinatorInterchangeableStruct);
            /*Location4169*/yield return ("NonLazinatorStruct", (object)NonLazinatorStruct);
            /*Location4170*/yield break;
        }
        /*Location4171*/
        public ILazinator ForEachLazinator(Func<ILazinator, ILazinator> changeFunc, bool exploreOnlyDeserializedChildren, bool changeThisLevel)
        {
            /*Location4172*/if ((!exploreOnlyDeserializedChildren && NonLazinatorInterchangeableClass != null) || (_NonLazinatorInterchangeableClass_Accessed && _NonLazinatorInterchangeableClass != null))
            {
                _NonLazinatorInterchangeableClass = (NonLazinatorInterchangeableClass) CloneOrChange_NonLazinatorInterchangeableClass(_NonLazinatorInterchangeableClass, l => l.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, true), true);
            }
            /*Location4173*/var deserialized_NonLazinatorInterchangeableStruct = NonLazinatorInterchangeableStruct;
            _NonLazinatorInterchangeableStruct = (NonLazinatorInterchangeableStruct) CloneOrChange_NonLazinatorInterchangeableStruct(_NonLazinatorInterchangeableStruct, l => l.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, true), true);/*Location4174*/if ((!exploreOnlyDeserializedChildren && NonLazinatorClass != null) || (_NonLazinatorClass_Accessed && _NonLazinatorClass != null))
            {
                _NonLazinatorClass = NonLazinatorDirectConverter.CloneOrChange_NonLazinatorClass(_NonLazinatorClass, l => l?.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, true), true);
            }
            /*Location4175*/var deserialized_NonLazinatorStruct = NonLazinatorStruct;
            _NonLazinatorStruct = NonLazinatorDirectConverter.CloneOrChange_NonLazinatorStruct(_NonLazinatorStruct, l => l?.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, true), true);/*Location4176*/if (changeThisLevel && changeFunc != null)
            {
                return changeFunc(this);
            }
            return this;
        }
        /*Location4177*/
        public void FreeInMemoryObjects()
        {
            _NonLazinatorClass = default;
            _NonLazinatorInterchangeableClass = default;
            _NonLazinatorInterchangeableStruct = default;
            _NonLazinatorStruct = default;
            _NonLazinatorClass_Accessed = _NonLazinatorInterchangeableClass_Accessed = _NonLazinatorInterchangeableStruct_Accessed = _NonLazinatorStruct_Accessed = false;
            IsDirty = false;
            DescendantIsDirty = false;
            HasChanged = false;
            DescendantHasChanged = false;
        }
        /*Location4178*/
        /* Conversion */
        
        public int LazinatorUniqueID => 1032;
        
        bool ContainsOpenGenericParameters => false;
        public LazinatorGenericIDType LazinatorGenericID => default;
        
        
        private bool _LazinatorObjectVersionChanged;
        private int _LazinatorObjectVersionOverride;
        public int LazinatorObjectVersion
        {
            get => _LazinatorObjectVersionChanged ? _LazinatorObjectVersionOverride : 0;
            set
            {
                _LazinatorObjectVersionOverride = value;
                _LazinatorObjectVersionChanged = true;
            }
        }
        
        
        /*Location4179*/public void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
            /*Location4180*/_NonLazinatorClass_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            /*Location4181*/_NonLazinatorInterchangeableClass_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            /*Location4182*/_NonLazinatorInterchangeableStruct_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            /*Location4183*/_NonLazinatorStruct_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            /*Location4184*/_NonLazinatorContainer_EndByteIndex = bytesSoFar;
            /*Location4185*/        }
            
            /*Location4186*/public void SerializeExistingBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
            {
                /*Location4187*/if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren)
                {
                    updateStoredBuffer = false;
                }
                /*Location4188*/int startPosition = writer.Position;
                WritePropertiesIntoBuffer(ref writer, includeChildrenMode, verifyCleanness, updateStoredBuffer, true);
                /*Location4189*/if (updateStoredBuffer)
                {
                    UpdateStoredBuffer(ref writer, startPosition, writer.Position - startPosition, includeChildrenMode, false);
                    /*Location4190*/}
                    /*Location4191*/}
                    /*Location4192*/
                    public void UpdateStoredBuffer(ref BinaryBufferWriter writer, int startPosition, int length, IncludeChildrenMode includeChildrenMode, bool updateDeserializedChildren)
                    {
                        /*Location4193*/_IsDirty = false;
                        if (includeChildrenMode == IncludeChildrenMode.IncludeAllChildren)
                        {
                            _DescendantIsDirty = false;/*Location4194*/
                            if (updateDeserializedChildren)
                            {
                                UpdateDeserializedChildren(ref writer, startPosition);
                                /*Location4195*/}
                                /*Location4196*//*Location4197*/
                            }
                            else
                            {
                                ThrowHelper.ThrowCannotUpdateStoredBuffer();
                            }
                            /*Location4198*/
                            var newBuffer = writer.Slice(startPosition, length);
                            LazinatorMemoryStorage = newBuffer;
                            /*Location4199*/}
                            /*Location4200*/
                            void UpdateDeserializedChildren(ref BinaryBufferWriter writer, int startPosition)
                            {
                                /*Location4201*/if (_NonLazinatorInterchangeableClass_Accessed && _NonLazinatorInterchangeableClass != null)
                                {
                                    _NonLazinatorInterchangeableClass = (NonLazinatorInterchangeableClass) CloneOrChange_NonLazinatorInterchangeableClass(_NonLazinatorInterchangeableClass, l => l.RemoveBufferInHierarchy(), true);
                                }
                                /*Location4202*/_NonLazinatorInterchangeableStruct = (NonLazinatorInterchangeableStruct) CloneOrChange_NonLazinatorInterchangeableStruct(_NonLazinatorInterchangeableStruct, l => l.RemoveBufferInHierarchy(), true);/*Location4203*/}
                                
                                /*Location4204*/
                                void WritePropertiesIntoBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer, bool includeUniqueID)
                                {
                                    int startPosition = writer.Position;
                                    int startOfObjectPosition = 0;
                                    // header information
                                    /*Location4205*/if (includeUniqueID)
                                    {
                                        CompressedIntegralTypes.WriteCompressedInt(ref writer, LazinatorUniqueID);
                                    }
                                    
                                    /*Location4206*/CompressedIntegralTypes.WriteCompressedInt(ref writer, Lazinator.Support.LazinatorVersionInfo.LazinatorIntVersion);
                                    CompressedIntegralTypes.WriteCompressedInt(ref writer, LazinatorObjectVersion);
                                    writer.Write((byte)includeChildrenMode);
                                    /*Location4207*/// write properties
                                    /*Location4208*/startOfObjectPosition = writer.Position;
                                    /*Location4209*/if ((includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode) && !_NonLazinatorClass_Accessed)
                                    {
                                        var deserialized = NonLazinatorClass;
                                    }
                                    /*Location4210*/var serializedBytesCopy_NonLazinatorClass = LazinatorMemoryStorage;
                                    var byteIndexCopy_NonLazinatorClass = _NonLazinatorClass_ByteIndex;
                                    var byteLengthCopy_NonLazinatorClass = _NonLazinatorClass_ByteLength;
                                    var copy_NonLazinatorClass = _NonLazinatorClass;
                                    WriteNonLazinatorObject(
                                    nonLazinatorObject: _NonLazinatorClass, isBelievedDirty: _NonLazinatorClass_Accessed || (includeChildrenMode != OriginalIncludeChildrenMode),
                                    isAccessed: _NonLazinatorClass_Accessed, writer: ref writer,
                                    getChildSliceForFieldFn: () => GetChildSlice(serializedBytesCopy_NonLazinatorClass, byteIndexCopy_NonLazinatorClass, byteLengthCopy_NonLazinatorClass, false, false, null),
                                    verifyCleanness: false,
                                    binaryWriterAction: (ref BinaryBufferWriter w, bool v) =>
                                    NonLazinatorDirectConverter.ConvertToBytes_NonLazinatorClass(ref w, copy_NonLazinatorClass, includeChildrenMode, v, updateStoredBuffer));
                                    /*Location4211*/if (updateStoredBuffer)
                                    {
                                        _NonLazinatorClass_ByteIndex = startOfObjectPosition - startPosition;
                                    }
                                    /*Location4212*/startOfObjectPosition = writer.Position;
                                    /*Location4213*/if ((includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode) && !_NonLazinatorInterchangeableClass_Accessed)
                                    {
                                        var deserialized = NonLazinatorInterchangeableClass;
                                    }
                                    /*Location4214*/var serializedBytesCopy_NonLazinatorInterchangeableClass = LazinatorMemoryStorage;
                                    var byteIndexCopy_NonLazinatorInterchangeableClass = _NonLazinatorInterchangeableClass_ByteIndex;
                                    var byteLengthCopy_NonLazinatorInterchangeableClass = _NonLazinatorInterchangeableClass_ByteLength;
                                    var copy_NonLazinatorInterchangeableClass = _NonLazinatorInterchangeableClass;
                                    WriteNonLazinatorObject(
                                    nonLazinatorObject: _NonLazinatorInterchangeableClass, isBelievedDirty: _NonLazinatorInterchangeableClass_Accessed || (includeChildrenMode != OriginalIncludeChildrenMode),
                                    isAccessed: _NonLazinatorInterchangeableClass_Accessed, writer: ref writer,
                                    getChildSliceForFieldFn: () => GetChildSlice(serializedBytesCopy_NonLazinatorInterchangeableClass, byteIndexCopy_NonLazinatorInterchangeableClass, byteLengthCopy_NonLazinatorInterchangeableClass, false, false, null),
                                    verifyCleanness: false,
                                    binaryWriterAction: (ref BinaryBufferWriter w, bool v) =>
                                    ConvertToBytes_NonLazinatorInterchangeableClass(ref w, copy_NonLazinatorInterchangeableClass, includeChildrenMode, v, updateStoredBuffer));
                                    /*Location4215*/if (updateStoredBuffer)
                                    {
                                        _NonLazinatorInterchangeableClass_ByteIndex = startOfObjectPosition - startPosition;
                                    }
                                    /*Location4216*/startOfObjectPosition = writer.Position;
                                    /*Location4217*/if ((includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode) && !_NonLazinatorInterchangeableStruct_Accessed)
                                    {
                                        var deserialized = NonLazinatorInterchangeableStruct;
                                    }
                                    /*Location4218*/var serializedBytesCopy_NonLazinatorInterchangeableStruct = LazinatorMemoryStorage;
                                    var byteIndexCopy_NonLazinatorInterchangeableStruct = _NonLazinatorInterchangeableStruct_ByteIndex;
                                    var byteLengthCopy_NonLazinatorInterchangeableStruct = _NonLazinatorInterchangeableStruct_ByteLength;
                                    var copy_NonLazinatorInterchangeableStruct = _NonLazinatorInterchangeableStruct;
                                    WriteNonLazinatorObject(
                                    nonLazinatorObject: _NonLazinatorInterchangeableStruct, isBelievedDirty: _NonLazinatorInterchangeableStruct_Accessed || (includeChildrenMode != OriginalIncludeChildrenMode),
                                    isAccessed: _NonLazinatorInterchangeableStruct_Accessed, writer: ref writer,
                                    getChildSliceForFieldFn: () => GetChildSlice(serializedBytesCopy_NonLazinatorInterchangeableStruct, byteIndexCopy_NonLazinatorInterchangeableStruct, byteLengthCopy_NonLazinatorInterchangeableStruct, false, false, null),
                                    verifyCleanness: false,
                                    binaryWriterAction: (ref BinaryBufferWriter w, bool v) =>
                                    ConvertToBytes_NonLazinatorInterchangeableStruct(ref w, copy_NonLazinatorInterchangeableStruct, includeChildrenMode, v, updateStoredBuffer));
                                    /*Location4219*/if (updateStoredBuffer)
                                    {
                                        _NonLazinatorInterchangeableStruct_ByteIndex = startOfObjectPosition - startPosition;
                                    }
                                    /*Location4220*/startOfObjectPosition = writer.Position;
                                    /*Location4221*/if ((includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode) && !_NonLazinatorStruct_Accessed)
                                    {
                                        var deserialized = NonLazinatorStruct;
                                    }
                                    /*Location4222*/var serializedBytesCopy_NonLazinatorStruct = LazinatorMemoryStorage;
                                    var byteIndexCopy_NonLazinatorStruct = _NonLazinatorStruct_ByteIndex;
                                    var byteLengthCopy_NonLazinatorStruct = _NonLazinatorStruct_ByteLength;
                                    var copy_NonLazinatorStruct = _NonLazinatorStruct;
                                    WriteNonLazinatorObject(
                                    nonLazinatorObject: _NonLazinatorStruct, isBelievedDirty: _NonLazinatorStruct_Accessed || (includeChildrenMode != OriginalIncludeChildrenMode),
                                    isAccessed: _NonLazinatorStruct_Accessed, writer: ref writer,
                                    getChildSliceForFieldFn: () => GetChildSlice(serializedBytesCopy_NonLazinatorStruct, byteIndexCopy_NonLazinatorStruct, byteLengthCopy_NonLazinatorStruct, false, false, null),
                                    verifyCleanness: false,
                                    binaryWriterAction: (ref BinaryBufferWriter w, bool v) =>
                                    NonLazinatorDirectConverter.ConvertToBytes_NonLazinatorStruct(ref w, copy_NonLazinatorStruct, includeChildrenMode, v, updateStoredBuffer));
                                    /*Location4223*/if (updateStoredBuffer)
                                    {
                                        _NonLazinatorStruct_ByteIndex = startOfObjectPosition - startPosition;
                                    }
                                    /*Location4224*/if (updateStoredBuffer)
                                    {
                                        /*Location4225*/_NonLazinatorContainer_EndByteIndex = writer.Position - startPosition;
                                        /*Location4226*/}
                                        /*Location4227*/}
                                        /*Location4228*/
                                        private static NonLazinatorInterchangeableClass ConvertFromBytes_NonLazinatorInterchangeableClass(LazinatorMemory storage)
                                        {
                                            if (storage.Length == 0)
                                            {
                                                return default(NonLazinatorInterchangeableClass);
                                            }
                                            NonLazinatorInterchangeClass interchange = new NonLazinatorInterchangeClass();
                                            interchange.DeserializeLazinator(storage);
                                            return interchange.Interchange_NonLazinatorInterchangeableClass(false);
                                        }
                                        
                                        private static void ConvertToBytes_NonLazinatorInterchangeableClass(ref BinaryBufferWriter writer,
                                        NonLazinatorInterchangeableClass itemToConvert, IncludeChildrenMode includeChildrenMode,
                                        bool verifyCleanness, bool updateStoredBuffer)
                                        {
                                            if (itemToConvert == null)
                                            {
                                                return;
                                            }
                                            
                                            NonLazinatorInterchangeClass interchange = new NonLazinatorInterchangeClass(itemToConvert);
                                            interchange.SerializeExistingBuffer(ref writer, includeChildrenMode, verifyCleanness, updateStoredBuffer);
                                        }
                                        
                                        
                                        private static NonLazinatorInterchangeableClass CloneOrChange_NonLazinatorInterchangeableClass(NonLazinatorInterchangeableClass itemToClone, Func<ILazinator, ILazinator> cloneOrChangeFunc, bool avoidCloningIfPossible)
                                        {
                                            if (itemToClone == null)
                                            {
                                                return default(NonLazinatorInterchangeableClass);
                                            }
                                            
                                            NonLazinatorInterchangeClass interchange = new NonLazinatorInterchangeClass(itemToClone);
                                            return interchange.Interchange_NonLazinatorInterchangeableClass(avoidCloningIfPossible ? false : true);
                                        }
                                        /*Location4229*/
                                        private static NonLazinatorInterchangeableStruct ConvertFromBytes_NonLazinatorInterchangeableStruct(LazinatorMemory storage)
                                        {
                                            NonLazinatorInterchangeStruct interchange = new NonLazinatorInterchangeStruct();
                                            interchange.DeserializeLazinator(storage);
                                            return interchange.Interchange_NonLazinatorInterchangeableStruct(false);
                                        }
                                        
                                        private static void ConvertToBytes_NonLazinatorInterchangeableStruct(ref BinaryBufferWriter writer,
                                        NonLazinatorInterchangeableStruct itemToConvert, IncludeChildrenMode includeChildrenMode,
                                        bool verifyCleanness, bool updateStoredBuffer)
                                        {
                                            
                                            NonLazinatorInterchangeStruct interchange = new NonLazinatorInterchangeStruct(itemToConvert);
                                            interchange.SerializeExistingBuffer(ref writer, includeChildrenMode, verifyCleanness, updateStoredBuffer);
                                        }
                                        
                                        
                                        private static NonLazinatorInterchangeableStruct CloneOrChange_NonLazinatorInterchangeableStruct(NonLazinatorInterchangeableStruct itemToClone, Func<ILazinator, ILazinator> cloneOrChangeFunc, bool avoidCloningIfPossible)
                                        {
                                            NonLazinatorInterchangeStruct interchange = new NonLazinatorInterchangeStruct(itemToClone);
                                            return interchange.Interchange_NonLazinatorInterchangeableStruct(avoidCloningIfPossible ? false : true);
                                        }
                                        /*Location4230*/
                                    }
                                }
