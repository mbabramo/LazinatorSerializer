using System;
using System.Collections.Generic;
using System.IO;
using Lazinator.Buffers; 
using Lazinator.Core;
using static Lazinator.Core.LazinatorUtilities;

namespace Lazinator.Core
{
    public interface ILazinator
    {
        /// <summary>
        /// The memory (generally rented from a memory pool) used to initialize a self-serialized class/struct during deserialization. Header information, fields and child ISerializeds can then be read from this. This should be set when deserializing an object that represents the top of the hierarchy.
        /// </summary>
        MemoryInBuffer HierarchyBytes { set; }
        /// <summary>
        /// The bytes used to initialize a self-serialized class/struct during initial deserialization. Header information, fields and child ISerializeds can then be read from this. This is set automatically by the Lazinator framework, either from HierarchyBytes or from the parent's LazinatorObjectBytes.
        /// </summary>
        ReadOnlyMemory<byte> LazinatorObjectBytes { get; set; }
        /// <summary>
        /// The parent (container) of the self-serialized class/struct, or null if there is none (because this is the top of the hierarchy or the parent is a struct or a class that doesn't implement ILazinator).
        /// </summary>
        ILazinator LazinatorParentClass { get; set; }
        /// <summary>
        /// A delegate to inform the parent that a child or other descendant is dirty.
        /// </summary>
        InformParentOfDirtinessDelegate InformParentOfDirtinessDelegate { get; set; }

        /// <summary>
        /// Deserializes the class/struct and any serialized descendants from the object's serialized bytes. This is automatically called the first time there is an attempt to read a field or child, or when LazinatorObjectBytes is set. This reads data in the header and then calls ConvertFromBytesAfterHeader.
        /// </summary>
        /// <returns>The number of bytes processed during deserialization</returns>
        int Deserialize();
        /// <summary>
        /// Initiates serialization starting from here (and optionally including descendants), using the original bytes if the object is clean and manually writing bytes if necessary.
        /// </summary>
        /// <param name="includeChildren">Whether child objects should be included. If false, the child objects will be skipped.</param>
        /// <param name="verifyCleanness">Whether double-checking is needed to ensure that objects thought to be clean really are clean</param>
        /// <returns></returns>
        MemoryInBuffer SerializeNewBuffer(IncludeChildrenMode includeChildrenMode, bool verifyCleanness);
        /// <summary>
        /// Continues serialization of this object and optionally its descendants by writing bytes into a pre-existing buffer.
        /// </summary>
        /// <param name="writer">The BinaryBufferWriter to stream bytes to</param>
        /// <param name="includeChildrenMode">Whether child objects should be included.  If false, the child objects will be skipped.</param>
        /// <param name="verifyCleanness">Whether double-checking is needed to ensure that objects thought to be clean really are clean</param>
        /// <param name="updateStoredBuffer">Whether the object being serialized should be updated to use the new buffer. This is ignored and treated as false if includeChildrenMode is not set to include all children.</param>
        void SerializeExistingBuffer(BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer);
        /// <summary>
        /// Clones the class/struct by serializing, including all children, and then deserializing.
        /// </summary>
        /// <returns>A cloned copy of the class/struct</returns>
        ILazinator CloneLazinator();
        /// <summary>
        /// Clones the class/struct, possibly excluding some or all children
        /// </summary>
        /// <param name="includeChildrenMode">Whether some or all children should be included</param>
        /// <returns>A cloned copy of the class/struct</returns>
        ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode);

        /// <summary>
        /// Indicates whether a self-serialized object is dirty, meaning that one of its fields has changed. A change in a child of the self-serialized object does not change this field.
        /// </summary>
        bool IsDirty { get; set; }
        /// <summary>
        /// Indicates whether a descendant of a self-serialized object is dirty. A change in a field of the self-serialized object does not change this field.
        /// </summary>
        bool DescendantIsDirty { get; set; }
        /// <summary>
        /// Enumerates all nodes in the hierarchy that are dirty, walking through the parts of the hierarchy that are dirty or have dirty descendants.
        /// </summary>
        /// <returns></returns>
        IEnumerable<ILazinator> GetDirtyNodes();
        /// <summary>
        /// Enumerates all nodes in the hierarchy that are dirty, walking through the hierarchy based on specified parameters.
        /// </summary>
        /// <param name="exploreCriterion">If non-null, then a node's children will be explored only if this function returns true.</param>
        /// <param name="matchCriterion">If non-null, then a dirty node will be yielded only if this function returns true.</param>
        /// <param name="stopExploringBelowMatch">If true, then once a dirty node is found, it will be enumerated, but its dirty descendants will not be separately enumerated.</param>
        /// <returns></returns>
        IEnumerable<ILazinator> EnumerateLazinatorNodes(Func<ILazinator, bool> matchCriterion, bool stopExploringBelowMatch, Func<ILazinator, bool> exploreCriterion, bool exploreOnlyDeserializedChildren);

        /// <summary>
        /// Converts the Lazinator object to a byte representation, if it has changed. This does not need to be called manually before serialization.
        /// </summary>
        void LazinatorConvertToBytes();
        /// <summary>
        /// Returns the serialized length of an object, performing the serialization needed to make the computation if necessary.
        /// </summary>
        /// <returns></returns>
        int GetByteLength();
        /// <summary>
        /// Calculates a 32-bit noncryptographic hash code based on the bytes of the object.
        /// </summary>
        uint GetBinaryHashCode32();
        /// <summary>
        /// Calculates a 64-bit noncryptographic hash code based on the bytes of the object.
        /// </summary>
        ulong GetBinaryHashCode64();
        /// <summary>
        /// Calculates a 64-bit noncryptographic hash code based on the bytes of the object.
        /// </summary>
        Guid GetBinaryHashCode128();

        /// <summary>
        /// The version number for this object.
        /// </summary>
        int LazinatorObjectVersion { get; set; }
    }
}
