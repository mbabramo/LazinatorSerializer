using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipelines;
using System.Linq;
using System.Text;
using Lazinator.Buffers;
using Lazinator.Collections;
using Lazinator.Exceptions;
using Lazinator.Support;

namespace Lazinator.Core
{
    public static class LazinatorUtilities
    {
        #region Delegate types

        // Delegate types. Methods matching these types must be passed into some of the methods below.

        public delegate LazinatorMemory EncodeManuallyDelegate(IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer);

        public delegate LazinatorMemory ReturnLazinatorMemoryDelegate();

        public delegate void WriteDelegate(ref BinaryBufferWriter writer);

        public delegate void WritePossiblyVerifyingCleannessDelegate(ref BinaryBufferWriter writer, bool verifyCleanness);

        #endregion

        #region Empties

        public static Memory<byte> EmptyMemory = new Memory<byte>();
        public static ReadOnlyMemory<byte> EmptyReadOnlyMemory = new ReadOnlyMemory<byte>();
        public static LazinatorMemory EmptyLazinatorMemory = new LazinatorMemory(new Memory<byte>());

        #endregion

        #region Encoding

        /// <summary>
        /// Serializes from the top of the hierarchy, using the original storage if the item is not dirty and does not need its cleanness verified or, if dirty, creating a stream to manually serialize. 
        /// </summary>
        /// <param name="includeChildrenMode">Includes children (and thus descendants) when converting to bytes.</param>
        /// <param name="originalIncludeChildrenMode">The original mode used to serialize this object.</param>
        /// <param name="verifyCleanness">If true, then the dirty-conversion will always be performed unless we are sure it is clean, and if the object is not believed to be dirty, the results will be compared to the clean version. This allows for errors from failure to serialize objects that have been changed to be caught during development. Set this to false if you may wish to dispose of the memory backing the original while still using the new deserialized bytes.</param>
        /// <param name="isBelievedDirty">An indication of whether the object to be converted to bytes is believed to be dirty, i.e. has had its dirty flag set.</param>
        /// <param name="isDefinitelyClean">An indication whether any storage, if it exists, is definitely clean. If the storage has never been converted into bytes, then it is definitely clean. If the storage does not exist (it hasn't been serialized yet), then this is irrelevant, because there is no need to verify cleanliness.</param>
        /// <param name="originalStorage">The storage of the item before any changes were made to it</param>
        /// <param name="encodeManuallyFn">The function that completes the conversion to bytes, without considering using the original storage for the item as a whole.</param>
        /// <returns></returns>
        public static LazinatorMemory EncodeOrRecycleToNewBuffer(IncludeChildrenMode includeChildrenMode, IncludeChildrenMode originalIncludeChildrenMode, bool verifyCleanness, bool isBelievedDirty, bool descendantIsBelievedDirty, bool isDefinitelyClean, LazinatorMemory originalStorage, EncodeManuallyDelegate encodeManuallyFn, bool updateStoredBuffer)
        {
            // if item has never been serialized before, there will be no storage, so we must convert to bytes.
            // we also must convert to bytes if we have to verify cleanness or if this is believed to be dirty,
            // unless the original storage is definitely clean.
            if (originalStorage == null || originalStorage.Length == 0 ||
                includeChildrenMode != originalIncludeChildrenMode ||
                    (!isDefinitelyClean
                        &&
                        (verifyCleanness ||
                        isBelievedDirty ||
                        (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && descendantIsBelievedDirty)
                        )
                     )
                )
                return encodeManuallyFn(includeChildrenMode, verifyCleanness, updateStoredBuffer);

            // We can use the original storage. But we still have to copy it into a new buffer, as requested.
            BinaryBufferWriter writer = new BinaryBufferWriter(originalStorage?.Length ?? 0); 
            writer.Write(originalStorage.Span);
            return writer.LazinatorMemory;
        }

        public static LazinatorMemory EncodeToNewBinaryBufferWriter<T>(T lazinatorObject, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer) where T : ILazinator, new()
        {
            BinaryBufferWriter writer = new BinaryBufferWriter(ExpandableBytes.MinMinBufferSize);
            lazinatorObject.SerializeExistingBuffer(ref writer, includeChildrenMode, verifyCleanness, updateStoredBuffer);
            return writer.LazinatorMemory;
        }

        /// <summary>
        /// Completes an action to write to binary, but then prefixes the binary writer with the total length of what was written, excluding the length itself
        /// </summary>
        /// <param name="writer">The binary writer</param>
        /// <param name="action">The action to complete</param>
        public static void WriteToBinaryWithIntLengthPrefix(ref BinaryBufferWriter writer, WriteDelegate action)
        {
            int lengthPosition = writer.Position;
            writer.Write((uint)0);
            action(ref writer);
            int afterPosition = writer.Position;
            writer.Position = lengthPosition;
            int length = (afterPosition - lengthPosition - sizeof(uint));
            writer.Write(length);
            writer.Position = afterPosition;
        }

        /// <summary>
        /// Completes an action to write to binary, without any length prefix.
        /// </summary>
        /// <param name="writer">The binary writer</param>
        /// <param name="action">The action to complete</param>
        public static void WriteToBinaryWithoutLengthPrefix(ref BinaryBufferWriter writer, WriteDelegate action)
        {
            action(ref writer);
        }

