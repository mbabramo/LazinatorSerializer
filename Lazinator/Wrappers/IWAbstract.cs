using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.WAbstract)]
    interface IWAbstract<T>
    {
        T Wrapped { get; set; }
    }
}