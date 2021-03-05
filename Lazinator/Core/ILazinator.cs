using System;
using System.Collections.Generic;
using Lazinator.Buffers;

namespace Lazinator.Core
{
    /// <summary>
    /// Every Lazinator object must implement this interface. The work of implementation is done in the code-behind. 
    /// </summary>
    public interface ILazinator
    {
        /// <summary>
        /// Serializes the Lazinator, if it has changed, into a new buffer, resetting dirtiness properties (but not HasChanged). This method will not create a new buffer if the 
        /// existing buffer already represents the current state of the object. If a new buffer is created, then the old buffer is disposed. The new buffer is stored internally
        /// in each in-memory Lazinator object.
        /// </summary>
        void SerializeLazinator();
        /// <summary>
        /// Initiates serialization starting from here, according to custom options, returning a new buffer.
        /// </summary>
        /// <param name="options">The serialization options</param>
        /// <returns></returns>
        LazinatorMemory SerializeLazinator(in LazinatorSerializationOptions options);
        /// <summary>
        /// Clones the class/struct, possibly excluding some or all children or descendants
        /// </summary>
        /// <param name="includeChildrenMode">Whether some or all children should be included</param>
        /// <param name="cloneBufferOptions">How the clone's buffer should relate to the original's</param>
        /// <returns>A cloned copy of the class/struct</returns>
        ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode = IncludeChildrenMode.IncludeAllChildren, CloneBufferOptions cloneBufferOptions = CloneBufferOptions.IndependentBuffers);
        /// <summary>
        /// Indicates whether a Lazinator object has been dirty since it was last deserialized (or since this property was manually set to false). This value does not change after a full or partial serialization, while IsDirty does change.
        /// </summary>
        bool HasChanged { get; set; }
        /// <summary>
        /// Indicates whether a child or other descendant of a Lazinator object has been dirty since the object was last deserialized (or since this property was manually set to false).
        /// </summary>
        bool DescendantHasChanged { get; set; }
        /// <summary>
        /// Indicates that a Lazinator object is dirty, meaning that one of its properties has changed since it was last serialized. A Lazinator object is considered dirty when a non-Lazinator property is accessed, even if it is not changed, unless there is a boolean Dirty property for that non-Lazinator property. 
        /// This may differ from HasChanged either if HasChanged is manually set to false or if IsDirty is reset when the corresponding Lazinator object is serialized.
        /// </summary>
        bool IsDirty { get; set; }
        /// <summary>
        /// Indicates whether a descendant of a Lazinator object is dirty.
        /// </summary>
        bool DescendantIsDirty { get; set; }
        /// <summary>
        /// Removes any native .Net objects, including those previously deserialized. Subsequent access to properties will thus be satisfied through the Lazinator memory storage. Typically, this is preceded by a call to UpdateStoredBuffer; otherwise, this has the effect of reverting to the last point at which the memory was up to date (e.g., initial deserialization or when a hash was obtained).
        /// </summary>
        void FreeInMemoryObjects();

