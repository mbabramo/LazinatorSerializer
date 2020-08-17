/*Location1118*//*Location1103*///de7a213e-74fe-8448-8ad7-cbe77e8b762f
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
{/*Location1104*/
    using Lazinator.Attributes;/*Location1105*/
    using Lazinator.Buffers;/*Location1106*/
    using Lazinator.Core;/*Location1107*/
    using Lazinator.Exceptions;/*Location1108*/
    using Lazinator.Support;/*Location1109*/
    using LazinatorTests.Examples;/*Location1110*/
    using System;/*Location1111*/
    using System.Buffers;/*Location1112*/
    using System.Collections.Generic;/*Location1113*/
    using System.Diagnostics;/*Location1114*/
    using System.IO;/*Location1115*/
    using System.Linq;/*Location1116*/
    using System.Runtime.InteropServices;/*Location1117*/
    using static Lazinator.Core.LazinatorUtilities;
    
    [Autogenerated]
    public partial class ExampleInterfaceContainer : ILazinator
    {
        /*Location1119*/public bool IsStruct => false;
        
        /*Location1120*//* Property definitions */
        
        /*Location1121*/        protected int _ExampleByInterface_ByteIndex;
        /*Location1122*/        protected int _ExampleListByInterface_ByteIndex;
        /*Location1123*/protected virtual int _ExampleByInterface_ByteLength => _ExampleListByInterface_ByteIndex - _ExampleByInterface_ByteIndex;
        /*Location1124*/private int _ExampleInterfaceContainer_EndByteIndex;
        /*Location1125*/protected virtual int _ExampleListByInterface_ByteLength => _ExampleInterfaceContainer_EndByteIndex - _ExampleListByInterface_ByteIndex;
        
        /*Location1126*/
        protected IExample _ExampleByInterface;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public IExample ExampleByInterface
        {
            get
            {
                if (!_ExampleByInterface_Accessed)
                {
                    Lazinate_ExampleByInterface();
                } 
                return _ExampleByInterface;
            }
            set
            {
                if (_ExampleByInterface != null)
                {
                    _ExampleByInterface.LazinatorParents = _ExampleByInterface.LazinatorParents.WithRemoved(this);
                }
                if (value != null)
                {
                    value.LazinatorParents = value.LazinatorParents.WithAdded(this);
                }
                
                IsDirty = true;
                DescendantIsDirty = true;
                _ExampleByInterface = value;
                _ExampleByInterface_Accessed = true;
            }
        }
        protected bool _ExampleByInterface_Accessed;
        private void Lazinate_ExampleByInterface()
        {
            if (LazinatorObjectBytes.Length == 0)
            {
                _ExampleByInterface = null;
            }
            else
            {
                LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _ExampleByInterface_ByteIndex, _ExampleByInterface_ByteLength, false, false, null);
                
                _ExampleByInterface = DeserializationFactory.Instance.CreateBasedOnType<IExample>(childData, this); 
            }
            
            _ExampleByInterface_Accessed = true;
        }
        
        /*Location1127*/
        protected List<IExample> _ExampleListByInterface;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public List<IExample> ExampleListByInterface
        {
            get
            {
                if (!_ExampleListByInterface_Accessed)
                {
                    Lazinate_ExampleListByInterface();
                }
                IsDirty = true; 
                return _ExampleListByInterface;
            }
            set
            {
                IsDirty = true;
                DescendantIsDirty = true;
                _ExampleListByInterface = value;
                _ExampleListByInterface_Accessed = true;
            }
        }
        protected bool _ExampleListByInterface_Accessed;
        private void Lazinate_ExampleListByInterface()
        {
            if (LazinatorObjectBytes.Length == 0)
            {
                _ExampleListByInterface = default(List<IExample>);
            }
            else
            {
                LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _ExampleListByInterface_ByteIndex, _ExampleListByInterface_ByteLength, false, false, null);
                _ExampleListByInterface = ConvertFromBytes_List_GIExample_g(childData);
            }
            
            _ExampleListByInterface_Accessed = true;
        }
        
        /*Location1130*/
        /* Serialization, deserialization, and object relationships */
        
        public ExampleInterfaceContainer(IncludeChildrenMode originalIncludeChildrenMode)
        {
            OriginalIncludeChildrenMode = originalIncludeChildrenMode;
        }
        
        public ExampleInterfaceContainer(LazinatorMemory serializedBytes, ILazinator parent = null)
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
            ExampleInterfaceContainer clone;
            if (cloneBufferOptions == CloneBufferOptions.NoBuffer)
            {
                clone = new ExampleInterfaceContainer(includeChildrenMode);
                clone.LazinatorObjectVersion = LazinatorObjectVersion;
                clone = (ExampleInterfaceContainer)AssignCloneProperties(clone, includeChildrenMode);
            }
            else
            {
                LazinatorMemory bytes = EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, false, IsDirty, DescendantIsDirty, false, LazinatorMemoryStorage, false, this);
                clone = new ExampleInterfaceContainer(bytes);
            }
            return clone;
        }
        
        public virtual ILazinator AssignCloneProperties(ILazinator clone, IncludeChildrenMode includeChildrenMode)
        {
            clone.FreeInMemoryObjects();
            ExampleInterfaceContainer typedClone = (ExampleInterfaceContainer) clone;
            /*Location1128*/if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren)
            {
                if (ExampleByInterface == null)
                {
                    typedClone.ExampleByInterface = null;
                }
                else
                {
                    typedClone.ExampleByInterface = (IExample) ExampleByInterface.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer);
                }
                
            }
            
            /*Location1129*/typedClone.ExampleListByInterface = CloneOrChange_List_GIExample_g(ExampleListByInterface, l => l?.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer), false);
            
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
        
        /*Location1131*/
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
        
        /*Location1132*/public virtual IEnumerable<(string propertyName, ILazinator descendant)> EnumerateLazinatorDescendants(Func<ILazinator, bool> matchCriterion, bool stopExploringBelowMatch, Func<ILazinator, bool> exploreCriterion, bool exploreOnlyDeserializedChildren, bool enumerateNulls)
        {
            /*Location1133*/if (enumerateNulls && (!exploreOnlyDeserializedChildren || _ExampleByInterface_Accessed) && ExampleByInterface == null)
            {
                yield return ("ExampleByInterface", default);
            }
            else
            {
                if ((!exploreOnlyDeserializedChildren && ExampleByInterface != null) || (_ExampleByInterface_Accessed && _ExampleByInterface != null))
                {
                    bool isMatch_ExampleByInterface = matchCriterion == null || matchCriterion(ExampleByInterface);
                    bool shouldExplore_ExampleByInterface = exploreCriterion == null || exploreCriterion(ExampleByInterface);
                    if (isMatch_ExampleByInterface)
                    {
                        yield return ("ExampleByInterface", ExampleByInterface);
                    }
                    if ((!stopExploringBelowMatch || !isMatch_ExampleByInterface) && shouldExplore_ExampleByInterface)
                    {
                        foreach (var toYield in ExampleByInterface.EnumerateLazinatorDescendants(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
                        {
                            yield return ("ExampleByInterface" + "." + toYield.propertyName, toYield.descendant);
                        }
                    }
                }
                
            }
            
            /*Location1134*/yield break;
        }
        /*Location1135*/
        
        public virtual IEnumerable<(string propertyName, object descendant)> EnumerateNonLazinatorProperties()
        {
            /*Location1136*/yield return ("ExampleListByInterface", (object)ExampleListByInterface);
            /*Location1137*/yield break;
        }
        /*Location1138*/
        public virtual ILazinator ForEachLazinator(Func<ILazinator, ILazinator> changeFunc, bool exploreOnlyDeserializedChildren, bool changeThisLevel)
        {
            /*Location1139*/if ((!exploreOnlyDeserializedChildren && ExampleByInterface != null) || (_ExampleByInterface_Accessed && _ExampleByInterface != null))
            {
                _ExampleByInterface = (IExample) _ExampleByInterface.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, true);
            }
            /*Location1140*/if ((!exploreOnlyDeserializedChildren && ExampleListByInterface != null) || (_ExampleListByInterface_Accessed && _ExampleListByInterface != null))
            {
                _ExampleListByInterface = (List<IExample>) CloneOrChange_List_GIExample_g(_ExampleListByInterface, l => l?.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, true), true);
            }
            /*Location1141*/if (changeThisLevel && changeFunc != null)
            {
                return changeFunc(this);
            }
            return this;
        }
        /*Location1142*/
        public virtual void FreeInMemoryObjects()
        {
            _ExampleByInterface = default;
            _ExampleListByInterface = default;
            _ExampleByInterface_Accessed = _ExampleListByInterface_Accessed = false;
            IsDirty = false;
            DescendantIsDirty = false;
            HasChanged = false;
            DescendantHasChanged = false;
        }
        /*Location1143*/
        /* Conversion */
        
        public virtual int LazinatorUniqueID => 1046;
        
        protected virtual bool ContainsOpenGenericParameters => false;
        public virtual LazinatorGenericIDType LazinatorGenericID => default;
        
        
        public virtual int LazinatorObjectVersion { get; set; } = 0;
        
        
        /*Location1144*/public virtual void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
            /*Location1145*/_ExampleByInterface_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren)
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            
            /*Location1146*/_ExampleListByInterface_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            /*Location1147*/_ExampleInterfaceContainer_EndByteIndex = bytesSoFar;
            /*Location1148*/        }
            
            /*Location1149*/public virtual void SerializeExistingBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
            {
                /*Location1150*/if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren)
                {
                    updateStoredBuffer = false;
                }
                /*Location1151*/int startPosition = writer.Position;
                WritePropertiesIntoBuffer(ref writer, includeChildrenMode, verifyCleanness, updateStoredBuffer, true);
                /*Location1152*/if (updateStoredBuffer)
                {
                    UpdateStoredBuffer(ref writer, startPosition, writer.Position - startPosition, includeChildrenMode, false);
                    /*Location1153*/}
                    /*Location1154*/}
                    /*Location1155*/
                    public virtual void UpdateStoredBuffer(ref BinaryBufferWriter writer, int startPosition, int length, IncludeChildrenMode includeChildrenMode, bool updateDeserializedChildren)
                    {
                        /*Location1156*/_IsDirty = false;
                        if (includeChildrenMode == IncludeChildrenMode.IncludeAllChildren)
                        {
                            _DescendantIsDirty = false;/*Location1157*/
                            if (updateDeserializedChildren)
                            {
                                UpdateDeserializedChildren(ref writer, startPosition);
                                /*Location1158*/}
                                /*Location1159*//*Location1160*/
                            }
                            else
                            {
                                ThrowHelper.ThrowCannotUpdateStoredBuffer();
                            }
                            /*Location1161*/
                            var newBuffer = writer.Slice(startPosition, length);
                            LazinatorMemoryStorage = newBuffer;
                            /*Location1162*/}
                            /*Location1163*/
                            protected virtual void UpdateDeserializedChildren(ref BinaryBufferWriter writer, int startPosition)
                            {
                                /*Location1164*/if (_ExampleByInterface_Accessed && _ExampleByInterface != null)
                                {
                                    ExampleByInterface.UpdateStoredBuffer(ref writer, startPosition + _ExampleByInterface_ByteIndex + sizeof(int), _ExampleByInterface_ByteLength - sizeof(int), IncludeChildrenMode.IncludeAllChildren, true);
                                }
                                
                                /*Location1165*/if (_ExampleListByInterface_Accessed && _ExampleListByInterface != null)
                                {
                                    _ExampleListByInterface = (List<IExample>) CloneOrChange_List_GIExample_g(_ExampleListByInterface, l => l.RemoveBufferInHierarchy(), true);
                                }
                                /*Location1166*/}
                                
                                /*Location1167*/
                                protected virtual void WritePropertiesIntoBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer, bool includeUniqueID)
                                {
                                    int startPosition = writer.Position;
                                    int startOfObjectPosition = 0;
                                    // header information
                                    /*Location1168*/if (includeUniqueID)
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
                                    /*Location1169*/CompressedIntegralTypes.WriteCompressedInt(ref writer, Lazinator.Support.LazinatorVersionInfo.LazinatorIntVersion);
                                    CompressedIntegralTypes.WriteCompressedInt(ref writer, LazinatorObjectVersion);
                                    writer.Write((byte)includeChildrenMode);
                                    /*Location1170*/// write properties
                                    /*Location1171*/startOfObjectPosition = writer.Position;
                                    /*Location1172*/if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren)
                                    {
                                        if ((includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode) && !_ExampleByInterface_Accessed)
                                        {
                                            var deserialized = ExampleByInterface;
                                        }
                                        WriteChild(ref writer, ref _ExampleByInterface, includeChildrenMode, _ExampleByInterface_Accessed, () => GetChildSlice(LazinatorMemoryStorage, _ExampleByInterface_ByteIndex, _ExampleByInterface_ByteLength, false, false, null), verifyCleanness, updateStoredBuffer, false, false, this);
                                    }
                                    
                                    /*Location1173*/if (updateStoredBuffer)
                                    {
                                        _ExampleByInterface_ByteIndex = startOfObjectPosition - startPosition;
                                    }
                                    /*Location1174*/startOfObjectPosition = writer.Position;
                                    /*Location1175*/if ((includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode) && !_ExampleListByInterface_Accessed)
                                    {
                                        var deserialized = ExampleListByInterface;
                                    }
                                    /*Location1176*/WriteNonLazinatorObject(
                                    nonLazinatorObject: _ExampleListByInterface, isBelievedDirty: _ExampleListByInterface_Accessed || (includeChildrenMode != OriginalIncludeChildrenMode),
                                    isAccessed: _ExampleListByInterface_Accessed, writer: ref writer,
                                    getChildSliceForFieldFn: () => GetChildSlice(LazinatorMemoryStorage, _ExampleListByInterface_ByteIndex, _ExampleListByInterface_ByteLength, false, false, null),
                                    verifyCleanness: false,
                                    binaryWriterAction: (ref BinaryBufferWriter w, bool v) =>
                                    ConvertToBytes_List_GIExample_g(ref w, _ExampleListByInterface,
                                    includeChildrenMode, v, updateStoredBuffer));
                                    /*Location1177*/if (updateStoredBuffer)
                                    {
                                        _ExampleListByInterface_ByteIndex = startOfObjectPosition - startPosition;
                                    }
                                    /*Location1178*/if (updateStoredBuffer)
                                    {
                                        /*Location1179*/_ExampleInterfaceContainer_EndByteIndex = writer.Position - startPosition;
                                        /*Location1180*/}
                                        /*Location1181*/}
                                        /*Location1182*/
                                        /* Conversion of supported collections and tuples */
                                        /*Location1183*/
                                        private static List<IExample> ConvertFromBytes_List_GIExample_g(LazinatorMemory storage)
                                        {
                                            if (storage.Length == 0)
                                            {
                                                return default(List<IExample>);
                                            }
                                            ReadOnlySpan<byte> span = storage.Span;
                                            int bytesSoFar = 0;
                                            int collectionLength = span.ToDecompressedInt(ref bytesSoFar);
                                            
                                            List<IExample> collection = new List<IExample>(collectionLength);
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
                                                    var item = DeserializationFactory.Instance.CreateBasedOnType<IExample>(childData);
                                                    collection.Add(item);
                                                }
                                                bytesSoFar += lengthCollectionMember;
                                            }
                                            
                                            return collection;
                                        }/*Location1184*/
                                        
                                        private static void ConvertToBytes_List_GIExample_g(ref BinaryBufferWriter writer, List<IExample> itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
                                        {
                                            if (itemToConvert == default(List<IExample>))
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
                                        /*Location1185*/
                                        private static List<IExample> CloneOrChange_List_GIExample_g(List<IExample> itemToClone, Func<ILazinator, ILazinator> cloneOrChangeFunc, bool avoidCloningIfPossible)
                                        {
                                            if (itemToClone == null)
                                            {
                                                return default;
                                            }
                                            
                                            int collectionLength = itemToClone.Count;
                                            List<IExample> collection = avoidCloningIfPossible ? itemToClone : new List<IExample>(collectionLength);
                                            int itemToCloneCount = itemToClone.Count;
                                            for (int itemIndex = 0; itemIndex < itemToCloneCount; itemIndex++)
                                            {
                                                if (avoidCloningIfPossible)
                                                {
                                                    if (itemToClone[itemIndex] != null)
                                                    {
                                                        itemToClone[itemIndex] = (IExample) (cloneOrChangeFunc(itemToClone[itemIndex]));
                                                    }
                                                    continue;
                                                }
                                                if (itemToClone[itemIndex] == null)
                                                {
                                                    collection.Add(null);
                                                }
                                                else
                                                {
                                                    var itemCopied = (IExample) (cloneOrChangeFunc(itemToClone[itemIndex]));
                                                    collection.Add(itemCopied);
                                                }
                                                
                                            }
                                            return collection;
                                        }
                                        /*Location1186*/
                                    }
                                }
