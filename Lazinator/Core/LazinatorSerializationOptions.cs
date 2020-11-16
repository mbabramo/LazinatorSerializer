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

        public LazinatorSerializationOptions(IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            IncludeChildrenMode = includeChildrenMode;
            VerifyCleanness = verifyCleanness;
            UpdateStoredBuffer = updateStoredBuffer;
            if (IncludeChildrenMode != IncludeChildrenMode.IncludeAllChildren && updateStoredBuffer)
                ThrowHelper.ThrowCannotUpdateStoredBuffer();
        }

        public static LazinatorSerializationOptions Default = new LazinatorSerializationOptions(IncludeChildrenMode.IncludeAllChildren, false, true);
    }
}
