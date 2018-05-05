using System;
using System.Buffers;
using System.IO;
using System.Text;
using Lazinator.Buffers; 
using Lazinator.Core;
using Lazinator.Exceptions;
using Lazinator.Support;

namespace Lazinator.Core
{
    public static class LazinatorUtilities
    {
        // Delegate types. Methods matching these types must be passed into some of the methods below.

        public delegate void InformParentOfDirtinessDelegate();

        public delegate MemoryInBuffer StreamManuallyDelegate(IncludeChildrenMode includeChildrenMode, bool verifyCleanness);

        public delegate ReadOnlyMemory<byte> ReturnReadOnlyMemoryDelegate();

        public delegate void WriteDelegate(BinaryBufferWriter writer);

        public delegate void WritePossiblyVerifyingCleannessDelegate(BinaryBufferWriter writer, bool verifyCleanness);

        /// <summary>
        /// Serializes from the top of the hierarchy, using the original storage if the item is not dirty and does not need its cleanness verified or, if dirty, creating a stream to manually serialize. 
        /// </summary>
        /// <param name="includeChildrenMode">Includes children (and thus descendants) when converting to bytes.</param>
        /// <param name="originalIncludeChildrenMode">The original mode used to serialize this object.</param>
        /// <param name="allowRecycleOfOriginalStorage">If true, the original storage may be returned when the object is cleaned.</param>
        /// <param name="verifyCleanness">If true, then the dirty-conversion will always be performed unless we are sure it is clean, and if the object is not believed to be dirty, the results will be compared to the clean version. This allows for errors from failure to serialize objects that have been changed to be caught during development. Set this to false if you may wish to dispose of the memory backing the original while still using the new deserialized bytes.</param>
        /// <param name="isBelievedDirty">An indication of whether the object to be converted to bytes is believed to be dirty, i.e. has had its dirty flag set.</param>
        /// <param name="isDefinitelyClean">An indication whether any storage, if it exists, is definitely clean. If the storage has never been converted into bytes, then it is definitely clean. If the storage does not exist (it hasn't been serialized yet), then this is irrelevant, because there is no need to verify cleanliness.</param>
        /// <param name="originalStorage">The storage of the item before any changes were made to it</param>
        /// <param name="streamManually_Fn">The function that completes the conversion to bytes, without considering using the original storage for the item as a whole.</param>
        /// <returns></returns>
        public static MemoryInBuffer EncodeOrRecycleToNewBuffer(IncludeChildrenMode includeChildrenMode, IncludeChildrenMode originalIncludeChildrenMode, bool allowRecycleOfOriginalStorage, bool verifyCleanness, bool isBelievedDirty, bool descendantIsBelievedDirty, bool isDefinitelyClean, ReadOnlyMemory<byte> originalStorage, StreamManuallyDelegate streamManually_Fn)
        {
            // if item has never been serialized before, there will be no storage, so we must convert to bytes.
            // we also must convert to bytes if we have to verify cleanness or if this is believed to be dirty,
            // unless the original storage is definitely clean.
            if (originalStorage.Length == 0 || 
                includeChildrenMode != originalIncludeChildrenMode ||
                        (!isDefinitelyClean 
                            && 
                        (verifyCleanness || 
                        isBelievedDirty || 
                        (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && descendantIsBelievedDirty)
                         )
                         )
            )
                return streamManually_Fn(includeChildrenMode, verifyCleanness);

            // We can use the original storage. But we still have to copy it. 
            BinaryBufferWriter writer = new BinaryBufferWriter(originalStorage.Length);
            writer.Write(originalStorage.Span);
            return writer.MemoryInBuffer;
        }

        public static MemoryInBuffer EncodeToNewBinaryBufferWriter<T>(T selfSerialized, IncludeChildrenMode includeChildrenMode, bool verifyCleanness) where T : ILazinator
        {
            BinaryBufferWriter writer = new BinaryBufferWriter(BinaryBufferWriter.MinMinBufferSize);
            selfSerialized.SerializeExistingBuffer(writer, includeChildrenMode, verifyCleanness);
            return writer.MemoryInBuffer;
        }

