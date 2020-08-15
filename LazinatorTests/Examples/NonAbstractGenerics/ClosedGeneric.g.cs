/*Location4035*//*Location4020*///5d05017e-abcf-7523-a888-acebd68070be
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
{/*Location4021*/
    using Lazinator.Attributes;/*Location4022*/
    using Lazinator.Buffers;/*Location4023*/
    using Lazinator.Core;/*Location4024*/
    using Lazinator.Exceptions;/*Location4025*/
    using Lazinator.Support;/*Location4026*/
    using LazinatorTests.Examples;/*Location4027*/
    using System;/*Location4028*/
    using System.Buffers;/*Location4029*/
    using System.Collections.Generic;/*Location4030*/
    using System.Diagnostics;/*Location4031*/
    using System.IO;/*Location4032*/
    using System.Linq;/*Location4033*/
    using System.Runtime.InteropServices;/*Location4034*/
    using static Lazinator.Core.LazinatorUtilities;
    
    [Autogenerated]
    public partial class ClosedGeneric : OpenGeneric<ExampleChild>, ILazinator
    {
        /*Location4036*//* Property definitions */
        
        
        /*Location4037*/
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
        /*Location4039*/        /* Clone overrides */
        
        public ClosedGeneric(LazinatorConstructorEnum constructorEnum) : base(constructorEnum)
        {
        }
        
        public ClosedGeneric(LazinatorMemory serializedBytes, ILazinator parent = null) : base(serializedBytes, parent)
        {
        }
        
        public override ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode = IncludeChildrenMode.IncludeAllChildren, CloneBufferOptions cloneBufferOptions = CloneBufferOptions.IndependentBuffers)
        {
            var clone = new ClosedGeneric(LazinatorConstructorEnum.LazinatorConstructor)
            {
                OriginalIncludeChildrenMode = includeChildrenMode
            };
            clone = CompleteClone(this, clone, includeChildrenMode, cloneBufferOptions);
            return clone;
        }
        
        public override ILazinator AssignCloneProperties(ILazinator clone, IncludeChildrenMode includeChildrenMode)
        {
            base.AssignCloneProperties(clone, includeChildrenMode);
            ClosedGeneric typedClone = (ClosedGeneric) clone;
            /*Location4038*/typedClone.AnotherPropertyAdded = AnotherPropertyAdded;
            
            return typedClone;
        }
        
        /* Properties */
        /*Location4040*/
        public override IEnumerable<(string propertyName, ILazinator descendant)> EnumerateLazinatorDescendants(Func<ILazinator, bool> matchCriterion, bool stopExploringBelowMatch, Func<ILazinator, bool> exploreCriterion, bool exploreOnlyDeserializedChildren, bool enumerateNulls)
        {
            foreach (var inheritedYield in base.EnumerateLazinatorDescendants(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
            {
                yield return inheritedYield;
            }
            /*Location4041*/yield break;
        }
        /*Location4042*/
        
        public override IEnumerable<(string propertyName, object descendant)> EnumerateNonLazinatorProperties()
        {
            foreach (var inheritedYield in base.EnumerateNonLazinatorProperties())
            {
                yield return inheritedYield;
            }
            /*Location4043*/yield return ("AnotherPropertyAdded", (object)AnotherPropertyAdded);
            /*Location4044*/yield break;
        }
        /*Location4045*/
        public override ILazinator ForEachLazinator(Func<ILazinator, ILazinator> changeFunc, bool exploreOnlyDeserializedChildren, bool changeThisLevel)
        {
            base.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, false);
            /*Location4046*/if (changeThisLevel && changeFunc != null)
            {
                return changeFunc(this);
            }
            return this;
        }
        /*Location4047*/
        public override void FreeInMemoryObjects()
        {
            base.FreeInMemoryObjects();
            
            IsDirty = false;
            DescendantIsDirty = false;
            HasChanged = false;
            DescendantHasChanged = false;
        }
        /*Location4048*/
        /* Conversion */
        
        public override int LazinatorUniqueID => 1050;
        
        protected override bool ContainsOpenGenericParameters => false;
        public override LazinatorGenericIDType LazinatorGenericID => default;
        
        
        public override int LazinatorObjectVersion { get; set; } = 0;
        
        
        /*Location4049*/public override void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            base.ConvertFromBytesAfterHeader(OriginalIncludeChildrenMode, serializedVersionNumber, ref bytesSoFar);
            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
            /*Location4050*/_AnotherPropertyAdded = span.ToDecompressedInt(ref bytesSoFar);
            /*Location4051*/        }
            
            /*Location4052*/public override void SerializeExistingBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
            {
                /*Location4053*/if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren)
                {
                    updateStoredBuffer = false;
                }
                /*Location4054*/int startPosition = writer.Position;
                WritePropertiesIntoBuffer(ref writer, includeChildrenMode, verifyCleanness, updateStoredBuffer, true);
                /*Location4055*/if (updateStoredBuffer)
                {
                    UpdateStoredBuffer(ref writer, startPosition, writer.Position - startPosition, includeChildrenMode, false);
                    /*Location4056*/}
                    /*Location4057*/}
                    /*Location4058*/
                    public override void UpdateStoredBuffer(ref BinaryBufferWriter writer, int startPosition, int length, IncludeChildrenMode includeChildrenMode, bool updateDeserializedChildren)
                    {
                        /*Location4059*/_IsDirty = false;
                        if (includeChildrenMode == IncludeChildrenMode.IncludeAllChildren)
                        {
                            _DescendantIsDirty = false;/*Location4060*/
                            if (updateDeserializedChildren)
                            {
                                UpdateDeserializedChildren(ref writer, startPosition);
                                /*Location4061*/}
                                /*Location4062*//*Location4063*/
                            }
                            else
                            {
                                ThrowHelper.ThrowCannotUpdateStoredBuffer();
                            }
                            /*Location4064*/
                            var newBuffer = writer.Slice(startPosition, length);
                            LazinatorMemoryStorage = newBuffer;
                            /*Location4065*/}
                            /*Location4066*/
                            protected override void UpdateDeserializedChildren(ref BinaryBufferWriter writer, int startPosition)
                            {
                                base.UpdateDeserializedChildren(ref writer, startPosition);
                                /*Location4067*/}
                                
                                /*Location4068*/
                                protected override void WritePropertiesIntoBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer, bool includeUniqueID)
                                {
                                    base.WritePropertiesIntoBuffer(ref writer, includeChildrenMode, verifyCleanness, updateStoredBuffer, includeUniqueID);
                                    /*Location4069*/// write properties
                                    /*Location4070*/CompressedIntegralTypes.WriteCompressedInt(ref writer, _AnotherPropertyAdded);
                                    /*Location4071*/}
                                    /*Location4072*/
                                }
                            }