        /// <summary>
        /// Completes an action to write to binary, but then prefixes the binary writer with the total length of what was written, excluding the length itself
        /// </summary>
        /// <param name="writer">The binary writer</param>
        /// <param name="action">The action to complete</param>
        public static void WriteToBinaryWithByteLengthPrefix(ref BinaryBufferWriter writer, WriteDelegate action)
        {
            int lengthPosition = writer.Position;
            writer.Write((byte)0);
            action(ref writer);
            int afterPosition = writer.Position;
            writer.Position = lengthPosition;
            int length = (afterPosition - lengthPosition - sizeof(byte));
            if (length > 250)
                throw new LazinatorSerializationException("Writing with byte length prefix limited to items no more than 250 bytes.");
            writer.Write((byte)length);
            writer.Position = afterPosition;
        }

        /// <summary>
        /// Initiates a binary write to a child of a Lazinator object, optionally including a length prefix, using existing storage if possible
        /// </summary>
        /// <param name="writer">The binary writer</param>
        /// <param name="child">The child to be written. This child will be written regardless of includeChildrenMode.</param>
        /// <param name="includeChildrenMode">An indication of which descendants of this child should be written.</param>
        /// <param name="childHasBeenAccessed">True if the child's value has been accessed.</param>
        /// <param name="getChildSliceFn">A function to return the child's original storage</param>
        /// <param name="verifyCleanness">If true, cleanness of any nonserialized fields in the child will be verified if necessary</param>
        /// <param name="updateStoredBuffer">If true, updates the child object's byte buffer to store the serialized information.</param>
        /// <param name="restrictLengthTo250Bytes">If true, the length is stored in a single byte. If the length might be bigger then this, and length is not being skipped, set this to true.</param>
        /// <param name="skipLength">If true, the length is omitted altogether.</param>
        /// <param name="parent">The parent of the object being written</param>
        public static void WriteChild<T>(ref BinaryBufferWriter writer, ref T child,
            IncludeChildrenMode includeChildrenMode, bool childHasBeenAccessed,
            ReturnLazinatorMemoryDelegate getChildSliceFn, bool verifyCleanness, bool updateStoredBuffer, bool restrictLengthTo250Bytes, bool skipLength, ILazinator parent) where T : ILazinator
        {
            bool childCouldHaveChanged = childHasBeenAccessed || (child != null && includeChildrenMode != child.OriginalIncludeChildrenMode);
            LazinatorMemory childStorage = default;
            if (!childHasBeenAccessed && child != null)
            {
                childStorage = getChildSliceFn();
                if (childStorage == null || childStorage.Memory.Length == 0)
                    childCouldHaveChanged = true; // child might be an uninitialized struct and the object has not been previously deserialized. Thus, we treat this as an object that has been changed, so that we can serialize it. 
            }
            else
            {
                // check for a child that has been accessed (or otherwise could have changed) and that is in memory and totally clean. 
                if (childCouldHaveChanged && child != null && !child.IsDirty && !child.DescendantIsDirty && includeChildrenMode == IncludeChildrenMode.IncludeAllChildren && includeChildrenMode == child.OriginalIncludeChildrenMode) 
                {
                    // In this case, we update the childStorage to reflect the child's own storage, rather than a slice in the parent's storage. The reason is that the buffer may have been updated if the same object appears more than once in the object hierarchy, or the child may have updated its storage after a manual call to EnsureLazinatorMemoryUpToDate.
                    childStorage = child.LazinatorMemoryStorage;
                    if (childStorage != null && childStorage.Memory.Length != 0)
                        childCouldHaveChanged = false;
                }
            }
            if (!childCouldHaveChanged)
            {
                int startPosition = writer.Position;
                childStorage = WriteExistingChildStorage(ref writer, getChildSliceFn, restrictLengthTo250Bytes, skipLength, childStorage);
                if (updateStoredBuffer)
                {
                    if (child != null)
                    {
                        int length = childStorage.Length;
                        if (!skipLength)
                        {
                            startPosition += restrictLengthTo250Bytes ? 1 : 4;
                            // note that the length is set correctly
                        }
                        if (child.LazinatorMemoryStorage?.OwnedMemory is ExpandableBytes e)
                        {
                            child.UpdateStoredBuffer(ref writer, startPosition, length, includeChildrenMode, true);
                        }
                    }
                }
            }
            else
            {
                if (child == null)
                {
                    WriteNullChild(ref writer, restrictLengthTo250Bytes, skipLength);
                }
                else
                {
                    WriteChildToBinary(ref writer, ref child, includeChildrenMode, verifyCleanness, updateStoredBuffer, restrictLengthTo250Bytes, skipLength);
                }
            }
            AddParentToChildless(ref child, parent);
        }

        public static void WriteNullChild(ref BinaryBufferWriter writer, bool restrictLengthTo250Bytes, bool skipLength)
        {
            if (!skipLength)
            {
                if (restrictLengthTo250Bytes)
                    writer.Write((byte)0);
                else
                    writer.Write((uint)0);
            }
        }

        public static LazinatorMemory WriteExistingChildStorage(ref BinaryBufferWriter writer, ReturnLazinatorMemoryDelegate getChildSliceFn, bool restrictLengthTo250Bytes, bool skipLength, LazinatorMemory childStorage)
        {
            // The child is null, not because it was set to null, but because it was never accessed. Thus, we need to use the last version from storage (or just to store a zero-length if this is the first time saving it).
            if (childStorage == null || childStorage.Length == 0)
                childStorage = getChildSliceFn(); // this is the storage holding the child, which has never been accessed
            if (skipLength)
                writer.Write(childStorage.Span);
            else if (restrictLengthTo250Bytes)
                childStorage.ReadOnlySpan.Write_WithByteLengthPrefix(ref writer);
            else
                childStorage.ReadOnlySpan.Write_WithIntLengthPrefix(ref writer);
            return childStorage;
        }

