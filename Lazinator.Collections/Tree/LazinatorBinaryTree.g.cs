//b3ed247d-e1e4-a5c8-c674-b47aea1426a1
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.395
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

#nullable disable
namespace Lazinator.Collections.Tree
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
    using System.Threading.Tasks;
    using static Lazinator.Core.LazinatorUtilities;
    
    [Autogenerated]
    public partial class LazinatorBinaryTree<T> : ILazinator, ILazinatorAsync
    {
        public bool IsStruct => false;
        
        /* Property definitions */
        
        protected int _Root_ByteIndex;
        private int _LazinatorBinaryTree_T_EndByteIndex;
        protected virtual  int _Root_ByteLength => _LazinatorBinaryTree_T_EndByteIndex - _Root_ByteIndex;
        protected virtual int _OverallEndByteIndex => _LazinatorBinaryTree_T_EndByteIndex;
        
        
        protected LazinatorBinaryTreeNode<T> _Root;
        public virtual LazinatorBinaryTreeNode<T> Root
        {
            get
            {
                if (!_Root_Accessed)
                {
                    LazinateRoot();
                } 
                return _Root;
            }
            set
            {
                if (_Root != null)
                {
                    _Root.LazinatorParents = _Root.LazinatorParents.WithRemoved(this);
                }
                if (value != null)
                {
                    value.LazinatorParents = value.LazinatorParents.WithAdded(this);
                }
                
                IsDirty = true;
                DescendantIsDirty = true;
                _Root = value;
                _Root_Accessed = true;
            }
        }
        protected bool _Root_Accessed;
        private void LazinateRoot()
        {
            if (LazinatorMemoryStorage.Length == 0)
            {
                _Root = null;
            }
            else
            {
                LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _Root_ByteIndex, _Root_ByteLength, null);
                childData.LoadInitialReadOnlyMemory();
                _Root = DeserializationFactory.Instance.CreateBaseOrDerivedType(257, (c, p) => new LazinatorBinaryTreeNode<T>(c, p), childData, this); 
                childData.ConsiderUnloadInitialReadOnlyMemory();
            }
            _Root_Accessed = true;
        }
        private async Task LazinateRootAsync()
        {
            if (LazinatorMemoryStorage.Length == 0)
            {
                _Root = null;
            }
            else
            {
                LazinatorMemory childData = await GetChildSliceAsync(LazinatorMemoryStorage, _Root_ByteIndex, _Root_ByteLength, null);
                _Root = DeserializationFactory.Instance.CreateBaseOrDerivedType(257, (c, p) => new LazinatorBinaryTreeNode<T>(c, p), childData, this); 
                await childData.ConsiderUnloadReadOnlyMemoryAsync();
            }
            _Root_Accessed = true;
        }
        public async ValueTask<LazinatorBinaryTreeNode<T>> GetRootAsync()
        {
            if (!_Root_Accessed)
            {
                await LazinateRootAsync();
            } 
            return _Root;
        }
        
        
        /* Serialization, deserialization, and object relationships */
        
        public LazinatorBinaryTree(IncludeChildrenMode originalIncludeChildrenMode = IncludeChildrenMode.IncludeAllChildren)
        {
            OriginalIncludeChildrenMode = originalIncludeChildrenMode;
        }
        
        public LazinatorBinaryTree(LazinatorMemory serializedBytes, ILazinator parent = null, IncludeChildrenMode originalIncludeChildrenMode = IncludeChildrenMode.IncludeAllChildren, int? lazinatorObjectVersion = null)
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
            return _OverallEndByteIndex;
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
        async public virtual ValueTask SerializeLazinatorAsync()
        {
            if (!IsDirty && !DescendantIsDirty && LazinatorMemoryStorage.Length > 0 && OriginalIncludeChildrenMode == IncludeChildrenMode.IncludeAllChildren)
            {
                return;
                
            }
            var previousBuffer = LazinatorMemoryStorage;
            if (LazinatorMemoryStorage.IsEmpty || IncludeChildrenMode.IncludeAllChildren != OriginalIncludeChildrenMode || (IsDirty || DescendantIsDirty))
            {
                LazinatorMemoryStorage = await EncodeToNewBufferAsync(LazinatorSerializationOptions.Default);
            }
            else
            {
                BufferWriterContainer writer = new BufferWriterContainer(LazinatorMemoryStorage.LengthInt ?? 0);
                await LazinatorMemoryStorage.WriteToBufferAsync(writer);
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
            BufferWriter writer = options.SerializeDiffs ? new BufferWriter(0, LazinatorMemoryStorage) : new BufferWriter(LazinatorMemoryStorage.LengthInt ?? 0);
            LazinatorMemoryStorage.WriteToBuffer(ref writer);
            return writer.LazinatorMemory;
        }
        async public virtual ValueTask<LazinatorMemory> SerializeLazinatorAsync(LazinatorSerializationOptions options) 
        {
            if (LazinatorMemoryStorage.IsEmpty || options.IncludeChildrenMode != OriginalIncludeChildrenMode || (options.SerializeDiffs || options.VerifyCleanness || IsDirty || (options.IncludeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && DescendantIsDirty)))
            {
                return await EncodeToNewBufferAsync(options);
            }
            BufferWriterContainer writer = options.SerializeDiffs ? new BufferWriterContainer(0, LazinatorMemoryStorage) : new BufferWriterContainer(LazinatorMemoryStorage.LengthInt ?? 0);
            await LazinatorMemoryStorage.WriteToBufferAsync(writer);
            return writer.LazinatorMemory;
        }
        
        
        protected virtual LazinatorMemory EncodeToNewBuffer(in LazinatorSerializationOptions options) 
        {
            int bufferSize = LazinatorMemoryStorage.Length == 0 ? ExpandableBytes.DefaultMinBufferSize : LazinatorMemoryStorage.LengthInt ?? ExpandableBytes.DefaultMinBufferSize;
            BufferWriter writer = options.SerializeDiffs ? new BufferWriter(0, LazinatorMemoryStorage) : new BufferWriter(bufferSize);
            SerializeToExistingBuffer(ref writer, options);
            return writer.LazinatorMemory;
        }
        async protected virtual ValueTask<LazinatorMemory> EncodeToNewBufferAsync(LazinatorSerializationOptions options) 
        {
            int bufferSize = LazinatorMemoryStorage.Length == 0 ? ExpandableBytes.DefaultMinBufferSize : LazinatorMemoryStorage.LengthInt ?? ExpandableBytes.DefaultMinBufferSize;
            BufferWriterContainer writer = options.SerializeDiffs ? new BufferWriterContainer(0, LazinatorMemoryStorage) : new BufferWriterContainer(bufferSize);
            await SerializeToExistingBufferAsync(writer, options);
            return writer.LazinatorMemory;
        }
        
        
        public virtual ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode = IncludeChildrenMode.IncludeAllChildren, CloneBufferOptions cloneBufferOptions = CloneBufferOptions.IndependentBuffers)
        {
            LazinatorBinaryTree<T> clone;
            if (cloneBufferOptions == CloneBufferOptions.NoBuffer)
            {
                clone = new LazinatorBinaryTree<T>(includeChildrenMode);
                clone.LazinatorObjectVersion = LazinatorObjectVersion;
                clone = (LazinatorBinaryTree<T>)AssignCloneProperties(clone, includeChildrenMode);
            }
            else
            {
                LazinatorMemory bytes = EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, IsDirty, DescendantIsDirty, false, LazinatorMemoryStorage, this);
                clone = new LazinatorBinaryTree<T>(bytes);
            }
            return clone;
        }
        async public virtual ValueTask<ILazinator> CloneLazinatorAsync(IncludeChildrenMode includeChildrenMode = IncludeChildrenMode.IncludeAllChildren, CloneBufferOptions cloneBufferOptions = CloneBufferOptions.IndependentBuffers)
        {
            LazinatorBinaryTree<T> clone;
            if (cloneBufferOptions == CloneBufferOptions.NoBuffer)
            {
                clone = new LazinatorBinaryTree<T>(includeChildrenMode);
                clone.LazinatorObjectVersion = LazinatorObjectVersion;
                clone = (LazinatorBinaryTree<T>)await AssignClonePropertiesAsync(clone, includeChildrenMode);
            }
            else
            {
                LazinatorMemory bytes = await EncodeOrRecycleToNewBufferAsync(includeChildrenMode, OriginalIncludeChildrenMode, IsDirty, DescendantIsDirty, false, LazinatorMemoryStorage, this);
                clone = new LazinatorBinaryTree<T>(bytes);
            }
            return (ILazinator)clone;
        }
        
        
        protected virtual ILazinator AssignCloneProperties(ILazinator clone, IncludeChildrenMode includeChildrenMode)
        {
            clone.FreeInMemoryObjects();
            LazinatorBinaryTree<T> typedClone = (LazinatorBinaryTree<T>) clone;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                if (Root == null)
                {
                    typedClone.Root = null;
                }
                else
                {
                    typedClone.Root = (LazinatorBinaryTreeNode<T>) Root.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer);
                }
            }
            
            return typedClone;
        }
        async protected virtual ValueTask<ILazinator> AssignClonePropertiesAsync(ILazinator clone, IncludeChildrenMode includeChildrenMode)
        {
            clone.FreeInMemoryObjects();
            LazinatorBinaryTree<T> typedClone = (LazinatorBinaryTree<T>) clone;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                if ((await GetRootAsync()) == null)
                {
                    typedClone.Root = null;
                }
                else
                {
                    typedClone.Root = (LazinatorBinaryTreeNode<T>) (await GetRootAsync()).CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer);
                }
            }
            
            return (ILazinator)typedClone;
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
        async public IAsyncEnumerable<ILazinator> EnumerateLazinatorNodesAsync(Func<ILazinator, bool> matchCriterion, bool stopExploringBelowMatch, Func<ILazinator, bool> exploreCriterion, bool exploreOnlyDeserializedChildren, bool enumerateNulls)
        {
            bool match = (matchCriterion == null) ? true : matchCriterion(this);
            bool explore = (!match || !stopExploringBelowMatch) && ((exploreCriterion == null) ? true : exploreCriterion(this));
            if (match)
            {
                yield return this;
            }
            if (explore)
            {
                await foreach (var item in EnumerateLazinatorDescendantsAsync(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
                {
                    yield return item.descendant;
                }
            }
        }
        
        
        public virtual IEnumerable<(string propertyName, ILazinator descendant)> EnumerateLazinatorDescendants(Func<ILazinator, bool> matchCriterion, bool stopExploringBelowMatch, Func<ILazinator, bool> exploreCriterion, bool exploreOnlyDeserializedChildren, bool enumerateNulls)
        {
            if (enumerateNulls && (!exploreOnlyDeserializedChildren || _Root_Accessed) && Root == null)
            {
                yield return ("Root", default);
            }
            else
            {
                if ((!exploreOnlyDeserializedChildren && Root != null) || (_Root_Accessed && _Root != null))
                {
                    bool isMatch_Root = matchCriterion == null || matchCriterion(Root);
                    bool shouldExplore_Root = exploreCriterion == null || exploreCriterion(Root);
                    if (isMatch_Root)
                    {
                        yield return ("Root", Root);
                    }
                    if ((!stopExploringBelowMatch || !isMatch_Root) && shouldExplore_Root)
                    {
                        foreach (var toYield in Root.EnumerateLazinatorDescendants(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
                        {
                            yield return ("Root" + "." + toYield.propertyName, toYield.descendant);
                        }
                    }
                }
            }
            yield break;
        }
        async public virtual IAsyncEnumerable<(string propertyName, ILazinator descendant)> EnumerateLazinatorDescendantsAsync(Func<ILazinator, bool> matchCriterion, bool stopExploringBelowMatch, Func<ILazinator, bool> exploreCriterion, bool exploreOnlyDeserializedChildren, bool enumerateNulls)
        {
            if (enumerateNulls && (!exploreOnlyDeserializedChildren || _Root_Accessed) && (await GetRootAsync()) == null)
            {
                yield return ("Root", default);
            }
            else
            {
                if ((!exploreOnlyDeserializedChildren && Root != null) || (_Root_Accessed && _Root != null))
                {
                    bool isMatch_Root = matchCriterion == null || matchCriterion((await GetRootAsync()));
                    bool shouldExplore_Root = exploreCriterion == null || exploreCriterion((await GetRootAsync()));
                    if (isMatch_Root)
                    {
                        yield return ("Root", (await GetRootAsync()));
                    }
                    if ((!stopExploringBelowMatch || !isMatch_Root) && shouldExplore_Root)
                    {
                        foreach (var toYield in (await GetRootAsync()).EnumerateLazinatorDescendants(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
                        {
                            yield return ("Root" + "." + toYield.propertyName, toYield.descendant);
                        }
                    }
                }
            }
            yield break;
        }
        
        #pragma warning disable CS1998
        
        public virtual IEnumerable<(string propertyName, object descendant)> EnumerateNonLazinatorProperties()
        {
            yield break;
        }
        async public virtual IAsyncEnumerable<(string propertyName, object descendant)> EnumerateNonLazinatorPropertiesAsync()
        {
            yield break;
        }
        
        #pragma warning restore CS1998
        
        public virtual ILazinator ForEachLazinator(Func<ILazinator, ILazinator> changeFunc, bool exploreOnlyDeserializedChildren, bool changeThisLevel)
        {
            if ((!exploreOnlyDeserializedChildren && Root != null) || (_Root_Accessed && _Root != null))
            {
                _Root = (LazinatorBinaryTreeNode<T>) _Root.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, true);
            }
            if (changeThisLevel && changeFunc != null)
            {
                return changeFunc(this);
            }
            return this;
        }
        async public virtual ValueTask<ILazinator> ForEachLazinatorAsync(Func<ILazinator, ILazinator> changeFunc, bool exploreOnlyDeserializedChildren, bool changeThisLevel)
        {
            if ((!exploreOnlyDeserializedChildren && (await GetRootAsync()) != null) || (_Root_Accessed && _Root != null))
            {
                _Root = (LazinatorBinaryTreeNode<T>) _Root.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, true);
            }
            if (changeThisLevel && changeFunc != null)
            {
                return (ILazinator)changeFunc(this);
            }
            return (ILazinator)this;
        }
        
        
        public virtual void FreeInMemoryObjects()
        {
            _Root = default;
            _Root_Accessed = false;
            IsDirty = false;
            DescendantIsDirty = false;
            HasChanged = false;
            DescendantHasChanged = false;
        }
        
        /* Conversion */
        
        public virtual int LazinatorUniqueID => 256;
        
        protected virtual bool ContainsOpenGenericParameters => true;
        public virtual LazinatorGenericIDType LazinatorGenericID => LazinatorGenericIDType.GetCachedForType<LazinatorBinaryTree<T>>(() => DeserializationFactory.Instance.GetUniqueIDListForGenericType(256, new Type[] { typeof(T) }));
        
        
        public virtual int LazinatorObjectVersion { get; set; } = 0;
        
        
        protected virtual int ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            TabbedText.WriteLine($"");
            TabbedText.WriteLine($"Converting Lazinator.Collections.Tree.LazinatorBinaryTree<T> from bytes at: " + LazinatorMemoryStorage.ToLocationString());
            ReadOnlySpan<byte> span = LazinatorMemoryStorage.InitialReadOnlyMemory.Span;
            ConvertFromBytesForPrimitiveProperties(span, includeChildrenMode, serializedVersionNumber, ref bytesSoFar);
            TabbedText.Tabs++;
            int lengthForLengths = 0;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                lengthForLengths += 4;
            }
            int totalChildrenSize = ConvertFromBytesForChildProperties(span, includeChildrenMode, serializedVersionNumber, bytesSoFar + lengthForLengths, ref bytesSoFar);;
            TabbedText.Tabs--;
            return bytesSoFar + totalChildrenSize;
        }
        
        protected virtual void ConvertFromBytesForPrimitiveProperties(ReadOnlySpan<byte> span, IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
        }
        
        protected virtual int ConvertFromBytesForChildProperties(ReadOnlySpan<byte> span, IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, int indexOfFirstChild, ref int bytesSoFar)
        {
            int totalChildrenBytes = 0;
            TabbedText.WriteLine($"Root: Length is at {bytesSoFar}; start location is {indexOfFirstChild + totalChildrenBytes}"); 
            _Root_ByteIndex = indexOfFirstChild + totalChildrenBytes;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                totalChildrenBytes += span.ToInt32(ref bytesSoFar);
            }
            _LazinatorBinaryTree_T_EndByteIndex = indexOfFirstChild + totalChildrenBytes;
            return totalChildrenBytes;
        }
        
        public virtual void SerializeToExistingBuffer(ref BufferWriter writer, in LazinatorSerializationOptions options)
        {
            TabbedText.WriteLine("");
            TabbedText.WriteLine($"Initiating serialization of Lazinator.Collections.Tree.LazinatorBinaryTree<T> at position {writer.ToLocationString()}");
            long startPosition = writer.OverallMemoryPosition;
            WritePropertiesIntoBuffer(ref writer, options, true);
            if (options.UpdateStoredBuffer)
            {
                UpdateStoredBuffer(ref writer, startPosition, writer.OverallMemoryPosition - startPosition, options.IncludeChildrenMode, false);
            }
        }
        async public virtual ValueTask SerializeToExistingBufferAsync(BufferWriterContainer writer, LazinatorSerializationOptions options)
        {
            TabbedText.WriteLine("");
            TabbedText.WriteLine($"Initiating serialization of Lazinator.Collections.Tree.LazinatorBinaryTree<T> at position {writer.ToLocationString()}");
            long startPosition = writer.OverallMemoryPosition;
            await WritePropertiesIntoBufferAsync(writer, options, true);
            if (options.UpdateStoredBuffer)
            {
                UpdateStoredBuffer(ref writer.Writer, startPosition, writer.OverallMemoryPosition - startPosition, options.IncludeChildrenMode, false);
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
            if (_Root_Accessed && _Root != null)
            {
                Root.UpdateStoredBuffer(ref writer, startPosition + _Root_ByteIndex, _Root_ByteLength, IncludeChildrenMode.IncludeAllChildren, true);
            }
            
        }
        
        
        
        protected virtual void WritePropertiesIntoBuffer(ref BufferWriter writer, in LazinatorSerializationOptions options, bool includeUniqueID)
        {
            long startPosition = writer.OverallMemoryPosition;
            TabbedText.WriteLine($"Writing properties for Lazinator.Collections.Tree.LazinatorBinaryTree<T>.");
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
            
            
            int lengthForLengths = 0;
            if (options.IncludeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && options.IncludeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                lengthForLengths += 4;
            }
            
            var previousLengthsPosition = writer.SetLengthsPosition(lengthForLengths);
            TabbedText.WriteLine($"Byte {writer.ActiveMemoryPosition}, After skipping {lengthForLengths} bytes to store lengths of child objects");
            WriteChildrenPropertiesIntoBuffer(ref writer, options, includeUniqueID, startPosition);
            writer.ResetLengthsPosition(previousLengthsPosition);
            TabbedText.WriteLine($"Byte {writer.ActiveMemoryPosition} (end of LazinatorBinaryTree<T>) ");
            
        }
        async protected virtual ValueTask WritePropertiesIntoBufferAsync(BufferWriterContainer writer, LazinatorSerializationOptions options, bool includeUniqueID)
        {
            long startPosition = writer.OverallMemoryPosition;
            TabbedText.WriteLine($"Writing properties for Lazinator.Collections.Tree.LazinatorBinaryTree<T>.");
            TabbedText.WriteLine($"Properties uniqueID {(LazinatorGenericID.IsEmpty ? LazinatorUniqueID.ToString() : String.Join(",",LazinatorGenericID.TypeAndInnerTypeIDs.ToArray()))} {(includeUniqueID ? "Included" : "Omitted")}, Lazinator version {Lazinator.Support.LazinatorVersionInfo.LazinatorIntVersion} Included, Object version {LazinatorObjectVersion} Included, IncludeChildrenMode {options.IncludeChildrenMode} Included");
            TabbedText.WriteLine($"IsDirty {IsDirty} DescendantIsDirty {DescendantIsDirty} HasParentClass {LazinatorParents.Any()}");
            if (includeUniqueID)
            {
                if (!ContainsOpenGenericParameters)
                {
                    CompressedIntegralTypes.WriteCompressedInt(ref writer.Writer, LazinatorUniqueID);
                }
                else
                {
                    WriteLazinatorGenericID(ref writer.Writer, LazinatorGenericID);
                }
            }
            CompressedIntegralTypes.WriteCompressedInt(ref writer.Writer, Lazinator.Support.LazinatorVersionInfo.LazinatorIntVersion);
            CompressedIntegralTypes.WriteCompressedInt(ref writer.Writer, LazinatorObjectVersion);
            writer.Write((byte)options.IncludeChildrenMode);
            // write properties
            
            
            int lengthForLengths = 0;
            if (options.IncludeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && options.IncludeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                lengthForLengths += 4;
            }
            
            var previousLengthsPosition = writer.SetLengthsPosition(lengthForLengths);
            TabbedText.WriteLine($"Byte {writer.ActiveMemoryPosition}, After skipping {lengthForLengths} bytes to store lengths of child objects");
            await WriteChildrenPropertiesIntoBufferAsync(writer, options, includeUniqueID, startPosition);
            writer.ResetLengthsPosition(previousLengthsPosition);
            TabbedText.WriteLine($"Byte {writer.ActiveMemoryPosition} (end of LazinatorBinaryTree<T>) ");
            
        }
        
        
        protected virtual void WritePrimitivePropertiesIntoBuffer(ref BufferWriter writer, /*<$$ if=async,0 $$>*/in /*<$$/if $$>*//*<$$ if=async,1 $$>*//*<$$/if $$>*/LazinatorSerializationOptions options, bool includeUniqueID)
        {
        }
        protected virtual void WriteChildrenPropertiesIntoBuffer(ref BufferWriter writer, LazinatorSerializationOptions options, bool includeUniqueID, long startOfObjectPosition)
        {
            long startOfChildPosition = 0;
            long lengthValue = 0;
            TabbedText.WriteLine($"Byte {writer.ActiveMemoryPosition}, Root (accessed? {_Root_Accessed}) (backing var null? {_Root == null}) ");
            TabbedText.Tabs++;
            startOfChildPosition = writer.OverallMemoryPosition;
            if (options.IncludeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && options.IncludeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                if ((options.IncludeChildrenMode != IncludeChildrenMode.IncludeAllChildren || options.IncludeChildrenMode != OriginalIncludeChildrenMode) && !_Root_Accessed)
                {
                    var deserialized = Root;
                }
                WriteChild(ref writer, ref _Root, options, _Root_Accessed, () => GetChildSlice(LazinatorMemoryStorage, _Root_ByteIndex, _Root_ByteLength, null), this);
                lengthValue = writer.OverallMemoryPosition - startOfChildPosition;
                if (lengthValue > int.MaxValue)
                {
                    ThrowHelper.ThrowTooLargeException(int.MaxValue);
                }
                writer.RecordLength((int) lengthValue);
            }
            if (options.UpdateStoredBuffer)
            {
                _Root_ByteIndex = (int) (startOfChildPosition - startOfObjectPosition);
                
            }
            TabbedText.Tabs--;
            if (options.UpdateStoredBuffer)
            {
                _LazinatorBinaryTree_T_EndByteIndex = (int) (writer.ActiveMemoryPosition - startOfObjectPosition);
            }
            
        }
        async protected virtual ValueTask WriteChildrenPropertiesIntoBufferAsync(BufferWriterContainer writer, LazinatorSerializationOptions options, bool includeUniqueID, long startOfObjectPosition)
        {
            long startOfChildPosition = 0;
            long lengthValue = 0;
            TabbedText.WriteLine($"Byte {writer.ActiveMemoryPosition}, Root (accessed? {_Root_Accessed}) (backing var null? {_Root == null}) ");
            TabbedText.Tabs++;
            startOfChildPosition = writer.Writer.OverallMemoryPosition;
            if (options.IncludeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && options.IncludeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                if ((options.IncludeChildrenMode != IncludeChildrenMode.IncludeAllChildren || options.IncludeChildrenMode != OriginalIncludeChildrenMode) && !_Root_Accessed)
                {
                    var deserialized = (await GetRootAsync());
                }
                await WriteNonAsyncChildAsync(writer, _Root, options, _Root_Accessed, async () => await GetChildSliceAsync(LazinatorMemoryStorage, _Root_ByteIndex, _Root_ByteLength, null), this);
                lengthValue = writer.OverallMemoryPosition - startOfChildPosition;
                if (lengthValue > int.MaxValue)
                {
                    ThrowHelper.ThrowTooLargeException(int.MaxValue);
                }
                writer.RecordLength((int) lengthValue);
            }
            if (options.UpdateStoredBuffer)
            {
                _Root_ByteIndex = (int) (startOfChildPosition - startOfObjectPosition);
                
            }
            TabbedText.Tabs--;
            if (options.UpdateStoredBuffer)
            {
                _LazinatorBinaryTree_T_EndByteIndex = (int) (writer.ActiveMemoryPosition - startOfObjectPosition);
            }
            
        }
        
    }
}
