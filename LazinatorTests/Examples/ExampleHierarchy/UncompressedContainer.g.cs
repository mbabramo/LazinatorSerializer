//d0eb2da3-d45c-8834-b2e6-a3ce7d762e5f
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.400, on 2024/01/23 07:48:08.588 AM.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

#nullable disable
namespace LazinatorTests.Examples.ExampleHierarchy
{
    #pragma warning disable 8019
    using Lazinator.Attributes;
    using Lazinator.Buffers;
    using Lazinator.Core;
    using Lazinator.Exceptions;
    using Lazinator.Support;
    using static Lazinator.Buffers.WriteUncompressedPrimitives;
    using System;
    using System.Buffers;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;
    using static Lazinator.Core.LazinatorUtilities;
    #pragma warning restore 8019
    
    [Autogenerated]
    public partial record UncompressedContainer : ILazinator
    {
        public bool IsStruct => false;
        
        /* Property definitions */
        
        
        
        protected string _MyUncompressed;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public string MyUncompressed
        {
            get
            {
                return _MyUncompressed;
            }
            set
            {
                IsDirty = true;
                _MyUncompressed = value;
            }
        }
        
        protected DateTime _MyUncompressedDateTime;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public DateTime MyUncompressedDateTime
        {
            get
            {
                return _MyUncompressedDateTime;
            }
            set
            {
                IsDirty = true;
                _MyUncompressedDateTime = value;
            }
        }
        
        protected decimal _MyUncompressedDecimal;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public decimal MyUncompressedDecimal
        {
            get
            {
                return _MyUncompressedDecimal;
            }
            set
            {
                IsDirty = true;
                _MyUncompressedDecimal = value;
            }
        }
        
        protected int _MyUncompressedInt;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public int MyUncompressedInt
        {
            get
            {
                return _MyUncompressedInt;
            }
            set
            {
                IsDirty = true;
                _MyUncompressedInt = value;
            }
        }
        
        protected long _MyUncompressedLong;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public long MyUncompressedLong
        {
            get
            {
                return _MyUncompressedLong;
            }
            set
            {
                IsDirty = true;
                _MyUncompressedLong = value;
            }
        }
        
        protected DateTime? _MyUncompressedNullableDateTime;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public DateTime? MyUncompressedNullableDateTime
        {
            get
            {
                return _MyUncompressedNullableDateTime;
            }
            set
            {
                IsDirty = true;
                _MyUncompressedNullableDateTime = value;
            }
        }
        
        protected decimal? _MyUncompressedNullableDecimal;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public decimal? MyUncompressedNullableDecimal
        {
            get
            {
                return _MyUncompressedNullableDecimal;
            }
            set
            {
                IsDirty = true;
                _MyUncompressedNullableDecimal = value;
            }
        }
        
        protected int? _MyUncompressedNullableInt;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public int? MyUncompressedNullableInt
        {
            get
            {
                return _MyUncompressedNullableInt;
            }
            set
            {
                IsDirty = true;
                _MyUncompressedNullableInt = value;
            }
        }
        
        protected long? _MyUncompressedNullableLong;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public long? MyUncompressedNullableLong
        {
            get
            {
                return _MyUncompressedNullableLong;
            }
            set
            {
                IsDirty = true;
                _MyUncompressedNullableLong = value;
            }
        }
        
        protected short? _MyUncompressedNullableShort;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public short? MyUncompressedNullableShort
        {
            get
            {
                return _MyUncompressedNullableShort;
            }
            set
            {
                IsDirty = true;
                _MyUncompressedNullableShort = value;
            }
        }
        
        protected TimeSpan? _MyUncompressedNullableTimeSpan;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public TimeSpan? MyUncompressedNullableTimeSpan
        {
            get
            {
                return _MyUncompressedNullableTimeSpan;
            }
            set
            {
                IsDirty = true;
                _MyUncompressedNullableTimeSpan = value;
            }
        }
        
