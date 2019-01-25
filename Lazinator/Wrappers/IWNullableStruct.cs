using Lazinator.Attributes;
using Lazinator.Core;

namespace Lazinator.Wrappers
{
    [Lazinator((int)LazinatorCoreUniqueIDs.IWrapperNullableStruct, -1)]
    interface IWNullableStruct<T> where T : struct, ILazinator
    {
        bool HasValue { get; set; }
        T NonNullValue { get; set; }
    }
}
