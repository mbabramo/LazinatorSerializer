using Lazinator.Attributes;

namespace Lazinator.Wrappers
{
    [Lazinator((int)LazinatorCoreUniqueIDs.IWAbstract)]
    interface IWAbstract<T>
    {
        T Wrapped { get; set; }
    }
}