        private static void AddParentToChildless<T>(ref T child, ILazinator parent) where T : ILazinator
        {
            if (child != null && !child.LazinatorParents.Any())
                child.LazinatorParents = child.LazinatorParents.WithAdded(parent); // set the parent so that this object can be used like a deserialized object
        }

        /// <summary>
        /// Initiates a binary write to a child of a Lazinator object, optionally including a length prefix, using the child's own storage if possible.
        /// </summary>
        /// <param name="writer">The binary writer</param>
        /// <param name="child">The child</param>
        /// <param name="includeChildrenMode"></param>
        /// <param name="verifyCleanness">If true, cleanness of any nonserialized fields in the child will be verified if necessary</param>
        /// <param name="updateStoredBuffer">If true, updates the child object's byte buffer to store the serialized information.</param>
        /// <param name="restrictLengthTo250Bytes">If true, the length is stored in a single byte. If the length might be bigger then this, and length is not being skipped, set this to true.</param>
        /// <param name="skipLength">If true, the length is omitted altogether.</param>
        /// <param name="parent">The parent of the object being written</param>
        public static void WriteChildToBinary<T>(ref BinaryBufferWriter writer, ref T child, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer, bool restrictLengthTo250Bytes, bool skipLength) where T : ILazinator
        {
            T childCopy = child;
            void action(ref BinaryBufferWriter w)
            {
                if (childCopy.LazinatorMemoryStorage == null || childCopy.LazinatorMemoryStorage.Length == 0 || childCopy.IsDirty || childCopy.DescendantIsDirty || verifyCleanness || includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != childCopy.OriginalIncludeChildrenMode)
                    childCopy.SerializeExistingBuffer(ref w, includeChildrenMode, verifyCleanness, updateStoredBuffer);
                else
                    w.Write(childCopy.LazinatorMemoryStorage.Span); // the childCopy has been accessed, but is unchanged, so we can use the storage holding the childCopy
            }
            if (skipLength)
                LazinatorUtilities.WriteToBinaryWithoutLengthPrefix(ref writer, action);
            else if (restrictLengthTo250Bytes)
                LazinatorUtilities.WriteToBinaryWithByteLengthPrefix(ref writer, action);
            else
                LazinatorUtilities.WriteToBinaryWithIntLengthPrefix(ref writer, action);
        }

        #endregion

        #region Generic IDs

        public static LazinatorGenericIDType GetGenericIDIfApplicable(bool containsOpenGenericParameters, int uniqueID, ReadOnlySpan<byte> span, ref int index)
        {
            if (containsOpenGenericParameters)
            {
                List<int> lazinatorGenericID = ReadLazinatorGenericID(span, ref index).TypeAndInnerTypeIDs;
                if (lazinatorGenericID[0] != uniqueID)
                {
                    throw new FormatException("Wrong Lazinator type initialized.");
                }
                return new LazinatorGenericIDType(lazinatorGenericID);
            }
            else
            {
                int readUniqueID = span.ToDecompressedInt(ref index);
                if (readUniqueID != uniqueID)
                {
                    throw new FormatException("Wrong Lazinator type initialized.");
                }
                return default;
            }
        }

        public static void WriteLazinatorGenericID(ref BinaryBufferWriter writer, LazinatorGenericIDType lazinatorGenericID)
        {
            // We write the first component before the total count to be consistent with non-generic items.
            CompressedIntegralTypes.WriteCompressedInt(ref writer, lazinatorGenericID.TypeAndInnerTypeIDs[0]);
            int numItems = lazinatorGenericID.TypeAndInnerTypeIDs.Count;
            writer.Write((byte)numItems);
            for (int i = 1; i < numItems; i++)
            {
                CompressedIntegralTypes.WriteCompressedInt(ref writer, lazinatorGenericID.TypeAndInnerTypeIDs[i]);
            }
        }

        public static LazinatorGenericIDType ReadLazinatorGenericID(ReadOnlySpan<byte> span, ref int index)
        {
            int mainID = span.ToDecompressedInt(ref index);
            List<int> l = new List<int>() { mainID };
            byte numEntries = span.ToByte(ref index);
            for (byte b = 1; b < numEntries; b++)
            {
                l.Add(span.ToDecompressedInt(ref index));
            }
            return new LazinatorGenericIDType(l);
        }

        #endregion

        #region Nonlazinators

