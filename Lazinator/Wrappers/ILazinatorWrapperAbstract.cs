using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorWrapperAbstract)]
    public interface ILazinatorWrapperAbstract<T>
    {
        T Value { get; set; }
    }
}