using Lazinator.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lazinator.Core
{
    /// <summary>
    /// Options to control the Lazinator serialization process
    /// </summary>
    public readonly struct LazinatorSerializationOptions
    {
        /// <summary>
        /// Indicates which children should be serialized along with a parent Lazinator object. When a child is serialized, the mode will also apply to its children, recursively through the hierarchy. Children can be designated as excludable or includable with appropriate attributes. Any non-nullable reference type object will always be included, regardless of this setting. 
        /// </summary>
        public readonly IncludeChildrenMode IncludeChildrenMode;
        /// <summary>
        /// Whether double-checking is needed to ensure that non-Lazinator objects that have corresponding manual _Dirty properties and are thought to be clean really are clean. An UnexpectedDirtinessException is thrown if an object whose current status indicates that it is clean is in fact dirty. This is not recommended in production code, because it defeats the time-savings associated with having manual _Dirty properties
        /// </summary>
        public readonly bool VerifyCleanness;
        /// <summary>
        /// Whether the objects being serialized should be updated to use the new buffer. This must be false if includeChildrenMode is not set to include all children. If false, then the returned memory will be independent of the existing memory.
        /// </summary>
        public readonly bool UpdateStoredBuffer;
        /// <summary>
        /// Whether to serialize only data that has changed from the previous serialization, instead of all the data in the object hierarchy. When serializing
        /// diffs, if a previously serialized object has not changed, then the serializer may simply note the location of that object in memory rather than 
        /// copying it. This works only with objects that are separable and is intended for large object graphs.
        /// </summary>
        public readonly bool SerializeDiffs;
        /// <summary>
        /// If a Lazinator object is splittable across buffers, then, once the total number of bytes written crosses this value after serializing a child object, the next buffer is moved onto. This has no
        /// effect in a Lazinator object not splittable across buffers.
        /// </summary>
        public readonly int NextBufferThreshold;
        /// <summary>
        /// When serializing diffs, the minimum number of bytes that an untouched section of memory must be to record a reference to this memory instead of putting the memory into a new buffer.
        /// </summary>
        public readonly int SerializeDiffsThreshold;

        const int DefaultSerializeDiffsThreshold = 1024;

        public LazinatorSerializationOptions(IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer, bool serializeDiffs, int nextBufferThreshold=int.MaxValue, int serializeDiffsThreshold=DefaultSerializeDiffsThreshold)
        {
            IncludeChildrenMode = includeChildrenMode;
            VerifyCleanness = verifyCleanness;
            UpdateStoredBuffer = updateStoredBuffer;
            SerializeDiffs = serializeDiffs;
            NextBufferThreshold = nextBufferThreshold;
            SerializeDiffsThreshold = serializeDiffsThreshold;
            if (IncludeChildrenMode != IncludeChildrenMode.IncludeAllChildren && updateStoredBuffer)
                ThrowHelper.ThrowCannotUpdateStoredBuffer();
        }

        public static LazinatorSerializationOptions Default = new LazinatorSerializationOptions(IncludeChildrenMode.IncludeAllChildren, false, true, false);
        public static LazinatorSerializationOptions DefaultDiffSerialization = new LazinatorSerializationOptions(IncludeChildrenMode.IncludeAllChildren, false, true, true, 65000);


        public bool SplittingPossible => SerializeDiffs || NextBufferThreshold != int.MaxValue;

        public LazinatorSerializationOptions WithoutSplittingPossible() => new LazinatorSerializationOptions(IncludeChildrenMode, VerifyCleanness, UpdateStoredBuffer, false, int.MaxValue);
    }
}