        /// <summary>
        /// Initiates the convertion to binary of a non-lazinator object. 
        /// </summary>
        /// <param name="nonLazinatorObject">An object that does not implement ILazinator</param>
        /// <param name="isBelievedDirty">An indication of whether the object to be converted to bytes is believed to be dirty, i.e. has had its dirty flag set.</param>
        /// <param name="isAccessed">An indication of whether the object has been accessed.</param>
        /// <param name="writer">The binary writer</param>
        /// <param name="getChildSliceForFieldFn"></param>
        /// <param name="verifyCleanness">If true, then the dirty-conversion will always be performed unless we are sure it is clean, and if the object is not believed to be dirty, the results will be compared to the clean version. This allows for errors from failure to serialize objects that have been changed to be caught during development.</param>
        /// <param name="binaryWriterAction"></param>
        public static void WriteNonLazinatorObject(object nonLazinatorObject,
            bool isBelievedDirty, bool isAccessed, BinaryBufferWriter writer, ReturnReadOnlyMemoryDelegate getChildSliceForFieldFn,
            bool verifyCleanness, WritePossiblyVerifyingCleannessDelegate binaryWriterAction)
        {
            ReadOnlyMemory<byte> original = getChildSliceForFieldFn();
            if (!isAccessed)
            {
                // object has never been loaded into memory, so there is no need to verify cleanness
                // just return what we have.
                original.Span.Write_WithUintLengthPrefix(writer);
            }
            else if (isBelievedDirty || original.Length == 0)
            {
                // We definitely need to write to binary, because either the dirty flag has been set or the original storage doesn't have anything to help us.
                void action(BinaryBufferWriter w) => binaryWriterAction(w, verifyCleanness);
                WriteToBinaryWithUintLengthPrefix(writer, action);
            }
            else
            {
                if (verifyCleanness)
                {
                    ReadOnlyMemory<byte> revised = CreateStreamForNonLazinatorObject(nonLazinatorObject, binaryWriterAction);
                    ConfirmMatch(original, revised);
                }
                original.Span.Write_WithUintLengthPrefix(writer);
            }
        }

        /// <summary>
        /// Completes an action to write to binary, but then prefixes the binary writer with the total length of what was written, excluding the length itself
        /// </summary>
        /// <param name="writer">The binary writer</param>
        /// <param name="action">The action to complete</param>
        public static void WriteToBinaryWithUintLengthPrefix(BinaryBufferWriter writer, WriteDelegate action)
        {
            int lengthPosition = writer.Position;
            writer.Write((uint)0);
            action(writer);
            int afterPosition = writer.Position;
            writer.Position = lengthPosition;
            int length = (afterPosition - lengthPosition - sizeof(uint));
            writer.Write(length);
            writer.Position = afterPosition; 
        }

        /// <summary>
        /// Completes an action to write to binary, but then prefixes the binary writer with the total length of what was written, excluding the length itself
        /// </summary>
        /// <param name="writer">The binary writer</param>
        /// <param name="action">The action to complete</param>
        public static void WriteToBinaryWithByteLengthPrefix(BinaryBufferWriter writer, WriteDelegate action)
        {
            int lengthPosition = writer.Position;
            writer.Write((byte)0);
            action(writer);
            int afterPosition = writer.Position;
            writer.Position = lengthPosition;
            int length = (afterPosition - lengthPosition - sizeof(byte));
            writer.Write(length);
            writer.Position = afterPosition;
        }

        /// <summary>
        /// Initiates a binary write to a child of a self-serialized object, without any length information. 
        /// </summary>
        /// <param name="writer">The binary writer</param>
        /// <param name="child">The child</param>
        /// <param name="childHasBeenAccessed">True if the child's value has been accessed.</param>
        /// <param name="getChildSliceFn">A function to return the child's original storage</param>
        /// <param name="verifyCleanness">If true, cleanness of any nonserialized fields in the child will be verified if necessary</param>
        public static void WriteChildWithoutLength<T>(BinaryBufferWriter writer, T child, IncludeChildrenMode includeChildrenMode, bool childHasBeenAccessed, ReturnReadOnlyMemoryDelegate getChildSliceFn, bool verifyCleanness) where T : ILazinator
        {
            if (!childHasBeenAccessed && child != null)
                childHasBeenAccessed = true; // child is an uninitialized struct
            if (!childHasBeenAccessed && includeChildrenMode == IncludeChildrenMode.IncludeAllChildren)
            {
                ReadOnlyMemory<byte> childStorage = getChildSliceFn(); // this is the storage holding the child, which has never been accessed
                childStorage.Span.Write(writer);
            }
            else
            {
                if (child == null)
                    return; // child has been changed to null -- skip this item
                else
                {
                    if (child.IsDirty || verifyCleanness || child.LazinatorObjectBytes.Length == 0)
                        child.SerializeExistingBuffer(writer, includeChildrenMode, verifyCleanness);
                    else
                        child.LazinatorObjectBytes.Span.Write(writer); // the child has been accessed, but is unchanged, so we can use the storage holding the child
                }
            }
        }

