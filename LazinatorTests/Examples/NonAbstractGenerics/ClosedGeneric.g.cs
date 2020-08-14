/*Location3797*//*Location3782*///5d05017e-abcf-7523-a888-acebd68070be
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
{/*Location3783*/
    using Lazinator.Attributes;/*Location3784*/
    using Lazinator.Buffers;/*Location3785*/
    using Lazinator.Core;/*Location3786*/
    using Lazinator.Exceptions;/*Location3787*/
    using Lazinator.Support;/*Location3788*/
    using LazinatorTests.Examples;/*Location3789*/
    using System;/*Location3790*/
    using System.Buffers;/*Location3791*/
    using System.Collections.Generic;/*Location3792*/
    using System.Diagnostics;/*Location3793*/
    using System.IO;/*Location3794*/
    using System.Linq;/*Location3795*/
    using System.Runtime.InteropServices;/*Location3796*/
    using static Lazinator.Core.LazinatorUtilities;
    
    [Autogenerated]
    public partial class ClosedGeneric : OpenGeneric<ExampleChild>, ILazinator
    {
        /*Location3798*//* Property definitions */
        
        
        /*Location3799*/
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
        /*Location3801*/        /* Clone overrides */
        
        public ClosedGeneric(LazinatorConstructorEnum constructorEnum) : base(constructorEnum)
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
            /*Location3800*/typedClone.AnotherPropertyAdded = AnotherPropertyAdded;
            
            return typedClone;
        }
        
        /* Properties */
        /*Location3802*/
        public override IEnumerable<(string propertyName, ILazinator descendant)> EnumerateLazinatorDescendants(Func<ILazinator, bool> matchCriterion, bool stopExploringBelowMatch, Func<ILazinator, bool> exploreCriterion, bool exploreOnlyDeserializedChildren, bool enumerateNulls)
        {
            foreach (var inheritedYield in base.EnumerateLazinatorDescendants(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
            {
                yield return inheritedYield;
            }
            /*Location3803*/yield break;
        }
        /*Location3804*/
        
        public override IEnumerable<(string propertyName, object descendant)> EnumerateNonLazinatorProperties()
        {
            foreach (var inheritedYield in base.EnumerateNonLazinatorProperties())
            {
                yield return inheritedYield;
            }
            /*Location3805*/yield return ("AnotherPropertyAdded", (object)AnotherPropertyAdded);
            /*Location3806*/yield break;
        }
        /*Location3807*/
        public override ILazinator ForEachLazinator(Func<ILazinator, ILazinator> changeFunc, bool exploreOnlyDeserializedChildren, bool changeThisLevel)
        {
            base.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, false);
            /*Location3808*/if (changeThisLevel && changeFunc != null)
            {
                return changeFunc(this);
            }
            return this;
        }
        /*Location3809*/
        public override void FreeInMemoryObjects()
        {
            base.FreeInMemoryObjects();
            
            IsDirty = false;
            DescendantIsDirty = false;
            HasChanged = false;
            DescendantHasChanged = false;
        }
        /*Location3810*/
        /* Conversion */
        
        public override int LazinatorUniqueID => 1050;
        
        protected override bool ContainsOpenGenericParameters => false;
        public override LazinatorGenericIDType LazinatorGenericID => default;
        
        
        public override int LazinatorObjectVersion { get; set; } = 0;
        
        
        /*Location3811*/public override void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            base.ConvertFromBytesAfterHeader(OriginalIncludeChildrenMode, serializedVersionNumber, ref bytesSoFar);
            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
            /*Location3812*/_AnotherPropertyAdded = span.ToDecompressedInt(ref bytesSoFar);
            /*Location3813*/        }
            
            /*Location3814*/public override void SerializeExistingBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
            {
                /*Location3815*/if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren)
                {
                    updateStoredBuffer = false;
                }
                /*Location3816*/int startPosition = writer.Position;
                WritePropertiesIntoBuffer(ref writer, includeChildrenMode, verifyCleanness, updateStoredBuffer, true);
                /*Location3817*/if (updateStoredBuffer)
                {
                    UpdateStoredBuffer(ref writer, startPosition, writer.Position - startPosition, includeChildrenMode, false);
                    /*Location3818*/}
                    /*Location3819*/}
                    /*Location3820*/
                    public override void UpdateStoredBuffer(ref BinaryBufferWriter writer, int startPosition, int length, IncludeChildrenMode includeChildrenMode, bool updateDeserializedChildren)
                    {
                        /*Location3821*/_IsDirty = false;
                        if (includeChildrenMode == IncludeChildrenMode.IncludeAllChildren)
                        {
                            _DescendantIsDirty = false;/*Location3822*/
                            if (updateDeserializedChildren)
                            {
                                UpdateDeserializedChildren(ref writer, startPosition);
                                /*Location3823*/}
                                /*Location3824*//*Location3825*/
                            }
                            else
                            {
                                ThrowHelper.ThrowCannotUpdateStoredBuffer();
                            }
                            /*Location3826*/
                            var newBuffer = writer.Slice(startPosition, length);
                            LazinatorMemoryStorage = newBuffer;
                            /*Location3827*/}
                            /*Location3828*/
                            protected override void UpdateDeserializedChildren(ref BinaryBufferWriter writer, int startPosition)
                            {
                                base.UpdateDeserializedChildren(ref writer, startPosition);
                                /*Location3829*/}
                                
                                /*Location3830*/
                                protected override void WritePropertiesIntoBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer, bool includeUniqueID)
                                {
                                    base.WritePropertiesIntoBuffer(ref writer, includeChildrenMode, verifyCleanness, updateStoredBuffer, includeUniqueID);
                                    /*Location3831*/// write properties
                                    /*Location3832*/CompressedIntegralTypes.WriteCompressedInt(ref writer, _AnotherPropertyAdded);
                                    /*Location3833*/}
                                    /*Location3834*/
                                }
                            }
