using Lazinator.Attributes;
using Lazinator.Core;

namespace Lazinator.Wrappers
{
    /// <summary>
    /// A Lazinator interface for a Lazinator wrapper for a nullable struct. A wrapper can be used as a generic type where the unwrapped object cannot be.
    /// </summary>
    [Lazinator((int)LazinatorCoreUniqueIDs.IWrapperNullableStruct, -1)]
    interface IWNullableStruct<T> where T : struct, ILazinator
    {
        bool HasValue { get; set; }
        T NonNullValue { get; set; }
    }
}
