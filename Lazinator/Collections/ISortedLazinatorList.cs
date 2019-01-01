using Lazinator.Core;
using Lazinator.Attributes;

namespace Lazinator.Collections
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.ISortedLazinatorList)]
    internal interface ISortedLazinatorList<T> where T : ILazinator
    {
        bool AllowDuplicates { get; set; }
    }
}