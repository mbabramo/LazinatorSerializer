/*Location7281*//*Location7267*///bbce8323-6d23-5b3c-1198-afd0832fbb69
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.380
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

#nullable disable
namespace LazinatorTests.Examples.Subclasses
{/*Location7268*/
    using Lazinator.Attributes;/*Location7269*/
    using Lazinator.Buffers;/*Location7270*/
    using Lazinator.Core;/*Location7271*/
    using Lazinator.Exceptions;/*Location7272*/
    using Lazinator.Support;/*Location7273*/
    using System;/*Location7274*/
    using System.Buffers;/*Location7275*/
    using System.Collections.Generic;/*Location7276*/
    using System.Diagnostics;/*Location7277*/
    using System.IO;/*Location7278*/
    using System.Linq;/*Location7279*/
    using System.Runtime.InteropServices;/*Location7280*/
    using static Lazinator.Core.LazinatorUtilities;
    
    [Autogenerated]
    public partial class ClassWithLocalEnum : ILazinator
    {
        /*Location7282*/public bool IsStruct => false;
        
        /*Location7283*//* Property definitions */
        
        /*Location7284*/        protected int _MyEnumList_ByteIndex;
        /*Location7285*/private int _ClassWithLocalEnum_EndByteIndex;
        /*Location7286*/protected virtual int _MyEnumList_ByteLength => _ClassWithLocalEnum_EndByteIndex - _MyEnumList_ByteIndex;
        
        /*Location7287*/
        protected global::LazinatorTests.Examples.Subclasses.ClassWithLocalEnum.EnumWithinClass _MyEnum;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public global::LazinatorTests.Examples.Subclasses.ClassWithLocalEnum.EnumWithinClass MyEnum
        {
            get
            {
                return _MyEnum;
            }
            set
            {
                IsDirty = true;
                _MyEnum = value;
            }
        }
        /*Location7288*/
        protected List<EnumWithinClass> _MyEnumList;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public List<EnumWithinClass> MyEnumList
        {
            get
            {
                if (!_MyEnumList_Accessed)
                {
                    Lazinate_MyEnumList();
                }
                IsDirty = true; 
                return _MyEnumList;
            }
            set
            {
                IsDirty = true;
                DescendantIsDirty = true;
                _MyEnumList = value;
                _MyEnumList_Accessed = true;
            }
        }
        protected bool _MyEnumList_Accessed;
        private void Lazinate_MyEnumList()
        {
            if (LazinatorObjectBytes.Length == 0)
            {
                _MyEnumList = default(List<EnumWithinClass>);
            }
            else
            {
                LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _MyEnumList_ByteIndex, _MyEnumList_ByteLength, false, false, null);
                _MyEnumList = ConvertFromBytes_List_GEnumWithinClass_g(childData);
            }
            
            _MyEnumList_Accessed = true;
        }
        
        /*Location7291*/
        /* Serialization, deserialization, and object relationships */
        
        public ClassWithLocalEnum(LazinatorConstructorEnum constructorEnum)
        {
        }
        