        protected uint? _MyUncompressedNullableUInt;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public uint? MyUncompressedNullableUInt
        {
            get
            {
                return _MyUncompressedNullableUInt;
            }
            set
            {
                IsDirty = true;
                _MyUncompressedNullableUInt = value;
            }
        }
        
        protected ulong? _MyUncompressedNullableULong;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ulong? MyUncompressedNullableULong
        {
            get
            {
                return _MyUncompressedNullableULong;
            }
            set
            {
                IsDirty = true;
                _MyUncompressedNullableULong = value;
            }
        }
        
        protected ushort? _MyUncompressedNullableUShort;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ushort? MyUncompressedNullableUShort
        {
            get
            {
                return _MyUncompressedNullableUShort;
            }
            set
            {
                IsDirty = true;
                _MyUncompressedNullableUShort = value;
            }
        }
        
        protected short _MyUncompressedShort;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public short MyUncompressedShort
        {
            get
            {
                return _MyUncompressedShort;
            }
            set
            {
                IsDirty = true;
                _MyUncompressedShort = value;
            }
        }
        
        protected TimeSpan _MyUncompressedTimeSpan;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public TimeSpan MyUncompressedTimeSpan
        {
            get
            {
                return _MyUncompressedTimeSpan;
            }
            set
            {
                IsDirty = true;
                _MyUncompressedTimeSpan = value;
            }
        }
        
        protected uint _MyUncompressedUInt;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public uint MyUncompressedUInt
        {
            get
            {
                return _MyUncompressedUInt;
            }
            set
            {
                IsDirty = true;
                _MyUncompressedUInt = value;
            }
        }
        
        protected ulong _MyUncompressedULong;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ulong MyUncompressedULong
        {
            get
            {
                return _MyUncompressedULong;
            }
            set
            {
                IsDirty = true;
                _MyUncompressedULong = value;
            }
        }
        
        protected ushort _MyUncompressedUShort;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ushort MyUncompressedUShort
        {
            get
            {
                return _MyUncompressedUShort;
            }
            set
            {
                IsDirty = true;
                _MyUncompressedUShort = value;
            }
        }
        
        /* Serialization, deserialization, and object relationships */
        
        public UncompressedContainer(IncludeChildrenMode originalIncludeChildrenMode = IncludeChildrenMode.IncludeAllChildren)
        {
            OriginalIncludeChildrenMode = originalIncludeChildrenMode;
        }
        
        public UncompressedContainer(LazinatorMemory serializedBytes, ILazinator parent = null, IncludeChildrenMode originalIncludeChildrenMode = IncludeChildrenMode.IncludeAllChildren, int? lazinatorObjectVersion = null)
        {
            if (lazinatorObjectVersion != null)
            {
                LazinatorObjectVersion = (int) lazinatorObjectVersion;
            }
            OriginalIncludeChildrenMode = originalIncludeChildrenMode;
            LazinatorParents = new LazinatorParentsCollection(parent);
            DeserializeLazinator(serializedBytes);
            HasChanged = false;
            DescendantHasChanged = false;
        }
        
        public virtual LazinatorParentsCollection LazinatorParents { get; set; }
        
        public virtual LazinatorMemory LazinatorMemoryStorage
        {
            get;
            set;
        }
        
        public virtual IncludeChildrenMode OriginalIncludeChildrenMode { get; set; }
        
        public virtual bool HasChanged { get; set; }
        
