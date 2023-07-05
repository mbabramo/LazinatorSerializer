//278cdede-1f2a-009c-d5c1-ec2b1b17ac14
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.395
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

#nullable disable
namespace LazinatorTests.Examples.Collections
{
    using Lazinator.Attributes;
    using Lazinator.Buffers;
    using Lazinator.Core;
    using Lazinator.Exceptions;
    using Lazinator.Support;
    using LazinatorTests.Examples;
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
    public partial class Derived_DotNetList_Nested_NonLazinator : DotNetList_Nested_NonLazinator, ILazinator
    {
        /* Property definitions */
        
        protected int _MyLevel2ListNestedNonLazinatorType_ByteIndex;
        private int _Derived_DotNetList_Nested_NonLazinator_EndByteIndex;
        protected virtual  int _MyLevel2ListNestedNonLazinatorType_ByteLength => _Derived_DotNetList_Nested_NonLazinator_EndByteIndex - _MyLevel2ListNestedNonLazinatorType_ByteIndex;
        protected override int _OverallEndByteIndex => _Derived_DotNetList_Nested_NonLazinator_EndByteIndex;
        
        
        protected int _MyLevel2Int;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public int MyLevel2Int
        {
            get
            {
                return _MyLevel2Int;
            }
            set
            {
                IsDirty = true;
                _MyLevel2Int = value;
            }
        }
        
        protected List<List<NonLazinatorClass>> _MyLevel2ListNestedNonLazinatorType;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public List<List<NonLazinatorClass>> MyLevel2ListNestedNonLazinatorType
        {
            get
            {
                if (!_MyLevel2ListNestedNonLazinatorType_Accessed)
                {
                    TabbedText.WriteLine($"Accessing MyLevel2ListNestedNonLazinatorType");
                    LazinateMyLevel2ListNestedNonLazinatorType();
                }
                IsDirty = true; 
                return _MyLevel2ListNestedNonLazinatorType;
            }
            set
            {
                IsDirty = true;
                DescendantIsDirty = true;
                _MyLevel2ListNestedNonLazinatorType = value;
                _MyLevel2ListNestedNonLazinatorType_Accessed = true;
            }
        }
        protected bool _MyLevel2ListNestedNonLazinatorType_Accessed;
        private void LazinateMyLevel2ListNestedNonLazinatorType()
        {
            if (LazinatorMemoryStorage.Length == 0)
            {
                _MyLevel2ListNestedNonLazinatorType = default(List<List<NonLazinatorClass>>);
            }
            else
            {
                LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _MyLevel2ListNestedNonLazinatorType_ByteIndex, _MyLevel2ListNestedNonLazinatorType_ByteLength, null);
                TabbedText.WriteLine($"ILazinator location: {childData.ToLocationString()}");_MyLevel2ListNestedNonLazinatorType = ConvertFromBytes_List_GList_GNonLazinatorClass_g_g(childData);
            }
            _MyLevel2ListNestedNonLazinatorType_Accessed = true;
        }
        
        /* Clone overrides */
        
        public Derived_DotNetList_Nested_NonLazinator(IncludeChildrenMode originalIncludeChildrenMode = IncludeChildrenMode.IncludeAllChildren) : base(originalIncludeChildrenMode)
        {
        }
        
        public Derived_DotNetList_Nested_NonLazinator(LazinatorMemory serializedBytes, ILazinator parent = null, IncludeChildrenMode originalIncludeChildrenMode = IncludeChildrenMode.IncludeAllChildren, int? lazinatorObjectVersion = null) : base(serializedBytes, parent, originalIncludeChildrenMode, lazinatorObjectVersion)
        {
        }
        
