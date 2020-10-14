//90ef819c-49a5-3923-fe00-f4195129f639
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.393
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

#nullable disable
namespace LazinatorTests.Examples.Hierarchy
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
    public partial class RecursiveExample : ILazinator
    {
        public bool IsStruct => false;
        
        /* Property definitions */
        
        protected int _RecursiveClass_ByteIndex;
        protected int _RecursiveInterface_ByteIndex;
        protected virtual int _RecursiveClass_ByteLength => _RecursiveInterface_ByteIndex - _RecursiveClass_ByteIndex;
        private int _RecursiveExample_EndByteIndex;
        protected int _RecursiveInterface_ByteLength => _RecursiveExample_EndByteIndex - _RecursiveInterface_ByteIndex;
        protected virtual int _OverallEndByteIndex => _RecursiveExample_EndByteIndex;
        
        
        protected RecursiveExample _RecursiveClass;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public RecursiveExample RecursiveClass
        {
            get
            {
                if (!_RecursiveClass_Accessed)
                {
                    LazinateRecursiveClass();
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
        private void LazinateRecursiveClass()
        {
            if (LazinatorMemoryStorage.Length == 0)
            {
                _RecursiveClass = null;
            }else
            {
                LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _RecursiveClass_ByteIndex, _RecursiveClass_ByteLength, true, false, null);
                
                _RecursiveClass = DeserializationFactory.Instance.CreateBaseOrDerivedType(1047, (c, p) => new RecursiveExample(c, p), childData, this); 
            }
            _RecursiveClass_Accessed = true;
        }
        
        
        protected IRecursiveExample _RecursiveInterface;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public IRecursiveExample RecursiveInterface
        {
            get
            {
                if (!_RecursiveInterface_Accessed)
                {
                    LazinateRecursiveInterface();
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
        private void LazinateRecursiveInterface()
        {
            if (LazinatorMemoryStorage.Length == 0)
            {
                _RecursiveInterface = null;
            }else
            {
                LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _RecursiveInterface_ByteIndex, _RecursiveInterface_ByteLength, true, false, null);
                
                _RecursiveInterface = DeserializationFactory.Instance.CreateBasedOnType<IRecursiveExample>(childData, this); 
            }
            _RecursiveInterface_Accessed = true;
        }
        
        
        /* Serialization, deserialization, and object relationships */
        
        public RecursiveExample(IncludeChildrenMode originalIncludeChildrenMode = IncludeChildrenMode.IncludeAllChildren)
        {
            OriginalIncludeChildrenMode = originalIncludeChildrenMode;
        }
        
        public RecursiveExample(LazinatorMemory serializedBytes, ILazinator parent = null, IncludeChildrenMode originalIncludeChildrenMode = IncludeChildrenMode.IncludeAllChildren, int? lazinatorObjectVersion = null)
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
        
        public virtual IncludeChildrenMode OriginalIncludeChildrenMode { get; set; }
        
        protected virtual int Deserialize()
        {
            FreeInMemoryObjects();
            int bytesSoFar = 0;
            ReadOnlySpan<byte> span = LazinatorMemoryStorage.InitialMemory.Span;
            if (span.Length == 0)
            {
                return 0;
            }
            
            ReadGenericIDIfApplicable(ContainsOpenGenericParameters, LazinatorUniqueID, span, ref bytesSoFar);
            
            int lazinatorLibraryVersion = span.ToDecompressedInt32(ref bytesSoFar);
            
            int serializedVersionNumber = span.ToDecompressedInt32(ref bytesSoFar);
            
            OriginalIncludeChildrenMode = (IncludeChildrenMode)span.ToByte(ref bytesSoFar);
            
            ConvertFromBytesAfterHeader(OriginalIncludeChildrenMode, serializedVersionNumber, ref bytesSoFar);
            return _OverallEndByteIndex;
        }
        
        public virtual LazinatorMemory SerializeLazinator(IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer) 
        {
            if (LazinatorMemoryStorage.IsEmpty || includeChildrenMode != OriginalIncludeChildrenMode || (verifyCleanness || IsDirty || (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && DescendantIsDirty)))
            {
                return EncodeToNewBuffer(includeChildrenMode, verifyCleanness, updateStoredBuffer);
            }
            BinaryBufferWriter writer = new BinaryBufferWriter(LazinatorMemoryStorage.Length);
            LazinatorMemoryStorage.WriteToBinaryBuffer(ref writer);
            return writer.LazinatorMemory;
        }
        
        protected virtual LazinatorMemory EncodeToNewBuffer(IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer) 
        {
            int bufferSize = LazinatorMemoryStorage.Length == 0 ? ExpandableBytes.DefaultMinBufferSize : LazinatorMemoryStorage.Length;
            BinaryBufferWriter writer = new BinaryBufferWriter(bufferSize);
            SerializeToExistingBuffer(ref writer, includeChildrenMode, verifyCleanness, updateStoredBuffer);
            return writer.LazinatorMemory;
        }
        
        public virtual ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode = IncludeChildrenMode.IncludeAllChildren, CloneBufferOptions cloneBufferOptions = CloneBufferOptions.IndependentBuffers)
        {
            RecursiveExample clone;
            if (cloneBufferOptions == CloneBufferOptions.NoBuffer)
            {
                clone = new RecursiveExample(includeChildrenMode);
                clone.LazinatorObjectVersion = LazinatorObjectVersion;
                clone = (RecursiveExample)AssignCloneProperties(clone, includeChildrenMode);
            }
            else
            {
                LazinatorMemory bytes = EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, false, IsDirty, DescendantIsDirty, false, LazinatorMemoryStorage, false, this);
                clone = new RecursiveExample(bytes);
            }
            return clone;
        }
        
        protected virtual ILazinator AssignCloneProperties(ILazinator clone, IncludeChildrenMode includeChildrenMode)
        {
            clone.FreeInMemoryObjects();
            RecursiveExample typedClone = (RecursiveExample) clone;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                if (RecursiveClass == null)
                {
                    typedClone.RecursiveClass = null;
                }else
                {
                    typedClone.RecursiveClass = (RecursiveExample) RecursiveClass.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer);
                }
            }
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                if (RecursiveInterface == null)
                {
                    typedClone.RecursiveInterface = null;
                }else
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
        
        protected virtual void DeserializeLazinator(LazinatorMemory serializedBytes)
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
        
        public virtual void UpdateStoredBuffer()
        {
            if (!IsDirty && !DescendantIsDirty && LazinatorMemoryStorage.Length > 0 && OriginalIncludeChildrenMode == IncludeChildrenMode.IncludeAllChildren)
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
                LazinatorMemoryStorage.WriteToBinaryBuffer(ref writer);
                LazinatorMemoryStorage = writer.LazinatorMemory;
            }
            OriginalIncludeChildrenMode = IncludeChildrenMode.IncludeAllChildren;
            if (!LazinatorParents.Any())
            {
                previousBuffer.Dispose();
            }
        }
        
        public virtual bool NonBinaryHash32 => false;
        
        
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
            if (enumerateNulls && (!exploreOnlyDeserializedChildren || _RecursiveClass_Accessed) && RecursiveClass == null)
            {
                yield return ("RecursiveClass", default);
            }else
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
            if (enumerateNulls && (!exploreOnlyDeserializedChildren || _RecursiveInterface_Accessed) && RecursiveInterface == null)
            {
                yield return ("RecursiveInterface", default);
            }else
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
            yield break;
        }
        
        
        public virtual IEnumerable<(string propertyName, object descendant)> EnumerateNonLazinatorProperties()
        {
            yield break;
        }
        
        public virtual ILazinator ForEachLazinator(Func<ILazinator, ILazinator> changeFunc, bool exploreOnlyDeserializedChildren, bool changeThisLevel)
        {
            if ((!exploreOnlyDeserializedChildren && RecursiveClass != null) || (_RecursiveClass_Accessed && _RecursiveClass != null))
            {
                _RecursiveClass = (RecursiveExample) _RecursiveClass.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, true);
            }
            if ((!exploreOnlyDeserializedChildren && RecursiveInterface != null) || (_RecursiveInterface_Accessed && _RecursiveInterface != null))
            {
                _RecursiveInterface = (IRecursiveExample) _RecursiveInterface.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, true);
            }
            if (changeThisLevel && changeFunc != null)
            {
                return changeFunc(this);
            }
            return this;
        }
        
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
        
        /* Conversion */
        
        public virtual int LazinatorUniqueID => 1047;
        
        protected virtual bool ContainsOpenGenericParameters => false;
        public virtual LazinatorGenericIDType LazinatorGenericID => default;
        
        
        public virtual int LazinatorObjectVersion { get; set; } = 0;
        
        
        protected virtual void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            ReadOnlySpan<byte> span = LazinatorMemoryStorage.InitialMemory.Span;
            ConvertFromBytesForPrimitiveProperties(span, includeChildrenMode, serializedVersionNumber, ref bytesSoFar);
            int lengthForLengths = 0;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                lengthForLengths += 8;
            }
            int totalChildrenSize = ConvertFromBytesForChildProperties(span, includeChildrenMode, serializedVersionNumber, bytesSoFar + lengthForLengths, ref bytesSoFar);
            bytesSoFar += totalChildrenSize;
        }
        
        protected virtual void ConvertFromBytesForPrimitiveProperties(ReadOnlySpan<byte> span, IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
        }
        
        protected virtual int ConvertFromBytesForChildProperties(ReadOnlySpan<byte> span, IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, int indexOfFirstChild, ref int bytesSoFar)
        {
            int totalChildrenBytes = 0;
            TabbedText.WriteLine($"Reading length of RecursiveClass at byte location {bytesSoFar} to determine location: {indexOfFirstChild + totalChildrenBytes}"); 
            _RecursiveClass_ByteIndex = indexOfFirstChild + totalChildrenBytes;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                totalChildrenBytes += span.ToInt32(ref bytesSoFar);
            }
            TabbedText.WriteLine($"Reading length of RecursiveInterface at byte location {bytesSoFar} to determine location: {indexOfFirstChild + totalChildrenBytes}"); 
            _RecursiveInterface_ByteIndex = indexOfFirstChild + totalChildrenBytes;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                totalChildrenBytes += span.ToInt32(ref bytesSoFar);
            }
            _RecursiveExample_EndByteIndex = indexOfFirstChild + totalChildrenBytes;
            return totalChildrenBytes;
        }
        
        public virtual void SerializeToExistingBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            TabbedText.WriteLine($"Initiating serialization of LazinatorTests.Examples.Hierarchy.RecursiveExample ");
            if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren)
            {
                updateStoredBuffer = false;
            }
            int startPosition = writer.Position;
            WritePropertiesIntoBuffer(ref writer, includeChildrenMode, verifyCleanness, updateStoredBuffer, true);
            if (updateStoredBuffer)
            {
                UpdateStoredBuffer(ref writer, startPosition, writer.Position - startPosition, includeChildrenMode, false);
            }
        }
        
        public virtual void UpdateStoredBuffer(ref BinaryBufferWriter writer, int startPosition, int length, IncludeChildrenMode includeChildrenMode, bool updateDeserializedChildren)
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
        
        protected virtual void UpdateDeserializedChildren(ref BinaryBufferWriter writer, int startPosition)
        {
            if (_RecursiveClass_Accessed && _RecursiveClass != null)
            {
                RecursiveClass.UpdateStoredBuffer(ref writer, startPosition + _RecursiveClass_ByteIndex + sizeof(int), _RecursiveClass_ByteLength - sizeof(int), IncludeChildrenMode.IncludeAllChildren, true);
            }
            if (_RecursiveInterface_Accessed && _RecursiveInterface != null)
            {
                RecursiveInterface.UpdateStoredBuffer(ref writer, startPosition + _RecursiveInterface_ByteIndex + sizeof(int), _RecursiveInterface_ByteLength - sizeof(int), IncludeChildrenMode.IncludeAllChildren, true);
            }
        }
        
        
        protected virtual void WritePropertiesIntoBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer, bool includeUniqueID)
        {
            int startPosition = writer.Position;
            TabbedText.WriteLine($"Writing properties for LazinatorTests.Examples.Hierarchy.RecursiveExample starting at {writer.Position}.");
            TabbedText.WriteLine($"Includes? uniqueID {(LazinatorGenericID.IsEmpty ? LazinatorUniqueID.ToString() : String.Join("","",LazinatorGenericID.TypeAndInnerTypeIDs.ToArray()))} {includeUniqueID}, Lazinator version {Lazinator.Support.LazinatorVersionInfo.LazinatorIntVersion} True, Object version {LazinatorObjectVersion} True, IncludeChildrenMode {includeChildrenMode} True");
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
            writer.Write((byte)includeChildrenMode);
            // write properties
            
            
            int lengthForLengths = 0;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                lengthForLengths += 8;
            }
            Span<byte> lengthsSpan = writer.FreeSpan.Slice(0, lengthForLengths);
            writer.Skip(lengthForLengths);TabbedText.WriteLine($"Byte {writer.Position}, Leaving {lengthForLengths} bytes to store lengths of child objects");
            WriteChildrenPropertiesIntoBuffer(ref writer, includeChildrenMode, verifyCleanness, updateStoredBuffer, includeUniqueID, startPosition, ref lengthsSpan);
            TabbedText.WriteLine($"Byte {writer.Position} (end of RecursiveExample) ");
        }
        
        protected virtual void WritePrimitivePropertiesIntoBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer, bool includeUniqueID)
        {
        }
        
        protected virtual void WriteChildrenPropertiesIntoBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer, bool includeUniqueID, int startOfObjectPosition, ref Span<byte> lengthsSpan)
        {
            int startOfChildPosition = 0;
            int lengthValue = 0;
            TabbedText.WriteLine($"Byte {writer.Position}, RecursiveClass (accessed? {_RecursiveClass_Accessed}) (backing var null? {_RecursiveClass == null}) ");
            TabbedText.Tabs++;
            startOfChildPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                if ((includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode) && !_RecursiveClass_Accessed)
                {
                    var deserialized = RecursiveClass;
                }
                WriteChild(ref writer, ref _RecursiveClass, includeChildrenMode, _RecursiveClass_Accessed, () => GetChildSlice(LazinatorMemoryStorage, _RecursiveClass_ByteIndex, _RecursiveClass_ByteLength, true, false, null), verifyCleanness, updateStoredBuffer, false, true, this);
                lengthValue = writer.Position - startOfChildPosition;
                WriteInt(lengthsSpan, lengthValue);
                lengthsSpan = lengthsSpan.Slice(sizeof(int));
            }
            if (updateStoredBuffer)
            {
                _RecursiveClass_ByteIndex = startOfChildPosition - startOfObjectPosition;
                
            }
            TabbedText.Tabs--;
            TabbedText.WriteLine($"Byte {writer.Position}, RecursiveInterface (accessed? {_RecursiveInterface_Accessed}) (backing var null? {_RecursiveInterface == null}) ");
            TabbedText.Tabs++;
            startOfChildPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                if ((includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode) && !_RecursiveInterface_Accessed)
                {
                    var deserialized = RecursiveInterface;
                }
                WriteChild(ref writer, ref _RecursiveInterface, includeChildrenMode, _RecursiveInterface_Accessed, () => GetChildSlice(LazinatorMemoryStorage, _RecursiveInterface_ByteIndex, _RecursiveInterface_ByteLength, true, false, null), verifyCleanness, updateStoredBuffer, false, true, this);
                lengthValue = writer.Position - startOfChildPosition;
                WriteInt(lengthsSpan, lengthValue);
                lengthsSpan = lengthsSpan.Slice(sizeof(int));
            }
            if (updateStoredBuffer)
            {
                _RecursiveInterface_ByteIndex = startOfChildPosition - startOfObjectPosition;
                
            }
            TabbedText.Tabs--;
            if (updateStoredBuffer)
            {
                _RecursiveExample_EndByteIndex = writer.Position - startOfObjectPosition;
            }
        }
        
    }
}
