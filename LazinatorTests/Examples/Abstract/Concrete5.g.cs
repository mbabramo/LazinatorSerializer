//1b5c76f8-e239-4f62-20aa-443b1ac5fb5e
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.220
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
    public partial class Concrete5 : Abstract4, ILazinator
    {
        /* Clone overrides */
        
        public Concrete5() : base()
        {
        }
        
        public override ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode, CloneBufferOptions cloneBufferOptions = CloneBufferOptions.LinkedBuffer)
        {
            LazinatorMemory bytes = EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, false, IsDirty, DescendantIsDirty, false, LazinatorMemoryStorage, (EncodeManuallyDelegate)EncodeToNewBuffer, cloneBufferOptions == CloneBufferOptions.SharedBuffer);
            var clone = new Concrete5()
            {
                LazinatorParents = LazinatorParents,
                OriginalIncludeChildrenMode = includeChildrenMode
            };
            clone.DeserializeLazinator(bytes);
            if (cloneBufferOptions == CloneBufferOptions.IndependentBuffers)
            {
                clone.LazinatorMemoryStorage.DisposeIndependently();
            }
            clone.LazinatorParents = default;
            return clone;
        }
        
        /* Properties */
        protected int _IntList5_ByteIndex;
        protected override int _IntList4_ByteLength => _IntList5_ByteIndex - _IntList4_ByteIndex;
        private int _Concrete5_EndByteIndex;
        protected virtual int _IntList5_ByteLength => _Concrete5_EndByteIndex - _IntList5_ByteIndex;
        
        protected string _String4;
        public override string String4
        {
            get
            {
                return _String4;
            }
            set
            {
                IsDirty = true;
                _String4 = value;
            }
        }
        protected string _String5;
        public string String5
        {
            get
            {
                return _String5;
            }
            set
            {
                IsDirty = true;
                _String5 = value;
            }
        }
        protected List<int> _IntList4;
        public override List<int> IntList4
        {
            get
            {
                if (!_IntList4_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _IntList4 = default(List<int>);
                    }
                    else
                    {
                        LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _IntList4_ByteIndex, _IntList4_ByteLength, false, false, null);
                        _IntList4 = ConvertFromBytes_List_Gint_g(childData);
                    }
                    _IntList4_Accessed = true;
                }
                IsDirty = true; 
                return _IntList4;
            }
            set
            {
                IsDirty = true;
                DescendantIsDirty = true;
                _IntList4 = value;
                _IntList4_Accessed = true;
            }
        }
        protected List<int> _IntList5;
        public List<int> IntList5
        {
            get
            {
                if (!_IntList5_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _IntList5 = default(List<int>);
                    }
                    else
                    {
                        LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _IntList5_ByteIndex, _IntList5_ByteLength, false, false, null);
                        _IntList5 = ConvertFromBytes_List_Gint_g(childData);
                    }
                    _IntList5_Accessed = true;
                }
                IsDirty = true; 
                return _IntList5;
            }
            set
            {
                IsDirty = true;
                DescendantIsDirty = true;
                _IntList5 = value;
                _IntList5_Accessed = true;
            }
        }
        protected bool _IntList5_Accessed;
        
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
            yield return ("String4", (object)String4);
            yield return ("String5", (object)String5);
            yield return ("IntList4", (object)IntList4);
            yield return ("IntList5", (object)IntList5);
            yield break;
        }
        
        public override void FreeInMemoryObjects()
        {
            base.FreeInMemoryObjects();
            _IntList4 = default;
            _IntList5 = default;
            _IntList4_Accessed = _IntList5_Accessed = false;
            IsDirty = false;
            DescendantIsDirty = false;
            HasChanged = false;
            DescendantHasChanged = false;
        }
        
        /* Conversion */
        
        public override int LazinatorUniqueID => 239;
        
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
            _String4 = span.ToString_VarIntLengthUtf8(ref bytesSoFar);
            _String5 = span.ToString_VarIntLengthUtf8(ref bytesSoFar);
            _IntList4_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            _IntList5_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            _Concrete5_EndByteIndex = bytesSoFar;
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
                    _LazinatorMemoryStorage.DisposeWithThis(newBuffer);
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
            EncodeCharAndString.WriteStringUtf8WithVarIntPrefix(ref writer, _String4);
            EncodeCharAndString.WriteStringUtf8WithVarIntPrefix(ref writer, _String5);
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren && !_IntList4_Accessed)
            {
                var deserialized = IntList4;
            }
            WriteNonLazinatorObject(
            nonLazinatorObject: _IntList4, isBelievedDirty: _IntList4_Accessed,
            isAccessed: _IntList4_Accessed, writer: ref writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorMemoryStorage, _IntList4_ByteIndex, _IntList4_ByteLength, false, false, null),
            verifyCleanness: false,
            binaryWriterAction: (ref BinaryBufferWriter w, bool v) =>
            ConvertToBytes_List_Gint_g(ref w, _IntList4,
            includeChildrenMode, v, updateStoredBuffer));
            if (updateStoredBuffer)
            {
                _IntList4_ByteIndex = startOfObjectPosition - startPosition;
            }
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren && !_IntList5_Accessed)
            {
                var deserialized = IntList5;
            }
            WriteNonLazinatorObject(
            nonLazinatorObject: _IntList5, isBelievedDirty: _IntList5_Accessed,
            isAccessed: _IntList5_Accessed, writer: ref writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorMemoryStorage, _IntList5_ByteIndex, _IntList5_ByteLength, false, false, null),
            verifyCleanness: false,
            binaryWriterAction: (ref BinaryBufferWriter w, bool v) =>
            ConvertToBytes_List_Gint_g(ref w, _IntList5,
            includeChildrenMode, v, updateStoredBuffer));
            if (updateStoredBuffer)
            {
                _IntList5_ByteIndex = startOfObjectPosition - startPosition;
            }
            if (updateStoredBuffer)
            {
                _Concrete5_EndByteIndex = writer.Position - startPosition;
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
