/*Location7397*//*Location7383*///0553901c-117b-95f8-3deb-96c184495bdc
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.380
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

#nullable disable
namespace LazinatorCollections.ByteSpan
{/*Location7384*/
    using Lazinator.Attributes;/*Location7385*/
    using Lazinator.Buffers;/*Location7386*/
    using Lazinator.Core;/*Location7387*/
    using Lazinator.Exceptions;/*Location7388*/
    using Lazinator.Support;/*Location7389*/
    using System;/*Location7390*/
    using System.Buffers;/*Location7391*/
    using System.Collections.Generic;/*Location7392*/
    using System.Diagnostics;/*Location7393*/
    using System.IO;/*Location7394*/
    using System.Linq;/*Location7395*/
    using System.Runtime.InteropServices;/*Location7396*/
    using static Lazinator.Core.LazinatorUtilities;
    
    [Autogenerated]
    public partial class LazinatorByteSpan : ILazinator
    {
        /*Location7398*/[DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsStruct => false;
        
        /*Location7399*//* Property definitions */
        
        /*Location7400*/        protected int _ReadOnly_ByteIndex;
        /*Location7401*/        protected int _ReadOrWrite_ByteIndex;
        /*Location7402*/protected virtual int _ReadOnly_ByteLength => _ReadOrWrite_ByteIndex - _ReadOnly_ByteIndex;
        /*Location7403*/private int _LazinatorByteSpan_EndByteIndex;
        /*Location7404*/protected virtual int _ReadOrWrite_ByteLength => _LazinatorByteSpan_EndByteIndex - _ReadOrWrite_ByteIndex;
        
        /*Location7405*/[DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private ReadOnlyMemory<byte> _ReadOnly;
        internal ReadOnlySpan<byte> ReadOnly
        {
            [DebuggerStepThrough]
            get
            {
                if (!_ReadOnly_Accessed)
                {
                    LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _ReadOnly_ByteIndex, _ReadOnly_ByteLength, false, false, null);
                    return childData.Span;
                }
                return _ReadOnly.Span;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _ReadOnly = new ReadOnlyMemory<byte>((value).ToArray());
                _ReadOnly_Accessed = true;
            }
        }
        protected bool _ReadOnly_Accessed;
        /*Location7406*/
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected Memory<byte> _ReadOrWrite;
        internal Memory<byte> ReadOrWrite
        {
            [DebuggerStepThrough]
            get
            {
                if (!_ReadOrWrite_Accessed)
                {
                    Lazinate_ReadOrWrite();
                }
                IsDirty = true; 
                return _ReadOrWrite;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                DescendantIsDirty = true;
                _ReadOrWrite = value;
                _ReadOrWrite_Accessed = true;
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected bool _ReadOrWrite_Accessed;
        private void Lazinate_ReadOrWrite()
        {
            if (LazinatorObjectBytes.Length == 0)
            {
                _ReadOrWrite = default(Memory<byte>);
            }
            else
            {
                LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _ReadOrWrite_ByteIndex, _ReadOrWrite_ByteLength, false, false, null);
                _ReadOrWrite = ConvertFromBytes_Memory_Gbyte_g(childData);
            }
            
            _ReadOrWrite_Accessed = true;
        }
        
        /*Location7409*/
        /* Serialization, deserialization, and object relationships */
        
        public LazinatorByteSpan(LazinatorConstructorEnum constructorEnum)
        {
        }
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public virtual LazinatorParentsCollection LazinatorParents { get; set; }
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
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
            PostDeserialization();
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
            var clone = new LazinatorByteSpan(LazinatorConstructorEnum.LazinatorConstructor)
            {
                OriginalIncludeChildrenMode = includeChildrenMode
            };
            clone = CompleteClone(this, clone, includeChildrenMode, cloneBufferOptions);
            return clone;
        }
        
        public virtual ILazinator AssignCloneProperties(ILazinator clone, IncludeChildrenMode includeChildrenMode)
        {
            clone.FreeInMemoryObjects();
            LazinatorByteSpan typedClone = (LazinatorByteSpan) clone;
            /*Location7407*/typedClone.ReadOnly = CloneOrChange_ReadOnlySpan_Gbyte_g(ReadOnly, l => l.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer), false);
            /*Location7408*/typedClone.ReadOrWrite = CloneOrChange_Memory_Gbyte_g(ReadOrWrite, l => l.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer), false);
            