        public override ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode = IncludeChildrenMode.IncludeAllChildren, CloneBufferOptions cloneBufferOptions = CloneBufferOptions.IndependentBuffers)
        {
            Derived_DotNetList_Nested_NonLazinator clone;
            if (cloneBufferOptions == CloneBufferOptions.NoBuffer)
            {
                clone = new Derived_DotNetList_Nested_NonLazinator(includeChildrenMode);
                clone.LazinatorObjectVersion = LazinatorObjectVersion;
                clone = (Derived_DotNetList_Nested_NonLazinator)AssignCloneProperties(clone, includeChildrenMode);
            }
            else
            {
                LazinatorMemory bytes = EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, IsDirty, DescendantIsDirty, false, LazinatorMemoryStorage, this);
                clone = new Derived_DotNetList_Nested_NonLazinator(bytes);
            }
            return clone;
        }
        
        protected override ILazinator AssignCloneProperties(ILazinator clone, IncludeChildrenMode includeChildrenMode)
        {
            base.AssignCloneProperties(clone, includeChildrenMode);
            Derived_DotNetList_Nested_NonLazinator typedClone = (Derived_DotNetList_Nested_NonLazinator) clone;
            typedClone.MyLevel2Int = MyLevel2Int;
            typedClone.MyLevel2ListNestedNonLazinatorType = CloneOrChange_List_GList_GNonLazinatorClass_g_g(MyLevel2ListNestedNonLazinatorType, l => l?.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer), false);
            
            return typedClone;
        }
        
        /* Properties */
        
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
            yield return ("MyLevel2Int", (object)MyLevel2Int);
            yield return ("MyLevel2ListNestedNonLazinatorType", (object)MyLevel2ListNestedNonLazinatorType);
            yield break;
        }
        
        public override ILazinator ForEachLazinator(Func<ILazinator, ILazinator> changeFunc, bool exploreOnlyDeserializedChildren, bool changeThisLevel)
        {
            base.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, false);
            if ((!exploreOnlyDeserializedChildren && MyLevel2ListNestedNonLazinatorType != null) || (_MyLevel2ListNestedNonLazinatorType_Accessed && _MyLevel2ListNestedNonLazinatorType != null))
            {
                _MyLevel2ListNestedNonLazinatorType = (List<List<NonLazinatorClass>>) CloneOrChange_List_GList_GNonLazinatorClass_g_g(_MyLevel2ListNestedNonLazinatorType, l => l?.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, true), true);
            }
            if (changeThisLevel && changeFunc != null)
            {
                return changeFunc(this);
            }
            return this;
        }
        
        public override void FreeInMemoryObjects()
        {
            base.FreeInMemoryObjects();
            _MyLevel2ListNestedNonLazinatorType = default;
            _MyLevel2ListNestedNonLazinatorType_Accessed = false;
            IsDirty = false;
            DescendantIsDirty = false;
            HasChanged = false;
            DescendantHasChanged = false;
        }
        
        /* Conversion */
        
        public override int LazinatorUniqueID => 1060;
        
        protected override bool ContainsOpenGenericParameters => false;
        public override LazinatorGenericIDType LazinatorGenericID => default;
        
        
        public override int LazinatorObjectVersion { get; set; } = 0;
        
        
        protected override int ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            TabbedText.WriteLine($"");
            TabbedText.WriteLine($"Converting LazinatorTests.Examples.Collections.Derived_DotNetList_Nested_NonLazinator at position: " + LazinatorMemoryStorage.ToLocationString());
            ReadOnlySpan<byte> span = LazinatorMemoryStorage.InitialReadOnlyMemory.Span;
            ConvertFromBytesForPrimitiveProperties(span, includeChildrenMode, serializedVersionNumber, ref bytesSoFar);
            TabbedText.Tabs++;
            int lengthForLengths = 8;
            int totalChildrenSize = ConvertFromBytesForChildLengths(span, includeChildrenMode, serializedVersionNumber, bytesSoFar + lengthForLengths, ref bytesSoFar);;
            TabbedText.Tabs--;
            return bytesSoFar + totalChildrenSize;
        }
        
        protected override void ConvertFromBytesForPrimitiveProperties(ReadOnlySpan<byte> span, IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            base.ConvertFromBytesForPrimitiveProperties(span, OriginalIncludeChildrenMode, serializedVersionNumber, ref bytesSoFar);
            TabbedText.WriteLine($"Reading MyLevel2Int at byte location {bytesSoFar}"); 
            _MyLevel2Int = span.ToDecompressedInt32(ref bytesSoFar);
        }
        
        protected override int ConvertFromBytesForChildLengths(ReadOnlySpan<byte> span, IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, int indexOfFirstChild, ref int bytesSoFar)
        {
            int totalChildrenBytes = 0;
            totalChildrenBytes = base.ConvertFromBytesForChildLengths(span, OriginalIncludeChildrenMode, serializedVersionNumber, indexOfFirstChild, ref bytesSoFar);
            TabbedText.WriteLine($"MyLevel2ListNestedNonLazinatorType: Length is {bytesSoFar} past above position; start location is {indexOfFirstChild + totalChildrenBytes} past above position"); 
            _MyLevel2ListNestedNonLazinatorType_ByteIndex = indexOfFirstChild + totalChildrenBytes;
            totalChildrenBytes += span.ToInt32(ref bytesSoFar);
            _Derived_DotNetList_Nested_NonLazinator_EndByteIndex = indexOfFirstChild + totalChildrenBytes;
            return totalChildrenBytes;
        }
        
        public override void SerializeToExistingBuffer(ref BufferWriter writer, in LazinatorSerializationOptions options)
        {
            TabbedText.WriteLine("");
            TabbedText.WriteLine($"Initiating serialization of LazinatorTests.Examples.Collections.Derived_DotNetList_Nested_NonLazinator at position {writer.ToLocationString()}");
            int startPosition = writer.ActiveMemoryPosition;
            WritePropertiesIntoBuffer(ref writer, options, true);
            if (options.UpdateStoredBuffer)
            {
                UpdateStoredBuffer(ref writer, startPosition, writer.ActiveMemoryPosition - startPosition, options.IncludeChildrenMode, false);
            }
        }
        
        public override void UpdateStoredBuffer(ref BufferWriter writer, long startPosition, long length, IncludeChildrenMode includeChildrenMode, bool updateDeserializedChildren)
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
        
        protected override void UpdateDeserializedChildren(ref BufferWriter writer, long startPosition)
        {
            base.UpdateDeserializedChildren(ref writer, startPosition);
            if (_MyLevel2ListNestedNonLazinatorType_Accessed && _MyLevel2ListNestedNonLazinatorType != null)
            {
                _MyLevel2ListNestedNonLazinatorType = (List<List<NonLazinatorClass>>) CloneOrChange_List_GList_GNonLazinatorClass_g_g(_MyLevel2ListNestedNonLazinatorType, l => l.RemoveBufferInHierarchy(), true);
            }
            
        }
        
        
        protected override void WritePropertiesIntoBuffer(ref BufferWriter writer, in LazinatorSerializationOptions options, bool includeUniqueID)
        {
            int startPosition = writer.ActiveMemoryPosition;
            TabbedText.WriteLine($"Writing properties for LazinatorTests.Examples.Collections.Derived_DotNetList_Nested_NonLazinator.");
            TabbedText.WriteLine($"Properties uniqueID {(LazinatorGenericID.IsEmpty ? LazinatorUniqueID.ToString() : String.Join(",",LazinatorGenericID.TypeAndInnerTypeIDs.ToArray()))} {(includeUniqueID ? "Included" : "Omitted")}, Lazinator version {Lazinator.Support.LazinatorVersionInfo.LazinatorIntVersion} Included, Object version {LazinatorObjectVersion} Included, IncludeChildrenMode {options.IncludeChildrenMode} Included");
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
            int lengthForLengths = 8;
            
            var previousLengthsPosition = writer.SetLengthsPosition(lengthForLengths);
            TabbedText.WriteLine($"Location {writer.ToLocationString()}, after skipping {lengthForLengths} bytes to store lengths of child objects");
            WriteChildrenPropertiesIntoBuffer(ref writer, options, includeUniqueID, startPosition);
            writer.ResetLengthsPosition(previousLengthsPosition);
            TabbedText.WriteLine($"Position {writer.ToLocationString()} (end of Derived_DotNetList_Nested_NonLazinator) ");
            
        }
        
        protected override void WritePrimitivePropertiesIntoBuffer(ref BufferWriter writer, in LazinatorSerializationOptions options, bool includeUniqueID)
        {
            base.WritePrimitivePropertiesIntoBuffer(ref writer, options, includeUniqueID);
            TabbedText.WriteLine($"Position {writer.ToLocationString()}, MyLevel2Int value {_MyLevel2Int}");
            TabbedText.Tabs++;
            CompressedIntegralTypes.WriteCompressedInt(ref writer, _MyLevel2Int);
            TabbedText.Tabs--;
        }
        protected override void WriteChildrenPropertiesIntoBuffer(ref BufferWriter writer, LazinatorSerializationOptions options, bool includeUniqueID, int startOfObjectPosition)
        {
            base.WriteChildrenPropertiesIntoBuffer(ref writer, options, includeUniqueID, startOfObjectPosition);
            if (options.SplittingPossible)
            {
                options = options.WithoutSplittingPossible();
            }
            int startOfChildPosition = 0;
            TabbedText.WriteLine($"Position {writer.ToLocationString()}, MyLevel2ListNestedNonLazinatorType (accessed? {_MyLevel2ListNestedNonLazinatorType_Accessed})");
            TabbedText.Tabs++;
            startOfChildPosition = writer.ActiveMemoryPosition;
            if ((options.IncludeChildrenMode != IncludeChildrenMode.IncludeAllChildren || options.IncludeChildrenMode != OriginalIncludeChildrenMode) && !_MyLevel2ListNestedNonLazinatorType_Accessed)
            {
                var deserialized = MyLevel2ListNestedNonLazinatorType;
            }
            WriteNonLazinatorObject(
            nonLazinatorObject: _MyLevel2ListNestedNonLazinatorType, isBelievedDirty: _MyLevel2ListNestedNonLazinatorType_Accessed || (options.IncludeChildrenMode != OriginalIncludeChildrenMode),
            isAccessed: _MyLevel2ListNestedNonLazinatorType_Accessed, writer: ref writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorMemoryStorage, _MyLevel2ListNestedNonLazinatorType_ByteIndex, _MyLevel2ListNestedNonLazinatorType_ByteLength, null),
            verifyCleanness: false,
            binaryWriterAction: (ref BufferWriter w, bool v) =>
            ConvertToBytes_List_GList_GNonLazinatorClass_g_g(ref w, _MyLevel2ListNestedNonLazinatorType,
            options));
            if (options.UpdateStoredBuffer)
            {
                _MyLevel2ListNestedNonLazinatorType_ByteIndex = startOfChildPosition - startOfObjectPosition;
                
            }
            TabbedText.Tabs--;
            if (options.UpdateStoredBuffer)
            {
                _Derived_DotNetList_Nested_NonLazinator_EndByteIndex = writer.ActiveMemoryPosition - startOfObjectPosition;
            }
            
        }
        /* Conversion of supported collections and tuples */
        
        private static List<List<NonLazinatorClass>> ConvertFromBytes_List_GList_GNonLazinatorClass_g_g(LazinatorMemory storage)
        {
            if (storage.Length == 0)
            {
                return default(List<List<NonLazinatorClass>>);
            }
            ReadOnlySpan<byte> span = storage.InitialReadOnlyMemory.Span;
            int bytesSoFar = 0;
            int collectionLength = span.ToDecompressedInt32(ref bytesSoFar);
            
            List<List<NonLazinatorClass>> collection = new List<List<NonLazinatorClass>>(collectionLength);
            for (int itemIndex = 0; itemIndex < collectionLength; itemIndex++)
            {
                int lengthCollectionMember = span.ToInt32(ref bytesSoFar);
                if (lengthCollectionMember == 0)
                {
                    collection.Add(default(List<NonLazinatorClass>));
                }
                else
                {
                    LazinatorMemory childData = storage.Slice(bytesSoFar, lengthCollectionMember);
                    var item = ConvertFromBytes_List_GNonLazinatorClass_g(childData);
                    collection.Add(item);
                }bytesSoFar += lengthCollectionMember;
            }
            
            return collection;
        }
        
        private static void ConvertToBytes_List_GList_GNonLazinatorClass_g_g(ref BufferWriter writer, List<List<NonLazinatorClass>> itemToConvert, LazinatorSerializationOptions options)
        {
            if (itemToConvert == default(List<List<NonLazinatorClass>>))
            {
                return;
            }
            CompressedIntegralTypes.WriteCompressedInt(ref writer, itemToConvert.Count);
            int itemToConvertCount = itemToConvert.Count;
            for (int itemIndex = 0; itemIndex < itemToConvertCount; itemIndex++)
            {
                if (itemToConvert[itemIndex] == default(List<NonLazinatorClass>))
                {
                    writer.Write((int)0);
                }
                else 
                {
                    
                    void action(ref BufferWriter w) => ConvertToBytes_List_GNonLazinatorClass_g(ref w, itemToConvert[itemIndex], options);
                    WriteToBinaryWithInt32LengthPrefix(ref writer, action);
                }
                
            }
        }
        
        private static List<List<NonLazinatorClass>> CloneOrChange_List_GList_GNonLazinatorClass_g_g(List<List<NonLazinatorClass>> itemToClone, Func<ILazinator, ILazinator> cloneOrChangeFunc, bool avoidCloningIfPossible)
        {
            if (itemToClone == null)
            {
                return default;
            }
            int collectionLength = itemToClone.Count;
            List<List<NonLazinatorClass>> collection = new List<List<NonLazinatorClass>>(collectionLength);
            int itemToCloneCount = itemToClone.Count;
            for (int itemIndex = 0; itemIndex < itemToCloneCount; itemIndex++)
            {
                if (itemToClone[itemIndex] == null)
                {
                    collection.Add(default(List<NonLazinatorClass>));
                }
                else
                {
                    var itemCopied = (List<NonLazinatorClass>) CloneOrChange_List_GNonLazinatorClass_g(itemToClone[itemIndex], cloneOrChangeFunc, avoidCloningIfPossible);
                    collection.Add(itemCopied);
                }
            }
            return collection;
        }
        
        private static List<NonLazinatorClass> ConvertFromBytes_List_GNonLazinatorClass_g(LazinatorMemory storage)
        {
            if (storage.Length == 0)
            {
                return default(List<NonLazinatorClass>);
            }
            ReadOnlySpan<byte> span = storage.InitialReadOnlyMemory.Span;
            int bytesSoFar = 0;
            int collectionLength = span.ToDecompressedInt32(ref bytesSoFar);
            
            List<NonLazinatorClass> collection = new List<NonLazinatorClass>(collectionLength);
            for (int itemIndex = 0; itemIndex < collectionLength; itemIndex++)
            {
                int lengthCollectionMember = span.ToInt32(ref bytesSoFar);
                if (lengthCollectionMember == 0)
                {
                    collection.Add(default(NonLazinatorClass));
                }
                else
                {
                    LazinatorMemory childData = storage.Slice(bytesSoFar, lengthCollectionMember);
                    var item = NonLazinatorDirectConverter.ConvertFromBytes_NonLazinatorClass(childData);
                    collection.Add(item);
                }bytesSoFar += lengthCollectionMember;
            }
            
            return collection;
        }
        
        private static void ConvertToBytes_List_GNonLazinatorClass_g(ref BufferWriter writer, List<NonLazinatorClass> itemToConvert, LazinatorSerializationOptions options)
        {
            if (itemToConvert == default(List<NonLazinatorClass>))
            {
                return;
            }
            CompressedIntegralTypes.WriteCompressedInt(ref writer, itemToConvert.Count);
            int itemToConvertCount = itemToConvert.Count;
            for (int itemIndex = 0; itemIndex < itemToConvertCount; itemIndex++)
            {
                if (itemToConvert[itemIndex] == default(NonLazinatorClass))
                {
                    writer.Write((int)0);
                }
                else 
                {
                    
                    void action(ref BufferWriter w) => NonLazinatorDirectConverter.ConvertToBytes_NonLazinatorClass(ref w, itemToConvert[itemIndex], options);
                    WriteToBinaryWithInt32LengthPrefix(ref writer, action);
                }
                
            }
        }
        
        private static List<NonLazinatorClass> CloneOrChange_List_GNonLazinatorClass_g(List<NonLazinatorClass> itemToClone, Func<ILazinator, ILazinator> cloneOrChangeFunc, bool avoidCloningIfPossible)
        {
            if (itemToClone == null)
            {
                return default;
            }
            int collectionLength = itemToClone.Count;
            List<NonLazinatorClass> collection = new List<NonLazinatorClass>(collectionLength);
            int itemToCloneCount = itemToClone.Count;
            for (int itemIndex = 0; itemIndex < itemToCloneCount; itemIndex++)
            {
                if (itemToClone[itemIndex] == null)
                {
                    collection.Add(default(NonLazinatorClass));
                }
                else
                {
                    var itemCopied = (NonLazinatorClass) itemToClone[itemIndex];
                    collection.Add(itemCopied);
                }
            }
            return collection;
        }
        
    }
}
