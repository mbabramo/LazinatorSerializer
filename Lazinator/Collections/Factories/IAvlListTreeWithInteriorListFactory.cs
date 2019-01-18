using Lazinator.Core;
using Lazinator.Attributes;

namespace Lazinator.Collections.Factories
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.IAvlTree)]
    public interface IAvlListTreeWithInteriorListFactory<T> where T : ILazinator
    {
        bool UseLinkedList { get; set; }
        int InteriorMaxCapacity { get; set; }
    }
}