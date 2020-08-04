using Lazinator.Attributes;

namespace Lazinator.Wrappers
{
    /// <summary>
    /// A wrapper for an abstract class. A wrapper can be used as a generic type where the unwrapped object cannot be.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Lazinator((int)LazinatorCoreUniqueIDs.IWAbstract)]
    interface IWAbstract<T>
    {
        T Wrapped { get; set; }
    }
}