            return typedClone;
        }
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public virtual bool HasChanged { get; set; }
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected bool _IsDirty;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
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
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected bool _DescendantHasChanged;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
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
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected bool _DescendantIsDirty;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
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
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public virtual LazinatorMemory LazinatorMemoryStorage
        {
            get;
            set;
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
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
        
        /*Location7410*/
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
        
        /*Location7411*/public virtual IEnumerable<(string propertyName, ILazinator descendant)> EnumerateLazinatorDescendants(Func<ILazinator, bool> matchCriterion, bool stopExploringBelowMatch, Func<ILazinator, bool> exploreCriterion, bool exploreOnlyDeserializedChildren, bool enumerateNulls)
        {
            /*Location7412*/yield break;
        }
        /*Location7413*/
        
        public virtual IEnumerable<(string propertyName, object descendant)> EnumerateNonLazinatorProperties()
        {
            /*Location7414*/yield return ("ReadOnly", (object)ReadOnly.ToString());
            /*Location7415*/yield return ("ReadOrWrite", (object)ReadOrWrite);
            /*Location7416*/yield break;
        }
        /*Location7417*/
        public virtual ILazinator ForEachLazinator(Func<ILazinator, ILazinator> changeFunc, bool exploreOnlyDeserializedChildren, bool changeThisLevel)
        {
            /*Location7418*/if (!exploreOnlyDeserializedChildren)
            {
                var deserialized_ReadOnly = ReadOnly;
                if (!_ReadOnly_Accessed)
                {
                    ReadOnly = deserialized_ReadOnly;
                }
            }
            /*Location7419*/if (!exploreOnlyDeserializedChildren)
            {
                var deserialized_ReadOrWrite = ReadOrWrite;
            }
            /*Location7420*/if (changeThisLevel && changeFunc != null)
            {
                return changeFunc(this);
            }
            return this;
        }
        /*Location7421*/
        public virtual void FreeInMemoryObjects()
        {
            _ReadOnly = default;
            _ReadOrWrite = default;
            _ReadOnly_Accessed = _ReadOrWrite_Accessed = false;
            IsDirty = false;
            DescendantIsDirty = false;
            HasChanged = false;
            DescendantHasChanged = false;
        }
        /*Location7422*/
        /* Conversion */
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public virtual int LazinatorUniqueID => 251;
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected virtual bool ContainsOpenGenericParameters => false;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public virtual LazinatorGenericIDType LazinatorGenericID => default;
        
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public virtual int LazinatorObjectVersion { get; set; } = 0;
        
        
        /*Location7423*/public virtual void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
            /*Location7424*/_ReadOnly_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            /*Location7425*/_ReadOrWrite_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            /*Location7426*/_LazinatorByteSpan_EndByteIndex = bytesSoFar;
            /*Location7427*/        }
            
