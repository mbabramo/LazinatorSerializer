using System;
using System.IO;
using Lazinator.Buffers; 
using Lazinator.Core;
using static Lazinator.Core.LazinatorUtilities;

namespace Lazinator.Core
{
    public interface ILazinator
    {
        /// <summary>
        /// A factory for producing classes implementing ILazinator based on their integer IDs, thus allowing for inheritance.
        /// </summary>
        DeserializationFactory DeserializationFactory { get; set; }
        /// <summary>
        /// The memory (generally rented from a memory pool) used to initialize a self-serialized class/struct during deserialization. Header information, fields and child ISerializeds can then be read from this. This should be set when deserializing an object that represents the top of the hierarchy.
        /// </summary>
        MemoryInBuffer HierarchyBytes { get; set; }
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
        void Deserialize();
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
        void SerializeExistingBuffer(BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness);
        /// <summary>
        /// Clones the class/struct by serializing, including all children, and then deserializing.
        /// </summary>
        /// <returns>A cloned copy of the class/struct</returns>
        ILazinator Clone();
        /// <summary>
        /// Clones the class/struct, possibly excluding some or all children
        /// </summary>
        /// <param name="includeChildrenMode">Whether some or all children should be included</param>
        /// <returns>A cloned copy of the class/struct</returns>
        ILazinator Clone(IncludeChildrenMode includeChildrenMode);

        /// <summary>
        /// Indicates whether a self-serialized object is dirty, meaning that one of its fields has changed. A change in a child of the self-serialized object does not change this field.
        /// </summary>
        bool IsDirty { get; set; }
        /// <summary>
        /// Indicates whether a descendant of a self-serialized object is dirty. A change in a field of the self-serialized object does not change this field.
        /// </summary>
        bool DescendantIsDirty { get; set; }

        /// <summary>
        /// The version number for this object.
        /// </summary>
        int LazinatorObjectVersion { get; set; }
    }
}
