//be1f2d01-d723-8398-112f-09bfa6e466fb
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.318
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LazinatorTests.Examples
{
    using Lazinator.Attributes;
    using Lazinator.Buffers;
    using Lazinator.Core;
    using Lazinator.Exceptions;
    using Lazinator.Support;
    using System;
    using System.Buffers;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;
    using static Lazinator.Core.LazinatorUtilities;
    
    [Autogenerated]
    public partial struct NonLazinatorInterchangeStruct : ILazinator
    {
        public bool IsStruct => true;
        
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
                throw new FormatException("Wrong Lazinator type initialized.");
            }
            
            int lazinatorLibraryVersion = span.ToDecompressedInt(ref bytesSoFar);
            
            int serializedVersionNumber = span.ToDecompressedInt(ref bytesSoFar);
            
            OriginalIncludeChildrenMode = (IncludeChildrenMode)span.ToByte(ref bytesSoFar);
            
            ConvertFromBytesAfterHeader(OriginalIncludeChildrenMode, serializedVersionNumber, ref bytesSoFar);
            return bytesSoFar;
        }
        
        public LazinatorMemory SerializeLazinator(IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            return EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, verifyCleanness, IsDirty, DescendantIsDirty, false, LazinatorMemoryStorage, (EncodeManuallyDelegate) EncodeToNewBuffer, updateStoredBuffer);
        }
        
        LazinatorMemory EncodeToNewBuffer(IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer) => LazinatorUtilities.EncodeToNewBinaryBufferWriter(this, includeChildrenMode, verifyCleanness, updateStoredBuffer);
        
        public ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode = IncludeChildrenMode.IncludeAllChildren, CloneBufferOptions cloneBufferOptions = CloneBufferOptions.IndependentBuffers)
        {
            var clone = new NonLazinatorInterchangeStruct()
            {
                OriginalIncludeChildrenMode = includeChildrenMode
            };
            if (clone.LazinatorObjectVersion != LazinatorObjectVersion)
            {
                clone.LazinatorObjectVersion = LazinatorObjectVersion;
            }
            
            if (cloneBufferOptions == CloneBufferOptions.NoBuffer)
            {
                AssignCloneProperties(ref clone, includeChildrenMode);
            }
            else
            {
                LazinatorMemory bytes = EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, false, IsDirty, DescendantIsDirty, false, LazinatorMemoryStorage, (EncodeManuallyDelegate)EncodeToNewBuffer, false);
                clone.DeserializeLazinator(bytes);
            }
            clone.LazinatorParents = default;
            return clone;
        }
        
        void AssignCloneProperties(ref NonLazinatorInterchangeStruct clone, IncludeChildrenMode includeChildrenMode)
        {
            clone.FreeInMemoryObjects();
            clone.MyInt = MyInt;
            clone.MyString = MyString;
            
            clone.IsDirty = false;}
            
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
            ReadOnlyMemory<byte> LazinatorObjectBytes => LazinatorMemoryStorage?.Memory ?? LazinatorUtilities.EmptyReadOnlyMemory;
            
            public void UpdateStoredBuffer()
            {
                if (LazinatorMemoryStorage == null)
                {
                    throw new NotSupportedException("Cannot use UpdateStoredBuffer on a struct that has not been deserialized. Clone the struct instead."); 
                }
                if (!IsDirty && !DescendantIsDirty && LazinatorObjectBytes.Length > 0 && OriginalIncludeChildrenMode == IncludeChildrenMode.IncludeAllChildren)
                {
                    return;
                }
                LazinatorMemoryStorage = EncodeOrRecycleToNewBuffer(IncludeChildrenMode.IncludeAllChildren, OriginalIncludeChildrenMode, false, IsDirty, DescendantIsDirty, false, LazinatorMemoryStorage, (EncodeManuallyDelegate)EncodeToNewBuffer, true);
                OriginalIncludeChildrenMode = IncludeChildrenMode.IncludeAllChildren;
            }
            
            public int GetByteLength()
            {
                UpdateStoredBuffer();
                return LazinatorObjectBytes.Length;
            }
            
            public bool NonBinaryHash32 => false;
            
            /* Property definitions */
            
            
            
            int _MyInt;
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
            
            string _MyString;
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            public string MyString
            {
                get
                {
                    return _MyString;
                }
                set
                {
                    IsDirty = true;
                    _MyString = value;
                }
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
            
            public IEnumerable<(string propertyName, ILazinator descendant)> EnumerateLazinatorDescendants(Func<ILazinator, bool> matchCriterion, bool stopExploringBelowMatch, Func<ILazinator, bool> exploreCriterion, bool exploreOnlyDeserializedChildren, bool enumerateNulls)
            {
                yield break;
            }
            
            
            public IEnumerable<(string propertyName, object descendant)> EnumerateNonLazinatorProperties()
            {
                yield return ("MyInt", (object)MyInt);
                yield return ("MyString", (object)MyString);
                yield break;
            }
            
            public ILazinator ForEachLazinator(Func<ILazinator, ILazinator> changeFunc, bool exploreOnlyDeserializedChildren)
            {
                return changeFunc(this);
            }
            
            public void FreeInMemoryObjects()
            {
                
                IsDirty = false;
                DescendantIsDirty = false;
                HasChanged = false;
                DescendantHasChanged = false;
            }
            
            /* Conversion */
            
            public int LazinatorUniqueID => 278;
            
            bool ContainsOpenGenericParameters => false;
            public LazinatorGenericIDType LazinatorGenericID
            {
                get => default;
                set { }
            }
            
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
            
            
            public void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
            {
                ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
                _MyInt = span.ToDecompressedInt(ref bytesSoFar);
                _MyString = span.ToString_VarIntLengthUtf8(ref bytesSoFar);
            }
            
            public void SerializeExistingBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
            {
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
            
            public void UpdateStoredBuffer(ref BinaryBufferWriter writer, int startPosition, int length, IncludeChildrenMode includeChildrenMode, bool updateDeserializedChildren)
            {
                _IsDirty = false;
                if (includeChildrenMode == IncludeChildrenMode.IncludeAllChildren)
                {
                    _DescendantIsDirty = false;
                    if (updateDeserializedChildren)
                    {
                    }
                    
                }
                else
                {
                    throw new LazinatorSerializationException("Cannot update stored buffer when serializing only some children.");
                }
                
                var newBuffer = writer.Slice(startPosition, length);
                LazinatorMemoryStorage = newBuffer;
            }
            
            void WritePropertiesIntoBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer, bool includeUniqueID)
            {
                // header information
                if (includeUniqueID)
                {
                    CompressedIntegralTypes.WriteCompressedInt(ref writer, LazinatorUniqueID);
                }
                
                CompressedIntegralTypes.WriteCompressedInt(ref writer, Lazinator.Support.LazinatorVersionInfo.LazinatorIntVersion);
                CompressedIntegralTypes.WriteCompressedInt(ref writer, LazinatorObjectVersion);
                writer.Write((byte)includeChildrenMode);
                // write properties
                CompressedIntegralTypes.WriteCompressedInt(ref writer, _MyInt);
                EncodeCharAndString.WriteStringUtf8WithVarIntPrefix(ref writer, _MyString);
            }
            
        }
    }