        /// <summary>
        /// Initiates the conversion to binary of a non-lazinator object. 
        /// </summary>
        /// <param name="nonLazinatorObject">An object that does not implement ILazinator</param>
        /// <param name="isBelievedDirty">An indication of whether the object to be converted to bytes is believed to be dirty, e.g. has had its dirty flag set.</param>
        /// <param name="isAccessed">An indication of whether the object has been accessed.</param>
        /// <param name="writer">The binary writer</param>
        /// <param name="getChildSliceForFieldFn"></param>
        /// <param name="verifyCleanness">If true, then the dirty-conversion will always be performed unless we are sure it is clean, and if the object is not believed to be dirty, the results will be compared to the clean version. This allows for errors from failure to serialize objects that have been changed to be caught during development.</param>
        /// <param name="binaryWriterAction"></param>
        public static void WriteNonLazinatorObject(object nonLazinatorObject,
            bool isBelievedDirty, bool isAccessed, ref BinaryBufferWriter writer, ReturnLazinatorMemoryDelegate getChildSliceForFieldFn,
            bool verifyCleanness, WritePossiblyVerifyingCleannessDelegate binaryWriterAction)
        {
            LazinatorMemory original = getChildSliceForFieldFn();
            int length = original.Length;
            if (!isAccessed && length > 0)
            {
                // object has never been loaded into memory, so there is no need to verify cleanness
                // just return what we have.
                original.ReadOnlySpan.Write_WithIntLengthPrefix(ref writer);
            }
            else if (isBelievedDirty || length == 0)
            {
                // We definitely need to write to binary, because either the dirty flag has been set or the original storage doesn't have anything to help us.
                void action(ref BinaryBufferWriter w) => binaryWriterAction(ref w, verifyCleanness);
                WriteToBinaryWithIntLengthPrefix(ref writer, action);
            }
            else
            {
                if (verifyCleanness)
                {
                    ReadOnlyMemory<byte> revised = ConvertNonLazinatorObjectToBytes(nonLazinatorObject, binaryWriterAction);
                    ConfirmMatch(original.Memory, revised);
                }
                original.ReadOnlySpan.Write_WithIntLengthPrefix(ref writer);
            }
        }

        /// <summary>
        /// Initiates the conversion to binary of a non-lazinator object, omitting the length prefix. This can be used on the last property of a struct or sealed class.
        /// </summary>
        /// <param name="nonLazinatorObject">An object that does not implement ILazinator</param>
        /// <param name="isBelievedDirty">An indication of whether the object to be converted to bytes is believed to be dirty, e.g. has had its dirty flag set.</param>
        /// <param name="isAccessed">An indication of whether the object has been accessed.</param>
        /// <param name="writer">The binary writer</param>
        /// <param name="getChildSliceForFieldFn"></param>
        /// <param name="verifyCleanness">If true, then the dirty-conversion will always be performed unless we are sure it is clean, and if the object is not believed to be dirty, the results will be compared to the clean version. This allows for errors from failure to serialize objects that have been changed to be caught during development.</param>
        /// <param name="binaryWriterAction"></param>
        public static void WriteNonLazinatorObject_WithoutLengthPrefix(object nonLazinatorObject,
            bool isBelievedDirty, bool isAccessed, ref BinaryBufferWriter writer, ReturnLazinatorMemoryDelegate getChildSliceForFieldFn,
            bool verifyCleanness, WritePossiblyVerifyingCleannessDelegate binaryWriterAction)
        {
            LazinatorMemory original = getChildSliceForFieldFn();
            if (!isAccessed)
            {
                // object has never been loaded into memory, so there is no need to verify cleanness
                // just return what we have.
                writer.Write(original.Span);
            }
            else if (isBelievedDirty || original.Length == 0)
            {
                // We definitely need to write to binary, because either the dirty flag has been set or the original storage doesn't have anything to help us.
                void action(ref BinaryBufferWriter w) => binaryWriterAction(ref w, verifyCleanness);
                WriteToBinaryWithoutLengthPrefix(ref writer, action);
            }
            else
            {
                if (verifyCleanness)
                {
                    ReadOnlyMemory<byte> revised = ConvertNonLazinatorObjectToBytes(nonLazinatorObject, binaryWriterAction);
                    ConfirmMatch(original.Memory, revised);
                }
                writer.Write(original.Span);
            }
        }

        /// <summary>
        /// Converts a non-Lazinator property to bytes by creating a binary buffer writer and calling an action that accepts a binary writer as a parameter.
        /// This is used only when it is necessary to verify the cleanness of a non-Lazinator property.
        /// </summary>
        /// <param name="nonLazinatorObject">The object to be converted</param>
        /// <param name="binaryWriterAction">The method that uses a binary writer to write the bytes for the non Lazinator fields. The second parameter will be ignored.</param>
        /// <returns></returns>
        public static ReadOnlyMemory<byte> ConvertNonLazinatorObjectToBytes(object nonLazinatorObject, WritePossiblyVerifyingCleannessDelegate binaryWriterAction)
        {
            if (nonLazinatorObject == null)
                return new ReadOnlyMemory<byte>();

            BinaryBufferWriter writer = new BinaryBufferWriter();
            binaryWriterAction(ref writer, true);
            var memory = writer.LazinatorMemory.Memory;
            byte[] bytes = new byte[memory.Length];
            memory.CopyTo(bytes);
            writer.LazinatorMemory.Dispose();
            return bytes;
        }

        /// <summary>
        /// Verifies that the original storage for a non-Lazinator object matches rewritten storage. (though it can be used for any storage) matches the storage generated by rewriting to bytes. Calls an Exception if that is not the case.
        /// </summary>
        /// <param name="originalStorage">The original storage before any changes could have been made</param>
        /// <param name="rewrittenStorage">The rewritten storage after changes could have been made, but were not thought to have been.</param>
        public static void ConfirmMatch(ReadOnlyMemory<byte> originalStorage, ReadOnlyMemory<byte> rewrittenStorage)
        {
            bool matches = rewrittenStorage.Span.Matches(originalStorage.Span);
            if (!matches)
            {
                int location = rewrittenStorage.Span.FindFirstNonMatch(originalStorage.Span);
                string explanation;
                if (location == int.MaxValue)
                    explanation = "Internal error. One of rewrittenStorage and originalStorage was null, while other wasn't, or they differed in length.";
                else if (location == int.MaxValue - 1)
                    explanation = "Internal error. Matches and FindFirstNonMatch inconsistent.";
                else
                    explanation = $"An object was found to have changed, even though internal records indicated that IsDirty was not set. The location in the bytestream was {location}. The stack trace will show what the object was";
                throw new UnexpectedDirtinessException(explanation);
            }
        }

