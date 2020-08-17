/*Location4931*//*Location4916*///5d05017e-abcf-7523-a888-acebd68070be
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.380
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

#nullable disable
namespace LazinatorTests.Examples.NonAbstractGenerics
{/*Location4917*/
    using Lazinator.Attributes;/*Location4918*/
    using Lazinator.Buffers;/*Location4919*/
    using Lazinator.Core;/*Location4920*/
    using Lazinator.Exceptions;/*Location4921*/
    using Lazinator.Support;/*Location4922*/
    using LazinatorTests.Examples;/*Location4923*/
    using System;/*Location4924*/
    using System.Buffers;/*Location4925*/
    using System.Collections.Generic;/*Location4926*/
    using System.Diagnostics;/*Location4927*/
    using System.IO;/*Location4928*/
    using System.Linq;/*Location4929*/
    using System.Runtime.InteropServices;/*Location4930*/
    using static Lazinator.Core.LazinatorUtilities;
    
    [Autogenerated]
    public partial class ClosedGeneric : OpenGeneric<ExampleChild>, ILazinator
    {
        /*Location4932*//* Property definitions */
        
        
        /*Location4933*/
        protected int _AnotherPropertyAdded;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public int AnotherPropertyAdded
        {
            get
            {
                return _AnotherPropertyAdded;
            }
            set
            {
                IsDirty = true;
                _AnotherPropertyAdded = value;
            }
        }
        /*Location4935*/        /* Clone overrides */
        
        public ClosedGeneric(IncludeChildrenMode originalIncludeChildrenMode) : base(originalIncludeChildrenMode)
        {
        }
        
        public ClosedGeneric(LazinatorMemory serializedBytes, ILazinator parent = null) : base(serializedBytes, parent)
        {
        }
        
        public override ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode = IncludeChildrenMode.IncludeAllChildren, CloneBufferOptions cloneBufferOptions = CloneBufferOptions.IndependentBuffers)
        {
            ClosedGeneric clone;
            if (cloneBufferOptions == CloneBufferOptions.NoBuffer)
            {
                clone = new ClosedGeneric(includeChildrenMode);
                clone.LazinatorObjectVersion = LazinatorObjectVersion;
                clone = (ClosedGeneric)AssignCloneProperties(clone, includeChildrenMode);
            }
            else
            {
                LazinatorMemory bytes = EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, false, IsDirty, DescendantIsDirty, false, LazinatorMemoryStorage, false, this);
                clone = new ClosedGeneric(bytes);
            }
            return clone;
        }
        
        public override ILazinator AssignCloneProperties(ILazinator clone, IncludeChildrenMode includeChildrenMode)
        {
            base.AssignCloneProperties(clone, includeChildrenMode);
            ClosedGeneric typedClone = (ClosedGeneric) clone;
            /*Location4934*/typedClone.AnotherPropertyAdded = AnotherPropertyAdded;
            
            return typedClone;
        }
        
        /* Properties */
        /*Location4936*/
        public override IEnumerable<(string propertyName, ILazinator descendant)> EnumerateLazinatorDescendants(Func<ILazinator, bool> matchCriterion, bool stopExploringBelowMatch, Func<ILazinator, bool> exploreCriterion, bool exploreOnlyDeserializedChildren, bool enumerateNulls)
        {
            foreach (var inheritedYield in base.EnumerateLazinatorDescendants(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
            {
                yield return inheritedYield;
            }
            /*Location4937*/yield break;
        }
        /*Location4938*/
        
        public override IEnumerable<(string propertyName, object descendant)> EnumerateNonLazinatorProperties()
        {
            foreach (var inheritedYield in base.EnumerateNonLazinatorProperties())
            {
                yield return inheritedYield;
            }
            /*Location4939*/yield return ("AnotherPropertyAdded", (object)AnotherPropertyAdded);
            /*Location4940*/yield break;
        }
        /*Location4941*/
        public override ILazinator ForEachLazinator(Func<ILazinator, ILazinator> changeFunc, bool exploreOnlyDeserializedChildren, bool changeThisLevel)
        {
            base.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, false);
            /*Location4942*/if (changeThisLevel && changeFunc != null)
            {
                return changeFunc(this);
            }
            return this;
        }
        /*Location4943*/
        public override void FreeInMemoryObjects()
        {
            base.FreeInMemoryObjects();
            
            IsDirty = false;
            DescendantIsDirty = false;
            HasChanged = false;
            DescendantHasChanged = false;
        }
        /*Location4944*/
        /* Conversion */
        
        public override int LazinatorUniqueID => 1050;
        
        protected override bool ContainsOpenGenericParameters => false;
        public override LazinatorGenericIDType LazinatorGenericID => default;
        
        
        public override int LazinatorObjectVersion { get; set; } = 0;
        
        
        /*Location4945*/public override void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            base.ConvertFromBytesAfterHeader(OriginalIncludeChildrenMode, serializedVersionNumber, ref bytesSoFar);
            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
            /*Location4946*/_AnotherPropertyAdded = span.ToDecompressedInt(ref bytesSoFar);
            /*Location4947*/        }
            
            /*Location4948*/public override void SerializeExistingBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
            {
                /*Location4949*/if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren)
                {
                    updateStoredBuffer = false;
                }
                /*Location4950*/int startPosition = writer.Position;
                WritePropertiesIntoBuffer(ref writer, includeChildrenMode, verifyCleanness, updateStoredBuffer, true);
                /*Location4951*/if (updateStoredBuffer)
                {
                    UpdateStoredBuffer(ref writer, startPosition, writer.Position - startPosition, includeChildrenMode, false);
                    /*Location4952*/}
                    /*Location4953*/}
                    /*Location4954*/
                    public override void UpdateStoredBuffer(ref BinaryBufferWriter writer, int startPosition, int length, IncludeChildrenMode includeChildrenMode, bool updateDeserializedChildren)
                    {
                        /*Location4955*/_IsDirty = false;
                        if (includeChildrenMode == IncludeChildrenMode.IncludeAllChildren)
                        {
                            _DescendantIsDirty = false;/*Location4956*/
                            if (updateDeserializedChildren)
                            {
                                UpdateDeserializedChildren(ref writer, startPosition);
                                /*Location4957*/}
                                /*Location4958*//*Location4959*/
                            }
                            else
                            {
                                ThrowHelper.ThrowCannotUpdateStoredBuffer();
                            }
                            /*Location4960*/
                            var newBuffer = writer.Slice(startPosition, length);
                            LazinatorMemoryStorage = newBuffer;
                            /*Location4961*/}
                            /*Location4962*/
                            protected override void UpdateDeserializedChildren(ref BinaryBufferWriter writer, int startPosition)
                            {
                                base.UpdateDeserializedChildren(ref writer, startPosition);
                                /*Location4963*/}
                                
                                /*Location4964*/
                                protected override void WritePropertiesIntoBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer, bool includeUniqueID)
                                {
                                    base.WritePropertiesIntoBuffer(ref writer, includeChildrenMode, verifyCleanness, updateStoredBuffer, includeUniqueID);
                                    /*Location4965*/// write properties
                                    /*Location4966*/CompressedIntegralTypes.WriteCompressedInt(ref writer, _AnotherPropertyAdded);
                                    /*Location4967*/}
                                    /*Location4968*/
                                }
                            }