        public ClassWithLocalEnum(LazinatorMemory serializedBytes, ILazinator parent = null)
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
            var clone = new ClassWithLocalEnum(LazinatorConstructorEnum.LazinatorConstructor)
            {
                OriginalIncludeChildrenMode = includeChildrenMode
            };
            clone = CompleteClone(this, clone, includeChildrenMode, cloneBufferOptions);
            return clone;
        }
        
        public virtual ILazinator AssignCloneProperties(ILazinator clone, IncludeChildrenMode includeChildrenMode)
        {
            clone.FreeInMemoryObjects();
            ClassWithLocalEnum typedClone = (ClassWithLocalEnum) clone;
            /*Location7289*/typedClone.MyEnum = MyEnum;
            /*Location7290*/typedClone.MyEnumList = CloneOrChange_List_GEnumWithinClass_g(MyEnumList, l => l?.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer), false);
            
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
        
        /*Location7292*/
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
        
        /*Location7293*/public virtual IEnumerable<(string propertyName, ILazinator descendant)> EnumerateLazinatorDescendants(Func<ILazinator, bool> matchCriterion, bool stopExploringBelowMatch, Func<ILazinator, bool> exploreCriterion, bool exploreOnlyDeserializedChildren, bool enumerateNulls)
        {
            /*Location7294*/yield break;
        }
        /*Location7295*/
        
        public virtual IEnumerable<(string propertyName, object descendant)> EnumerateNonLazinatorProperties()
        {
            /*Location7296*/yield return ("MyEnum", (object)MyEnum);
            /*Location7297*/yield return ("MyEnumList", (object)MyEnumList);
            /*Location7298*/yield break;
        }
        /*Location7299*/
        public virtual ILazinator ForEachLazinator(Func<ILazinator, ILazinator> changeFunc, bool exploreOnlyDeserializedChildren, bool changeThisLevel)
        {
            /*Location7300*/if ((!exploreOnlyDeserializedChildren && MyEnumList != null) || (_MyEnumList_Accessed && _MyEnumList != null))
            {
                _MyEnumList = (List<EnumWithinClass>) CloneOrChange_List_GEnumWithinClass_g(_MyEnumList, l => l?.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, true), true);
            }
            /*Location7301*/if (changeThisLevel && changeFunc != null)
            {
                return changeFunc(this);
            }
            return this;
        }
        /*Location7302*/
        public virtual void FreeInMemoryObjects()
        {
            _MyEnumList = default;
            _MyEnumList_Accessed = false;
            IsDirty = false;
            DescendantIsDirty = false;
            HasChanged = false;
            DescendantHasChanged = false;
        }
        /*Location7303*/
        /* Conversion */
        
        public virtual int LazinatorUniqueID => 1055;
        
        protected virtual bool ContainsOpenGenericParameters => false;
        public virtual LazinatorGenericIDType LazinatorGenericID => default;
        
        
        public virtual int LazinatorObjectVersion { get; set; } = 0;
        
        
        /*Location7304*/public virtual void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
            /*Location7305*/_MyEnum = (global::LazinatorTests.Examples.Subclasses.ClassWithLocalEnum.EnumWithinClass)span.ToDecompressedInt(ref bytesSoFar);
            /*Location7306*/_MyEnumList_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            /*Location7307*/_ClassWithLocalEnum_EndByteIndex = bytesSoFar;
            /*Location7308*/        }
            
            /*Location7309*/public virtual void SerializeExistingBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
            {
                /*Location7310*/if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren)
                {
                    updateStoredBuffer = false;
                }
                /*Location7311*/int startPosition = writer.Position;
                WritePropertiesIntoBuffer(ref writer, includeChildrenMode, verifyCleanness, updateStoredBuffer, true);
                /*Location7312*/if (updateStoredBuffer)
                {
                    UpdateStoredBuffer(ref writer, startPosition, writer.Position - startPosition, includeChildrenMode, false);
                    /*Location7313*/}
                    /*Location7314*/}
                    /*Location7315*/
                    public virtual void UpdateStoredBuffer(ref BinaryBufferWriter writer, int startPosition, int length, IncludeChildrenMode includeChildrenMode, bool updateDeserializedChildren)
                    {
                        /*Location7316*/_IsDirty = false;
                        if (includeChildrenMode == IncludeChildrenMode.IncludeAllChildren)
                        {
                            _DescendantIsDirty = false;/*Location7317*/
                            if (updateDeserializedChildren)
                            {
                                UpdateDeserializedChildren(ref writer, startPosition);
                                /*Location7318*/}
                                /*Location7319*//*Location7320*/
                            }
                            else
                            {
                                ThrowHelper.ThrowCannotUpdateStoredBuffer();
                            }
                            /*Location7321*/
                            var newBuffer = writer.Slice(startPosition, length);
                            LazinatorMemoryStorage = newBuffer;
                            /*Location7322*/}
                            /*Location7323*/
                            protected virtual void UpdateDeserializedChildren(ref BinaryBufferWriter writer, int startPosition)
                            {
                                /*Location7324*/if (_MyEnumList_Accessed && _MyEnumList != null)
                                {
                                    _MyEnumList = (List<EnumWithinClass>) CloneOrChange_List_GEnumWithinClass_g(_MyEnumList, l => l.RemoveBufferInHierarchy(), true);
                                }
                                /*Location7325*/}
                                
                                /*Location7326*/
                                protected virtual void WritePropertiesIntoBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer, bool includeUniqueID)
                                {
                                    int startPosition = writer.Position;
                                    int startOfObjectPosition = 0;
                                    // header information
                                    /*Location7327*/if (includeUniqueID)
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
                                    /*Location7328*/CompressedIntegralTypes.WriteCompressedInt(ref writer, Lazinator.Support.LazinatorVersionInfo.LazinatorIntVersion);
                                    CompressedIntegralTypes.WriteCompressedInt(ref writer, LazinatorObjectVersion);
                                    writer.Write((byte)includeChildrenMode);
                                    /*Location7329*/// write properties
                                    /*Location7330*/CompressedIntegralTypes.WriteCompressedInt(ref writer, (int) _MyEnum);
                                    /*Location7331*/startOfObjectPosition = writer.Position;
                                    /*Location7332*/if ((includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode) && !_MyEnumList_Accessed)
                                    {
                                        var deserialized = MyEnumList;
                                    }
                                    /*Location7333*/WriteNonLazinatorObject(
                                    nonLazinatorObject: _MyEnumList, isBelievedDirty: _MyEnumList_Accessed || (includeChildrenMode != OriginalIncludeChildrenMode),
                                    isAccessed: _MyEnumList_Accessed, writer: ref writer,
                                    getChildSliceForFieldFn: () => GetChildSlice(LazinatorMemoryStorage, _MyEnumList_ByteIndex, _MyEnumList_ByteLength, false, false, null),
                                    verifyCleanness: false,
                                    binaryWriterAction: (ref BinaryBufferWriter w, bool v) =>
                                    ConvertToBytes_List_GEnumWithinClass_g(ref w, _MyEnumList,
                                    includeChildrenMode, v, updateStoredBuffer));
                                    /*Location7334*/if (updateStoredBuffer)
                                    {
                                        _MyEnumList_ByteIndex = startOfObjectPosition - startPosition;
                                    }
                                    /*Location7335*/if (updateStoredBuffer)
                                    {
                                        /*Location7336*/_ClassWithLocalEnum_EndByteIndex = writer.Position - startPosition;
                                        /*Location7337*/}
                                        /*Location7338*/}
                                        /*Location7339*/
                                        /* Conversion of supported collections and tuples */
                                        /*Location7340*/
                                        private static List<EnumWithinClass> ConvertFromBytes_List_GEnumWithinClass_g(LazinatorMemory storage)
                                        {
                                            if (storage.Length == 0)
                                            {
                                                return default(List<EnumWithinClass>);
                                            }
                                            ReadOnlySpan<byte> span = storage.Span;
                                            int bytesSoFar = 0;
                                            int collectionLength = span.ToDecompressedInt(ref bytesSoFar);
                                            
                                            List<EnumWithinClass> collection = new List<EnumWithinClass>(collectionLength);
                                            for (int itemIndex = 0; itemIndex < collectionLength; itemIndex++)
                                            {
                                                global::LazinatorTests.Examples.Subclasses.ClassWithLocalEnum.EnumWithinClass item = (global::LazinatorTests.Examples.Subclasses.ClassWithLocalEnum.EnumWithinClass)span.ToDecompressedInt(ref bytesSoFar);
                                                collection.Add(item);
                                            }
                                            
                                            return collection;
                                        }/*Location7341*/
                                        
                                        private static void ConvertToBytes_List_GEnumWithinClass_g(ref BinaryBufferWriter writer, List<EnumWithinClass> itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
                                        {
                                            if (itemToConvert == default(List<EnumWithinClass>))
                                            {
                                                return;
                                            }
                                            CompressedIntegralTypes.WriteCompressedInt(ref writer, itemToConvert.Count);
                                            int itemToConvertCount = itemToConvert.Count;
                                            for (int itemIndex = 0; itemIndex < itemToConvertCount; itemIndex++)
                                            {
                                                CompressedIntegralTypes.WriteCompressedInt(ref writer, (int) itemToConvert[itemIndex]);
                                            }
                                        }
                                        /*Location7342*/
                                        private static List<EnumWithinClass> CloneOrChange_List_GEnumWithinClass_g(List<EnumWithinClass> itemToClone, Func<ILazinator, ILazinator> cloneOrChangeFunc, bool avoidCloningIfPossible)
                                        {
                                            if (itemToClone == null)
                                            {
                                                return default;
                                            }
                                            
                                            int collectionLength = itemToClone.Count;
                                            List<EnumWithinClass> collection = new List<EnumWithinClass>(collectionLength);
                                            int itemToCloneCount = itemToClone.Count;
                                            for (int itemIndex = 0; itemIndex < itemToCloneCount; itemIndex++)
                                            {
                                                var itemCopied = (global::LazinatorTests.Examples.Subclasses.ClassWithLocalEnum.EnumWithinClass) itemToClone[itemIndex];
                                                collection.Add(itemCopied);
                                            }
                                            return collection;
                                        }
                                        /*Location7343*/
                                    }
                                }
