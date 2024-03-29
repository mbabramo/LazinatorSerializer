using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Lazinator.Buffers;
using Lazinator.Exceptions;
using Lazinator.Support;
using static System.Buffers.Binary.BinaryPrimitives;

namespace Lazinator.Core
{
    /// <summary>
    /// Offers a variety of utilities, many of which are used internally by Lazinator code-behind.
    /// </summary>
    public static class LazinatorUtilities
    {
        #region Delegate types

        // Delegate types. Methods matching these types must be passed into some of the methods below.

        public delegate LazinatorMemory ReturnLazinatorMemoryDelegate();
        public delegate ValueTask<LazinatorMemory> ReturnLazinatorMemoryDelegateAsync();

        public delegate void WriteDelegate(ref BufferWriter writer);
        public delegate ValueTask WriteDelegateAsync(BufferWriterContainer writer);

        public delegate void WritePossiblyVerifyingCleannessDelegate(ref BufferWriter writer, bool verifyCleanness);

        public delegate ValueTask WritePossiblyVerifyingCleannessDelegateAsync(BufferWriterContainer writer, bool verifyCleanness);

        #endregion

        #region Encoding

        /// <summary>
        /// Serializes from the top of the hierarchy, using the original storage if the item is not dirty and does not need its cleanness verified or, if dirty, creating a stream to manually serialize. 
        /// </summary>
        /// <param name="includeChildrenMode">Includes children (and thus descendants) when converting to bytes.</param>
        /// <param name="originalIncludeChildrenMode">The original mode used to serialize this object.</param>
        /// <param name="isBelievedDirty">An indication of whether the object to be converted to bytes is believed to be dirty, i.e. has had its dirty flag set.</param>
        /// <param name="isDefinitelyClean">An indication whether any storage, if it exists, is definitely clean. If the storage has never been converted into bytes, then it is definitely clean. If the storage does not exist (it hasn't been serialized yet), then this is irrelevant, because there is no need to verify cleanliness.</param>
        /// <param name="originalStorage">The storage of the item before any changes were made to it</param>
        /// <param name="lazinator">A Lazinator class (for a struct, the other overload of this function should be called).</param>
        /// <returns></returns>
        public static LazinatorMemory EncodeOrRecycleToNewBuffer<T>(IncludeChildrenMode includeChildrenMode, IncludeChildrenMode originalIncludeChildrenMode, bool isBelievedDirty, bool descendantIsBelievedDirty, bool isDefinitelyClean, LazinatorMemory originalStorage, T lazinator) where T : ILazinator
        {
            // if item has never been serialized before, there will be no storage, so we must convert to bytes.
            // we also must convert to bytes if we have to verify cleanness or if this is believed to be dirty,
            // unless the original storage is definitely clean.
            if (originalStorage.IsEmpty ||
                includeChildrenMode != originalIncludeChildrenMode ||
                    (!isDefinitelyClean
                        &&
                        (
                        isBelievedDirty ||
                        (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && descendantIsBelievedDirty)
                        )
                     )
                )
                return EncodeToNewBufferWriter(lazinator, includeChildrenMode);

            // We can use the original storage. But we still have to copy it into a new buffer, as requested.
            BufferWriter writer = new BufferWriter(GetBufferSize(originalStorage.Length));
            originalStorage.WriteToBuffer(ref writer);
            return writer.LazinatorMemory;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int GetBufferSize(long ultimatelyRequiredLength) => ultimatelyRequiredLength > int.MaxValue || ultimatelyRequiredLength == 0 ? ExpandableBytes.DefaultMinBufferSize : (int)ultimatelyRequiredLength;

        private static LazinatorMemory EncodeToNewBufferWriter<T>(T lazinatorObject, IncludeChildrenMode includeChildrenMode) where T : ILazinator
        {
            int bufferSize = GetBufferSize(lazinatorObject.LazinatorMemoryStorage.Length);
            BufferWriter writer = new BufferWriter(bufferSize);
            lazinatorObject.SerializeToExistingBuffer(ref writer, new LazinatorSerializationOptions(includeChildrenMode, false, false, false));
            return writer.LazinatorMemory;
        }

        /// <summary>
        /// Serializes from the top of the hierarchy, using the original storage if the item is not dirty and does not need its cleanness verified or, if dirty, creating a stream to manually serialize. 
        /// </summary>
        /// <param name="includeChildrenMode">Includes children (and thus descendants) when converting to bytes.</param>
        /// <param name="originalIncludeChildrenMode">The original mode used to serialize this object.</param>
        /// <param name="isBelievedDirty">An indication of whether the object to be converted to bytes is believed to be dirty, i.e. has had its dirty flag set.</param>
        /// <param name="isDefinitelyClean">An indication whether any storage, if it exists, is definitely clean. If the storage has never been converted into bytes, then it is definitely clean. If the storage does not exist (it hasn't been serialized yet), then this is irrelevant, because there is no need to verify cleanliness.</param>
        /// <param name="originalStorage">The storage of the item before any changes were made to it</param>
        /// <param name="lazinator">A Lazinator class (for a struct, the other overload of this function should be called).</param>
        /// <returns></returns>
        public async static ValueTask<LazinatorMemory> EncodeOrRecycleToNewBufferAsync<T>(IncludeChildrenMode includeChildrenMode, IncludeChildrenMode originalIncludeChildrenMode, bool isBelievedDirty, bool descendantIsBelievedDirty, bool isDefinitelyClean, LazinatorMemory originalStorage, T lazinator) where T : ILazinator, ILazinatorAsync
        {
            if (originalStorage.IsEmpty || 
                includeChildrenMode != originalIncludeChildrenMode ||
                    (!isDefinitelyClean
                        &&
                        (isBelievedDirty ||
                        (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && descendantIsBelievedDirty)
                        )
                     )
                )
                return await EncodeToNewBufferWriterAsync(lazinator, includeChildrenMode);

            // We can use the original storage. But we still have to copy it into a new buffer, as requested.
            BufferWriterContainer writer = new BufferWriterContainer(GetBufferSize(originalStorage.Length));
            await originalStorage.WriteToBufferAsync(writer);
            return writer.LazinatorMemory;
        }

        private async static ValueTask<LazinatorMemory> EncodeToNewBufferWriterAsync<T>(T lazinatorObject, IncludeChildrenMode includeChildrenMode) where T : ILazinator, ILazinatorAsync
        {
            int bufferSize = GetBufferSize(lazinatorObject.LazinatorMemoryStorage.Length);
            BufferWriterContainer writer = new BufferWriterContainer(bufferSize);
            await lazinatorObject.SerializeToExistingBufferAsync(writer, new LazinatorSerializationOptions(includeChildrenMode, false, false, false));
            return writer.LazinatorMemory;
        }

        /// <summary>
        /// Completes an action to write to binary, but then prefixes the binary writer with the total length of what was written, excluding the length itself
        /// </summary>
        /// <param name="writer">The binary writer</param>
        /// <param name="action">The action to complete</param>
        public static void WriteToBinaryWithInt16LengthPrefix(ref BufferWriter writer, WriteDelegate action)
        {
            int lengthPosition = writer.ActiveMemoryPosition;
            writer.Write((Int16)0);
            action(ref writer);
            int afterPosition = writer.ActiveMemoryPosition;
            writer.ActiveMemoryPosition = lengthPosition;
            Int32 length = (afterPosition - lengthPosition - sizeof(Int16));
            if (length > Int16.MaxValue)
                ThrowHelper.ThrowTooLargeException(Int16.MaxValue);
            writer.Write((Int16) length);
            writer.ActiveMemoryPosition = afterPosition;
        }
        /// <summary>
        /// Completes an action to write to binary, but then prefixes the binary writer with the total length of what was written, excluding the length itself
        /// </summary>
        /// <param name="writer">The binary writer</param>
        /// <param name="action">The action to complete</param>
        public static void WriteToBinaryWithInt32LengthPrefix(ref BufferWriter writer, WriteDelegate action)
        {
            int lengthPosition = writer.ActiveMemoryPosition;
            writer.Write((Int32)0);
            action(ref writer);
            int afterPosition = writer.ActiveMemoryPosition;
            writer.ActiveMemoryPosition = lengthPosition;
            long length = (afterPosition - lengthPosition - sizeof(uint));
            if (length > Int32.MaxValue)
                ThrowHelper.ThrowTooLargeException(Int32.MaxValue);
            writer.Write((Int32)length);
            writer.ActiveMemoryPosition = afterPosition;
        }

        /// <summary>
        /// Completes an action to write to binary, but then prefixes the binary writer with the total length of what was written, excluding the length itself
        /// </summary>
        /// <param name="writer">The binary writer</param>
        /// <param name="action">The action to complete</param>
        public async static ValueTask WriteToBinaryWithInt16LengthPrefixAsync(BufferWriterContainer writer, WriteDelegateAsync action)
        {
            int lengthPosition = writer.ActiveMemoryPosition;
            writer.Write((Int16)0);
            await action(writer);
            int afterPosition = writer.ActiveMemoryPosition;
            writer.Writer.ActiveMemoryPosition = lengthPosition;
            Int32 length = (Int32) ((afterPosition - lengthPosition - sizeof(Int16)));
            if (length > Int16.MaxValue)
                ThrowHelper.ThrowTooLargeException(Int16.MaxValue);
            writer.Write((Int16) length);
            writer.Writer.ActiveMemoryPosition = afterPosition;
        }
        /// <summary>
        /// Completes an action to write to binary, but then prefixes the binary writer with the total length of what was written, excluding the length itself
        /// </summary>
        /// <param name="writer">The binary writer</param>
        /// <param name="action">The action to complete</param>
        public async static ValueTask WriteToBinaryWithInt32LengthPrefixAsync(BufferWriterContainer writer, WriteDelegateAsync action)
        {
            int lengthPosition = writer.ActiveMemoryPosition;
            writer.Write((int)0);
            await action(writer);
            int afterPosition = writer.ActiveMemoryPosition;
            writer.Writer.ActiveMemoryPosition = lengthPosition;
            long length = (afterPosition - lengthPosition - sizeof(Int32));
            if (length > Int32.MaxValue)
                ThrowHelper.ThrowTooLargeException(Int32.MaxValue);
            writer.Write((Int32) length);
            writer.Writer.ActiveMemoryPosition = afterPosition;
        }

        /// <summary>
        /// Completes an action to write to binary, without any length prefix.
        /// </summary>
        /// <param name="writer">The binary writer</param>
        /// <param name="action">The action to complete</param>
        public static void WriteToBinaryWithoutLengthPrefix(ref BufferWriter writer, WriteDelegate action)
        {
            action(ref writer);
        }

        /// <summary>
        /// Completes an action to write to binary, without any length prefix.
        /// </summary>
        /// <param name="writer">The binary writer</param>
        /// <param name="action">The action to complete</param>
        public async static ValueTask WriteToBinaryWithoutLengthPrefixAsync(BufferWriterContainer writer, WriteDelegateAsync action)
        {
            await action(writer);
        }

        /// <summary>
        /// Completes an action to write to binary, but then prefixes the binary writer with the total length of what was written, excluding the length itself
        /// </summary>
        /// <param name="writer">The binary writer</param>
        /// <param name="action">The action to complete</param>
        public static void WriteToBinaryWithByteLengthPrefix(ref BufferWriter writer, WriteDelegate action)
        {
            int lengthPosition = writer.ActiveMemoryPosition;
            writer.Write((byte)0);
            action(ref writer);
            int afterPosition = writer.ActiveMemoryPosition;
            writer.ActiveMemoryPosition = lengthPosition;
            int length = (afterPosition - lengthPosition - sizeof(byte));
            if (length > byte.MaxValue)
                ThrowHelper.ThrowTooLargeException(byte.MaxValue);
            writer.Write((byte)length);
            writer.ActiveMemoryPosition = afterPosition;
        }

        /// <summary>
        /// Completes an action to write to binary, but then prefixes the binary writer with the total length of what was written, excluding the length itself
        /// </summary>
        /// <param name="writer">The binary writer</param>
        /// <param name="action">The action to complete</param>
        public async static ValueTask WriteToBinaryWithByteLengthPrefixAsync(BufferWriterContainer writer, WriteDelegateAsync action)
        {
            int lengthPosition = writer.ActiveMemoryPosition;
            writer.Write((byte)0);
            await action(writer);
            int afterPosition = writer.ActiveMemoryPosition;
            writer.Writer.ActiveMemoryPosition = lengthPosition;
            int length = (afterPosition - lengthPosition - sizeof(byte));
            if (length > byte.MaxValue)
                ThrowHelper.ThrowTooLargeException(byte.MaxValue);
            writer.Write((byte)length);
            writer.Writer.ActiveMemoryPosition = afterPosition;
        }

        /// <summary>
        /// Writes a byte value to a span, returning true if the span is at least one byte long.
        /// </summary>
        public static bool WriteValueToSpan(Span<byte> targetSpan, byte value) => BufferWriter.LittleEndianStorage ? TryWriteUInt16LittleEndian(targetSpan, (byte)value) : TryWriteUInt16BigEndian(targetSpan, (byte)value);
        /// <summary>
        /// Writes a byte value to a span, returning true if the span is at least two bytes long.
        /// </summary>
        public static bool WriteValueToSpan(Span<byte> targetSpan, ushort value) => BufferWriter.LittleEndianStorage ? TryWriteUInt16LittleEndian(targetSpan, (ushort)value) : TryWriteUInt16BigEndian(targetSpan, (ushort)value);
        /// <summary>
        /// Writes a byte value to a span, returning true if the span is at least four bytes long.
        /// </summary>
        public static bool WriteValueToSpan(Span<byte> targetSpan, uint value) => BufferWriter.LittleEndianStorage ? TryWriteUInt32LittleEndian(targetSpan, (uint)value) : TryWriteUInt32BigEndian(targetSpan, (uint)value);
        /// <summary>
        /// Writes a byte value to a span, returning true if the span is at least eight bytes long.
        /// </summary>
        public static bool WriteValueToSpan(Span<byte> targetSpan, ulong value) => BufferWriter.LittleEndianStorage ? TryWriteUInt64LittleEndian(targetSpan, (ulong)value) : TryWriteUInt64BigEndian(targetSpan, (ulong)value);

        /// <summary>
        /// Initiates a binary write to a child of a Lazinator object, optionally including a length prefix, using existing storage if possible
        /// </summary>
        /// <param name="writer">The binary writer</param>
        /// <param name="child">The child to be written. This child will be written regardless of includeChildrenMode.</param>
        /// <param name="options">Serialization options</param>
        /// <param name="childHasBeenAccessed">True if the child's value has been accessed.</param>
        /// <param name="getChildSliceFn">A function to return the child's original storage</param>
        /// <param name="parent">The parent of the object being written</param>
        /// 
        public static void WriteChild<T>(ref BufferWriter writer, ref T child,
            LazinatorSerializationOptions options, bool childHasBeenAccessed, ReturnLazinatorMemoryDelegate getChildSliceFn, ILazinator parent) where T : ILazinator
        {
            bool childCouldHaveChanged = childHasBeenAccessed || (child != null && options.IncludeChildrenMode != child.OriginalIncludeChildrenMode);
            LazinatorMemory childStorage = default;
            if (!childHasBeenAccessed && child != null)
            {
                childStorage = getChildSliceFn();
                if (childStorage.IsEmpty)
                    childCouldHaveChanged = true; // child might be an uninitialized struct and the object has not been previously deserialized. Thus, we treat this as an object that has been changed, so that we can serialize it. 
            }
            else
            {
                // check for a child that has been accessed (or otherwise could have changed) and that is in memory and totally clean. 
                if (childCouldHaveChanged && child != null && !child.IsDirty && !child.DescendantIsDirty && options.IncludeChildrenMode == IncludeChildrenMode.IncludeAllChildren && options.IncludeChildrenMode == child.OriginalIncludeChildrenMode)
                {
                    // In this case, we update the childStorage to reflect the child's own storage, rather than a slice in the parent's storage. The reason is that the buffer may have been updated if the same object appears more than once in the object hierarchy, or the child may have updated its storage after a manual call to UpdateStoredBuffer.
                    childStorage = child.LazinatorMemoryStorage;
                    if (!childStorage.IsEmpty)
                        childCouldHaveChanged = false;
                }
            }
            if (!childCouldHaveChanged)
            {
                int startPosition = writer.ActiveMemoryPosition;
                childStorage = WriteExistingChildStorage(ref writer, getChildSliceFn, childStorage, options.SerializeDiffs, options.SerializeDiffsThreshold);
                if (options.UpdateStoredBuffer)
                {
                    if (child != null)
                    {
                        long length = childStorage.Length;
                        child.UpdateStoredBuffer(ref writer, startPosition, length, options.IncludeChildrenMode, true);
                    }
                }
            }
            else
            {
                if (child != null)
                {
                    WriteChildToBinary(ref writer, ref child, options);
                }
            }
            AddParentToChildless(ref child, parent);
        }

        /// <summary>
        /// Initiates a binary write to a non-async child of a Lazinator object that has AsyncLazinatorMemory defined, optionally including a length prefix, using existing storage if possible
        /// </summary>
        /// <param name="writer">The binary writer</param>
        /// <param name="child">The child to be written. This child will be written regardless of includeChildrenMode.</param>
        /// <param name="options">Serialization options</param>
        /// <param name="childHasBeenAccessed">True if the child's value has been accessed.</param>
        /// <param name="getChildSliceFn">A function to return the child's original storage</param>
        /// <param name="parent">The parent of the object being written</param>
        /// 
        public async static ValueTask WriteNonAsyncChildAsync<T>(BufferWriterContainer writer, T child,
            LazinatorSerializationOptions options, bool childHasBeenAccessed, ReturnLazinatorMemoryDelegateAsync getChildSliceFn, ILazinator parent) where T : ILazinator
        {
            bool childCouldHaveChanged = childHasBeenAccessed || (child != null && options.IncludeChildrenMode != child.OriginalIncludeChildrenMode);
            LazinatorMemory childStorage = default;
            if (!childHasBeenAccessed && child != null)
            {
                childStorage = await getChildSliceFn();
                if (childStorage.IsEmpty)
                    childCouldHaveChanged = true; // child might be an uninitialized struct and the object has not been previously deserialized. Thus, we treat this as an object that has been changed, so that we can serialize it. 
            }
            else
            {
                // check for a child that has been accessed (or otherwise could have changed) and that is in memory and totally clean. 
                if (childCouldHaveChanged && child != null && !child.IsDirty && !child.DescendantIsDirty && options.IncludeChildrenMode == IncludeChildrenMode.IncludeAllChildren && options.IncludeChildrenMode == child.OriginalIncludeChildrenMode)
                {
                    // In this case, we update the childStorage to reflect the child's own storage, rather than a slice in the parent's storage. The reason is that the buffer may have been updated if the same object appears more than once in the object hierarchy, or the child may have updated its storage after a manual call to UpdateStoredBuffer.
                    childStorage = child.LazinatorMemoryStorage;
                    if (!childStorage.IsEmpty)
                        childCouldHaveChanged = false;
                }
            }
            if (!childCouldHaveChanged)
            {
                int startPosition = writer.ActiveMemoryPosition;
                childStorage = await WriteExistingChildStorageAsync(writer, getChildSliceFn, childStorage, options.SerializeDiffs);
                if (options.UpdateStoredBuffer)
                {
                    if (child != null)
                    {
                        long length = childStorage.Length;
                        child.UpdateStoredBuffer(ref writer.Writer, startPosition, length, options.IncludeChildrenMode, true);
                    }
                }
            }
            else
            {
                if (child != null)
                {
                    WriteChildToBinary(ref writer.Writer, ref child, options);
                }
            }
            AddParentToChildless(ref child, parent);
        }

        /// <summary>
        /// Initiates a binary write to a child of a Lazinator object that has AsyncLazinatorMemory defined, optionally including a length prefix, using existing storage if possible
        /// </summary>
        /// <param name="writer">The binary writer</param>
        /// <param name="child">The child to be written. This child will be written regardless of includeChildrenMode.</param>
        /// <param name="options">Serialization options</param>
        /// <param name="childHasBeenAccessed">True if the child's value has been accessed.</param>
        /// <param name="getChildSliceFn">A function to return the child's original storage</param>
        /// <param name="parent">The parent of the object being written</param>
        /// 
        public async static ValueTask WriteChildAsync<T>(BufferWriterContainer writer, T child,
            LazinatorSerializationOptions options, bool childHasBeenAccessed, ReturnLazinatorMemoryDelegateAsync getChildSliceFn, ILazinator parent) where T : ILazinator, ILazinatorAsync
        {
            bool childCouldHaveChanged = childHasBeenAccessed || (child != null && options.IncludeChildrenMode != child.OriginalIncludeChildrenMode);
            LazinatorMemory childStorage = default;
            if (!childHasBeenAccessed && child != null)
            {
                childStorage = await getChildSliceFn();
                if (childStorage.IsEmpty)
                    childCouldHaveChanged = true; // child might be an uninitialized struct and the object has not been previously deserialized. Thus, we treat this as an object that has been changed, so that we can serialize it. 
            }
            else
            {
                // check for a child that has been accessed (or otherwise could have changed) and that is in memory and totally clean. 
                if (childCouldHaveChanged && child != null && !child.IsDirty && !child.DescendantIsDirty && options.IncludeChildrenMode == IncludeChildrenMode.IncludeAllChildren && options.IncludeChildrenMode == child.OriginalIncludeChildrenMode)
                {
                    // In this case, we update the childStorage to reflect the child's own storage, rather than a slice in the parent's storage. The reason is that the buffer may have been updated if the same object appears more than once in the object hierarchy, or the child may have updated its storage after a manual call to UpdateStoredBuffer.
                    childStorage = child.LazinatorMemoryStorage;
                    if (!childStorage.IsEmpty)
                        childCouldHaveChanged = false;
                }
            }
            if (!childCouldHaveChanged)
            {
                int startPosition = writer.ActiveMemoryPosition;
                childStorage = await WriteExistingChildStorageAsync(writer, getChildSliceFn, childStorage, options.SerializeDiffs);
                if (options.UpdateStoredBuffer)
                {
                    if (child != null)
                    {
                        long length = childStorage.Length;
                        child.UpdateStoredBuffer(ref writer.Writer, startPosition, length, options.IncludeChildrenMode, true);
                    }
                }
            }
            else
            {
                if (child != null)
                {
                    await WriteChildToBinaryAsync(writer, child, options);
                }
            }
            AddParentToChildless(ref child, parent);
        }

        public static void WriteNullChild_LengthsSeparate(ref BufferWriter writer, bool restrictLengthTo255Bytes)
        {
            if (restrictLengthTo255Bytes)
            {
                writer.RecordLength((byte)0);
            }
            else
            {
                writer.RecordLength((int)0);
            }
        }


        /// <summary>
        /// Writes a child or a reference to a child to a binary buffer, where that child has not been previously accessed. This thus obtains the last version from storage (or stores a zero length if this
        /// is the first time saving the child and it really is empty).
        /// </summary>
        /// <param name="writer">The binary writer</param>
        /// <param name="getChildSliceFn">A function that returns the Lazinator memory containing the child</param>
        /// <param name="childStorage">The Lazinator memory containing the child, if such memory has been accessed</param>
        /// <param name="considerWriteReferenceOnly">Check whether to include a reference to previously written memory instead of the buffer itself, when serializing diffs</param>
        /// <param name="serializeDiffsThreshold">The minimum number of bytes to justify including a reference to previously written memory</param>
        /// <returns></returns>
        public static LazinatorMemory WriteExistingChildStorage(ref BufferWriter writer, ReturnLazinatorMemoryDelegate getChildSliceFn, LazinatorMemory childStorage, bool considerWriteReferenceOnly, int serializeDiffsThreshold)
        {
            if (childStorage.IsEmpty)
                childStorage = getChildSliceFn(); // this is the storage holding the child, which has never been accessed
            if (considerWriteReferenceOnly && !childStorage.IsEmpty && childStorage.Length > serializeDiffsThreshold)
                writer.InsertReferenceToPreviousVersion(childStorage.MemoryRangeIndex, childStorage.FurtherOffsetIntoMemoryBlock, childStorage.Length);
            else
                childStorage.WriteToBuffer(ref writer);
            return childStorage;
        }

        /// <summary>
        /// Writes a child to a binary buffer, where that child has not been previously accessed. This thus obtains the last version from storage (or stores a zer length if this
        /// is the first time saving the child and it really is empty).
        /// </summary>
        /// <param name="writer">The binary writer async container</param>
        /// <param name="getChildSliceFn">A function that asynchronously returns the Lazinator memory containing the child</param>
        /// <param name="childStorage">The Lazinator memory containing the child, if such memory has been accessed</param>
        /// <param name="considerWriteReferenceOnly">Check whether to include a reference to previously written memory instead of the buffer itself, when serializing diffs</param>
        /// <param name="serializeDiffsThreshold">The minimum number of bytes to justify including a reference to previously written memory</param>
        /// <returns></returns>
        public async static ValueTask<LazinatorMemory> WriteExistingChildStorageAsync(BufferWriterContainer writer, ReturnLazinatorMemoryDelegateAsync getChildSliceFn, LazinatorMemory childStorage, bool writeReferenceOnly)
        {
            if (childStorage.IsEmpty)
                childStorage = await getChildSliceFn(); // this is the storage holding the child, which has never been accessed
            if (writeReferenceOnly && !childStorage.IsEmpty)
                writer.InsertReferenceToPreviousVersion(childStorage.MemoryRangeIndex, childStorage.FurtherOffsetIntoMemoryBlock, childStorage.Length);
            else
                await childStorage.WriteToBufferAsync(writer);
            return childStorage;
        }

        private static void AddParentToChildless<T>(ref T child, ILazinator parent) where T : ILazinator
        {
            if (child != null && !child.LazinatorParents.Any())
                child.LazinatorParents = child.LazinatorParents.WithAdded(parent); // set the parent so that this object can be used like a deserialized object
        }

        /// <summary>
        /// Initiates a binary write of a child of a Lazinator object, optionally including a length prefix, using the child's own storage if possible.
        /// </summary>
        /// <param name="writer">The binary writer</param>
        /// <param name="child">The child</param>
        /// <param name="options">Serialization options</param>
        /// <param name="parent">The parent of the object being written</param>

        private static void WriteChildToBinary<T>(ref BufferWriter writer, ref T child, LazinatorSerializationOptions options) where T : ILazinator
        {
            T childCopy = child;
            void action(ref BufferWriter w)
            {
                if (childCopy.LazinatorMemoryStorage.IsEmpty || childCopy.IsDirty || childCopy.DescendantIsDirty || options.VerifyCleanness || options.IncludeChildrenMode != IncludeChildrenMode.IncludeAllChildren || options.IncludeChildrenMode != childCopy.OriginalIncludeChildrenMode)
                    childCopy.SerializeToExistingBuffer(ref w, options);
                else
                    childCopy.LazinatorMemoryStorage.WriteToBuffer(ref w); // the childCopy has been accessed, but is unchanged, so we can use the storage holding the childCopy
            }
            LazinatorUtilities.WriteToBinaryWithoutLengthPrefix(ref writer, action);
        }

        private async static ValueTask WriteChildToBinaryAsync<T>(BufferWriterContainer writer, T child, LazinatorSerializationOptions options) where T : ILazinator, ILazinatorAsync
        {
            T childCopy = child;
            async ValueTask action(BufferWriterContainer w)
            {
                if (childCopy.LazinatorMemoryStorage.IsEmpty || childCopy.IsDirty || childCopy.DescendantIsDirty || options.VerifyCleanness || options.IncludeChildrenMode != IncludeChildrenMode.IncludeAllChildren || options.IncludeChildrenMode != childCopy.OriginalIncludeChildrenMode)
                    await childCopy.SerializeToExistingBufferAsync(w, options);
                else
                    await childCopy.LazinatorMemoryStorage.WriteToBufferAsync(w); // the childCopy has been accessed, but is unchanged, so we can use the storage holding the childCopy
            }
            await LazinatorUtilities.WriteToBinaryWithoutLengthPrefixAsync(writer, action);
        }

        #endregion

        #region Generic IDs

        /// <summary>
        /// Called in Lazinator code-behind.
        /// Reads a generic ID from a span, if the type contains open generic parameters. Confirms that the outer type (or the only type, if nongeneric)
        /// is the expected type.
        /// </summary>
        /// <param name="containsOpenGenericParameters">True if the Lazinator type contains open generic parameters.</param>
        /// <param name="uniqueID">The expected unique ID for the outermost type</param>
        /// <param name="span">The span to read from</param>
        /// <param name="index">The current index within the span, which will be incremented after the information is read</param>
        /// <returns>The generic ID type, or default for a nongeneric</returns>
        public static LazinatorGenericIDType ReadGenericIDIfApplicable(bool containsOpenGenericParameters, int uniqueID, ReadOnlySpan<byte> span, ref int index)
        {
            if (containsOpenGenericParameters)
            {
                List<int> lazinatorGenericID = ReadLazinatorGenericID(span, ref index).TypeAndInnerTypeIDs;
                if (lazinatorGenericID[0] != uniqueID)
                {
                    ThrowHelper.ThrowFormatException();
                }
                return new LazinatorGenericIDType(lazinatorGenericID);
            }
            else
            {
                int readUniqueID = span.ToDecompressedInt32(ref index);
                if (readUniqueID != uniqueID)
                {
                    ThrowHelper.ThrowFormatException();
                }
                return default;
            }
        }

        /// <summary>
        /// Called in Lazinator code-behind. 
        /// Writes a Lazinator generic ID to a binary buffer.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="lazinatorGenericID"></param>
        public static void WriteLazinatorGenericID(ref BufferWriter writer, LazinatorGenericIDType lazinatorGenericID)
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

        /// <summary>
        /// Reads a Lazinator generic ID. This is called by Lazinator where Lazinator already knows that it is dealing with an open generic type.
        /// </summary>
        /// <param name="span"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static LazinatorGenericIDType ReadLazinatorGenericID(ReadOnlySpan<byte> span, ref int index)
        {
            int mainID = span.ToDecompressedInt32(ref index);
            List<int> l = new List<int>() { mainID };
            byte numEntries = span.ToByte(ref index);
            for (byte b = 1; b < numEntries; b++)
            {
                l.Add(span.ToDecompressedInt32(ref index));
            }
            return new LazinatorGenericIDType(l);
        }

        #endregion

        #region Nonlazinators

        /// <summary>
        /// Initiates the conversion to binary of a non-lazinator object, writing the length to the appropriate spot earlier in the buffer.
        /// </summary>
        /// <param name="nonLazinatorObject">An object that does not implement ILazinator</param>
        /// <param name="isBelievedDirty">An indication of whether the object to be converted to bytes is believed to be dirty, e.g. has had its dirty flag set.</param>
        /// <param name="isAccessed">An indication of whether the object has been accessed.</param>
        /// <param name="writer">The binary writer</param>
        /// <param name="getChildSliceForFieldFn">A function to return the child slice of memory for the non-Lazinator object</param>
        /// <param name="verifyCleanness">If true, then the dirty-conversion will always be performed unless we are sure it is clean, and if the object is not believed to be dirty, the results will be compared to the clean version. This allows for errors from failure to serialize objects that have been changed to be caught during development.</param>
        /// <param name="binaryWriterAction">The action to complete the write to the binary buffer</param>
        /// 
        public static void WriteNonLazinatorObject(object nonLazinatorObject,
            bool isBelievedDirty, bool isAccessed, ref BufferWriter writer, ReturnLazinatorMemoryDelegate getChildSliceForFieldFn,
            bool verifyCleanness, WritePossiblyVerifyingCleannessDelegate binaryWriterAction)
        {
            int startPosition = writer.ActiveMemoryPosition;
            WriteNonLazinatorObject_WithoutLengthPrefix(nonLazinatorObject, isBelievedDirty, isAccessed, ref writer, getChildSliceForFieldFn, verifyCleanness, binaryWriterAction);
            long length = writer.ActiveMemoryPosition - startPosition;
            writer.RecordLength((int)length);
        }

        /// <summary>
        /// Initiates the conversion to binary of a non-lazinator object, omitting the length prefix. This can be used on the last property of a struct or sealed class.
        /// </summary>
        /// <param name="nonLazinatorObject">An object that does not implement ILazinator</param>
        /// <param name="isBelievedDirty">An indication of whether the object to be converted to bytes is believed to be dirty, e.g. has had its dirty flag set.</param>
        /// <param name="isAccessed">An indication of whether the object has been accessed.</param>
        /// <param name="writer">The binary writer</param>
        /// <param name="getChildSliceForFieldFn">A function to return the child slice of memory for the non-Lazinator object</param>
        /// <param name="verifyCleanness">If true, then the dirty-conversion will always be performed unless we are sure it is clean, and if the object is not believed to be dirty, the results will be compared to the clean version. This allows for errors from failure to serialize objects that have been changed to be caught during development.</param>
        /// <param name="binaryWriterAction">The action to complete the write to the binary buffer</param>
        public static void WriteNonLazinatorObject_WithoutLengthPrefix(object nonLazinatorObject,
            bool isBelievedDirty, bool isAccessed, ref BufferWriter writer, ReturnLazinatorMemoryDelegate getChildSliceForFieldFn,
            bool verifyCleanness, WritePossiblyVerifyingCleannessDelegate binaryWriterAction)
        {
            LazinatorMemory original = getChildSliceForFieldFn();
            int length = (int) original.Length;
            if (!isAccessed && length > 0)
            {
                // object has never been loaded into memory, so there is no need to verify cleanness
                // just return what we have.
                original.WriteToBuffer(ref writer);
            }
            else if (isBelievedDirty || length == 0)
            {
                // We definitely need to write to binary, because either the dirty flag has been set or the original storage doesn't have anything to help us.
                void action(ref BufferWriter w) => binaryWriterAction(ref w, verifyCleanness);
                WriteToBinaryWithoutLengthPrefix(ref writer, action);
            }
            else
            {
                if (verifyCleanness)
                {
                    ReadOnlyMemory<byte> revised = ConvertNonLazinatorObjectToBytes(nonLazinatorObject, binaryWriterAction);
                    ConfirmMatch(original, revised);
                }
                original.WriteToBuffer(ref writer);
            }
        }

        /// <summary>
        /// Initiates the conversion to binary of a non-lazinator object, writing the length to the appropriate spot earlier in the buffer.
        /// </summary>
        /// <param name="nonLazinatorObject">An object that does not implement ILazinator</param>
        /// <param name="isBelievedDirty">An indication of whether the object to be converted to bytes is believed to be dirty, e.g. has had its dirty flag set.</param>
        /// <param name="isAccessed">An indication of whether the object has been accessed.</param>
        /// <param name="writer">The binary writer</param>
        /// <param name="getChildSliceForFieldFn">A function to return the child slice of memory for the non-Lazinator object</param>
        /// <param name="verifyCleanness">If true, then the dirty-conversion will always be performed unless we are sure it is clean, and if the object is not believed to be dirty, the results will be compared to the clean version. This allows for errors from failure to serialize objects that have been changed to be caught during development.</param>
        /// <param name="binaryWriterAction">The action to complete the write to the binary buffer</param>
        /// 
        public async static ValueTask WriteNonLazinatorObjectAsync(object nonLazinatorObject,
            bool isBelievedDirty, bool isAccessed, BufferWriterContainer writer, ReturnLazinatorMemoryDelegateAsync getChildSliceForFieldFn,
            bool verifyCleanness, WritePossiblyVerifyingCleannessDelegate binaryWriterAction)
        {
            int startPosition = writer.ActiveMemoryPosition;
            await WriteNonLazinatorObject_WithoutLengthPrefixAsync(nonLazinatorObject, isBelievedDirty, isAccessed, writer, getChildSliceForFieldFn, verifyCleanness, binaryWriterAction);
            long length = writer.ActiveMemoryPosition - startPosition;
            writer.RecordLength((int)length);
        }

        /// <summary>
        /// Initiates the conversion to binary of a non-lazinator object, omitting the length prefix. This can be used on the last property of a struct or sealed class.
        /// </summary>
        /// <param name="nonLazinatorObject">An object that does not implement ILazinator</param>
        /// <param name="isBelievedDirty">An indication of whether the object to be converted to bytes is believed to be dirty, e.g. has had its dirty flag set.</param>
        /// <param name="isAccessed">An indication of whether the object has been accessed.</param>
        /// <param name="writer">The binary writer</param>
        /// <param name="getChildSliceForFieldFn">A function to return the child slice of memory for the non-Lazinator object</param>
        /// <param name="verifyCleanness">If true, then the dirty-conversion will always be performed unless we are sure it is clean, and if the object is not believed to be dirty, the results will be compared to the clean version. This allows for errors from failure to serialize objects that have been changed to be caught during development.</param>
        /// <param name="binaryWriterAction">The action to complete the write to the binary buffer</param>
        public async static ValueTask WriteNonLazinatorObject_WithoutLengthPrefixAsync(object nonLazinatorObject,
            bool isBelievedDirty, bool isAccessed, BufferWriterContainer writer, ReturnLazinatorMemoryDelegateAsync getChildSliceForFieldFn,
            bool verifyCleanness, WritePossiblyVerifyingCleannessDelegate binaryWriterAction)
        {
            LazinatorMemory original = await getChildSliceForFieldFn();
            long length = original.Length;
            if (!isAccessed && length > 0)
            {
                // object has never been loaded into memory, so there is no need to verify cleanness
                // just return what we have.
                original.WriteToBuffer(ref writer.Writer);
            }
            else if (isBelievedDirty || length == 0)
            {
                // We definitely need to write to binary, because either the dirty flag has been set or the original storage doesn't have anything to help us.
                void action(ref BufferWriter w) => binaryWriterAction(ref w, verifyCleanness);
                WriteToBinaryWithoutLengthPrefix(ref writer.Writer, action);
            }
            else
            {
                if (verifyCleanness)
                {
                    ReadOnlyMemory<byte> revised = ConvertNonLazinatorObjectToBytes(nonLazinatorObject, binaryWriterAction);
                    ConfirmMatch(original, revised);
                }
                original.WriteToBuffer(ref writer.Writer);
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

            BufferWriter writer = new BufferWriter();
            binaryWriterAction(ref writer, true);
            var memory = writer.LazinatorMemory;
            byte[] bytes = new byte[memory.Length];
            memory.CopyToArray(bytes);
            writer.LazinatorMemory.Dispose();
            return bytes;
        }

        /// <summary>
        /// Verifies that the original storage for a non-Lazinator object matches rewritten storage (though it can be used for any storage). Matches the storage generated by rewriting to bytes. Calls an Exception if that is not the case.
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
        /// Verifies that the original storage for a non-Lazinator object matches rewritten storage (though it can be used for any storage). Matches the storage generated by rewriting to bytes. Calls an Exception if that is not the case.
        /// </summary>
        /// <param name="originalStorage">The original storage before any changes could have been made</param>
        /// <param name="rewrittenStorage">The rewritten storage after changes could have been made, but were not thought to have been.</param>
        public static void ConfirmMatch(LazinatorMemory originalStorage, ReadOnlyMemory<byte> rewrittenStorage)
        {
            if (originalStorage.SingleMemory)
            {
                ConfirmMatch((ReadOnlyMemory<byte>) originalStorage.OnlyMemory, rewrittenStorage);
                return;
            }
            bool matches = originalStorage.Matches(rewrittenStorage.Span);
            if (!matches)
            {
                throw new UnexpectedDirtinessException();
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
        /// Enumerates all nodes in the hierarchy asynchronously.
        /// </summary>
        /// <returns>The dirty nodes</returns>
        public static IAsyncEnumerable<ILazinator> EnumerateAllNodesAsync(this ILazinatorAsync startNode)
        {
            return startNode.EnumerateLazinatorNodesAsync(x => true, false, x => true, false, false);
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
        /// Enumerates the Lazinator children of a node, along with their names
        /// </summary>
        /// <param name="node">The node</param>
        /// <returns>The Lazinator children of the node along with their property names</returns>
        public static IAsyncEnumerable<(string propertyName, ILazinator descendant)> EnumerateLazinatorChildrenAsync(this ILazinatorAsync node, bool deserializedOnly = false)
        {
            return node.EnumerateLazinatorDescendantsAsync(x => true, true, x => false, deserializedOnly, true);
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
        /// Enumerates the descendants of a node that have already been deserialized asynchronously.
        /// </summary>
        /// <param name="node">The node</param>
        /// <returns>The node and any deserialized descendants</returns>
        public static IAsyncEnumerable<(string propertyName, ILazinator descendant)> EnumerateDeserializedDescendantsAsync(this ILazinatorAsync node)
        {
            return node.EnumerateLazinatorDescendantsAsync(x => true, true, x => true, true, false);
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
                bool topNodesEqual;
                string comparison;
                try
                {
                    topNodesEqual = TopNodesOfHierarchyEqual(firstHierarchy, secondHierarchy, out comparison);
                }
                catch (UnsetNonnullableLazinatorException unsetException)
                {
                    throw new Exception($"Hashes were expected to be same, but differed. Difference cannot be traced because of unset nonnullable Lazinator.", unsetException);
                }
                if (topNodesEqual)
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
            if (firstHierarchy is IList firstList && secondHierarchy is IList secondList)
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
            while (startNode != null && startNode.LazinatorParents.Any())
            {
                startNode = startNode.LazinatorParents.LastAdded;
                if (startNode != null) // could become null after call to Any
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
            hierarchy.SerializeLazinator(); // we must actually convert it to bytes -- if we just mark things clean, then that will be misleading, and further serialization will be incorrect
            MarkHierarchyUnchanged(hierarchy, true);
        }

        /// <summary>
        /// Marks all Lazinator objects in a hierarchy as having not been changed. This has no effect on whether the objects are marked as currently dirty.
        /// </summary>
        /// <param name="hierarchy">The node containing the top of the hierarchy to mark as unchanged</param>
        /// <param name="alsoKeepClean">If true, nodes are marked as not being dirty. However, this should be used only after updating the stored buffer.</param>
        public static void MarkHierarchyUnchanged(this ILazinator hierarchy, bool alsoKeepClean = false)
        {
            if (hierarchy != null)
                hierarchy.ForEachLazinator(
                    node =>
                    {
                        node.HasChanged = false;
                        node.DescendantHasChanged = false;
                        if (alsoKeepClean)
                        {
                            node.IsDirty = false;
                            node.DescendantIsDirty = false;
                        }
                        return node;
                    },
                    true, true);
        }

        /// <summary>
        /// Checks whether a hierarchy's descendant dirtiness is consistent. Where it is not, that may indicate a circular hierarchy.
        /// A hierarchy is consistent where the ancestor of a dirty node has DescendantIsDirty = true.
        /// </summary>
        /// <param name="startPoint">The starting point in the Lazinator hierarchy.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Throws if a hierarchy's descendant dirtiness is not consistent.
        /// A hierarchy is consistent where the ancestor of a dirty node has DescendantIsDirty = true.
        /// </summary>
        /// <param name="startPoint"></param>
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
        /// Clones a Lazinator object into an object that contains no buffer.
        /// </summary>
        /// <typeparam name="T">The type of the Lazinator object</typeparam>
        /// <param name="lazinator">The lazinator object</param>
        /// <param name="includeChildrenMode">Whether some or all children should be included</param>
        /// <returns>A clone of the Lazinator object</returns>
        public static T CloneNoBuffer<T>(this T lazinator,
            IncludeChildrenMode includeChildrenMode = IncludeChildrenMode.IncludeAllChildren) where T : ILazinator =>
            CloneLazinatorTyped(lazinator, includeChildrenMode, CloneBufferOptions.NoBuffer);

        /// <summary>
        /// Clones a Lazinator object, returning the object as the generic type. The type of the object will be inferred by C#,
        /// so this is a useful method to clone a Lazinator object and return a Lazinator object of the same type.
        /// </summary>
        /// <typeparam name="T">The type of the Lazinator object</typeparam>
        /// <param name="lazinator">The lazinator object</param>
        /// <param name="includeChildrenMode">Whether some or all children should be included</param>
        /// <param name="cloneBufferOptions">How the clone's buffer should relate to the original's</param>
        /// <returns>A clone of the Lazinator object</returns>
        public static T CloneLazinatorTyped<T>(this T lazinator, IncludeChildrenMode includeChildrenMode = IncludeChildrenMode.IncludeAllChildren, CloneBufferOptions cloneBufferOptions = CloneBufferOptions.IndependentBuffers) where T : ILazinator
        {
            if (EqualityComparer<T>.Default.Equals(lazinator, default(T)))
                return default(T);
            T clone = (T)lazinator.CloneLazinator(includeChildrenMode, cloneBufferOptions);
            return clone;
        }

        /// <summary>
        /// Clones a Lazinator object, returning the object as the generic type. The type of the object will be inferred by C#,
        /// so this is a useful method to clone a Lazinator object and return a Lazinator object of the same type.
        /// </summary>
        /// <typeparam name="T">The type of the Lazinator object</typeparam>
        /// <param name="lazinator">The lazinator object</param>
        /// <param name="includeChildrenMode">Whether some or all children should be included</param>
        /// <param name="cloneBufferOptions">How the clone's buffer should relate to the original's</param>
        /// <returns>A clone of the Lazinator object</returns>
        public async static ValueTask<T> CloneLazinatorTypedAsync<T>(this T lazinator, IncludeChildrenMode includeChildrenMode = IncludeChildrenMode.IncludeAllChildren, CloneBufferOptions cloneBufferOptions = CloneBufferOptions.IndependentBuffers) where T : ILazinator, ILazinatorAsync
        {
            if (EqualityComparer<T>.Default.Equals(lazinator, default(T)))
                return default(T);
            T clone = (T)await lazinator.CloneLazinatorAsync(includeChildrenMode, cloneBufferOptions);
            return clone;
        }

        /// <summary>
        /// Updates the Lazinator's memory storage and then copies this to an array.
        /// </summary>
        /// <returns></returns>
        public static byte[] CopyToArray(this ILazinator lazinator)
        {
            lazinator.SerializeLazinator();
            byte[] array = new byte[lazinator.LazinatorMemoryStorage.Length];
            lazinator.LazinatorMemoryStorage.CopyToArray(array);
            return array;
        }

        /// <summary>
        /// Serializes the Lazinator to an array without updating the Lazinator's memory storage.
        /// </summary>
        /// <returns></returns>
        public static byte[] SerializeToArray(this ILazinator lazinator)
        {
            LazinatorMemory memory = lazinator.SerializeLazinator(new LazinatorSerializationOptions(IncludeChildrenMode.IncludeAllChildren, false, false, false));
            byte[] array = new byte[memory.Length];
            memory.CopyToArray(array);
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
            // The following code could alternatively be used if array pooling is not needed: return new SimpleMemoryOwner(new byte[minimumSize * 2]);
            var toRent = LazinatorMemoryPool.Rent(minimumSize);
            return toRent;
        }

        /// <summary>
        /// Returns a slice of serialized bytes corresponding to a child, excluding the length prefix.
        /// </summary>
        /// <param name="serializedBytes">The serialized bytes for the parent object</param>
        /// <param name="byteOffset">The byte offset into the parent object of the length prefix for the child object</param>
        /// <param name="byteLength">The byte length of the child, including the length (if applicable)</param>
        /// <param name="fixedLength"The fixed length of the child, if the length is not included in the serialized bytes
        /// <returns></returns>
        public static LazinatorMemory GetChildSlice(LazinatorMemory serializedBytes, long byteOffset, long byteLength, int? fixedLength)
        {
            if (serializedBytes.IsEmpty)
            {
                return LazinatorMemory.EmptyLazinatorMemory;
            }
            if (fixedLength != null)
                return serializedBytes.Slice(byteOffset, (int)fixedLength);
            return serializedBytes.Slice(byteOffset, byteLength);
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
        public async static ValueTask<LazinatorMemory> GetChildSliceAsync(LazinatorMemory serializedBytes, int byteOffset, int byteLength, int? fixedLength)
        {
            var result = GetChildSlice(serializedBytes, byteOffset, byteLength, fixedLength);
            await result.LoadInitialReadOnlyMemoryAsync();
            return result;
        }

        /// <summary>
        /// Fully deserialize the lazinator at this node and return the Lazinator object (or a copy if it is a struct).
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
            }, false, true);
            return lazinator;
        }

        /// <summary>
        /// Removes a buffer from this Lazinator node and ensures that all descendants are deserialized, since the buffer will no longer be available to deserialize them.
        /// </summary>
        /// <param name="lazinator">The Lazinator node</param>
        /// <returns>The node with the buffer removed (or a copy if a Lazinator struct)</returns>
        public static ILazinator RemoveBufferInHierarchy(this ILazinator lazinator, bool disposeBuffer = false)
        {
            if (lazinator == null)
                return null;
            lazinator = lazinator.ForEachLazinator(l => RemoveBuffer_Helper(l, disposeBuffer), false, true); // this will visit every node, thus deserializing everything, and it will remove the buffer at each node if needed.
            return lazinator;
        }

        /// <summary>
        /// Removes a buffer from this Lazinator node, without affecting its children. This should not be done if any other node might still need the buffer.
        /// </summary>
        /// <param name="lazinator">The Lazinator node</param>
        /// <returns>The node with the buffer removed (or a copy if a Lazinator struct)</returns>
        private static ILazinator RemoveBuffer_Helper(this ILazinator lazinator, bool disposeBuffer)
        {
            if (lazinator == null)
                return null;
            var existingBuffer = lazinator.LazinatorMemoryStorage;
            lazinator.LazinatorMemoryStorage = default;
            if (disposeBuffer)
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
            lazinator.SerializeLazinator();
            return ((ReadOnlyMemory<byte>)lazinator.LazinatorMemoryStorage.GetConsolidatedMemory()).GetMemoryStream();
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

        /// <summary>
        /// Generates a 32-bit hash code from the binary storage of the object (unless NonBinaryHash32 is set).
        /// </summary>
        /// <param name="lazinator"></param>
        /// <returns></returns>
        public static uint GetBinaryHashCode32(this ILazinator lazinator)
        {
            if (lazinator.NonBinaryHash32)
                return (uint) lazinator.GetHashCode();
            if (!lazinator.IsDirty && !lazinator.DescendantIsDirty && lazinator.OriginalIncludeChildrenMode == IncludeChildrenMode.IncludeAllChildren && lazinator.LazinatorMemoryStorage.IsEmpty == false && lazinator.LazinatorMemoryStorage.Disposed == false)
                return FarmhashByteSpans.Hash32(lazinator.LazinatorMemoryStorage.OnlyMemory.Span);
            else
            {
                LazinatorMemory serialized =
                    lazinator.SerializeLazinator(new LazinatorSerializationOptions(IncludeChildrenMode.IncludeAllChildren, false, false, false));
                var result = FarmhashByteSpans.Hash32(serialized.InitialReadOnlyMemory.Span);
                serialized.Dispose();
                return result;
            }
        }

        /// Generates a 64-bit hash code from the binary storage of the object (unless NonBinaryHash32 is set).
        public static ulong GetBinaryHashCode64(this ILazinator lazinator)
        {
            if (!lazinator.IsDirty && !lazinator.DescendantIsDirty && lazinator.OriginalIncludeChildrenMode == IncludeChildrenMode.IncludeAllChildren && lazinator.LazinatorMemoryStorage.IsEmpty == false && lazinator.LazinatorMemoryStorage.Disposed == false)
            {
                var result = FarmhashByteSpans.Hash64(lazinator.LazinatorMemoryStorage.OnlyMemory.Span);
                return result;
            }
            else
            {
                LazinatorMemory serialized =
                    lazinator.SerializeLazinator(new LazinatorSerializationOptions(IncludeChildrenMode.IncludeAllChildren, false, false, false));
                var result = FarmhashByteSpans.Hash64(serialized.InitialReadOnlyMemory.Span);
                serialized.Dispose();
                return result;
            }
        }

        /// Generates a 128-bit hash code from the binary storage of the object (unless NonBinaryHash32 is set).
        public static Guid GetBinaryHashCode128(this ILazinator lazinator)
        {
            if (!lazinator.IsDirty && !lazinator.DescendantIsDirty && lazinator.OriginalIncludeChildrenMode == IncludeChildrenMode.IncludeAllChildren && lazinator.LazinatorMemoryStorage.IsEmpty == false && lazinator.LazinatorMemoryStorage.Disposed == false)
                return FarmhashByteSpans.Hash128(lazinator.LazinatorMemoryStorage.OnlyMemory.Span);
            else
            {
                LazinatorMemory serialized =
                    lazinator.SerializeLazinator(new LazinatorSerializationOptions(IncludeChildrenMode.IncludeAllChildren, false, false, false));
                var result = FarmhashByteSpans.Hash128(serialized.InitialReadOnlyMemory.Span);
                serialized.Dispose();
                return result;
            }
        }

        #endregion
    }
}