        protected bool _IsDirty;
        public virtual bool IsDirty
        {
            [DebuggerStepThrough]
            get => _IsDirty|| LazinatorMemoryStorage.Length == 0;
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
        
        public virtual bool NonBinaryHash32 => false;
        
        protected virtual void DeserializeLazinator(LazinatorMemory serializedBytes)
        {
            LazinatorMemoryStorage = serializedBytes;
            int length = Deserialize();
            if (length != LazinatorMemoryStorage.Length)
            {
                LazinatorMemoryStorage = LazinatorMemoryStorage.Slice(0, length);
            }
        }
        
        protected virtual int Deserialize()
        {
            FreeInMemoryObjects();
            int bytesSoFar = 0;
            ReadOnlySpan<byte> span = LazinatorMemoryStorage.InitialReadOnlyMemory.Span;
            if (span.Length == 0)
            {
                return 0;
            }
            
            ReadGenericIDIfApplicable(ContainsOpenGenericParameters, LazinatorUniqueID, span, ref bytesSoFar);
            
            int lazinatorLibraryVersion = span.ToDecompressedInt32(ref bytesSoFar);
            
            int serializedVersionNumber = span.ToDecompressedInt32(ref bytesSoFar);
            
            OriginalIncludeChildrenMode = (IncludeChildrenMode)span.ToByte(ref bytesSoFar);
            
            int totalBytes = ConvertFromBytesAfterHeader(OriginalIncludeChildrenMode, serializedVersionNumber, ref bytesSoFar);
            return totalBytes;
        }
        
        public virtual void SerializeLazinator()
        {
            if (!IsDirty && !DescendantIsDirty && LazinatorMemoryStorage.Length > 0 && OriginalIncludeChildrenMode == IncludeChildrenMode.IncludeAllChildren)
            {
                return;
                
            }
            var previousBuffer = LazinatorMemoryStorage;
            if (LazinatorMemoryStorage.IsEmpty || IncludeChildrenMode.IncludeAllChildren != OriginalIncludeChildrenMode || (IsDirty || DescendantIsDirty))
            {
                LazinatorMemoryStorage = EncodeToNewBuffer(LazinatorSerializationOptions.Default);
            }
            else
            {
                BufferWriter writer = new BufferWriter(LazinatorMemoryStorage.LengthInt ?? 0);
                LazinatorMemoryStorage.WriteToBuffer(ref writer);
                LazinatorMemoryStorage = writer.LazinatorMemory;
            }
            OriginalIncludeChildrenMode = IncludeChildrenMode.IncludeAllChildren;
            if (!LazinatorParents.Any())
            {
                previousBuffer.Dispose();
            }
        }
        
        public virtual LazinatorMemory SerializeLazinator(in LazinatorSerializationOptions options) 
        {
            if (LazinatorMemoryStorage.IsEmpty || options.IncludeChildrenMode != OriginalIncludeChildrenMode || (options.SerializeDiffs || options.VerifyCleanness || IsDirty || (options.IncludeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && DescendantIsDirty)))
            {
                return EncodeToNewBuffer(options);
            }
            BufferWriter writer = new BufferWriter(LazinatorMemoryStorage.LengthInt ?? 0);
            LazinatorMemoryStorage.WriteToBuffer(ref writer);
            return writer.LazinatorMemory;
        }
        
        protected virtual LazinatorMemory EncodeToNewBuffer(in LazinatorSerializationOptions options) 
        {
            int bufferSize = LazinatorMemoryStorage.Length == 0 ? ExpandableBytes.DefaultMinBufferSize : LazinatorMemoryStorage.LengthInt ?? ExpandableBytes.DefaultMinBufferSize;
            BufferWriter writer = new BufferWriter(bufferSize);
            SerializeToExistingBuffer(ref writer, options);
            return writer.LazinatorMemory;
        }
        
        public virtual ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode = IncludeChildrenMode.IncludeAllChildren, CloneBufferOptions cloneBufferOptions = CloneBufferOptions.IndependentBuffers)
        {
            UncompressedContainer clone;
            if (cloneBufferOptions == CloneBufferOptions.NoBuffer)
            {
                clone = new UncompressedContainer(includeChildrenMode);
                clone.LazinatorObjectVersion = LazinatorObjectVersion;
                clone = (UncompressedContainer)AssignCloneProperties(clone, includeChildrenMode);
            }
            else
            {
                LazinatorMemory bytes = EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, IsDirty, DescendantIsDirty, false, LazinatorMemoryStorage, this);
                clone = new UncompressedContainer(bytes);
            }
            return clone;
        }
        