        #endregion

        #region Enumeration

        /// <summary>
        /// Enumerates all nodes in the hierarchy.
        /// </summary>
        /// <returns>The dirty nodes</returns>
        public static IEnumerable<ILazinator> EnumerateAllNodes(this ILazinator startNode)
        {
            return startNode.EnumerateLazinatorNodes(x => true, false, x => true, false, false);
        }

        /// <summary>
        /// Enumerates the Lazinator children of a node, along with their names
        /// </summary>
        /// <param name="node">The node</param>
        /// <returns>The Lazinator children of the node along with their property names</returns>
        public static IEnumerable<(string propertyName, ILazinator descendant)> EnumerateLazinatorChildren(this ILazinator node, bool deserializedOnly = false)
        {
            return node.EnumerateLazinatorDescendants(x => true, true, x => false, deserializedOnly, true);
        }

        /// <summary>
        /// Enumerates the descendants of a node that have already been deserialized.
        /// </summary>
        /// <param name="node">The node</param>
        /// <returns>The node and any deserialized descendants</returns>
        public static IEnumerable<(string propertyName, ILazinator descendant)> EnumerateDeserializedDescendants(this ILazinator node)
        {
            return node.EnumerateLazinatorDescendants(x => true, true, x => true, true, false);
        }

        /// <summary>
        /// Returns a dynamic object indicating the Lazinator children of a node, along with their names.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="deserializedOnly"></param>
        /// <returns></returns>
        public static dynamic ViewLazinatorChildren(this ILazinator node, bool deserializedOnly = false)
        {
            dynamic d = new System.Dynamic.ExpandoObject();
            foreach (var childInfo in EnumerateLazinatorChildren(node, deserializedOnly))
            {
                ((IDictionary<String, object>)d)[childInfo.propertyName] = childInfo.descendant;
            }
            return d;
        }

        /// <summary>
        /// Returns a string representing the entire hierarchy beginning with this node.
        /// </summary>
        /// <param name="node">The node</param>
        /// <returns>A hierarchical string representation</returns>
        public static string GetHierarchyString(this ILazinator node)
        {
            var clone = node.CloneLazinator(IncludeChildrenMode.IncludeAllChildren, CloneBufferOptions.IndependentBuffers); // clone to minimize side effects from accessing properties
            HierarchyTree tree = new HierarchyTree(clone);
            return tree.ToString();
        }

        /// <summary>
        /// Verifies that the 64-bit hashes of each node in two hierarchies match, indicating likely equality.
        /// If the hashes do not match, then this will walk the hierarchy tree to find the first node where 
        /// a difference is manifest when children are excluded.
        /// </summary>
        /// <param name="firstHierarchy">The first Lazinator hierarchy</param>
        /// <param name="secondHierarchy">The second Lazinator hierarchy</param>
        /// <returns>True if the hashes match</returns>
        public static void ConfirmHierarchiesEqual(ILazinator firstHierarchy, ILazinator secondHierarchy, string propertyNameSequence = "")
        {
            ulong firstHash = firstHierarchy?.GetBinaryHashCode64() ?? 0;
            ulong secondHash = secondHierarchy?.GetBinaryHashCode64() ?? 0;

            if (firstHash != secondHash)
            {
                if (TopNodesOfHierarchyEqual(firstHierarchy, secondHierarchy, out string comparison))
                {
                    // Difference must be in a child. Find the children with the match failure.
                    var firstHierarchyNodes = EnumerateLazinatorChildren(firstHierarchy).ToList();
                    var secondHierarchyNodes = EnumerateLazinatorChildren(secondHierarchy).ToList();
                    var zipped = firstHierarchyNodes.Zip(secondHierarchyNodes, (x, y) => (x, y));
                    foreach (var pair in zipped)
                    {
                        try
                        {
                            if (pair.x.propertyName != pair.y.propertyName)
                                throw new Exception($"Children properties unexpectedly differed {pair.x.propertyName} vs. {pair.y.propertyName}");
                            ConfirmHierarchiesEqual(pair.x.descendant, pair.y.descendant, propertyNameSequence == "" ? pair.x.propertyName : propertyNameSequence + " > " + pair.x.propertyName);
                        }
                        catch
                        {
                            throw;
                        }
                    }
                }
                string errorMessage = comparison;
                throw new Exception($"Hashes were expected to be same, but differed. Difference traced to {propertyNameSequence}:" + Environment.NewLine + errorMessage);
            }
        }

