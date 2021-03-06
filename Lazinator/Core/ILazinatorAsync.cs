﻿using Lazinator.Buffers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lazinator.Core
{
    public interface ILazinatorAsync : ILazinator
    {
        /// <summary>
        /// Serializes the Lazinator, if it has changed, into a new buffer, resetting dirtiness properties (but not HasChanged). This method will not create a new buffer if the 
        /// existing buffer already represents the current state of the object. If a new buffer is created, then the old buffer is disposed. The new buffer is stored internally
        /// in each in-memory Lazinator object.
        /// </summary>
        ValueTask SerializeLazinatorAsync();
        /// <summary>
        /// Initiates serialization starting from here, according to custom options, returning a new buffer.
        /// </summary>
        /// <param name="options">The serialization options</param>
        /// <returns></returns>
        ValueTask<LazinatorMemory> SerializeLazinatorAsync(LazinatorSerializationOptions options);
        /// <summary>
        /// Clones the class/struct, possibly excluding some or all children or descendants
        /// </summary>
        /// <param name="includeChildrenMode">Whether some or all children should be included</param>
        /// <param name="cloneBufferOptions">How the clone's buffer should relate to the original's</param>
        /// <returns>A cloned copy of the class/struct</returns>
        ValueTask<ILazinator> CloneLazinatorAsync(IncludeChildrenMode includeChildrenMode = IncludeChildrenMode.IncludeAllChildren, CloneBufferOptions cloneBufferOptions = CloneBufferOptions.IndependentBuffers);

        /// <summary>
        /// Enumerates nodes in the hierarchy, including the node at the top of the hierarchy, based on specified parameters.
        /// </summary>
        /// <param name="matchCriterion">If non-null, then a node will be yielded only if this function returns true.</param>
        /// <param name="stopExploringBelowMatch">If true, then once a matching node is found, it will be enumerated, but its dirty descendants will not be separately enumerated.</param>
        /// <param name="exploreCriterion">If non-null, then a node's children will be explored only if this function returns true.</param>
        /// <param name="exploreOnlyDeserializedChildren">If true, then children are enumerated only if they have been deserialized (and are thus not stored solely in bytes).</param>
        /// <param name="enumerateNulls">If true, then NULL classes and default structs will be enumerated.</param>
        /// <returns>The matched nodes</returns>
        IAsyncEnumerable<ILazinator> EnumerateLazinatorNodesAsync(Func<ILazinator, bool> matchCriterion, bool stopExploringBelowMatch, Func<ILazinator, bool> exploreCriterion, bool exploreOnlyDeserializedChildren, bool enumerateNulls);
        /// <summary>
        /// Enumerates child nodes along with their property names based on specified parameters.
        /// </summary>
        /// <param name="matchCriterion">If non-null, then a node will be yielded only if this function returns true.</param>
        /// <param name="stopExploringBelowMatch">If true, then once a matching node is found, it will be enumerated, but its dirty descendants will not be separately enumerated.</param>
        /// <param name="exploreCriterion">If non-null, then a node's children will be explored only if this function returns true.</param>
        /// <param name="exploreOnlyDeserializedChildren">If true, then children are enumerated only if they have been deserialized (and are thus not stored solely in bytes).</param>
        /// <param name="enumerateNulls">If true, then NULL classes and default structs will be enumerated.</param>
        IAsyncEnumerable<(string propertyName, ILazinator descendant)> EnumerateLazinatorDescendantsAsync(Func<ILazinator, bool> matchCriterion, bool stopExploringBelowMatch, Func<ILazinator, bool> exploreCriterion, bool exploreOnlyDeserializedChildren, bool enumerateNulls);
        /// <summary>
        /// Enumerates the primitive and other non-Lazinator properties in the Lazinator object, including inherited properties but not including descendants' properties.
        /// </summary>
        /// <returns>The names of properties along with the property values converted to object</returns>
        IAsyncEnumerable<(string propertyName, object descendant)> EnumerateNonLazinatorPropertiesAsync();
        /// <summary>
        /// Applies a function to each descendant of the Lazinator and to itself. It then returns itself (or if the Lazinator is a struct, a copy of itself).
        /// </summary>
        /// <param name="changeFunc">A function to change the Lazinator</param>
        /// <param name="exploreOnlyDeserializedChildren">If true, children that have not been deserialized are ignored.</param>
        /// <param name="changeThisLevel">Whether the change function should be applied at this level or only at lower levels</param>
        /// <returns>The Lazinator object, as transformed. (This is necessary to support this operation on Lazinator structs.)</returns>
        ValueTask<ILazinator> ForEachLazinatorAsync(Func<ILazinator, ILazinator> changeFunc, bool exploreOnlyDeserializedChildren, bool changeThisLevel);
        /// <summary>
        /// This is primarily used internally during serialization of Lazinator objects. Continues serialization of this object and optionally its descendants by writing bytes into a pre-existing buffer. 
        /// </summary>
        /// <param name="writer">The BufferWriter to stream bytes to</param>
        /// <param name="options">Serialization options</param>
        ValueTask SerializeToExistingBufferAsync(BufferWriterContainer writer, LazinatorSerializationOptions options);
    }
}
