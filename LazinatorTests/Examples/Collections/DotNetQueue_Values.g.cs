/*Location6404*//*Location6390*///003a6545-5266-bab5-6136-c2c5edcb5276
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
{/*Location6391*/
    using Lazinator.Attributes;/*Location6392*/
    using Lazinator.Buffers;/*Location6393*/
    using Lazinator.Core;/*Location6394*/
    using Lazinator.Exceptions;/*Location6395*/
    using Lazinator.Support;/*Location6396*/
    using System;/*Location6397*/
    using System.Buffers;/*Location6398*/
    using System.Collections.Generic;/*Location6399*/
    using System.Diagnostics;/*Location6400*/
    using System.IO;/*Location6401*/
    using System.Linq;/*Location6402*/
    using System.Runtime.InteropServices;/*Location6403*/
    using static Lazinator.Core.LazinatorUtilities;
    
    [Autogenerated]
    public partial class DotNetQueue_Values : ILazinator
    {
        /*Location6405*/public bool IsStruct => false;
        
        /*Location6406*//* Property definitions */
        
        /*Location6407*/        protected int _MyQueueInt_ByteIndex;
        /*Location6408*/private int _DotNetQueue_Values_EndByteIndex;
        /*Location6409*/protected virtual int _MyQueueInt_ByteLength => _DotNetQueue_Values_EndByteIndex - _MyQueueInt_ByteIndex;
        
        /*Location6410*/
        protected Queue<int> _MyQueueInt;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public Queue<int> MyQueueInt
        {
            get
            {
                if (!_MyQueueInt_Accessed)
                {
                    Lazinate_MyQueueInt();
                } 
                return _MyQueueInt;
            }
            set
            {
                IsDirty = true;
                DescendantIsDirty = true;
                _MyQueueInt = value;
                _MyQueueInt_Dirty = true;
                _MyQueueInt_Accessed = true;
            }
        }
        protected bool _MyQueueInt_Accessed;
        private void Lazinate_MyQueueInt()
        {
            if (LazinatorObjectBytes.Length == 0)
            {
                _MyQueueInt = default(Queue<int>);
                _MyQueueInt_Dirty = true; 
            }
            else
            {
                LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _MyQueueInt_ByteIndex, _MyQueueInt_ByteLength, false, false, null);
                _MyQueueInt = ConvertFromBytes_Queue_Gint_g(childData);
            }
            
            _MyQueueInt_Accessed = true;
        }
        
        /*Location6411*/
        private bool _MyQueueInt_Dirty;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool MyQueueInt_Dirty
        {
            get => _MyQueueInt_Dirty;
            set
            {
                if (_MyQueueInt_Dirty != value)
                {
                    _MyQueueInt_Dirty = value;
                }
                if (value && !IsDirty)
                {
                    IsDirty = true;
                }
            }
        }
        /*Location6413*/
        /* Serialization, deserialization, and object relationships */
        
        public DotNetQueue_Values(IncludeChildrenMode originalIncludeChildrenMode)
        {
            OriginalIncludeChildrenMode = originalIncludeChildrenMode;
        }
        
        public DotNetQueue_Values(LazinatorMemory serializedBytes, ILazinator parent = null)
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
            DotNetQueue_Values clone;
            if (cloneBufferOptions == CloneBufferOptions.NoBuffer)
            {
                clone = new DotNetQueue_Values(includeChildrenMode);
                clone.LazinatorObjectVersion = LazinatorObjectVersion;
                clone = (DotNetQueue_Values)AssignCloneProperties(clone, includeChildrenMode);
            }
            else
            {
                LazinatorMemory bytes = EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, false, IsDirty, DescendantIsDirty, false, LazinatorMemoryStorage, false, this);
                clone = new DotNetQueue_Values(bytes);
            }
            return clone;
        }
        
        public virtual ILazinator AssignCloneProperties(ILazinator clone, IncludeChildrenMode includeChildrenMode)
        {
            clone.FreeInMemoryObjects();
            DotNetQueue_Values typedClone = (DotNetQueue_Values) clone;
            /*Location6412*/typedClone.MyQueueInt = CloneOrChange_Queue_Gint_g(MyQueueInt, l => l?.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer), false);
            
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
        
        /*Location6414*/
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
        
        /*Location6415*/public virtual IEnumerable<(string propertyName, ILazinator descendant)> EnumerateLazinatorDescendants(Func<ILazinator, bool> matchCriterion, bool stopExploringBelowMatch, Func<ILazinator, bool> exploreCriterion, bool exploreOnlyDeserializedChildren, bool enumerateNulls)
        {
            /*Location6416*/yield break;
        }
        /*Location6417*/
        
        public virtual IEnumerable<(string propertyName, object descendant)> EnumerateNonLazinatorProperties()
        {
            /*Location6418*/yield return ("MyQueueInt", (object)MyQueueInt);
            /*Location6419*/yield break;
        }
        /*Location6420*/
        public virtual ILazinator ForEachLazinator(Func<ILazinator, ILazinator> changeFunc, bool exploreOnlyDeserializedChildren, bool changeThisLevel)
        {
            /*Location6421*/if ((!exploreOnlyDeserializedChildren && MyQueueInt != null) || (_MyQueueInt_Accessed && _MyQueueInt != null))
            {
                _MyQueueInt = (Queue<int>) CloneOrChange_Queue_Gint_g(_MyQueueInt, l => l?.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, true), true);
            }
            /*Location6422*/if (changeThisLevel && changeFunc != null)
            {
                return changeFunc(this);
            }
            return this;
        }
        /*Location6423*/
        public virtual void FreeInMemoryObjects()
        {
            _MyQueueInt = default;
            _MyQueueInt_Accessed = false;
            IsDirty = false;
            DescendantIsDirty = false;
            HasChanged = false;
            DescendantHasChanged = false;
        }
        /*Location6424*/
        /* Conversion */
        
        public virtual int LazinatorUniqueID => 1010;
        
        protected virtual bool ContainsOpenGenericParameters => false;
        public virtual LazinatorGenericIDType LazinatorGenericID => default;
        
        
        public virtual int LazinatorObjectVersion { get; set; } = 0;
        
        
        /*Location6425*/public virtual void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
            /*Location6426*/_MyQueueInt_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            /*Location6427*/_DotNetQueue_Values_EndByteIndex = bytesSoFar;
            /*Location6428*/        }
            
            /*Location6429*/public virtual void SerializeExistingBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
            {
                /*Location6430*/if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren)
                {
                    updateStoredBuffer = false;
                }
                /*Location6431*/int startPosition = writer.Position;
                WritePropertiesIntoBuffer(ref writer, includeChildrenMode, verifyCleanness, updateStoredBuffer, true);
                /*Location6432*/if (updateStoredBuffer)
                {
                    UpdateStoredBuffer(ref writer, startPosition, writer.Position - startPosition, includeChildrenMode, false);
                    /*Location6433*/}
                    /*Location6434*/}
                    /*Location6435*/
                    public virtual void UpdateStoredBuffer(ref BinaryBufferWriter writer, int startPosition, int length, IncludeChildrenMode includeChildrenMode, bool updateDeserializedChildren)
                    {
                        /*Location6436*/_IsDirty = false;
                        if (includeChildrenMode == IncludeChildrenMode.IncludeAllChildren)
                        {
                            _DescendantIsDirty = false;/*Location6437*/
                            if (updateDeserializedChildren)
                            {
                                UpdateDeserializedChildren(ref writer, startPosition);
                                /*Location6438*/}
                                /*Location6439*//*Location6440*/
                            }
                            else
                            {
                                ThrowHelper.ThrowCannotUpdateStoredBuffer();
                            }
                            /*Location6441*/
                            var newBuffer = writer.Slice(startPosition, length);
                            LazinatorMemoryStorage = newBuffer;
                            /*Location6442*/}
                            /*Location6443*/
                            protected virtual void UpdateDeserializedChildren(ref BinaryBufferWriter writer, int startPosition)
                            {
                                /*Location6444*/if (_MyQueueInt_Accessed && _MyQueueInt != null)
                                {
                                    _MyQueueInt = (Queue<int>) CloneOrChange_Queue_Gint_g(_MyQueueInt, l => l.RemoveBufferInHierarchy(), true);
                                }
                                /*Location6445*/}
                                
                                /*Location6446*/
                                protected virtual void WritePropertiesIntoBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer, bool includeUniqueID)
                                {
                                    int startPosition = writer.Position;
                                    int startOfObjectPosition = 0;
                                    // header information
                                    /*Location6447*/if (includeUniqueID)
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
                                    /*Location6448*/CompressedIntegralTypes.WriteCompressedInt(ref writer, Lazinator.Support.LazinatorVersionInfo.LazinatorIntVersion);
                                    CompressedIntegralTypes.WriteCompressedInt(ref writer, LazinatorObjectVersion);
                                    writer.Write((byte)includeChildrenMode);
                                    /*Location6449*/// write properties
                                    /*Location6450*/startOfObjectPosition = writer.Position;
                                    /*Location6451*/if ((includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode) && !_MyQueueInt_Accessed)
                                    {
                                        var deserialized = MyQueueInt;
                                    }
                                    /*Location6452*/WriteNonLazinatorObject(
                                    nonLazinatorObject: _MyQueueInt, isBelievedDirty: MyQueueInt_Dirty || (includeChildrenMode != OriginalIncludeChildrenMode),
                                    isAccessed: _MyQueueInt_Accessed, writer: ref writer,
                                    getChildSliceForFieldFn: () => GetChildSlice(LazinatorMemoryStorage, _MyQueueInt_ByteIndex, _MyQueueInt_ByteLength, false, false, null),
                                    verifyCleanness: verifyCleanness,
                                    binaryWriterAction: (ref BinaryBufferWriter w, bool v) =>
                                    ConvertToBytes_Queue_Gint_g(ref w, _MyQueueInt,
                                    includeChildrenMode, v, updateStoredBuffer));
                                    /*Location6453*/if (updateStoredBuffer)
                                    {
                                        _MyQueueInt_ByteIndex = startOfObjectPosition - startPosition;
                                    }
                                    /*Location6454*/if (updateStoredBuffer)
                                    {
                                        /*Location6455*/_DotNetQueue_Values_EndByteIndex = writer.Position - startPosition;
                                        /*Location6456*/}
                                        /*Location6457*/}
                                        /*Location6458*/
                                        /* Conversion of supported collections and tuples */
                                        /*Location6459*/
                                        private static Queue<int> ConvertFromBytes_Queue_Gint_g(LazinatorMemory storage)
                                        {
                                            if (storage.Length == 0)
                                            {
                                                return default(Queue<int>);
                                            }
                                            ReadOnlySpan<byte> span = storage.Span;
                                            int bytesSoFar = 0;
                                            int collectionLength = span.ToDecompressedInt(ref bytesSoFar);
                                            
                                            Queue<int> collection = new Queue<int>(collectionLength);
                                            for (int itemIndex = 0; itemIndex < collectionLength; itemIndex++)
                                            {
                                                int item = span.ToDecompressedInt(ref bytesSoFar);
                                                collection.Enqueue(item);
                                            }
                                            
                                            return collection;
                                        }/*Location6460*/
                                        
                                        private static void ConvertToBytes_Queue_Gint_g(ref BinaryBufferWriter writer, Queue<int> itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
                                        {
                                            if (itemToConvert == default(Queue<int>))
                                            {
                                                return;
                                            }
                                            CompressedIntegralTypes.WriteCompressedInt(ref writer, itemToConvert.Count);
                                            int itemToConvertCount = itemToConvert.Count;
                                            var q = System.Linq.Enumerable.ToList(itemToConvert);
                                            for (int itemIndex = 0; itemIndex < itemToConvertCount; itemIndex++)
                                            {
                                                CompressedIntegralTypes.WriteCompressedInt(ref writer, q[itemIndex]);
                                            }
                                        }
                                        /*Location6461*/
                                        private static Queue<int> CloneOrChange_Queue_Gint_g(Queue<int> itemToClone, Func<ILazinator, ILazinator> cloneOrChangeFunc, bool avoidCloningIfPossible)
                                        {
                                            if (itemToClone == null)
                                            {
                                                return default;
                                            }
                                            
                                            int collectionLength = itemToClone.Count;
                                            Queue<int> collection = new Queue<int>(collectionLength);
                                            int itemToCloneCount = itemToClone.Count;
                                            var q = System.Linq.Enumerable.ToList(itemToClone);
                                            for (int itemIndex = 0; itemIndex < itemToCloneCount; itemIndex++)
                                            {
                                                var itemCopied = (int) q[itemIndex];
                                                collection.Enqueue(itemCopied);
                                            }
                                            return collection;
                                        }
                                        /*Location6462*/
                                    }
                                }
