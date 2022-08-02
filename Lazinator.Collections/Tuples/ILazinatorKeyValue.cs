using Lazinator.Attributes;
using Lazinator.Core;

namespace Lazinator.Collections
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.ILazinatorKeyValue)]
    public interface ILazinatorKeyValue<TKey, TValue> where TKey : ILazinator where TValue : ILazinator
    {
        TKey Key { get; set; }
        TValue Value { get; set; }
    }
}