        /// <summary>
        /// Returns true if two hierarchies, when stripped of their Lazinator children, have the same hash. If not, a string comparison is returned.
        /// Note that 
        /// </summary>
        /// <param name="firstHierarchy"></param>
        /// <param name="secondHierarchy"></param>
        /// <param name="comparison"></param>
        /// <returns></returns>
        public static bool TopNodesOfHierarchyEqual(ILazinator firstHierarchy, ILazinator secondHierarchy, out string comparison)
        {
            comparison = "";
            if (firstHierarchy == null && secondHierarchy == null)
            {
                return true;
            }
            if ((firstHierarchy == null) != (secondHierarchy == null))
            {
                comparison = $"{(firstHierarchy == null ? "First was null; second was not" : "Second was null; first was not")}";
                return false;
            }
            if (firstHierarchy is ILazinatorList firstList && secondHierarchy is ILazinatorList secondList)
            {
                if (firstList.Count == secondList.Count)
                    return true;
                else
                {
                    comparison = $"First list had {firstList.Count} members, while second list had {secondList.Count}";
                    return false;
                }
            };
            ILazinator firstWithoutChildren = firstHierarchy.CloneLazinatorTyped(IncludeChildrenMode.ExcludeAllChildren);
            ILazinator secondWithoutChildren = secondHierarchy.CloneLazinatorTyped(IncludeChildrenMode.ExcludeAllChildren);
            if (firstWithoutChildren.GetBinaryHashCode64() != secondWithoutChildren.GetBinaryHashCode64())
            {
                string firstHierarchyTreeString = new HierarchyTree(firstWithoutChildren).ToString();
                string secondHierarchyTreeString = new HierarchyTree(secondWithoutChildren).ToString();

                StringBuilder sb = new StringBuilder();
                if (firstHierarchyTreeString == secondHierarchyTreeString)
                {
                    sb.Append("Both had same hierarchy tree (so they must differ in a way that does not affect their textual representation):");
                    sb.Append(firstHierarchyTreeString);
                }
                else
                {
                    // difference is apparent from hierarchy, so no 
                    sb.AppendLine($"First:");
                    sb.AppendLine(firstHierarchyTreeString);
                    sb.AppendLine("");
                    sb.AppendLine($"Second:");
                    sb.AppendLine(secondHierarchyTreeString);
                }
                comparison = sb.ToString();
                return false;
            }
            else
            {
                comparison = null;
                return true;
            }
        }

        /// <summary>
        /// Enumerates all nodes in the hierarchy that are or have been dirty, walking through the parts of the hierarchy that are dirty or have dirty descendants.
        /// </summary>
        /// <returns>The dirty nodes</returns>
        public static IEnumerable<ILazinator> GetDirtyNodes(this ILazinator startNode, bool restrictToCurrentlyDirty)
        {
            if (restrictToCurrentlyDirty)
                return startNode.EnumerateLazinatorNodes(x => x.IsDirty, false, x => x.IsDirty || x.DescendantIsDirty, true, false);
            return startNode.EnumerateLazinatorNodes(x => x.HasChanged, false, x => x.HasChanged || x.DescendantHasChanged, true, false);
        }

        /// <summary>
        /// Gets a hierarchy of ancestors of this node, excluding this node, to the highest reachable point in the hierarchy.
        /// Because descendants of structs do not contain references to those structs, if a struct is reached, that will always
        /// count as the top of the hierarchy, even where a struct may be a child of another node.
        /// If the node is part of multiple hierarchies, then its parent will be considered the most recent added.
        /// </summary>
        /// <param name="startNode">The starting point, which is not included in the hierarchy.</param>
        /// <returns>The chain of nodes from the starting point to the top</returns>
        public static List<ILazinator> GetClassAncestorsToTop(this ILazinator startNode)
        {
            List<ILazinator> currentList = new List<ILazinator>();
            while (startNode.LazinatorParents.Any())
            {
                startNode = startNode.LazinatorParents.LastAdded;
                currentList.Add(startNode);
            }
            return currentList;
        }

        /// <summary>
        /// Gets a list of nodes from the highest reachable point in the hierarchy to but excluding this node.
        /// </summary>
        /// <param name="destinationNode">The node that is below the hierarchy to be returned</param>
        /// <returns>The chain of nodes from the top to this node</returns>
        public static List<ILazinator> GetClassAncestorsFromTop(this ILazinator destinationNode)
        {
            var l = GetClassAncestorsToTop(destinationNode);
            l.Reverse();
            return l;
        }

        /// <summary>
        /// Marks all Lazinator objects in a hierarchy as clean, so that IsDirty and HasChanged will be false for every object in the hierarchy. This is useful when changes to a node have been persisted to some external store but the node remains in memory.
        /// </summary>
        /// <param name="hierarchy">The node containing the top of the hierarchy to mark as clean</param>
        public static void MarkHierarchyClean(this ILazinator hierarchy)
        {
            hierarchy.EnsureLazinatorMemoryUpToDate(); // we must actually convert it to bytes -- if we just mark things clean, then that will be misleading, and further serialization will be incorrect
            MarkHierarchyUnchanged(hierarchy);
        }

        /// <summary>
        /// Marks all Lazinator objects in a hierarchy as having not been changed. This has no effect on whether the objects are marked as currently dirty.
        /// </summary>
        /// <param name="hierarchy">The node containing the top of the hierarchy to mark as unchanged</param>
        public static void MarkHierarchyUnchanged(this ILazinator hierarchy)
        {
            if (hierarchy != null)
                hierarchy.ForEachLazinator(
                    node =>
                    {
                        node.HasChanged = false;
                        node.DescendantHasChanged = false;
                        return node;
                    },
                    true);
        }

        public static bool DescendantDirtinessIsConsistent(this ILazinator startPoint)
        {

            if (startPoint != null && startPoint.IsDirty)
            {
                ILazinator parent = startPoint.LazinatorParents.LastAdded;
                int levels = 0;
                const int maxLevels = 1000;
                while (parent != null)
                {
                    if (!parent.DescendantIsDirty && !parent.IsDirty)
                    {
                        return false;
                    }
                    parent = parent.LazinatorParents.LastAdded;
                    levels++;
                    if (levels > maxLevels)
                        throw new Exception("Hierarchy inconsistency error. The hierarchy appears to be circular. The root of the hierarchy should have LazinatorParents set to null.");
                }
            }
            return true;
        }