        protected virtual ILazinator AssignCloneProperties(ILazinator clone, IncludeChildrenMode includeChildrenMode)
        {
            clone.FreeInMemoryObjects();
            UncompressedContainer typedClone = (UncompressedContainer) clone;
            typedClone.MyUncompressed = MyUncompressed;
            typedClone.MyUncompressedDateTime = MyUncompressedDateTime;
            typedClone.MyUncompressedDecimal = MyUncompressedDecimal;
            typedClone.MyUncompressedInt = MyUncompressedInt;
            typedClone.MyUncompressedLong = MyUncompressedLong;
            typedClone.MyUncompressedNullableDateTime = MyUncompressedNullableDateTime;
            typedClone.MyUncompressedNullableDecimal = MyUncompressedNullableDecimal;
            typedClone.MyUncompressedNullableInt = MyUncompressedNullableInt;
            typedClone.MyUncompressedNullableLong = MyUncompressedNullableLong;
            typedClone.MyUncompressedNullableShort = MyUncompressedNullableShort;
            typedClone.MyUncompressedNullableTimeSpan = MyUncompressedNullableTimeSpan;
            typedClone.MyUncompressedNullableUInt = MyUncompressedNullableUInt;
            typedClone.MyUncompressedNullableULong = MyUncompressedNullableULong;
            typedClone.MyUncompressedNullableUShort = MyUncompressedNullableUShort;
            typedClone.MyUncompressedShort = MyUncompressedShort;
            typedClone.MyUncompressedTimeSpan = MyUncompressedTimeSpan;
            typedClone.MyUncompressedUInt = MyUncompressedUInt;
            typedClone.MyUncompressedULong = MyUncompressedULong;
            typedClone.MyUncompressedUShort = MyUncompressedUShort;
            
            return typedClone;
        }
        
        
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
        
        public virtual IEnumerable<(string propertyName, ILazinator descendant)> EnumerateLazinatorDescendants(Func<ILazinator, bool> matchCriterion, bool stopExploringBelowMatch, Func<ILazinator, bool> exploreCriterion, bool exploreOnlyDeserializedChildren, bool enumerateNulls)
        {
            yield break;
        }
        
        
        public virtual IEnumerable<(string propertyName, object descendant)> EnumerateNonLazinatorProperties()
        {
            yield return ("MyUncompressed", (object)MyUncompressed);
            yield return ("MyUncompressedDateTime", (object)MyUncompressedDateTime);
            yield return ("MyUncompressedDecimal", (object)MyUncompressedDecimal);
            yield return ("MyUncompressedInt", (object)MyUncompressedInt);
            yield return ("MyUncompressedLong", (object)MyUncompressedLong);
            yield return ("MyUncompressedNullableDateTime", (object)MyUncompressedNullableDateTime);
            yield return ("MyUncompressedNullableDecimal", (object)MyUncompressedNullableDecimal);
            yield return ("MyUncompressedNullableInt", (object)MyUncompressedNullableInt);
            yield return ("MyUncompressedNullableLong", (object)MyUncompressedNullableLong);
            yield return ("MyUncompressedNullableShort", (object)MyUncompressedNullableShort);
            yield return ("MyUncompressedNullableTimeSpan", (object)MyUncompressedNullableTimeSpan);
            yield return ("MyUncompressedNullableUInt", (object)MyUncompressedNullableUInt);
            yield return ("MyUncompressedNullableULong", (object)MyUncompressedNullableULong);
            yield return ("MyUncompressedNullableUShort", (object)MyUncompressedNullableUShort);
            yield return ("MyUncompressedShort", (object)MyUncompressedShort);
            yield return ("MyUncompressedTimeSpan", (object)MyUncompressedTimeSpan);
            yield return ("MyUncompressedUInt", (object)MyUncompressedUInt);
            yield return ("MyUncompressedULong", (object)MyUncompressedULong);
            yield return ("MyUncompressedUShort", (object)MyUncompressedUShort);
            yield break;
        }
        