            /*Location7428*/public virtual void SerializeExistingBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
            {
                /*Location7429*/if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren)
                {
                    updateStoredBuffer = false;
                }
                /*Location7430*/PreSerialization(verifyCleanness, updateStoredBuffer);
                int startPosition = writer.Position;
                WritePropertiesIntoBuffer(ref writer, includeChildrenMode, verifyCleanness, updateStoredBuffer, true);
                /*Location7431*/if (updateStoredBuffer)
                {
                    UpdateStoredBuffer(ref writer, startPosition, writer.Position - startPosition, includeChildrenMode, false);
                    /*Location7432*/}
                    /*Location7433*/}
                    /*Location7434*/
                    public virtual void UpdateStoredBuffer(ref BinaryBufferWriter writer, int startPosition, int length, IncludeChildrenMode includeChildrenMode, bool updateDeserializedChildren)
                    {
                        /*Location7435*/_IsDirty = false;
                        if (includeChildrenMode == IncludeChildrenMode.IncludeAllChildren)
                        {
                            _DescendantIsDirty = false;/*Location7436*/
                            if (updateDeserializedChildren)
                            {
                                UpdateDeserializedChildren(ref writer, startPosition);
                                /*Location7437*/}
                                /*Location7438*//*Location7439*/
                            }
                            else
                            {
                                ThrowHelper.ThrowCannotUpdateStoredBuffer();
                            }
                            /*Location7440*/
                            var newBuffer = writer.Slice(startPosition, length);
                            LazinatorMemoryStorage = newBuffer;
                            /*Location7441*/}
                            /*Location7442*/
                            protected virtual void UpdateDeserializedChildren(ref BinaryBufferWriter writer, int startPosition)
                            {
                                /*Location7443*/}
                                
                                /*Location7444*/
                                protected virtual void WritePropertiesIntoBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer, bool includeUniqueID)
                                {
                                    int startPosition = writer.Position;
                                    int startOfObjectPosition = 0;
                                    // header information
                                    /*Location7445*/if (includeUniqueID)
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
                                    /*Location7446*/CompressedIntegralTypes.WriteCompressedInt(ref writer, Lazinator.Support.LazinatorVersionInfo.LazinatorIntVersion);
                                    CompressedIntegralTypes.WriteCompressedInt(ref writer, LazinatorObjectVersion);
                                    writer.Write((byte)includeChildrenMode);
                                    /*Location7447*/// write properties
                                    /*Location7448*/startOfObjectPosition = writer.Position;
                                    /*Location7449*/if ((includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode) && !_ReadOnly_Accessed)
                                    {
                                        var deserialized = ReadOnly;
                                    }
                                    /*Location7450*/WriteNonLazinatorObject(
                                    nonLazinatorObject: _ReadOnly, isBelievedDirty: _ReadOnly_Accessed || (includeChildrenMode != OriginalIncludeChildrenMode),
                                    isAccessed: _ReadOnly_Accessed, writer: ref writer,
                                    getChildSliceForFieldFn: () => GetChildSlice(LazinatorMemoryStorage, _ReadOnly_ByteIndex, _ReadOnly_ByteLength, false, false, null),
                                    verifyCleanness: false,
                                    binaryWriterAction: (ref BinaryBufferWriter w, bool v) =>
                                    ConvertToBytes_ReadOnlySpan_Gbyte_g(ref w, _ReadOnly.Span,
                                    includeChildrenMode, v, updateStoredBuffer));
                                    /*Location7451*/if (updateStoredBuffer)
                                    {
                                        _ReadOnly_ByteIndex = startOfObjectPosition - startPosition;
                                    }
                                    /*Location7452*/startOfObjectPosition = writer.Position;
                                    /*Location7453*/if ((includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode) && !_ReadOrWrite_Accessed)
                                    {
                                        var deserialized = ReadOrWrite;
                                    }
                                    /*Location7454*/WriteNonLazinatorObject(
                                    nonLazinatorObject: _ReadOrWrite, isBelievedDirty: _ReadOrWrite_Accessed || (includeChildrenMode != OriginalIncludeChildrenMode),
                                    isAccessed: _ReadOrWrite_Accessed, writer: ref writer,
                                    getChildSliceForFieldFn: () => GetChildSlice(LazinatorMemoryStorage, _ReadOrWrite_ByteIndex, _ReadOrWrite_ByteLength, false, false, null),
                                    verifyCleanness: false,
                                    binaryWriterAction: (ref BinaryBufferWriter w, bool v) =>
                                    ConvertToBytes_Memory_Gbyte_g(ref w, _ReadOrWrite,
                                    includeChildrenMode, v, updateStoredBuffer));
                                    /*Location7455*/if (updateStoredBuffer)
                                    {
                                        _ReadOrWrite_ByteIndex = startOfObjectPosition - startPosition;
                                    }
                                    /*Location7456*/if (updateStoredBuffer)
                                    {
                                        /*Location7457*/_LazinatorByteSpan_EndByteIndex = writer.Position - startPosition;
                                        /*Location7458*/}
                                        /*Location7459*/}
                                        /*Location7460*/
                                        /* Conversion of supported collections and tuples */
                                        /*Location7461*/
                                        private static void ConvertToBytes_ReadOnlySpan_Gbyte_g(ref BinaryBufferWriter writer, ReadOnlySpan<byte> itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
                                        {
                                            ReadOnlySpan<byte> toConvert = (itemToConvert);
                                            for (int i = 0; i < toConvert.Length; i++)
                                            {
                                                writer.Write(toConvert[i]);
                                            }
                                        }
                                        /*Location7462*/private static ReadOnlySpan<byte> CloneOrChange_ReadOnlySpan_Gbyte_g(ReadOnlySpan<byte> itemToClone, Func<ILazinator, ILazinator> cloneOrChangeFunc, bool avoidCloningIfPossible)
                                        {
                                            var clone = new Span<byte>(new byte[itemToClone.Length * sizeof(byte)]);
                                            itemToClone.CopyTo(clone);
                                            return clone;
                                        }
                                        /*Location7463*/
                                        private static Memory<byte> ConvertFromBytes_Memory_Gbyte_g(LazinatorMemory storage)
                                        {
                                            /*Location7464*/return storage.Memory.ToArray();
                                        }/*Location7465*/
                                        
                                        private static void ConvertToBytes_Memory_Gbyte_g(ref BinaryBufferWriter writer, Memory<byte> itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
                                        {
                                            /*Location7466*/writer.Write(itemToConvert.Span);
                                        }
                                        /*Location7467*/
                                        private static Memory<byte> CloneOrChange_Memory_Gbyte_g(Memory<byte> itemToClone, Func<ILazinator, ILazinator> cloneOrChangeFunc, bool avoidCloningIfPossible)
                                        {
                                            int collectionLength = itemToClone.Length;
                                            Memory<byte> collection = new Memory<byte>(new byte[collectionLength]);
                                            var collectionAsSpan = collection.Span;
                                            var itemToCloneSpan = itemToClone.Span;
                                            int itemToCloneCount = itemToCloneSpan.Length;
                                            for (int itemIndex = 0; itemIndex < itemToCloneCount; itemIndex++)
                                            {
                                                var itemCopied = (byte) itemToCloneSpan[itemIndex];
                                                collectionAsSpan[itemIndex] = itemCopied;
                                            }
                                            return collection;
                                        }
                                        /*Location7468*/
                                    }
                                }