        public static void ConfirmDescendantDirtinessConsistency(this ILazinator startPoint)
        {
            if (!DescendantDirtinessIsConsistent(startPoint))
            {
                throw new Exception("Hierarchy inconsistency error. The ancestor of a dirty node does not have descendant is dirty set.");
            }
        }

        #endregion

        #region Cloning

        /// <summary>
        /// Clones a Lazinator object, returning the object as its own type.
        /// </summary>
        /// <typeparam name="T">The type of the Lazinator object</typeparam>
        /// <param name="lazinator">The lazinator object</param>
        /// <param name="includeChildrenMode">Whether some or all children should be included</param>
        /// <param name="cloneBufferOptions">How the clone's buffer should relate to the original's</param>
        /// <returns>A clone of the Lazinator object</returns>
        public static T CloneLazinatorTyped<T>(this T lazinator, IncludeChildrenMode includeChildrenMode = IncludeChildrenMode.IncludeAllChildren, CloneBufferOptions cloneBufferOptions = CloneBufferOptions.LinkedBuffer) where T : ILazinator
        {
            if (EqualityComparer<T>.Default.Equals(lazinator, default(T)))
                return default(T);
            T clone = (T)lazinator.CloneLazinator(includeChildrenMode, cloneBufferOptions);
            return clone;
        }

        /// <summary>
        /// Updates the Lazinator's memory storage and then copies this to an array.
        /// </summary>
        /// <returns></returns>
        public static byte[] CopyToArray(this ILazinator lazinator)
        {
            lazinator.EnsureLazinatorMemoryUpToDate();
            byte[] array = new byte[lazinator.LazinatorMemoryStorage.Length];
            lazinator.LazinatorMemoryStorage.Memory.CopyTo(array);
            return array;
        }

        /// <summary>
        /// Serializes the Lazinator to an array without updating the Lazinator's memory storage.
        /// </summary>
        /// <returns></returns>
        public static byte[] SerializeToArray(this ILazinator lazinator)
        {
            LazinatorMemory memory = lazinator.SerializeLazinator(IncludeChildrenMode.IncludeAllChildren, false, false);
            byte[] array = new byte[memory.Length];
            memory.Memory.CopyTo(array);
            memory.Dispose();
            return array;
        }

        #endregion

        #region Memory management

        /// <summary>
        /// The Lazinator memory pool. By default, this is set to the default shared memory pool, but this can be 
        /// changed on application startup, resulting in changing the memory pool to be used for all Lazinator objects.
        /// </summary>
        public static MemoryPool<byte> LazinatorMemoryPool = MemoryPool<byte>.Shared;

        /// <summary>
        /// Rents a memory buffer. This can be returned by calling Dispose() on the memory. 
        /// </summary>
        /// <param name="minimumSize"></param>
        /// <returns></returns>
        public static IMemoryOwner<byte> GetRentedMemory(int minimumSize)
        {
            // The following code could alternatively be used if array pooling is not needed: return new SimpleMemoryOwner<byte>(new byte[minimumSize * 2]);
            var toRent = LazinatorMemoryPool.Rent(minimumSize);
            return toRent;
        }

        /// <summary>
        /// Returns a slice of serialized bytes corresponding to a child, excluding the length prefix.
        /// </summary>
        /// <param name="serializedBytes">The serialized bytes for the parent object</param>
        /// <param name="byteOffset">The byte offset into the parent object of the length prefix for the child object</param>
        /// <param name="byteLength">The byte length of the child, including the length prefix</param>
        /// <param name="lengthInSingleByte">Indicates that only one byte of the serialized bytes is used to store the object</param>
        /// <param name="fixedLength"The fixed length of the child, if the length is not included in the serialized bytes
        /// <returns></returns>
        public static LazinatorMemory GetChildSlice(LazinatorMemory serializedBytes, int byteOffset, int byteLength, bool omitLength, bool lengthInSingleByte, int? fixedLength)
        {
            if (serializedBytes == null || byteLength == 0)
            {
                return EmptyLazinatorMemory;
            }
            if (omitLength) // length is omitted because the child takes up the full slice
                return serializedBytes.Slice(byteOffset, byteLength);
            if (fixedLength != null)
                return serializedBytes.Slice(byteOffset, (int)fixedLength);
            if (lengthInSingleByte)
                return serializedBytes.Slice(byteOffset + sizeof(byte), byteLength - sizeof(byte));
            return serializedBytes.Slice(byteOffset + sizeof(int), byteLength - sizeof(int));
        }
        

        /// <summary>
        /// Fully deserialize the lazinator at this node and below, and return the Lazinator object (or a copy if it is a struct).
        /// </summary>
        /// <param name="lazinator">The Lazinator node to deserialize</param>
        /// <returns></returns>
        public static ILazinator FullyDeserialize(this ILazinator lazinator)
        {
            if (lazinator == null)
                return null;
            lazinator = lazinator.ForEachLazinator(l =>
            {
                return l;
            }, false);
            return lazinator;
        }

        /// <summary>
        /// Removes a buffer from this Lazinator node and all its deserialized descendants
        /// </summary>
        /// <param name="lazinator">The Lazinator node</param>
        /// <returns>The node with the buffer removed (or a copy if a Lazinator struct)</returns>
        public static ILazinator RemoveBufferInHierarchy(this ILazinator lazinator)
        {
            if (lazinator == null)
                return null;
            lazinator = lazinator.FullyDeserialize();
            return lazinator.ForEachLazinator(l => RemoveBuffer_Helper(l), true);
        }

