//5594e9eb-e8d9-5c6b-9464-809f48bec925
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.232
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LazinatorTests.Examples.Abstract
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
    public partial class Concrete6 : Concrete5, ILazinator
    {
        /* Clone overrides */
        
        public Concrete6() : base()
        {
        }
        
        public override ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode = IncludeChildrenMode.IncludeAllChildren, CloneBufferOptions cloneBufferOptions = CloneBufferOptions.LinkedBuffer)
        {
            var clone = new Concrete6()
            {
                OriginalIncludeChildrenMode = includeChildrenMode
            };
            if (clone.LazinatorObjectVersion != LazinatorObjectVersion)
            {
                clone.LazinatorObjectVersion = LazinatorObjectVersion;
            }
            
            if (cloneBufferOptions == CloneBufferOptions.NoBuffer)
            {
                AssignCloneProperties(clone, includeChildrenMode);
            }
            else
            {
                LazinatorMemory bytes = EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, false, IsDirty, DescendantIsDirty, false, LazinatorMemoryStorage, (EncodeManuallyDelegate)EncodeToNewBuffer, cloneBufferOptions == CloneBufferOptions.SharedBuffer);
                clone.DeserializeLazinator(bytes);
                if (cloneBufferOptions == CloneBufferOptions.IndependentBuffers)
                {
                    clone.LazinatorMemoryStorage.DisposeIndependently();
                }
            }
            clone.LazinatorParents = default;
            return clone;
        }
        
        protected override void AssignCloneProperties(ILazinator clone, IncludeChildrenMode includeChildrenMode)
        {
            base.AssignCloneProperties(clone, includeChildrenMode);
            Concrete6 typedClone = (Concrete6) clone;
            typedClone.IntList6 = Clone_List_Gint_g(IntList6);
        }
        
        /* Properties */
        protected int _IntList6_ByteIndex;
        private int _Concrete6_EndByteIndex;
        protected virtual int _IntList6_ByteLength => _Concrete6_EndByteIndex - _IntList6_ByteIndex;
        
        protected List<int> _IntList6;
        public List<int> IntList6
        {
            get
            {
                if (!_IntList6_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _IntList6 = default(List<int>);
                    }
                    else
                    {
                        LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _IntList6_ByteIndex, _IntList6_ByteLength, false, false, null);
                        _IntList6 = ConvertFromBytes_List_Gint_g(childData);
                    }
                    _IntList6_Accessed = true;
                }
                IsDirty = true; 
                return _IntList6;
            }
            set
            {
                IsDirty = true;
                DescendantIsDirty = true;
                _IntList6 = value;
                _IntList6_Accessed = true;
            }
        }
        protected bool _IntList6_Accessed;
        
        public override IEnumerable<(string propertyName, ILazinator descendant)> EnumerateLazinatorDescendants(Func<ILazinator, bool> matchCriterion, bool stopExploringBelowMatch, Func<ILazinator, bool> exploreCriterion, bool exploreOnlyDeserializedChildren, bool enumerateNulls)
        {
            foreach (var inheritedYield in base.EnumerateLazinatorDescendants(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
            {
                yield return inheritedYield;
            }
            yield break;
        }
        
        
        public override IEnumerable<(string propertyName, object descendant)> EnumerateNonLazinatorProperties()
        {
            foreach (var inheritedYield in base.EnumerateNonLazinatorProperties())
            {
                yield return inheritedYield;
            }
            yield return ("IntList6", (object)IntList6);
            yield break;
        }
        
        public override void FreeInMemoryObjects()
        {
            base.FreeInMemoryObjects();
            _IntList6 = default;
            _IntList6_Accessed = false;
            IsDirty = false;
            DescendantIsDirty = false;
            HasChanged = false;
            DescendantHasChanged = false;
        }
        
        /* Conversion */
        
        public override int LazinatorUniqueID => 249;
        
        protected override bool ContainsOpenGenericParameters => false;
        public override LazinatorGenericIDType LazinatorGenericID
        {
            get => default;
            set { }
        }
        
        public override int LazinatorObjectVersion { get; set; } = 0;
        
        
        public override void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            base.ConvertFromBytesAfterHeader(OriginalIncludeChildrenMode, serializedVersionNumber, ref bytesSoFar);
            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
            _IntList6_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            _Concrete6_EndByteIndex = bytesSoFar;
        }
        
        public override void SerializeExistingBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren)
            {
                updateStoredBuffer = false;
            }
            int startPosition = writer.Position;
            WritePropertiesIntoBuffer(ref writer, includeChildrenMode, verifyCleanness, updateStoredBuffer, true);
            if (updateStoredBuffer)
            {
                
                _IsDirty = false;
                if (includeChildrenMode == IncludeChildrenMode.IncludeAllChildren)
                {
                    _DescendantIsDirty = false;
                }
                else
                {
                    throw new Exception("Cannot update stored buffer when serializing only some children.");
                }
                
                var newBuffer = writer.Slice(startPosition);
                if (_LazinatorMemoryStorage != null)
                {
                    var ownedMemory = _LazinatorMemoryStorage.OwnedMemory;
                    if (LazinatorParents.ParentsShareBuffer(ownedMemory))
                    {
                        _LazinatorMemoryStorage.DisposeWithThis(newBuffer);
                    }
                    else
                    {
                        _LazinatorMemoryStorage.ReplaceWithNewBuffer(newBuffer);
                    }
                }
                _LazinatorMemoryStorage = newBuffer;
            }
        }
        
        protected override void WritePropertiesIntoBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer, bool includeUniqueID)
        {
            int startPosition = writer.Position;
            int startOfObjectPosition = 0;
            base.WritePropertiesIntoBuffer(ref writer, includeChildrenMode, verifyCleanness, updateStoredBuffer, includeUniqueID);
            // write properties
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode && !_IntList6_Accessed)
            {
                var deserialized = IntList6;
            }
            WriteNonLazinatorObject(
            nonLazinatorObject: _IntList6, isBelievedDirty: _IntList6_Accessed || (includeChildrenMode != OriginalIncludeChildrenMode),
            isAccessed: _IntList6_Accessed, writer: ref writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorMemoryStorage, _IntList6_ByteIndex, _IntList6_ByteLength, false, false, null),
            verifyCleanness: false,
            binaryWriterAction: (ref BinaryBufferWriter w, bool v) =>
            ConvertToBytes_List_Gint_g(ref w, _IntList6,
            includeChildrenMode, v, updateStoredBuffer));
            if (updateStoredBuffer)
            {
                _IntList6_ByteIndex = startOfObjectPosition - startPosition;
            }
            if (updateStoredBuffer)
            {
                _Concrete6_EndByteIndex = writer.Position - startPosition;
            }
        }
        
        /* Conversion of supported collections and tuples */
        
        private static List<int> ConvertFromBytes_List_Gint_g(LazinatorMemory storage)
        {
            if (storage.Length == 0)
            {
                return default(List<int>);
            }
            ReadOnlySpan<byte> span = storage.Span;
            
            int bytesSoFar = 0;
            int collectionLength = span.ToDecompressedInt(ref bytesSoFar);
            
            List<int> collection = new List<int>(collectionLength);
            for (int itemIndex = 0; itemIndex < collectionLength; itemIndex++)
            {
                int item = span.ToDecompressedInt(ref bytesSoFar);
                collection.Add(item);
            }
            
            return collection;
        }
        
        private static void ConvertToBytes_List_Gint_g(ref BinaryBufferWriter writer, List<int> itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            if (itemToConvert == default(List<int>))
            {
                return;
            }
            CompressedIntegralTypes.WriteCompressedInt(ref writer, itemToConvert.Count);
            int itemToConvertCount = itemToConvert.Count;
            for (int itemIndex = 0; itemIndex < itemToConvertCount; itemIndex++)
            {
                CompressedIntegralTypes.WriteCompressedInt(ref writer, itemToConvert[itemIndex]);
            }
        }
        
        private static List<int> Clone_List_Gint_g(List<int> itemToClone)
        {
            if (itemToClone == null)
            {
                return default;
            }
            
            int collectionLength = itemToClone.Count;
            List<int> collection = new List<int>(collectionLength);
            int itemToCloneCount = itemToClone.Count;
            for (int itemIndex = 0; itemIndex < itemToCloneCount; itemIndex++)
            {
                var itemCopied = (int) itemToClone[itemIndex];
                collection.Add(itemCopied);
            }
            return collection;
        }
        
    }
}
