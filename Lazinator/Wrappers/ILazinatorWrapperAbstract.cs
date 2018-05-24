using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorWrapperAbstract)]
    interface ILazinatorWrapperAbstract<T>
    {
        T Wrapped { get; set; }
    }
}