        /// <summary>
        /// Removes a buffer from this Lazinator node, without affecting its children. This should not be done if any other node might still need the buffer.
        /// </summary>
        /// <param name="lazinator">The Lazinator node</param>
        /// <returns>The node with the buffer removed (or a copy if a Lazinator struct)</returns>
        private static ILazinator RemoveBuffer_Helper(this ILazinator lazinator)
        {
            if (lazinator == null)
                return null;
            var existingBuffer = lazinator.LazinatorMemoryStorage;
            lazinator.LazinatorMemoryStorage = null;
            if (existingBuffer != null)
                existingBuffer.Dispose();
            return lazinator;
        }

        /// <summary>
        /// Get a MemoryStream from ReadOnlyMemory
        /// </summary>
        /// <param name="memory">The source memory</param>
        /// <returns>The resulting memory stream</returns>
        public static MemoryStream GetMemoryStream(this ReadOnlyMemory<byte> memory)
        {
            MemoryStream stream = new MemoryStream();
            stream.Write(memory.Span);
            stream.Flush();
            stream.Position = 0;
            return stream;
        }

        /// <summary>
        /// Get a memory stream containing the lazinator object.
        /// </summary>
        /// <param name="lazinator"></param>
        /// <returns></returns>
        public static MemoryStream GetMemoryStream(this ILazinator lazinator)
        {
            lazinator.EnsureLazinatorMemoryUpToDate();
            return lazinator.LazinatorMemoryStorage.ReadOnlyMemory.GetMemoryStream();
        }

        /// <summary>
        /// Gets a pipe containing the Lazinator object. 
        /// </summary>
        /// <param name="lazinator"></param>
        /// <returns></returns>
        public static (Pipe pipe, int bytes) GetPipe(this ILazinator lazinator)
        {
            lazinator.EnsureLazinatorMemoryUpToDate();
            Pipe pipe = new Pipe();
            AddToPipe(lazinator, pipe);
            pipe.Writer.Complete();
            return (pipe, lazinator.LazinatorMemoryStorage.Length);
        }

        /// <summary>
        /// Writes a Lazinator object into a pipe.
        /// </summary>
        /// <param name="lazinator"></param>
        /// <param name="pipe"></param>
        public static void AddToPipe(this ILazinator lazinator, Pipe pipe)
        {
            pipe.Writer.Write(lazinator.LazinatorMemoryStorage.Span);
        }

        /// <summary>
        /// Creates a new byte array representing the contents of a stream.
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static byte[] ToByteArray(this Stream stream)
        {
            stream.Position = 0;
            byte[] buffer = new byte[stream.Length];
            int streamLength = Convert.ToInt32(stream.Length);
            for (int totalBytesCopied = 0; totalBytesCopied < streamLength;)
                totalBytesCopied += stream.Read(buffer, totalBytesCopied, streamLength - totalBytesCopied);
            return buffer;
        }

        #endregion

        #region Hashing

        public static uint GetBinaryHashCode32(this ILazinator lazinator)
        {
            if (lazinator.NonBinaryHash32)
                return (uint) lazinator.GetHashCode();
            if (!lazinator.IsDirty && !lazinator.DescendantIsDirty && lazinator.OriginalIncludeChildrenMode == IncludeChildrenMode.IncludeAllChildren && lazinator.LazinatorMemoryStorage != null && lazinator.LazinatorMemoryStorage.Disposed == false)
                return FarmhashByteSpans.Hash32(lazinator.LazinatorMemoryStorage.Memory.Span);
            else
            {
                LazinatorMemory serialized =
                    lazinator.SerializeLazinator(IncludeChildrenMode.IncludeAllChildren, false, false);
                var result = FarmhashByteSpans.Hash32(serialized.ReadOnlySpan);
                serialized.Dispose();
                return result;
            }
        }

        public static ulong GetBinaryHashCode64(this ILazinator lazinator)
        {
            if (!lazinator.IsDirty && !lazinator.DescendantIsDirty && lazinator.OriginalIncludeChildrenMode == IncludeChildrenMode.IncludeAllChildren && lazinator.LazinatorMemoryStorage != null && lazinator.LazinatorMemoryStorage.Disposed == false)
                return FarmhashByteSpans.Hash64(lazinator.LazinatorMemoryStorage.Memory.Span);
            else
            {
                LazinatorMemory serialized =
                    lazinator.SerializeLazinator(IncludeChildrenMode.IncludeAllChildren, false, false);
                var result = FarmhashByteSpans.Hash64(serialized.ReadOnlySpan);
                serialized.Dispose();
                return result;
            }
        }

        public static Guid GetBinaryHashCode128(this ILazinator lazinator)
        {
            if (!lazinator.IsDirty && !lazinator.DescendantIsDirty && lazinator.OriginalIncludeChildrenMode == IncludeChildrenMode.IncludeAllChildren && lazinator.LazinatorMemoryStorage != null && lazinator.LazinatorMemoryStorage.Disposed == false)
                return FarmhashByteSpans.Hash128(lazinator.LazinatorMemoryStorage.Memory.Span);
            else
            {
                LazinatorMemory serialized =
                    lazinator.SerializeLazinator(IncludeChildrenMode.IncludeAllChildren, false, false);
                var result = FarmhashByteSpans.Hash128(serialized.ReadOnlySpan);
                serialized.Dispose();
                return result;
            }
        }

        #endregion


    }
}