        public virtual ILazinator ForEachLazinator(Func<ILazinator, ILazinator> changeFunc, bool exploreOnlyDeserializedChildren, bool changeThisLevel)
        {
            if (changeThisLevel && changeFunc != null)
            {
                return changeFunc(this);
            }
            return this;
        }
        
        public virtual void FreeInMemoryObjects()
        {
            
            IsDirty = false;
            DescendantIsDirty = false;
            HasChanged = false;
            DescendantHasChanged = false;
        }
        
        /* Conversion */
        
        public virtual int LazinatorUniqueID => 1092;
        
        protected virtual bool ContainsOpenGenericParameters => false;
        public virtual LazinatorGenericIDType LazinatorGenericID => default;
        
        
        public virtual int LazinatorObjectVersion { get; set; } = 0;
        
        
        protected virtual int ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            ReadOnlySpan<byte> span = LazinatorMemoryStorage.InitialReadOnlyMemory.Span;
            ConvertFromBytesForPrimitiveProperties(span, includeChildrenMode, serializedVersionNumber, ref bytesSoFar);
            int lengthForLengths = 0;
            int totalChildrenSize = ConvertFromBytesForChildLengths(span, includeChildrenMode, serializedVersionNumber, bytesSoFar + lengthForLengths, ref bytesSoFar);;
            return bytesSoFar + totalChildrenSize;
        }
        
        protected virtual void ConvertFromBytesForPrimitiveProperties(ReadOnlySpan<byte> span, IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            _MyUncompressed = span.ToString_VarIntLengthUtf8(ref bytesSoFar);
            _MyUncompressedDateTime = span.ToDateTime(ref bytesSoFar);
            _MyUncompressedDecimal = span.ToDecimal(ref bytesSoFar);
            _MyUncompressedInt = span.ToInt32(ref bytesSoFar);
            _MyUncompressedLong = span.ToInt64(ref bytesSoFar);
            _MyUncompressedNullableDateTime = span.ToNullableDateTime(ref bytesSoFar);
            _MyUncompressedNullableDecimal = span.ToNullableDecimal(ref bytesSoFar);
            _MyUncompressedNullableInt = span.ToNullableInt32(ref bytesSoFar);
            _MyUncompressedNullableLong = span.ToNullableInt64(ref bytesSoFar);
            _MyUncompressedNullableShort = span.ToNullableInt16(ref bytesSoFar);
            _MyUncompressedNullableTimeSpan = span.ToNullableTimeSpan(ref bytesSoFar);
            _MyUncompressedNullableUInt = span.ToNullableUInt32(ref bytesSoFar);
            _MyUncompressedNullableULong = span.ToNullableUInt64(ref bytesSoFar);
            _MyUncompressedNullableUShort = span.ToNullableUInt16(ref bytesSoFar);
            _MyUncompressedShort = span.ToInt16(ref bytesSoFar);
            _MyUncompressedTimeSpan = span.ToTimeSpan(ref bytesSoFar);
            _MyUncompressedUInt = span.ToUInt32(ref bytesSoFar);
            _MyUncompressedULong = span.ToUInt64(ref bytesSoFar);
            _MyUncompressedUShort = span.ToUInt16(ref bytesSoFar);
        }
        
        protected virtual int ConvertFromBytesForChildLengths(ReadOnlySpan<byte> span, IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, int indexOfFirstChild, ref int bytesSoFar)
        {
            int totalChildrenBytes = 0;
            return totalChildrenBytes;
        }
        
        public virtual void SerializeToExistingBuffer(ref BufferWriter writer, in LazinatorSerializationOptions options)
        {
            int startPosition = writer.ActiveMemoryPosition;
            WritePropertiesIntoBuffer(ref writer, options, true);
            if (options.UpdateStoredBuffer)
            {
                UpdateStoredBuffer(ref writer, startPosition, writer.ActiveMemoryPosition - startPosition, options.IncludeChildrenMode, false);
            }
        }
        
        public virtual void UpdateStoredBuffer(ref BufferWriter writer, long startPosition, long length, IncludeChildrenMode includeChildrenMode, bool updateDeserializedChildren)
        {
            _IsDirty = false;
            if (includeChildrenMode == IncludeChildrenMode.IncludeAllChildren)
            {
                _DescendantIsDirty = false;
                if (updateDeserializedChildren)
                {
                    UpdateDeserializedChildren(ref writer, startPosition);
                }
                
            }
            else
            {
                ThrowHelper.ThrowCannotUpdateStoredBuffer();
            }
            
            var newBuffer = writer.Slice(startPosition, length);
            LazinatorMemoryStorage = newBuffer;
        }
        
        protected virtual void UpdateDeserializedChildren(ref BufferWriter writer, long startPosition)
        {
            
        }
        
        
        protected virtual void WritePropertiesIntoBuffer(ref BufferWriter writer, in LazinatorSerializationOptions options, bool includeUniqueID)
        {
            if (includeUniqueID)
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
            CompressedIntegralTypes.WriteCompressedInt(ref writer, Lazinator.Support.LazinatorVersionInfo.LazinatorIntVersion);
            CompressedIntegralTypes.WriteCompressedInt(ref writer, LazinatorObjectVersion);
            writer.Write((byte)options.IncludeChildrenMode);
            // write properties
            
            WritePrimitivePropertiesIntoBuffer(ref writer, options, includeUniqueID);
            
        }
        
        protected virtual void WritePrimitivePropertiesIntoBuffer(ref BufferWriter writer, in LazinatorSerializationOptions options, bool includeUniqueID)
        {
            EncodeCharAndString.WriteStringUtf8WithVarIntPrefix(ref writer, _MyUncompressed);
            WriteUncompressedPrimitives.WriteDateTime(ref writer, _MyUncompressedDateTime);
            WriteUncompressedPrimitives.WriteDecimal(ref writer, _MyUncompressedDecimal);
            WriteUncompressedPrimitives.WriteInt(ref writer, _MyUncompressedInt);
            WriteUncompressedPrimitives.WriteLong(ref writer, _MyUncompressedLong);
            WriteUncompressedPrimitives.WriteNullableDateTime(ref writer, _MyUncompressedNullableDateTime);
            WriteUncompressedPrimitives.WriteNullableDecimal(ref writer, _MyUncompressedNullableDecimal);
            WriteUncompressedPrimitives.WriteNullableInt(ref writer, _MyUncompressedNullableInt);
            WriteUncompressedPrimitives.WriteNullableLong(ref writer, _MyUncompressedNullableLong);
            WriteUncompressedPrimitives.WriteNullableShort(ref writer, _MyUncompressedNullableShort);
            WriteUncompressedPrimitives.WriteNullableTimeSpan(ref writer, _MyUncompressedNullableTimeSpan);
            WriteUncompressedPrimitives.WriteNullableUInt(ref writer, _MyUncompressedNullableUInt);
            WriteUncompressedPrimitives.WriteNullableULong(ref writer, _MyUncompressedNullableULong);
            WriteUncompressedPrimitives.WriteNullableUShort(ref writer, _MyUncompressedNullableUShort);
            WriteUncompressedPrimitives.WriteShort(ref writer, _MyUncompressedShort);
            WriteUncompressedPrimitives.WriteTimeSpan(ref writer, _MyUncompressedTimeSpan);
            WriteUncompressedPrimitives.WriteUInt(ref writer, _MyUncompressedUInt);
            WriteUncompressedPrimitives.WriteULong(ref writer, _MyUncompressedULong);
            WriteUncompressedPrimitives.WriteUShort(ref writer, _MyUncompressedUShort);
        }
        protected virtual void WriteChildrenPropertiesIntoBuffer(ref BufferWriter writer, LazinatorSerializationOptions options, bool includeUniqueID, int startOfObjectPosition)
        {
            
        }
    }
}
#nullable restore
