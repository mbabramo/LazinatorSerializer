//2def3d38-9762-86f2-76a7-bae0ead3dad4
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.395
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

#nullable disable
namespace LazinatorTests.Examples.ExampleHierarchy
{
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
            int totalChildrenSize = ConvertFromBytesForChildProperties(span, includeChildrenMode, serializedVersionNumber, bytesSoFar + lengthForLengths, ref bytesSoFar);
            return bytesSoFar + totalChildrenSize;
        }
        
        protected virtual void ConvertFromBytesForPrimitiveProperties(ReadOnlySpan<byte> span, IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            TabbedText.WriteLine($"Reading MyUncompressed at byte location {bytesSoFar}"); 
            _MyUncompressed = span.ToString_VarIntLengthUtf8(ref bytesSoFar);
            TabbedText.WriteLine($"Reading MyUncompressedDateTime at byte location {bytesSoFar}"); 
            _MyUncompressedDateTime = span.ToDateTime(ref bytesSoFar);
            TabbedText.WriteLine($"Reading MyUncompressedDecimal at byte location {bytesSoFar}"); 
            _MyUncompressedDecimal = span.ToDecimal(ref bytesSoFar);
            TabbedText.WriteLine($"Reading MyUncompressedInt at byte location {bytesSoFar}"); 
            _MyUncompressedInt = span.ToInt32(ref bytesSoFar);
            TabbedText.WriteLine($"Reading MyUncompressedLong at byte location {bytesSoFar}"); 
            _MyUncompressedLong = span.ToInt64(ref bytesSoFar);
            TabbedText.WriteLine($"Reading MyUncompressedNullableDateTime at byte location {bytesSoFar}"); 
            _MyUncompressedNullableDateTime = span.ToNullableDateTime(ref bytesSoFar);
            TabbedText.WriteLine($"Reading MyUncompressedNullableDecimal at byte location {bytesSoFar}"); 
            _MyUncompressedNullableDecimal = span.ToNullableDecimal(ref bytesSoFar);
            TabbedText.WriteLine($"Reading MyUncompressedNullableInt at byte location {bytesSoFar}"); 
            _MyUncompressedNullableInt = span.ToNullableInt32(ref bytesSoFar);
            TabbedText.WriteLine($"Reading MyUncompressedNullableLong at byte location {bytesSoFar}"); 
            _MyUncompressedNullableLong = span.ToNullableInt64(ref bytesSoFar);
            TabbedText.WriteLine($"Reading MyUncompressedNullableShort at byte location {bytesSoFar}"); 
            _MyUncompressedNullableShort = span.ToNullableInt16(ref bytesSoFar);
            TabbedText.WriteLine($"Reading MyUncompressedNullableTimeSpan at byte location {bytesSoFar}"); 
            _MyUncompressedNullableTimeSpan = span.ToNullableTimeSpan(ref bytesSoFar);
            TabbedText.WriteLine($"Reading MyUncompressedNullableUInt at byte location {bytesSoFar}"); 
            _MyUncompressedNullableUInt = span.ToNullableUInt32(ref bytesSoFar);
            TabbedText.WriteLine($"Reading MyUncompressedNullableULong at byte location {bytesSoFar}"); 
            _MyUncompressedNullableULong = span.ToNullableUInt64(ref bytesSoFar);
            TabbedText.WriteLine($"Reading MyUncompressedNullableUShort at byte location {bytesSoFar}"); 
            _MyUncompressedNullableUShort = span.ToNullableUInt16(ref bytesSoFar);
            TabbedText.WriteLine($"Reading MyUncompressedShort at byte location {bytesSoFar}"); 
            _MyUncompressedShort = span.ToInt16(ref bytesSoFar);
            TabbedText.WriteLine($"Reading MyUncompressedTimeSpan at byte location {bytesSoFar}"); 
            _MyUncompressedTimeSpan = span.ToTimeSpan(ref bytesSoFar);
            TabbedText.WriteLine($"Reading MyUncompressedUInt at byte location {bytesSoFar}"); 
            _MyUncompressedUInt = span.ToUInt32(ref bytesSoFar);
            TabbedText.WriteLine($"Reading MyUncompressedULong at byte location {bytesSoFar}"); 
            _MyUncompressedULong = span.ToUInt64(ref bytesSoFar);
            TabbedText.WriteLine($"Reading MyUncompressedUShort at byte location {bytesSoFar}"); 
            _MyUncompressedUShort = span.ToUInt16(ref bytesSoFar);
        }
        
        protected virtual int ConvertFromBytesForChildProperties(ReadOnlySpan<byte> span, IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, int indexOfFirstChild, ref int bytesSoFar)
        {
            int totalChildrenBytes = 0;
            return totalChildrenBytes;
        }
        
        public virtual void SerializeToExistingBuffer(ref BufferWriter writer, in LazinatorSerializationOptions options)
        {
            TabbedText.WriteLine($"Initiating serialization of LazinatorTests.Examples.ExampleHierarchy.UncompressedContainer ");
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
            TabbedText.WriteLine($"Writing properties for LazinatorTests.Examples.ExampleHierarchy.UncompressedContainer starting at {writer.ActiveMemoryPosition}.");
            TabbedText.WriteLine($"Includes? uniqueID {(LazinatorGenericID.IsEmpty ? LazinatorUniqueID.ToString() : String.Join("","",LazinatorGenericID.TypeAndInnerTypeIDs.ToArray()))} {includeUniqueID}, Lazinator version {Lazinator.Support.LazinatorVersionInfo.LazinatorIntVersion} True, Object version {LazinatorObjectVersion} True, IncludeChildrenMode {options.IncludeChildrenMode} True");
            TabbedText.WriteLine($"IsDirty {IsDirty} DescendantIsDirty {DescendantIsDirty} HasParentClass {LazinatorParents.Any()}");
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
            TabbedText.WriteLine($"Byte {writer.ActiveMemoryPosition} (end of UncompressedContainer) ");
            
        }
        
        protected virtual void WritePrimitivePropertiesIntoBuffer(ref BufferWriter writer, in LazinatorSerializationOptions options, bool includeUniqueID)
        {
            TabbedText.WriteLine($"Byte {writer.ActiveMemoryPosition}, MyUncompressed value {_MyUncompressed}");
            TabbedText.Tabs++;
            EncodeCharAndString.WriteStringUtf8WithVarIntPrefix(ref writer, _MyUncompressed);
            TabbedText.Tabs--;
            TabbedText.WriteLine($"Byte {writer.ActiveMemoryPosition}, MyUncompressedDateTime value {_MyUncompressedDateTime}");
            TabbedText.Tabs++;
            WriteUncompressedPrimitives.WriteDateTime(ref writer, _MyUncompressedDateTime);
            TabbedText.Tabs--;
            TabbedText.WriteLine($"Byte {writer.ActiveMemoryPosition}, MyUncompressedDecimal value {_MyUncompressedDecimal}");
            TabbedText.Tabs++;
            WriteUncompressedPrimitives.WriteDecimal(ref writer, _MyUncompressedDecimal);
            TabbedText.Tabs--;
            TabbedText.WriteLine($"Byte {writer.ActiveMemoryPosition}, MyUncompressedInt value {_MyUncompressedInt}");
            TabbedText.Tabs++;
            WriteUncompressedPrimitives.WriteInt(ref writer, _MyUncompressedInt);
            TabbedText.Tabs--;
            TabbedText.WriteLine($"Byte {writer.ActiveMemoryPosition}, MyUncompressedLong value {_MyUncompressedLong}");
            TabbedText.Tabs++;
            WriteUncompressedPrimitives.WriteLong(ref writer, _MyUncompressedLong);
            TabbedText.Tabs--;
            TabbedText.WriteLine($"Byte {writer.ActiveMemoryPosition}, MyUncompressedNullableDateTime value {_MyUncompressedNullableDateTime}");
            TabbedText.Tabs++;
            WriteUncompressedPrimitives.WriteNullableDateTime(ref writer, _MyUncompressedNullableDateTime);
            TabbedText.Tabs--;
            TabbedText.WriteLine($"Byte {writer.ActiveMemoryPosition}, MyUncompressedNullableDecimal value {_MyUncompressedNullableDecimal}");
            TabbedText.Tabs++;
            WriteUncompressedPrimitives.WriteNullableDecimal(ref writer, _MyUncompressedNullableDecimal);
            TabbedText.Tabs--;
            TabbedText.WriteLine($"Byte {writer.ActiveMemoryPosition}, MyUncompressedNullableInt value {_MyUncompressedNullableInt}");
            TabbedText.Tabs++;
            WriteUncompressedPrimitives.WriteNullableInt(ref writer, _MyUncompressedNullableInt);
            TabbedText.Tabs--;
            TabbedText.WriteLine($"Byte {writer.ActiveMemoryPosition}, MyUncompressedNullableLong value {_MyUncompressedNullableLong}");
            TabbedText.Tabs++;
            WriteUncompressedPrimitives.WriteNullableLong(ref writer, _MyUncompressedNullableLong);
            TabbedText.Tabs--;
            TabbedText.WriteLine($"Byte {writer.ActiveMemoryPosition}, MyUncompressedNullableShort value {_MyUncompressedNullableShort}");
            TabbedText.Tabs++;
            WriteUncompressedPrimitives.WriteNullableShort(ref writer, _MyUncompressedNullableShort);
            TabbedText.Tabs--;
            TabbedText.WriteLine($"Byte {writer.ActiveMemoryPosition}, MyUncompressedNullableTimeSpan value {_MyUncompressedNullableTimeSpan}");
            TabbedText.Tabs++;
            WriteUncompressedPrimitives.WriteNullableTimeSpan(ref writer, _MyUncompressedNullableTimeSpan);
            TabbedText.Tabs--;
            TabbedText.WriteLine($"Byte {writer.ActiveMemoryPosition}, MyUncompressedNullableUInt value {_MyUncompressedNullableUInt}");
            TabbedText.Tabs++;
            WriteUncompressedPrimitives.WriteNullableUInt(ref writer, _MyUncompressedNullableUInt);
            TabbedText.Tabs--;
            TabbedText.WriteLine($"Byte {writer.ActiveMemoryPosition}, MyUncompressedNullableULong value {_MyUncompressedNullableULong}");
            TabbedText.Tabs++;
            WriteUncompressedPrimitives.WriteNullableULong(ref writer, _MyUncompressedNullableULong);
            TabbedText.Tabs--;
            TabbedText.WriteLine($"Byte {writer.ActiveMemoryPosition}, MyUncompressedNullableUShort value {_MyUncompressedNullableUShort}");
            TabbedText.Tabs++;
            WriteUncompressedPrimitives.WriteNullableUShort(ref writer, _MyUncompressedNullableUShort);
            TabbedText.Tabs--;
            TabbedText.WriteLine($"Byte {writer.ActiveMemoryPosition}, MyUncompressedShort value {_MyUncompressedShort}");
            TabbedText.Tabs++;
            WriteUncompressedPrimitives.WriteShort(ref writer, _MyUncompressedShort);
            TabbedText.Tabs--;
            TabbedText.WriteLine($"Byte {writer.ActiveMemoryPosition}, MyUncompressedTimeSpan value {_MyUncompressedTimeSpan}");
            TabbedText.Tabs++;
            WriteUncompressedPrimitives.WriteTimeSpan(ref writer, _MyUncompressedTimeSpan);
            TabbedText.Tabs--;
            TabbedText.WriteLine($"Byte {writer.ActiveMemoryPosition}, MyUncompressedUInt value {_MyUncompressedUInt}");
            TabbedText.Tabs++;
            WriteUncompressedPrimitives.WriteUInt(ref writer, _MyUncompressedUInt);
            TabbedText.Tabs--;
            TabbedText.WriteLine($"Byte {writer.ActiveMemoryPosition}, MyUncompressedULong value {_MyUncompressedULong}");
            TabbedText.Tabs++;
            WriteUncompressedPrimitives.WriteULong(ref writer, _MyUncompressedULong);
            TabbedText.Tabs--;
            TabbedText.WriteLine($"Byte {writer.ActiveMemoryPosition}, MyUncompressedUShort value {_MyUncompressedUShort}");
            TabbedText.Tabs++;
            WriteUncompressedPrimitives.WriteUShort(ref writer, _MyUncompressedUShort);
            TabbedText.Tabs--;
        }
        protected virtual void WriteChildrenPropertiesIntoBuffer(ref BufferWriter writer, LazinatorSerializationOptions options, bool includeUniqueID, int startOfObjectPosition)
        {
            
        }
    }
}