        /// <summary>
        /// Initiates a binary write to a child of a self-serialized object, including a length prefix
        /// </summary>
        /// <param name="writer">The binary writer</param>
        /// <param name="child">The child</param>
        /// <param name="includeChildrenMode"></param>
        /// <param name="childHasBeenAccessed">True if the child's value has been accessed.</param>
        /// <param name="getChildSliceFn">A function to return the child's original storage</param>
        /// <param name="verifyCleanness">If true, cleanness of any nonserialized fields in the child will be verified if necessary</param>
        /// <param name="restrictLengthTo250Bytes"></param>
        public static void WriteChildWithLength<T>(BinaryBufferWriter writer, T child,
            IncludeChildrenMode includeChildrenMode, bool childHasBeenAccessed,
            ReturnReadOnlyMemoryDelegate getChildSliceFn, bool verifyCleanness, bool restrictLengthTo250Bytes) where T : ILazinator
        {
            if (!childHasBeenAccessed && child != null)
                childHasBeenAccessed = true; // child is an uninitialized struct
            if (!childHasBeenAccessed && includeChildrenMode == IncludeChildrenMode.IncludeAllChildren)
            {
                // The child is null, not because it was set to null, but because it was never accessed. Thus, we need to use the last version from storage (or just to store a zero-length if this is the first time saving it).
                ReadOnlyMemory<byte> childStorage = getChildSliceFn(); // this is the storage holding the child, which has never been accessed
                if (restrictLengthTo250Bytes)
                    childStorage.Span.Write_WithByteLengthPrefix(writer);
                else
                    childStorage.Span.Write_WithUintLengthPrefix(writer);
            }
            else
            {
                if (child == null)
                    writer.Write((uint) 0); // child has been changed to null
                else
                {
                    void action(BinaryBufferWriter w)
                    {
                        if (child.IsDirty || verifyCleanness || child.LazinatorObjectBytes.Length == 0)
                            child.SerializeExistingBuffer(w, includeChildrenMode, verifyCleanness);
                        else
                            child.LazinatorObjectBytes.Span.Write(w); // the child has been accessed, but is unchanged, so we can use the storage holding the child
                    }
                    if (restrictLengthTo250Bytes)
                        LazinatorUtilities.WriteToBinaryWithByteLengthPrefix(writer, action);
                    else
                        LazinatorUtilities.WriteToBinaryWithUintLengthPrefix(writer, action);
                }
            }
        }

        /// <summary>
        /// Converts a non-Lazinator property to bytes by creating a stream and calling an action that accepts a binary writer as a parameter.
        /// This is used when it is necessary to verify the cleanness of a non-Lazinator property.
        /// </summary>
        /// <param name="nonLazinatorObject">The object to be converted</param>
        /// <param name="binaryWriterAction">The method that uses a binary writer to write the bytes for the non self-serialized fields. The second parameter will be ignored.</param>
        /// <returns></returns>
        public static ReadOnlyMemory<byte> CreateStreamForNonLazinatorObject(object nonLazinatorObject, WritePossiblyVerifyingCleannessDelegate binaryWriterAction)
        {
            if (nonLazinatorObject == null)
                return new ReadOnlyMemory<byte>();

            using (BinaryBufferWriter writer = new BinaryBufferWriter())
            {
                binaryWriterAction(writer, true);
                return writer.MemoryInBuffer.FilledMemory;
            }
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

        /// <summary>
        /// Returns a slice of serialized bytes corresponding to a child, excluding the length prefix.
        /// </summary>
        /// <param name="serializedBytes">The serialized bytes for the parent object</param>
        /// <param name="byteOffset">The byte offset into the parent object of the length prefix for the child object</param>
        /// <param name="byteLength">The byte length of the child, including the length prefix</param>
        /// <returns></returns>
        public static ReadOnlyMemory<byte> GetChildSlice(ReadOnlyMemory<byte> serializedBytes, int byteOffset, int byteLength)
        {
            if (byteLength <= sizeof(int))
            {
                return new ReadOnlyMemory<byte>();
            }
            return serializedBytes.Slice(byteOffset + sizeof(int), byteLength - sizeof(int));
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

        /// <summary>
        /// Clones a Lazinator class, returning the object as its own type. Thsi cannot be used for Lazinator structs.
        /// </summary>
        /// <typeparam name="T">The type of the Lazinator object</typeparam>
        /// <param name="lazinator">The lazinator object</param>
        /// <returns>A clone of the Lazinator class</returns>
        public static T CloneLazinatorTyped<T>(this T lazinator) where T : class, ILazinator
        {
            return lazinator.CloneLazinator() as T;
        }

        /// <summary>
        /// Clones a Lazinator class, returning the object as its own type, with an option to exclude children. Thsi cannot be used for Lazinator structs.
        /// </summary>
        /// <typeparam name="T">The type of the Lazinator object</typeparam>
        /// <param name="lazinator">The lazinator object</param>
        /// <param name="includeChildrenMode">Whether to include children of the class being cloned.</param>
        /// <returns>A clone of the Lazinator class</returns>
        public static T CloneLazinatorTyped<T>(this T lazinator, IncludeChildrenMode includeChildrenMode) where T : class, ILazinator
        {
            return lazinator.CloneLazinator(includeChildrenMode) as T;
        }

        /// <summary>
        /// Returns a rented memory buffer. This will be disposed when Dispose() is called on the object containing it.
        /// </summary>
        /// <param name="minimumSize"></param>
        /// <returns></returns>
        public static IMemoryOwner<byte> GetRentedMemory(int minimumSize)
        {
            return MemoryPool<byte>.Shared.Rent(minimumSize);
        }
        
    }
}
