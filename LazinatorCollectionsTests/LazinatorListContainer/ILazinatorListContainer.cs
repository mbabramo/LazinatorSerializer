using Lazinator.Attributes;
using Lazinator.Wrappers;
using LazinatorCollections;

namespace LazinatorCollectionsTests
{
    [Lazinator((int)CollectionsTestsObjectIDs.ListContainer)]
    public interface ILazinatorListContainer
    {
        int MyInt { get; set; }
        LazinatorList<ExampleChild> MyList { get; set; }
        LazinatorList<WByte> MyStructList { get; set; }
        long MyLong { get; set; }
    }
}