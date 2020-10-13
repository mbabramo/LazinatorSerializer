using Lazinator.Buffers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lazinator.Core
{
    public interface ILazinatorAsync
    {
        /// <summary>
        /// Initiates serialization starting from here (and optionally including descendants), using the original bytes if the object is clean and manually writing bytes if necessary.
        /// </summary>
        /// <param name="includeChildrenMode">Whether child objects should be included. If false, the child objects will be skipped.</param>
        /// <param name="verifyCleanness">Whether double-checking is needed to ensure that objects thought to be clean really are clean</param>
        /// <param name="updateStoredBuffer">Whether the object being serialized should be updated to use the new buffer. This is ignored and treated as false if includeChildrenMode is not set to include all children. If false, then the returned memory will be wholly independent of the existing memory.</param>
        /// <returns></returns>
        Task<LazinatorMemory> SerializeLazinatorAsync(IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer);
        /// <summary>
        /// Clones the class/struct, possibly excluding some or all children or descendants
        /// </summary>
        /// <param name="includeChildrenMode">Whether some or all children should be included</param>
        /// <param name="cloneBufferOptions">How the clone's buffer should relate to the original's</param>
        /// <returns>A cloned copy of the class/struct</returns>
        Task<ILazinator> CloneLazinatorAsync(IncludeChildrenMode includeChildrenMode = IncludeChildrenMode.IncludeAllChildren, CloneBufferOptions cloneBufferOptions = CloneBufferOptions.IndependentBuffers);

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
        Task<ILazinator> ForEachLazinatorAsync(Func<ILazinator, ILazinator> changeFunc, bool exploreOnlyDeserializedChildren, bool changeThisLevel);

        /// <summary>
        /// This is primarily used internally for communication between Lazinator objects. Continues serialization of this object and optionally its descendants by writing bytes into a pre-existing buffer. 
        /// </summary>
        /// <param name="writer">The BinaryBufferWriter to stream bytes to</param>
        /// <param name="includeChildrenMode">Whether child objects should be included.  If false, the child objects will be skipped.</param>
        /// <param name="verifyCleanness">Whether double-checking is needed to ensure that objects thought to be clean really are clean</param>
        /// <param name="updateStoredBuffer">Whether the object being serialized should be updated to use the new buffer. This is ignored and treated as false if includeChildrenMode is not set to include all children.</param>
        Task SerializeToExistingBufferAsync(BinaryBufferWriterContainer writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer);
        /// <summary>
        /// This updates the stored buffer. This may be used to obtain LazinatorMemoryStorage before making further changes to the object.
        /// </summary>
        /// <param name="writer">The BinaryBufferWriter containing the new stored buffer</param>
        /// <param name="startPosition">The start position within the writer</param>
        /// <param name="length">The length within the writer</param>
        /// <param name="includeChildrenMode">Whether child objects should be included.</param>
        /// <param name="updateDeserializedChildren">Whether deserialized children should also have buffers updated</param>
        Task UpdateStoredBufferAsync(BinaryBufferWriterContainer writer, int startPosition, int length, IncludeChildrenMode includeChildrenMode, bool updateDeserializedChildren);
    }
}
