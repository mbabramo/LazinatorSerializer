using Lazinator.Attributes;
using Lazinator.Collections;
using Lazinator.Support;
using Lazinator.Core;

namespace Lazinator.Collections
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorList)]
    public interface ILazinatorList<T> where T : ILazinator
    {
        LazinatorOffsetList Offsets { get; set; }
    }
}