        /// <summary>
        /// Enumerates nodes in the hierarchy, including the node at the top of the hierarchy, based on specified parameters.
        /// </summary>
        /// <param name="matchCriterion">If non-null, then a node will be yielded only if this function returns true.</param>
        /// <param name="stopExploringBelowMatch">If true, then once a matching node is found, it will be enumerated, but its dirty descendants will not be separately enumerated.</param>
        /// <param name="exploreCriterion">If non-null, then a node's children will be explored only if this function returns true.</param>
        /// <param name="exploreOnlyDeserializedChildren">If true, then children are enumerated only if they have been deserialized (and are thus not stored solely in bytes).</param>
        /// <param name="enumerateNulls">If true, then NULL classes and default structs will be enumerated.</param>
        /// <returns>The matched nodes</returns>
        IEnumerable<ILazinator> EnumerateLazinatorNodes(Func<ILazinator, bool> matchCriterion, bool stopExploringBelowMatch, Func<ILazinator, bool> exploreCriterion, bool exploreOnlyDeserializedChildren, bool enumerateNulls);
        /// <summary>
        /// Enumerates child nodes along with their property names based on specified parameters.
        /// </summary>
        /// <param name="matchCriterion">If non-null, then a node will be yielded only if this function returns true.</param>
        /// <param name="stopExploringBelowMatch">If true, then once a matching node is found, it will be enumerated, but its dirty descendants will not be separately enumerated.</param>
        /// <param name="exploreCriterion">If non-null, then a node's children will be explored only if this function returns true.</param>
        /// <param name="exploreOnlyDeserializedChildren">If true, then children are enumerated only if they have been deserialized (and are thus not stored solely in bytes).</param>
        /// <param name="enumerateNulls">If true, then NULL classes and default structs will be enumerated.</param>
        IEnumerable<(string propertyName, ILazinator descendant)> EnumerateLazinatorDescendants(Func<ILazinator, bool> matchCriterion, bool stopExploringBelowMatch, Func<ILazinator, bool> exploreCriterion, bool exploreOnlyDeserializedChildren, bool enumerateNulls);
        /// <summary>
        /// Enumerates the primitive and other non-Lazinator properties in the Lazinator object, including inherited properties but not including descendants' properties.
        /// </summary>
        /// <returns>The names of properties along with the property values converted to object</returns>
        IEnumerable<(string propertyName, object descendant)> EnumerateNonLazinatorProperties();
        /// <summary>
        /// Applies a function to each descendant of the Lazinator and to itself. It then returns itself (or if the Lazinator is a struct, a copy of itself).
        /// </summary>
        /// <param name="changeFunc">A function to change the Lazinator</param>
        /// <param name="exploreOnlyDeserializedChildren">If true, children that have not been deserialized are ignored.</param>
        /// <param name="changeThisLevel">Whether the change function should be applied at this level or only at lower levels</param>
        /// <returns>The Lazinator object, as transformed. (This is necessary to support this operation on Lazinator structs.)</returns>
        ILazinator ForEachLazinator(Func<ILazinator, ILazinator> changeFunc, bool exploreOnlyDeserializedChildren, bool changeThisLevel);

        /// <summary>
        /// This is primarily used internally after serialization to update the buffer of an in-memory Lazinator object to a specified range of bytes. 
        /// </summary>
        /// <param name="writer">The BinaryBufferWriter containing the new stored buffer</param>
        /// <param name="startPosition">The start position within the writer</param>
        /// <param name="length">The length within the writer</param>
        /// <param name="includeChildrenMode">Whether child objects should be included.</param>
        /// <param name="updateDeserializedChildren">Whether deserialized children should also have buffers updated</param>
        void UpdateStoredBuffer(ref BinaryBufferWriter writer, long startPosition, long length, IncludeChildrenMode includeChildrenMode, bool updateDeserializedChildren);

        /// <summary>
        /// This is primarily used internally during serialization of Lazinator objects. Continues serialization of this object and optionally its descendants by writing bytes into a pre-existing buffer. 
        /// </summary>
        /// <param name="writer">The BinaryBufferWriter to stream bytes to</param>
        /// <param name="options">Serialization options</param>
        void SerializeToExistingBuffer(ref BinaryBufferWriter writer, LazinatorSerializationOptions options);
        /// <summary>
        /// The memory used to initialize a Lazinator class/struct during initial deserialization. Header information, fields and child ISerializeds can then be read from this. This is set automatically by the Lazinator framework, either from DeserializeLazinator or from the parent's memory storage.
        /// </summary>
        LazinatorMemory LazinatorMemoryStorage { get; set; }
        /// <summary>
        /// An indication of whether children were included when the object was serialized into LazinatorMemoryStorage.
        /// </summary>
        IncludeChildrenMode OriginalIncludeChildrenMode { get; }
        /// <summary>
        /// Returns true if the Lazinator object is a struct. This is used internally by Lazinator.
        /// </summary>
        bool IsStruct { get; }
        /// <summary>
        /// Returns true if the Lazinator type hash a NonBinaryHash attribute, in which case calls to GetBinaryHashCode32 use the regular GetHashCode method (cast to uint).
        /// </summary>
        bool NonBinaryHash32 { get; }
        /// <summary>
        /// The parent (container) of the Lazinator class/struct, or null if there is none (because this is the top of the hierarchy or the parent is a struct or a class that doesn't implement ILazinator).
        /// </summary>
        LazinatorParentsCollection LazinatorParents { get; set; }
        /// <summary>
        /// The version number for this object.
        /// </summary>
        int LazinatorObjectVersion { get; set; }
    }
}
